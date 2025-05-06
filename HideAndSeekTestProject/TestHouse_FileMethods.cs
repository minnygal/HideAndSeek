using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for static House file-related methods
    /// 
    /// These are unit tests
    /// </summary>
    [TestFixture]
    public class TestHouse_FileMethods
    {
        [SetUp]
        public void SetUp()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("House GetFullHouseFileName Success")]
        public void Test_House_GetFullHouseFileName()
        {
            Assert.That(House.GetFullHouseFileName("my_house"), Is.EqualTo("my_house.house.json"));
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
        [Category("House GetFullHouseFileName ArgumentException Failure")]
        public void Test_House_GetFullHouseFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that getting full house file name with invalid file name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    House.GetFullHouseFileName(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" is invalid " +
                                                               "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestHouse_FileMethods_TestData), nameof(TestHouse_FileMethods_TestData.TestCases_For_Test_House_GetHouseFileNames_SingleHouseFile))]
        [Category("House GetHouseFileNames Success")]
        public void Test_House_GetHouseFileNames_SingleHouseFile(Func<IEnumerable<string>> GetHouseFileNames)
        {
            SetHouseFileSystemForGetHouseFileNamesTest(
                new string[] { "AGame.game.json", "DefaultHouse.house.json", "NotAHouse.json", "HideAndSeekClassLibrary.dll",
                               "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json", "HideAndSeekConsole.dll",
                               "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb", "HideAndSeekConsole.runtimeconfig.json",
                               "OtherGame.game.json", "TestableIO.System.IO.Abstractions.dll",
                               "TestableIO.System.IO.Abstractions.Wrappers.dll"
                              });
            Assert.That(GetHouseFileNames(), Is.EquivalentTo(new List<string>() { "DefaultHouse" }));
        }

        [TestCaseSource(typeof(TestHouse_FileMethods_TestData), nameof(TestHouse_FileMethods_TestData.TestCases_For_Test_House_GetHouseFileNames_MultipleHouseFiles))]
        [Category("House GetHouseFileNames Success")]
        public void Test_House_GetHouseFileNames_MultipleHouseFiles(Func<IEnumerable<string>> GetHouseFileNames)
        {
            SetHouseFileSystemForGetHouseFileNamesTest(
                new string[] { "1CoolHouse$$.house.json", "AGame.game.json", "DefaultHouse.house.json", "NotAHouse.json",
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json",
                               "HideAndSeekConsole.dll", "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb",
                               "HideAndSeekConsole.runtimeconfig.json", "OtherGame.game.json", "SecretMansion.house.json",
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll",
                               "TestHouse.house.json"
                             });
            Assert.That(GetHouseFileNames(), Is.EquivalentTo(new List<string>() { "1CoolHouse$$", "DefaultHouse", "SecretMansion", "TestHouse" }));
        }

        [TestCaseSource(typeof(TestHouse_FileMethods_TestData), nameof(TestHouse_FileMethods_TestData.TestCases_For_Test_House_GetHouseFileNames_NoHouseFiles))]
        [Category("House GetHouseFileNames Success")]
        public void Test_House_GetHouseFileNames_NoHouseFiles(Func<IEnumerable<string>> GetHouseFileNames)
        {
            SetHouseFileSystemForGetHouseFileNamesTest(
                new string[] { "AGame.game.json", "NotAHouse.json",
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json",
                               "HideAndSeekConsole.dll", "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb",
                               "HideAndSeekConsole.runtimeconfig.json", "OtherGame.game.json",
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll"
                             });
            Assert.That(GetHouseFileNames(), Is.Empty);
        }

        [Test]
        [Category("House GetHouseFileNames Failure")]
        public void Test_House_GetHouseFileNames_AndCheckErrorMessage_ForInvalidDirectoryName()
        {
            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<DirectoryNotFoundException>(() => House.GetHouseFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"));
                Assert.That(exception.Message, Is.EqualTo("Could not find a part of the path 'C:\\Users\\Tester\\Desktop\\HideAndSeekConsole'."));
            });
        }

        private static void SetHouseFileSystemForGetHouseFileNamesTest(string[] fileNames)
        {
            // Set up mock file system
            Mock<IFileSystem> mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup((d) => d.Directory.GetFiles("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                          .Returns(fileNames); // Set up mock to return files
            mockFileSystem.Setup((d) => d.Path.GetDirectoryName(It.IsAny<string>())).Returns("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"); // Mock default directory name assigned if no argument passed in
            House.FileSystem = mockFileSystem.Object; // Set House file system
        }
    }
}