using System.Collections;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for House tests for file-related methods
    /// </summary>
    public static class TestHouse_FileMethods_TestData
    {
        public static IEnumerable TestCases_For_Test_House_GetHouseFileNames_SingleHouseFile
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames())
                    .SetName("Test_House_GetHouseFileNames_SingleHouseFile - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_House_GetHouseFileNames_SingleHouseFile - directory name");
            }
        }

        public static IEnumerable TestCases_For_Test_House_GetHouseFileNames_MultipleHouseFiles
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames())
                    .SetName("Test_House_GetHouseFileNames_MultipleHouseFiles - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_House_GetHouseFileNames_MultipleHouseFiles - directory name");
            }
        }

        public static IEnumerable TestCases_For_Test_House_GetHouseFileNames_NoHouseFiles
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames())
                    .SetName("Test_House_GetHouseFileNames_NoHouseFiles - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => House.GetHouseFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_House_GetHouseFileNames_NoHouseFiles - directory name");
            }
        }
    }
}
