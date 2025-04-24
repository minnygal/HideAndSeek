using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for LoadGame with nonexistent or corrupt House file
    /// (integration tests using SavedGame, House, Location, and LocationWithHidingPlace)
    /// </summary>
    [TestFixture]
    public class TestGameController_LoadGame_HouseFailure
    {
        private GameController gameController;
        private Exception exception;

        [SetUp]
        public void Setup()
        {
            gameController = null;
            exception = null;
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            GameController.FileSystem = new FileSystem(); // Set static GameController file system to new file system
        }

        [Test]
        [Category("GameController LoadGame House FileNotFoundException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForNonexistentHouseFile()
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"NonexistentHouse\"" + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
            "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("my_saved_game.game.json", textInFile).Object;

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                "DefaultHouse.house.json", TestGameController_LoadGame_HouseFailure_TestData.DefaultHouse_Serialized); // Mock default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("NonexistentHouse.json")).Returns(false); // Mock that nonexistent House file does not exist
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Create new game controller
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, "DefaultHouse");

            // Assert that loading game with nonexistent House file raises exception
            exception = Assert.Throws<FileNotFoundException>(() => gameController.LoadGame("my_saved_game"));

            // Assert that exception message is as expected
            Assert.That(exception.Message, Is.EqualTo("Cannot load game because house layout file NonexistentHouse does not exist"));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid))]
        [Category("GameController LoadGame House JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid(
            string exceptionMessageEnding, string fileText)
        {
            exception = Assert.Throws<JsonException>(() => GetExceptionWhenLoadGameWithCorruptHouseFile(fileText));
            Assert.That(exception.Message, Is.EqualTo("Cannot process because data is corrupt - " +
                                                      "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                      exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue))]
        [Category("GameController LoadGame House ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue(
            string exceptionMessageEnding, string fileText)
        {
            exception = Assert.Throws<ArgumentException>(() => GetExceptionWhenLoadGameWithCorruptHouseFile(fileText));
            Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                      "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                                      exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection))]
        [Category("GameController LoadGame House JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection(string fileText)
        {
            exception = Assert.Throws<JsonException>(() => GetExceptionWhenLoadGameWithCorruptHouseFile(fileText));
            Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                          "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                          "The JSON value could not be converted to HideAndSeek.Direction."));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue))]
        [Category("GameController LoadGame House ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue(
            string errorMessageEnding, string fileText)
        {
            exception = Assert.Throws<ArgumentException>(() => GetExceptionWhenLoadGameWithCorruptHouseFile(fileText));
            Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                      "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                                      errorMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation))]
        [Category("GameController LoadGame House InvalidOperationException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation(
            string errorMessageEnding, string fileText)
        {
            exception = Assert.Throws<InvalidOperationException>(() => GetExceptionWhenLoadGameWithCorruptHouseFile(fileText));
            Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                      "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                      errorMessageEnding));
        }

        /// <summary>
        /// Call LoadGame to load game from saved game file referencing corrupt House file
        /// Fails test if exception not thrown
        /// </summary>
        /// <param name="houseFileText">Text in corrupt House file</param>
        private void GetExceptionWhenLoadGameWithCorruptHouseFile(string houseFileText)
        {
            // Initialize variable to text in SavedGame file
            string textInSavedGameFile =
            "{" +
                "\"HouseFileName\":\"CorruptHouse\"" + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_PlayerLocation_NoOpponentsGame + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                TestGameController_LoadGame_HouseFailure_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
            "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("my_saved_game.game.json", textInSavedGameFile).Object;
            
            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                "DefaultHouse.house.json", TestGameController_LoadGame_HouseFailure_TestData.DefaultHouse_Serialized); // Create mock that returns text for default House file
            houseMockFileSystem = MockFileSystemHelper.SetMockOfFileSystem_ToReadAllText(houseMockFileSystem, "CorruptHouse.house.json", houseFileText); // Set up mock to return corrupt House file text
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Load game from SavedGame with corrupt House file
            new GameController(new Opponent[] { new Mock<Opponent>().Object }, "DefaultHouse").LoadGame("my_saved_game"); // Should throw exception

            // If exception not thrown, fail test
            Assert.Fail();
        }
    }
}