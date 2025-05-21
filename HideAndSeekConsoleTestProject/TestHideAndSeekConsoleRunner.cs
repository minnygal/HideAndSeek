using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    [TestFixture]
    public class TestHideAndSeekConsoleRunner
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
        }

        [Test]
        [Category("ConsoleRunner")]
        public void Test_ConsoleRunner_ProgramStarted_TextDisplayed()
        {
            // ARRANGE
            // Set up mock GameController to return Status
            mockGameController.SetupGet((gc) => gc.Status).Returns(
                "You are in the Entry. You see the following exits:" + Environment.NewLine +
                " - the Hallway is to the East" + Environment.NewLine +
                " - the Garage is Out" + Environment.NewLine +
                "You have not found any opponents");

            // Set up mock GameController to return Prompt
            mockGameController.SetupGet((gc) => gc.Prompt).Returns("1: Which direction do you want to go: ");

            // Set up mock to return user input to terminate program
            mockConsoleIO.Setup((cio) => cio.ReadLine()).Returns("exit");

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // ACT
            // Run game
            consoleRunner.RunGame();

            // ASSERT
            Assert.That(sbActualTextDisplayed.ToString(), Is.EqualTo(
                "Welcome to the Hide And Seek Console App!" + Environment.NewLine +
                "Navigate through rooms in a virtual house to find all the hiding opponents in the fewest number of moves possible." + Environment.NewLine +
                "-To MOVE, enter the direction in which you want to move." + Environment.NewLine +
                "-To CHECK if any opponents are hiding in your current location, enter \"check\"." + Environment.NewLine +
                "-To TELEPORT to a random location with a hiding place, enter \"teleport\"." + Environment.NewLine +
                "-For a NEW custom game, enter \"new\" and follow the prompts." + Environment.NewLine +
                "-To SAVE your progress, enter \"save\" followed by a space and a name for your game." + Environment.NewLine +
                "-To LOAD a saved game, enter \"load\" followed by a space and the name of your game." + Environment.NewLine +
                "-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game." + Environment.NewLine +
                "-To EXIT the program, enter \"exit\"" + Environment.NewLine +
                Environment.NewLine +
                "Welcome to tester house!" + Environment.NewLine +
                "You are in the Entry. You see the following exits:" + Environment.NewLine +
                " - the Hallway is to the East" + Environment.NewLine +
                " - the Garage is Out" + Environment.NewLine +
                "You have not found any opponents" + Environment.NewLine +
                "1: Which direction do you want to go: ")); // Assert that text displayed is as expected
        }

        [Test]
        [Category("ConsoleRunner Move Success")]
        public void Test_ConsoleRunner_Move()
        {
            // ARRANGE
            // Set up mock GameController to return Status
            mockGameController.SetupSequence((gc) => gc.Status)
                .Returns(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents")
                .Returns("You are in the Garage. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is In" + Environment.NewLine +
                    "You have not found any opponents");

            // Set up mock GameController to return Prompt
            mockGameController.SetupSequence((gc) => gc.Prompt)
                .Returns("1: Which direction do you want to go: ")
                .Returns("2: Which direction do you want to go (or type 'check'): ");

            // Set up mock GameController to return message when Move
            mockGameController.Setup((gc) => gc.Move(Direction.Out)).Returns("Moving Out");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("out") // Move to Garage
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // ACT
            // Run game
            consoleRunner.RunGame();

            // ASSERT
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "Welcome to tester house!" + Environment.NewLine +
                "You are in the Entry. You see the following exits:" + Environment.NewLine +
                " - the Hallway is to the East" + Environment.NewLine +
                " - the Garage is Out" + Environment.NewLine +
                "You have not found any opponents" + Environment.NewLine +
                "1: Which direction do you want to go: " +
                "Moving Out" + Environment.NewLine +
                Environment.NewLine +
                "You are in the Garage. You see the following exit:" + Environment.NewLine +
                " - the Hallway is In" + Environment.NewLine +
                "You have not found any opponents" + Environment.NewLine +
                "2: Which direction do you want to go (or type 'check'): "));
        }

        [Test]
        [Category("ConsoleRunner Move Failure")]
        public void Test_ConsoleRunner_Move_NoLocationInDirection()
        {
            // ARRANGE
            // Set up mock GameController to return Status
            mockGameController.Setup((gc) => gc.Status).Returns(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents");

            // Set up mock GameController to return Prompt
            mockGameController.Setup((gc) => gc.Prompt)
                .Returns("1: Which direction do you want to go: ");

            // Set up mock GameController to throw exception when Move
            mockGameController.Setup((gc) => gc.Move(Direction.Up))
                .Throws(new InvalidOperationException("There is no exit for location \"Hallway\" in direction \"Up\""));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("up") // Move Up
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // ACT
            // Run game
            consoleRunner.RunGame();

            Console.WriteLine(sbActualTextDisplayed.ToString());

            // ASSERT
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "Welcome to tester house!" + Environment.NewLine +
                "You are in the Entry. You see the following exits:" + Environment.NewLine +
                " - the Hallway is to the East" + Environment.NewLine +
                " - the Garage is Out" + Environment.NewLine +
                "You have not found any opponents" + Environment.NewLine +
                "1: Which direction do you want to go: " +
                "There is no exit for location \"Hallway\" in direction \"Up\"" + Environment.NewLine +
                Environment.NewLine +
                "You are in the Entry. You see the following exits:" + Environment.NewLine +
                " - the Hallway is to the East" + Environment.NewLine +
                " - the Garage is Out" + Environment.NewLine +
                "You have not found any opponents" + Environment.NewLine +
                "1: Which direction do you want to go: "));
        }
    }
}