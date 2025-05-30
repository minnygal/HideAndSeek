using HideAndSeek;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using Moq;
using System.Text;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// HideAndSeekConsoleRunner unit tests for command to start new game
    /// Verify constructor called (or not called) as expected based on user input for opponents
    /// Verify RestartGame called (or not called) as expected based on user input for house file name
    /// </summary>

    [TestFixture]
    public class TestHideAndSeekConsoleRunner_NewGame
    {
        HideAndSeekConsoleRunner consoleRunner; // Console runner object to be tested
        Mock<IGameController> mockInitialGameController; // GameController mock initially used by HideAndSeekConsoleRunner object
        Mock<IGameControllerFactory> mockGameControllerFactory; // GameControllerFactory mock used by HideAndSeekConsoleRunner object
        StringBuilder sbActualTextDisplayed; // StringBuilder to capture actual text displayed (not including user input)
        Mock<IConsoleIO> mockConsoleIO; // IConsoleIO mock used by HideAndSeekConsoleRunner object

        [SetUp]
        public void SetUp()
        {
            // Initialize class variables
            consoleRunner = null;
            mockInitialGameController = new Mock<IGameController>();
            mockGameControllerFactory = new Mock<IGameControllerFactory>();
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

            // Set up mock initial game controller to return appropriate values
            mockInitialGameController.SetupGet((gc) => gc.House.Name).Returns("initial house"); // Return House name (used in welcome message)
            mockInitialGameController.SetupGet((gc) => gc.Status).Returns("[initial gc status]"); // Return status
            mockInitialGameController.SetupGet((gc) => gc.Prompt).Returns("[initial gc prompt]"); // Return prompt
        }

        /// <summary>
        /// Helper method to get mocked IGetFileNamesAdapter that returns two house file names
        /// </summary>
        /// <returns>Mocked IGetFileNamesAdapter returning two house file names</returns>
        private static IGetFileNamesAdapter GetFileNamesAdapterReturning2HouseFileNames()
        {
            Mock<IGetFileNamesAdapter> mockAdapter = new Mock<IGetFileNamesAdapter>(); // Create adapter mock
            mockAdapter.Setup((a) => a.GetHouseFileNames()).Returns(new string[] { "DefaultHouse", "NewHouse" }); // Set mock to return file names
            return mockAdapter.Object; // Return adapter object
        }

        /// <summary>
        /// Helper method to return mock game controller with properties' getters set
        /// </summary>
        /// <returns>Mock game controller with properties' getters set</returns>
        private static Mock<IGameController> GetGameControllerMockForNewGameController()
        {
            // Create new mock game controller and set up to return appropriate values for properties
            Mock<IGameController> mockNewGameController = new Mock<IGameController>(); // Create new mock game controller
            mockNewGameController.SetupGet((gc) => gc.House.Name).Returns("new house"); // Return House name (used in welcome message)
            mockNewGameController.SetupGet((gc) => gc.Status).Returns("[new gc status]"); // Return status
            mockNewGameController.SetupGet((gc) => gc.Prompt).Returns("[new gc prompt]"); // Return prompt
            return mockNewGameController;
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Default Success")]
        public void Test_ConsoleRunner_NewGame_Opponents_Default_House_Default()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("") // Default house
                .Returns("exit"); // Exit game

            // Set variable to new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = GetGameControllerMockForNewGameController();

            // Set up game controller factory to return mock game controller object
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object, 
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "New game started" + Environment.NewLine +
                    "Welcome to new house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[new gc status]" + Environment.NewLine +
                    "[new gc prompt]"));

                // Verify that expected game controller factory method was called
                mockGameControllerFactory.Verify((factory) => factory.GetDefaultGameController(), Times.Once());

                // Verify that new game controller RestartGame method with file name was not called
                mockNewGameController.Verify((gc) => gc.RestartGame(It.IsAny<string>()), Times.Never());
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Quit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("exit"); // Exit game

            // Set up game controller factory to return mock game controller object
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(new Mock<IGameController>().Object);

            // Create console runner with mocked GameController, IConsoleIO, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object, mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: "));

                // Verify that no methods were called on game controller factory
                mockGameControllerFactory.VerifyNoOtherCalls();
            });
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [Category("ConsoleRunner NewGame Opponents Number Success Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Number(int numberOfOpponents)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns(numberOfOpponents.ToString()) // Number of opponents
                .Returns("exit"); // Exit game

            // Set up game controller factory
            mockGameControllerFactory.SetupProperty((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, 10); // Set maximum number of opponents with default names to 10
            mockGameControllerFactory.Setup((factory) => factory.GetGameControllerWithSpecificNumberOfOpponents(numberOfOpponents))
                                     .Returns(new Mock<IGameController>().Object); // Method returns mock game controller object

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify that game controller factory method and property called as expected
                mockGameControllerFactory.Verify((factory) => factory.GetGameControllerWithSpecificNumberOfOpponents(numberOfOpponents), Times.Once()); //Method called with valid number of opponents
                mockGameControllerFactory.Verify((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, Times.Once); // Property accessed
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(11)]
        [TestCase(100)]
        [Category("ConsoleRunner NewGame Opponents Number Failure Success Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Number_OutOfRange_ThenValid(int invalidNumberOfOpponents)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns(invalidNumberOfOpponents.ToString()) // Invalid number for number of opponents
                .Returns("1") // Valid number for number of opponents
                .Returns("exit"); // Exit game

            // Set up mock game controller factory
            mockGameControllerFactory.SetupProperty((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, 10); // Set maximum number of opponents with default names
            mockGameControllerFactory.Setup((factory) => factory.GetGameControllerWithSpecificNumberOfOpponents(1))
                                     .Returns(new Mock<IGameController>().Object); // Method returns mock game controller object when valid number of opponents entered

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Cannot create a new game because the number of opponents specified is invalid (must be between 1 and 10)" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify that game controller factory method and property called as expected
                mockGameControllerFactory.Verify((factory) => factory.GetGameControllerWithSpecificNumberOfOpponents(1), Times.Once()); // Method called with valid number of opponents only
                mockGameControllerFactory.Verify((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, Times.AtLeast(2)); // Property accessed
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Number Failure Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Number_OutOfRange_ThenQuit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("0") // Invalid number for number of opponents
                .Returns("exit"); // Exit game

            // Set up mock game controller factory
            mockGameControllerFactory.SetupProperty((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, 10); // Set maximum number of opponents with default names
            
            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Cannot create a new game because the number of opponents specified is invalid (must be between 1 and 10)" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: "));

                // Verify that game controller factory property accessed as expected but no other calls
                mockGameControllerFactory.Verify((factory) => factory.MaximumNumberOfOpponentsWithDefaultNames, Times.Once); // Property accessed
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [TestCase("Alice", new string[] { "Alice" })]
        [TestCase("@Homebody, George", new string[] { "@Homebody", "George" })]
        [TestCase("1Luv, George", new string[] { "1Luv", "George" })]
        [TestCase("Mary Ann, George", new string[] { "Mary Ann", "George" })]
        [TestCase("Harry,George", new string[] { "Harry", "George" })]
        [TestCase("@Awesome, Named1, Harry, Mary Ann, Peter, James, John, Eve, Hope, Patrick, Jill", 
                  new string[] { "@Awesome", "Named1", "Harry", "Mary Ann", "Peter", "James", "John", "Eve", "Hope", "Patrick", "Jill" })]
        [Category("ConsoleRunner NewGame Opponents Names Success Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Names(string namesOfOpponentsUserInput, string[] namesOfOpponentsPassedToFactory)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns(namesOfOpponentsUserInput) // Names of opponents
                .Returns("exit"); // Exit game

            // Set up game controller factory
            mockGameControllerFactory.Setup((factory) => factory.IsValidOpponentName(It.IsAny<string>())).Returns(true); // Return that inputs are valid opponent names
            mockGameControllerFactory.Setup((factory) => factory.GetGameControllerWithSpecificNamesOfOpponents(namesOfOpponentsPassedToFactory))
                                     .Returns(new Mock<IGameController>().Object); // Method returns mock game controller object

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify that game controller factory method and property called as expected
                mockGameControllerFactory.Verify((factory) => factory.IsValidOpponentName(It.IsAny<string>()), Times.AtLeastOnce); // Method called to check if opponent names are valid
                mockGameControllerFactory.Verify((factory) => factory.GetGameControllerWithSpecificNamesOfOpponents(namesOfOpponentsPassedToFactory), Times.Once()); //Method called with valid names of opponents
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [TestCase(",")]
        [TestCase("George,")]
        [TestCase("George, ")]
        [TestCase(",George")]
        [TestCase(" ,George")]
        [Category("ConsoleRunner NewGame Opponents Names Failure Success Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Names_Invalid_ThenValid(string invalidNamesOfOpponentsUserInput)
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns(invalidNamesOfOpponentsUserInput) // Invalid name/s for opponents
                .Returns("John") // Valid name for opponent
                .Returns("exit"); // Exit game

            // Set up mock game controller factory
            mockGameControllerFactory.Setup((factory) => factory.IsValidOpponentName(It.IsAny<string>())).Returns(false); // Return that inputs are not valid opponent names
            mockGameControllerFactory.Setup((factory) => factory.IsValidOpponentName("John")).Returns(true); // Return that John is a valid opponent name
            mockGameControllerFactory.Setup((factory) => factory.IsValidOpponentName("George")).Returns(true); // Return that George is a valid opponent name
            mockGameControllerFactory.Setup((factory) => factory.GetGameControllerWithSpecificNamesOfOpponents(new string[] { "John" }))
                                     .Returns(new Mock<IGameController>().Object); // Method returns mock game controller object when valid opponent name entered

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Cannot create a new game because opponent name \"\" is invalid (is empty or contains only whitespace)" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify that game controller factory methods called as expected
                mockGameControllerFactory.Verify((factory) => factory.GetGameControllerWithSpecificNamesOfOpponents(new string[] { "John" }), Times.Once()); // Method called with valid name of opponent
                mockGameControllerFactory.Verify((factory) => factory.IsValidOpponentName(It.IsAny<string>()), Times.AtLeast(2)); // Method called to check if opponent names are valid
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Names Failure Quit")]
        public void Test_ConsoleRunner_NewGame_Opponents_Names_Invalid_ThenQuit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns(",") // Invalid names for opponents
                .Returns("exit"); // Exit game

            // Set up mock game controller factory
            mockGameControllerFactory.Setup((factory) => factory.IsValidOpponentName(It.IsAny<string>())).Returns(false); // Return that inputs are not valid opponent names

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "[initial gc status]" + Environment.NewLine +
                    "[initial gc prompt]" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: " +
                    "Cannot create a new game because opponent name \"\" is invalid (is empty or contains only whitespace)" + Environment.NewLine +
                    "How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: "));

                // Verify that game controller factory property called as expected but no other calls
                mockGameControllerFactory.Verify((factory) => factory.IsValidOpponentName(It.IsAny<string>()), Times.Once); // Method called to check if opponent names are valid
                mockGameControllerFactory.VerifyNoOtherCalls(); // No other calls
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Quit")]
        public void Test_ConsoleRunner_NewGame_House_Quit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("exit"); // Exit game

            // Create new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = new Mock<IGameController>();

            // Set up game controller factory to return mock game controller object with no setup
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify no methods were called on mock new game controller
                mockNewGameController.VerifyNoOtherCalls();
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Name Success Quit")]
        public void Test_ConsoleRunner_NewGame_House_ValidNameOfExistingFile()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("NewHouse") // Specific House
                .Returns("exit"); // Exit game

            // Create and set up new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = GetGameControllerMockForNewGameController();
            mockNewGameController.Setup((gc) => gc.RestartGame("NewHouse")).Returns(mockNewGameController.Object); // RestartGame returns mocked game controller

            // Set up game controller factory to return mock game controller object
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "New game started" + Environment.NewLine +
                    "Welcome to new house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[new gc status]" + Environment.NewLine +
                    "[new gc prompt]"));

                // Verify method called once to restart game with specific House file name
                mockNewGameController.Verify((gc) => gc.RestartGame("NewHouse"), Times.Once); // Method called once to restart game with specific House file name
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Name Failure Quit")]
        public void Test_ConsoleRunner_NewGame_House_InvalidName_ThenQuit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("\\ //") // Invalid House file name
                .Returns("exit"); // Exit game

            // Create and set up new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = GetGameControllerMockForNewGameController();

            // Set up game controller factory to return mock game controller object
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "Cannot load house layout because file name \"\\ //\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)" + Environment.NewLine +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify no calls to mock new game controller
                mockNewGameController.VerifyNoOtherCalls();
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Name Failure Success Quit")]
        public void Test_ConsoleRunner_NewGame_House_InvalidName_ThenValid()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("\\ //") // Invalid House file name
                .Returns("NewHouse") // Name of existing House file
                .Returns("exit"); // Exit game

            // Create and set up new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = GetGameControllerMockForNewGameController();
            mockNewGameController.Setup((gc) => gc.RestartGame("NewHouse")).Returns(mockNewGameController.Object); // RestartGame returns mocked game controller

            // Set up game controller factory to return mock game controller
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "Cannot load house layout because file name \"\\ //\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)" + Environment.NewLine +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "New game started" + Environment.NewLine +
                    "Welcome to new house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[new gc status]" + Environment.NewLine +
                    "[new gc prompt]"));

                // Verify method called once to restart game with specific House file name
                mockNewGameController.Verify((gc) => gc.RestartGame("NewHouse"), Times.Once);
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Name Failure Quit")]
        public void Test_ConsoleRunner_NewGame_House_NonexistentFile_ThenQuit()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("NonexistentHouse") // Name of nonexistent House file
                .Returns("exit"); // Exit game

            // Create new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = new Mock<IGameController>();

            // Set up game controller factory to return mock game controller object
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "Cannot load house layout because no house file with name \"NonexistentHouse\" exists" + Environment.NewLine +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: "));

                // Verify no calls to mock new game controller
                mockNewGameController.VerifyNoOtherCalls();
            });
        }

        [Test]
        [Category("ConsoleRunner NewGame Opponents Default House Name Failure Success Quit")]
        public void Test_ConsoleRunner_NewGame_House_NonexistentFile_ThenValid()
        {
            // Set up mock to return user input
            mockConsoleIO.SetupSequence((cio) => cio.ReadLine())
                .Returns("new") // New game
                .Returns("") // Default opponents
                .Returns("NonexistentHouse") // Name of nonexistent House file
                .Returns("NewHouse") // Name of existing House file
                .Returns("exit"); // Exit game

            // Create and set up new mock game controller for factory to return
            Mock<IGameController> mockNewGameController = GetGameControllerMockForNewGameController();
            mockNewGameController.Setup((gc) => gc.RestartGame("NewHouse")).Returns(mockNewGameController.Object); // RestartGame returns mocked game controller

            // Set up game controller factory to return mock game controller
            mockGameControllerFactory.Setup((factory) => factory.GetDefaultGameController()).Returns(mockNewGameController.Object);

            // Create console runner with mocked GameController, IConsoleIO, IGetFileNamesAdapter, and IGameControllerFactory
            consoleRunner = new HideAndSeekConsoleRunner(mockInitialGameController.Object, mockConsoleIO.Object,
                                                         GetFileNamesAdapterReturning2HouseFileNames(), mockGameControllerFactory.Object);

            // Run game
            consoleRunner.RunGame();

            Assert.Multiple(() =>
            {
                // Assert text displayed is as expected
                Assert.That(sbActualTextDisplayed.ToString(), Does.EndWith(
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "Cannot load house layout because no house file with name \"NonexistentHouse\" exists" + Environment.NewLine +
                    "Here are the names of the house layout files available:" + Environment.NewLine +
                    " - DefaultHouse" + Environment.NewLine +
                    " - NewHouse" + Environment.NewLine +
                    "Type a house layout file name or just press Enter to use the default house layout: " +
                    "New game started" + Environment.NewLine +
                    "Welcome to new house!" + Environment.NewLine +
                    Environment.NewLine +
                    "[new gc status]" + Environment.NewLine +
                    "[new gc prompt]"));

                // Verify expected method with expected parameter called
                mockNewGameController.Verify((gc) => gc.RestartGame("NewHouse"), Times.Once); // Method called once to restart game with name of existing file
                mockNewGameController.Verify((gc) => gc.RestartGame("NonexistentHouse"), Times.Never); // Method never called to restart game with name of nonexistent file
            });
        }
    }
}