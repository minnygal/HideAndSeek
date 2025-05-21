using Moq;
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for DeleteGame method to delete saved game file
    /// 
    /// These are integration tests using House, Location, and LocationWithHidingPlace
    /// although these classes are not tested
    /// </summary>
    [TestFixture]
    public class TestGameController_DeleteGame
    {
        private GameController gameController;
        private House house; // House object passed in to GameController constructor

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set House variable
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Office", "under the table"); // Create a location with hiding place
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
        }

        [SetUp]
        public void Setup()
        {
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, house); // Create GameController with mocked Opponent and House
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [Test]
        [Category("GameController DeleteGame ArgumentException Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("", " ", "my saved game", "my\\saved\\game", "my/saved/game", "my/saved\\ game")] string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that attempting to delete game with invalid file name raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    new GameController().DeleteGame(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" " +
                                                               "is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("GameController DeleteGame FileNotFoundException Failure")]
        public void Test_GameController_DeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_nonexistent.game.json")).Returns(false); // Mock that file does not exist
            GameController.FileSystem = mockFileSystemForGameController.Object;

            Assert.Multiple(() =>
            {
                // Assert that attempting to delete nonexistent game raises exception
                Exception exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    new GameController().DeleteGame("my_nonexistent_game");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Could not delete game because file my_nonexistent_game does not exist"));
            });
        }

        [Test]
        [Category("GameController DeleteGame Success")]
        public void Test_GameController_DeleteGame_AndCheckSuccessMessage()
        {
            // Set up mock for GameController file system
            Mock<IFileSystem> mockFileSystemForGameController = new Mock<IFileSystem>();
            mockFileSystemForGameController.Setup(manager => manager.File.Exists("my_saved_game.game.json")).Returns(true); // Mock that file exists
            GameController.FileSystem = mockFileSystemForGameController.Object;

            Assert.Multiple(() =>
            {
                // Delete game and assert that return message is as expected
                Assert.That(new GameController().DeleteGame("my_saved_game"), Is.EqualTo("Game file my_saved_game has been successfully deleted"));

                // Verify that File.Delete was called once
                mockFileSystemForGameController.Verify(manager => manager.File.Delete("my_saved_game.game.json"), Times.Once);
            });
        }
    }
}