using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeekTestProject
{
    /// <summary>
    /// TestCaseData for some House tests for creating House objects with CreateHouse method
    /// </summary>
    public static class HouseTests_TestCaseData
    {
        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt
        {
            get
            {
                // Empty file
                yield return new TestCaseData("")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - empty file");

                // File with only whitespace
                yield return new TestCaseData(" ")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("ABCDeaoueou[{}}({}")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - random characters");
            }
        }

    }
}
