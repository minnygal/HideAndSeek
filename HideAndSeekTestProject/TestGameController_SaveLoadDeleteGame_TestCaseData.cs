using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// TestCaseData for some GameController tests for saving/loading/deleting games
    /// </summary>
    public class TestGameController_SaveLoadDeleteGame_TestCaseData
    {
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
            // Hide all opponents in specified locations
            gameController.RehideAllOpponents(MyTestSavedGame.OpponentsAndHidingPlaces.Values);

            // Return set up GameController
            return gameController;
        }

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
                yield return new TestCaseData(StartNewGameWithSpecificHidingInfo_MockFileSystem, MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - no opponents found");

                // 3 Opponents found
                yield return new TestCaseData(StartNewGameAndFind3Opponents_MockFileSystem, MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - 3 opponents found");
            }
        }

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckProperties
        public static IEnumerable Test_GameController_ParseInput_ToLoadGame_AndCheckProperties
        {
            get
            {
                // No Opponents found
                yield return new TestCaseData("Entry", 1, new List<string>(), MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents)
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckProperties - no opponents found");
                
                // 3 Opponents found
                yield return new TestCaseData("Bathroom", 7, new List<string>() { "Joe", "Owen", "Ana" }, MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents)
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
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.", "")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 2.", "  ")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.", "ABCDeaoueou[{}}({}")
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

                // Invalid current location
                yield return new TestCaseData("invalid CurrentLocation",
                        "{" +
                            MyTestSavedGame.SerializedTestSavedGame_HouseFileName + "," +
                            "\"PlayerLocation\":\"Tree\"," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                            MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                            MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                        "}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData - invalid CurrentLocation");

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
