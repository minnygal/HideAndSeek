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
    /// SavedGame tests for test serialization and deserialization
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
            SavedGame savedGame = new SavedGame(TestSavedGame_SerializationAndDeserialization_TestData.GetDefaultHouse(), 
                                                "DefaultHouse", "Entry", 1, 
                                                TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces, 
                                                new List<string>());
            string serializedGame = JsonSerializer.Serialize(savedGame);
            Assert.That(serializedGame, Is.EqualTo(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_NoFoundOpponents));
        }

        // Tests all properties' getters
        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_3FoundOpponents()
        {
            // Initialize variable to expected text for serialized SavedGame
            string expectedSavedGameText =
                "{" +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_HouseFileName + "," +
                    "\"PlayerLocation\":\"Bathroom\"" + "," +
                    "\"MoveNumber\":7" + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                    "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
                "}";
            
            // Create SavedGame object
            SavedGame savedGame = new SavedGame(TestSavedGame_SerializationAndDeserialization_TestData.GetDefaultHouse(), 
                                                "DefaultHouse", "Bathroom", 7, 
                                                TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces,
                                                new List<string>() { "Joe", "Owen", "Ana" });
            
            // Serialize SavedGame object
            string serializedGame = JsonSerializer.Serialize(savedGame);
            
            // Assert that serialized SavedGame text is as expected
            Assert.That(serializedGame, Is.EqualTo(expectedSavedGameText));
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Success")]
        public void Test_SavedGame_Deserialize_NoFoundOpponents()
        {
            // Set mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", 
                                    TestSavedGame_SerializationAndDeserialization_TestData.DefaultHouse_Serialized);

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_HouseFileName + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                "}";

            // Attempt to deserialize text from file into SavedGame object
            savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameFileText);

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that SavedGame properties are as expected
                Assert.That(savedGame.HouseFileName, Is.EqualTo("DefaultHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Key),
                    Is.EquivalentTo(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces.Select((kvp) => kvp.Key)), "names of opponents in opponents and hiding locations dictionary");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Value),
                    Is.EquivalentTo(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces.Select((kvp) => kvp.Value)), "names of hiding locations in opponents and hiding locations dictionary");
                Assert.That(savedGame.FoundOpponents, Is.Empty, "no found opponents");

                // Assert that House properties are as expected
                Assert.That(savedGame.House.Name, Is.EqualTo("my house"));
                Assert.That(savedGame.House.HouseFileName, Is.EqualTo("DefaultHouse"));
                Assert.That(savedGame.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House player starting point");
            });
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Success")]
        public void Test_SavedGame_Deserialize_3FoundOpponents()
        {
            // Set mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", 
                                    TestSavedGame_SerializationAndDeserialization_TestData.DefaultHouse_Serialized);

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_HouseFileName + "," +
                    "\"PlayerLocation\":\"Bathroom\"" + "," +
                    "\"MoveNumber\":7" + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                    "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
                "}";

            // Attempt to deserialize text from file into SavedGame object
            savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameFileText);

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that SavedGame properties are as expected
                Assert.That(savedGame.HouseFileName, Is.EqualTo("DefaultHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Bathroom"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(7), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Key),
                    Is.EquivalentTo(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces.Select((kvp) => kvp.Key)), "names of opponents in opponents and hiding locations dictionary");
                Assert.That(savedGame.OpponentsAndHidingLocations.Select((kvp) => kvp.Value),
                    Is.EquivalentTo(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_OpponentsAndHidingPlaces.Select((kvp) => kvp.Value)), "names of hiding locations in opponents and hiding locations dictionary");
                Assert.That(savedGame.FoundOpponents.Count(), Is.EqualTo(3), "3 found opponents");
                Assert.That(savedGame.FoundOpponents, Is.EquivalentTo(new List<string>() { "Joe", "Owen", "Ana" }), "names of found opponents");

                // Assert that House properties are as expected
                Assert.That(savedGame.House.Name, Is.EqualTo("my house"));
                Assert.That(savedGame.House.HouseFileName, Is.EqualTo("DefaultHouse"));
                Assert.That(savedGame.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House player starting point");
            });
        }

        // Tests all properties' setters and House getter
        [Test]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForNullReferenceException_ForMissingHouseFileName()
        {
            // Initialize variable to text representing serialized SavedGame
            string textInFile =
                "{" +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                    TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
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

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenNoJsonTokens))]
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

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing))]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenPropertyIsMissing(string expectedErrorMessage, string textInFile)
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", 
                                    TestSavedGame_SerializationAndDeserialization_TestData.DefaultHouse_Serialized);

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

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue))]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue(string expectedErrorMessage, string textInFile)
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", 
                                    TestSavedGame_SerializationAndDeserialization_TestData.DefaultHouse_Serialized);

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

        [Test]
        [Category("SavedGame Deserialize ArgumentException Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenFileDataHasInvalidValue_ForHouseFileName()
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json",
                                    TestSavedGame_SerializationAndDeserialization_TestData.DefaultHouse_Serialized);

            Assert.Multiple(() =>
            {
                // Assert that deserialization raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(
                        "{" +
                            "\"HouseFileName\":\"a8}{{ /@uaou12 \"" + "," +
                            TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_PlayerLocation_NoFoundOpponents + "," +
                            TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_MoveNumber_NoFoundOpponents + "," +
                            TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_OpponentsAndHidingLocations + "," +
                            TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_FoundOpponents_NoFoundOpponents +
                        "}");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot perform action because file name \"a8}{{ /@uaou12 \" is invalid " +
                                                          "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileFormatIsInvalid(string exceptionMessageEnding, string houseFileText)
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with an invalid file format throws an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_NoFoundOpponents);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file DefaultHouse is corrupt - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForInvalidDataException_WhenHouseFileDataInvalidValue(string exceptionMessageEnding, string houseFileText)
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with invalid file data raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_NoFoundOpponents);
                });
                
                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file DefaultHouse is invalid - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_SerializationAndDeserialization_TestData), nameof(TestSavedGame_SerializationAndDeserialization_TestData.TestCases_For_Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection))]
        [Category("SavedGame Deserialize House Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForJsonException_WhenHouseFileDataHasInvalidDirection(string houseFileText)
        {
            // Set House file system to mock file system to return text in file
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", houseFileText);

            Assert.Multiple(() =>
            {
                // Assert that deserializing a SavedGame with a reference to a House with an invalid Direction raises an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    JsonSerializer.Deserialize<SavedGame>(TestSavedGame_SerializationAndDeserialization_TestData.SavedGame_Serialized_NoFoundOpponents);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot process because data in house layout file DefaultHouse is corrupt - " +
                                                              "The JSON value could not be converted to HideAndSeek.Direction."));
            });
        }
    }
}
