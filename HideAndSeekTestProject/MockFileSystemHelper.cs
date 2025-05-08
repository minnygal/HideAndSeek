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
    /// Helper methods for tests requiring mock IFileSystem
    /// </summary>
    public static class MockFileSystemHelper
    {
        /// <summary>
        /// Helper method to create and return a mocked file system set up to return the specified text
        /// when File.ReadAllText called with specified file name
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="textInFile">Text to return when File.ReadAllText called</param>
        /// <returns>File system object</returns>
        public static IFileSystem GetMockedFileSystem_ToReadAllText(string fileName, string textInFile)
        {
            return GetMockOfFileSystem_ToReadAllText(fileName, textInFile).Object;
        }

        /// <summary>
        /// Helper method to create and return a mock file system set up to return the specified text
        /// when File.ReadAllText called with specified file name
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="textInFile">Text to return when File.ReadAllText called</param>
        /// <returns>Mock object of file system</returns>
        public static Mock<IFileSystem> GetMockOfFileSystem_ToReadAllText(string fileName, string textInFile)
        {
            return SetMockOfFileSystem_ToReadAllText(new Mock<IFileSystem>(), fileName, textInFile);
        }

        /// <summary>
        /// Helper method to set up provided mock file system to return the specified text
        /// when File.ReadAllText called with specified file name
        /// </summary>
        /// <param name="fileSystemMock">Mock file system to set up</param>
        /// <param name="fileName">Name of file</param>
        /// <param name="textInFile">Text to return when File.ReadAllText called</param>
        /// <returns>Mock object of file system</returns>
        public static Mock<IFileSystem> SetMockOfFileSystem_ToReadAllText(
            Mock<IFileSystem> fileSystemMock, string fileName, string textInFile)
        {
            fileSystemMock.Setup((s) => s.File.Exists(fileName)).Returns(true); // Mock that file exists
            fileSystemMock.Setup((s) => s.File.ReadAllText(fileName)).Returns(textInFile); // Mock text read from file
            return fileSystemMock; // Return mock file system
        }
    }
}
