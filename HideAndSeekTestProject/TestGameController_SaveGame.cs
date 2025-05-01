using Moq;
using System.IO.Abstractions;
using System.Linq;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for SavedGame method to save game to file
    /// (integration tests using SavedGame, House, Location, and LocationWithHidingPlace)
    /// </summary>
    [TestFixture]
    public class TestGameController_SaveGame
    {
        private GameController gameController;

        [SetUp]
        public void Setup()
        {
            gameController = null;
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
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

            // Set up game controller
            gameController = new GameController(TestGameController_SaveGame_TestData.MockedOpponents, 
                                                TestGameController_SaveGame_TestData.GetDefaultHouse());

            // Save game and assert that return message is correct
            Assert.That(gameController.SaveGame(fileName), Is.EqualTo(expected));
        }

        // Tests default House, and tests custom House set via constructor and via ReloadGame
        [TestCaseSource(typeof(TestGameController_SaveGame_TestData),
                        nameof(TestGameController_SaveGame_TestData.TestCases_For_Test_GameController_SaveGame_AndCheckTextSavedToFile))]
        public void Test_GameController_SaveGame_AndCheckTextSavedToFile(Func<GameController> StartNewGame, string expectedTextInSavedGameFile)
        {
            // Create variable to store text written to SavedGame file
            string? actualTextInSavedGameFile = null;

            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.WriteAllText("my_saved_game.game.json", It.IsAny<string>()))
                    .Callback((string path, string text) =>
                    {
                        actualTextInSavedGameFile = text; // Store text written to file in variable
                    });
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Start and attempt to save game
            gameController = StartNewGame();
            gameController.SaveGame("my_saved_game");

            // Assume no exception was thrown
            // Assert that actual text in file is equal to expected text in file
            Assert.That(actualTextInSavedGameFile, Is.EqualTo(expectedTextInSavedGameFile));
        }

        [Test]
        [Category("GameController SaveGame ArgumentException Failure")]
        public void Test_GameController_SaveGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            // Set game controller
            gameController = new GameController(TestGameController_SaveGame_TestData.MockedOpponents,
                                                TestGameController_SaveGame_TestData.GetDefaultHouse());

            Assert.Multiple(() =>
            {
                // Assert that saving game with invalid file name raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    gameController.SaveGame(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                               "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("GameController SaveGame InvalidOperationException Failure")]
        public void Test_GameController_SaveGame_AndCheckErrorMessage_ForAlreadyExistingFile()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(system => system.File.Exists("fileName.game.json")).Returns(true); // Mock that file already exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Set up game cotroller
            gameController = new GameController(TestGameController_SaveGame_TestData.MockedOpponents,
                                                TestGameController_SaveGame_TestData.GetDefaultHouse());

            Assert.Multiple(() =>
            {
                // Assert that saving game with file name of already existing file raises exception
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    gameController.SaveGame("fileName");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot perform action because a file named fileName already exists"));
            });
        }
    }
}