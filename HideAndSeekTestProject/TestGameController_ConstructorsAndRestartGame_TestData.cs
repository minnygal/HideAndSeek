using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for constructors and RestartGame
    /// </summary>
    public static class TestGameController_ConstructorsAndRestartGame_TestData
    {
        /// <summary>
        /// Array of mocked Opponents (named)
        /// </summary>
        /// <returns>Array of mocked Opponents</returns>
        private static Opponent[] MockedOpponents
        {
            get
            {
                // Create Opponent mocks
                Mock<Opponent> opponent1 = new Mock<Opponent>();
                opponent1.Setup((o) => o.Name).Returns("Joe");

                Mock<Opponent> opponent2 = new Mock<Opponent>();
                opponent2.Setup((o) => o.Name).Returns("Bob");

                Mock<Opponent> opponent3 = new Mock<Opponent>();
                opponent3.Setup((o) => o.Name).Returns("Ana");

                Mock<Opponent> opponent4 = new Mock<Opponent>();
                opponent4.Setup((o) => o.Name).Returns("Owen");

                Mock<Opponent> opponent5 = new Mock<Opponent>();
                opponent5.Setup((o) => o.Name).Returns("Jimmy");

                // Return mocked Opponents
                return new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object };
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

        // Text representing serialized custom House for test
        private static string CustomTestHouse_Serialized
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

        public static readonly House CustomHouse = GetCustomTestHouse();

        /// <summary>
        /// Get new House object for testing purposes (lines up with CustomTestHouse_ property values)
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        private static House GetCustomTestHouse()
        {
            // Create Landing and connect to new location: Hallway
            Location landing = new Location("Landing");
            Location hallway = landing.AddExit(Direction.North, "Hallway");

            // Connect Hallway to new locations: Bedroom, Sensory Room, Kitchen, Pantry, Bathroom, Living Room, Office, Attic
            LocationWithHidingPlace bedroom = hallway.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace sensoryRoom = hallway.AddExit(Direction.Northeast, "Sensory Room", "under the beanbags");
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.East, "Kitchen", "beside the stove");
            LocationWithHidingPlace pantry = hallway.AddExit(Direction.Southeast, "Pantry", "behind the food");
            LocationWithHidingPlace bathroom = hallway.AddExit(Direction.Southwest, "Bathroom", "in the tub");
            LocationWithHidingPlace livingRoom = hallway.AddExit(Direction.West, "Living Room", "behind the sofa");
            LocationWithHidingPlace office = hallway.AddExit(Direction.Northwest, "Office", "under the desk");
            LocationWithHidingPlace attic = hallway.AddExit(Direction.Up, "Attic", "behind a trunk");

            // Connect Bedroom to new Closet and existing Sensory Room
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.North, "Closet", "between the coats");
            bedroom.AddExit(Direction.East, sensoryRoom);

            // Connect Kitchen to existing Pantry and new Cellar and Yard
            kitchen.AddExit(Direction.South, pantry);
            LocationWithHidingPlace cellar = kitchen.AddExit(Direction.Down, "Cellar", "behind the canned goods");
            LocationWithHidingPlace yard = kitchen.AddExit(Direction.Out, "Yard", "behind a bush");

            // Connect Living Room to existing Office and Bathroom
            livingRoom.AddExit(Direction.North, office);
            livingRoom.AddExit(Direction.South, bathroom);

            // Create list of Location objects (no hiding places)
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>() { landing, hallway };

            // Create list of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                bedroom,
                closet,
                sensoryRoom,
                kitchen,
                cellar,
                pantry,
                yard,
                bathroom,
                livingRoom,
                office,
                attic
            };

            // Create and return new House
            return new House("test house", "TestHouse", landing, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        /// <summary>
        /// Helper method to get simplified House object for game play tests
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        public static House GetHouseForGamePlayTests()
        {
            // Create and connect Location and LocationWithHidingPlace objects
            Location startingPlace = new Location("Start");
            LocationWithHidingPlace bedroom = startingPlace.AddExit(Direction.West, "Bedroom", "under the bed");
            LocationWithHidingPlace office = bedroom.AddExit(Direction.North, "Office", "under the desk");
            Location hallway = office.AddExit(Direction.East, "Hallway");
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.South, "Kitchen", "beside the stove");
            LocationWithHidingPlace pantry = kitchen.AddExit(Direction.East, "Pantry", "behind the canned goods");

            // Create House
            House house = new House(
                "test house",
                "TestHouse",
                startingPlace,
                new List<Location>() { startingPlace, hallway },
                new List<LocationWithHidingPlace>() { bedroom, office, kitchen, pantry });

            // Return House
            return house;
        }

        /// <summary>
        /// Helper method to set House mock file system to return valid default House text
        /// </summary>
        /// <returns>Mock of file system</returns>
        public static Mock<IFileSystem> SetUpHouseMockFileSystemToReturnValidDefaultHouseText()
        {
            // Set up House mock file system to return valid default House text
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("DefaultHouse.house.json", DefaultHouse_Serialized);
            House.FileSystem = houseMockFileSystem.Object; // Set static House file system to mock file system
            return houseMockFileSystem; // Return mock object
        }

        /// <summary>
        /// Helper method to set House mock file system to return valid default House text
        /// </summary>
        /// <param name="houseMockFileSystem">House mock file system</param>
        /// <returns>House mock file system after set up with default House file text</returns>
        public static Mock<IFileSystem> SetUpHouseMockFileSystemToReturnValidDefaultHouseText(Mock<IFileSystem> houseMockFileSystem)
        {
            // Set up House mock file system to return valid default House text
            houseMockFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseMockFileSystem, "DefaultHouse.house.json", DefaultHouse_Serialized);
            House.FileSystem = houseMockFileSystem.Object; // Set static House file system to mock file system
            return houseMockFileSystem; // Return mock object
        }

        /// <summary>
        /// Helper method to set House mock file system to return valid test House text
        /// </summary>
        /// <returns>Mock of file system</returns>
        public static Mock<IFileSystem> SetUpHouseMockFileSystemToReturnValidCustomTestHouseText()
        {
            // Set up House mock file system to return valid test House text
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("TestHouse.house.json", CustomTestHouse_Serialized);
            House.FileSystem = houseMockFileSystem.Object; // Set static House file system to mock file system
            return houseMockFileSystem; // Return mock object
        }

        public static IEnumerable TestCases_For_Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault
        {
            get
            {
                // RestartGame method with House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create new GameController with default House
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system for custom test House file
                        return gameController.RestartGame("TestHouse"); // Call RestartGame
                    },
                    "test house",
                    "TestHouse",
                    CustomTestHouse_LocationsWithoutHidingPlaces,
                    CustomTestHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName HouseSet Success");

                // Parameterless constructor
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(); // Return GameController
                    },
                    "my house",
                    "DefaultHouse",
                    DefaultHouse_LocationsWithoutHidingPlaces,
                    DefaultHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - parameterless")
                    .SetCategory("GameController Constructor Parameterless HouseSet Success");

                // Constructor with House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController("TestHouse"); // Return GameController
                    },
                    "test house",
                    "TestHouse",
                    CustomTestHouse_LocationsWithoutHidingPlaces,
                    CustomTestHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - file name")
                    .SetCategory("GameController Constructor SpecifyHouseFileName HouseSet Success");

                // Constructor with number of opponents
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(1); // Return GameController
                    },
                    "my house",
                    "DefaultHouse",
                    DefaultHouse_LocationsWithoutHidingPlaces,
                    DefaultHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponents HouseSet Success");

                // Constructor with number of opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(1, "TestHouse"); // Return GameController
                    },
                    "test house",
                    "TestHouse",
                    CustomTestHouse_LocationsWithoutHidingPlaces,
                    CustomTestHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - # opponents - file name")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName HouseSet Success");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(new string[] { "Lisa" }); // Return GameController
                    },
                    "my house",
                    "DefaultHouse",
                    DefaultHouse_LocationsWithoutHidingPlaces,
                    DefaultHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames HouseSet Success");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(new string[] { "Lisa" }, "TestHouse"); // Return GameController
                    },
                    "test house",
                    "TestHouse",
                    CustomTestHouse_LocationsWithoutHidingPlaces,
                    CustomTestHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - opponent names - file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName HouseSet Success");

                // Constructor with Opponents
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(MockedOpponents); // Return GameController
                    },
                    "my house",
                    "DefaultHouse",
                    DefaultHouse_LocationsWithoutHidingPlaces,
                    DefaultHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponents HouseSet Success");

                // Constructor with Opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(MockedOpponents, "TestHouse"); // Return GameController
                    },
                    "test house",
                    "TestHouse",
                    CustomTestHouse_LocationsWithoutHidingPlaces,
                    CustomTestHouse_LocationsWithHidingPlaces)
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault - constructor - Opponents - file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName HouseSet Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject
        {
            get
            {
                // Initial game not completed before RestartGame with House object called
                yield return new TestCaseData(
                    CustomHouse,
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        return gameController.RestartGame(CustomHouse); // Restart game with House and return GameController
                    })
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - RestartGame - initial game not completed")
                    .SetCategory("GameController RestartGame SpecifyHouse Success");

                // Initial game completed before RestartGame with House called
                yield return new TestCaseData(
                    CustomHouse,
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        gameController = FindAllOpponentsInDefaultHouse(); // Find all Opponents
                        return gameController.RestartGame(CustomHouse); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - RestartGame - initial game completed")
                    .SetCategory("GameController RestartGame SpecifyHouse Success");

                // Constructor with House object
                yield return new TestCaseData(
                    CustomHouse,
                    () => new GameController(MockedOpponents, CustomHouse))
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - constructor - House")
                    .SetCategory("GameController Constructor SpecifyHouse Success");

                // Constructor with number of opponents and House object
                yield return new TestCaseData(
                    CustomHouse,
                    () => new GameController(2, CustomHouse))
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - constructor - # opponents and House")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouse Success");

                // Constructor with names of opponents and House object
                yield return new TestCaseData(
                    CustomHouse,
                    () => new GameController(new string[] { "Nancy", "Steve" }, CustomHouse))
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - constructor - opponent names and House")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouse Success");

                // Constructor with Opponents and House object
                yield return new TestCaseData(
                    CustomHouse,
                    () => new GameController(new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object }, CustomHouse))
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - constructor - Opponents and House")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouse Success");

                // Constructor with SavedGame object
                yield return new TestCaseData(
                    CustomHouse,
                    () =>
                    {
                        // Create mock of SavedGame object
                        Mock<SavedGame> savedGameMock = new Mock<SavedGame>();
                        savedGameMock.Setup((sg) => sg.House).Returns(CustomHouse);
                        savedGameMock.Setup((sg) => sg.PlayerLocation).Returns("Landing");
                        savedGameMock.Setup((sg) => sg.MoveNumber).Returns(1);
                        savedGameMock.Setup((sg) => sg.OpponentsAndHidingLocations).Returns(
                            new Dictionary<string, string>()
                            {
                                { "Amy", "Kitchen" },
                                { "Steve", "Bedroom" }
                            });

                        // Return new GameController created with SavedGame object
                        return new GameController(savedGameMock.Object);
                    })
                    .SetName("Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject - constructor - SavedGame")
                    .SetCategory("GameController Constructor SpecifySavedGame Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CheckOpponentsSetSuccessfully
        {
            get
            {
                // Parameterless constructor
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(); // Return GameController
                    },
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - parameterless")
                    .SetCategory("GameController Constructor Parameterless OpponentsSet Success");

                // Constructor with House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController("TestHouse"); // Return GameController
                    },
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - file name")
                    .SetCategory("GameController Constructor SpecifyHouseFileName OpponentsSet Success");

                // Constructor with House object
                yield return new TestCaseData(
                    () => new GameController(CustomHouse),
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - House")
                    .SetCategory("GameController Constructor SpecifyHouse OpponentsSet Success");

                // Constructor with number of opponents
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(3); // Return GameController
                    },
                    new string[] { "Joe", "Bob", "Ana" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponents OpponentsSet Success");

                // Constructor with number of opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(2, "TestHouse"); // Return GameController
                    },
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - # opponents - file name")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName OpponentsSet Success");

                // Constructor with number of opponents and House object
                yield return new TestCaseData(
                    () => new GameController(2, CustomHouse),
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - # opponents - House")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouse OpponentsSet Success");

                // Constructor with names of opponents
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(new string[] { "Lisa", "Steve", "Mindy", "Andy", "Patrick", "Marcy", "Anne" }); // Return GameController
                    },
                    new string[] { "Lisa", "Steve", "Mindy", "Andy", "Patrick", "Marcy", "Anne" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames OpponentsSet Success");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(new string[] { "George", "Georgina" }, "TestHouse"); // Return GameController
                    },
                    new string[] { "George", "Georgina" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - opponent names - file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName OpponentsSet Success");

                // Constructor with names of opponents and House object
                yield return new TestCaseData(
                    () => new GameController(new string[] { "George", "Georgina" }, CustomHouse),
                    new string[] { "George", "Georgina" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - opponent names - House")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouse OpponentsSet Success");

                // Constructor with Opponents
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        return new GameController(MockedOpponents); // Return GameController
                    },
                    MockedOpponents.Select((o) => o.Name).ToArray())
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponents OpponentsSet Success");

                // Constructor with Opponents and House file name
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system to return valid test House text
                        return new GameController(MockedOpponents, "TestHouse"); // Return GameController
                    },
                    MockedOpponents.Select((o) => o.Name).ToArray())
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - Opponents - file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName OpponentsSet Success");

                // Constructor with Opponents and House object
                yield return new TestCaseData(
                    () => new GameController(MockedOpponents, CustomHouse),
                    MockedOpponents.Select((o) => o.Name).ToArray())
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - Opponents - House")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouse OpponentsSet Success");

                // Constructor with SavedGame object
                yield return new TestCaseData(
                    () => 
                    {
                        // Create mock of SavedGame object
                        Mock<SavedGame> savedGameMock = new Mock<SavedGame>();
                        savedGameMock.Setup((sg) => sg.House).Returns(CustomHouse);
                        savedGameMock.Setup((sg) => sg.PlayerLocation).Returns("Landing");
                        savedGameMock.Setup((sg) => sg.MoveNumber).Returns(1);
                        savedGameMock.Setup((sg) => sg.OpponentsAndHidingLocations).Returns(
                            new Dictionary<string, string>()
                            {
                                { "Amy", "Kitchen" },
                                { "Steve", "Bedroom" }
                            });

                        // Return new GameController created with SavedGame object
                        return new GameController(savedGameMock.Object);
                    },
                    new string[] { "Amy", "Steve" })
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - constructor - SavedGame")
                    .SetCategory("GameController Constructor SpecifySavedGame OpponentsSet Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName
        {
            get
            {
                // RestartGame with invalid House file name
                yield return new TestCaseData(() =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        new GameController(MockedOpponents, "DefaultHouse").RestartGame("@eou]} {(/");
                    })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName ArgumentException Failure");

                // Constructor with invalid House file name
                yield return new TestCaseData(() =>
                    {
                        new GameController("@eou]} {(/");
                    })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - constructor - file name")
                    .SetCategory("GameController Constructor SpecifyHouseFileName ArgumentException Failure");

                // Constructor with number of opponents and invalid House file name
                yield return new TestCaseData(() =>
                    {
                        new GameController(2, "@eou]} {(/");
                    })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - constructor - # opponents & file name")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName ArgumentException Failure");

                // Constructor with names of opponents and invalid House file name
                yield return new TestCaseData(() =>
                {
                    new GameController(new string[] { "Nancy" }, "@eou]} {(/");
                })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - constructor - opponent names & file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName ArgumentException Failure");

                // Constructor with Opponents and invalid House file name
                yield return new TestCaseData(() =>
                {
                    new GameController(MockedOpponents, "@eou]} {(/");
                })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - constructor - Opponents & file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName ArgumentException Failure");
            }
        }

        /// <summary>
        /// Helper method to set House file system to mock that file does not exist
        /// </summary>
        private static void SetUpMockFileSystemForNonexistentHouseFile(string name)
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup((manager) => manager.File.Exists(name)).Returns(false);
            House.FileSystem = fileSystem.Object;
        }
        
        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist
        {
            get
            {
                // RestartGame method with House file name
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create new GameController with default House
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system for nonexistent House file
                        gameController.RestartGame("MyNonexistentFile"); // Call RestartGame
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName FileNotFoundException Failure");

                // Parameterless constructor
                yield return new TestCaseData(
                    "DefaultHouse",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("DefaultHouse.house.json"); // Set up mock file system for nonexistent default House file
                        new GameController(); // Call GameController parameterless constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - parameterless")
                    .SetCategory("GameController Constructor Parameterless FileNotFoundException Failure");

                // Constructor with House file name
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system for nonexistent House file
                        new GameController("MyNonexistentFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - file name")
                    .SetCategory("GameController Constructor SpecifyHouseFileName FileNotFoundException Failure");

                // Constructor with number of opponents and House file name
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system for nonexistent House file
                        new GameController(1, "MyNonexistentFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName FileNotFoundException Failure");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system for nonexistent House file
                        new GameController(new string[] { "Lisa" }, "MyNonexistentFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName FileNotFoundException Failure");

                // Constructor with Opponents and House file name
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system for nonexistent House file
                        new GameController(MockedOpponents, "MyNonexistentFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponentAndHouseFileName FileNotFoundException Failure");
            }
        }
        
        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt
        {
            get
            {
                // RestartGame method with House file name
                yield return new TestCaseData(
                    "MyCorruptFile",
                    (Mock<IFileSystem> mockFileSystem) => {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(mockFileSystem); // Set up mock file system to return valid default House text
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create new GameController
                        gameController.RestartGame("MyCorruptFile"); // Call RestartGame
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName FileNotFoundException Failure");

                // Parameterless constructor
                yield return new TestCaseData(
                    "DefaultHouse",
                    (Mock<IFileSystem> mockFileSystem) => {
                        House.FileSystem = mockFileSystem.Object; // Set static House file system to mock file system
                        new GameController(); // Call GameController parameterless constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - constructor - parameterless")
                    .SetCategory("GameController Constructor Parameterless FileNotFoundException Failure");

                // Constructor with House file name
                yield return new TestCaseData(
                    "MyCorruptFile",
                    (Mock<IFileSystem> mockFileSystem) => {
                        House.FileSystem = mockFileSystem.Object; // Set static House file system to mock file system
                        new GameController("MyCorruptFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - constructor - file name")
                    .SetCategory("GameController Constructor SpecifyHouseFileName FileNotFoundException Failure");

                // Constructor with number of opponents and House file name
                yield return new TestCaseData(
                    "MyCorruptFile",
                    (Mock<IFileSystem> mockFileSystem) => {
                        House.FileSystem = mockFileSystem.Object; // Set static House file system to mock file system
                        new GameController(1, "MyCorruptFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName FileNotFoundException Failure");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(
                    "MyCorruptFile",
                    (Mock<IFileSystem> mockFileSystem) => {
                        House.FileSystem = mockFileSystem.Object; // Set static House file system to mock file system
                        new GameController(new string[] { "Lisa" }, "MyCorruptFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - constructor - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName FileNotFoundException Failure");

                // Constructor with Opponents and House file name
                yield return new TestCaseData(
                    "MyCorruptFile",
                    (Mock<IFileSystem> mockFileSystem) => {
                        House.FileSystem = mockFileSystem.Object; // Set static House file system to mock file system
                        new GameController(MockedOpponents, "MyCorruptFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponentAndHouseFileName FileNotFoundException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber
        {
            get
            {
                // Constructor with number of opponents
                yield return new TestCaseData(() => { new GameController(0); })
                    .SetName("Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber - only number")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponents ArgumentException Failure");

                // Constructor with number of opponents and House file name
                yield return new TestCaseData(() => { new GameController(0, "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber - and file name")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName ArgumentException Failure");

                // Constructor with number of opponents and House object
                yield return new TestCaseData(() => { new GameController(0, CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber - and House object")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouse ArgumentException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForInvalidName
        {
            get
            {
                // Constructor with names of opponents
                yield return new TestCaseData(() => { new GameController(new string[] { "Jenny", " " }); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponentNames_AndCheckErrorMessage_ForInvalidName - only names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames ArgumentException Failure");

                // Constructor with names of opponents and House file name
                yield return new TestCaseData(() => { new GameController(new string[] { "Jenny", " " }, "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponentNames_AndCheckErrorMessage_ForInvalidName - and file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName ArgumentException Failure");

                // Constructor with names of opponents and House object
                yield return new TestCaseData(() => { new GameController(new string[] { "Jenny", " " }, CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponentNames_AndCheckErrorMessage_ForInvalidName - and House object")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouse ArgumentException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames
        {
            get
            {
                // Constructor with empty string list
                yield return new TestCaseData(() => { new GameController(Array.Empty<string>()); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames - only names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames ArgumentException Failure");

                // Constructor with empty string list and House file name
                yield return new TestCaseData(() => { new GameController(Array.Empty<string>(), "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames - and file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName ArgumentException Failure");

                // Constructor with empty string list and House object
                yield return new TestCaseData(() => { new GameController(Array.Empty<string>(), CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames - and House object")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouse ArgumentException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName
        {
            get
            {
                // Constructor with string list
                yield return new TestCaseData(() => { new GameController(new string[] { "Anne", "Bob", null }); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName - only names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames ArgumentNullException Failure");

                // Constructor with string list and House file name
                yield return new TestCaseData(() => { new GameController(new string[] { "Anne", "Bob", null }, "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName - and file name")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName ArgumentNullException Failure");

                // Constructor with string list and House object
                yield return new TestCaseData(() => { new GameController(new string[] { "Anne", "Bob", null }, CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName - and House object")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouse ArgumentNullException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents
        {
            get
            {
                // Constructor with empty Opponent list
                yield return new TestCaseData(() => { new GameController(Array.Empty<Opponent>()); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents - only names")
                    .SetCategory("GameController Constructor SpecifyOpponents ArgumentException Failure");

                // Constructor with empty Opponent list and House file name
                yield return new TestCaseData(() => { new GameController(Array.Empty<Opponent>(), "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents - and file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName ArgumentException Failure");

                // Constructor with empty Opponent list and House object
                yield return new TestCaseData(() => { new GameController(Array.Empty<Opponent>(), CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents - and House object")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouse ArgumentException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent
        {
            get
            {
                // Constructor with Opponent list with null value
                yield return new TestCaseData(() => { new GameController(new Opponent[] { null }); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent - only names")
                    .SetCategory("GameController Constructor SpecifyOpponents ArgumentNullException Failure");

                // Constructor with Opponent list with null value and House file name
                yield return new TestCaseData(() => { new GameController(new Opponent[] { null }, "TestHouse"); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent - and file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName ArgumentNullException Failure");

                // Constructor with Opponent list with null value and House object
                yield return new TestCaseData(() => { new GameController(new Opponent[] { null }, CustomHouse); })
                    .SetName("Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent - and House object")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouse ArgumentNullException Failure");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents
        {
            get
            {
                // Constructor with number of opponents
                yield return new TestCaseData(
                    () => new GameController(2), // Return GameController
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponents FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with names of opponents
                yield return new TestCaseData(
                    () => new GameController(new string[] { "Lisa", "Steve"  }),
                    new string[] { "Lisa", "Steve" })
                    .SetName("Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents - constructor - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNames FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with Opponents
                yield return new TestCaseData(
                    () =>
                    {
                        // Create Opponent mocks
                        Mock<Opponent> opponent1 = new Mock<Opponent>();
                        opponent1.Setup((o) => o.Name).Returns("Annie");

                        Mock<Opponent> opponent2 = new Mock<Opponent>();
                        opponent2.Setup((o) => o.Name).Returns("Paul");

                        // Return GameController
                        return new GameController(new Opponent[] { opponent1.Object, opponent2.Object }); 
                    },
                    new string[] { "Annie", "Paul" })
                    .SetName("Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponents FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_FullGame_InCustomHouse_With2Opponents
        {
            get
            {
                // Constructor with number of opponents
                yield return new TestCaseData(
                    () => new GameController(2, "TestHouse"),
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByFileName_With2Opponents - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with names of opponents
                yield return new TestCaseData(
                    () => new GameController(new string[] { "Lisa", "Steve" }, "TestHouse"),
                    new string[] { "Lisa", "Steve" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByFileName_With2Opponents - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with Opponents
                yield return new TestCaseData(
                    () =>
                    {
                        // Create Opponent mocks
                        Mock<Opponent> opponent1 = new Mock<Opponent>();
                        opponent1.Setup((o) => o.Name).Returns("Annie");

                        Mock<Opponent> opponent2 = new Mock<Opponent>();
                        opponent2.Setup((o) => o.Name).Returns("Paul");

                        // Return GameController
                        return new GameController(new Opponent[] { opponent1.Object, opponent2.Object }, "TestHouse");
                    },
                    new string[] { "Annie", "Paul" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByFileName_With2Opponents - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents
        {
            get
            {
                // Constructor with number of opponents and House object
                yield return new TestCaseData(
                    () => new GameController(2, GetHouseForGamePlayTests()), // Return GameController
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with names of opponents and House object
                yield return new TestCaseData(
                    () => new GameController(new string[] { "Lisa", "Steve" }, GetHouseForGamePlayTests()),
                    new string[] { "Lisa", "Steve" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents - opponent names")
                    .SetCategory("GameController Constructor SpecifyOpponentNamesAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with Opponents and House object
                yield return new TestCaseData(
                    () =>
                    {
                        // Create Opponent mocks
                        Mock<Opponent> opponent1 = new Mock<Opponent>();
                        opponent1.Setup((o) => o.Name).Returns("Annie");

                        Mock<Opponent> opponent2 = new Mock<Opponent>();
                        opponent2.Setup((o) => o.Name).Returns("Paul");

                        // Return GameController
                        return new GameController(new Opponent[] { opponent1.Object, opponent2.Object }, GetHouseForGamePlayTests());
                    },
                    new string[] { "Annie", "Paul" })
                    .SetName("Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");
            }
        }

        /// <summary>
        /// Helper method to find all Opponents (using Move/Check methods)
        /// for Test_GameController_RestartGame with initial game completed
        /// </summary>
        /// <param name="gameController">GameController at start of game</param>
        /// <returns>GameController after all Opponents found</returns>
        private static GameController FindAllOpponentsInDefaultHouse()
        {
            GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
            gameController.Move(Direction.East); // Move to Hallway
            gameController.Move(Direction.Northwest); // Move to Kitchen
            gameController.CheckCurrentLocation(); // Check Kitchen and find Bob and Owen
            gameController.Move(Direction.Southeast); // Move to Hallway
            gameController.Move(Direction.North); // Move to Bathroom
            gameController.CheckCurrentLocation(); // Check Bathroom and find Ana
            gameController.Move(Direction.South); // Move to Hallway
            gameController.Move(Direction.Up); // Move to Landing
            gameController.Move(Direction.South); // Move to Pantry
            gameController.CheckCurrentLocation(); // Check Pantry and find Bob and Jimmy
            return gameController;
        }

        public static IEnumerable TestCases_For_Test_GameController_RestartGame
        {
            get
            {
                // Initial game not completed before parameterless RestartGame called
                yield return new TestCaseData(
                    "Entry",
                    new List<string>() { "Attic", "Bathroom", "Kids Room", "Master Bedroom", "Attic" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        return new GameController(MockedOpponents, "DefaultHouse").RestartGame(); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterless - initial game not completed")
                    .SetCategory("GameController RestartGame Parameterless HidingLocations MoveNumber CurrentLocation GameOver Success");

                // Initial game not completed before RestartGame with House file name called
                yield return new TestCaseData(
                    "Landing",
                    new List<string>() { "Bedroom", "Closet", "Sensory Room", "Kitchen", "Bedroom" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText();
                        return gameController.RestartGame("TestHouse"); // Restart game with test House file name and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterized - file name - initial game not completed")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName HidingLocations MoveNumber CurrentLocation GameOver Success");

                // Initial game not completed before RestartGame with House object called
                yield return new TestCaseData(
                    "Start",
                    new List<string>() { "Bedroom", "Office", "Kitchen", "Pantry", "Bedroom" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        return gameController.RestartGame(GetHouseForGamePlayTests()); // Restart game with House and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterized - House - initial game not completed")
                    .SetCategory("GameController RestartGame SpecifyHouse HidingLocations MoveNumber CurrentLocation GameOver Success");

                // Initial game completed before parameterless RestartGame called
                yield return new TestCaseData(
                    "Entry",
                    new List<string>() { "Attic", "Bathroom", "Kids Room", "Master Bedroom", "Attic" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        return FindAllOpponentsInDefaultHouse().RestartGame(); // Find all Opponents, restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterless - initial game completed")
                    .SetCategory("GameController RestartGame Parameterless HidingLocations MoveNumber CurrentLocation GameOver Success");

                // Initial game completed before RestartGame with House file name called
                yield return new TestCaseData(
                    "Landing",
                    new List<string>() { "Bedroom", "Closet", "Sensory Room", "Kitchen", "Bedroom" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        gameController = FindAllOpponentsInDefaultHouse(); // Find all Opponents
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText();
                        return gameController.RestartGame("TestHouse"); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterized - file name - initial game completed")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName HidingLocations MoveNumber CurrentLocation GameOver Success");

                // Initial game completed before RestartGame with House called
                yield return new TestCaseData(
                    "Start",
                    new List<string>() { "Bedroom", "Office", "Kitchen", "Pantry", "Bedroom" },
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText();
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        gameController = FindAllOpponentsInDefaultHouse(); // Find all Opponents
                        return gameController.RestartGame(GetHouseForGamePlayTests()); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterized - House - initial game completed")
                    .SetCategory("GameController RestartGame SpecifyHouse HidingLocations MoveNumber CurrentLocation GameOver Success");
            }
        }
    }
}