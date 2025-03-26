using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// SavedGame tests to test SavedGame constructors and SavedGame public properties
    /// </summary>
    [TestFixture]
    public class TestSavedGame_Basic
    {
        private SavedGame savedGame;

        [SetUp]
        public void SetUp()
        {
            // Set SavedGame to null
            savedGame = null;
        }

        // Tests all setters except House and HouseFileName (accesses House and HouseFileName propertyes' backing fields)
        [Test]
        [Category("SavedGame Constructor Success")]
        public void Test_SavedGame_Constructor_WithHouse_AndHouseFileName()
        {
            // Create new House
            House house = MyTestHouse.GetNewTestHouse();

            // Create SavedGame using parameterized constructor
            savedGame = new SavedGame(house, "TestHouse", "Entry", 1, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());

            // Assert that SavedGame properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(savedGame.House, Is.EqualTo(house));
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "house file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations, Is.EquivalentTo(MyTestSavedGame.OpponentsAndHidingPlaces), "opponents and hiding locations");
                Assert.That(savedGame.FoundOpponents, Is.Empty, "no found opponents");
            });
        }

        // Does NOT test House and HouseFileName setters
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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), fileName, "Entry", 1, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"House file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("SavedGame House Failure")]
        public void Test_SavedGame_Set_House_AndCheckErrorMessage_ForAlreadyHasValue()
        {
            // Create new SavedGame (giving House property's backing field a value)
            savedGame = MyTestSavedGame.GetNewTestSavedGame_NoFoundOpponents();

            Assert.Multiple(() =>
            {
                // Assert that setting House property to another value raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame.House = new Mock<House>().Object;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("House property already has a value"));
            });
        }

        [Test]
        [Category("SavedGame HouseFileName Failure")]
        public void Test_SavedGame_Set_HouseFileName_AndCheckErrorMessage_ForAlreadyHasValue()
        {
            // Create new SavedGame (giving HouseFileName property's backing field a value)
            savedGame = MyTestSavedGame.GetNewTestSavedGame_NoFoundOpponents();

            Assert.Multiple(() =>
            {
                // Assert that setting HouseFileName property to another value raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame.HouseFileName = "NewFileName";
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("HouseFileName property already has a value"));
            });
        }

        [TestCase("Entry")]
        [TestCase("Pantry")]
        [TestCase("Landing")]
        [Category("SavedGame PlayerLocation Success")]
        public void Test_SavedGame_Set_PlayerLocation(string playerLocation)
        {
            // Create SavedGame object with valid player location
            savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", playerLocation, 1, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());

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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Alaska", 1, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid PlayerLocation"));
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
            savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", moveNumber, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());

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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", moveNumber, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());
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
            savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, new List<string>());

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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, new Dictionary<string, string>(), new List<string>());
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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
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
            savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, MyTestSavedGame.OpponentsAndHidingPlaces, new List<string>());

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
            savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, foundOpponentsAsList);

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
                    savedGame = new SavedGame(MyTestHouse.GetNewTestHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, foundOpponentsAsList);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("found opponent is not an opponent"));
            });
        }
    }
}