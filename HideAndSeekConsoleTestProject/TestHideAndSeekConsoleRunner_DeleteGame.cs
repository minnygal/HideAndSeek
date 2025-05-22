using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner tests for command to delete game
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

        [Test]
        [Category("ConsoleRunner Delete Success")]
        public void Test_ConsoleRunner_Delete_WithFileNameEntered()
        {
            // Set up mock GameController to return message when save
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
    }
}