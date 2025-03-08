using HideAndSeek;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeekTestProject
{
    /// <summary>
    /// Class to test FileExtensions methods for getting full file name for JSON and validating file name
    /// </summary>
    [TestFixture]
    public class FileExtensionsTests
    {
        IFileSystem fileSystem;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fileSystem = new FileSystem(); // Only static methods being tested so only need to create once
        }

        [Test]
        [Category("FileExtensions GetFullFileNameForJson")]
        public void Test_FileExtensions_GetFullFileNameForJson()
        {
            Assert.That(fileSystem.GetFullFileNameForJson("myFile"), Is.EqualTo("myFile.json"));
        }

        [TestCase("myFile")]
        [TestCase("123")]
        [Category("FileExtensions IsNameValid Success")]
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
        [Category("FileExtensions IsNameValid Failure")]
        public void Test_FileExtensions_IsValidName_ReturnsFalse(string fileName)
        {
            Assert.That(fileSystem.IsValidName(fileName), Is.False);
        }
    }
}
