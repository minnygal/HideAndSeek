﻿using Moq;
using System.IO.Abstractions;
using System.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for saving/loading/deleting games
    /// </summary>
    [TestFixture]
    public class TestGameController_SaveLoadDeleteGame
    {
        private GameController gameController;
        private string message; // Message returned by GameController's ParseInput method

        [SetUp]
        public void Setup()
        {
            gameController = null;
            message = null;
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
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("save", "load", "delete")] string commandKeyword,
            [Values(" ", " my saved game", " my\\saved\\game", " my/saved/game", " my/saved\\ game")] string restOfCommand)
        {
            gameController = new GameController("DefaultHouse");
            message = gameController.ParseInput(commandKeyword + restOfCommand);
            Assert.That(message, Is.EqualTo($"Cannot perform action because file name \"{restOfCommand.Substring(1)}\" " +
                                            $"is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [TestCase("save")]
        [TestCase("load")]
        [TestCase("delete")]
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForNoFileName(string commandWord)
        {
            gameController = new GameController("DefaultHouse");
            message = gameController.ParseInput(commandWord);
            Assert.That(message, Is.EqualTo("Cannot perform action because no file name was entered"));
        }

        [TestCase("a", "Game successfully saved in a"),]
        [TestCase("my_saved_game", "Game successfully saved in my_saved_game")]
        [Category("GameController Save Success")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.WriteAllText($"{fileName}.json", It.IsAny<string>())); // Accept any text written to file
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set up game cotroller
            gameController = new GameController("DefaultHouse");

            // Attempt to save game
            message = gameController.ParseInput($"save {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        // Tests default House, and tests custom House set via constructor and via ReloadGame
        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
                        nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile))]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile(string houseFileName, string houseFileText, 
                    Func<Mock<IFileSystem>, GameController> startNewGame, string expectedTextInSavedGameFile)
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
            gameController = startNewGame(mockHouseFileSystem); // Mock House file system is used in test cases calling RestartGame
            gameController.ParseInput("save my_saved_game");

            // Assert that actual text in file is equal to expected text in file
            Assert.That(actualTextInSavedGameFile, Is.EqualTo(expectedTextInSavedGameFile));
        }

        [Test]
        [Category("GameController Save Failure")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckErrorMessage_ForAlreadyExistingFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.Exists("fileName.json")).Returns(true); // Mock that file already exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set up game cotroller
            gameController = new GameController("DefaultHouse");

            // Attempt to save game
            message = gameController.ParseInput($"save fileName");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot perform action because a file named fileName already exists"));
        }

        [TestCase("a", "Game successfully loaded from a")]
        [TestCase("my_saved_game", "Game successfully loaded from my_saved_game")]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{fileName}.json",
                                            TestGameController_SaveLoadDeleteGame_TestCaseData.SavedGame_Serialized_NoFoundOpponents);
            
            // Create new GameController
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with load command
            message = gameController.ParseInput($"load {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
                        nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents))]
        public void Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents(
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

            // Have game controller parse file name with load command and store message
            message = gameController.ParseInput($"load my_saved_game");

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

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
            nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents))]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents(
            string playerLocation, int moveNumber, List<string> foundOpponents, Action<GameController> CheckHidingPlacesWithOpponents)
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

            // Have game controller parse file name with load command
            message = ParseInputToLoadGameSuccessfully(savedGameFileText);

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

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
                CheckHidingPlacesWithOpponents(gameController);

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
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForNonexistentSavedGameFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;
            
            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because file my_saved_game does not exist"));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), 
            nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData))]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData(string endOfErrorMessage, string textInFile)
        {
            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_corrupt_game.json", textInFile);

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_corrupt_game");
            
            // Assert that error message is correct
            Assert.That(message, Is.EqualTo($"Cannot process because data is corrupt - {endOfErrorMessage}"));
        }

        [Test]
        [Category("GameController Delete Success")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckSuccessMessage()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_saved_game");

            // Verify that File.Delete was called once
            mockFileSystemForGameController.Verify(manager => manager.File.Delete("my_saved_game.json"), Times.Once);

            // Assert that success message is as expected
            Assert.That(message, Is.EqualTo("Game file my_saved_game has been successfully deleted"));
        }

        [Test]
        [Category("GameController Delete Failure")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent_game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Create new game controller
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_nonexistent_game");

            // Assert that error message is as expected
            Assert.That(message, Is.EqualTo("Could not delete game because file my_nonexistent_game does not exist"));
        }

        /// <summary>
        /// Helper method to call GameController ParseInput to load game with specific SavedGame file text
        /// </summary>
        /// <param name="savedGameFileText">Text in SavedGame file</param>
        /// <returns>Message returned by GameController ParseInput</returns>
        private string ParseInputToLoadGameSuccessfully(string savedGameFileText)
        {
            // Set up mock for file system for GameController
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.json", savedGameFileText);
            
            // Create new GameController
            gameController = new GameController("DefaultHouse");

            // Have game controller parse file name with load command and return message
            return gameController.ParseInput($"load my_saved_game");
        }
    }
}
