using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for House tests for CreateHouse method
    /// </summary>
    [TestFixture]
    public static class TestHouse_CreateHouse_TestData
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

        /// <summary>
        /// Enumerable of Location names in Locations property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_Locations
        {
            get
            {
                return new List<string>()
                    {
                        "Attic",
                        "Hallway",
                        "Bathroom",
                        "Kids Room",
                        "Master Bedroom",
                        "Nursery",
                        "Pantry",
                        "Second Bathroom",
                        "Kitchen",
                        "Master Bath",
                        "Garage",
                        "Landing",
                        "Living Room",
                        "Entry"
                    };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithoutHidingPlaces property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_LocationsWithoutHidingPlaces
        {
            get
            {
                return new List<string>()
                        {
                            "Hallway",
                            "Landing",
                            "Entry"
                        };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithHidingPlaces property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_LocationsWithHidingPlaces
        {
            get
            {
                return new List<string>()
                    {
                        "Attic",
                        "Bathroom",
                        "Kids Room",
                        "Master Bedroom",
                        "Nursery",
                        "Pantry",
                        "Second Bathroom",
                        "Kitchen",
                        "Master Bath",
                        "Garage",
                        "Living Room",
                    };
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid
        {
            get
            {
                // NO PROPERTIES
                // Empty file
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - empty file");

                // File with only whitespace
                yield return new TestCaseData("The input does not contain any JSON tokens. " +
                                              "Expected the input to start with a valid JSON token, when isFinalBlock is true. " +
                                              "Path: $ | LineNumber: 0 | BytePositionInLine: 1.",
                        " ")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - only whitespace");

                // File with random letters and characters
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - random characters");

                // MISSING PROPERTIES
                // File missing Name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: Name",
                        "{" +
                            DefaultHouse_Serialized_HouseFileName + "," +
                            DefaultHouse_Serialized_PlayerStartingPoint + "," +
                            DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                            DefaultHouse_Serialized_LocationsWithHidingPlaces +
                        "}")
                    .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - no Name");

                // File missing HouseFileName
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: HouseFileName",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - no HouseFileName");

                // File missing PlayerStartingPoint
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: PlayerStartingPoint",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - no PlayerStartingPoint");

                // File missing LocationsWithoutHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithoutHidingPlaces",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - no LocationsWithoutHidingPlaces");

                // File missing LocationsWithHidingPlaces
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.House' was missing required properties, including the following: LocationsWithHidingPlaces",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid - no LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue
        {
            get
            {
                // Invalid Name (whitespace)
                yield return new TestCaseData("house name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           "\"Name\":\" \"" + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - invalid Name - whitespace");

                // Invalid HouseFileName (whitespace)
                yield return new TestCaseData("house file name \" \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           "\"HouseFileName\":\" \"" + "," +
                           DefaultHouse_Serialized_PlayerStartingPoint + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - invalid HouseFileName - whitespace");

                // Invalid PlayerStartingPoint (whitespace)
                yield return new TestCaseData("player starting point location name \" \" is invalid (is empty or contains only whitespace)",
                        "{" +
                           DefaultHouse_Serialized_Name + "," +
                           DefaultHouse_Serialized_HouseFileName + "," +
                           "\"PlayerStartingPoint\":\" \"" + "," +
                           DefaultHouse_Serialized_LocationsWithoutHidingPlaces + "," +
                           DefaultHouse_Serialized_LocationsWithHidingPlaces +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - invalid PlayerStartingPoint - whitespace");

                // Invalid LocationsWithoutHidingPlaces - Location name is invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid (is empty or contains only whitespace)",
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - LocationsWithoutHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace name in invalid (whitespace)
                yield return new TestCaseData("location name \" \" is invalid (is empty or contains only whitespace)",
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location Name - whitespace");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace description is invalid (whitespace)
                yield return new TestCaseData("hiding place \" \" is invalid (is empty or contains only whitespace)",
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue - LocationsWithHidingPlaces - invalid Location HidingPlace - whitespace");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection - LocationsWithoutHidingPlaces");

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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection - LocationsWithHidingPlaces");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue
        {
            get
            {
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - empty");

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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid LocationsWithoutHidingPlaces - no exits");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has no exits
                yield return new TestCaseData("location \"Attic\" must be assigned at least one exit",
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
                                    "}" +
                                "}" +
                           "]" +
                       "}")
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid LocationsWithHidingPlaces - no exits");
            }
        }

        public static IEnumerable TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation - PlayerStartingPoint");

                // Invalid LocationsWithoutHidingPlaces - Location has nonexistent exit
                yield return new TestCaseData("\"Hallway\" exit location \"Dungeon\" in direction \"Down\" does not exist",
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation - LocationsWithoutHidingPlaces");

                // Invalid LocationsWithHidingPlaces - LocationWithHidingPlace has nonexistent exit
                yield return new TestCaseData("\"Bathroom\" exit location \"Dungeon\" in direction \"Down\" does not exist",
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
                   .SetName("Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation - LocationsWithHidingPlaces");
            }
        }
    }
}
