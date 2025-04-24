using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for custom Opponents
    /// 
    /// The success tests for constructors accepting the number or names of opponents
    /// are integration tests using House, Opponent, Location, and LocationWithHidingPlace.
    /// 
    /// The success tests for the constructor accepting Opponent objects
    /// are integration tests using House, Location, and LocationWithHidingPlace.
    /// 
    /// Failure tests are not integration tests.
    /// </summary>
    [TestFixture]
    public class TestGameController_CustomOpponents
    {
        GameController gameController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set House file system to return text for default House
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.house.json", 
                                    TestGameController_CustomOpponents_TestData.DefaultHouse_Serialized);
        }

        [SetUp]
        public void Setup()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData), 
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNumberOfOpponents))]
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNumberOfOpponents(
            int numberOfOpponents, string[] expectedNames, string[] expectedHidingPlaces, int[] mockRandomValuesList)
        {
            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController
            gameController = new GameController(numberOfOpponents, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(expectedNames), "Opponents' expectedNames");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(11)]
        [TestCase(500)]
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForInvalidNumberOfOpponents(int numberOfOpponents)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with invalid number of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(numberOfOpponents, "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController " +
                                                          "because the number of Opponents specified is invalid (must be between 1 and 10)"));
            });
        }
        
        /// <summary>
        /// Test that playing full game with custom Opponents 
        /// (either number or names set in GameController constructor)
        /// goes as expected, checking return messages and GameController properties
        /// 
        /// NOTE: Don't hide any Opponents in garage because test expects Garage to be empty
        /// NOTE: FinishGame is called when player is in Hallway on move 5 (after checking empty Garage)
        /// </summary>
        /// <param name="mockRandomValuesList">Values for mock random for House Random (determines Opponents' hiding locations)</param>
        /// <param name="CreateGameController">Function to return GameController set up with custom Opponents</param>
        /// <param name="FinishGame">Function to return GameController after finish game, making assertions along the way</param>
        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData), 
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_FullGame_WithCustomOpponents_AndCheckMessageAndProperties))]
        public void Test_GameController_FullGame_WithCustomOpponents_AndCheckMessageAndProperties(
            int[] mockRandomValuesList, Func<GameController> CreateGameController, Func<GameController, GameController> FinishGame)
        {
            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController
            gameController = CreateGameController();

            // Play game and make assertions
            Assert.Multiple(() =>
            {
                // Check properties when game started
                Assert.That(gameController.GameOver, Is.False, "game not over at beginning");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the East" +
                    Environment.NewLine + " - the Garage is Out" +
                    Environment.NewLine + "You have not found any opponents"), "status at beginning of game");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "prompt at beginning of game");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number at beginning of game");

                // Go to Garage and check properties
                Assert.That(gameController.Move(Direction.Out), Is.EqualTo("Moving Out"), "message when moving out to Garage"); // Go Out to Garage
                Assert.That(gameController.GameOver, Is.False, "game not over when enter Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have not found any opponents"), "status when enter Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "prompt when enter Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number when enter Garage");

                // Check Garage and check properties
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the car"), "message when checking Garage"); // Check Garage, no Opponents found
                Assert.That(gameController.GameOver, Is.False, "game not over after check Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" +
                    Environment.NewLine + " - the Entry is In" +
                    Environment.NewLine + "Someone could hide behind the car" +
                    Environment.NewLine + "You have not found any opponents"), "status after check Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "prompt after check Garage");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "move number after check Garage");

                // Move to Entry, then Hallway
                gameController.Move(Direction.In); // Go In to Entry
                gameController.Move(Direction.East); // Go East to Hallway

                // Play game to end, making assertions along the way
                gameController = FinishGame(gameController);

                // Assert that game is over
                Assert.That(gameController.GameOver, Is.True, "game over at end");
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData), 
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNamesOfOpponents))]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNamesOfOpponents(string[] names, string[] expectedHidingPlaces)
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 1 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 2 in Pantry
                1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent 3 in Bathroom
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 4 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 5 in Pantry
            ];

            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController
            gameController = new GameController(names, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(names), "Opponents' expectedNames");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [Test]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForEmptyArrayOfNamesOfOpponents()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty array of names of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(Array.Empty<string>(), "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no names for Opponents were passed in"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForInvalidOpponentName(string invalidName)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with array with invalid name of Opponent raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(new string[] {invalidName}, "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith($"opponent name \"{invalidName}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData),
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedOpponents))]
        [Category("GameController Constructor SpecifiedOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedOpponents(Opponent[] opponents, string[] expectedHidingPlaces)
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 1 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 2 in Pantry
                1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent 3 in Bathroom
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 4 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 5 in Pantry
            ];

            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController with Opponents
            gameController = new GameController(opponents, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys, Is.EquivalentTo(opponents), "Opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [Test]
        [Category("GameController Constructor SpecifiedOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForEmptyArrayOfOpponents()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty array of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(Array.Empty<Opponent>(), "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no Opponents were passed in"));
            });
        }
    }
}