using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// TestCaseData for some GameController tests for saving/loading/deleting games
    /// </summary>
    public class SaveGameTests_TestCaseData
    {
        // Text file version of default saved GameController HouseFileName property
        private static readonly string fileText_Property_HouseFileName = "\"HouseFileName\":\"DefaultHouse\"";

        // Text file version of default saved GameController PlayerLocation property
        private static readonly string fileText_Property_PlayerLocation_Entry = "\"PlayerLocation\":\"Entry\"";

        // Text file version of default saved GameController MoveNumber property
        private static readonly string fileText_Property_MoveNumber_1 = "\"MoveNumber\":1";

        // Text file version of default saved GameController MoveNumber property
        private static readonly string fileText_Property_OpponentsAndHidingLocations =
            "\"OpponentsAndHidingLocations\":" +
            "{" +
                "\"Joe\":\"Kitchen\"," +
                "\"Bob\":\"Pantry\"," +
                "\"Ana\":\"Bathroom\"," +
                "\"Owen\":\"Kitchen\"," +
                "\"Jimmy\":\"Pantry\"" +
            "}";

        // Text file version of default saved GameController MoveNumber property
        private static readonly string fileText_Property_FoundOpponents = "\"FoundOpponents\":[]";

        // Text file version of saved GameController returned by StartNewGameWithSpecificHidingInfo_MockFileSystem method
        private static readonly string fileText_GameWithSpecificHidingInfo_NoOpponentsFound = 
            "{" +
                fileText_Property_HouseFileName + "," +
                fileText_Property_PlayerLocation_Entry + "," +
                fileText_Property_MoveNumber_1 + "," +
                fileText_Property_OpponentsAndHidingLocations + "," +
                fileText_Property_FoundOpponents +
            "}";

        /// <summary>
        /// Helper method to start new game with Opponents in specific hiding places and with specified file system
        /// (Solution for TestCaseData not accepting overloaded methods)
        /// </summary>
        /// <param name="fileSystem">File system for GameController to use</param>
        /// <returns>New GameController object set up for game and using specified file system</returns>
        private static GameController StartNewGameWithSpecificHidingInfo_MockFileSystem(IFileSystem fileSystem)
        {
            // Create GameController with specified file system (opponents hidden in random locations with hiding spots)
            GameController gameController = new GameController(fileSystem);

            // Return set up GameController
            return StartNewGameWithSpecificHidingInfo(gameController);
        }

        /// <summary>
        /// Helper method to start new game with Opponents in specific hiding places and with default GameController file system
        /// (Solution for TestCaseData not accepting overloaded methods)
        /// </summary>
        /// <returns>New GameController object set up for game and using default file system</returns>
        private static GameController StartNewGameWithSpecificHidingInfo_DefaultFileSystem()
        {
            // Create GameController with default GameController file system (opponents hidden in random locations with hiding spots)
            GameController gameController = new GameController();

            // Return set up GameController
            return StartNewGameWithSpecificHidingInfo(gameController);
        }

        /// <summary>
        /// Helper method to reset game with Opponents in specific hiding places
        /// </summary>
        /// <param name="gameController">GameController to set up for game</param>
        /// <returns>GameController set up for game</returns>
        private static GameController StartNewGameWithSpecificHidingInfo(GameController gameController)
        {
            // Create enumerable of hiding places for opponents to hide
            IEnumerable<string> hidingPlaces = new List<string>()
            {
                "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry"
            };

            // Hide all opponents in specified locations
            gameController.RehideAllOpponents(hidingPlaces);

            // Return set up GameController
            return gameController;
        }

        // Text file version of saved GameController returned by Find3Opponents method
        private static readonly string fileText_GameWithSpecificHidingInfo_3OpponentsFound = 
            "{" +
                fileText_Property_HouseFileName + "," +
                "\"PlayerLocation\":\"Bathroom\"," +
                "\"MoveNumber\":7," +
                fileText_Property_OpponentsAndHidingLocations + "," +
                "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
            "}";

        /// <summary>
        /// Helper method to start and find 3 opponents in new game with predetermined hiding places and specified file system
        /// (Solution for TestCaseData not accepting overloaded methods)
        /// </summary>
        /// <param name="fileSystem">File system for GameController</param>
        /// <returns>GameController after 3 opponents have been found</returns>
        private static GameController StartNewGameAndFind3Opponents_MockFileSystem(IFileSystem fileSystem)
        {
            // Create game controller with opponents hiding in specific places
            GameController gameController = StartNewGameWithSpecificHidingInfo_MockFileSystem(fileSystem);

            // Find 3 opponents and return game controller
            return Find3Opponents(gameController);
        }

        /// <summary>
        /// Helper method to start and find 3 opponents in new game with predetermined hiding places and default file system
        /// (Solution for TestCaseData not accepting overloaded methods)
        /// </summary>
        /// <returns>GameController after 3 opponents have been found</returns>
        private static GameController StartNewGameAndFind3Opponents_DefaultFileSystem()
        {
            // Create game controller with opponents hiding in specific places
            GameController gameController = StartNewGameWithSpecificHidingInfo_DefaultFileSystem();

            // Find 3 opponents and return game controller
            return Find3Opponents(gameController);
        }

        /// <summary>
        /// Helper method to find 3 opponents (hidden in predetermined places with a StartNewGame method)
        /// </summary>
        /// <param name="gameController">GameController with opponents hidden in specific places</param>
        /// <returns>GameController after 3 opponents have been found</returns>
        private static GameController Find3Opponents(GameController gameController)
        {
            // Go to Kitchen and check to find 2 opponents
            gameController.ParseInput("East");
            gameController.ParseInput("Northwest");
            gameController.ParseInput("check");

            // Go to Bathroom and check to find 1 opponent
            gameController.ParseInput("Southeast");
            gameController.ParseInput("North");
            gameController.ParseInput("check");

            // Return game controller
            return gameController;
        }

        // Test case data for Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile test
        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile
        {
            get
            {
                // No Opponents found
                yield return new TestCaseData(StartNewGameWithSpecificHidingInfo_MockFileSystem)
                    .Returns(fileText_GameWithSpecificHidingInfo_NoOpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - no opponents found");

                // 3 Opponents found
                yield return new TestCaseData(StartNewGameAndFind3Opponents_MockFileSystem)
                    .Returns(fileText_GameWithSpecificHidingInfo_3OpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - 3 opponents found");
            }
        }

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckProperties
        public static IEnumerable Test_GameController_ParseInput_ToLoadGame_AndCheckProperties
        {
            get
            {
                // No Opponents found
                yield return new TestCaseData("Entry", 1, new List<string>(), fileText_GameWithSpecificHidingInfo_NoOpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckProperties - no opponents found");
                
                // 3 Opponents found
                yield return new TestCaseData("Bathroom", 7, new List<string>() { "Joe", "Owen", "Ana" }, fileText_GameWithSpecificHidingInfo_3OpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckProperties - 3 opponents found");
            }
        }

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData
        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData
        {
            get
            {
                // RANDOM ISSUES
                // No data in file
                yield return new TestCaseData("Cannot process because data is corrupt", "")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("Cannot process because data is corrupt", "  ")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("Cannot process because data is corrupt", "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - just characters in file");

                // MISSING KEY/VALUE SET
                // Missing player location
                yield return new TestCaseData("Cannot process because data is corrupt",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing player location");

                // Missing move number
                yield return new TestCaseData("Cannot process because data is corrupt",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("Cannot process because data is corrupt",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("Cannot process because data is corrupt",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - missing found opponents");

                // INVALID VALUE DATA
                // Invalid current location
                yield return new TestCaseData("Cannot process because data is corrupt - invalid CurrentLocation",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            fileText_Property_MoveNumber_1 + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid CurrentLocation");

                // Invalid (negative) move number
                yield return new TestCaseData("Cannot process because data is corrupt - invalid MoveNumber",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            "\"MoveNumber\":-1" + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid MoveNumber");

                // No opponents
                yield return new TestCaseData("Cannot process because data is corrupt - no opponents",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            "\"OpponentsAndHidingLocations\":{}" + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no opponents");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("Cannot process because data is corrupt - invalid hiding location for opponent",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Tree\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            fileText_Property_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("Cannot process because data is corrupt - invalid hiding location for opponent",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Hallway\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            fileText_Property_FoundOpponents +
                        "}")            
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("Cannot process because data is corrupt - found opponent is not an opponent",
                        "{" +
                            fileText_Property_HouseFileName + "," +
                            fileText_Property_PlayerLocation_Entry + "," +
                            fileText_Property_MoveNumber_1 + "," +
                            fileText_Property_OpponentsAndHidingLocations + "," +
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - found opponent Steve is not opponent");
            }
        }
    }
}
