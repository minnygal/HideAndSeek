using Moq;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for RestartGame and RehideAllOpponents methods
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    [TestFixture]
    public class TestGameController_RestartRehide
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            // Set static House properties
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                                "DefaultHouse.house.json", TestGameController_RestartRehide_TestData.DefaultHouse_Serialized); // Set static House file system to mock file system
            House.Random = new Random(); // Set static House Random property to new Random number generator

            // Create Opponent mocks
            Mock<Opponent> opponent1 = new Mock<Opponent>();
            opponent1.Setup((o) => o.Name).Returns("Joe");

            Mock<Opponent> opponent2 = new Mock<Opponent>();
            opponent2.Setup((o) => o.Name).Returns("Bob");

            Mock<Opponent> opponent3 = new Mock<Opponent>();
            opponent3.Setup((o) => o.Name).Returns("Ana");

            Mock<Opponent> opponent4 = new Mock<Opponent>();
            opponent4.Setup((o) => o.Name).Returns("Owen");

            Mock<Opponent> opponent5 = new Mock<Opponent>();
            opponent5.Setup((o) => o.Name).Returns("Jimmy");

            // Create new GameController with mocked Opponents and default House layout
            gameController = new GameController(
                new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object },
                "DefaultHouse");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [TestCaseSource(typeof(TestGameController_RestartRehide_TestData), nameof(TestGameController_RestartRehide_TestData.TestCases_For_Test_GameController_RestartGame))]
        [Category("GameController RestartGame HidingLocations Success")]
        public void Test_GameController_RestartGame(Func<GameController, GameController> GetGameController)
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 1 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 2 in Pantry
                1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent 3 in Bathroom
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 4 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 5 in Pantry
            ];

            // Create mock random number generator
            Random mockRandomNumberGenerator = new MockRandomWithValueList(mockRandomValuesList);

            // Set House random number generator to mock random
            House.Random = mockRandomNumberGenerator;

            // Set GameController
            gameController = GetGameController(gameController);

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), 
                    Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponent hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no found opponents");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Entry"), "current location");
                Assert.That(gameController.GameOver, Is.False, "game not over");
            });
        }

        [Test]
        [Category("GameController RehideAllOpponents HidingLocations Success")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces()
        {
            // Create enumerable of hiding places for opponents to hide
            IEnumerable<string> hidingPlaces = new List<string>()
            {
                "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry"
            };

            // Hide all opponents in specified locations
            gameController.RehideAllOpponents(hidingPlaces);

            // Assert that hiding places (values) in OpponentsAndHidingLocations dictionary are set correctly
            Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(hidingPlaces));
        }

        [Test]
        [Category("GameController RehideAllOpponents InvalidOperationException Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForNonexistentLocation()
        {
            Assert.Multiple(() =>
            {
                // Assert that hiding an Opponent in a location with an invalid name raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    gameController.RehideAllOpponents(new List<string>() { "Dungeon", "Lavatory", "Eggshells", "Worm Hole", "Zoo" });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("location with hiding place \"Dungeon\" does not exist in House"));
            });
        }

        [TestCase()]
        [TestCase("Living Room")]
        [TestCase("Living Room", "Kitchen")]
        [TestCase("Living Room", "Kitchen", "Pantry")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic", "Master Bedroom", "Kids Room")]
        [Category("GameController RehideAllOpponents ArgumentOutOfRangeException Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForIncorrectNumberOfHidingPlaces(params string[] hidingPlaces)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling method with an enumerable with an incorrect number of hiding places raises an exception
                var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    gameController.RehideAllOpponents(hidingPlaces);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("The number of hiding places must equal the number of opponents. (Parameter 'hidingPlaces')"));
            });
        }
    }
}