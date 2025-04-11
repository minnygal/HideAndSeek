using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for LoadGame method to load saved game from file
    /// </summary>
    [TestFixture]
    public class TestGameController_LoadGame
    {
        private GameController gameController;
        private string message; // Message returned by LoadGame
        private Exception exception; // Exception thrown by LoadGame

        [SetUp]
        public void Setup()
        {
            gameController = null;
            message = null;
            exception = null;
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                               "DefaultHouse.json", TestGameController_LoadGame_TestCaseData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [TestCase("a", "Game successfully loaded from a")]
        [TestCase("my_saved_game", "Game successfully loaded from my_saved_game")]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{fileName}.json",
                                            TestGameController_LoadGame_TestCaseData.SavedGame_Serialized_NoFoundOpponents);

            // Create new GameController
            gameController = new GameController("DefaultHouse");

            // Have game controller load game
            message = gameController.LoadGame(fileName);

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestCaseData),
                        nameof(TestGameController_LoadGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_WithNoFoundOpponents))]
        public void Test_GameController_LoadGame_WithNoFoundOpponents(
            string savedGameFileText, string houseName, string houseFileName, string houseFileText, string housePlayerStartingPoint,
            IEnumerable<string> locations, IEnumerable<string> locationsWithoutHidingPlaces, IEnumerable<string> locationsWithHidingPlaces,
            string currentLocation, int moveNumber, IEnumerable<string> opponents, IEnumerable<string> opponentHidingLocations)
        {
            // Set mock file system for House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{houseFileName}.json", houseFileText);

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.json", savedGameFileText);

            // Create new GameController with specified House layout
            gameController = new GameController(houseFileName);

            // Have game controller load game and store message
            message = gameController.LoadGame("my_saved_game");

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that load game return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo(houseName), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(houseFileName), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo(housePlayerStartingPoint), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(locations), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithoutHidingPlaces), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithHidingPlaces), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(currentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(opponents), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(opponentHidingLocations), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no opponents found");

                // Assert that locations with hiding places that should not have hidden Opponents do not have any hidden Opponents
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(opponentHidingLocations.Distinct().Contains(l.Name))).ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestCaseData),
            nameof(TestGameController_LoadGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_WithFoundOpponents))]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_WithFoundOpponents(
            string playerLocation, int moveNumber, List<string> foundOpponents, Action<IEnumerable<LocationWithHidingPlace>> CheckHidingPlacesWithOpponents)
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                   $"\"PlayerLocation\":\"{playerLocation}\"" + "," +
                   $"\"MoveNumber\":{moveNumber}" + "," +
                    TestGameController_LoadGame_TestCaseData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                   $"\"FoundOpponents\":[{string.Join(",", foundOpponents.Select((o) => $"\"{o}\""))}]" +
                "}";

            // Set up mock for file system for GameController
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.json", savedGameFileText);

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Load game from SavedGame file and get message
            message = gameController.LoadGame("my_saved_game");

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that LoadGame return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("my house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("DefaultHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestCaseData.DefaultHouse_Locations), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestCaseData.DefaultHouse_LocationsWithoutHidingPlaces), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestCaseData.DefaultHouse_LocationsWithHidingPlaces), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(playerLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(foundOpponents), "found opponents");

                // Check hiding places that should have Opponents still hidden there and make sure Opponents found are as expected
                CheckHidingPlacesWithOpponents(gameController.House.LocationsWithHidingPlaces);

                // Assert that all hiding places which had no Opponents hidden there in this game are empty
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(TestGameController_LoadGame_TestCaseData.SavedGame_OpponentsAndHidingPlaces
                                                                              .Select((kvp) => kvp.Value).Distinct().Contains(l.Name)))
                                                              .ToList().ForEach((l) =>
                                                              {
                                                                  Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                                                              });
            });
        }

        [Test]
        [Category("GameController LoadGame Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            gameController = new GameController("DefaultHouse");
            exception = Assert.Throws<InvalidDataException>(() => gameController.LoadGame(fileName));
            Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because file name \"{fileName}\" " +
                                            $"is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [Test]
        [Category("GameController LoadGame Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForNonexistentSavedGameFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller load game
            message = gameController.LoadGame("my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because file my_saved_game does not exist"));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestCaseData),
            nameof(TestGameController_LoadGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileFormatIsInvalid))]
        [Category("GameController LoadGame JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileDataHasInvalidValue(string endOfExceptionMessage, string textInFile)
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<JsonException>(() => GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile));

            // Assert that error message is correct
            Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestCaseData),
            nameof(TestGameController_LoadGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileDataHasInvalidValue))]
        [Category("GameController LoadGame InvalidDataException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileFormatIsInvalid(string endOfExceptionMessage, string textInFile)
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<InvalidDataException>(() => GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile));

            // Assert that error message is correct
            Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
        }

        /// <summary>
        /// Call LoadGame to load game from corrupt saved game file
        /// Fails test if exception not thrown
        /// </summary>
        /// <param name="savedGameFileText">Text in corrupt SavedGame file</param>
        private void GetExceptionWhenLoadGameWithCorruptSavedGameFile(string savedGameFileText)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_corrupt_game.json", savedGameFileText);

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Load game from corrupt SavedGame file
            gameController.LoadGame("my_corrupt_game"); // Should throw exception

            // If exception not thrown, fail test
            Assert.Fail();
        }
    }
}
