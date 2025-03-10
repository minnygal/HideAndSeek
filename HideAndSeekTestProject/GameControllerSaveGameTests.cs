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
            mockFileSystem = new Mock<IFileSystem>();
        }

        [Test, Category("GameController Save Load Delete Error")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_WhenFileNameIsInvalid(
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
        [Category("GameController Save Load Delete Error")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_WhenNoFileNameEntered(string commandWord)
        {
            gameController = new GameController();
            message = gameController.ParseInput(commandWord);
            Assert.That(message, Is.EqualTo("Cannot perform action because no file name was entered"));
        }

        [Test, Category("GameController Save Error")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckErrorMessage_WhenFileAlreadyExists()
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

        [TestCase("a", "Game successfully saved in a"), ]
        [TestCase("my_saved_game", "Game successfully saved in my_saved_game")]
        [Category("GameController Save Success")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckSuccessMessage_WhenFileNameIsValid(string fileName, string expected)
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

        [TestCaseSource(typeof(SaveGameTests_TestCaseData), nameof(SaveGameTests_TestCaseData.TestCases_For_Test_GameController_ParseInput_AndCheckTextSavedToFile))]
        [Category("GameController Save Success")]
        public string Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile(Func<IFileSystem, GameController> startNewGame)
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

            // Return actual text written to file
            return actualTextInFile;
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
        [TestCaseSource(typeof(SaveGameTests_TestCaseData), nameof(SaveGameTests_TestCaseData.TestCases_For_Test_GameController_ParseInput_LoadGame_AndCheckGameIsLoadedSuccessfully))]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckGameIsLoadedSuccessfully(string currentLocation, int moveNumber, List<string> foundOpponents, string textInFile)
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

        [Test, Category("GameController Load Error")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDoesNotExist()
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

        [TestCaseSource(typeof(SaveGameTests_TestCaseData), nameof(SaveGameTests_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataIsInvalid))]
        [Category("GameController Load Error")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataIsInvalid(string errorMessage, string textInFile)
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo(errorMessage));
        }

        [Test, Category("GameController Delete Error")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckErrorMessage_WhenFileDoesNotExist()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(false); // Mock that file does not exist

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_saved_game");

            // Assert that error message is as expected
            Assert.That(message, Is.EqualTo("Could not delete game because file my_saved_game does not exist"));
        }

        [Test, Category("GameController Delete Success")]
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
    }
}
