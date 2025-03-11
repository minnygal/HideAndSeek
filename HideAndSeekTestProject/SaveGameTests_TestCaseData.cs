using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// TestCaseData for some GameController tests for saving/loading/deleting games and SavedGame tests
    /// </summary>
    public class SaveGameTests_TestCaseData
    {
        // Text file version of saved GameController returned by StartNewGameWithSpecificHidingInfo_MockFileSystem method
        private static readonly string fileText_GameWithSpecificHidingInfo_NoOpponentsFound = "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}";

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
            // Create enumerable of places to hide opponents
            IEnumerable<LocationWithHidingPlace> hidingPlaces = new List<LocationWithHidingPlace>()
            {
                gameController.House.GetLocationWithHidingPlaceByName("Kitchen"),
                gameController.House.GetLocationWithHidingPlaceByName("Pantry"),
                gameController.House.GetLocationWithHidingPlaceByName("Bathroom"),
                gameController.House.GetLocationWithHidingPlaceByName("Kitchen"),
                gameController.House.GetLocationWithHidingPlaceByName("Pantry")
            };

            // Hide all Opponents in specific hiding places
            gameController.RehideAllOpponents(hidingPlaces);

            // Return set up GameController
            return gameController;
        }

        // Text file version of saved GameController returned by Find3Opponents method
        private static readonly string fileText_GameWithSpecificHidingInfo_3OpponentsFound = "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"Bathroom\",\"MoveNumber\":7,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]}";

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
        public static IEnumerable TestCases_For_Test_GameController_ParseInput_AndCheckTextSavedToFile
        {
            get
            {
                yield return new TestCaseData(StartNewGameWithSpecificHidingInfo_MockFileSystem)
                    .Returns(fileText_GameWithSpecificHidingInfo_NoOpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - no opponents found");
                yield return new TestCaseData(StartNewGameAndFind3Opponents_MockFileSystem)
                    .Returns(fileText_GameWithSpecificHidingInfo_3OpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile - 3 opponents found");
            }
        }

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckGameIsLoadedSuccessfully
        public static IEnumerable TestCases_For_Test_GameController_ParseInput_LoadGame_AndCheckGameIsLoadedSuccessfully
        {
            get
            {
                yield return new TestCaseData("StartingPoint", 1, new List<string>(), fileText_GameWithSpecificHidingInfo_NoOpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckGameIsLoadedSuccessfully - no opponents found");
                yield return new TestCaseData("Bathroom", 7, new List<string>() { "Joe", "Owen", "Ana" }, fileText_GameWithSpecificHidingInfo_3OpponentsFound)
                    .SetName("Test_GameController_ParseInput_ToLoadGame_AndCheckGameIsLoadedSuccessfully - 3 opponents found");
            }
        }

        // Test case data for Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataIsInvalid
        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenFileDataIsInvalid
        {
            get
            {
                // RANDOM ISSUES
                // No data in file
                yield return new TestCaseData("Cannot process because data is corrupt", "")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("Cannot process because data is corrupt", "  ")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("Cannot process because data is corrupt", "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - just characters in file");

                // MISSING KEY/VALUE SET
                // Missing current location
                yield return new TestCaseData("Cannot process because data is corrupt",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - missing current location");

                // Missing move number
                yield return new TestCaseData("Cannot process because data is corrupt",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[\"Joe\",\"Bob\",\"Ana\",\"Owen\",\"Jimmy\"]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - missing move number");

                // Missing all opponents
                yield return new TestCaseData("Cannot process because data is corrupt",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - missing all opponents");

                // Missing found opponents
                yield return new TestCaseData("Cannot process because data is corrupt",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"}}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - missing found opponents");

                // INVALID VALUE DATA
                // Invalid current location
                yield return new TestCaseData("Cannot process because data is corrupt - invalid CurrentLocation",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"Tree\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - invalid CurrentLocation");

                // Invalid (negative) move number
                yield return new TestCaseData("Cannot process because data is corrupt - invalid MoveNumber",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":-1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - invalid MoveNumber");

                // No opponents
                yield return new TestCaseData("Cannot process because data is corrupt - no opponents",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - no opponents");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("Cannot process because data is corrupt - invalid hiding location for opponent",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Tree\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("Cannot process because data is corrupt - invalid hiding location for opponent",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Hallway\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("Cannot process because data is corrupt - found opponent is not an opponent",
                                              "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"StartingPoint\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[\"Steve\"]}")
                    .SetName("Test_GameController_ParseInput_ToLoadGame_CheckErrorMessage_FileDataInvalid - found opponent Steve is not opponent");
            }
        }
    }
}
