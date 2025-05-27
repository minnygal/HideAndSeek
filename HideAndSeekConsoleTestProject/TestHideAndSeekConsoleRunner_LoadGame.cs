using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.IO.Abstractions;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner tests for command to load game
    /// 
    /// 
    /// These are integration tests relying upon SavedGame's GetSavedGameFileNames static method
    /// and the file system stored in its static property.
    /// The file system is mocked to control the output.
    /// </summary>
    [TestFixture]
    public class TestHideAndSeekConsoleRunner_LoadGame
    {
        HideAndSeekConsoleRunner consoleRunner; // Console runner object to be tested
        Mock<IGameController> mockGameController; // GameController mock used by HideAndSeekConsoleRunner object
        StringBuilder sbActualTextDisplayed; // StringBuilder to capture actual text displayed (not including user input)
        Mock<IConsoleIO> mockConsoleIO; // IConsoleIO mock used by HideAndSeekConsoleRunner object
        Mock<IFileSystem> mockFileSystem; // IFileSystem mock used by SavedGame class

        [SetUp]
        public void SetUp()
        {
            // Reset SavedGame file system (may or may not be reassigned to mock in test)
            SavedGame.FileSystem = new FileSystem();

            // Initialize class variables
            mockGameController = new Mock<IGameController>();
            consoleRunner = null;
            mockConsoleIO = new Mock<IConsoleIO>();
            sbActualTextDisplayed = new StringBuilder();
            mockFileSystem = new Mock<IFileSystem>();

            // Set up mock IConsoleIO to write text to StringBuilder
            mockConsoleIO.Setup((cio) => cio.WriteLine(It.IsAny<string>())).Callback((string message) =>
            {
                sbActualTextDisplayed.AppendSafeWithNewLine(message); // Append text written to StringBuilder
            });

            mockConsoleIO.Setup((cio) => cio.WriteLine()).Callback(() =>
            {
                sbActualTextDisplayed.Append(Environment.NewLine); // Append new line to StringBuilder
            });

            mockConsoleIO.Setup((cio) => cio.Write(It.IsAny<string>())).Callback((string message) =>
            {
                sbActualTextDisplayed.Append(message); // Append text written to StringBuilder
            });


            // Set up mock GameController to return House name (used in welcome message)
            mockGameController.SetupGet((gc) => gc.House.Name).Returns("tester house");

            // Set up mock GameController to return Status
            mockGameController.SetupGet((gc) => gc.Status).Returns("[status]");

            // Set up mock GameController to return Prompt
            mockGameController.SetupGet((gc) => gc.Prompt).Returns("[prompt]");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            SavedGame.FileSystem = new FileSystem(); // Reset SavedGame file system
        }

        /// <summary>
        /// Helper method to set up mock file system to return two files
        /// </summary>
        private void SetUpMockFileSystemToReturnTwoFiles()
        {
            // Set up mock file system
            mockFileSystem.Setup((fs) => fs.Path.GetDirectoryName(It.IsAny<string>())).Returns("directoryName"); // Return directory name
            mockFileSystem.Setup((fs) => fs.Directory.GetFiles("directoryName"))
                .Returns(new string[] { "myFile.game.json", "otherFile.game.json" }); // Return file names
            SavedGame.FileSystem = mockFileSystem.Object; // Set SavedGame file system to mock file system
        }

        [Test]
        [Category("ConsoleRunner Load Success")]
        public void Test_ConsoleRunner_Load_WithFileNameEntered()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock GameController to return message when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Returns("[load return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load myFile") // Load game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[load return message]" + Environment.NewLine +
                    "Welcome to tester house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was called
                mockGameController.Verify((gc) => gc.LoadGame("myFile"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_WithFileNameEntered_InvalidFileName()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load \\/ file") // Load game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Cannot load game because file name is invalid" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that GameController load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_WithFileNameEntered_NonexistentFileName()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load nonexistentFile") // Load game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Cannot load game because file does not exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_WithFileNameEntered_ExceptionThrownByGameController()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock GameController to throw exception when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Throws(new Exception("[load exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load myFile") // Start load
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[load exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was called
                mockGameController.Verify((gc) => gc.LoadGame("myFile"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Success")]
        public void Test_ConsoleRunner_Load_SelectFileNameSeparately()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock GameController to return message when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Returns("[load return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load") // Start load
                .Returns("myFile") // Specify file
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the saved game files available:" + Environment.NewLine +
                    " - myFile" + Environment.NewLine +
                    " - otherFile" + Environment.NewLine +
                    "Enter the name of the saved game file to load: " +
                    "[load return message]" + Environment.NewLine +
                    "Welcome to tester house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was called
                mockGameController.Verify((gc) => gc.LoadGame("myFile"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_SelectFileNameSeparately_InvalidFileName()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load") // Start load
                .Returns("\\/ invalid") // Specify file
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the saved game files available:" + Environment.NewLine +
                    " - myFile" + Environment.NewLine +
                    " - otherFile" + Environment.NewLine +
                    "Enter the name of the saved game file to load: " +
                    "Cannot load game because file name is invalid" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_SelectFileNameSeparately_ExceptionThrownByGameController()
        {
            // Set up mock file system to return two files
            SetUpMockFileSystemToReturnTwoFiles();

            // Set up mock GameController to throw exception when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Throws(new Exception("[load exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load") // Start load
                .Returns("myFile") // Specify file
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the saved game files available:" + Environment.NewLine +
                    " - myFile" + Environment.NewLine +
                    " - otherFile" + Environment.NewLine +
                    "Enter the name of the saved game file to load: " +
                    "[load exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was called
                mockGameController.Verify((gc) => gc.LoadGame("myFile"), Times.Once);
            });
        }

        [TestCase("load")]
        [TestCase("load aFile")]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_NoExistingFiles(string loadInput)
        {
            // Set up mock file system to return no files
            mockFileSystem.Setup((fs) => fs.Path.GetDirectoryName(It.IsAny<string>())).Returns("directoryName"); // Return directory name
            mockFileSystem.Setup((fs) => fs.Directory.GetFiles("directoryName"))
                .Returns(Array.Empty<string>()); // Return empty array (no existing files)
            SavedGame.FileSystem = mockFileSystem.Object; // Set SavedGame file system to mock file system

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns(loadInput) // Start load
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Cannot load game because no saved game files exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }
    }
}