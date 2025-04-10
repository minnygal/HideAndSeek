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
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 1 opponent
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNumber_1Opponent(GameController gameController)
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
                    Environment.NewLine + "You have found 1 of 1 opponent: Joe"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 1 opponent
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNames_1Opponent(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Amy
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 1 of 1 opponent: Amy"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 2 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNumber_2Opponents(GameController gameController)
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
                    Environment.NewLine + "You have found 1 of 2 opponents: Joe"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
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
                        Environment.NewLine + "You have found 1 of 2 opponents: Joe"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number when enter Pantry");

                // Check Pantry and find Bob
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 2 of 2 opponents: Joe, Bob"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 2 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNames_2Opponents(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Amy
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 1 of 2 opponents: Amy"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
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
                        Environment.NewLine + "You have found 1 of 2 opponents: Amy"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number when enter Pantry");

                // Check Pantry and find John
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 2 of 2 opponents: Amy, John"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 6 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNumber_6Opponents(GameController gameController)
        {
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
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 4 of 6 opponents: Joe, Owen, Mary, Ana"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");

                // Check Pantry and find Bob and Jimmy
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 6 of 6 opponents: Joe, Owen, Mary, Ana, Bob, Jimmy"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 6 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNames_6Opponents(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Amy, Robert, and Zelda
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 3 opponents hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 3 of 6 opponents: Amy, Robert, Zelda"), "status after check Kitchen");
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
                    Environment.NewLine + "You have found 3 of 6 opponents: Amy, Robert, Zelda"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Wendy
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 4 of 6 opponents: Amy, Robert, Zelda, Wendy"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 4 opponents");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 4 of 6 opponents: Amy, Robert, Zelda, Wendy"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");

                // Check Pantry and find John and Gina
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 6 of 6 opponents: Amy, Robert, Zelda, Wendy, John, Gina"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 9 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNumber_9Opponents(GameController gameController)
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
                    Environment.NewLine + "You have found 4 of 9 opponents: Joe, Owen, Mary, Andy"), "status after check Kitchen");
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
                    Environment.NewLine + $"You have found 4 of 9 opponents: Joe, Owen, Mary, Andy"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Ana and Tony
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + $"You have found 6 of 9 opponents: Joe, Owen, Mary, Andy, Ana, Tony"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 6 opponent");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 6 of 9 opponents: Joe, Owen, Mary, Andy, Ana, Tony"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");
                Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                // Check Pantry and find Bob, Jimmy, And Alice
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 3 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 9 of 9 opponents: Joe, Owen, Mary, Andy, Ana, Tony, Bob, Jimmy, Alice"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 10 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNumber_10Opponents(GameController gameController)
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
                    Environment.NewLine + "You have found 4 of 10 opponents: Joe, Owen, Mary, Andy"), "status after check Kitchen");
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
                    Environment.NewLine + "You have found 4 of 10 opponents: Joe, Owen, Mary, Andy"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Ana and Tony
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 6 of 10 opponents: Joe, Owen, Mary, Andy, Ana, Tony"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 6 opponent");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 6 of 10 opponents: Joe, Owen, Mary, Andy, Ana, Tony"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");
                Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                // Check Pantry and find Bob, Jimmy, Alice, and Jill
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 4 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 10 of 10 opponents: Joe, Owen, Mary, Andy, Ana, Tony, Bob, Jimmy, Alice, Jill"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 10 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNames_10Opponents(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Amy, Robert, Zelda, and Rose
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 4 opponents hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 4 of 10 opponents: Amy, Robert, Zelda, Rose"), "status after check Kitchen");
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
                    Environment.NewLine + "You have found 4 of 10 opponents: Amy, Robert, Zelda, Rose"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Wendy and Benjamin
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 6 of 10 opponents: Amy, Robert, Zelda, Rose, Wendy, Benjamin"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 6 opponent");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 6 of 10 opponents: Amy, Robert, Zelda, Rose, Wendy, Benjamin"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");
                Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                // Check Pantry and find John, Gina, Paul, and Mike
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 4 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 10 of 10 opponents: Amy, Robert, Zelda, Rose, Wendy, Benjamin, John, Gina, Paul, Mike"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method for FinishGame parameter of 
        /// Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 15 opponents
        /// </summary>
        /// <param name="gameController">Game controller on move 5 in Hallway</param>
        /// <returns>Game controller after game finished</returns>
        private static GameController FinishGame_SpecifiedNames_15Opponents(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                // Go to Kitchen
                gameController.ParseInput("Northwest"); // Go Northwest to Kitchen
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "move number when enter Kitchen");

                // Check Kitchen and find Amy, Robert, Zelda, Rose, Patrick, and Sarah
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 6 opponents hiding next to the stove"), "message when checking Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + "Someone could hide next to the stove" +
                    Environment.NewLine + "You have found 6 of 15 opponents: Amy, Robert, Zelda, Rose, Patrick, Sarah"), "status after check Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number after check Kitchen");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 6 opponents");

                // Go to Hallway
                gameController.ParseInput("Southeast"); // Go Southeast to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "move number when enter Hallway from Kitchen");

                // Go to Bathroom
                gameController.ParseInput("North"); // Go North to Bathroom
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 6 of 15 opponents: Amy, Robert, Zelda, Rose, Patrick, Sarah"), "status when enter Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "move number when enter Bathroom");

                // Check Bathroom and find Wendy, Benjamin, and Chris
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 3 opponents hiding behind the door"), "message when checking Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the South" +
                    Environment.NewLine + "Someone could hide behind the door" +
                    Environment.NewLine + "You have found 9 of 15 opponents: Amy, Robert, Zelda, Rose, Patrick, Sarah, Wendy, " +
                                          "Benjamin, Chris"), "status after check Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number after check Bathroom");
                Assert.That(gameController.GameOver, Is.False, "game not over after find first 9 opponent");

                // Go to Hallway
                gameController.ParseInput("South"); // Go South to Hallway
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "move number when enter Hallway from Bathroom");

                // Go to Landing
                gameController.ParseInput("Up"); // Go Up to Landing
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "move number when enter Landing from Hallway");

                // Go to Pantry
                gameController.ParseInput("South"); // Go South to Pantry
                Assert.That(gameController.Status, Is.EqualTo(
                        "You are in the Pantry. You see the following exits:" +
                        Environment.NewLine + " - the Landing is to the North" +
                        Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 9 of 15 opponents: Amy, Robert, Zelda, Rose, Patrick, Sarah, Wendy, " +
                                          "Benjamin, Chris"), "status when enter Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "move number when enter Pantry");
                Assert.That(gameController.GameOver, Is.False, "game not over after find 6 opponents");

                // Check Pantry and find John, Gina, Paul, Mike, Cassie, and Jonathan
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 6 opponents hiding inside a cabinet"), "message when checking Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exits:" +
                    Environment.NewLine + " - the Landing is to the North" +
                    Environment.NewLine + "Someone could hide inside a cabinet" +
                    Environment.NewLine + "You have found 15 of 15 opponents: Amy, Robert, Zelda, Rose, Patrick, Sarah, Wendy, " +
                                          "Benjamin, Chris, John, Gina, Paul, Mike, Cassie, Jonathan"), "status after check Pantry");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "move number after check Pantry");
            });

            // Return game controller
            return gameController;
        }

        public static IEnumerable TestCases_For_Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties
        {
            get
            {
                // SPECIFIED NUMBER OF OPPONENTS
                // Specified number of Opponents - 1
                yield return new TestCaseData( // Joe in Kitchen
                    new int[] { 1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4 }, // Hide Opponent in Kitchen
                    () => new GameController(1, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNumber_1Opponent(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 1 opponent")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNumberOfOpponents Success");

                // Specified number of Opponents - 2
                yield return new TestCaseData( // Joe in Kitchen, Bob in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 // Hide opponent in Pantry
                    },
                    () => new GameController(2, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNumber_2Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 2 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNumberOfOpponents Success");
                
                // Specified number of Opponents - 6
                yield return new TestCaseData( // Joe, Owen, and Mary in Kitchen; Ana in Bathroom; Bob and Jimmy in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(6, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNumber_6Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 6 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNumberOfOpponents Success");
                
                // Specified number of Opponents - 9
                yield return new TestCaseData( // Joe, Owen, Mary, and Andy in Kitchen; Ana and Tony in Bathroom; Bob, Jimmy, and Alice in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(9, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNumber_9Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 9 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNumberOfOpponents Success");

                // Specified number of Opponents - 10
                yield return new TestCaseData( // Joe, Owen, Mary, and Andy in Kitchen; Ana and Tony in Bathroom; Bob, Jimmy, Alice, and Jill in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(10, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNumber_10Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify number - 10 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNumberOfOpponents Success");

                // SPECIFIED NAMES OF OPPONENTS
                // Specified names of Opponents - 1
                yield return new TestCaseData( // Amy in Kitchen
                    new int[] { 1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4 }, // Hide Opponent in Kitchen
                    () => new GameController(new string[] { "Amy" }, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNames_1Opponent(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 1 opponent")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNamesOfOpponents Success");

                // Specified names of Opponents - 3
                yield return new TestCaseData( // Amy in Kitchen, John in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2 // Hide opponent in Pantry
                    },
                    () => new GameController(new string[] { "Amy", "John" }, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNames_2Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 2 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNamesOfOpponents Success");

                // Specified names of Opponents - 6
                yield return new TestCaseData( // Amy, Robert, and Zelda in Kitchen; Wendy in Bathroom; John and Gina in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(new string[] { "Amy", "John", "Wendy", "Robert", "Gina", "Zelda" }, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNames_6Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 6 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNamesOfOpponents Success");

                // Specified names of Opponents - 10
                yield return new TestCaseData( // Amy, Robert, Zelda, and Rose in Kitchen; Wendy and Benjamin in Bathroom; John, Gina, Paul, and Mike in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(new string[] { "Amy", "John", "Wendy", "Robert", "Gina", "Zelda", "Paul", "Benjamin", "Rose", "Mike"}, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNames_10Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 10 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNamesOfOpponents Success");

                // Specified names of Opponents - 15
                yield return new TestCaseData( // Amy, Robert, Zelda, Rose, Patrick, and Sarah in Kitchen; Wendy, Benjamin, and Chris in Bathroom; John, Gina, Paul, Mike, Cassie, and Jonathan in Pantry
                    new int[] {
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                        1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent in Bathroom
                        1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent in Kitchen
                        0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent in Pantry
                    },
                    () => new GameController(new string[] { "Amy", "John", "Wendy", "Robert", "Gina", "Zelda", "Paul", "Benjamin", "Rose", "Mike",
                                                            "Patrick", "Cassie", "Chris", "Sarah", "Jonathan" }, "DefaultHouse"),
                    (GameController gameController) => FinishGame_SpecifiedNames_15Opponents(gameController))
                .SetName("Test_GameController_ParseInput_ForFullGame_WithCustomOpponents_AndCheckMessageAndProperties - specify names - 15 opponents")
                .SetCategory("GameController ParseInput Move Check Message Prompt Status MoveNumber GameOver Constructor SpecifiedNamesOfOpponents Success");
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
