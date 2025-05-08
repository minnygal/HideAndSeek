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
    /// Test data for some SavedGame tests for deserialization
    /// </summary>
    public static class TestSavedGame_Deserialize_TestData
    {
        /// <summary>
        /// Dictionary of Opponents and associated LocationWithHidingPlace names for SavedGame for tests
        /// </summary>
        public static Dictionary<string, string> SavedGame_OpponentsAndHidingPlaces
        {
            get
            {
                return new Dictionary<string, string>()
                       {
                           { "Joe", "Kitchen" },
                           { "Bob", "Pantry" },
                           { "Ana", "Bathroom" },
                           { "Owen", "Kitchen" },
                           { "Jimmy", "Pantry" }
                       };
            }
        }

        /// <summary>
        /// Text for serialized test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_NoFoundOpponents
        {
            get
            {
                return
                    "{" +
                        SavedGame_Serialized_HouseFileName + "," +
                        SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                        SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                        SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                        SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                    "}";
            }
        }

        /// <summary>
        /// Text for serialized HouseFileName property of test SavedGame
        /// </summary>
        public static string SavedGame_Serialized_HouseFileName
        {
            get
            {
                return "\"HouseFileName\":\"DefaultHouse\"";
            }
        }

        /// <summary>
        /// Text for serialized PlayerLocation property of test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_PlayerLocation_NoFoundOpponents
        {
            get
            {
                return "\"PlayerLocation\":\"Entry\"";
            }
        }

        /// <summary>
        /// Text for serialized MoveNumber property of test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_MoveNumber_NoFoundOpponents
        {
            get
            {
                return "\"MoveNumber\":1";
            }
        }

        /// <summary>
        /// Text for serialized OpponentsAndHidingLocations property of test SavedGame
        /// </summary>
        public static string SavedGame_Serialized_OpponentsAndHidingLocations
        {
            get
            {
                return
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}";
            }
        }

        /// <summary>
        /// Text for serialized FoundOpponents property of test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_FoundOpponents_NoFoundOpponents
        {
            get
            {
                return "\"FoundOpponents\":[]";
            }
        }

        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized
        {
            get
            {
                return
                    "{" +
                        "\"Name\":\"my house\"" + "," +
                        "\"HouseFileName\":\"DefaultHouse\"" + "," +
                        "\"PlayerStartingPoint\":\"Entry\"" + "," +
                        "\"LocationsWithoutHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"Name\":\"Hallway\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
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
                                    "\"South\":\"Hallway\"" +
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
                    "}";
            }
        }

        /// <summary>
        /// Text representing Name property of default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized_Name
        {
            get
            {
                return "\"Name\":\"my house\"";
            }
        }

        /// <summary>
        /// Text representing HouseFileName property of default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized_HouseFileName
        {
            get
            {
                return "\"HouseFileName\":\"DefaultHouse\"";
            }
        }

        /// <summary>
        /// Text representing PlayerStartingPoint property of default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized_PlayerStartingPoint
        {
            get
            {
                return "\"PlayerStartingPoint\":\"Entry\"";
            }
        }

        /// <summary>
        /// Text representing LocationsWithoutHidingPlaces property of default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized_LocationsWithoutHidingPlaces
        {
            get
            {
                return "\"LocationsWithoutHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"Name\":\"Hallway\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
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
                        "]";
            }
        }

        /// <summary>
        /// Text representing LocationsWithHidingPlaces property of default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized_LocationsWithHidingPlaces
        {
            get
            {
                return "\"LocationsWithHidingPlaces\":" +
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
                                    "\"South\":\"Hallway\"" +
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
                        "]";
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenNoJsonTokens
        {
            get
            {
                // No data in file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenNoJsonTokens - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 2.",
                        "  ")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenNoJsonTokens - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenNoJsonTokens - just characters in file");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenPropertyIsMissing
        {
            get
            {
                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, " +
                                              "including the following: PlayerLocation",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenPropertyIsMissing - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, " +
                                              "including the following: MoveNumber",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenPropertyIsMissing - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, " +
                                              "including the following: OpponentsAndHidingLocations",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenPropertyIsMissing - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, " +
                                              "including the following: FoundOpponents",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenPropertyIsMissing - missing found opponents");

            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenFileDataHasInvalidValue
        {
            get
            {
                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation - location \"Tree\" does not exist in House",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid PlayerLocation");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("location with hiding place \"Tree\" does not exist in House",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Tree\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("location with hiding place \"Hallway\" does not exist in House",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Hallway\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenFileDataHasInvalidValue - found opponent Steve is not opponent");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid
        {
            get
            {
                // NO PROPERTIES
                // Empty file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - empty file");

                // File with only whitespace
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 1.",
                        " ")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - random characters");

                // MISSING PROPERTIES
                // File missing Name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, " +
                                              "including the following: Name",
                        "{" +
                            DefaultHouse_Serialized_HouseFileName + "," +
                            DefaultHouse_Serialized_PlayerStartingPoint + "," +
                            DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                            DefaultHouse_Serialized_LocationsWithHidingPlaces +
                        "}")
                    .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, " +
                                              "including the following: HouseFileName",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, " +
                                              "including the following: PlayerStartingPoint",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, " +
                                              "including the following: LocationsWithoutHidingPlaces",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, " +
                                              "including the following: LocationsWithHidingPlaces",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue
        {
            get
            {
                // INVALID VALUE OF WHITESPACE
                // Invalid Name - whitespace
                yield return new TestCaseData("house name \" \" is invalid " +
                                              "(is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\" \"" + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid Name - whitespace");

                // Invalid HouseFileName - whitespace
                yield return new TestCaseData("house file name \" \" is invalid " +
                                              "(is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint - whitespace
                yield return new TestCaseData("player starting point location name \" \" is invalid " +
                                              "(is empty or contains only whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid " +
                                              "(is empty or contains only whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
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
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid " +
                                              "(is empty or contains only whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - LocationsWithHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace description is invalid (whitespace)
                yield return new TestCaseData("hiding place \" \" is invalid " +
                                              "(is empty or contains only whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - LocationsWithHidingPlaces - invalid Location HidingPlace - whitespace");

                // OTHER INVALID VALUES
                // Invalid LocationsWithHidingPlaces - empty list
                yield return new TestCaseData("locations with hiding places list is empty",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid LocationsWithHidingPlaces - empty");

                // Invalid LocationsWithoutHidingPlaces - Location has no exits
                yield return new TestCaseData("location \"Hallway\" must be assigned at least one exit",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
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
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has no exits
                yield return new TestCaseData("location \"Attic\" must be assigned at least one exit",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           "\"LocationsWithoutHidingPlaces\":" +
                           "[" +
                                "{" +
                                    "\"Name\":\"Entry\"," +
                                    "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Up\":\"Attic\"" +
                                    "}" +
                                "}" +
                           "]" + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue - invalid LocationsWithHidingPlaces - no exits");

            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue_NonexistentLocation
        {
            get
            {
                // Invalid PlayerStartingPoint - not a Location
                yield return new TestCaseData("player starting point location \"Dungeon\" does not exist in House",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\"Dungeon\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue_NonexistentLocation - invalid PlayerStartingPoint - nonexistent");

                // Invalid LocationsWithoutHidingPlaces - Location has nonexistent exit
                yield return new TestCaseData("\"Hallway\" exit location \"Dungeon\" " +
                                              "in direction \"Down\" does not exist",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
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
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue_NonexistentLocation - invalid LocationsWithoutHidingPlaces - nonexistent exit");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has nonexistent exit
                yield return new TestCaseData("\"Bathroom\" exit location \"Dungeon\" " +
                                              "in direction \"Down\" does not exist",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataInvalidValue_NonexistentLocation - invalid LocationsWithHidingPlaces - nonexistent exit");
            }
        }

        public static IEnumerable TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection
        {
            get
            {
                // Invalid LocationsWithoutHidingPlaces - exit Direction is invalid
                yield return new TestCaseData("{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection - LocationsWithoutHidingPlaces");

                // Invalid LocationsWithHidingPlaces - exit direction is invalid
                yield return new TestCaseData("{" +
                            DefaultHouse_Serialized_Name + "," +
                            DefaultHouse_Serialized_HouseFileName + "," +
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
                   .SetName("Test_SavedGame_Deserialize_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection - LocationsWithHidingPlaces");
            }
        }
    }
}