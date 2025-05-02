using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for FileExtensions tests for methods for getting full file name for JSON file and validating file name
    /// </summary>
    public static class TestFileExtensions_TestData
    {
        private static readonly IFileSystem fileSystem = new FileSystem();

        public static IEnumerable TestCases_For_Test_FileExtensions_GetFullFileNameForJson_WithValidFileName
        {
            get
            {
                // Using extension method
                yield return new TestCaseData(() => fileSystem.GetFullFileNameForJson("myFile"))
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_WithValidFileName - extension method")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod Success");

                // Using class method
                yield return new TestCaseData(() => FileExtensions.GetFullFileNameForJson("myFile"))
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_WithValidFileName - class method")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod Success");
            }
        }

        public static IEnumerable TestCases_For_Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName
        {
            get
            {
                // Using extension method
                yield return new TestCaseData("",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - empty")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData(" ",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - whitespace")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData(" myFile",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - space at beginning")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData("my file",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - space in middle")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData("myFile ",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - space at end")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData("\\my\\File\\",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - backslashes")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");

                yield return new TestCaseData("//my//File//",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - extension method - forwardslashes")
                    .SetCategory("FileExtensions GetFullFileNameForJson ExtensionMethod ArgumentException Failure");


                // Using class method
                yield return new TestCaseData("",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - empty")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData(" ",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - whitespace")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData(" myFile",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - space at beginning")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData("my file",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - space in middle")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData("myFile ",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - space at end")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData("\\my\\File\\",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - backslashes")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");

                yield return new TestCaseData("/my/File/",
                    (string fileName) =>
                    {
                        fileSystem.GetFullFileNameForJson(fileName);
                    })
                    .SetName("Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName - class method - forwardslashes")
                    .SetCategory("FileExtensions GetFullFileNameForJson ClassMethod ArgumentException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_FileExtensions_IsValidName_ReturnsTrue
        {
            get
            {
                // Using extension method
                yield return new TestCaseData("myValidFile",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - extension method - alphabetical")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod True");

                yield return new TestCaseData("12314",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - extension method - numerical")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod True");

                yield return new TestCaseData("file1Of2",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - extension method - alphanumeric")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod True");

                // Using class method
                yield return new TestCaseData("myValidFile",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - class method - alphabetical")
                    .SetCategory("FileExtensions IsValidName ClassMethod True");

                yield return new TestCaseData("12314",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - class method - numeric")
                    .SetCategory("FileExtensions IsValidName ClassMethod True");

                yield return new TestCaseData("file1Of2",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsTrue - class method - alphanumeric")
                    .SetCategory("FileExtensions IsValidName ClassMethod True");
            }
        }

        public static IEnumerable TestCases_For_Test_FileExtensions_IsValidName_ReturnsFalse
        {
            get
            {
                // Using extension method
                yield return new TestCaseData("",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - empty")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData(" ",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - whitespace")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData(" myFile",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - space at beginning")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData("my file",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - space in middle")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData("myFile ",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - space at end")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData("\\my\\File\\",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - backslashes")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                yield return new TestCaseData("/my/File/",
                    (string fileName) => fileSystem.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - extension method - forwardslashes")
                    .SetCategory("FileExtensions IsValidName ExtensionMethod False");

                // Using class method
                yield return new TestCaseData("",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - empty")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData(" ",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - whitespace")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData(" myFile",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - space at beginning")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData("my file",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - space in middle")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData("myFile ",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - space at end")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData("\\my\\File\\",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - backslashes")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");

                yield return new TestCaseData("/my/File/",
                    (string fileName) => FileExtensions.IsValidName(fileName))
                    .SetName("Test_FileExtensions_IsValidName_ReturnsFalse - class method - forwardslashes")
                    .SetCategory("FileExtensions IsValidName ClassMethod False");
            }
        }
    }
}