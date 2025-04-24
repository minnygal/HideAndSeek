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
    /// (creates new GameController with unmocked House, Location, and LocationWithHidingPlace objects,
    ///  but does not test these objects)
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
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, "DefaultHouse"); // Create new GameController with mocked Opponent and default House
            message = null;
            exception = null;
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                               "DefaultHouse.house.json", TestGameController_DeleteGame_TestData.DefaultHouse_Serialized); // Set mock file system for House property to return default House file text
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [Test]
        [Category("GameController DeleteGame ArgumentException Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            
            exception = Assert.Throws<ArgumentException>(() => gameController.DeleteGame(fileName));
            Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                       "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                           "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [Test]
        [Category("GameController DeleteGame FileNotFoundException Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent.game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

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
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.game.json")).Returns(true); // Mock that file exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            // Have game controller delete game
            message = gameController.DeleteGame("my_saved_game");

            // Verify that File.Delete was called once
            mockFileSystemForGameController.Verify(manager => manager.File.Delete("my_saved_game.game.json"), Times.Once);

            // Assert that success message is as expected
            Assert.That(message, Is.EqualTo("Game file my_saved_game has been successfully deleted"));
        }
    }
}
