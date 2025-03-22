using Moq;
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for saving/loading/deleting games
    /// </summary>
    public class GameControllerSaveGameTests
    {
        private GameController gameController;
        private string message; // Message returned by GameController's ParseInput method
        private Mock<IFileSystem> mockFileSystem; // Mock file system to be passed to GameController upon creation
        
        [SetUp]
        public void Setup()
        {
            gameController = null;
            message = null;
            mockFileSystem = new Mock<IFileSystem>(); // Set mock file system variable to new file system
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("save", "load", "delete")] string commandKeyword,
            [Values(" ", " my saved game", " my\\saved\\game", " my/saved/game", " my/saved\\ game")] string restOfCommand)
        {
            gameController = new GameController();
            message = gameController.ParseInput(commandKeyword + restOfCommand);
            Assert.That(message, Is.EqualTo($"Cannot perform action because file name \"{restOfCommand.Substring(1)}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [TestCase("save")]
        [TestCase("load")]
        [TestCase("delete")]
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForNoFileName(string commandWord)
        {
            gameController = new GameController();
            message = gameController.ParseInput(commandWord);
            Assert.That(message, Is.EqualTo("Cannot perform action because no file name was entered"));
        }

        [TestCase("a", "Game successfully saved in a"),]
        [TestCase("my_saved_game", "Game successfully saved in my_saved_game")]
        [Category("GameController Save Success")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for file system
            mockFileSystem.Setup(system => system.File.WriteAllText($"{fileName}.json", It.IsAny<string>())); // Accept any text written to file

            // Set up game cotroller
            gameController = new GameController(mockFileSystem.Object);

            // Attempt to save game
            message = gameController.ParseInput($"save {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_TestCaseData), nameof(GameControllerSaveGameTests_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile))]
        [Category("GameController Save Success")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile(Func<IFileSystem, GameController> startNewGame, string expectedTextInFile)
        {
            // Create variable to store text written to file
            string? actualTextInFile = null;

            // Set up mock for file system
            mockFileSystem.Setup(system => system.File.WriteAllText("my_saved_game.json", It.IsAny<string>()))
                    .Callback((string path, string text) =>
                    {
                        actualTextInFile = text; // Store text written to file in variable
                    });

            // Start and attempt to save game
            gameController = startNewGame(mockFileSystem.Object);
            gameController.ParseInput("save my_saved_game");

            // Assert that actual text in file is equal to expected text in file
            Assert.That(actualTextInFile, Is.EqualTo(expectedTextInFile));
        }

        [Test]
        [Category("GameController Save Failure")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckErrorMessage_ForAlreadyExistingFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(system => system.File.Exists("fileName.json")).Returns(true); // Mock that file already exists

            // Set up game cotroller
            gameController = new GameController(mockFileSystem.Object);

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
            // Initialize variable to text stored in mock file
            string textInFile = "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"Entry\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}";

            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists($"{fileName}.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText($"{fileName}.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput($"load {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        // Does not check message
        [TestCaseSource(typeof(GameControllerSaveGameTests_TestCaseData), nameof(GameControllerSaveGameTests_TestCaseData.Test_GameController_ParseInput_ToLoadGame_AndCheckProperties))]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckProperties(string currentLocation, int moveNumber, List<string> foundOpponents, string textInFile)
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput($"load my_saved_game");

            // Create expected enumerable of all opponents' names
            IEnumerable<string> expectedAllOpponentsNames = new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" };

            // Create expected enumerable of all opponents' hiding places
            IEnumerable<string> expectedAllOpponentsHidingPlaces = new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry"};
            
            // Assert that game state has been restored successfully
            Assert.Multiple(() =>
            {
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(currentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(expectedAllOpponentsNames), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(expectedAllOpponentsHidingPlaces), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(foundOpponents), "found opponents");
            });
        }

        [Test]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForNonexistentSavedGameFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(false); // Mock that file does not exist

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because file my_saved_game does not exist"));
        }

        [Test]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForNonexistentHouseFile()
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"NonexistentHouse\"" + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                TestSavedGame_Data.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = new Mock<IFileSystem>(); // Create new mock file system for House
            houseMockFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true); // Mock that default House file exists
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(TestHouse_Data.SerializedTestHouse); // Mock text in default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("NonexistentHouse.json")).Returns(false); // Mock that nonexistent House file does not exist
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because house layout file NonexistentHouse does not exist"));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_TestCaseData), nameof(GameControllerSaveGameTests_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData))]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData(string endOfErrorMessage, string textInFile)
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_corrupt_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_corrupt_game.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_corrupt_game");
            
            // Assert that error message is correct
            Assert.That(message, Is.EqualTo($"Cannot process because data is corrupt - {endOfErrorMessage}"));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_CorruptHouse_TestCaseData), 
            nameof(GameControllerSaveGameTests_CorruptHouse_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid(string fileText, string exceptionMessageEnding)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot process because data is corrupt - " +
                                            "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                            exceptionMessageEnding));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_CorruptHouse_TestCaseData),
            nameof(GameControllerSaveGameTests_CorruptHouse_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue(string fileText, string exceptionMessageEnding)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot process because data is corrupt - " +
                                            "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                            exceptionMessageEnding));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_CorruptHouse_TestCaseData),
            nameof(GameControllerSaveGameTests_CorruptHouse_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection(string fileText)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Does.StartWith("Cannot process because data is corrupt - " +
                                                "Cannot process because data in house layout file CorruptHouse is corrupt - The JSON value could not be converted to HideAndSeek.Direction."));
        }

        [TestCaseSource(typeof(GameControllerSaveGameTests_CorruptHouse_TestCaseData),
            nameof(GameControllerSaveGameTests_CorruptHouse_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue(string fileText, string errorMessageEnding)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo($"Cannot process because data is corrupt - {errorMessageEnding}"));
        }

        [Test]
        [Category("GameController Delete Success")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckSuccessMessage()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_saved_game");

            // Verify that File.Delete was called once
            mockFileSystem.Verify(manager => manager.File.Delete("my_saved_game.json"), Times.Once);

            // Assert that success message is as expected
            Assert.That(message, Is.EqualTo("Game file my_saved_game has been successfully deleted"));
        }

        [Test]
        [Category("GameController Delete Failure")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_nonexistent_game.json")).Returns(false); // Mock that file does not exist

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_nonexistent_game");

            // Assert that error message is as expected
            Assert.That(message, Is.EqualTo("Could not delete game because file my_nonexistent_game does not exist"));
        }

        private GameController GetGameControllerForCorruptHouseFileTests(string fileText)
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"CorruptHouse\"" + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                TestSavedGame_Data.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = new Mock<IFileSystem>(); // Create new mock file system for House
            houseMockFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true); // Mock that default House file exists
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(TestHouse_Data.SerializedTestHouse); // Mock text in default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("CorruptHouse.json")).Returns(true); // Mock that corrupt House file does exist
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("CorruptHouse.json")).Returns(fileText); // Mock text in corrupt House file
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Return new game controller (Random not mocked, so truly random hiding places generated)
            return new GameController(mockFileSystem.Object);
        }
    }
}
