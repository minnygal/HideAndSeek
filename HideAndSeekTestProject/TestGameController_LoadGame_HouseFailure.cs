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
    /// GameController tests for ParseInput LoadGame with nonexistent or corrupt House file
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
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = new Mock<IFileSystem>(); // Create new mock file system for House
            houseMockFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true); // Mock that default House file exists
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(TestGameController_LoadGame_HouseFailure_TestCaseData.DefaultHouse_Serialized); // Mock text in default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("NonexistentHouse.json")).Returns(false); // Mock that nonexistent House file does not exist
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

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
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot process because data is corrupt - " +
                                            "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                            exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue(string exceptionMessageEnding, string fileText)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot process because data is corrupt - " +
                                            "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                            exceptionMessageEnding));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection(string fileText)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Does.StartWith("Cannot process because data is corrupt - " +
                                                "Cannot process because data in house layout file CorruptHouse is corrupt - The JSON value could not be converted to HideAndSeek.Direction."));
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestCaseData),
            nameof(TestGameController_LoadGame_HouseFailure_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue))]
        [Category("GameController Load House Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue(string errorMessageEnding, string fileText)
        {
            // Get GameController
            gameController = GetGameControllerForCorruptHouseFileTests(fileText);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo($"Cannot process because data is corrupt - Cannot process because data in house layout file CorruptHouse is invalid - {errorMessageEnding}"));
        }

        private GameController GetGameControllerForCorruptHouseFileTests(string fileText)
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"CorruptHouse\"" + "," +
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
            "}";

            // Set up mock for GameController file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(textInFile); // Mock what file returns

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = new Mock<IFileSystem>(); // Create new mock file system for House
            houseMockFileSystem.Setup((manager) => manager.File.Exists("DefaultHouse.json")).Returns(true); // Mock that default House file exists
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("DefaultHouse.json")).Returns(TestGameController_LoadGame_HouseFailure_TestCaseData.DefaultHouse_Serialized); // Mock text in default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("CorruptHouse.json")).Returns(true); // Mock that corrupt House file does exist
            houseMockFileSystem.Setup((manager) => manager.File.ReadAllText("CorruptHouse.json")).Returns(fileText); // Mock text in corrupt House file
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Return new game controller (Random not mocked, so truly random hiding places generated)
            return new GameController(mockFileSystem.Object);
        }
    }
}
