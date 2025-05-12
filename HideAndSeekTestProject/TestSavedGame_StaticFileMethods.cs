using Moq;
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// SavedGame unit tests for static file methods
    /// </summary>
    [TestFixture]
    public class TestSavedGame_StaticFileMethods
    {
        [Test]
        [Category("SavedGame GetFullSavedGameFileName Success")]
        public void Test_SavedGame_GetFullSavedGameFileName()
        {
            Assert.That(SavedGame.GetFullSavedGameFileName("my_saved_game"), Is.EqualTo("my_saved_game.game.json"));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("my file")]
        [TestCase(" myFile")]
        [TestCase("myFile ")]
        [TestCase("\\")]
        [TestCase("\\myFile")]
        [TestCase("myFile\\")]
        [TestCase("my\\File")]
        [TestCase("/")]
        [TestCase("/myFile")]
        [TestCase("myFile/")]
        [TestCase("my/File")]
        [Category("SavedGame GetFullSavedGameFileName ArgumentException Failure")]
        public void Test_SavedGame_GetFullSavedGameFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that getting full saved game file name with invalid file name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    SavedGame.GetFullSavedGameFileName(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" is invalid " +
                                                               "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_StaticFileMethods_TestData),
            nameof(TestSavedGame_StaticFileMethods_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "DefaultHouse.house.json", "NotASavedGame.json", "HideAndSeekClassLibrary.dll",
                               "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json", "HideAndSeekConsole.dll",
                               "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb", "HideAndSeekConsole.runtimeconfig.json",
                               "MyGame.game.json", "OtherHouse.house.json", "TestableIO.System.IO.Abstractions.dll",
                               "TestableIO.System.IO.Abstractions.Wrappers.dll"
                              });
            Assert.That(GetSavedGameFileNames(), Is.EquivalentTo(new List<string>() { "MyGame" }));
        }

        [TestCaseSource(typeof(TestSavedGame_StaticFileMethods_TestData),
            nameof(TestSavedGame_StaticFileMethods_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "1G@m3.game.json", "AGame.game.json", "DefaultHouse.house.json", "NotASavedGame.json",
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json",
                               "HideAndSeekConsole.dll", "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb",
                               "HideAndSeekConsole.runtimeconfig.json", "MyGame.game.json", "OtherHouse.house.json",
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll",
                               "Winning.game.json"
                             });
            Assert.That(GetSavedGameFileNames(), Is.EquivalentTo(new List<string>() { "1G@m3", "AGame", "MyGame", "Winning" }));
        }

        [TestCaseSource(typeof(TestSavedGame_StaticFileMethods_TestData),
            nameof(TestSavedGame_StaticFileMethods_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "DefaultHouse.house.json", "FriendHouse.house.json", "NotASavedGame.json",
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb",
                               "HideAndSeekConsole.deps.json", "HideAndSeekConsole.dll", "HideAndSeekConsole.exe",
                               "HideAndSeekConsole.pdb", "HideAndSeekConsole.runtimeconfig.json",
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll"
                             });
            Assert.That(GetSavedGameFileNames(), Is.Empty);
        }

        [Test]
        [Category("SavedGame GetSavedGameFileNames Failure")]
        public void Test_SavedGame_GetSavedGameFileNames_AndCheckErrorMessage_ForInvalidDirectoryName()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling method with name of nonexistent directory raises exception
                Exception exception = Assert.Throws<DirectoryNotFoundException>(() =>
                {
                    SavedGame.GetSavedGameFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Could not find a part of the path 'C:\\Users\\Tester\\Desktop\\HideAndSeekConsole'."));
            });
        }

        /// <summary>
        /// Helper method to set SavedGame file system to return specific file names
        /// </summary>
        /// <param name="fileNames">File names to be returned</param>
        private static void SetSavedGameFileSystemForGetSavedGameFileNamesTest(string[] fileNames)
        {
            Mock<IFileSystem> mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup((d) => d.Directory.GetFiles("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                          .Returns(fileNames); // Set up mock to return files
            mockFileSystem.Setup((d) => d.Path.GetDirectoryName(It.IsAny<string>()))
                          .Returns("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"); // Mock default directory name assigned
            SavedGame.FileSystem = mockFileSystem.Object; // Set SavedGame file system
        }
    }
}