using Moq;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// SavedGame tests for constructors and public properties
    /// </summary>
    [TestFixture]
    public class TestSavedGame
    {
        private SavedGame savedGame;
        private Dictionary<string, string> opponentsAndHidingPlaces = new Dictionary<string, string>()
        {
            { "Joe", "Kitchen" },
            { "Bob", "Pantry" },
            { "Ana", "Bathroom" },
            { "Owen", "Kitchen" },
            { "Jimmy", "Pantry" }
        };

        [SetUp]
        public void SetUp()
        {
            savedGame = null;
            SavedGame.FileSystem = new FileSystem(); // Set static SavedGame file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            SavedGame.FileSystem = new FileSystem(); // Set static SavedGame file system to new file system
        }

        [Test]
        [Category("SavedGame GetFullSavedGameFileName Success")]
        public void Test_SavedGame_GetFullSavedGameFileName()
        {
            Assert.That(SavedGame.GetFullSavedGameFileName("my_saved_game"), Is.EqualTo("my_saved_game.game.json"));
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
        [Category("SavedGame GetFullSavedGameFileName ArgumentException Failure")]
        public void Test_SavedGame_GetFullSavedGameFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that getting full saved game file name with invalid file name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    SavedGame.GetFullSavedGameFileName(fileName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot perform action because file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestSavedGame_TestData), nameof(TestSavedGame_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_SingleSavedGameFile(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "DefaultHouse.house.json", "NotASavedGame.json", "HideAndSeekClassLibrary.dll",
                               "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json", "HideAndSeekConsole.dll",
                               "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb", "HideAndSeekConsole.runtimeconfig.json",
                               "MyGame.game.json", "OtherHouse.house.json", "TestableIO.System.IO.Abstractions.dll",
                               "TestableIO.System.IO.Abstractions.Wrappers.dll"
                              });
            Assert.That(GetSavedGameFileNames(), Is.EquivalentTo(new List<string>() { "MyGame" }));
        }

        [TestCaseSource(typeof(TestSavedGame_TestData), nameof(TestSavedGame_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_MultipleSavedGameFiles(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "1G@m3.game.json", "AGame.game.json", "DefaultHouse.house.json", "NotASavedGame.json", 
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb", "HideAndSeekConsole.deps.json", 
                               "HideAndSeekConsole.dll", "HideAndSeekConsole.exe", "HideAndSeekConsole.pdb", 
                               "HideAndSeekConsole.runtimeconfig.json", "MyGame.game.json", "OtherHouse.house.json", 
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll", 
                               "Winning.game.json"
                             });
            Assert.That(GetSavedGameFileNames(), Is.EquivalentTo(new List<string>() { "1G@m3", "AGame", "MyGame", "Winning"}));
        }

        [TestCaseSource(typeof(TestSavedGame_TestData), nameof(TestSavedGame_TestData.TestCases_For_Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles))]
        [Category("SavedGame GetSavedGameFileNames Success")]
        public void Test_SavedGame_GetSavedGameFileNames_NoSavedGameFiles(Func<IEnumerable<string>> GetSavedGameFileNames)
        {
            SetSavedGameFileSystemForGetSavedGameFileNamesTest(
                new string[] { "DefaultHouse.house.json", "FriendHouse.house.json", "NotASavedGame.json", 
                               "HideAndSeekClassLibrary.dll", "HideAndSeekClassLibrary.pdb", 
                               "HideAndSeekConsole.deps.json", "HideAndSeekConsole.dll", "HideAndSeekConsole.exe", 
                               "HideAndSeekConsole.pdb", "HideAndSeekConsole.runtimeconfig.json",
                               "TestableIO.System.IO.Abstractions.dll", "TestableIO.System.IO.Abstractions.Wrappers.dll"
                             });
            Assert.That(GetSavedGameFileNames(), Is.Empty);
        }

        [Test]
        [Category("SavedGame GetSavedGameFileNames Failure")]
        public void Test_SavedGame_GetSavedGameFileNames_AndCheckErrorMessage_ForInvalidDirectoryName()
        {
            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<DirectoryNotFoundException>(() => SavedGame.GetSavedGameFileNames("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"));
                Assert.That(exception.Message, Is.EqualTo("Could not find a part of the path 'C:\\Users\\Tester\\Desktop\\HideAndSeekConsole'."));
            });
        }

        private static void SetSavedGameFileSystemForGetSavedGameFileNamesTest(string[] fileNames)
        {
            // Set up mock file system
            Mock<IFileSystem> mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup((d) => d.Directory.GetFiles("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"))
                          .Returns(fileNames); // Set up mock to return files
            mockFileSystem.Setup((d) => d.Path.GetDirectoryName(It.IsAny<string>())).Returns("C:\\Users\\Tester\\Desktop\\HideAndSeekConsole"); // Mock default directory name assigned if no argument passed in
            SavedGame.FileSystem = mockFileSystem.Object; // Set SavedGame file system
        }

        // Tests all setters except House and HouseFileName (accesses House and HouseFileName propertyes' backing fields)
        [Test]
        [Category("SavedGame Constructor Success")]
        public void Test_SavedGame_Constructor_WithHouse_AndHouseFileName()
        {
            House house = GetMockedHouse(); // Get mocked House

            // Create SavedGame using parameterized constructor
            savedGame = new SavedGame(house, "TestHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>());

            // Assert that SavedGame properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(savedGame.House, Is.SameAs(house), "House object is same");
                Assert.That(savedGame.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "player location");
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(savedGame.OpponentsAndHidingLocations, Is.EquivalentTo(opponentsAndHidingPlaces), "opponents and hiding locations");
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
        [Category("SavedGame Constructor ArgumentException Failure")]
        public void Test_SavedGame_Constructor_WithHouse_AndHouseFileName_AndCheckErrorMessage_ForInvalidFileName(string fileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with an invalid file name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    savedGame = new SavedGame(GetMockedHouse(), fileName, "Entry", 1, opponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"House file name \"{fileName}\" is invalid " +
                                                               "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("SavedGame House InvalidOperationException Failure")]
        public void Test_SavedGame_Set_House_AndCheckErrorMessage_ForAlreadyHasValue()
        {
            // Create new SavedGame (giving House property's backing field a value)
            savedGame = new SavedGame(GetMockedHouse(), "DefaultHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>()); ;

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
        [Category("SavedGame HouseFileName InvalidOperationException Failure")]
        public void Test_SavedGame_Set_HouseFileName_AndCheckErrorMessage_ForAlreadyHasValue()
        {
            // Create new SavedGame (giving HouseFileName property's backing field a value)
            savedGame = new SavedGame(GetMockedHouse(), "DefaultHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>()); ;

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

        [Test]
        [Category("SavedGame PlayerLocation Success")]
        public void Test_SavedGame_Set_PlayerLocation()
        {
            // Create SavedGame object with initial valid player location
            savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>());

            Assert.Multiple(() =>
            {
                // Assert that player location property's getter returns initial value
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Entry"), "initial value");

                // Change player location property's value and check
                savedGame.PlayerLocation = "Pantry";
                Assert.That(savedGame.PlayerLocation, Is.EqualTo("Pantry"), "changed value");
            });
            
        }

        [Test]
        [Category("SavedGame PlayerLocation InvalidOperationException Failure")]
        public void Test_SavedGame_Set_PlayerLocation_AndCheckErrorMessage_ForInvalidValue()
        {
            Mock<House> houseMock = new Mock<House>();
            houseMock.Setup((h) => h.DoesLocationWithHidingPlaceExist(It.IsAny<string>())).Returns(true);
            houseMock.Setup((h) => h.DoesLocationExist("Alaska")).Returns(false);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with invalid player location raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame = new SavedGame(houseMock.Object, "TestHouse", "Alaska", 1, opponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("invalid PlayerLocation - location \"Alaska\" does not exist in House"));
            });
        }

        [Test]
        [Category("SavedGame MoveNumber Success")]
        public void Test_SavedGame_Set_MoveNumber()
        {
            // Create SavedGame object with valid move number
            savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>());

            Assert.Multiple(() =>
            {
                // Assert that move number property's getter returns initial value
                Assert.That(savedGame.MoveNumber, Is.EqualTo(1), "initial value");

                // Change move number property's value and check
                savedGame.MoveNumber = 54;
                Assert.That(savedGame.MoveNumber, Is.EqualTo(54), "changed value");
            });
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1000)]
        [Category("SavedGame MoveNumber ArgumentOutOfRangeException Failure")]
        public void Test_SavedGame_Set_MoveNumber_AndCheckErrorMessage_ForInvalidValue(int moveNumber)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with invalid move number raises an exception
                var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", moveNumber, opponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("MoveNumber is invalid - must be positive number"));
            });
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations Success")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces()
        {
            // Create dictionary of valid opponents and initial hiding places
            Dictionary<string, string> initialOpponentsAndHidingPlaces = new Dictionary<string, string>();
            initialOpponentsAndHidingPlaces.Add("Joe", "Kitchen");
            initialOpponentsAndHidingPlaces.Add("Bob", "Pantry");
            initialOpponentsAndHidingPlaces.Add("Ana", "Bathroom");
            initialOpponentsAndHidingPlaces.Add("Owen", "Attic");
            initialOpponentsAndHidingPlaces.Add("Jimmy", "Garage");

            // Create dictionary of valid opponents and different hiding places
            Dictionary<string, string> changedOpponentsAndHidingPlaces = new Dictionary<string, string>();
            changedOpponentsAndHidingPlaces.Add("Joe", "Master Bedroom");
            changedOpponentsAndHidingPlaces.Add("Bob", "Master Bath");
            changedOpponentsAndHidingPlaces.Add("Ana", "Pantry");
            changedOpponentsAndHidingPlaces.Add("Owen", "Garage");
            changedOpponentsAndHidingPlaces.Add("Jimmy", "Attic");

            // Create SavedGame object with valid opponents and initial hiding places dictionary
            savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, initialOpponentsAndHidingPlaces, new List<string>());

            Assert.Multiple(() =>
            {
                // Assert that opponents and hiding places property's getter returns initial expected value
                Assert.That(savedGame.OpponentsAndHidingLocations, Is.EquivalentTo(initialOpponentsAndHidingPlaces), "initial value");

                // Change opponent and hiding places property's value and check
                savedGame.OpponentsAndHidingLocations = changedOpponentsAndHidingPlaces;
                Assert.That(savedGame.OpponentsAndHidingLocations, Is.EquivalentTo(changedOpponentsAndHidingPlaces), "changed value");
            });
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations ArgumentException Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForEmptyDictionary()
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with empty dictionary for OpponentsAndHidingLocations raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, 
                                              new Dictionary<string, string>(), new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("invalid OpponentsAndHidingLocations - no opponents"));
            });
        }

        [TestCase("Dungeon", "Living Room", "Attic", "Kitchen", "Pantry")]
        [TestCase("Living Room", "Dungeon", "Attic", "Kitchen", "Pantry")]
        [TestCase("Bathroom", "Second Bathroom", "Dungeon", "Living Room", "Garage")]
        [TestCase("Kids Room", "Nursery", "Nursery", "Dungeon", "Master Bedroom")]
        [TestCase("Kids Room", "Nursery", "Nursery", "Master Bedroom", "Dungeon")]
        [TestCase("Dungeon", "Dungeon", "Nursery", "Master Bedroom", "Pantry")]
        [Category("SavedGame OpponentsAndHidingLocations InvalidOperationException Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForNonexistentLocation
            (string location1, string location2, string location3, string location4, string location5)
        {
            Mock<House> houseMock = new Mock<House>();
            houseMock.Setup((h) => h.DoesLocationExist(It.IsAny<string>())).Returns(true);
            houseMock.Setup((h) => h.DoesLocationWithHidingPlaceExist(It.IsAny<string>())).Returns(true);
            houseMock.Setup((h) => h.DoesLocationWithHidingPlaceExist("Dungeon")).Returns(false);

            // Create dictionary of opponents with at least one with an invalid hiding place (nonexistent location)
            Dictionary<string, string> invalidOpponentsAndHidingPlaces = new Dictionary<string, string>();
            invalidOpponentsAndHidingPlaces.Add("Joe", location1);
            invalidOpponentsAndHidingPlaces.Add("Bob", location2);
            invalidOpponentsAndHidingPlaces.Add("Ana", location3);
            invalidOpponentsAndHidingPlaces.Add("Owen", location4);
            invalidOpponentsAndHidingPlaces.Add("Jimmy", location5);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with nonexistent (invalid) opponent hiding place/s raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame = new SavedGame(houseMock.Object, "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("location with hiding place \"Dungeon\" does not exist in House"));
            });
        }

        [Test]
        [Category("SavedGame OpponentsAndHidingLocations InvalidOperationException Failure")]
        public void Test_SavedGame_Set_OpponentsAndHidingPlaces_AndCheckErrorMessage_ForLocationWithoutHidingPlace()
        {
            Mock<House> houseMock = new Mock<House>();
            houseMock.Setup((h) => h.DoesLocationExist(It.IsAny<string>())).Returns(true);
            houseMock.Setup((h) => h.DoesLocationWithHidingPlaceExist("Entry")).Returns(false);

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
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame = new SavedGame(houseMock.Object, "TestHouse", "Entry", 1, invalidOpponentsAndHidingPlaces, new List<string>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("location with hiding place \"Entry\" does not exist in House"));
            });
        }

        [Test]
        [Category("SavedGame FoundOpponents Success")]
        public void Test_SavedGame_Set_FoundOpponents_ToEmptyList()
        {
            // Create SavedGame object with valid empty list for FoundOpponents
            savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, opponentsAndHidingPlaces, new List<string>());

            Assert.Multiple(() =>
            {
                // Assert that found opponents property's getter returns expected value after object creation
                Assert.That(savedGame.FoundOpponents, Is.Empty, "empty after object created");
                
                // Set found opponents property to valid but not-empty list
                savedGame.FoundOpponents = new List<string>() { "Joe" };

                // Set found opponents property back to valid empty list and check
                savedGame.FoundOpponents = new List<string>();
                Assert.That(savedGame.FoundOpponents, Is.Empty, "empty after property set several times");
            });
            
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
            List<string> initialFoundOpponentsAsList = foundOpponents.ToList();

            // Create dictionary of valid opponents and hiding places
            Dictionary<string, string> validOpponentsAndHidingPlaces = new Dictionary<string, string>();
            validOpponentsAndHidingPlaces.Add("Joe", "Kitchen");
            validOpponentsAndHidingPlaces.Add("Bob", "Pantry");
            validOpponentsAndHidingPlaces.Add("Ana", "Bathroom");
            validOpponentsAndHidingPlaces.Add("Owen", "Attic");
            validOpponentsAndHidingPlaces.Add("Jimmy", "Garage");

            // Create SavedGame object with valid found opponents list
            savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, initialFoundOpponentsAsList);

            Assert.Multiple(() =>
            {
                // Assert that found opponents property's getter returns expected value after object creation
                Assert.That(savedGame.FoundOpponents, Is.EquivalentTo(initialFoundOpponentsAsList), "initial value");

                // Set found opponents property to another valid value and check
                List<string> changedFoundOpponentsList = new List<string>() { "Jimmy", "Owen" };
                savedGame.FoundOpponents = changedFoundOpponentsList;
                Assert.That(savedGame.FoundOpponents, Is.EquivalentTo(changedFoundOpponentsList), "changed value");
            });
            
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
        [Category("SavedGame FoundOpponents InvalidOperationException Failure")]
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
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    savedGame = new SavedGame(GetMockedHouse(), "TestHouse", "Entry", 1, validOpponentsAndHidingPlaces, foundOpponentsAsList);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("found opponent is not an opponent"));
            });
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