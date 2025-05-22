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

            // Set up mock GameController to return Status
            mockGameController.SetupGet((gc) => gc.Status).Returns("[status]");

            // Set up mock GameController to return Prompt
            mockGameController.SetupGet((gc) => gc.Prompt).Returns("[prompt]");
        }

        [Test]
        [Category("ConsoleRunner Exit")]
        public void Test_ConsoleRunner_ProgramStarted_AndExit()
        {
            // Set up mock to return user input to terminate program
            mockConsoleIO.Setup((cio) => cio.ReadLine()).Returns("exit");

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            // Assert text displayed is as expected
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
                "[status]" + Environment.NewLine +
                "[prompt]")); // Assert that text displayed is as expected
        }

        [Test]
        [Category("ConsoleRunner Move Success")]
        public void Test_ConsoleRunner_Move()
        {
            // Set up mock GameController to return message when Move
            mockGameController.Setup((gc) => gc.Move(Direction.Out)).Returns("Moving Out");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("out") // Move to Garage
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Moving Out" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Move method was called
                mockGameController.Verify((gc) => gc.Move(Direction.Out), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Move Failure")]
        public void Test_ConsoleRunner_Move_NoLocationInDirection()
        {
            // Set up mock GameController to throw exception when Move
            mockGameController.Setup((gc) => gc.Move(Direction.Up))
                .Throws(new InvalidOperationException("There is no exit for location \"Hallway\" in direction \"Up\""));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("up") // Move Up
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "There is no exit for location \"Hallway\" in direction \"Up\"" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Move method was called
                mockGameController.Verify((gc) => gc.Move(Direction.Up), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Check Success")]
        public void Test_ConsoleRunner_Check()
        {
            // Set up mock GameController to return message when check
            mockGameController.Setup((gc) => gc.CheckCurrentLocation()).Returns("[check current location return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("check") // Move Up
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[check current location return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that CheckCurrentLocation method was called
                mockGameController.Verify((gc) => gc.CheckCurrentLocation(), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Check Failure")]
        public void Test_ConsoleRunner_Check_NoHidingPlace()
        {
            // Set up mock GameController to throw exception when check
            mockGameController.Setup((gc) => gc.CheckCurrentLocation())
                .Throws(new InvalidOperationException("There is no hiding place in the Hallway"));

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("check") // Move Up
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "There is no hiding place in the Hallway" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that CheckCurrentLocation method was called
                mockGameController.Verify((gc) => gc.CheckCurrentLocation(), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner Teleport Success")]
        public void Test_ConsoleRunner_Teleport()
        {
            // Set up mock GameController to return message when teleport
            mockGameController.Setup((gc) => gc.Teleport()).Returns("[teleport return message]");

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("teleport") // Teleport
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[teleport return message]" + Environment.NewLine +
                    Environment.NewLine +
                    "[status]" + Environment.NewLine +
                    "[prompt]"));

                // Verify that Teleport method was called
                mockGameController.Verify((gc) => gc.Teleport(), Times.Once);
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("invalid")]
        [TestCase("go east")]
        [Category("ConsoleRunner Failure")]
        public void Test_ConsoleRunner_InvalidCommand(string command)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns(command) // Command
                .Returns("exit"); // Exit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            // Assert text displayed is as expected
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "That's not a valid direction" + Environment.NewLine +
                Environment.NewLine +
                "[status]" + Environment.NewLine +
                "[prompt]"));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("oh my")]
        [TestCase("bye")]
        [Category("ConsoleRunner GameOver Move Check Success")]
        public void Test_ConsoleRunner_GameOver_DoNotPlayAgain(string inputToQuit)
        {
            // Set up mock GameController to return whether game is over
            mockGameController.SetupSequence((gc) => gc.GameOver)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            // Set up mock GameController to implement check method
            mockGameController.Setup((gc) => gc.CheckCurrentLocation()).Returns("[check current location return message]");

            // Set up mock GameController to implement move method
            mockGameController.Setup((gc) => gc.Move(Direction.Out)).Returns("[move return message]");

            // Set up mock GameController to return move number
            mockGameController.Setup((gc) => gc.MoveNumber).Returns(2);

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("out") // Move to Garage
                .Returns("check") // Check current location
                .Returns(inputToQuit); // Quit game

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            // Assert text displayed is as expected
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "[check current location return message]" + Environment.NewLine +
                Environment.NewLine +
                "You won the game in 2 moves!" + Environment.NewLine +
                "Press P to play again, any other key to quit. "));
        }

        [TestCase('P')]
        [TestCase('p')]
        [Category("ConsoleRunner GameOver Move Check Success")]
        public void Test_ConsoleRunner_GameOver_PlayAgain(char inputToPlayAgain)
        {
            // Set up mock GameController to return whether game is over
            mockGameController.SetupSequence((gc) => gc.GameOver)
                .Returns(false)
                .Returns(false)
                .Returns(true)
                .Returns(false);

            // Set up mock GameController to implement check method
            mockGameController.Setup((gc) => gc.CheckCurrentLocation()).Returns("[check current location return message]");

            // Set up mock GameController to implement move method
            mockGameController.Setup((gc) => gc.Move(Direction.Out)).Returns("[move return message]");

            // Set up mock GameController to return move number
            mockGameController.Setup((gc) => gc.MoveNumber).Returns(2);

            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("out") // Move to Garage
                .Returns("check") // Check current location
                .Returns("exit"); // Quit game

            // Set up mock to return user input to play again
            mockConsoleIO.Setup((cio) => cio.ReadKey()).Returns(new ConsoleKeyInfo(inputToPlayAgain, ConsoleKey.P, false, false, false)); // Play again

            // Create console runner with mocked GameController and IConsoleIO
            consoleRunner = new HideAndSeekConsoleRunner(mockGameController.Object, mockConsoleIO.Object);

            // Run game
            consoleRunner.RunGame();

            // Assert text displayed is as expected
            Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                "[check current location return message]" + Environment.NewLine +
                Environment.NewLine +
                "You won the game in 2 moves!" + Environment.NewLine +
                "Press P to play again, any other key to quit. " +
                Environment.NewLine +
                "Welcome to tester house!" + Environment.NewLine +
                "[status]" + Environment.NewLine +
                "[prompt]"));
        }
    }
}