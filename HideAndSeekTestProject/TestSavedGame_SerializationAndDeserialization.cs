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
    /// SavedGame tests to test serialization and deserialization
    /// </summary>
    [TestFixture]
    public class TestSavedGame_SerializationAndDeserialization
    {
        private SavedGame savedGame;

        [SetUp]
        public void SetUp()
        {
            savedGame = null;
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        // Tests all properties' getters
        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_NoFoundOpponents()
        {
            string serializedGame = JsonSerializer.Serialize(MyTestSavedGame.GetNewTestSavedGame_NoFoundOpponents());
            Assert.That(serializedGame, Is.EqualTo(MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents));
        }

        // Tests all properties' getters
        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_3FoundOpponents()
        {
            string serializedGame = JsonSerializer.Serialize(MyTestSavedGame.GetNewTestSavedGame_3FoundOpponents());
            Assert.That(serializedGame, Is.EqualTo(MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents));
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Success")]
        public void Test_SavedGame_Deserialize_NoFoundOpponents()
        {
            // Set mock file system to House property
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("TestHouse.json", MyTestHouse.SerializedTestHouse);

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                "}";

            // Attempt to deserialize text from file into SavedGame object
            savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameFileText);

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that SavedGame properties are as expected
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Key),
                    Is.EquivalentTo(MyTestSavedGame.OpponentsAndHidingPlaces.Select((kvp) => kvp.Key)), "names of opponents in opponents and hiding locations dictionary");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Value),
                    Is.EquivalentTo(MyTestSavedGame.OpponentsAndHidingPlaces.Select((kvp) => kvp.Value)), "names of hiding locations in opponents and hiding locations dictionary");
                Assert.That(savedGame.FoundOpponents, Is.Empty, "no found opponents");

                // Assert that House properties are as expected
                Assert.That(savedGame.House.Name, Is.EqualTo("test house"));
                Assert.That(savedGame.House.HouseFileName, Is.EqualTo("TestHouse"));
                Assert.That(savedGame.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House player starting point");
            });
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Success")]
        public void Test_SavedGame_Deserialize_3FoundOpponents()
        {
            // Set mock file system to House property
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("TestHouse.json", MyTestHouse.SerializedTestHouse);

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_PlayerLocation + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_MoveNumber + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_FoundOpponents +
                "}";

            // Attempt to deserialize text from file into SavedGame object
            savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameFileText);

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that SavedGame properties are as expected
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Bathroom"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(7), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Key),
                    Is.EquivalentTo(MyTestSavedGame.OpponentsAndHidingPlaces.Select((kvp) => kvp.Key)), "names of opponents in opponents and hiding locations dictionary");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Value),
                    Is.EquivalentTo(MyTestSavedGame.OpponentsAndHidingPlaces.Select((kvp) => kvp.Value)), "names of hiding locations in opponents and hiding locations dictionary");
                Assert.That(savedGame.FoundOpponents.Count(), Is.EqualTo(3), "3 found opponents");
                Assert.That(savedGame.FoundOpponents, Is.EquivalentTo(MyTestSavedGame.FoundOpponents_3FoundOpponents), "names of found opponents");

                // Assert that House properties are as expected
                Assert.That(savedGame.House.Name, Is.EqualTo("test house"));
                Assert.That(savedGame.House.HouseFileName, Is.EqualTo("TestHouse"));
                Assert.That(savedGame.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House player starting point");
            });
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForNullReferenceException_ForMissingHouseFileName()
        {
            string textInFile =
                "{" +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that deserializing with a missing house file name property raises an exception
                var exception = Assert.Throws<NullReferenceException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("House has not been set"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens))]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens(string expectedErrorMessage, string textInFile)
        {
            Assert.Multiple(() =>
            {
                // Assert that deserialization raises an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing))]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing(string expectedErrorMessage, string textInFile)
        {
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("DefaultHouse.json", MyTestHouse.SerializedTestHouse);

            Assert.Multiple(() =>
            {
                // Assert that deserialization raises an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue))]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue(string expectedErrorMessage, string textInFile)
        {
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("DefaultHouse.json", MyTestHouse.SerializedTestHouse);

            Assert.Multiple(() =>
            {
                // Assert that deserialization raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(textInFile);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo(expectedErrorMessage));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid(string exceptionMessageEnding, string houseFileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with an invalid file format throws an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file DefaultHouse is corrupt - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue(string exceptionMessageEnding, string houseFileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with invalid file data of whitespace value throws an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents);
                });
                
                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file DefaultHouse is invalid - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_Deserialization_TestCaseData), nameof(TestSavedGame_Deserialization_TestCaseData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection(string houseFileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.CreateMockFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with an invalid Direction throws an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(MyTestSavedGame.SerializedTestSavedGame_NoFoundOpponents);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data in house layout file DefaultHouse is corrupt - The JSON value could not be converted to HideAndSeek.Direction."));
            });
        }
    }
}
