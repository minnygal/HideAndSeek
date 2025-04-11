using Moq;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for saving/loading/deleting games
    /// </summary>
    [TestFixture]
    public class TestGameController_SaveLoadDeleteGame
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
                               "DefaultHouse.json", TestGameController_SaveLoadDeleteGame_TestCaseData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [Test]
        [Category("GameController SaveGame Failure")]
        public void Test_GameController_SaveGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            gameController = new GameController("DefaultHouse");
            exception = Assert.Throws<InvalidDataException>(() => gameController.SaveGame(fileName));
            Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because file name \"{fileName}\" " +
                                            $"is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
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
        [Category("GameController DeleteGame Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            gameController = new GameController("DefaultHouse");
            exception = Assert.Throws<InvalidDataException>(() => gameController.DeleteGame(fileName));
            Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because file name \"{fileName}\" " +
                                            $"is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [TestCase("a", "Game successfully saved in a"),]
        [TestCase("my_saved_game", "Game successfully saved in my_saved_game")]
        [Category("GameController SaveGame Success")]
        public void Test_GameController_SaveGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.WriteAllText($"{fileName}.json", It.IsAny<string>())); // Accept any text written to file
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set up game cotroller
            gameController = new GameController("DefaultHouse");

            // Save game
            message = gameController.SaveGame(fileName);

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        // Tests default House, and tests custom House set via constructor and via ReloadGame
        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
                        nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_SaveGame_AndCheckTextSavedToFile))]
        public void Test_GameController_SaveGame_AndCheckTextSavedToFile(string houseFileName, string houseFileText, 
                    Func<Mock<IFileSystem>, GameController> StartNewGame, string expectedTextInSavedGameFile)
        {
            // Set House file system
            Mock<IFileSystem> mockHouseFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(houseFileName, houseFileText);
            House.FileSystem = mockHouseFileSystem.Object;

            // Create variable to store text written to SavedGame file
            string? actualTextInSavedGameFile = null;

            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.WriteAllText("my_saved_game.json", It.IsAny<string>()))
                    .Callback((string path, string text) =>
                    {
                        actualTextInSavedGameFile = text; // Store text written to file in variable
                    });
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Start and attempt to save game
            gameController = StartNewGame(mockHouseFileSystem); // Mock House file system is used in test cases calling RestartGame
            gameController.SaveGame("my_saved_game");

            // Assume no exception was thrown
            // Assert that actual text in file is equal to expected text in file
            Assert.That(actualTextInSavedGameFile, Is.EqualTo(expectedTextInSavedGameFile));
        }

        [Test]
        [Category("GameController SaveGame Failure")]
        public void Test_GameController_SaveGame_AndCheckErrorMessage_ForAlreadyExistingFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.Exists("fileName.json")).Returns(true); // Mock that file already exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set up game cotroller
            gameController = new GameController("DefaultHouse");

            // Assert that saving game with file name of already existing file raises exception
            exception = Assert.Throws<InvalidOperationException>(() => gameController.SaveGame("fileName"));

            // Assert that exception message is correct
            Assert.That(exception.Message, Is.EqualTo("Cannot perform action because a file named fileName already exists"));
        }

        [TestCase("a", "Game successfully loaded from a")]
        [TestCase("my_saved_game", "Game successfully loaded from my_saved_game")]
        [Category("GameController LoadGame Success")]
        public void Test_GameController_LoadGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{fileName}.json",
                                            TestGameController_SaveLoadDeleteGame_TestCaseData.SavedGame_Serialized_NoFoundOpponents);
            
            // Create new GameController
            gameController = new GameController("DefaultHouse");

            // Have game controller load game
            message = gameController.LoadGame(fileName);

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
                        nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_WithNoFoundOpponents))]
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

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
            nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_WithFoundOpponents))]
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
                    TestGameController_SaveLoadDeleteGame_TestCaseData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
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
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(TestGameController_SaveLoadDeleteGame_TestCaseData.DefaultHouse_Locations), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_SaveLoadDeleteGame_TestCaseData.DefaultHouse_LocationsWithoutHidingPlaces), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestGameController_SaveLoadDeleteGame_TestCaseData.DefaultHouse_LocationsWithHidingPlaces), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(playerLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(foundOpponents), "found opponents");

                // Check hiding places that should have Opponents still hidden there and make sure Opponents found are as expected
                CheckHidingPlacesWithOpponents(gameController.House.LocationsWithHidingPlaces);

                // Assert that all hiding places which had no Opponents hidden there in this game are empty
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(TestGameController_SaveLoadDeleteGame_TestCaseData.SavedGame_OpponentsAndHidingPlaces
                                                                              .Select((kvp) => kvp.Value).Distinct().Contains(l.Name)))
                                                              .ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
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

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
            nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileFormatIsInvalid))]
        [Category("GameController LoadGame JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileDataHasInvalidValue(string endOfExceptionMessage, string textInFile)
        {
            // Assert that loading corrupt game raises exception
            exception = Assert.Throws<JsonException>(() => GetExceptionWhenLoadGameWithCorruptSavedGameFile(textInFile));
            
            // Assert that error message is correct
            Assert.That(exception.Message, Is.EqualTo($"Cannot process because data is corrupt - {endOfExceptionMessage}"));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData),
            nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenSavedGameFileDataHasInvalidValue))]
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

        [Test]
        [Category("GameController DeleteGame Success")]
        public void Test_GameController_DeleteGame_AndCheckSuccessMessage()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller delete game
            message = gameController.DeleteGame("my_saved_game");

            // Verify that File.Delete was called once
            mockFileSystemForGameController.Verify(manager => manager.File.Delete("my_saved_game.json"), Times.Once);

            // Assert that success message is as expected
            Assert.That(message, Is.EqualTo("Game file my_saved_game has been successfully deleted"));
        }

        [Test]
        [Category("GameController DeleteGame Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent_game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Assert that attempt to delete nonexistent game raises exception
            exception = Assert.Throws<FileNotFoundException>(() => gameController.DeleteGame("my_nonexistent_game"));

            // Assert that error message is as expected
            Assert.That(exception.Message, Is.EqualTo("Could not delete game because file my_nonexistent_game does not exist"));
        }
    }
}
