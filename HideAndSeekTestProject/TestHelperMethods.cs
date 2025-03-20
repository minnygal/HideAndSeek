using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// Helper methods for tests
    /// </summary>
    public static class TestHelperMethods
    {
        /// <summary>
        /// Helper method to create and return a mock file system
        /// set up to return the specified text
        /// when File.ReadAllText called with specified file name
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="textInFile">Text to return when File.ReadAllText called</param>
        /// <returns></returns>
        public static IFileSystem CreateMockFileSystem_ToReadAllText(string fileName, string textInFile)
        {
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>(); // Create mock file system
            fileSystemMock.Setup((s) => s.File.Exists(fileName)).Returns(true); // Mock that file exists
            fileSystemMock.Setup((s) => s.File.ReadAllText(fileName)).Returns(textInFile); // Mock text read from file
            return fileSystemMock.Object; // Return file system
        }
    }
}
