using System;
using System.Collections.Generic;
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
    public class TestSavedGame_Serialization
    {
        private SavedGame savedGame;

        [SetUp]
        public void SetUp()
        {
            // Set SavedGame to null
            savedGame = null;
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
    }
}
