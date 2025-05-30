﻿using System.Collections;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for SavedGame tests for static file methods
    /// </summary>
    public static class TestSavedGame_StaticFileMethods_TestData
    {
        public static IEnumerable TestCases_For_Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames())
                    .SetName("Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile - directory name");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames())
                    .SetName("Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles - directory name");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles
        {
            get
            {
                // No directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames())
                    .SetName("Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles - no argument");

                // Directory path passed in
                yield return new TestCaseData(() => SavedGame.GetSavedGameFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                    .SetName("Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles - directory name");
            }
        }
    }
}