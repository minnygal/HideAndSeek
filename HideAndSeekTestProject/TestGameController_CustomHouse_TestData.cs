using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for testing layout of non-default House 
    /// set using GameController constructor or RestartGame method
    /// </summary>
    public static class TestGameController_CustomHouse_TestData
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

        // Text representing serialized custom House for test
        private static readonly string textInHouseFile =
        #region test House file text
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

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName
        {
            get
            {
                yield return new TestCaseData(() =>
                {
                    new GameController("@eou]} {(/"); // Call GameController constructor
                })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - constructor")
                    .SetCategory("GameController Constructor SpecifyHouseFileName ArgumentException Failure");

                yield return new TestCaseData(() =>
                {
                    new GameController(MockedOpponents, "DefaultHouse").RestartGame("@eou]} {(/"); // Create new GameController and call RestartGame
                })
                    .SetName("Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName ArgumentException Failure");
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
                // Parameterless constructor
                yield return new TestCaseData(
                    "DefaultHouse",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("DefaultHouse.house.json"); // Set up mock file system
                        new GameController(); // Call GameController parameterless constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - parameterless")
                    .SetCategory("GameController Constructor Parameterless FileNotFoundException Failure");

                // Parameterized constructor
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () => {
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system
                        new GameController("MyNonexistentFile"); // Call GameController parameterized constructor
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - constructor - parameterized")
                    .SetCategory("GameController Constructor SpecifyHouseFileName FileNotFoundException Failure");

                // RestartGame method
                yield return new TestCaseData(
                    "MyNonexistentFile",
                    () =>{
                        GameController gameController = new GameController(MockedOpponents, "DefaultHouse"); // Create new GameController
                        SetUpMockFileSystemForNonexistentHouseFile("MyNonexistentFile.house.json"); // Set up mock file system
                        gameController.RestartGame("MyNonexistentFile"); // Call RestartGame
                    })
                    .SetName("Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName FileNotFoundException Failure");
            }
        }

        /// <summary>
        /// Helper method to get GameController with House property 
        /// set to custom House via GameController constructor
        /// </summary>
        /// <returns>GameController with custom House</returns>
        private static GameController GetGameController_WithCustomHouseSetViaConstructor()
        {
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("TestHouse.house.json", textInHouseFile); // Set House file system to mock
            return new GameController(MockedOpponents, "TestHouse"); // Return new GameController with custom House
        }
        
        /// <summary>
        /// Helper method to get GameController with House property
        /// set to custom House via RestartGame method
        /// </summary>
        /// <returns>GameController with custom House</returns>
        private static GameController GetGameController_WithCustomHouseSetViaRestartGame()
        {
            // Set up House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                                                    "DefaultHouse.house.json", TestGameController_CustomHouse_TestData.DefaultHouse_Serialized); // Create mock file system to return default House file text
            MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseMockFileSystem, "TestHouse.house.json", textInHouseFile); // Set up mock file system to return test House file text
            House.FileSystem = houseMockFileSystem.Object; // Set static House file system to mock file system 
            
            // Return GameController after RestartGame called
            return new GameController(MockedOpponents, "DefaultHouse") // Create new GameController with default House
                       .RestartGame("TestHouse"); // Restart game with custom House
        }

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileNameCheckNameAndFileNameProperties
        {
            get
            {
                // GameController parameterized constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckNameAndFileNameProperties - constructor")
                    .SetCategory("GameController Constructor SpecifyHouseFileName Name FileName Success");

                // RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckNameAndFileNameProperties - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName Name FileName Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileNameCheckLocationNamesAndExits
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckLocationNamesAndExits - constructor")
                    .SetCategory("GameController Constructor SpecifyHouseFileName LocationsWithoutHidingPlaces LocationsWithHidingPlaces Exits Success");
                
                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckLocationNamesAndExits - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName LocationsWithoutHidingPlaces LocationsWithHidingPlaces Exits Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileName_CheckLocationsWithHidingPlaces_NamesAndHidingPlaces
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckLocationsWithHidingPlaces_NamesAndHidingPlaces - constructor")
                    .SetCategory("GameController Constructorb SpecifyHouseFileName LocationsWithHidingPlaces Name HidingPlace Success");

                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_SpecifyHouseFileName_CheckLocationsWithHidingPlaces_NamesAndHidingPlaces - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName LocationsWithHidingPlaces Name HidingPlace Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_SpecifyHouseFileName_FullGame_AndCheckMessageAndProperties
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_SpecifyHouseFileName_FullGame_AndCheckMessageAndProperties - constructor")
                    .SetCategory("GameController Constructor SpecifyHouseFileName Move Check Message Prompt Status MoveNumber GameOver InvalidOperationException Success");

                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_SpecifyHouseFileName_FullGame_AndCheckMessageAndProperties - RestartGame")
                    .SetCategory("GameController RestartGame SpecifyHouseFileName Move Check Message Prompt Status MoveNumber GameOver InvalidOperationException Success");
            }
        }
    }
}
