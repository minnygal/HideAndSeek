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
    /// (only relevant to LoadGame overload accepting name of file storing serialized SavedGame)
    /// 
    /// These are integration tests using SavedGame, House, Location, and LocationWithHidingPlace
    /// </summary>
    [TestFixture]
    public class TestGameController_LoadGame_HouseFailure
    {
        private GameController gameController;
        private Exception exception;
        private static readonly Opponent[] mockedOpponent = new Opponent[] { new Mock<Opponent>().Object };

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

        /// <summary>
        /// Get GameController created with minimal House object and mocked Opponent
        /// </summary>
        /// <returns>GameController created with minimal House object and mocked Opponent</returns>
        private static GameController GetMinimalGameController()
        {
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Office", "under the table"); // Create a location with hiding place
            House house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            return new GameController(mockedOpponent, house); // Create GameController with House
        }

        [Test]
        [Category("GameController LoadGame House FileNotFoundException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_ForNonexistentHouseFile()
        {
            // Initialize variable to text in SavedGame file
            string textInFile =
            "{" +
                "\"HouseFileName\":\"NonexistentHouse\"" + "," +
                "\"PlayerLocation\":\"Entry\"" + "," +
                "\"MoveNumber\":1" + "," +
                "\"OpponentsAndHidingLocations\":" +
                "{" +
                    "\"Joe\":\"Kitchen\"," +
                    "\"Bob\":\"Pantry\"," +
                    "\"Ana\":\"Bathroom\"," +
                    "\"Owen\":\"Kitchen\"," +
                    "\"Jimmy\":\"Pantry\"" +
                "}" + "," +
                "\"FoundOpponents\":[]" +
            "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.game.json", textInFile);

            // Set up mock for House file system
            Mock<IFileSystem> houseMockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                "DefaultHouse.house.json", TestGameController_LoadGame_HouseFailure_TestData.DefaultHouse_Serialized); // Mock default House file
            houseMockFileSystem.Setup((manager) => manager.File.Exists("NonexistentHouse.json")).Returns(false); // Mock that nonexistent House file does not exist
            House.FileSystem = houseMockFileSystem.Object; // Set House file system to mock file system

            // Get game controller
            gameController = GetMinimalGameController();

            Assert.Multiple(() =>
            {
                // Assert that loading game with nonexistent House file raises exception
                exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    gameController.LoadGame("my_saved_game");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot load game because house layout file NonexistentHouse does not exist"));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid))]
        [Category("GameController LoadGame House JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileFormatIsInvalid(
            string exceptionMessageEnding, string fileText)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading game with House file with invalid format raises exception
                exception = Assert.Throws<JsonException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptHouseFile(fileText);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot process because data is corrupt - " +
                                                          "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                          exceptionMessageEnding));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue))]
        [Category("GameController LoadGame House ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasWhitespaceValue(
            string exceptionMessageEnding, string fileText)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading game with House file with invalid whitespace value raises exception
                exception = Assert.Throws<ArgumentException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptHouseFile(fileText);
                });
                
                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                              "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                                              exceptionMessageEnding));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection))]
        [Category("GameController LoadGame House JsonException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidDirection(string fileText)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading game with House file with invalid Direction value raises exception
                exception = Assert.Throws<JsonException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptHouseFile(fileText);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                              "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                              "The JSON value could not be converted to HideAndSeek.Direction."));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue))]
        [Category("GameController LoadGame House ArgumentException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue(
            string errorMessageEnding, string fileText)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading game with House file with invalid value raises exception
                exception = Assert.Throws<ArgumentException>(() =>
                {
                    GetExceptionWhenLoadGameWithCorruptHouseFile(fileText);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                              "Cannot process because data in house layout file CorruptHouse is invalid - " +
                                                              errorMessageEnding));
            });
        }

        [TestCaseSource(typeof(TestGameController_LoadGame_HouseFailure_TestData),
            nameof(TestGameController_LoadGame_HouseFailure_TestData.TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation))]
        [Category("GameController LoadGame House InvalidOperationException Failure")]
        public void Test_GameController_LoadGame_AndCheckErrorMessage_WhenHouseFileDataHasInvalidValue_NonexistentLocation(
            string errorMessageEnding, string fileText)
        {
            Assert.Multiple(() =>
            {
                // Assert that loading game with House file with nonexistent location value raises exception
                exception = Assert.Throws<InvalidOperationException>(() =>
                { 
                    GetExceptionWhenLoadGameWithCorruptHouseFile(fileText);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data is corrupt - " +
                                                              "Cannot process because data in house layout file CorruptHouse is corrupt - " +
                                                              errorMessageEnding));
            });
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
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            // Set up mock for GameController file system
            GameController.FileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText("my_saved_game.game.json", textInSavedGameFile).Object;
            
            // Set up mock for House file system
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("CorruptHouse.house.json", houseFileText); // Set House file system to mock file system

            // Set game controller
            gameController = GetMinimalGameController();

            // Load game from SavedGame with corrupt House file
            gameController.LoadGame("my_saved_game"); // Should throw exception

            // If exception not thrown, fail test
            Assert.Fail();
        }
    }
}