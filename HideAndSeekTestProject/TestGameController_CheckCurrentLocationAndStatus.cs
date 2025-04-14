using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for CheckCurrentLocation method called in default House,
    /// checking Status, CurrentLocation, Move, and GameOver properties along the way; and
    /// automatically testing GameController constructor with default House file name passed in
    /// </summary>
    public class TestGameController_CheckCurrentLocationAndStatus
    {
        GameController gameController;

        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        private static string DefaultHouse_Serialized
        {
            get
            {
                return
                    "{" +
                        "\"Name\":\"my house\"" + "," +
                        "\"HouseFileName\":\"DefaultHouse\"" + "," +
                        "\"PlayerStartingPoint\":\"Entry\"" + "," +
                        "\"LocationsWithoutHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"Name\":\"Hallway\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Entry\"," +
                                    "\"Northwest\":\"Kitchen\"," +
                                    "\"North\":\"Bathroom\"," +
                                    "\"South\":\"Living Room\"," +
                                    "\"Up\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Landing\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Hallway\"," +
                                    "\"Up\":\"Attic\"," +
                                    "\"Southeast\":\"Kids Room\"," +
                                    "\"Northwest\":\"Master Bedroom\"," +
                                    "\"Southwest\":\"Nursery\"," +
                                    "\"South\":\"Pantry\"," +
                                    "\"West\":\"Second Bathroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Entry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Out\":\"Garage\"," +
                                    "\"East\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" + "," +
                        "\"LocationsWithHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"HidingPlace\":\"in a trunk\"," +
                                "\"Name\":\"Attic\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{\"HidingPlace\":\"behind the door\"," +
                                "\"Name\":\"Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"South\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the bunk beds\"," +
                                "\"Name\":\"Kids Room\"," +
                                "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Northwest\":\"Landing\"" +
                                    "}" +
                                "}," +
                            "{" +
                                "\"HidingPlace\":\"under the bed\"," +
                                "\"Name\":\"Master Bedroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Landing\"," +
                                    "\"East\":\"Master Bath\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the changing table\"," +
                                "\"Name\":\"Nursery\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Northeast\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"inside a cabinet\"," +
                                "\"Name\":\"Pantry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the shower\"," +
                                "\"Name\":\"Second Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"East\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"next to the stove\"," +
                                "\"Name\":\"Kitchen\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the tub\"," +
                                "\"Name\":\"Master Bath\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Master Bedroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the car\"," +
                                "\"Name\":\"Garage\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"In\":\"Entry\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the sofa\"," +
                                "\"Name\":\"Living Room\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" +
                    "}";
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set static House file system to mock file system (not changed in any tests)
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                "DefaultHouse.json", DefaultHouse_Serialized);

            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                            1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                            0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                            1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                            1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                            0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 // Hide opponent in Pantry
                           ]);
        }

        [SetUp]
        public void SetUp()
        {
            gameController = new GameController(5, "DefaultHouse"); // Create new GameController with 5 opponents and default House layout
            
            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "start in Entry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1));
                Assert.That(gameController.GameOver, Is.False, "game not over when started");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the East" +
                    Environment.NewLine + " - the Garage is Out" +
                    Environment.NewLine + "You have not found any opponents"), "status when game started");
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [Test]
        [Category("GameController CheckCurrentLocation Status MoveNumber GameOver CurrentLocation Failure")]
        public void Test_GameController_CheckCurrentlocation_InLocationWithoutHidingPlace_AndCheckErrorMessageAndProperties()
        {
            Location initialLocation = gameController.CurrentLocation; // Get initial location before attempt to check
            string initialStatus = gameController.Status; // Get initial status before attempt to check

            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() => {
                    gameController.CheckCurrentLocation();
                });
                Assert.That(exception.Message, Is.EqualTo("There is no hiding place in the Entry"), "exception message");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "status does not change");
                Assert.That(gameController.CurrentLocation, Is.EqualTo(initialLocation), "current location does not change");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number increments");
                Assert.That(gameController.GameOver, Is.False, "game not over after trying to check");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_WhenNoOpponentHiding()
        {
            // Move to Garage
            gameController.Move(Direction.Out);

            Assert.Multiple(() =>
            {
                // Check that properties are as expected
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exits:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have not found any opponents"), "status after move to Garage");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Garage"), "current location after move to Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after move to Garage");
                Assert.That(gameController.GameOver, Is.False, "game not over after move to Garage");

                // Check current location and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the car"), "message when check Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exits:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have not found any opponents"), "status after check Garage");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Garage"), "current location after check Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after check Garage");
                Assert.That(gameController.GameOver, Is.False, "game not over after check Garage");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_When1OpponentHiding()
        {
            // Move to Bathroom
            gameController.Move(Direction.East); // Move East from Entry to Hallway
            gameController.Move(Direction.North); // Move North from Hallway to Bathroom

            Assert.Multiple(() =>
            {
                // Check that properties are as expected
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have not found any opponents"), "status after move to Bathroom");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"));
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after move to Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after move to Bathroom");

                // Check current location and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind the door"), "message when check Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Ana"), "status after check Bathroom");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "current location after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after check Bathroom");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_WhenMultipleOpponentsHiding()
        {
            // Move to Kitchen
            gameController.Move(Direction.East); // Move East from Entry to Hallway
            gameController.Move(Direction.Northwest); // Move Northwest from Hallway to Kitchen

            Assert.Multiple(() =>
            {
                // Check that properties are as expected
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have not found any opponents"), "status after move to Kitchen");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"));
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after move to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after move to Kitchen");

                // Check current location and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding next to the stove"), "message when check Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 2 of 5 opponents: Joe, Owen"), "status after check Kitchen");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"), "current location after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after check Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after check Kitchen");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_FindAllOpponents()
        {
            // Move to Bathroom
            gameController.Move(Direction.East); // Move East from Entry to Hallway
            gameController.Move(Direction.North); // Move North from Hallway to Bathroom

            Assert.Multiple(() =>
            {
                // Check Bathroom and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind the door"), "message when check Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Ana"), "status after check Bathroom");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "current location after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after check Bathroom");

                // Move to Hallway and check Status
                gameController.Move(Direction.South); // Move South from Bathroom to Hallway
                Assert.That(gameController.Status, Is.EqualTo("You are in the Hallway. You see the following exits:" +
                    Environment.NewLine + " - the Landing is Up" +
                    Environment.NewLine + " - the Bathroom is to the North" +
                    Environment.NewLine + " - the Living Room is to the South" +
                    Environment.NewLine + " - the Entry is to the West" +
                    Environment.NewLine + " - the Kitchen is to the Northwest" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Ana"), "status in Hallway after found 1 opponent");
                
                // Move to Kitchen
                gameController.Move(Direction.Northwest); // Move Northwest from Hallway to Kitchen

                // Check Kitchen and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding next to the stove"), "message when check Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Ana, Joe, Owen"), "status after check Kitchen");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Kitchen"), "current location after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after check Kitchen");

                // Move to Hallway and check Status
                gameController.Move(Direction.Southeast); // Move Southeast from Kitchen to Hallway
                Assert.That(gameController.Status, Is.EqualTo("You are in the Hallway. You see the following exits:" +
                    Environment.NewLine + " - the Landing is Up" +
                    Environment.NewLine + " - the Bathroom is to the North" +
                    Environment.NewLine + " - the Living Room is to the South" +
                    Environment.NewLine + " - the Entry is to the West" +
                    Environment.NewLine + " - the Kitchen is to the Northwest" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Ana, Joe, Owen"), "status in Hallway after found 3 opponents");

                // Move to the Pantry
                gameController.Move(Direction.Up); // Move Up from Hallway to Landing
                gameController.Move(Direction.South); // Move South from Landing to Pantry

                // Check Pantry and check return message and properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding inside a cabinet"), "message when check Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 5 of 5 opponents: Ana, Joe, Owen, Bob, Jimmy"), "status after check Pantry");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Pantry"), "current location after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number after check Pantry");
                Assert.That(gameController.GameOver, Is.True, "game over after check Pantry");
            });
        }
    }
}
