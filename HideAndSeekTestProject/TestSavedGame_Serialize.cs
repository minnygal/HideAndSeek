using Moq;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// SavedGame unit tests for serialization
    /// </summary>
    [TestFixture]
    public class TestSavedGame_Serialize
    {
        // Tests all properties' getters
        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_NoFoundOpponents()
        {
            // Initialize variable to expected text for serialized SavedGame
            string expectedSavedGameText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
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

            // Create SavedGame object
            SavedGame savedGame = new SavedGame(GetMockedHouse(), "DefaultHouse", "Entry", 1,
                                                new Dictionary<string, string>()
                                                {
                                                    { "Joe", "Kitchen" },
                                                    { "Bob", "Pantry" },
                                                    { "Ana", "Bathroom" },
                                                    { "Owen", "Kitchen" },
                                                    { "Jimmy", "Pantry" }
                                                },
                                                new List<string>());

            // Serialize SavedGame object
            string serializedGame = JsonSerializer.Serialize(savedGame);

            // Assert that serialized SavedGame object is as expected
            Assert.That(serializedGame, Is.EqualTo(expectedSavedGameText));
        }

        // Tests all properties' getters
        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_3FoundOpponents()
        {
            // Initialize variable to expected text for serialized SavedGame
            string expectedSavedGameText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Bathroom\"" + "," +
                    "\"MoveNumber\":7" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
                "}";

            // Create SavedGame object
            SavedGame savedGame = new SavedGame(GetMockedHouse(),
                                                "DefaultHouse", "Bathroom", 7,
                                                new Dictionary<string, string>()
                                                {
                                                    { "Joe", "Kitchen" },
                                                    { "Bob", "Pantry" },
                                                    { "Ana", "Bathroom" },
                                                    { "Owen", "Kitchen" },
                                                    { "Jimmy", "Pantry" }
                                                },
                                                new List<string>() { "Joe", "Owen", "Ana" });

            // Serialize SavedGame object
            string serializedGame = JsonSerializer.Serialize(savedGame);

            // Assert that serialized SavedGame text is as expected
            Assert.That(serializedGame, Is.EqualTo(expectedSavedGameText));
        }

        /// <summary>
        /// Helper method to get mocked House object
        /// (DoesLocationExist and DoesLocationWithHidingPlaceExist always return true)
        /// </summary>
        /// <returns>Mocked House object</returns>
        private static House GetMockedHouse()
        {
            Mock<House> houseMock = new Mock<House>();
            houseMock.Setup((h) => h.DoesLocationExist(It.IsAny<string>())).Returns(true); // Set up mock to return true for any location
            houseMock.Setup((h) => h.DoesLocationWithHidingPlaceExist(It.IsAny<string>())).Returns(true); // Set up mock to return true for any location
            return houseMock.Object;
        }
    }
}