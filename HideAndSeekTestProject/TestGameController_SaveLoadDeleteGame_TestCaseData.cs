using Moq;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// TestCaseData for some GameController tests for saving/loading/deleting games
    /// </summary>
    public static class TestGameController_SaveLoadDeleteGame_TestCaseData
    {
        /// <summary>
        /// Dictionary of Opponents and associated LocationWithHidingPlace names
        /// for SavedGame for tests
        /// </summary>
        public static Dictionary<string, string> SavedGame_OpponentsAndHidingPlaces
        {
            get
            {
                Dictionary<string, string> opponentsAndHidingPlaces = new Dictionary<string, string>();
                opponentsAndHidingPlaces.Add("Joe", "Kitchen");
                opponentsAndHidingPlaces.Add("Bob", "Pantry");
                opponentsAndHidingPlaces.Add("Ana", "Bathroom");
                opponentsAndHidingPlaces.Add("Owen", "Kitchen");
                opponentsAndHidingPlaces.Add("Jimmy", "Pantry");
                return opponentsAndHidingPlaces;
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
                        SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                        SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                        SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                        SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                    "}";
            }
        }

        /// <summary>
        /// Text for serialized HouseFileName property of SavedGame for tests
        /// </summary>
        public static string SavedGame_Serialized_HouseFileName
        {
            get
            {
                return "\"HouseFileName\":\"DefaultHouse\"";
            }
        }

        /// <summary>
        /// Text for serialized PlayerLocation property for test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_PlayerLocation_NoOpponentsGame
        {
            get
            {
                return "\"PlayerLocation\":\"Entry\"";
            }
        }

        /// <summary>
        /// Text for serialized MoveNumber property for test SavedGame with no found opponents
        /// </summary>
        public static string SavedGame_Serialized_MoveNumber_NoFoundOpponents
        {
            get
            {
                return "\"MoveNumber\":1";
            }
        }

        /// <summary>
        /// Text for serialized OpponentsAndHidingLocations property for test SavedGame
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
        /// Text for serialized FoundOpponents property for test SavedGame with no found opponents
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
        private static readonly string serializedCustomTestHouse =
            #region serialized test House
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
                            "\"East\":\"Sensory Room\"" +
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
        #endregion

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile
        {
            get
            {
                // Default House, no Opponents found
                yield return new TestCaseData(
                        "DefaultHouse.json",
                        DefaultHouse_Serialized,
                        (IFileSystem fileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Create GameController with specified file system, hide all Opponents in specified locations, and return GameController
                            return new GameController(fileSystem, "DefaultHouse")
                                       .RehideAllOpponents(SavedGame_OpponentsAndHidingPlaces.Values);
                        }, 
                        SavedGame_Serialized_NoFoundOpponents)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - default House - no opponents found")
                    .SetCategory("GameController Save Success");

                // Default House, 3 Opponents found
                yield return new TestCaseData(
                        "DefaultHouse.json",
                        DefaultHouse_Serialized,
                        (IFileSystem fileSystem, Mock<IFileSystem> mockHouseFileSystem) => 
                        {
                            // Create GameController with specified file system and hide all Opponents in specified locations
                            GameController gameController = new GameController(fileSystem, "DefaultHouse")
                                                                .RehideAllOpponents(SavedGame_OpponentsAndHidingPlaces.Values);

                            // Go to Kitchen and check to find 2 Opponents
                            gameController.ParseInput("East");
                            gameController.ParseInput("Northwest");
                            gameController.ParseInput("check");

                            // Go to Bathroom and check to find 1 Opponent
                            gameController.ParseInput("Southeast");
                            gameController.ParseInput("North");
                            gameController.ParseInput("check");

                            // Return GameController after 3 Opponents found
                            return gameController;
                        },
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            "\"PlayerLocation\":\"Bathroom\"" + "," +
                            "\"MoveNumber\":7" + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - default House - 3 opponents found")
                    .SetCategory("GameController Save Success");
                
                // Custom House with constructor, no Opponents found
                yield return new TestCaseData(
                        "TestHouse.json",
                        serializedCustomTestHouse,
                        (IFileSystem fileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            return new GameController(fileSystem, "TestHouse") // Create GameController with specified file system and specific House
                                       .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Landing\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - custom House - constructor - no opponents found")
                    .SetCategory("GameController Save Success CustomHouse Constructor");

                // Custom House with ReloadGame, no Opponents found
                yield return new TestCaseData(
                    "TestHouse.json",
                        serializedCustomTestHouse,
                        (IFileSystem gameControllerFileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Add DefaultHouse file to House file system mock and set House file system
                            mockHouseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(
                                                  mockHouseFileSystem, "DefaultHouse.json", DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Return GameController with restarted game and rehidden Opponents
                            return new GameController(gameControllerFileSystem, "DefaultHouse") // Create GameController with specified file system
                                       .RestartGame("TestHouse") // and restart game with specific House
                                       .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Landing\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - custom House - ReloadGame - no opponents found")
                    .SetCategory("GameController Save Success CustomHouse ReloadGame");

                // Custom House with constructor, 3 Opponents found
                yield return new TestCaseData(
                        "TestHouse.json",
                        serializedCustomTestHouse,
                        (IFileSystem gameControllerFileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Add DefaultHouse file to House file system mock and set House file system
                            mockHouseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(
                                                  mockHouseFileSystem, "DefaultHouse.json", DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Initialize to GameController with restarted game and rehidden Opponents
                            GameController gameController = new GameController(gameControllerFileSystem, "DefaultHouse") // Create GameController with specified file system and default House
                                   .RestartGame("TestHouse") // and restart game with specific House
                                   .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                            // Go to Cellar and find 1 Opponent there
                            gameController.ParseInput("North");
                            gameController.ParseInput("East");
                            gameController.ParseInput("Down");
                            gameController.ParseInput("Check");

                            // Go to Yard and find 2 Opponents there
                            gameController.ParseInput("Up");
                            gameController.ParseInput("Out");
                            gameController.ParseInput("Check");

                            // Return GameController
                            return gameController;
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Yard\"" + "," +
                            "\"MoveNumber\":8" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[\"Ana\",\"Bob\",\"Jimmy\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - custom House - constructor - 3 opponents found")
                    .SetCategory("GameController Save Success CustomHouse Constructor");

                // Custom House with ReloadGame, 3 Opponents found
                yield return new TestCaseData(
                        "TestHouse.json",
                        serializedCustomTestHouse,
                        (IFileSystem fileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Initialize GameController
                            GameController gameController = new GameController(fileSystem, "TestHouse") // Create GameController with specified file system and specific House
                                                                .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                            // Go to Cellar and find 1 Opponent there
                            gameController.ParseInput("North");
                            gameController.ParseInput("East");
                            gameController.ParseInput("Down");
                            gameController.ParseInput("Check");

                            // Go to Yard and find 2 Opponents there
                            gameController.ParseInput("Up");
                            gameController.ParseInput("Out");
                            gameController.ParseInput("Check");

                            // Return GameController
                            return gameController;
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Yard\"" + "," +
                            "\"MoveNumber\":8" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[\"Ana\",\"Bob\",\"Jimmy\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - custom House - ReloadGame - 3 opponents found")
                    .SetCategory("GameController Save Success CustomHouse ReloadGame");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents
        {
            get
            {
                // Default House
                yield return new TestCaseData(
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[]" +
                        "}",
                        "my house", 
                        "DefaultHouse", 
                        DefaultHouse_Serialized,
                        "Entry",
                        DefaultHouse_Locations,
                        DefaultHouse_LocationsWithoutHidingPlaces,
                        DefaultHouse_LocationsWithHidingPlaces,
                        "Entry", 
                        1,
                        new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" },
                        new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents - default House")
                    .SetCategory("GameController Load Success");
                
                // Custom test House
                yield return new TestCaseData(
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Landing\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}",
                        "test house",
                        "TestHouse",
                        serializedCustomTestHouse,
                        "Landing",
                        new List<string>() { "Landing", "Hallway", "Bedroom", "Closet", "Sensory Room", "Kitchen", "Cellar",
                                             "Pantry", "Yard", "Bathroom", "Living Room", "Office", "Attic" },
                        new List<string>() { "Landing", "Hallway" },
                        new List<string>() { "Bedroom", "Closet", "Sensory Room", "Kitchen", "Cellar", 
                                             "Pantry", "Yard", "Bathroom", "Living Room", "Office", "Attic"},
                        "Landing",
                        1,
                        new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" },
                        new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents - custom House")
                    .SetCategory("GameController Load CustomHouse Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData
        {
            get
            {
                // RANDOM ISSUES
                // No data in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 2.", 
                        "  ")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.", 
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - just characters in file");

                // MISSING KEY/VALUE SET
                // Missing House file name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: HouseFileName",
                        "{" +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing house file name");
                
                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: PlayerLocation",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: MoveNumber",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: OpponentsAndHidingLocations",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: FoundOpponents",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing found opponents");

                // INVALID VALUE DATA
                // Invalid House file name
                yield return new TestCaseData("Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                            "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid HouseFileName");

                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid PlayerLocation");

                // Invalid (negative) move number
                yield return new TestCaseData("invalid MoveNumber",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            "\"MoveNumber\":-1" + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid MoveNumber");

                // No opponents
                yield return new TestCaseData("no opponents",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            "\"OpponentsAndHidingLocations\":{}" + "," +
                            SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no opponents");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("invalid hiding location for opponent",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
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
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("invalid hiding location for opponent",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
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
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
                        "{" +
                            SavedGame_Serialized_HouseFileName + "," +
                            SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                            SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - found opponent Steve is not opponent");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents
        {
            get
            {
                // 1 found Opponent
                yield return new TestCaseData("Bathroom", 4, new List<string>() { "Ana" },
                        (GameController gameController) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(gameController.House.LocationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Opponents Bob and Jimmy are hiding in the Pantry
                                Assert.That(gameController.House.LocationsWithHidingPlaces
                                                .Where((l) => l.Name == "Pantry").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                            "Bob and Jimmy are hidden in the Pantry");
                            });
                        })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents - 1 found opponent");

                // 2 found Opponents
                yield return new TestCaseData("Pantry", 10, new List<string>() { "Bob", "Jimmy" },
                        (GameController gameController) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(gameController.House.LocationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Ana is hiding in the Bathroom
                                Assert.That(gameController.House.LocationsWithHidingPlaces
                                                .Where((l) => l.Name == "Bathroom").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Ana" }),
                                            "Ana is hidden in the Bathroom");
                            });
                        })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents - 2 found opponents");

                // 3 found Opponents
                yield return new TestCaseData("Bathroom", 15, new List<string>() { "Joe", "Owen", "Ana" },
                        (GameController gameController) =>
                        {
                            // Assert that Opponents Joe and Owen are hiding in the Kitchen
                            Assert.That(gameController.House.LocationsWithHidingPlaces
                                            .Where((l) => l.Name == "Pantry").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                        "Bob and Jimmy are hidden in the Kitchen");
                        })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents - 3 found opponents");

                // 4 found Opponents
                yield return new TestCaseData("Pantry", 20, new List<string>() { "Joe", "Owen", "Bob", "Jimmy" },
                        (GameController gameController) =>
                        {
                            // Assert that Ana is hiding in the Bathroom
                            Assert.That(gameController.House.LocationsWithHidingPlaces
                                            .Where((l) => l.Name == "Bathroom").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Ana" }),
                                        "Ana is hidden in the Bathroom");
                        })
                    .SetName("Test_GameController_ParseInput_ToLoadGame_WithFoundOpponents - 4 found opponents");
            }
        }
    }
}