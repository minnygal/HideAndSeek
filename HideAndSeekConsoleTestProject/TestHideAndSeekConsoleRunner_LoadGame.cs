using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner unit tests for command to load game
    /// </summary>
    [TestFixture]
    public class TestHideAndSeekConsoleRunner_LoadGame
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
        /// <returns>Mocked IGetFileNamesAdapter returning two saved game file names</returns>
        private static IGetFileNamesAdapter GetFileNamesAdapterReturning2SavedGameFileNames()
        {
            Mock<IGetFileNamesAdapter> mockAdapter = new Mock<IGetFileNamesAdapter>(); // Create adapter mock
            mockAdapter.Setup((a) => a.GetSavedGameFileNames()).Returns(new string[] { "myFile", "otherFile" }); // Set mock to return file names
            return mockAdapter.Object; // Return adapter object
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_LoadGame_TestData), 
            nameof(TestHideAndSeekConsoleRunner_LoadGame_TestData.TestCases_For_Test_ConsoleRunner_Load))]
        [Category("ConsoleRunner Load Success")]
        public void Test_ConsoleRunner_Load(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeSuccessMessage)
        {
            // Set up mock GameController to return message when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Returns("[load return message]");

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
                    "[load return message]" + Environment.NewLine +
                    "Welcome to tester house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was called
                mockGameController.Verify((gc) => gc.LoadGame("myFile"), Times.Once);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_LoadGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_LoadGame_TestData.TestCases_For_Test_ConsoleRunner_Load_WithInvalidFileName))]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_WithInvalidFileName(
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
                    "Cannot load game because file name \"\\/ invalid\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that GameController load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_LoadGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_LoadGame_TestData.TestCases_For_Test_ConsoleRunner_Load_WithNonexistentFileName))]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_WithNonexistentFileName(
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
                    "Cannot load game because file does not exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [TestCaseSource(typeof(TestHideAndSeekConsoleRunner_LoadGame_TestData),
            nameof(TestHideAndSeekConsoleRunner_LoadGame_TestData.TestCases_For_Test_ConsoleRunner_Load_ExceptionThrownByGameController))]
        [Category("ConsoleRunner Load Failure")]
        public void Test_ConsoleRunner_Load_ExceptionThrownByGameController(
            Func<Mock<IConsoleIO>, Mock<IConsoleIO>> GetMockConsoleIOWithUserInputSetUp, string textBeforeErrorMessage)
        {
            // Set up mock GameController to throw exception when load
            mockGameController.Setup((gc) => gc.LoadGame("myFile")).Throws(new Exception("[load exception message]"));

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
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns(loadInput) // Start load
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
                    "Cannot load game because no saved game files exist" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Load Exit")]
        public void Test_ConsoleRunner_Load_SeparateFileNameSelection_Exit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("load") // Start load
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
                    "Enter the name of the saved game file to load: "));

                // Verify that Load method was not called
                mockGameController.Verify((gc) => gc.LoadGame(It.IsAny<string>()), Times.Never);
            });
        }
    }
}