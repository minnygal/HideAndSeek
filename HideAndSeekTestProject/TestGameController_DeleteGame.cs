using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for DeleteGame method to delete saved game file
    /// </summary>
    [TestFixture]
    public class TestGameController_DeleteGame
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
                               "DefaultHouse.json", TestGameController_DeleteGame_TestData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
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
    }
}
