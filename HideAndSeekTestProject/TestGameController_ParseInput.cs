using HideAndSeek;
using System.IO.Abstractions;
using System.Collections.Generic;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for moving and checking for Opponents via ParseInput method in default House,
    /// and value of Prompt property as navigate through House.
    /// Automatically tests GameController constructor with only DefaultHouse passed in.
    /// Does not include save/load/delete game tests (contained in separate file).
    /// </summary>
    public class TestGameController_ParseInput
    {
        GameController gameController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set variable to text representing serialized default House for test
            string textInHouseFile =
                #region default House file text
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
            #endregion

            // Set static House file system to mock file system (not changed in any tests)
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", textInHouseFile);
        }

        [SetUp]
        public void SetUp()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
            gameController = new GameController("DefaultHouse"); // Create new GameController with default House layout
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [TestCase("north", "south", "east", "west", "northeast", "southwest", "southeast", "northwest", "up", "down", "in", "out")]
        [TestCase("n", "s", "e", "w", "ne", "sw", "se", "nw", "u", "d", "i", "o")]
        public void Test_GameController_ParseInput_Move_InLowercaseDirection_AndCheckMessage(
            string north, string south, string east, string west, string northeast, string southwest, 
            string southeast, string northwest, string up, string down, string inText, string outText)
        {
            Assert.Multiple(() =>
            {
                // Go Out to Garage
                Assert.That(gameController.ParseInput(outText), Is.EqualTo("Moving Out"), "Out to Garage");

                // Go In to Entry
                Assert.That(gameController.ParseInput(inText), Is.EqualTo("Moving In"), "In to Entry");

                // Go East to Hallway
                Assert.That(gameController.ParseInput(east), Is.EqualTo("Moving East"), "East to Hallway");

                // Go North to Bathroom
                Assert.That(gameController.ParseInput(north), Is.EqualTo("Moving North"), "North to Bathroom");

                // Go South to Hallway
                Assert.That(gameController.ParseInput(south), Is.EqualTo("Moving South"), "South to Hallway");

                // Go Northwest to Kitchen
                Assert.That(gameController.ParseInput(northwest), Is.EqualTo("Moving Northwest"), "Northwest to Kitchen");

                // Go Southeast to Hallway
                Assert.That(gameController.ParseInput(southeast), Is.EqualTo("Moving Southeast"), "Southeast to Hallway");

                // Go Up to Landing
                Assert.That(gameController.ParseInput(up), Is.EqualTo("Moving Up"), "Up to Landing");

                // Go Southwest to Nursery
                Assert.That(gameController.ParseInput(southwest), Is.EqualTo("Moving Southwest"), "Southwest to Nursery");

                // Go Northeast to Landing
                Assert.That(gameController.ParseInput(northeast), Is.EqualTo("Moving Northeast"), "Northeast to Landing");

                // Go Down to Hallway
                Assert.That(gameController.ParseInput(down), Is.EqualTo("Moving Down"), "Down to Hallway");

                // Go West to Entry
                Assert.That(gameController.ParseInput(west), Is.EqualTo("Moving West"), "West to Entry");
            });
        }

        [TestCase("E")]
        [TestCase("East")]
        [TestCase("eAst")]
        [TestCase("eASt")]
        [TestCase("EAST")]
        [Category("GameController ParseInput Move Message Success")]
        public void Test_GameController_ParseInput_Move_InMixedCaseDirection_AndCheckMessage(string directionText)
        {
            Assert.That(gameController.ParseInput(directionText), Is.EqualTo("Moving East"));
        }

        [Test]
        [Category("GameController ParseInput Move Message Status Success")]
        public void Test_GameController_ParseInput_Move_InCapitalizedDirection_AndCheckMessageAndStatus()
        {
            Assert.Multiple(() =>
            {
                // Move East from Entry to Hallway.
                Assert.That(gameController.ParseInput("EAST"), Is.EqualTo("Moving East"), "parsing \"East\" from Entry returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Hallway. You see the following exits:" +
                    Environment.NewLine + " - the Landing is Up" +
                    Environment.NewLine + " - the Bathroom is to the North" +
                    Environment.NewLine + " - the Living Room is to the South" +
                    Environment.NewLine + " - the Entry is to the West" +
                    Environment.NewLine + " - the Kitchen is to the Northwest" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Hallway");

                // Move Up from Hallway to Landing.
                Assert.That(gameController.ParseInput("UP"), Is.EqualTo("Moving Up"), "parsing \"Up\" from Hallway returns appropriate text");
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
                Assert.That(gameController.ParseInput("NORTHWEST"), Is.EqualTo("Moving Northwest"), "parsing \"Northwest\" from Landing returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Master Bedroom. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the Southeast" +
                    Environment.NewLine + " - the Master Bath is to the East" +
                    Environment.NewLine + "Someone could hide under the bed" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Master Bedroom");

                // Move East from Master Bedroom to Master Bath.
                Assert.That(gameController.ParseInput("EAST"), Is.EqualTo("Moving East"), "parsing \"East\" from Master Bedroom returns appropriate text");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Master Bath. You see the following exits:" +
                    Environment.NewLine + " - the Master Bedroom is to the West" +
                    Environment.NewLine + "Someone could hide in the tub" +
                    Environment.NewLine + "You have not found any opponents"), "game status appropriately changed to text for Master Bath");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("}{yaeu\\@!//")]
        [TestCase("No")]
        [TestCase("Northuperly")]
        [Category("GameController ParseInput Move Message Status Failure")]
        public void Test_GameController_ParseInput_Move_InInvalidDirection_AndCheckMessageAndStatus(string directionText)
        {
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                Assert.That(gameController.ParseInput(directionText), Is.EqualTo("That's not a valid direction"), "parsing \"X\" returns invalid direction error message");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "game status does not change");
            });
        }

        [Test]
        [Category("GameController ParseInput Move Message Status Failure")]
        public void Test_GameController_ParseInput_Move_InDirectionWithNoLocation_AndCheckMessageAndStatus()
        {
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                Assert.That(gameController.ParseInput("Up"), Is.EqualTo("There's no exit in that direction"), "parsing returns no exit in that direction error message");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "game status does not change");
            });
        }

        [Test]
        [Category("GameController ParseInput Teleport Message Prompt Status MoveNumber GameOver Success")]
        public void Test_GameController_ParseInput_Teleport()
        {
            // Set House Random number generator to mock random
            House.Random = new MockRandomWithValueList([0]);

            Assert.Multiple(() =>
            {
                // Teleport and check return message
                Assert.That(gameController.ParseInput("teleport"), Is.EqualTo("Teleporting to random location with hiding place: Attic"), "message");

                // Check other game properties
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Attic"), "current location");
                Assert.That(gameController.Status, Is.EqualTo("You are in the Attic. You see the following exits:"
                    + Environment.NewLine + " - the Landing is Down"
                    + Environment.NewLine + "Someone could hide in a trunk"
                    + Environment.NewLine + "You have not found any opponents"), "status");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "prompt");
                Assert.That(gameController.GameOver, Is.False, "game not over");
            });
        }

        /// <summary>
        /// Using ParseInput method, mimic full game and check ParseInput return message and GameController's public properties:
        /// Prompt, Status, MoveNumber, GameOver
        /// Also tests Opponent names and hiding places set with constructor without Opponent details specified
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
        /// -I used a custom method to hide Opponents.
        /// -I added/edited some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [Test]
        [Category("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Success")]
        public void Test_GameController_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties()
        {
            // Hide Opponents is specific hiding places
            gameController.RehideAllOpponents(new List<string>() { "Garage", "Kitchen", "Attic", "Attic", "Kitchen" });

            Assert.Multiple(() =>
            {
                // Assert that game is not over at beginning
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Check the starting point (Entry) -- there are no players hiding there
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "check game move number");
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("There is no hiding place in the Entry"), "check string returned when check in StartingPoint");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "check game move number");

                // Move to the Garage
                gameController.ParseInput("Out");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "check game move number");

                // Check the Garage for opponents
                // We hid Joe in the Garage, so validate ParseInput's return value and the properties
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("You found 1 opponent hiding behind the car"), "check string returned when check in Garage");

                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exits:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status after finding opponent in Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go (or type 'check'): "), "check prompt after finding opponent in Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "check game move number");

                // Move to the Bathroom, where nobody is hiding
                gameController.ParseInput("In");
                gameController.ParseInput("East");
                gameController.ParseInput("North");

                // Check the Bathroom to make sure nobody is hiding there
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("Nobody was hiding behind the door"), "check string returned when check in Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "check game move number");

                // Move to the Kitchen
                gameController.ParseInput("South");
                gameController.ParseInput("Northwest");

                // Check the Kitchen to make sure nobody is hiding there and check message and properties
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("You found 2 opponents hiding next to the stove"), "check string returned when check in Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Joe, Bob, Jimmy"), "check status after finding opponents in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go (or type 'check'): "), "check prompt after finding opponents in Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "check game move number");
                Assert.That(gameController.GameOver, Is.False, "check game not over after finding opponents in Kitchen");

                // Head up to the Landing and check move number
                gameController.ParseInput("Southeast");
                gameController.ParseInput("Up");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "check game move number");

                // Move to the Pantry
                gameController.ParseInput("South");

                // Check the Pantry (nobody's hiding there)
                Assert.That(gameController.ParseInput("check"), Is.EqualTo("Nobody was hiding inside a cabinet"), "check string returned when check in Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(15), "check game move number");

                // Move to the Attic and check move number
                gameController.ParseInput("North");
                gameController.ParseInput("Up");
                Assert.That(gameController.MoveNumber, Is.EqualTo(17), "check game move number");

                // Check the Attic to find the last two opponents, check message and properties, make sure the game is over
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
                // Move to Garage and check prompt
                gameController.ParseInput("Out"); // Move Out to Garage
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "prompt in Garage");
                
                // Move to Bathroom and check prompt
                gameController.ParseInput("In"); // Move In to Entry
                gameController.ParseInput("East"); // Move East to Hallway
                gameController.ParseInput("North"); // Move North to Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "prompt in Bathroom");
                
                // Move to Living Room and check prompt
                gameController.ParseInput("South"); // Move South to Hallway
                gameController.ParseInput("South"); // Move South to Living Room
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "prompt in Living Room");
                
                // Move to Kitchen and check prompt
                gameController.ParseInput("North"); // Move North to Hallway
                gameController.ParseInput("Northwest"); // Move Northwest to Kitchen
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "prompt in Kitchen");
                
                // Move to Attic and check prompt
                gameController.ParseInput("Southeast"); // Move Southeast to Hallway
                gameController.ParseInput("Up"); // Move Up to Landing
                gameController.ParseInput("Up"); // Move Up to Attic
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "prompt in Attic");
                
                // Move to Kids Room and check prompt
                gameController.ParseInput("Down"); // Move Down to Landing
                gameController.ParseInput("Southeast"); // Move Southeast to Kids Room
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "prompt in Kids Room");
                
                // Move to Pantry and check prompt
                gameController.ParseInput("Northwest"); // Move Northwest to Landing
                gameController.ParseInput("South"); // Move South to Pantry
                Assert.That(gameController.Prompt, Is.EqualTo("16: Which direction do you want to go (or type 'check'): "), "prompt in Pantry");
                
                // Move to Second Bathroom and check prompt
                gameController.ParseInput("North"); // Move North to Landing
                gameController.ParseInput("West"); // Move West to Second Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("18: Which direction do you want to go (or type 'check'): "), "prompt in Second Bathroom");
                
                // Move to Nursery and check prompt
                gameController.ParseInput("East"); // Move East to Landing
                gameController.ParseInput("Southwest"); // Move Southwest to Nursery
                Assert.That(gameController.Prompt, Is.EqualTo("20: Which direction do you want to go (or type 'check'): "), "prompt in Nursery");
                
                // Move to Master Bedroom and check prompt
                gameController.ParseInput("Northeast"); // Move Northeast to Landing
                gameController.ParseInput("Northwest"); // Move Northwest to Master Bedroom
                Assert.That(gameController.Prompt, Is.EqualTo("22: Which direction do you want to go (or type 'check'): "), "prompt in Master Bedroom");
                
                // Move to Master Bath and check prompt
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
                // Check prompt in Entry
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "prompt in Entry");
                
                // Move to Hallway and check prompt
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go: "), "prompt in Hallway");
                
                // Move to Landing and check prompt
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
                // Check move number at start of game
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number at start of game");

                // Move for first time and check move number
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after first move");

                // Move for second time and check move number
                gameController.ParseInput("Up"); // Move Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after second move");

                // Move for third time and check move number
                gameController.ParseInput("Up"); // Move Up to Attic
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after third move");

                // Move for fourth time and check move number
                gameController.ParseInput("Down"); // Move Down to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "move number after fourth move");
            });
        }

        [Test]
        [Category("GameController MoveNumber Check")]
        public void Test_GameController_MoveNumber_IncrementsWhenCheckForOpponents()
        {
            Assert.Multiple(() =>
            {
                // Check move number at start of game
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number at start of game");

                // Move to Hallway and check move number
                gameController.ParseInput("East"); // Move East to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after first move");

                // Move to Bathroom and check move number
                gameController.ParseInput("North"); // Move North to Bathroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after second move");

                // Check for opponents in Bathroom and check move number
                gameController.ParseInput("check"); // Check Bathroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number after first check");

                // Move to Hallway and check move number
                gameController.ParseInput("South"); // Move North to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "move number after fourth move");

                // Move to Landing and check move number
                gameController.ParseInput("Up"); // Move Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number after fifth move");

                // Move to Master Bedroom and check move number
                gameController.ParseInput("Northwest"); // Move Northwest to Master Bedroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after sixth move");

                // Check for opponents in Master Bedroom and check move number
                gameController.ParseInput("check"); // Check Master Bedroom
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number second check");

                // Move to Master Bath and check move number
                gameController.ParseInput("East"); // Move East to Master Bath
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number after eighth move");

                // Check for opponents in Master Bath and check move number
                gameController.ParseInput("check"); // Check Master Bath
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number third check");
            });
        }
    }
}