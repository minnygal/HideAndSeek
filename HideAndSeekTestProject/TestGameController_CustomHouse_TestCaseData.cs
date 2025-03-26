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
    /// TestCaseData for GameController tests for testing layout of non-default House 
    /// set using GameController constructor or RestartGame method 
    public static class TestGameController_CustomHouse_TestCaseData
    {
        private static readonly string textInHouseFile =
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

        private static GameController GetGameController_WithCustomHouseSetViaConstructor()
        {
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("TestHouse.json", textInHouseFile); // Set House file system to mock
            return new GameController("TestHouse"); // Return new GameController with custom House
        }
        
        private static GameController GetGameController_WithCustomHouseSetViaRestartGame()
        {
            House.FileSystem = new FileSystem(); // Set House file system to default
            GameController gameController = new GameController(); // Create new GameController with default House
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("TestHouse.json", textInHouseFile); // Set House file system to mock
            gameController.RestartGame("TestHouse"); // Restart game with custom House
            return gameController;
        }

        public static IEnumerable TestCases_For_Test_GameController_CustomHouse_NameAndFileNameProperties
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_CustomHouse_NameAndFileNameProperties - constructor")
                    .SetCategory("GameController CustomHouse Constructor");

                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_CustomHouse_NameAndFileNameProperties - RestartGame")
                    .SetCategory("GameController CustomHouse RestartGame");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CustomHouse_LocationNames_AndExits
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_CustomHouse_LocationNames_AndExits - constructor")
                    .SetCategory("GameController CustomHouse Constructor");
                
                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_CustomHouse_LocationNames_AndExits - RestartGame")
                    .SetCategory("GameController CustomHouse RestartGame");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CustomHouse_LocationsWithHidingPlaces_Names_And_HidingPlaces
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_CustomHouse_LocationsWithHidingPlaces_Names_And_HidingPlaces - constructor")
                    .SetCategory("GameController CustomHouse Constructor");

                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_CustomHouse_LocationsWithHidingPlaces_Names_And_HidingPlaces - RestartGame")
                    .SetCategory("GameController CustomHouse RestartGame");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_CustomHouse_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties
        {
            get
            {
                // Set custom House using GameController constructor
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaConstructor())
                    .SetName("Test_GameController_CustomHouse_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties - constructor")
                    .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Success");

                // Set custom House using GameController RestartGame method
                yield return new TestCaseData(GetGameController_WithCustomHouseSetViaRestartGame())
                    .SetName("Test_GameController_CustomHouse_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties - RestartGame")
                    .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Success");
            }
        }
    }
}
