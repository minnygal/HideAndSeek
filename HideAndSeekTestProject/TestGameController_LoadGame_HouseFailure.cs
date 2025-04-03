using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for ParseInput to load game with nonexistent or corrupt House file
    /// </summary>
    [TestFixture]
    public class TestGameController_LoadGame_HouseFailure
    {
        private GameController gameController;
        private string message; // Message returned by GameController's ParseInput method
        private Mock<IFileSystem> mockFileSystem; // Mock file system to be passed to GameController upon creation

        [SetUp]
        public void Setup()
        {
            gameController = null;
            message = null;
            mockFileSystem = new Mock<IFileSystem>(); // Set mock file system variable to new file system
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForNonexistentHouseFile()
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"NonexistentHouse\"" + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("my_saved_game.json", textInFile);

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                "DefaultHouse.json", TestGameController_LoadGame_HouseFailure_TestCaseData.DefaultHouse_Serialized); // Mock default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("NonexistentHouse.json")).Returns(false); // Mock that nonexistent House file does not exist
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object, "DefaultHouse");

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because house layout file NonexistentHouse does not exist"));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid(string exceptionMessageEnding, string fileText)
        {
            Assert.That(GetErrorMessageWhenParseInputToLoadGameWithCorruptHouseFile(fileText), 
            Is.EqualTo("Cannot process because data is corrupt - " +
                                   "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                   exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue(string exceptionMessageEnding, string fileText)
        {
           Assert.That(GetErrorMessageWhenParseInputToLoadGameWithCorruptHouseFile(fileText), 
                        Is.EqualTo("Cannot process because data is corrupt - " +
                                   "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                   exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection(string fileText)
        {
            Assert.That(GetErrorMessageWhenParseInputToLoadGameWithCorruptHouseFile(fileText), 
                        Does.StartWith("Cannot process because data is corrupt - " +
                                       "Cannot process because data in house layout file CorruptHouse is corrupt - The JSON value could not be converted to HideAndSeek.Direction."));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue(string errorMessageEnding, string fileText)
        {
            Assert.That(GetErrorMessageWhenParseInputToLoadGameWithCorruptHouseFile(fileText), 
                        Is.EqualTo($"Cannot process because data is corrupt - Cannot process because data in house layout file CorruptHouse is invalid - {errorMessageEnding}"));
        }

        /// <summary>
        /// Get error message when ParseInput is called to load game from SavedGame
        /// with corrupt House file
        /// </summary>
        /// <param name="houseFileText">Text in corrupt House file</param>
        /// <returns>Error message returned by ParseInput</returns>
        private string GetErrorMessageWhenParseInputToLoadGameWithCorruptHouseFile(string houseFileText)
        {
            // Initialize variable to text in SavedGame file
            string textInSavedGameFile =
            "{" +
                "\"HouseFileName\":\"CorruptHouse\"" + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                TestGameController_LoadGame_HouseFailure_TestCaseData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("my_saved_game.json", textInSavedGameFile);
            
            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                "DefaultHouse.json", TestGameController_LoadGame_HouseFailure_TestCaseData.DefaultHouse_Serialized); // Create mock that returns text for default House file
            houseMockFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseMockFileSystem, "CorruptHouse.json", houseFileText); // Set up mock to return corrupt House file text
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // ParseInput to load SavedGame and return message
            return new GameController(mockFileSystem.Object).ParseInput("load my_saved_game");
        }
    }
}
