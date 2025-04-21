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
    /// (not including tests with corrupt House file)
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
                               "DefaultHouse_h.json", TestGameController_LoadGame_TestData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
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
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{fileName}_sg.json",
                                            TestGameController_LoadGame_TestData.SavedGame_Serialized_NoFoundOpponents);

            // Create new GameController
            gameController = new GameController("DefaultHouse");

            // Have game controller load game
            message = gameController.LoadGame(fileName);

            // Assert that return message is as expected
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
                        nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_WithNoFoundOpponents_AndSameHouse))]
        public void Test_GameController_LoadGame_WithNoFoundOpponents_AndSameHouse(
            string savedGameFileText, string houseName, string houseFileName, string houseFileText, string housePlayerStartingPoint,
            IEnumerable<string> locations, IEnumerable<string> locationsWithoutHidingPlaces, IEnumerable<string> locationsWithHidingPlaces,
            string currentLocation, int moveNumber, IEnumerable<string> opponents, IEnumerable<string> opponentHidingLocations)
        {
            // ARRANGE
            // Set mock file system for House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{houseFileName}_h.json", houseFileText);

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game_sg.json", savedGameFileText);

            // Create new GameController with specified House layout
            gameController = new GameController(houseFileName);

            // ACT
            // Have game controller load game and store message
            message = gameController.LoadGame("my_saved_game");

            // ASSERT
            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that load game return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo(houseName), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(houseFileName), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo(housePlayerStartingPoint), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(locations), "House all savedGameLocations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithoutHidingPlaces), "House savedGameLocations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithHidingPlaces), "House savedGameLocations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(currentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(opponents), "all savedGameOpponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(opponentHidingLocations), "savedGameOpponents' hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no savedGameOpponents found");

                // Assert that savedGameLocations with hiding places that should not have hidden Opponents do not have any hidden Opponents
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(opponentHidingLocations.Distinct().Contains(l.Name))).ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
                        nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_WithNoFoundOpponents_AndDifferentHouse))]
        public void Test_GameController_LoadGame_WithNoFoundOpponents_AndDifferentHouse(
            string startHouseName, string startHouseFileName, string startHouseFileText, string startHousePlayerStartingPoint, // Starting House file name and properties
            IEnumerable<string> startHouseLocations, IEnumerable<string> startHouseLocationsWithoutHidingPlaces, IEnumerable<string> startHouseLocationsWithHidingPlaces, // Starting House properties
            string savedGameHouseName, string savedGameHouseFileName, string savedGameHouseFileText, string savedGamePlayerStartingPoint, // SavedGame House file name and properties
            IEnumerable<string> savedGameLocations, IEnumerable<string> savedGameLocationsWithoutHidingPlaces, IEnumerable<string> savedGameLocationsWithHidingPlaces, // SavedGame House properties
            string savedGameFileText, string savedGameCurrentLocation, int savedGameMoveNumber, IEnumerable<string> savedGameOpponents, IEnumerable<string> savedGameOpponentHidingLocations // SavedGame file text and properties
            )
        {
            // ARRANGE
            // Set mock file System for House
            Mock<IFileSystem> houseFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText($"{startHouseFileName}_h.json", startHouseFileText); // Mock House file for GameController creation
            houseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseFileSystem, $"{savedGameHouseFileName}_h.json", savedGameHouseFileText); // Mock House file for SavedGame
            House.FileSystem = houseFileSystem.Object; // Set House file system property

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game_sg.json", savedGameFileText);

            // Create new GameController with specified House layout for starting game
            gameController = new GameController(startHouseFileName);

            Assert.Multiple(() =>
            {
                // Assert that GameController's House's properties are as expected when GameController created but before loading game
                Assert.That(gameController.House.Name, Is.EqualTo(startHouseName), "starting House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(startHouseFileName), "starting House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo(startHousePlayerStartingPoint), "starting House player starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(startHouseLocations), "starting House all location names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(startHouseLocationsWithoutHidingPlaces), "starting House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(startHouseLocationsWithHidingPlaces), "starting House locations with hiding places names");

                // ACT
                // Have game controller load game and store message
                message = gameController.LoadGame("my_saved_game");

                // ASSERT
                // Assert that load game return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that GameController's House's properties are as expected after loading game
                Assert.That(gameController.House.Name, Is.EqualTo(savedGameHouseName), "loaded House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(savedGameHouseFileName), "loaded House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo(savedGamePlayerStartingPoint), "loaded House player starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(savedGameLocations), "loaded House all location names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(savedGameLocationsWithoutHidingPlaces), "loaded House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(savedGameLocationsWithHidingPlaces), "loaded House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(savedGameCurrentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(savedGameMoveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(savedGameOpponents), "all savedGameOpponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(savedGameOpponentHidingLocations), "savedGameOpponents' hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no savedGameOpponents found");

                // Assert that locations with hiding places that should not have hidden Opponents do not have any hidden Opponents
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(savedGameOpponentHidingLocations.Distinct().Contains(l.Name))).ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_AndSameHouse))]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_WithFoundOpponents_AndSameHouse(
            string playerLocation, int moveNumber, List<string> foundOpponents, Action<IEnumerable<LocationWithHidingPlace>> CheckHidingPlacesWithOpponents)
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                   $"\"PlayerLocation\":\"{playerLocation}\"" + "," +
                   $"\"MoveNumber\":{moveNumber}" + "," +
                    TestGameController_LoadGame_TestData.SavedGame_Serialized_OpponentsAndHidingLocations_DefaultHouse + "," +
                   $"\"FoundOpponents\":[{string.Join(",", foundOpponents.Select((o) => $"\"{o}\""))}]" +
                "}";

            // Set up mock for file system for GameController
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game_sg.json", savedGameFileText);

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
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_Locations), "House all savedGameLocations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithoutHidingPlaces), "House savedGameLocations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithHidingPlaces), "House savedGameLocations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(playerLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse.Keys.ToList()), "all savedGameOpponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse.Values.ToList()), "savedGameOpponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(foundOpponents), "found savedGameOpponents");

                // Check hiding places that should have Opponents still hidden there and make sure Opponents found are as expected
                CheckHidingPlacesWithOpponents(gameController.House.LocationsWithHidingPlaces);

                // Assert that all hiding places which had no Opponents hidden there in this game are empty
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse
                                                                              .Select((kvp) => kvp.Value).Distinct().Contains(l.Name)))
                                                              .ToList().ForEach((l) =>
                                                              {
                                                                  Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                                                              });
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_AndDifferentHouse))]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_WithFoundOpponents_AndDifferentHouse(
            string savedGameCurrentLocation, int savedGameMoveNumber, List<string> savedGameFoundOpponents, // SavedGame properties
            Action<IEnumerable<LocationWithHidingPlace>> CheckHidingPlacesWithOpponents // Check hiding places action
            )
        {
            // ARRANGE
            // Set mock file System for House
            Mock<IFileSystem> houseFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText($"TestHouse_h.json", TestGameController_LoadGame_TestData.CustomTestHouse_Serialized); // Mock House file for GameController creation
            houseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseFileSystem, "DefaultHouse_h.json", TestGameController_LoadGame_TestData.DefaultHouse_Serialized); // Mock House file for SavedGame
            House.FileSystem = houseFileSystem.Object; // Set House file system property

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                   $"\"PlayerLocation\":\"{savedGameCurrentLocation}\"" + "," +
                   $"\"MoveNumber\":{savedGameMoveNumber}" + "," +
                    TestGameController_LoadGame_TestData.SavedGame_Serialized_OpponentsAndHidingLocations_DefaultHouse + "," +
                   $"\"FoundOpponents\":[{string.Join(",", savedGameFoundOpponents.Select((o) => $"\"{o}\""))}]" +
                "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game_sg.json", savedGameFileText);

            // Create new game controller
            gameController = new GameController("TestHouse");

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
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_Locations), "House all savedGameLocations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithoutHidingPlaces), "House savedGameLocations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithHidingPlaces), "House savedGameLocations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(savedGameCurrentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(savedGameMoveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse.Keys.ToList()), "all savedGameOpponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse.Values.ToList()), "savedGameOpponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(savedGameFoundOpponents), "found opponents");

                // Check hiding places that should have Opponents still hidden there and make sure Opponents found are as expected
                CheckHidingPlacesWithOpponents(gameController.House.LocationsWithHidingPlaces);

                // Assert that all hiding places which had no Opponents hidden there in this game are empty
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(TestGameController_LoadGame_TestData.SavedGame_OpponentsAndHidingPlaces_InDefaultHouse
                                                                              .Select((kvp) => kvp.Value).Distinct().Contains(l.Name)))
                                                              .ToList().ForEach((l) =>
                                                              {
                                                                  Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                                                              });
            });
        }

        [Test]
        [Category("GameController LoadGame ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            gameController = new GameController("DefaultHouse");
            exception = Assert.Throws<ArgumentException>(() => gameController.LoadGame(fileName));
            Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                           "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [Test]
        [Category("GameController LoadGame FileNotFoundException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent_file.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            Assert.Multiple(() =>
            {
                // Assert that loading a game with a name of a nonexistent file raises an exception
                exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    gameController.LoadGame("my_nonexistent_file");
                });

                // Assert that error message is correct
                Assert.That(exception.Message, Is.EqualTo("Cannot load game because file my_nonexistent_file does not exist"));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue))]
        [Category("GameController LoadGame InvalidOperationException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue(string endOfExceptionMessage, string textInFile)
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<InvalidOperationException>(() => GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile));

            // Assert that error message is correct
            Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
        }

        [Test]
        [Category("GameController LoadGame ArgumentOutOfRangeException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForMoveNumber()
        {
            // Assert that loading game with invalid savedGameOpponents value raises exception
            exception = Assert.Throws<ArgumentOutOfRangeException>(() => {
                GetExceptionWhenLoadGameWithCorruptSavedGameFile(
                    "{" +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_HouseFileName + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame_DefaultHouse + "," +
                        "\"MoveNumber\":-1" + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_OpponentsAndHidingLocations_DefaultHouse + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                    "}");
            });

            // Assert that error message is correct
            Assert.That(exception.Message, Does.StartWith($"Cannot process because data is corrupt - MoveNumber is invalid - must be positive number"));
        }

        [Test]
        [Category("GameController LoadGame ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForOpponents()
        {
            // Assert that loading game with invalid savedGameOpponents value raises exception
            exception = Assert.Throws<ArgumentException>(() => {
                GetExceptionWhenLoadGameWithCorruptSavedGameFile(
                    "{" +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_HouseFileName + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame_DefaultHouse + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                        "\"OpponentsAndHidingLocations\":{}" + "," +
                        TestGameController_LoadGame_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                    "}");
            });

            // Assert that error message is correct
            Assert.That(exception.Message, Does.StartWith($"Cannot process because data is corrupt - invalid OpponentsAndHidingLocations - no opponents"));
        }

        [Test]
        [Category("GameController LoadGame ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForHouseFileName()
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<ArgumentException>(() => {
                GetExceptionWhenLoadGameWithCorruptSavedGameFile(
                        "{" +
                            "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                            TestGameController_LoadGame_TestData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame_DefaultHouse + "," +
                            TestGameController_LoadGame_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            TestGameController_LoadGame_TestData.SavedGame_Serialized_OpponentsAndHidingLocations_DefaultHouse + "," +
                            TestGameController_LoadGame_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}");
            });

            // Assert that error message is correct
            Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                          "Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid " +
                                                          "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid))]
        [Category("GameController LoadGame JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid(string endOfExceptionMessage, string textInFile)
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<JsonException>(() => GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile));

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
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_corrupt_game_sg.json", savedGameFileText);

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Load game from corrupt SavedGame file
            gameController.LoadGame("my_corrupt_game"); // Should throw exception

            // If exception not thrown, fail test
            Assert.Fail();
        }
    }
}
