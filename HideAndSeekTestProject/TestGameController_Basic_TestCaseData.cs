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
    public static class TestGameController_Basic_TestCaseData
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
            // Set up mock file system
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
                        GameController gameController = new GameController(); // Create new GameController
                        SetUpMockFileSystemForNonexistentHouseFile(); // Set up mock file system
                        gameController.RestartGame("MyNonexistentFile"); // Call RestartGame
                    })
                    .SetName("Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist - RestartGame")
                    .SetCategory("GameController RestartGame Failure");
            }
        }
    }
}
