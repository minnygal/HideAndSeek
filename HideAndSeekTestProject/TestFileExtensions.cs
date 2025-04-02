using HideAndSeek;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// FileExtensions tests for methods for getting full file name for JSON and validating file name
    /// </summary>
    [TestFixture]
    public class TestFileExtensions
    {
        IFileSystem fileSystem;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fileSystem = new FileSystem(); // Only static methods being tested so only need to create once
        }

        [Test]
        [Category("FileExtensions GetFullFileNameForJson Success")]
        public void Test_FileExtensions_GetFullFileNameForJson_WithValidFileName()
        {
            Assert.That(fileSystem.GetFullFileNameForJson("myFile"), Is.EqualTo("myFile.json"));
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
        [Category("FileExtensions GetFullFileNameForJson Failure")]
        public void Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that getting full file name with invalid file name raises exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    fileSystem.GetFullFileNameForJson(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCase("myFile")]
        [TestCase("123")]
        [Category("FileExtensions IsValidName Success")]
        public void Test_FileExtensions_IsValidName_ReturnsTrue(string fileName)
        {
            Assert.That(fileSystem.IsValidName(fileName), Is.True);
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
        [Category("FileExtensions IsValidName Failure")]
        public void Test_FileExtensions_IsValidName_ReturnsFalse(string fileName)
        {
            Assert.That(fileSystem.IsValidName(fileName), Is.False);
        }
    }
}
