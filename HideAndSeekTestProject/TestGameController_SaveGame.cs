using Moq;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for SavedGame method to save game to file
    /// </summary>
    [TestFixture]
    public class TestGameController_SaveGame
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
                               "DefaultHouse.json", TestGameController_SaveGame_TestCaseData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
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
        [TestCaseSource(typeof(TestGameController_SaveGame_TestCaseData),
                        nameof(TestGameController_SaveGame_TestCaseData.TestCases_For_Test_GameController_SaveGame_AndCheckTextSavedToFile))]
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
        public void Test_GameController_SaveGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            gameController = new GameController("DefaultHouse");
            exception = Assert.Throws<InvalidDataException>(() => gameController.SaveGame(fileName));
            Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because file name \"{fileName}\" " +
                                            $"is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
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
    }
}
