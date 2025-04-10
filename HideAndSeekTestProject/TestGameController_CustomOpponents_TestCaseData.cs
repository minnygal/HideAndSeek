using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HideAndSeek
{
    public static class TestGameController_CustomOpponents_TestCaseData
    {
        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized
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

        public static IEnumerable TestCases_For_Test_GameController_Constructor_WithSpecifiedNumberOfOpponents
        {
            get
            {
                yield return new TestCaseData(
                    1,
                    new string[] { "Joe" },
                    new string[] { "Kitchen" },
                    new int[] { 1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4 }) // Hide Opponent in Kitchen
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 1 opponent");

                yield return new TestCaseData(
                    2,
                    new string[] { "Joe", "Bob" },
                    new string[] { "Kitchen", "Pantry" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 2 opponents");

                yield return new TestCaseData(
                    3,
                    new string[] { "Joe", "Bob", "Ana" },
                    new string[] { "Kitchen", "Pantry", "Bathroom" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 3 opponents");

                yield return new TestCaseData(
                    4,
                    new string[] { "Joe", "Bob", "Ana", "Owen" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 4 opponents");

                yield return new TestCaseData(
                    5,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 5 opponents");

                yield return new TestCaseData(
                    6,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 6 opponents");

                yield return new TestCaseData(
                    7,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 7 opponents");

                yield return new TestCaseData(
                    8,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 8 opponents");

                yield return new TestCaseData(
                    9,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 9 opponents");

                yield return new TestCaseData(
                    10,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy", "Jill" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" },
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 10 opponents");
            }
        }

        /// <summary>
        /// Helper method to find first Opponent Joe in Kitchen
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="ofHowManyOpponentsStatusText">Text in status telling total number of Opponents</param>
        /// <returns>Game controller after first Opponent found</returns>
        private static GameController CheckKitchenAndFind1Opponent_DefaultOpponentName(GameController gameController, string ofHowManyOpponentsStatusText)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Joe
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + $"You have found 1 {ofHowManyOpponentsStatusText}: Joe"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method to check Kitchen and Bathroom to find first 6 Opponents with default names
        /// and then go to Pantry (but don't check Pantry)
        /// (Joe, Owen, Mary, and Andy in Kitchen; Ana and Tony in Bathroom)
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="ofHowManyOpponentsStatusText">Text in status telling total number of Opponents</param>
        /// <returns>Game controller after sixth Opponent found and moved to Pantry</returns>
        private static GameController CheckKitchenAndBathroomAndFind6Opponents_DefaultOpponentNames(GameController gameController, string ofHowManyOpponentsStatusText)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Joe, Owen, Mary, and Andy
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 4 opponents hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + $"You have found 4 {ofHowManyOpponentsStatusText}: Joe, Owen, Mary, Andy"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 4 opponents");

                // Go to Hallway
                gameController.ParseInput("Southeast"); // Go Southeast to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number when enter Hallway from Kitchen");

                // Go to Bathroom
                gameController.ParseInput("North"); // Go North to Bathroom
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + $"You have found 4 {ofHowManyOpponentsStatusText}: Joe, Owen, Mary, Andy"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Ana
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + $"You have found 6 {ofHowManyOpponentsStatusText}: Joe, Owen, Mary, Andy, Ana, Tony"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 6 opponent");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("S"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + $"You have found 6 {ofHowManyOpponentsStatusText}: Joe, Owen, Mary, Andy, Ana, Tony"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");
            });

            // Return game controller
            return gameController;
        }

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties
        {
            get
            {
                yield return new TestCaseData( // Joe in Kitchen
                    1,
                    new int[] { 1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4 }, // Hide Opponent in Kitchen
                    (GameController gameController) => CheckKitchenAndFind1Opponent_DefaultOpponentName(gameController, "of 1 opponent"))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties - 1 opponent");

                yield return new TestCaseData( // Joe in Kitchen, Bob in Pantry
                    2,
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 // Hide opponent in Pantry
                    },
                    (GameController gameController) => {
                        Assert.Multiple(() =>
                        {
                            // Find first Opponent Joe in Kitchen
                            gameController = CheckKitchenAndFind1Opponent_DefaultOpponentName(gameController, "of 2 opponents");
                            Assert.That(gameController.GameOver, Is.False, "game not over after find first opponent");

                            // Go to Hallway
                            gameController.ParseInput("Southeast"); // Go Southeast to Hallway
                            Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number when enter Hallway from Kitchen");

                            // Go to Landing
                            gameController.ParseInput("Up"); // Go Up to Landing
                            Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Landing");

                            // Go to Pantry
                            gameController.ParseInput("South"); // Go South to Pantry
                            Assert.That(gameController.Status, Is.EqualTo(
                                    "You are in the Pantry. You see the following exits:" +
                                    Environment.NewLine + " - the Landing is to the North" +
                                    Environment.NewLine + "Someone could hide inside a cabinet" +
                                    Environment.NewLine + $"You have found 1 of 2 opponents: Joe"), "status when enter Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number when enter Pantry");

                            // Check Pantry and find Bob
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding inside a cabinet"), "message when checking Pantry");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Pantry. You see the following exits:" +
                                Environment.NewLine + " - the Landing is to the North" +
                                Environment.NewLine + "Someone could hide inside a cabinet" +
                                Environment.NewLine + $"You have found 2 of 2 opponents: Joe, Bob"), "status after check Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number after check Pantry");
                        });

                        // Return game controller
                        return gameController;
                    })
                .SetName("Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties - 2 opponents");
                
                yield return new TestCaseData( // Joe, Owen, and Mary in Kitchen; Ana in Bathroom; Bob and Jimmy in Pantry
                    6,
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    (GameController gameController) => {
                        Assert.Multiple(() =>
                        {
                            // Go to Kitchen
                            gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                            Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                            // Check Kitchen and find Joe, Owen, and Mary
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 3 opponents hiding next to the stove"), "message when checking Kitchen");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Kitchen. You see the following exits:" +
                                Environment.NewLine + " - the Hallway is to the Southeast" +
                                Environment.NewLine + "Someone could hide next to the stove" +
                                Environment.NewLine + "You have found 3 of 6 opponents: Joe, Owen, Mary"), "status after check Kitchen");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
                            Assert.That(gameController.GameOver, Is.False, "game not over after find first 3 opponent");

                            // Go to Hallway
                            gameController.ParseInput("Southeast"); // Go Southeast to Hallway
                            Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number when enter Hallway from Kitchen");

                            // Go to Bathroom
                            gameController.ParseInput("North"); // Go North to Bathroom
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Bathroom. You see the following exits:" +
                                Environment.NewLine + " - the Hallway is to the South" +
                                Environment.NewLine + "Someone could hide behind the door" +
                                Environment.NewLine + "You have found 3 of 6 opponents: Joe, Owen, Mary"), "status when enter Bathroom");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                            // Check Bathroom and find Ana
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding behind the door"), "message when checking Bathroom");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Bathroom. You see the following exits:" +
                                Environment.NewLine + " - the Hallway is to the South" +
                                Environment.NewLine + "Someone could hide behind the door" +
                                Environment.NewLine + "You have found 4 of 6 opponents: Joe, Owen, Mary, Ana"), "status after check Bathroom");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Kitchen");
                            Assert.That(gameController.GameOver, Is.False, "game not over after find first 4 opponents");

                            // Go to Hallway
                            gameController.ParseInput("South"); // Go South to Hallway
                            Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                            // Go to Landing
                            gameController.ParseInput("Up"); // Go Up to Landing
                            Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                            // Go to Pantry
                            gameController.ParseInput("S"); // Go South to Pantry
                            Assert.That(gameController.Status, Is.EqualTo(
                                    "You are in the Pantry. You see the following exits:" +
                                    Environment.NewLine + " - the Landing is to the North" +
                                    Environment.NewLine + "Someone could hide inside a cabinet" +
                                Environment.NewLine + $"You have found 4 of 6 opponents: Joe, Owen, Mary, Ana"), "status when enter Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");

                            // Check Pantry and find Bob and Jimmy
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding inside a cabinet"), "message when checking Pantry");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Pantry. You see the following exits:" +
                                Environment.NewLine + " - the Landing is to the North" +
                                Environment.NewLine + "Someone could hide inside a cabinet" +
                                Environment.NewLine + $"You have found 6 of 6 opponents: Joe, Owen, Mary, Ana, Bob, Jimmy"), "status after check Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
                        });

                        // Return game controller
                        return gameController;
                    })
                .SetName("Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties - 6 opponents");
                
                yield return new TestCaseData( // Joe, Owen, Mary, and Andy in Kitchen; Ana and Tony in Bathroom; Bob, Jimmy, and Alice in Pantry
                    9,
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    (GameController gameController) =>
                    {
                        Assert.Multiple(() =>
                        {
                            // Check Kitchen and Bathroom and find first six Opponent
                            gameController = CheckKitchenAndBathroomAndFind6Opponents_DefaultOpponentNames(gameController, "of 9 opponents");
                            Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                            // Check Pantry and find Bob, Jimmy, And Alice
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 3 opponents hiding inside a cabinet"), "message when checking Pantry");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Pantry. You see the following exits:" +
                                Environment.NewLine + " - the Landing is to the North" +
                                Environment.NewLine + "Someone could hide inside a cabinet" +
                                Environment.NewLine + $"You have found 9 of 9 opponents: Joe, Owen, Mary, Andy, Ana, Tony, Bob, Jimmy, Alice"), "status after check Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
                        });

                        // Return game controller
                        return gameController;
                    })
                .SetName("Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties - 9 opponents");
                
                yield return new TestCaseData( // Joe, Owen, Mary, and Andy in Kitchen; Ana and Tony in Bathroom; Bob, Jimmy, Alice, and Jill in Pantry
                    10,
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    (GameController gameController) => {
                        Assert.Multiple(() =>
                        {
                            // Check Kitchen and Bathroom and find first six Opponent
                            gameController = CheckKitchenAndBathroomAndFind6Opponents_DefaultOpponentNames(gameController, "of 10 opponents");
                            Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                            // Check Pantry and find Bob, Jimmy, Alice, and Jill
                            Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 4 opponents hiding inside a cabinet"), "message when checking Pantry");
                            Assert.That(gameController.Status, Is.EqualTo(
                                "You are in the Pantry. You see the following exits:" +
                                Environment.NewLine + " - the Landing is to the North" +
                                Environment.NewLine + "Someone could hide inside a cabinet" +
                                Environment.NewLine + $"You have found 10 of 10 opponents: Joe, Owen, Mary, Andy, Ana, Tony, Bob, Jimmy, Alice, Jill"), "status after check Pantry");
                            Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
                        });

                        // Return game controller
                        return gameController;
                    })
                .SetName("Test_GameController_ParseInput_ForFullGame_WithSpecifiedNumberOfOpponents_AndCheckMessageAndProperties - 10 opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_WithSpecifiedNamesOfOpponents
        {
            get
            {
                yield return new TestCaseData(
                    new string[] { "Henry" }, 
                    new string[] { "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 1 opponent");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia" }, 
                    new string[] { "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 2 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul" }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 3 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy" }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 4 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 5 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 6 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 7 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 8 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 9 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn", "Jackie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 10 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn", "Jackie", "Alice", "Steve", "Jenn", "Jude", "Jamie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 15 opponents");
            }
        }
    }
}
