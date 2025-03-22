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
    /// TestCaseData for some House tests for creating House objects with CreateHouse method
    /// </summary>
    public static class TestHouse_TestCaseData
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
                            MyTestHouse.SerializedHouse_HouseFileName + "," +
                            MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                            MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                            MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                        "}",
                        "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: Name")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: HouseFileName")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: PlayerStartingPoint")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithoutHidingPlaces")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces +
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
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because house name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid Name - whitespace");

                // Invalid HouseFileName - whitespace
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because house file name \" \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint - whitespace
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because player starting point location name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
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
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because location name \" \" is invalid (is empty or contains only whitespace)")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasWhitespaceValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
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
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
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

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileDataHasInvalidDirection
        {
            get
            {
                // Invalid LocationsWithoutHidingPlaces - exit Direction is invalid
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\"Hallway\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"West\":\"Entry\"," +
                                        "\"Up\":\"Attic\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Insideout\":\"Hallway\"" +
                                    "}" +
                                "}" +
                            "]" + "," +
                           "\"LocationsWithHidingPlaces\":" +
                           "[" + // Must have at least one location with hiding place
                                "{" +
                                    "\"HidingPlace\":\"in a trunk\"," +
                                    "\"Name\":\"Attic\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Hallway\"" +
                                    "}" +
                                "}" +
                           "]" +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileDataHasInvalidDirection - LocationsWithoutHidingPlaces");

                // Invalid LocationsWithHidingPlaces - exit direction is invalid
                yield return new TestCaseData("{" +
                            MyTestHouse.SerializedTestHouse_Name + "," +
                            MyTestHouse.SerializedHouse_HouseFileName + "," +
                            "\"PlayerStartingPoint\":\"Master Bedroom\"" + "," +
                            "\"LocationsWithoutHidingPlaces\":" +
                            "[" +
                            "]" + "," +
                            "\"LocationsWithHidingPlaces\":" +
                            "[" +
                                "{" +
                                    "\"HidingPlace\":\"under the bed\"," +
                                    "\"Name\":\"Master Bedroom\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"East\":\"Master Bath\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"in the tub\"," +
                                    "\"Name\":\"Master Bath\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Insideout\":\"Master Bedroom\"" +
                                    "}" +
                                "}" +
                            "]" +
                        "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForJsonException_WhenFileDataHasInvalidDirection - LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue
        {
            get
            {
                // Invalid PlayerStartingPoint - not a Location
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\"Dungeon\"" + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because player starting point location \"Dungeon\" is not a location in the house")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid PlayerStartingPoint - nonexistent");

                // Invalid LocationsWithHidingPlaces - empty list
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\"Hallway\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"West\":\"Entry\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"East\":\"Hallway\"" +
                                    "}" +
                                "}" +
                           "]" + "," +
                           "\"LocationsWithHidingPlaces\":" +
                           "[" +
                           "]" +
                       "}",
                       "Cannot process because data in house layout file MyInvalidDataFile is invalid - Cannot perform action because locations with hiding places list is empty")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - empty");

                // Invalid LocationsWithoutHidingPlaces - Location has no exits
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\"Hallway\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Landing\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Hallway\"," +
                                        "\"Up\":\"Attic\"," +
                                        "\"Southeast\":\"Kids Room\"," +
                                        "\"Northwest\":\"Master Bedroom\"," +
                                        "\"Southwest\":\"Nursery\"," +
                                        "\"South\":\"Pantry\"," +
                                        "\"West\":\"Second Bathroom\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Out\":\"Garage\"," +
                                        "\"East\":\"Hallway\"" +
                                    "}" +
                                "}" +
                           "]" + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because location \"Hallway\" must be assigned at least one exit")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has no exits
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," + 
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           "\"LocationsWithHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"HidingPlace\":\"in a trunk\"," +
                                    "\"Name\":\"Attic\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                    "}" +
                                "}" +
                           "]" +
                       "}",
                       "Cannot perform action because location \"Attic\" must be assigned at least one exit")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - no exits");

                // Invalid LocationsWithoutHidingPlaces - Location has nonexistent exit
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\"Hallway\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Dungeon\"," +
                                        "\"West\":\"Entry\"," +
                                        "\"Northwest\":\"Kitchen\"," +
                                        "\"North\":\"Bathroom\"," +
                                        "\"South\":\"Living Room\"," +
                                        "\"Up\":\"Landing\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Landing\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Hallway\"," +
                                        "\"Up\":\"Attic\"," +
                                        "\"Southeast\":\"Kids Room\"," +
                                        "\"Northwest\":\"Master Bedroom\"," +
                                        "\"Southwest\":\"Nursery\"," +
                                        "\"South\":\"Pantry\"," +
                                        "\"West\":\"Second Bathroom\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Out\":\"Garage\"," +
                                        "\"East\":\"Hallway\"" +
                                    "}" +
                                "}" +
                           "]" + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}",
                       "Cannot perform action because \"Hallway\" exit location \"Dungeon\" in direction \"Down\" does not exist")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithoutHidingPlaces - nonexistent exit");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has nonexistent exit
                yield return new TestCaseData("{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           "\"LocationsWithHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"HidingPlace\":\"in a trunk\"," +
                                    "\"Name\":\"Attic\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Down\":\"Landing\"" +
                                    "}" +
                                "}," +
                                "{\"HidingPlace\":\"behind the door\"," +
                                    "\"Name\":\"Bathroom\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"South\":\"Hallway\"," +
                                        "\"Down\":\"Dungeon\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"in the bunk beds\"," +
                                    "\"Name\":\"Kids Room\"," +
                                    "\"ExitsForSerialization\":" +
                                        "{" +
                                            "\"Northwest\":\"Landing\"" +
                                        "}" +
                                    "}," +
                                "{" +
                                    "\"HidingPlace\":\"under the bed\"," +
                                    "\"Name\":\"Master Bedroom\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Southeast\":\"Landing\"," +
                                        "\"East\":\"Master Bath\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"behind the changing table\"," +
                                    "\"Name\":\"Nursery\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Northeast\":\"Landing\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"inside a cabinet\"," +
                                    "\"Name\":\"Pantry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"North\":\"Landing\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"in the shower\"," +
                                    "\"Name\":\"Second Bathroom\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"East\":\"Landing\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"next to the stove\"," +
                                    "\"Name\":\"Kitchen\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Southeast\":\"Hallway\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"in the tub\"," +
                                    "\"Name\":\"Master Bath\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"West\":\"Master Bedroom\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"behind the car\"," +
                                    "\"Name\":\"Garage\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"In\":\"Entry\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"HidingPlace\":\"behind the sofa\"," +
                                    "\"Name\":\"Living Room\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"North\":\"Hallway\"" +
                                    "}" +
                                "}" +
                           "]" +
                       "}",
                       "Cannot perform action because \"Bathroom\" exit location \"Dungeon\" in direction \"Down\" does not exist")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - nonexistent exit");
            }
        }
    }
}