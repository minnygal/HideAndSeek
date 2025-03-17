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
                // NO PROPERTIES
                // Empty file
                yield return new TestCaseData("", "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - empty file");

                // File with only whitespace
                yield return new TestCaseData(" ", "The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 1.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("ABCDeaoueou[{}}({}", "'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - random characters");

                // MISSING PROPERTIES
                // File missing Name
                yield return new TestCaseData("{" +
                            TestHouse_Data.SerializedHouse_HouseFileName + "," +
                            TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                            TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                            TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                        "}",
                        "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: Name")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: HouseFileName")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: PlayerStartingPoint")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithoutHidingPlaces")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithHidingPlaces")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue
        {
            get
            {
                // Invalid Name - whitespace
                yield return new TestCaseData("{" +
                           "\"Name\":\" \"" + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because house name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid Name - whitespace");

                // Invalid HouseFileName - whitespace
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because house file name \" \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint - whitespace
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because player starting point location name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\" \"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"West\":\"Entry\"," +
                                        "\"Northwest\":\"Kitchen\"," +
                                        "\"North\":\"Bathroom\"," +
                                        "\"South\":\"Living Room\"," +
                                        "\"Up\":\"Landing\"" +
                                    "}" +
                                "}" +
                           "]" + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because location name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           "\"LocationsWithHidingPlaces\":" +
                            "[" +
                                "{" +
                                    "\"HidingPlace\":\"in a trunk\"," +
                                    "\"Name\":\" \"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Landing\"" +
                                    "}" +
                                "}" +
                            "]" +
                       "}",
                       "Cannot perform action because location name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace description is invalid (whitespace)
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           "\"LocationsWithHidingPlaces\":" +
                            "[" +
                                "{" +
                                    "\"HidingPlace\":\" \"," +
                                    "\"Name\":\"Attic\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Landing\"" +
                                    "}" +
                                "}" +
                            "]" +
                       "}",
                       "Cannot perform action because hiding place \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location HidingPlace - whitespace");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue
        {
            get
            {
                // Invalid PlayerStartingPoint - not a Location
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\"Dungeon\"" + "," +
                           TestHouse_Data.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because player starting point location \"Dungeon\" is not a location in the house")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid PlayerStartingPoint - nonexistent");

                // Invalid LocationsWithoutHidingPlaces - Location has no exits
                yield return new TestCaseData("{" +
                           TestHouse_Data.SerializedTestHouse_Name + "," +
                           TestHouse_Data.SerializedHouse_HouseFileName + "," +
                           TestHouse_Data.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" + // Entry required for setting StartingLocation
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Out\":\"Garage\"," +
                                        "\"East\":\"Hallway\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Hallway\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                    "}" +
                                "}" +
                           "]" + "," +
                           TestHouse_Data.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because location \"Hallway\" must be assigned at least one exit")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");
            }
        }
    }
}