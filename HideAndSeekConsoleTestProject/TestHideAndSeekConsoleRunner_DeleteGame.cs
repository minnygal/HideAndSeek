using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.IO.Abstractions;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner tests for command to delete game
    /// 
    /// SelectFileNameSeparately tests are integration tests 
    /// relying upon SavedGame's GetSavedGameFileNames static method.
    /// The file system is mocked to control the output,
    /// but changes to GetSavedGameFileNames's implementation will impact this test class.
    /// </summary>
    [TestFixture]
    public class TestHideAndSeekConsoleRunner_DeleteGame
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

        [Test]
        [Category("ConsoleRunner Delete Success")]
        public void Test_ConsoleRunner_Delete_WithFileNameEntered()
        {
            // Set up mock GameController to return message when delete
            mockGameController.Setup((gc) => gc.DeleteGame("fileName")).Returns("[delete return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("delete fileName") // Delete game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[delete return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("fileName"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_WithFileNameEntered_InvalidFileName()
        {
            // Set up mock GameController to throw exception when delete
            mockGameController.Setup((gc) => gc.DeleteGame("(]}{)file"))
                .Throws(new ArgumentException("[delete exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("delete (]}{)file") // Delete game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[delete exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("(]}{)file"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_WithFileNameEntered_NonexistentFileName()
        {
            // Set up mock GameController to throw exception when delete
            mockGameController.Setup((gc) => gc.DeleteGame("nonexistentFile"))
                .Throws(new FileNotFoundException("[delete exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("delete nonexistentFile") // Delete game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[delete exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("nonexistentFile"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Delete Success")]
        public void Test_ConsoleRunner_Delete_SelectFileNameSeparately()
        {
            // Set up mock file system
            mockFileSystem.Setup((fs) => fs.Path.GetDirectoryName(It.IsAny<string>())).Returns("directoryName"); // Return directory name
            mockFileSystem.Setup((fs) => fs.Directory.GetFiles("directoryName"))
                .Returns(new string[] { "myFile.game.json", "otherFile.game.json" }); // Return file names
            SavedGame.FileSystem = mockFileSystem.Object; // Set SavedGame file system to mock file system

            // Set up mock GameController to return message when delete
            mockGameController.Setup((gc) => gc.DeleteGame("myFile")).Returns("[delete return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("delete") // Start delete
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
                    "Enter the name of the saved game file to delete: " +
                    "[delete return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("myFile"), Times.Once);
            });
        }

    }
}