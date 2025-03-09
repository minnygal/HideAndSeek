using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for:
    /// -moving via Move method
    /// -checking status of started game
    /// -checking Opponents' hiding places in started game
    /// -checking Opponents' hiding places in restarted game
    /// </summary>
    [TestFixture]
    public class GameControllerBasicTests
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            gameController = new GameController();
        }

        [Test]
        [Category("GameController HidingLocations")]
        public void Test_GameController_InitialHidingLocations()
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 1 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 2 in Pantry
                1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent 3 in Bathroom
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 4 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 5 in Pantry
            ];

            // Create new GameController
            gameController = new GameController();

            // Set House random number generator to mock random
            gameController.House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Reset game to rehide Opponents with new MockRandom
            gameController.RestartGame();

            // Assert that hiding places (values) in OpponentsAndHidingLocations dictionary are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(0).Value.ToString, Is.EqualTo("Kitchen"), "Opponent 1 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(1).Value.ToString, Is.EqualTo("Pantry"), "Opponent 2 hiding in Pantry");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(2).Value.ToString, Is.EqualTo("Bathroom"), "Opponent 3 hiding in Bathroom");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(3).Value.ToString, Is.EqualTo("Kitchen"), "Opponent 4 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(4).Value.ToString, Is.EqualTo("Pantry"), "Opponent 5 hiding in Pantry");
            });
        }

        [Test]
        [Category("GameController RehideAllOpponents HidingLocations")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces()
        {
            // Initialize variables for locations with hiding places to mocks
            LocationWithHidingPlace kitchen = new Mock<LocationWithHidingPlace>("Kitchen", "whatever").Object;
            LocationWithHidingPlace pantry = new Mock<LocationWithHidingPlace>("Pantry", "whatever").Object;
            LocationWithHidingPlace bathroom = new Mock<LocationWithHidingPlace>("Bathroom", "whatever").Object;

            // Create enumerable of hiding places for opponents to hide
            IEnumerable<LocationWithHidingPlace> hidingPlaces = new List<LocationWithHidingPlace>()
            {
                kitchen,
                pantry,
                bathroom,
                kitchen,
                pantry
            };

            // Hide all opponents in specified locations
            gameController.RehideAllOpponents(hidingPlaces);

            // Assert that hiding places (values) in OpponentsAndHidingLocations dictionary are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(0).Value, Is.EqualTo(kitchen), "Opponent 1 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(1).Value, Is.EqualTo(pantry), "Opponent 2 hiding in Pantry");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(2).Value, Is.EqualTo(bathroom), "Opponent 3 hiding in Bathroom");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(3).Value, Is.EqualTo(kitchen), "Opponent 4 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(4).Value, Is.EqualTo(pantry), "Opponent 5 hiding in Pantry");
            });
        }
    }
}