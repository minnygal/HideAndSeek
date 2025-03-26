using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// TestCaseData for some SavedGame tests for deserialization
    /// </summary>
    public static class TestSavedGame_Deserialization_TestCaseData
    {
        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens
        {
            get
            {
                // No data in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 2.",
                        "  ")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens - just characters in file");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing
        {
            get
            {
                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: PlayerLocation",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: MoveNumber",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: OpponentsAndHidingLocations",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: FoundOpponents",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing - missing found opponents");

            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue
        {
            get
            {
                // Invalid House file name
                yield return new TestCaseData("Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                            "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid HouseFileName");

                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid CurrentLocation");

                // Invalid (negative) move number
                yield return new TestCaseData("invalid MoveNumber",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            "\"MoveNumber\":-1" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid MoveNumber");

                // No opponents
                yield return new TestCaseData("no opponents",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            "\"OpponentsAndHidingLocations\":{}" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - no opponents");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("invalid hiding location for opponent",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Tree\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("invalid hiding location for opponent",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Hallway\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue - found opponent Steve is not opponent");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid
        {
            get
            {
                // NO PROPERTIES
                // Empty file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - empty file");

                // File with only whitespace
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 1.",
                        " ")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - random characters");

                // MISSING PROPERTIES
                // File missing Name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: Name",
                        "{" +
                            MyTestHouse.SerializedHouse_HouseFileName + "," +
                            MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                            MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                            MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: HouseFileName",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: PlayerStartingPoint",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithoutHidingPlaces",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithHidingPlaces",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid - no LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue
        {
            get
            {
                // INVALID VALUE OF WHITESPACE
                // Invalid Name - whitespace
                yield return new TestCaseData("Cannot perform action because house name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\" \"" + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid Name - whitespace");

                // Invalid HouseFileName - whitespace
                yield return new TestCaseData("Cannot perform action because house file name \" \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           MyTestHouse.SerializedHouse_PlayerStartingPoint + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint - whitespace
                yield return new TestCaseData("Cannot perform action because player starting point location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("Cannot perform action because location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("Cannot perform action because location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - LocationsWithHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace description is invalid (whitespace)
                yield return new TestCaseData("Cannot perform action because hiding place \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - LocationsWithHidingPlaces - invalid Location HidingPlace - whitespace");

                // OTHER INVALID VALUES
                // Invalid PlayerStartingPoint - not a Location
                yield return new TestCaseData("Cannot perform action because player starting point location \"Dungeon\" is not a location in the house",
                        "{" +
                           MyTestHouse.SerializedTestHouse_Name + "," +
                           MyTestHouse.SerializedHouse_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\"Dungeon\"" + "," +
                           MyTestHouse.SerializedHouse_LocationsWithoutHidingPlaces + "," +
                           MyTestHouse.SerializedTestHouse_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid PlayerStartingPoint - nonexistent");

                // Invalid LocationsWithHidingPlaces - empty list
                yield return new TestCaseData("Cannot perform action because locations with hiding places list is empty",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid LocationsWithHidingPlaces - empty");

                // Invalid LocationsWithoutHidingPlaces - Location has no exits
                yield return new TestCaseData("Cannot perform action because location \"Hallway\" must be assigned at least one exit",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has no exits
                yield return new TestCaseData("Cannot perform action because location \"Attic\" must be assigned at least one exit",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid LocationsWithHidingPlaces - no exits");

                // Invalid LocationsWithoutHidingPlaces - Location has nonexistent exit
                yield return new TestCaseData("Cannot perform action because \"Hallway\" exit location \"Dungeon\" in direction \"Down\" does not exist",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid LocationsWithoutHidingPlaces - nonexistent exit");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has nonexistent exit
                yield return new TestCaseData("Cannot perform action because \"Bathroom\" exit location \"Dungeon\" in direction \"Down\" does not exist",
                        "{" +
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
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue - invalid LocationsWithHidingPlaces - nonexistent exit");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection - LocationsWithoutHidingPlaces");

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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection - LocationsWithHidingPlaces");
            }
        }
    }
}
