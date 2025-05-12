namespace HideAndSeek
{
    /// <summary>
    /// FileExtensions tests for methods for getting full file name for JSON file and validating file name
    /// </summary>
    [TestFixture]
    public class TestFileExtensions
    {
        [TestCaseSource(typeof(TestFileExtensions_TestData), 
            nameof(TestFileExtensions_TestData.TestCases_For_Test_FileExtensions_GetFullFileNameForJson_WithValidFileName))]
        public void Test_FileExtensions_GetFullFileNameForJson_WithValidFileName(Func<string> GetFullFileNameForJson)
        {
            Assert.That(GetFullFileNameForJson(), Is.EqualTo("myFile.json"));
        }

        [TestCaseSource(typeof(TestFileExtensions_TestData), 
            nameof(TestFileExtensions_TestData.TestCases_For_Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName))]
        public void Test_FileExtensions_GetFullFileNameForJson_AndCheckErrorMessage_ForInvalidFileName(
            string fileName, Action<string> GetFullFileNameForJson)
        {
            Assert.Multiple(() =>
            {
                // Assert that getting full file name with invalid file name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    GetFullFileNameForJson(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestFileExtensions_TestData), 
            nameof(TestFileExtensions_TestData.TestCases_For_Test_FileExtensions_IsValidName_ReturnsTrue))]
        public void Test_FileExtensions_IsValidName_ReturnsTrue(string fileName, Func<string, bool> IsValidName)
        {
            Assert.That(IsValidName(fileName), Is.True);
        }

        [TestCaseSource(typeof(TestFileExtensions_TestData),
            nameof(TestFileExtensions_TestData.TestCases_For_Test_FileExtensions_IsValidName_ReturnsFalse))]
        public void Test_FileExtensions_IsValidName_ReturnsFalse(string fileName, Func<string, bool> IsValidName)
        {
            Assert.That(IsValidName(fileName), Is.False);
        }
    }
}