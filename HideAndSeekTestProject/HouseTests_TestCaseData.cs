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
        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid
        {
            get
            {
                // Empty file
                yield return new TestCaseData("", "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - empty file");

                // File with only whitespace
                yield return new TestCaseData(" ", "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 1.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("ABCDeaoueou[{}}({}", "'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileIsCorrupt - random characters");
            }
        }

    }
}
