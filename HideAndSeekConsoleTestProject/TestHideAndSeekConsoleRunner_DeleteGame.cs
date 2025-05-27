using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.IO.Abstractions;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner unit tests for command to delete game
    /// </summary>
    [TestFixture]
    public class TestHideAndSeekConsoleRunner_DeleteGame
    {
        HideAndSeekConsoleRunner consoleRunner; // Console runner object to be tested
        Mock<IGameController> mockGameController; // GameController mock used by HideAndSeekConsoleRunner object
        StringBuilder sbActualTextDisplayed; // StringBuilder to capture actual text displayed (not including user input)
        Mock<IConsoleIO> mockConsoleIO; // IConsoleIO mock used by HideAndSeekConsoleRunner object

        [SetUp]
        public void SetUp()
        {
            // Initialize class variables
            mockGameController = new Mock<IGameController>();
            consoleRunner = null;
            mockConsoleIO = new Mock<IConsoleIO>();
            sbActualTextDisplayed = new StringBuilder();

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

        /// <summary>
        /// Helper method to get mocked IGetFileNamesAdapter that returns two saved game file names
        /// </summary>
        private static IGetFileNamesAdapter GetFileNamesAdapterReturning2SavedGameFileNames()
        {
            Mock<IGetFileNamesAdapter> mockAdapter = new Mock<IGetFileNamesAdapter>(); // Create adapter mock
            mockAdapter.Setup((a) => a.GetSavedGameFileNames())
                .Returns(new string[] { "myFile", "otherFile" }); // Set mock to return file names
            return mockAdapter.Object; // Return adapter object
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_DeleteGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_DeleteGame_TestData.TestCases_For_Test_ConsoleRunner_Delete))]
        [Category("ConsoleRunner Delete Success")]
        public void Test_ConsoleRunner_Delete(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeSuccessMessage)
        {
            // Set up mock GameController to return message when delete
            mockGameController.Setup((gc) => gc.DeleteGame("myFile")).Returns("[delete return message]");

            // Set up mock to return user input
            mockConsoleIO = GetMockConsoleIOWithUserInputSetUp(mockConsoleIO);

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns 2 file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, GetFileNamesAdapterReturning2SavedGameFileNames());

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    textBeforeSuccessMessage +
                    "[delete return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("myFile"), Times.Once);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_DeleteGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_DeleteGame_TestData.TestCases_For_Test_ConsoleRunner_Delete_WithInvalidFileName))]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_WithInvalidFileName(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeErrorMessage)
        {
            // Set up mock to return user input
            mockConsoleIO = GetMockConsoleIOWithUserInputSetUp(mockConsoleIO);

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns 2 file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, GetFileNamesAdapterReturning2SavedGameFileNames());

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    textBeforeErrorMessage +
                    "Cannot delete game because file name is invalid" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that GameController delete method was not called
                mockGameController.Verify((gc) => gc.DeleteGame(It.IsAny<string>()), Times.Never);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_DeleteGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_DeleteGame_TestData.TestCases_For_Test_ConsoleRunner_Delete_WithNonexistentFileName))]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_WithNonexistentFileName(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeErrorMessage)
        {
            // Set up mock to return user input
            mockConsoleIO = GetMockConsoleIOWithUserInputSetUp(mockConsoleIO);

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns 2 file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, GetFileNamesAdapterReturning2SavedGameFileNames());

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    textBeforeErrorMessage +
                    "Cannot delete game because file does not exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was not called
                mockGameController.Verify((gc) => gc.DeleteGame(It.IsAny<string>()), Times.Never);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_DeleteGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_DeleteGame_TestData.TestCases_For_Test_ConsoleRunner_Delete_ExceptionThrownByGameController))]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_ExceptionThrownByGameController(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeErrorMessage)
        {
            // Set up mock GameController to throw exception when delete
            mockGameController.Setup((gc) => gc.DeleteGame("myFile")).Throws(new Exception("[delete exception message]"));

            // Set up mock to return user input
            mockConsoleIO = GetMockConsoleIOWithUserInputSetUp(mockConsoleIO);

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns 2 file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, GetFileNamesAdapterReturning2SavedGameFileNames());

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    textBeforeErrorMessage +
                    "[delete exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was called
                mockGameController.Verify((gc) => gc.DeleteGame("myFile"), Times.Once);
            });
        }

        [TestCase("delete")]
        [TestCase("delete aFile")]
        [Category("ConsoleRunner Delete Failure")]
        public void Test_ConsoleRunner_Delete_NoExistingFiles(string deleteInput)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns(deleteInput) // Start delete
                .Returns("exit"); // Exit game

            // Create mock IGetFileNamesAdapter that returns no saved game file names
            Mock<IGetFileNamesAdapter> mockAdapter = new Mock<IGetFileNamesAdapter>(); // Create adapter mock
            mockAdapter.Setup((a) => a.GetSavedGameFileNames()).Returns(Array.Empty<string>()); // Set mock to return empty array (no file names)

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns no file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, mockAdapter.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Cannot delete game because no saved game files exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Delete method was not called
                mockGameController.Verify((gc) => gc.DeleteGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Delete Exit")]
        public void Test_ConsoleRunner_Delete_SeparateFileNameSelection_Exit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("delete") // Start delete
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController, IConsoleIO, and IGetFileNamesAdapter that returns 2 file names
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object, GetFileNamesAdapterReturning2SavedGameFileNames());

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the saved game files available:" + Environment.NewLine +
                    " - myFile" + Environment.NewLine +
                    " - otherFile" + Environment.NewLine +
                    "Enter the name of the saved game file to delete: "));

                // Verify that Delete method was not called
                mockGameController.Verify((gc) => gc.DeleteGame(It.IsAny<string>()), Times.Never);
            });
        }
    }
}