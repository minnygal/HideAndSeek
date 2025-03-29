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
    /// TestCaseData for GameController basic tests
    /// </summary>
    public static class TestGameController_Basic_TestCaseData
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

        public static IEnumerable TestCases_For_Test_GameController_CheckErrorMessage_ForInvalidHouseFileName
        {
            get
            {
                yield return new TestCaseData(() =>
                {
                    new GameController("@eou]} {(/"); // Call GameController constructor
                })
                    .SetName("Test_GameController_CheckErrorMessage_ForInvalidHouseFileName - constructor")
                    .SetCategory("GameController Constructor Failure");

                yield return new TestCaseData(() =>
                {
                    new GameController().RestartGame("@eou]} {(/"); // Create new GameController and call RestartGame
                })
                    .SetName("Test_GameController_CheckErrorMessage_ForInvalidHouseFileName - RestartGame")
                    .SetCategory("GameController RestartGame Failure");
            }
        }

        /// <summary>
        /// Helper method to set House file system to mock that file does not exist
        /// </summary>
        private static void SetUpMockFileSystemForNonexistentHouseFile()
        {
            Mock<IFileSystem> fileSystem = new Mock<IFileSystem>();
            fileSystem.Setup((manager) => manager.File.Exists("MyNonexistentFile.json")).Returns(false);
            House.FileSystem = fileSystem.Object;
        }

        public static IEnumerable TestCases_For_Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist
        {
            get
            {
                yield return new TestCaseData(() =>
                    {
                        SetUpMockFileSystemForNonexistentHouseFile(); // Set up mock file system
                        new GameController("MyNonexistentFile"); // Call GameController constructor
                    })
                    .SetName("Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist - constructor")
                    .SetCategory("GameController Constructor Failure");

                yield return new TestCaseData(() =>
                    {
                        GameController gameController = new GameController("DefaultHouse"); // Create new GameController
                        SetUpMockFileSystemForNonexistentHouseFile(); // Set up mock file system
                        gameController.RestartGame("MyNonexistentFile"); // Call RestartGame
                    })
                    .SetName("Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist - RestartGame")
                    .SetCategory("GameController RestartGame Failure");
            }
        }
    }
}
