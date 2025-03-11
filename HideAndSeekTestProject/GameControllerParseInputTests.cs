using HideAndSeek;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for moving and checking for opponents via ParseInput method
    /// Also tests for value of Prompt property as navigate through House
    /// Not including save/load/delete game tests (contained in separate file)
    /// </summary>
    public class GameControllerParseInputTests
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            gameController = new GameController();
        }

        [Test]
        [Category("GameController ParseInput ValidMove")]
        public void Test_GameController_ParseInput_ForValidMoves_AndCheckMessageAndStatus()
        {
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // Move East from StartingPoint to Hallway.
                Assert.That(gameController.ParseInput("East"), Is.EqualTo("Moving East"), "parsing \"East\" from Entry returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Hallway. You see the following exits:" +
                    Environment.NewLine + " - the Landing is Up" +
                    Environment.NewLine + " - the Bathroom is to the North" +
                    Environment.NewLine + " - the Living Room is to the South" +
                    Environment.NewLine + " - the Entry is to the West" +
                    Environment.NewLine + " - the Kitchen is to the Northwest" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Hallway");

                // Move Up from Hallway to Landing.
                Assert.That(gameController.ParseInput("Up"), Is.EqualTo("Moving Up"), "parsing \"Up\" from Hallway returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Landing. You see the following exits:" +
                    Environment.NewLine + " - the Attic is Up" +
                    Environment.NewLine + " - the Kids Room is to the Southeast" +
                    Environment.NewLine + " - the Pantry is to the South" +
                    Environment.NewLine + " - the Second Bathroom is to the West" +
                    Environment.NewLine + " - the Nursery is to the Southwest" +
                    Environment.NewLine + " - the Master Bedroom is to the Northwest" +
                    Environment.NewLine + " - the Hallway is Down" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Landing");

                // Move Northwest from Landing to Master Bedroom.
                Assert.That(gameController.ParseInput("Northwest"), Is.EqualTo("Moving Northwest"), "parsing \"Northwest\" from Landing returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Master Bedroom. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the Southeast" +
                    Environment.NewLine + " - the Master Bath is to the East" +
                    Environment.NewLine + "Someone could hide under the bed" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Master Bedroom");

                // Move East from Master Bedroom to Master Bath.
                Assert.That(gameController.ParseInput("East"), Is.EqualTo("Moving East"), "parsing \"East\" from Master Bedroom returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Master Bath. You see the following exits:" +
                    Environment.NewLine + " - the Master Bedroom is to the West" +
                    Environment.NewLine + "Someone could hide in the tub" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Master Bath");
            });
        }

        [Test]
        [Category("GameController ParseInput InvalidMove")]
        public void Test_GameController_ParseInput_ForInvalidMove_InInvalidDirection_AndCheckMessageAndStatus()
        {
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                Assert.That(gameController.ParseInput("X"), Is.EqualTo("That's not a valid direction"), "parsing \"X\" returns invalid direction error message");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "game status does not change");
            });
        }

        [Test]
        [Category("GameController ParseInput InvalidMove")]
        public void Test_GameController_ParseInput_ForInvalidMove_InDirectionWithNoLocation_AndCheckMessageAndStatus()
        {
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                Assert.That(gameController.ParseInput("Up"), Is.EqualTo("There's no exit in that direction"), "parsing returns no exit in that direction error message");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "game status does not change");
            });
        }

        /// <summary>
        /// Using ParseInput method, mimic full game and check ParseInput return message and GameController's public properties:
        /// Prompt, Status, MoveNumber, GameOver
        /// 
        /// CREDIT: adapted from HideAndSeek project's GameControllerTests class's TestParseCheck() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/GameControllerTests.cs
        ///         Link valid as of 02-25-2025
        ///         
        /// CHANGES:
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I used a custom method, House.HideAllOpponents, to hide Opponents.
        /// -I added/edited some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [Test]
        [Category("GameController ParseInput ValidMove")]
        public void Test_GameController_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessagesAndPublicProperties()
        {
            Assert.Multiple(() =>
            {
                // Assert that game is not over at beginning
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Create enumerable of places for opponents to hide
                IEnumerable<LocationWithHidingPlace> hidingPlaces = new List<LocationWithHidingPlace>() 
                {
                    gameController.House.GetLocationWithHidingPlaceByName("Garage"),
                    gameController.House.GetLocationWithHidingPlaceByName("Kitchen"),
                    gameController.House.GetLocationWithHidingPlaceByName("Attic"),
                    gameController.House.GetLocationWithHidingPlaceByName("Attic"),
                    gameController.House.GetLocationWithHidingPlaceByName("Kitchen")
                };

                // Hide Opponents is specific hiding places
                gameController.RehideAllOpponents(hidingPlaces);

                // Check the StartingPoint -- there are no players hiding there
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "check game move number");
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("There is no hiding place in the Entry"), "check string returned when check in StartingPoint");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "check game move number");

                // Move to the Garage
                gameController.ParseInput("Out");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "check game move number");

                // We hid Joe in the Garage, so validate ParseInput's return value and the properties
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("You found 1 opponent hiding behind the car"), "check string returned when check in Garage");

                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exits:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status after finding opponent in Garage");

                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go (or type 'check'): "), "check prompt after finding opponent in Garage");

                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "check game move number");

                // Move to the bathroom, where nobody is hiding
                gameController.ParseInput("In");
                gameController.ParseInput("East");
                gameController.ParseInput("North");

                // Check the Bathroom to make sure nobody is hiding there
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("Nobody was hiding behind the door"), "check string returned when check in Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "check game move number");

                // Check the Kitchen to make sure nobody is hiding there
                gameController.ParseInput("South");
                gameController.ParseInput("Northwest");

                Assert.That(gameController.ParseInput("check"), Is.EqualTo("You found 2 opponents hiding next to the stove"), "check string returned when check in Kitchen");

                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Joe, Bob, Jimmy"), "check status after finding opponents in Kitchen");

                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go (or type 'check'): "), "check prompt after finding opponents in Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "check game move number");
                Assert.That(gameController.GameOver, Is.False, "check game not over after finding opponents in Kitchen");

                // Head up to the Landing, then check the Pantry (nobody's hiding there)
                gameController.ParseInput("Southeast");
                gameController.ParseInput("Up");

                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "check game move number");

                gameController.ParseInput("South");

                Assert.That(gameController.ParseInput("check"), Is.EqualTo("Nobody was hiding inside a cabinet"), "check string returned when check in Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(15), "check game move number");

                // Check the Attic to find the last two opponents, make sure the game is over
                gameController.ParseInput("North");
                gameController.ParseInput("Up");

                Assert.That(gameController.MoveNumber, Is.EqualTo(17), "check game move number");
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("You found 2 opponents hiding in a trunk"), "check string returned when check in Attic");

                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exits:" +
                    Environment.NewLine + " - the Landing is Down" +
                    Environment.NewLine + "Someone could hide in a trunk" +
                    Environment.NewLine +
                    "You have found 5 of 5 opponents: Joe, Bob, Jimmy, Ana, Owen"), "check game status after find opponents in Attic");

                Assert.That(gameController.Prompt, Is.EqualTo("18: Which direction do you want to go (or type 'check'): "), "check game prompt after find opponents in Attic");
                Assert.That(gameController.MoveNumber, Is.EqualTo(18), "check game move number");
                Assert.That(gameController.GameOver, Is.True, "check game over after all opponents found");
            });
        }

        [Test]
        [Category("GameController Prompt")]
        public void Test_GameController_Prompt_InLocationsWithHidingPlace()
        {
            Assert.Multiple(() =>
            {
                gameController.ParseInput("Out"); // Move Out to Garage
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "prompt in Garage");
                gameController.ParseInput("In"); // Move In to StartingPoint
                gameController.ParseInput("East"); // Move East to Hallway
                gameController.ParseInput("North"); // Move North to Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "prompt in Bathroom");
                gameController.ParseInput("South"); // Move South to Hallway
                gameController.ParseInput("South"); // Move South to Living Room
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "prompt in Living Room");
                gameController.ParseInput("North"); // Move North to Hallway
                gameController.ParseInput("Northwest"); // Move Northwest to Kitchen
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "prompt in Kitchen");
                gameController.ParseInput("Southeast"); // Move Southeast to Hallway
                gameController.ParseInput("Up"); // Move Up to Landing
                gameController.ParseInput("Up"); // Move Up to Attic
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "prompt in Attic");
                gameController.ParseInput("Down"); // Move Down to Landing
                gameController.ParseInput("Southeast"); // Move Southeast to Kids Room
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "prompt in Kids Room");
                gameController.ParseInput("Northwest"); // Move Northwest to Landing
                gameController.ParseInput("South"); // Move South to Pantry
                Assert.That(gameController.Prompt, Is.EqualTo("16: Which direction do you want to go (or type 'check'): "), "prompt in Pantry");
                gameController.ParseInput("North"); // Move North to Landing
                gameController.ParseInput("West"); // Move West to Second Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("18: Which direction do you want to go (or type 'check'): "), "prompt in Second Bathroom");
                gameController.ParseInput("East"); // Move East to Landing
                gameController.ParseInput("Southwest"); // Move Southwest to Nursery
                Assert.That(gameController.Prompt, Is.EqualTo("20: Which direction do you want to go (or type 'check'): "), "prompt in Nursery");
                gameController.ParseInput("Northeast"); // Move Northeast to Landing
                gameController.ParseInput("Northwest"); // Move Northwest to Master Bedroom
                Assert.That(gameController.Prompt, Is.EqualTo("22: Which direction do you want to go (or type 'check'): "), "prompt in Master Bedroom");
                gameController.ParseInput("East"); // Move East to Master Bath
                Assert.That(gameController.Prompt, Is.EqualTo("23: Which direction do you want to go (or type 'check'): "), "prompt in Master Bath");
            });
        }

        [Test]
        [Category("GameController Prompt")]
        public void Test_GameController_Prompt_InLocationsWithNoHidingPlace()
        {
            Assert.Multiple(() =>
            {
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "prompt in StartingPoint");
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go: "), "prompt in Hallway");
                gameController.ParseInput("Up"); // Move Up to Landing
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go: "), "prompt on Landing");
            });
        }

        [Test]
        [Category("GameController MoveNumber")]
        public void Test_GameController_MoveNumber_IncrementsWithEachMove()
        {
            Assert.Multiple(() =>
            {
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number at start of game");
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after first move");
                gameController.ParseInput("Up"); // Move Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after second move");
                gameController.ParseInput("Up"); // Move Up to Attic
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after third move");
                gameController.ParseInput("Down"); // Move Down to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "move number after fourth move");
            });
        }

        [Test]
        [Category("GameController MoveNumber")]
        public void Test_GameController_MoveNumber_IncrementsWhenCheckForOpponents()
        {
            Assert.Multiple(() =>
            {
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number at start of game");
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after first move");
                gameController.ParseInput("North"); // Move North to Bathroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after second move");
                gameController.ParseInput("check"); // Check Bathroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after first check");
                gameController.ParseInput("South"); // Move North to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "move number after fourth move");
                gameController.ParseInput("Up"); // Move Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number after fifth move");
                gameController.ParseInput("Northwest"); // Move Northwest to Master Bedroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after sixth move");
                gameController.ParseInput("check"); // Check Master Bedroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number second check");
                gameController.ParseInput("East"); // Move East to Master Bath
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number after eighth move");
                gameController.ParseInput("check"); // Check Master Bath
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number third check");
            });
        }
    }
}
