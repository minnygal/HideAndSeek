using HideAndSeek;
using Moq;

namespace HideAndSeekConsoleTestProject
{
    /// <summary>
    /// Test data for HideAndSeekConsoleRunner tests for command to load game
    /// </summary>
    public static class TestHideAndSeekConsoleRunner_LoadGame_TestData
    {
        public static IEnumerable<TestCaseData> TestCases_For_Test_ConsoleRunner_Load
        {
            get
            {
                // Inline file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load myFile") // Load game
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                    "") // No text before success message
                    .SetName("Test_ConsoleRunner_Load - inline file name selection");

                // Separate file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load") // Start load
                            .Returns("myFile") // Specify file
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                        // Text before success message
                        "Here are the names of the saved game files available:" + Environment.NewLine +
                        " - myFile" + Environment.NewLine +
                        " - otherFile" + Environment.NewLine +
                        "Enter the name of the saved game file to load: ")
                    .SetName("Test_ConsoleRunner_Load - separate file name selection");
            }
        }

        public static IEnumerable<TestCaseData> TestCases_For_Test_ConsoleRunner_Load_WithInvalidFileName
        {
            get
            {
                // Inline file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load \\/ invalid") // Load game
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock

                    },
                    "") // No text before error message
                    .SetName("Test_ConsoleRunner_Load_WithInvalidFileName - inline file name selection");

                // Separate file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load") // Start load
                            .Returns("\\/ invalid") // Specify file
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                        // Text before error message
                        "Here are the names of the saved game files available:" + Environment.NewLine +
                        " - myFile" + Environment.NewLine +
                        " - otherFile" + Environment.NewLine +
                        "Enter the name of the saved game file to load: ")
                    .SetName("Test_ConsoleRunner_Load_WithInvalidFileName - separate file name selection");
            }
        }

        public static IEnumerable<TestCaseData> TestCases_For_Test_ConsoleRunner_Load_WithNonexistentFileName
        {
            get
            {
                // Inline file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load nonexistentFile") // Load game
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                    "") // No text before error message
                    .SetName("Test_ConsoleRunner_Load_WithNonexistentFileName - inline file name selection");

                // Separate file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load") // Start load
                            .Returns("nonexistentFile") // Specify file
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                        // Text before error message
                        "Here are the names of the saved game files available:" + Environment.NewLine +
                        " - myFile" + Environment.NewLine +
                        " - otherFile" + Environment.NewLine +
                        "Enter the name of the saved game file to load: ")
                    .SetName("Test_ConsoleRunner_Load_WithNonexistentFileName - separate file name selection");
            }
        }

        public static IEnumerable<TestCaseData> TestCases_For_Test_ConsoleRunner_Load_ExceptionThrownByGameController
        {
            get
            {
                // Inline file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load myFile") // Start load
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                    "") // No text before error message
                    .SetName("Test_ConsoleRunner_Load_ExceptionThrownByGameController - inline file name selection");

                // Separate file name selection
                yield return new TestCaseData(
                    (Mock<IConsoleIO> mockConsoleIO) =>
                    {
                        mockConsoleIO.SetupSequence((cio) => cio.ReadLine()) // Set up user input
                            .Returns("load") // Start load
                            .Returns("myFile") // Specify file
                            .Returns("exit"); // Exit game
                        return mockConsoleIO; // Return console mock
                    },
                        // Text before error message
                        "Here are the names of the saved game files available:" + Environment.NewLine +
                        " - myFile" + Environment.NewLine +
                        " - otherFile" + Environment.NewLine +
                        "Enter the name of the saved game file to load: ")
                    .SetName("Test_ConsoleRunner_Load_ExceptionThrownByGameController - separate file name selection");
            }
        }
    }
}