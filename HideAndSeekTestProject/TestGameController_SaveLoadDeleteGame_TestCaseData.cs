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

        private static readonly string serializedCustomTestHouse =
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

        // Test case data for Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile test
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
                            return new GameController(fileSystem).RehideAllOpponents(MyTestSavedGame.OpponentsAndHidingPlaces.Values);
                        }, 
                        MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - default House - no opponents found")
                    .SetCategory("GameController Save Success");

                // Default House, 3 Opponents found
                yield return new TestCaseData(
                        "DefaultHouse.json",
                        DefaultHouse_Serialized,
                        (IFileSystem fileSystem, Mock<IFileSystem> mockHouseFileSystem) => 
                        {
                            // Create GameController with specified file system and hide all Opponents in specified locations
                            GameController gameController = new GameController(fileSystem).RehideAllOpponents(MyTestSavedGame.OpponentsAndHidingPlaces.Values);

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
                        MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents)
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
                            mockHouseFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true);
                            mockHouseFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Return GameController with restarted game and rehidden Opponents
                            return new GameController(gameControllerFileSystem) // Create GameController with specified file system
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

                 yield return new TestCaseData(
                        "TestHouse.json",
                        serializedCustomTestHouse,
                        (IFileSystem gameControllerFileSystem, Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Add DefaultHouse file to House file system mock and set House file system
                            mockHouseFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true);
                            mockHouseFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Initialize to GameController with restarted game and rehidden Opponents
                            GameController gameController = new GameController(gameControllerFileSystem) // Create GameController with specified file system
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

        // Test case data for Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents
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
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
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

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData
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
                // Missing house file name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: HouseFileName",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing house file name");
                
                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: PlayerLocation",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: MoveNumber",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: OpponentsAndHidingLocations",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: FoundOpponents",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing found opponents");

                // INVALID VALUE DATA
                // Invalid House file name
                yield return new TestCaseData("Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)",
                        "{" +
                            "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid HouseFileName");

                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid PlayerLocation");

                // Invalid (negative) move number
                yield return new TestCaseData("invalid MoveNumber",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            "\"MoveNumber\":-1" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid MoveNumber");

                // No opponents
                yield return new TestCaseData("no opponents",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            "\"OpponentsAndHidingLocations\":{}" + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no opponents");

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
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - Location does not exist");

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
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - found opponent Steve is not opponent");
            }
        }
    }
}