using Moq;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for SaveGame method to save game to file
    /// </summary>
    public static class TestGameController_SaveGame_TestData
    {
        /// <summary>
        /// Array with a mocked Opponents (named)
        /// </summary>
        public static Opponent[] MockedOpponents
        {
            get
            {
                // Create Opponent mocks
                Mock<Opponent> opponent1 = new Mock<Opponent>();
                opponent1.Setup((o) => o.Name).Returns("Joe");
                opponent1.Setup((o) => o.ToString()).Returns("Joe");

                Mock<Opponent> opponent2 = new Mock<Opponent>();
                opponent2.Setup((o) => o.Name).Returns("Bob");
                opponent2.Setup((o) => o.ToString()).Returns("Bob");

                Mock<Opponent> opponent3 = new Mock<Opponent>();
                opponent3.Setup((o) => o.Name).Returns("Ana");
                opponent3.Setup((o) => o.ToString()).Returns("Ana");

                Mock<Opponent> opponent4 = new Mock<Opponent>();
                opponent4.Setup((o) => o.Name).Returns("Owen");
                opponent4.Setup((o) => o.ToString()).Returns("Owen");

                Mock<Opponent> opponent5 = new Mock<Opponent>();
                opponent5.Setup((o) => o.Name).Returns("Jimmy");
                opponent5.Setup((o) => o.ToString()).Returns("Jimmy");

                // Return array of mocked Opponents
                return new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object };
            }
        }

        /// <summary>
        /// Dictionary of Opponents and associated LocationWithHidingPlace names
        /// for SavedGame for tests
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
        private static string SerializedCustomTestHouse
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
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_SaveGame_AndCheckTextSavedToFile
        {
            get
            {
                // Default House, no Opponents found
                yield return new TestCaseData(
                        "DefaultHouse.house.json",
                        DefaultHouse_Serialized,
                        (Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Create GameController with specified file system, hide all Opponents in specified locations, and return GameController
                            return new GameController(MockedOpponents, "DefaultHouse")
                                       .RehideAllOpponents(SavedGame_OpponentsAndHidingPlaces.Values);
                        }, 
                        SavedGame_Serialized_NoFoundOpponents)
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - default House - no opponents found")
                    .SetCategory("GameController SaveGame Success");

                // Default House, 3 Opponents found
                yield return new TestCaseData(
                        "DefaultHouse.house.json",
                        DefaultHouse_Serialized,
                        (Mock<IFileSystem> mockHouseFileSystem) => 
                        {
                            // Create GameController with specified file system and hide all Opponents in specified locations
                            GameController gameController = new GameController(MockedOpponents, "DefaultHouse")
                                                                .RehideAllOpponents(SavedGame_OpponentsAndHidingPlaces.Values);

                            // Go to Kitchen and check to find 2 Opponents
                            gameController.Move(Direction.East);
                            gameController.Move(Direction.Northwest);;
                            gameController.CheckCurrentLocation();

                            // Go to Bathroom and check to find 1 Opponent
                            gameController.Move(Direction.Southeast);;
                            gameController.Move(Direction.North);;
                            gameController.CheckCurrentLocation();

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
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - default House - 3 opponents found")
                    .SetCategory("GameController SaveGame Success");
                
                // Custom House with constructor, no Opponents found
                yield return new TestCaseData(
                        "TestHouse.house.json",
                        SerializedCustomTestHouse,
                        (Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            return new GameController(MockedOpponents, "TestHouse") // Create GameController with specified file system and specific House
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
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - constructor - no opponents found")
                    .SetCategory("GameController SaveGame Success CustomHouse Constructor");

                // Custom House with ReloadGame, no Opponents found
                yield return new TestCaseData(
                    "TestHouse.house.json",
                        SerializedCustomTestHouse,
                        (Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Add DefaultHouse file to House file system mock and set House file system
                            mockHouseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(
                                                  mockHouseFileSystem, "DefaultHouse.house.json", DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Return GameController with restarted game and rehidden Opponents
                            return new GameController(MockedOpponents, "DefaultHouse") // Create GameController with specified file system
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
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - ReloadGame - no opponents found")
                    .SetCategory("GameController SaveGame Success CustomHouse ReloadGame");

                // Custom House with constructor, 3 Opponents found
                yield return new TestCaseData(
                        "TestHouse.house.json",
                        SerializedCustomTestHouse,
                        (Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Add DefaultHouse file to House file system mock and set House file system
                            mockHouseFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(
                                                  mockHouseFileSystem, "DefaultHouse.house.json", DefaultHouse_Serialized);
                            House.FileSystem = mockHouseFileSystem.Object;

                            // Initialize to GameController with restarted game and rehidden Opponents
                            GameController gameController = new GameController(MockedOpponents, "DefaultHouse") // Create GameController with specified file system and default House
                                   .RestartGame("TestHouse") // and restart game with specific House
                                   .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                            // Go to Cellar and find 1 Opponent there
                            gameController.Move(Direction.North);;
                            gameController.Move(Direction.East);
                            gameController.Move(Direction.Down);;
                            gameController.CheckCurrentLocation();

                            // Go to Yard and find 2 Opponents there
                            gameController.Move(Direction.Up);;
                            gameController.Move(Direction.Out);;
                            gameController.CheckCurrentLocation();

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
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - constructor - 3 opponents found")
                    .SetCategory("GameController SaveGame Success CustomHouse Constructor");

                // Custom House with ReloadGame, 3 Opponents found
                yield return new TestCaseData(
                        "TestHouse.house.json",
                        SerializedCustomTestHouse,
                        (Mock<IFileSystem> mockHouseFileSystem) =>
                        {
                            // Initialize GameController
                            GameController gameController = new GameController(MockedOpponents, "TestHouse") // Create GameController with specified file system and specific House
                                                                .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                            // Go to Cellar and find 1 Opponent there
                            gameController.Move(Direction.North);;
                            gameController.Move(Direction.East);
                            gameController.Move(Direction.Down);;
                            gameController.CheckCurrentLocation();

                            // Go to Yard and find 2 Opponents there
                            gameController.Move(Direction.Up);;
                            gameController.Move(Direction.Out);;
                            gameController.CheckCurrentLocation();

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
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - ReloadGame - 3 opponents found")
                    .SetCategory("GameController SaveGame Success CustomHouse ReloadGame");
            }
        }
    }
}