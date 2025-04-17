using HideAndSeek;
using System.IO.Abstractions;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for Move and Teleport methods called in default House,
    /// checking CurrentLocation, Move, and GameOver properties along the way; and
    /// automatically testing GameController constructor with default House file name passed in
    /// </summary>
    public class TestGameController_MoveAndTeleport
    {
        GameController gameController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set static House file system to mock file system (not changed in any tests)
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                "DefaultHouse.json", TestGameController_MoveAndTeleport_TestData.DefaultHouse_Serialized);
        }

        [SetUp]
        public void SetUp()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
            gameController = new GameController("DefaultHouse"); // Create new GameController with default House layout

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "start in Entry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1));
                Assert.That(gameController.GameOver, Is.False, "game not over when started");
            });

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [Test]
        [Category("GameController Move CurrentLocation MoveNumber GameOver Success")]
        public void Test_GameController_Move_InAllDirections()
        {
            Assert.Multiple(() =>
            {
                // Move Out to Garage
                Assert.That(gameController.Move(Direction.Out), Is.EqualTo("Moving Out"), "Out to Garage");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Garage"), "now in Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2));

                // Move In to Entry
                Assert.That(gameController.Move(Direction.In), Is.EqualTo("Moving In"), "In to Entry");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "now in Entry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3));

                // Move East to Hallway
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "East to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 1st time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4));

                // Move North to Bathroom
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "North to Bathroom");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "now in Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(5));

                // Move South to Hallway
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "South to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(6));

                // Move Northwest to Kitchen
                Assert.That(gameController.Move(Direction.Northwest), Is.EqualTo("Moving Northwest"), "Northwest to Kitchen");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"), "now in Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7));

                // Move Southeast to Hallway
                Assert.That(gameController.Move(Direction.Southeast), Is.EqualTo("Moving Southeast"), "Southeast to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway again for 3rd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(8));

                // Move Up to Landing
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "Up to Landing");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Landing"), "now in Landing for 1st time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9));

                // Move Southwest to Nursery
                Assert.That(gameController.Move(Direction.Southwest), Is.EqualTo("Moving Southwest"), "Southwest to Nursery");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Nursery"), "now in Nursery");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10));

                // Move Northeast to Landing
                Assert.That(gameController.Move(Direction.Northeast), Is.EqualTo("Moving Northeast"), "Northeast to Landing");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Landing"), "now in Landing again for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11));

                // Move Down to Hallway
                Assert.That(gameController.Move(Direction.Down), Is.EqualTo("Moving Down"), "Down to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 4th time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(12));

                // Move West to Entry
                Assert.That(gameController.Move(Direction.West), Is.EqualTo("Moving West"), "West to Entry");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "now in Entry for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13));

                // Assert that game is not over
                Assert.That(gameController.GameOver, Is.False, "game not over after moving multiple times");
            });
        }

        [TestCaseSource(typeof(TestGameController_MoveAndTeleport_TestData), 
            nameof(TestGameController_MoveAndTeleport_TestData.TestCases_For_Test_GameController_Move_InAllDirectionsAsStrings))]
        [Category("GameController Move CurrentLocation MoveNumber GameOver Success")]
        public void Test_GameController_Move_InAllDirectionsAsStrings(
            string north, string south, string east, string west, string northeast, string southwest, 
            string southeast, string northwest, string up, string down, string inText, string outText)
        {
            Assert.Multiple(() =>
            {
                // Move Out to Garage
                Assert.That(gameController.Move(outText), Is.EqualTo("Moving Out"), "Out to Garage");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Garage"), "now in Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2));

                // Move In to Entry
                Assert.That(gameController.Move(inText), Is.EqualTo("Moving In"), "In to Entry");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "now in Entry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3));

                // Move East to Hallway
                Assert.That(gameController.Move(east), Is.EqualTo("Moving East"), "East to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 1st time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4));

                // Move North to Bathroom
                Assert.That(gameController.Move(north), Is.EqualTo("Moving North"), "North to Bathroom");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "now in Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(5));

                // Move South to Hallway
                Assert.That(gameController.Move(south), Is.EqualTo("Moving South"), "South to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(6));

                // Move Northwest to Kitchen
                Assert.That(gameController.Move(northwest), Is.EqualTo("Moving Northwest"), "Northwest to Kitchen");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"), "now in Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7));

                // Move Southeast to Hallway
                Assert.That(gameController.Move(southeast), Is.EqualTo("Moving Southeast"), "Southeast to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway again for 3rd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(8));

                // Move Up to Landing
                Assert.That(gameController.Move(up), Is.EqualTo("Moving Up"), "Up to Landing");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Landing"), "now in Landing for 1st time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9));

                // Move Southwest to Nursery
                Assert.That(gameController.Move(southwest), Is.EqualTo("Moving Southwest"), "Southwest to Nursery");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Nursery"), "now in Nursery");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10));

                // Move Northeast to Landing
                Assert.That(gameController.Move(northeast), Is.EqualTo("Moving Northeast"), "Northeast to Landing");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Landing"), "now in Landing again for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11));

                // Move Down to Hallway
                Assert.That(gameController.Move(down), Is.EqualTo("Moving Down"), "Down to Hallway");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "now in Hallway for 4th time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(12));

                // Move West to Entry
                Assert.That(gameController.Move(west), Is.EqualTo("Moving West"), "West to Entry");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "now in Entry for 2nd time");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13));

                // Assert that game is not over
                Assert.That(gameController.GameOver, Is.False, "game not over after moving multiple times");
            });
        }

        [TestCase("E")]
        [TestCase("East")]
        [TestCase("eAst")]
        [TestCase("eASt")]
        [TestCase("EAST")]
        [Category("GameController Move CurrentLocation MoveNumber GameOver Success")]
        public void Test_GameController_Move_InDirectionAsNotLowercaseString(string directionText)
        {
            Assert.Multiple(() =>
            {
                Assert.That(gameController.Move(directionText), Is.EqualTo("Moving East"), "return value");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Hallway"), "current location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number increments");
                Assert.That(gameController.GameOver, Is.False, "game not over after moving");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("}{yaeu\\@!//")]
        [TestCase("No")]
        [TestCase("Northuperly")]
        [Category("GameController Move CurrentLocation MoveNumber ArgumentException Failure")]
        public void Test_GameController_Move_AndCheckErrorMessageAndProperties_ForInvalidDirection(string directionText)
        {
            Location initialLocation = gameController.CurrentLocation; // Get initial location before attempt to move

            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<ArgumentException>(() => {
                    gameController.Move(directionText);
                });
                Assert.That(exception.Message, Does.StartWith("That's not a valid direction"), "exception message");
                Assert.That(gameController.CurrentLocation, Is.EqualTo(initialLocation), "current location does not change");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number does not increment");
                Assert.That(gameController.GameOver, Is.False, "game not over after trying to move");
            });
        }

        [TestCaseSource(typeof(TestGameController_MoveAndTeleport_TestData), 
            nameof(TestGameController_MoveAndTeleport_TestData.TestCases_For_Test_GameController_Move_InDirectionWithNoLocation_AndCheckErrorMessageAndProperties))]
        [Category("GameController Move CurrentLocation MoveNumber Failure")]
        public void Test_GameController_Move_InDirectionWithNoLocation_AndCheckErrorMessageAndProperties(
            Func<GameController, Exception> Move)
        {
            Location initialLocation = gameController.CurrentLocation; // Get initial location before attempt to move

            Assert.Multiple(() =>
            {
                Exception exception = Move(gameController);
                Assert.That(exception, Is.TypeOf<InvalidOperationException>());
                Assert.That(exception.Message, Is.EqualTo("There is no exit for location \"Entry\" in direction \"Up\""), "exception message");
                Assert.That(gameController.CurrentLocation, Is.EqualTo(initialLocation), "current location does not change");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number does not increment");
                Assert.That(gameController.GameOver, Is.False, "game not over after trying to move");
            });
        }

        [Test]
        [Category("GameController Teleport CurrentLocation MoveNumber GameOver Success")]
        public void Test_GameController_Teleport()
        {
            // Set House Random number generator to mock random
            House.Random = new MockRandomWithValueList([
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // First random location with hiding place is Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 // Second random location with hiding place is Pantry
            ]);

            Assert.Multiple(() =>
            {
                // Teleport from location without hiding place and check return message and properties
                Assert.That(gameController.Teleport(), Is.EqualTo("Teleporting to random location with hiding place: Kitchen"), "message when teleport from location without hiding place");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"), "current location after teleport from location without hiding place");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after teleport from location without hiding place");
                Assert.That(gameController.GameOver, Is.False, "game not over after teleport from location without hiding place");

                // Teleport from location with hiding place and check return message and properties
                Assert.That(gameController.Teleport(), Is.EqualTo("Teleporting to random location with hiding place: Pantry"), "message when teleport from location with hiding place");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Pantry"), "current location after teleport from location with hiding place");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after teleport from location with hiding place");
                Assert.That(gameController.GameOver, Is.False, "game not over after teleport from location with hiding place");
            });
        }

        [Test]
        [Category("GameController Teleport CurrentLocation MoveNumber GameOver Success")]
        public void Test_GameController_Teleport_AndLandInSameLocation()
        {
            // Set House Random number generator to mock random
            House.Random = new MockRandomWithValueList([0]);

            Assert.Multiple(() =>
            {
                // Teleport from Entry and check return message and properties
                Assert.That(gameController.Teleport(), Is.EqualTo("Teleporting to random location with hiding place: Attic"), "message when teleport from location without hiding place");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Attic"), "current location after teleport from location without hiding place");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after teleport from location without hiding place");
                Assert.That(gameController.GameOver, Is.False, "game not over after teleport from location without hiding place");

                // Teleport from Attic and check return message and properties
                Assert.That(gameController.Teleport(), Is.EqualTo("Teleporting to random location with hiding place: Attic"), "message when teleport from location with hiding place");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Attic"), "current location after teleport from location with hiding place");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after teleport from location with hiding place");
                Assert.That(gameController.GameOver, Is.False, "game not over after teleport from location with hiding place");
            });
        }
    }
}