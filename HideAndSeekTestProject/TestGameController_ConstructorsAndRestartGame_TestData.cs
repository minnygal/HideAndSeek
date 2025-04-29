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
                // RestartGame method
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
                // RestartGame method
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

        public static IEnumerable TestCases_For_Test_GameController_CheckHouseSetSuccessfully
        {
            get
            {
                // RestartGame method
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - RestartGame")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - parameterless")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - file name")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - # opponents")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - # opponents - file name")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - opponent names")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - opponent names - file name")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - Opponents")
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
                    .SetName("Test_GameController_CheckHouseSetSuccessfully - constructor - Opponents - file name")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName HouseSet Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CheckOpponentsSetSuccessfully
        {
            get
            {
                // RestartGame method
                yield return new TestCaseData(
                    () =>
                    {
                        SetUpHouseMockFileSystemToReturnValidDefaultHouseText(); // Set up mock file system to return valid default House text
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create new GameController with default House
                        SetUpHouseMockFileSystemToReturnValidCustomTestHouseText(); // Set up mock file system for custom test House file
                        return gameController.RestartGame("TestHouse"); // Call RestartGame
                    },
                    MockedOpponents.Select((o) => o.Name).ToArray())
                    .SetName("Test_GameController_CheckOpponentsSetSuccessfully - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName OpponentsSet Success");

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
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_FullGame_InDefaultHouse_With2Opponents_AndCheckMessageAndProperties
        {
            get
            {
                // Constructor with number of opponents
                yield return new TestCaseData(
                    () => new GameController(2), // Return GameController
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_FullGame_InDefaultHouse_With2Opponents_AndCheckMessageAndProperties - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponents FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with names of opponents
                yield return new TestCaseData(
                    () => new GameController(new string[] { "Lisa", "Steve"  }),
                    new string[] { "Lisa", "Steve" })
                    .SetName("Test_GameController_FullGame_InDefaultHouse_With2Opponents_AndCheckMessageAndProperties - constructor - opponent names")
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
                    .SetName("Test_GameController_FullGame_InDefaultHouse_With2Opponents_AndCheckMessageAndProperties - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponents FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_FullGame_InCustomHouse_With2Opponents_AndCheckMessageAndProperties
        {
            get
            {
                // Constructor with number of opponents
                yield return new TestCaseData(
                    () => new GameController(2, "TestHouse"), // Return GameController
                    new string[] { "Joe", "Bob" })
                    .SetName("Test_GameController_FullGame_InCustomHouse_With2Opponents_AndCheckMessageAndProperties - constructor - # opponents")
                    .SetCategory("GameController Constructor SpecifyNumberOfOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");

                // Constructor with names of opponents
                yield return new TestCaseData(
                    () => new GameController(new string[] { "Lisa", "Steve" }, "TestHouse"),
                    new string[] { "Lisa", "Steve" })
                    .SetName("Test_GameController_FullGame_InCustomHouse_With2Opponents_AndCheckMessageAndProperties - constructor - opponent names")
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
                    .SetName("Test_GameController_FullGame_InCustomHouse_With2Opponents_AndCheckMessageAndProperties - constructor - Opponents")
                    .SetCategory("GameController Constructor SpecifyOpponentsAndHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success");
            }
        }

        /// <summary>
        /// Helper method to find all Opponents (using Move/Check methods)
        /// for Test_GameController_RestartGame with initial game completed
        /// </summary>
        /// <param name="gameController">GameController at start of game</param>
        /// <returns>GameController after all Opponents found</returns>
        private static GameController FindAllOpponents()
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
                    () =>
                    {
                        return new GameController(MockedOpponents, "DefaultHouse").RestartGame(); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterless - initial game not completed");

                // Initial game not completed before parameterized RestartGame called
                yield return new TestCaseData(
                    () =>
                    {
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        gameController.RestartGame("DefaultHouse"); // Restart game with specific House layout
                        return gameController.RestartGame(); // Restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - both - initial game not completed");

                // Initial game completed before parameterless RestartGame called
                yield return new TestCaseData(
                    () =>
                    {
                        return FindAllOpponents().RestartGame(); // Find all Opponents, restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - parameterless - initial game completed");

                // Initial game completed before parameterized RestartGame called
                yield return new TestCaseData(
                    () =>
                    {
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create GameController
                        gameController.RestartGame("DefaultHouse"); // Restart game with specific House layout
                        return FindAllOpponents().RestartGame(); // Find all Opponents, restart game and return GameController
                    })
                    .SetName("Test_GameController_RestartGame - both - initial game completed");
            }
        }
    }
}
