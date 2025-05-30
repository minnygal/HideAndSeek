using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner unit tests for command to save game
    /// </summary>
    [TestFixture]
    public class TestHideAndSeekConsoleRunner_SaveGame
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

        [Test]
        [Category("ConsoleRunner Save Success")]
        public void Test_ConsoleRunner_Save()
        {
            // Set up mock GameController to return message when save
            mockGameController.Setup((gc) => gc.SaveGame("fileName")).Returns("[save return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("save fileName") // Save game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[save return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that SaveGame method was called
                mockGameController.Verify((gc) => gc.SaveGame("fileName"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Save Failure")]
        public void Test_ConsoleRunner_Save_InvalidFileName()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("save \\ /") // Save game with invalid file name
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    $"Cannot save game because file name \"\\ /\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that SaveGame method was not called
                mockGameController.Verify((gc) => gc.SaveGame(It.IsAny<string>()), Times.Never);
            });
        }

        [Test]
        [Category("ConsoleRunner Save Failure")]
        public void Test_ConsoleRunner_Save_AlreadyExistingFileName()
        {
            // Set up mock GameController to return message when save
            mockGameController.Setup((gc) => gc.SaveGame("alreadyExists"))
                .Throws(new InvalidOperationException("[save exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("save alreadyExists") // Save game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[save exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Move method was called
                mockGameController.Verify((gc) => gc.SaveGame("alreadyExists"), Times.Once);
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("ConsoleRunner Save Failure")]
        public void Test_ConsoleRunner_Save_NoFileName(string fileNameText)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns($"save{fileNameText}") // Save game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            // Assert text displayed is as expected
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "Cannot save game because no file name was entered" + Environment.NewLine +
                Environment.NewLine +
                "[status]" + Environment.NewLine +
                "[prompt]"));
        }

        [Test]
        [Category("ConsoleRunner Save Failure")]
        public void Test_ConsoleRunner_Save_ExceptionThrownByGameController()
        {
            // Set up mock GameController to throw exception when save
            mockGameController.Setup((gc) => gc.SaveGame("fileName")).Throws(new Exception("[save exception message]"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("save fileName") // Save game
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[save exception message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that SaveGame method was called
                mockGameController.Verify((gc) => gc.SaveGame("fileName"), Times.Once);
            });
        }
    }
}