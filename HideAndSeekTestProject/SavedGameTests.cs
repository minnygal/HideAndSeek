using HideAndSeekTestProject;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// SavedGame tests to test setters for SavedGame public properties and basic object serialization/deserialization
    /// Thorough testing of SavedGame deserialization failures is in GameControllerSaveGameTests_TestCaseData and GameControllerSaveGameTests
    /// </summary>
    public class SavedGameTests
    {
        private Dictionary<string, string> validOpponentsAndHidingPlacesDictionary;
        private SavedGame savedGame;

        [SetUp]
        public void SetUp()
        {
            // Set SavedGame to null
            savedGame = null;

            // Initialize dictionary to opponent names and valid hiding places
            validOpponentsAndHidingPlacesDictionary = new Dictionary<string, string>();
            validOpponentsAndHidingPlacesDictionary.Add("Joe", "Kitchen");
            validOpponentsAndHidingPlacesDictionary.Add("Bob", "Bathroom");
        }

        [Test]
        [Category("SavedGame Constructor Success")]
        public void Test_SavedGame_Constructor_WithHouse_AndHouseFileName()
        {
            // Create new House
            House house = TestHouse_Data.GetNewTestHouse();

            // Create SavedGame using parameterized constructor
            savedGame = new SavedGame(house, "TestHouse", "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());

            // Assert that SavedGame properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(savedGame.House, Is.EqualTo(house));
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Count(), Is.EqualTo(validOpponentsAndHidingPlacesDictionary.Count()), "number of opponents and hiding locations items");
                Assert.That(savedGame.FoundOpponents, Is.Empty, "no found opponents");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("my file")]
        [TestCase(" myFile")]
        [TestCase("myFile ")]
        [TestCase("\\")]
        [TestCase("\\myFile")]
        [TestCase("myFile\\")]
        [TestCase("my\\File")]
        [TestCase("/")]
        [TestCase("/myFile")]
        [TestCase("myFile/")]
        [TestCase("my/File")]
        [Category("SavedGame Constructor Failure")]
        public void Test_SavedGame_Constructor_WithHouse_AndHouseFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with an invalid file name raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), fileName, "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"House file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("SavedGame HouseFileName Success")]
        public void Test_SavedGame_Set_HouseFileName()
        {
            // Create SavedGame object with valid House file name
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());

            // Assume no exception is thrown (House loaded successfully from setter)
            // Assert that House file name property's getter returns expected value
            Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("my file")]
        [TestCase(" myFile")]
        [TestCase("myFile ")]
        [TestCase("\\")]
        [TestCase("\\myFile")]
        [TestCase("myFile\\")]
        [TestCase("my\\File")]
        [TestCase("/")]
        [TestCase("/myFile")]
        [TestCase("myFile/")]
        [TestCase("my/File")]
        [Category("SavedGame HouseFileName Failure")]
        public void Test_SavedGame_Set_HouseFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with an invalid file name raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), fileName, "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"House file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCase("Entry")]
        [TestCase("Pantry")]
        [TestCase("Landing")]
        [Category("SavedGame PlayerLocation Success")]
        public void Test_SavedGame_Set_PlayerLocation(string playerLocation)
        {
            // Create SavedGame object with valid player location
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", playerLocation, 1, validOpponentsAndHidingPlacesDictionary, new List<string>());

            // Assert that player location property's getter returns expected value
            Assert.That(savedGame.PlayerLocation, Is.EqualTo(playerLocation));
        }

        [Test]
        [Category("SavedGame PlayerLocation Failure")]
        public void Test_SavedGame_Set_PlayerLocation_AndCheckErrorMessage_ForInvalidValue()
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with invalid player location raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Alaska", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid CurrentLocation"));
            });
        }

        [TestCase(1)]
        [TestCase(52)]
        [TestCase(545)]
        [TestCase(5562)]
        [Category("SavedGame MoveNumber Success")]
        public void Test_SavedGame_Set_MoveNumber(int moveNumber)
        {
            // Create SavedGame object with valid move number
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", moveNumber, validOpponentsAndHidingPlacesDictionary, new List<string>());

            // Assert that move number property's getter returns expected value
            Assert.That(savedGame.MoveNumber, Is.EqualTo(moveNumber));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1000)]
        [Category("SavedGame MoveNumber Failure")]
        public void Test_SavedGame_Set_MoveNumber_AndCheckErrorMessage_ForInvalidValue(int moveNumber)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with invalid move number raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", moveNumber, validOpponentsAndHidingPlacesDictionary, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid MoveNumber"));
            });
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations Success")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces()
        {
            // Create dictionary of valid opponents and hiding places
            Dictionary<string, string> validOpponentsAndHidingPlaces = new Dictionary<string, string>();
            validOpponentsAndHidingPlaces.Add("Joe", "Kitchen");
            validOpponentsAndHidingPlaces.Add("Bob", "Pantry");
            validOpponentsAndHidingPlaces.Add("Ana", "Bathroom");
            validOpponentsAndHidingPlaces.Add("Owen", "Attic");
            validOpponentsAndHidingPlaces.Add("Jimmy", "Garage");

            // Create SavedGame object with valid opponents and hiding places dictionary
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, new List<string>());

            // Assert that opponents and hiding places property's getter returns expected value
            Assert.That(savedGame.OpponentsAndHidingLocations, Is.EquivalentTo(validOpponentsAndHidingPlaces));
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForEmptyDictionary()
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with empty dictionary for OpponentsAndHidingLocations raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, new Dictionary<string, string>(), new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("no opponents"));
            });
        }

        [TestCase("Dungeon", "Living Room", "Attic", "Kitchen", "Pantry")]
        [TestCase("Living Room", "Dungeon", "Attic", "Kitchen", "Pantry")]
        [TestCase("Bathroom", "Second Bathroom", "Dungeon", "Living Room", "Garage")]
        [TestCase("Kids Room", "Nursery", "Master Bathroom", "Dungeon", "Master Bedroom")]
        [TestCase("Kids Room", "Nursery", "Master Bathroom", "Master Bedroom", "Dungeon")]
        [Category("SavedGame OpponentsAndHidingLocations Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForNonexistentLocation
            (string location1, string location2, string location3, string location4, string location5)
        {
            // Create dictionary of opponents with invalid hiding places (nonexistent locations)
            Dictionary<string, string> invalidOpponentsAndHidingPlaces = new Dictionary<string, string>();
            invalidOpponentsAndHidingPlaces.Add("Joe", location1);
            invalidOpponentsAndHidingPlaces.Add("Bob", location2);
            invalidOpponentsAndHidingPlaces.Add("Ana", location3);
            invalidOpponentsAndHidingPlaces.Add("Owen", location4);
            invalidOpponentsAndHidingPlaces.Add("Jimmy", location5);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with nonexistent (invalid) opponent hiding places raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid hiding location for opponent"));
            });
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForLocationWithoutHidingPlace()
        {
            // Create dictionary of opponents with invalid hiding places (locations without hiding places)
            Dictionary<string, string> invalidOpponentsAndHidingPlaces = new Dictionary<string, string>();
            invalidOpponentsAndHidingPlaces.Add("Joe", "Entry");
            invalidOpponentsAndHidingPlaces.Add("Bob", "Landing");
            invalidOpponentsAndHidingPlaces.Add("Ana", "Hallway");
            invalidOpponentsAndHidingPlaces.Add("Owen", "Entry");
            invalidOpponentsAndHidingPlaces.Add("Jimmy", "Landing");

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with opponent hiding locations that don't have hiding places raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid hiding location for opponent"));
            });
        }

        [Test]
        [Category("SavedGame FoundOpponents Success")]
        public void Test_SavedGame_Set_FoundOpponents_ToEmptyList()
        {
            // Create SavedGame object with valid empty list for FoundOpponents
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());

            // Assert that found opponents property's getter returns expected value
            Assert.That(savedGame.FoundOpponents, Is.Empty);
        }

        [TestCase("Joe")]
        [TestCase("Bob")]
        [TestCase("Joe", "Bob")]
        [TestCase("Ana", "Jimmy")]
        [TestCase("Joe", "Bob", "Ana")]
        [TestCase("Jimmy", "Owen", "Bob")]
        [TestCase("Joe", "Bob", "Ana", "Owen", "Jimmy")]
        [TestCase("Ana", "Jimmy", "Bob", "Joe", "Owen")]
        [Category("SavedGame FoundOpponents Success")]
        public void Test_SavedGame_Set_FoundOpponents_ToList(params string[] foundOpponents)
        {
            // Convert array of existing (valid) found opponents to list
            List<string> foundOpponentsAsList = foundOpponents.ToList();

            // Create dictionary of valid opponents and hiding places
            Dictionary<string, string> validOpponentsAndHidingPlaces = new Dictionary<string, string>();
            validOpponentsAndHidingPlaces.Add("Joe", "Kitchen");
            validOpponentsAndHidingPlaces.Add("Bob", "Pantry");
            validOpponentsAndHidingPlaces.Add("Ana", "Bathroom");
            validOpponentsAndHidingPlaces.Add("Owen", "Attic");
            validOpponentsAndHidingPlaces.Add("Jimmy", "Garage");

            // Create SavedGame object with valid found opponents list
            savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, foundOpponentsAsList);

            // Assert that found opponents property's getter returns expected value
            Assert.That(savedGame.FoundOpponents, Is.EquivalentTo(foundOpponentsAsList));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("true")]
        [TestCase("100")]
        [TestCase("@{}({}@/")]
        [TestCase("Methuselah")]
        [TestCase("Methuselah", "Bob")]
        [TestCase("Joe", "Methuselah")]
        [TestCase("Methuselah", "Bob", "Ana")]
        [TestCase("Joe", "Methuselah", "Ana")]
        [TestCase("Joe", "Bob", "Methuselah")]
        [TestCase("Methuselah", "Bob", "Ana", "Owen", "Jimmy")]
        [TestCase("Joe", "Methuselah", "Ana", "Owen", "Jimmy")]
        [TestCase("Joe", "Bob", "Methuselah", "Owen", "Jimmy")]
        [TestCase("Joe", "Bob", "Ana", "Methuselah", "Jimmy")]
        [TestCase("Joe", "Bob", "Ana", "Owen", "Methuselah")]
        [Category("SavedGame FoundOpponents Failure")]
        public void Test_SavedGame_Set_FoundOpponents_AndCheckErrorMessage_ForNonexistentOpponent(params string[] foundOpponents)
        {
            // Convert array of found opponents (including invalid, nonexistent opponent) to list
            List<string> foundOpponentsAsList = foundOpponents.ToList();

            // Create dictionary of valid opponents and hiding places
            Dictionary<string, string> validOpponentsAndHidingPlaces = new Dictionary<string, string>();
            validOpponentsAndHidingPlaces.Add("Joe", "Kitchen");
            validOpponentsAndHidingPlaces.Add("Bob", "Pantry");
            validOpponentsAndHidingPlaces.Add("Ana", "Bathroom");
            validOpponentsAndHidingPlaces.Add("Owen", "Attic");
            validOpponentsAndHidingPlaces.Add("Jimmy", "Garage");

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with nonexistent found opponent raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    savedGame = new SavedGame(TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, foundOpponentsAsList);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("found opponent is not an opponent"));
            });
        }

        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_NoFoundOpponents()
        {
            SavedGame savedGame = new SavedGame(
                TestHouse_Data.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlacesDictionary, new List<string>());

            string serializedGame = JsonSerializer.Serialize(savedGame);

            string expectedSerializedGame =
                "{" +
                    TestHouse_Data.SerializedHouse_HouseFileName + "," +
                    "\"PlayerLocation\":\"Entry\"" + "," +
                    "\"MoveNumber\":1" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Bathroom\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[]" +
                "}";

            Assert.That(serializedGame, Is.EqualTo(expectedSerializedGame));
        }

        [Test]
        [Category("SavedGame Serialize Success")]
        public void Test_SavedGame_Serialize_1FoundOpponent()
        {
            SavedGame savedGame = new SavedGame(
                TestHouse_Data.GetNewTestHouse(), "TestHouse", "Bathroom", 7, validOpponentsAndHidingPlacesDictionary, new List<string>() { "Bob" });

            string serializedGame = JsonSerializer.Serialize(savedGame);

            string expectedSerializedGame =
                "{" +
                    TestHouse_Data.SerializedHouse_HouseFileName + "," +
                    "\"PlayerLocation\":\"Bathroom\"" + "," +
                    "\"MoveNumber\":7" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Bathroom\"" +
                    "}" + "," +
                    "\"FoundOpponents\":[\"Bob\"]" +
                "}";

            Assert.That(serializedGame, Is.EqualTo(expectedSerializedGame));
        }

        [Test]
        [Category("SavedGame Deserialize Success")]
        public void Test_SavedGame_Deserialize()
        {
            // Set mock file system to House property
            House.FileSystem = TestHelperMethods.CreateMockFileSystem_ToReadAllText("TestHouse.json", TestHouse_Data.SerializedTestHouse);

            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
                "}";

            // Attempt to deserialize text from file into SavedGame object
            savedGame = JsonSerializer.Deserialize<SavedGame>(savedGameFileText);

            // Assert that SavedGame properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(savedGame.House.Name, Is.EqualTo("test house"));
                Assert.That(savedGame.House.HouseFileName, Is.EqualTo("TestHouse"));
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "house file name");
                Assert.That(savedGame.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House player starting point");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations.Count(), Is.EqualTo(5), "number of opponents and hiding locations items");
                Assert.That(savedGame.FoundOpponents, Is.Empty, "no found opponents");
            });
        }



        [Test]
        [Category("SavedGame Deserialize Failure")]
        public void Test_SavedGame_Deserialize_AndCheckErrorMessage_ForNullReferenceException_ForMissingHouseFileName()
        {
            string textInFile =
                "{" +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    TestSavedGame_Data.SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
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