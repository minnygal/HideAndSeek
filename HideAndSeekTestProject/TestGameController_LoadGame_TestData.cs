using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for LoadGame method to load saved game from file
    /// </summary>
    public static class TestGameController_LoadGame_TestData
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

        /// <summary>
        /// Text representing serialized custom House for tests
        /// </summary>
        public static string CustomTestHouse_Serialized
        {
            get
            {
                return
                "{" +
                    "\"Name\":\"test house\"" + "," +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    "\"PlayerStartingPoint\":\"Landing\"" + "," +
                    "\"LocationsWithoutHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"Name\":\"Landing\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"Name\":\"Hallway\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"," +
                                "\"Northeast\":\"Sensory Room\"," +
                                "\"East\":\"Kitchen\"," +
                                "\"Southeast\":\"Pantry\"," +
                                "\"South\":\"Landing\"," +
                                "\"Southwest\":\"Bathroom\"," +
                                "\"West\":\"Living Room\"," +
                                "\"Northwest\":\"Office\"," +
                                "\"Up\":\"Attic\"" +
                            "}" +
                        "}" +
                    "]" + "," +
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"," +
                            "\"Name\":\"Bedroom\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Closet\"," +
                                "\"East\":\"Sensory Room\"," +
                                "\"South\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{\"HidingPlace\":\"between the coats\"," +
                            "\"Name\":\"Closet\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"under the beanbags\"," +
                            "\"Name\":\"Sensory Room\"," +
                            "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southwest\":\"Hallway\"," +
                                    "\"West\":\"Bedroom\"" +
                                "}" +
                            "}," +
                        "{" +
                            "\"HidingPlace\":\"beside the stove\"," +
                            "\"Name\":\"Kitchen\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Hallway\"," +
                                "\"South\":\"Pantry\"," +
                                "\"Down\":\"Cellar\"," +
                                "\"Out\":\"Yard\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the canned goods\"," +
                            "\"Name\":\"Cellar\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Up\":\"Kitchen\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the food\"," +
                            "\"Name\":\"Pantry\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Kitchen\"," +
                                "\"Northwest\":\"Hallway\"" +
                            "}" +
                        "}," + "{" +
                            "\"HidingPlace\":\"behind a bush\"," +
                            "\"Name\":\"Yard\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"In\":\"Kitchen\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"in the tub\"," +
                            "\"Name\":\"Bathroom\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Living Room\"," +
                                "\"Northeast\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the sofa\"," +
                            "\"Name\":\"Living Room\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Office\"," +
                                "\"East\":\"Hallway\"," +
                                "\"South\":\"Bathroom\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"under the desk\"," +
                            "\"Name\":\"Office\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Southeast\":\"Hallway\"," +
                                "\"South\":\"Living Room\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind a trunk\"," +
                            "\"Name\":\"Attic\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Down\":\"Hallway\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            }
        }

        /// <summary>
        /// Enumerable of Location names in Locations property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_Locations
        {
            get
            {
                return new List<string>()
                    {
                        "Landing",
                        "Hallway",
                        "Bedroom",
                        "Closet",
                        "Sensory Room",
                        "Kitchen",
                        "Cellar",
                        "Pantry",
                        "Yard",
                        "Bathroom",
                        "Living Room",
                        "Office",
                        "Attic"
                };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithoutHidingPlaces property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_LocationsWithoutHidingPlaces
        {
            get
            {
                return new List<string>() { "Landing", "Hallway" };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithHidingPlaces property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_LocationsWithHidingPlaces
        {
            get
            {
                return new List<string>()
                    {
                        "Bedroom", 
                        "Closet", 
                        "Sensory Room", 
                        "Kitchen", 
                        "Cellar",
                        "Pantry", 
                        "Yard", 
                        "Bathroom", 
                        "Living Room", 
                        "Office", 
                        "Attic"
                    };
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_WithFoundOpponents
        {
            get
            {
                // 1 found Opponent
                yield return new TestCaseData("Bathroom", 4, new List<string>() { "Ana" },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Opponents Bob and Jimmy are hiding in the Pantry
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Pantry").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                            "Bob and Jimmy are hidden in the Pantry");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - 1 found opponent");

                // 2 found Opponents
                yield return new TestCaseData("Pantry", 10, new List<string>() { "Bob", "Jimmy" },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Ana is hiding in the Bathroom
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Bathroom").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Ana" }),
                                            "Ana is hidden in the Bathroom");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - 2 found opponents");

                // 3 found Opponents
                yield return new TestCaseData("Bathroom", 15, new List<string>() { "Joe", "Owen", "Ana" },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Opponents Joe and Owen are hiding in the Kitchen
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Pantry").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                        "Bob and Jimmy are hidden in the Kitchen");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - 3 found opponents");

                // 4 found Opponents
                yield return new TestCaseData("Pantry", 20, new List<string>() { "Joe", "Owen", "Bob", "Jimmy" },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Ana is hiding in the Bathroom
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Bathroom").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Ana" }),
                                        "Ana is hidden in the Bathroom");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - 4 found opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid
        {
            get
            {
                // RANDOM ISSUES
                // No data in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 2.",
                        "  ")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - just characters in file");

                // MISSING KEY/VALUE SET
                // Missing House file name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: HouseFileName",
                        "{" +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing house file name");

                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: PlayerLocation",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: MoveNumber",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: OpponentsAndHidingLocations",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: FoundOpponents",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing found opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue
        {
            get
            {
                // INVALID VALUE DATA
                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation - location \"Tree\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid PlayerLocation");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("location with hiding place \"Tree\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Tree\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("location with hiding place \"Hallway\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Hallway\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - found opponent Steve is not opponent");
            }
        }
    }
}
