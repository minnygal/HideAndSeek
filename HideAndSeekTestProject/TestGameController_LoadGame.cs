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
    /// 
    /// These are integration tests using SavedGame, House, Opponent, Location, and LocationWithHidingPlace.
    /// Opponent object is mocked in initial GameController instantiation but real Opponents are created in LoadGame.
    /// </summary>
    [TestFixture]
    public class TestGameController_LoadGame
    {
        private GameController gameController;
        private static readonly Opponent[] mockedOpponent = new Opponent[] { new Mock<Opponent>().Object };

        [SetUp]
        public void Setup()
        {
            gameController = null;
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                               "DefaultHouse.house.json", TestGameController_LoadGame_TestData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system 
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        /// <summary>
        /// Get GameController created with minimal House object and mocked Opponent
        /// </summary>
        /// <returns>GameController created with minimal House object and mocked Opponent</returns>
        private static GameController GetMinimalGameController()
        {
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Office", "under the table"); // Create a location with hiding place
            House house = new House("test house", "TestHouse", "Entry",
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            return new GameController(mockedOpponent, house); // Create GameController with House
        }

        [TestCase("a", "Game successfully loaded from a")]
        [TestCase("my_saved_game", "Game successfully loaded from my_saved_game")]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set variable to SavedGame file text
            string savedGameFileText = 
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{fileName}.game.json", savedGameFileText);

            // Set game controller
            gameController = GetMinimalGameController();

            // Load game and assert that return message is as expected
            Assert.That(gameController.LoadGame(fileName), Is.EqualTo(expected));
        }

        [Test]
        [Category("GameController LoadGame House CurrentLocation MoveNumber OpponentsAndHidingLocations Success")]
        public void Test_GameController_LoadGame_WithNoFoundOpponents()
        {
            // ARRANGE
            // Set mock file system for House property to return House file text
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                "TestHouse.house.json", TestGameController_LoadGame_TestData.CustomTestHouse_Serialized);

            // Set variable to text in SavedGame file
            string savedGameFileText = 
                "{" +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    "\"PlayerLocation\":\"Landing\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Closet\"," +
                        "\"Bob\":\"Yard\"," +
                        "\"Ana\":\"Cellar\"," +
                        "\"Owen\":\"Attic\"," +
                        "\"Jimmy\":\"Yard\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            // Set up mock for GameController file system to return SavedGame file text
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.game.json", savedGameFileText);

            // Set game controller
            gameController = GetMinimalGameController();

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Have game controller load game and assert that return message is as expected
                Assert.That(gameController.LoadGame("my_saved_game"), Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that GameController's House's properties are as expected after loading game
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "loaded House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "loaded House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Landing"), "loaded House player starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), 
                    Is.EquivalentTo(TestGameController_LoadGame_TestData.CustomTestHouse_Locations), "loaded House all location names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), 
                    Is.EquivalentTo(TestGameController_LoadGame_TestData.CustomTestHouse_LocationsWithoutHidingPlaces), "loaded House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), 
                    Is.EquivalentTo(TestGameController_LoadGame_TestData.CustomTestHouse_LocationsWithHidingPlaces), "loaded House locations with hiding places names");

                // Assert that GameController properties are as expected after loading game
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Landing"), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), 
                    Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), 
                    Is.EquivalentTo(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no opponents found");

                // Assert that locations with hiding places that should not have hidden Opponents do not have any hidden Opponents
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(new List<string>(){ "Closet", "Yard", "Cellar", "Attic" }.Contains(l.Name)))
                                                              .ToList().ForEach((l) =>
                                                              {
                                                                  Assert.That(l.CheckHidingPlace(), Is.Empty, $"no Opponents hiding in the {l.Name}");
                                                              });
            });
        }


        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_WithFoundOpponents))]
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
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                   $"\"FoundOpponents\":[{string.Join(",", foundOpponents.Select((o) => $"\"{o}\""))}]" +
                "}";
            
            // Set up mock for file system for GameController
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.game.json", savedGameFileText);

            // Get game controller
            gameController = GetMinimalGameController();

            Assert.Multiple(() =>
            {
                //ACT/ASSERT
                // Have game controller load game and assert that load game return message is as expected
                Assert.That(gameController.LoadGame("my_saved_game"), Is.EqualTo("Game successfully loaded from my_saved_game"), "LoadGame return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("my house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("DefaultHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_Locations), "House all opponent names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithoutHidingPlaces), "House opponent without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_LoadGame_TestData.DefaultHouse_LocationsWithHidingPlaces), "House opponent with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(playerLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), 
                    Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), 
                    Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(foundOpponents), "found opponents");

                // Check hiding places that should have Opponents still hidden there and make sure Opponents found are as expected
                CheckHidingPlacesWithOpponents(gameController.House.LocationsWithHidingPlaces);

                // Assert that all hiding places which had no Opponents hidden there in this game are empty
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(new List<string>() { "Kitchen", "Pantry", "Bathroom" }.Contains(l.Name)))
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
            // Set game controller
            gameController = GetMinimalGameController();

            Assert.Multiple(() =>
            {
                // Assert that loading game with invalid file name raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    gameController.LoadGame(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                               "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("GameController LoadGame FileNotFoundException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent_file.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set game controller
            gameController = GetMinimalGameController();

            Assert.Multiple(() =>
            {
                // Assert that loading a game with a name of a nonexistent file raises an exception
                Exception exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    gameController.LoadGame("my_nonexistent_file");
                });

                // Assert that exception message is correct
                Assert.That(exception.Message, Is.EqualTo("Cannot load game because file my_nonexistent_file does not exist"));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue))]
        [Category("GameController LoadGame InvalidOperationException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue(string endOfExceptionMessage, string textInFile)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading corrupt game raises exception
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
            });
        }

        [Test]
        [Category("GameController LoadGame ArgumentOutOfRangeException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForMoveNumber()
        {
            // Set variable to text in SavedGame file
            string textInFile = 
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":-1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that loading game with invalid move number value raises exception
                Exception exception = Assert.Throws<ArgumentOutOfRangeException>(() => {
                    GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot process because data is corrupt - MoveNumber is invalid - must be positive number"));
            });
        }

        [Test]
        [Category("GameController LoadGame ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForOpponents()
        {
            // Set variable to text in SavedGame file
            string textInFile =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":{}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that loading game with invalid opponents value raises exception
                Exception exception = Assert.Throws<ArgumentException>(() => {
                    GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot process because data is corrupt - invalid OpponentsAndHidingLocations - no opponents"));
            });
        }

        [Test]
        [Category("GameController LoadGame ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue_ForHouseFileName()
        {
            // Set variable to text in SavedGame file
            string textInFile =
                "{" +
                    "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that loading corrupt game raises exception
                Exception exception = Assert.Throws<ArgumentException>(() => {
                    GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                              "Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid " +
                                                              "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_TestData),
            nameof(TestGameController_LoadGame_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid))]
        [Category("GameController LoadGame JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid(string endOfExceptionMessage, string textInFile)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading corrupt game raises exception
                Exception exception = Assert.Throws<JsonException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
            });
        }

        /// <summary>
        /// Call LoadGame to load game from corrupt saved game file
        /// Fails test if exception not thrown
        /// </summary>
        /// <param name="savedGameFileText">Text in corrupt SavedGame file</param>
        private void GetExceptionWhenLoadGameWithCorruptSavedGameFile(string savedGameFileText)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_corrupt_game.game.json", savedGameFileText);

            // Get game controller
            gameController = GetMinimalGameController();

            // Load game from corrupt SavedGame file
            gameController.LoadGame("my_corrupt_game"); // Should throw exception

            // If exception not thrown, fail test
            Assert.Fail();
        }
    }
}