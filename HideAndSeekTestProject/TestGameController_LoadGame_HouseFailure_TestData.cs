using System.Collections;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for some GameController tests for LoadGame with corrupt House files
    /// </summary>
    public static class TestGameController_LoadGame_HouseFailure_TestData
    {
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

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid
        {
            get
            {
                // NO PROPERTIES
                // Empty file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - empty file");

                // File with only whitespace
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 1.",
                        " ")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - random characters");

                // MISSING PROPERTIES
                // File missing Name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: Name",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerStartingPoint\":\"Entry\"" + "," +
                            DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                            DefaultHouse_Serialized_LocationsWithHidingPlaces +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: HouseFileName",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: PlayerStartingPoint",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithoutHidingPlaces",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithHidingPlaces",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid - no LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue
        {
            get
            {
                // Invalid Name (whitespace)
                yield return new TestCaseData("house name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\" \"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - invalid Name - whitespace");

                // Invalid HouseFileName (whitespace)
                yield return new TestCaseData("house file name \" \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint (whitespace)
                yield return new TestCaseData("player starting point location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace description is invalid (whitespace)
                yield return new TestCaseData("hiding place \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location HidingPlace - whitespace");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection
        {
            get
            {
                // Invalid LocationsWithoutHidingPlaces - exit Direction is invalid
                yield return new TestCaseData("{" +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection - LocationsWithoutHidingPlaces");

                // Invalid LocationsWithHidingPlaces - exit Direction is invalid
                yield return new TestCaseData("{" +
                            "\"Name\":\"my house\"" + "," +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection - LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue
        {
            get
            {
                // Invalid LocationsWithHidingPlaces - empty list
                yield return new TestCaseData("locations with hiding places list is empty",
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - empty");

                // Invalid LocationsWithoutHidingPlaces - Location has no exits
                yield return new TestCaseData("location \"Hallway\" must be assigned at least one exit",
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has no exits
                yield return new TestCaseData("location \"Attic\" must be assigned at least one exit",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - no exits");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation
        {
            get
            {
                // Invalid PlayerStartingPoint - not a Location
                yield return new TestCaseData("player starting point location \"Dungeon\" does not exist in House",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Dungeon\"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation - PlayerStartingPoint");

                // Invalid LocationsWithoutHidingPlaces - Location has nonexistent exit
                yield return new TestCaseData("\"Hallway\" exit location \"Dungeon\" in direction \"Down\" does not exist",
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation - LocationsWithoutHidingPlaces");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has nonexistent exit
                yield return new TestCaseData("\"Bathroom\" exit location \"Dungeon\" in direction \"Down\" does not exist",
                        "{" +
                           "\"Name\":\"my house\"" + "," +
                           "\"HouseFileName\":\"DefaultHouse\"" + "," +
                           "\"PlayerStartingPoint\":\"Entry\"" + "," +
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
                   .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation - LocationsWithHidingPlaces");
            }
        }
    }
}