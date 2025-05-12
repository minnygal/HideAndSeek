using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for CheckCurrentLocation method
    ///  (checking FoundOpponents, Status, CurrentLocation, MoveNumber, and GameOver properties along the way)
    /// 
    /// These are integration tests using House, Location, and LocationWithHidingPlace
    /// </summary>
    public class TestGameController_CheckCurrentLocation
    {
        GameController gameController;
        House house;

        [SetUp]
        public void SetUp()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
            gameController = null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        private Opponent[] MockedOpponents
        {
            get
            {
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

                return new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object };
            }
        }

        [Test]
        [Category("GameController CheckCurrentLocation FoundOpponents Status MoveNumber GameOver CurrentLocation InvalidOperationException Failure")]
        public void Test_GameController_CheckCurrentlocation_AndCheckErrorMessageAndProperties_InLocationWithoutHidingPlace()
        {
            // ARRANGE
            // Set up GameController
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = entryLocation.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            gameController = new GameController(MockedOpponents, house); // Create GameController with House

            // Get initial status before attempt to check
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Assert that checking current location raises exception
                Exception exception = Assert.Throws<InvalidOperationException>(() => {
                    gameController.CheckCurrentLocation();
                });

                // Assert that properties are as expected
                Assert.That(exception.Message, Is.EqualTo("There is no hiding place in the Entry"), "exception message");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no found opponents after trying to check");
                Assert.That(gameController.Status, Is.EqualTo(initialStatus), "status does not change");
                Assert.That(gameController.CurrentLocation, Is.SameAs(entryLocation), "current location does not change");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number does not increment");
                Assert.That(gameController.GameOver, Is.False, "game not over after trying to check");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation FoundOpponents Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_WhenNoOpponentHiding()
        {
            // ARRANGE
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            LocationWithHidingPlace otherLocationWithHidingPlace = entryLocationWithHidingPlace.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace, otherLocationWithHidingPlace }); // Create House
            House.Random = new MockRandomWithValueList(new List<int>() { 1 }); // Set House random number generator so Opponents will be hidden in second location with hiding place
            gameController = new GameController(MockedOpponents, house); // Create GameController with House

            // Get initial status before attempt to check
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Check current location and assert that message is as expected
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the coat rack"), "message when check");

                // Assert that properties are as expected
                Assert.That(gameController.FoundOpponents, Is.Empty, "no found opponents after check");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exit:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    "Someone could hide behind the coat rack" + Environment.NewLine +
                    "You have not found any opponents"), "status after check");
                Assert.That(gameController.CurrentLocation, Is.SameAs(entryLocationWithHidingPlace), "current location after check");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after check");
                Assert.That(gameController.GameOver, Is.False, "game not over after check");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation FoundOpponents Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_When1OpponentHiding()
        {
            // ARRANGE
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            LocationWithHidingPlace otherLocationWithHidingPlace = entryLocationWithHidingPlace.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace, otherLocationWithHidingPlace }); // Create House
            House.Random = new MockRandomWithValueList(new List<int>() { 0, 1, 1, 1, 1 }); // Set House random number generator so only first Opponent will be hidden in entry location with hiding place
            gameController = new GameController(MockedOpponents, house); // Create GameController with House

            // Get initial status before attempt to check
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Check current location and assert that message is as expected
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind the coat rack"), "message when check");

                // Assert that properties are as expected
                Assert.That(gameController.FoundOpponents.Select((o) => o.Name), Is.EquivalentTo(new List<string>() { "Joe" }), "found opponents after check");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exit:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    "Someone could hide behind the coat rack" + Environment.NewLine +
                    "You have found 1 of 5 opponents: Joe"), "status after check");
                Assert.That(gameController.CurrentLocation, Is.SameAs(entryLocationWithHidingPlace), "current location after check");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after check");
                Assert.That(gameController.GameOver, Is.False, "game not over after check");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation FoundOpponents Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_WhenMultipleOpponentsHiding()
        {
            // ARRANGE
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            LocationWithHidingPlace otherLocationWithHidingPlace = entryLocationWithHidingPlace.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace, otherLocationWithHidingPlace }); // Create House
            House.Random = new MockRandomWithValueList(new List<int>() { 0, 0, 1, 1, 1 }); // Set House random number generator so only first two Opponents will be hidden in entry location with hiding place
            gameController = new GameController(MockedOpponents, house); // Create GameController with House

            // Get initial status before attempt to check
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Check current location and assert that message is as expected
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding behind the coat rack"), "message when check");

                // Assert that properties are as expected
                Assert.That(gameController.FoundOpponents.Select((o) => o.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob" }), "found opponents after check");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exit:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    "Someone could hide behind the coat rack" + Environment.NewLine +
                    "You have found 2 of 5 opponents: Joe, Bob"), "status after check");
                Assert.That(gameController.CurrentLocation, Is.SameAs(entryLocationWithHidingPlace), "current location after check");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after check");
                Assert.That(gameController.GameOver, Is.False, "game not over after check");
            });
        }

        [Test]
        [Category("GameController CheckCurrentLocation FoundOpponents Status MoveNumber GameOver CurrentLocation Success")]
        public void Test_GameController_CheckCurrentLocation_FindAllOpponents()
        {
            // ARRANGE
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            Location location = entryLocationWithHidingPlace.AddExit(Direction.West, "Hallway"); // Add new connecting location
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              new List<Location>() { location  },
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace }); // Create House
            gameController = new GameController(MockedOpponents, house); // Create GameController with House

            // Get initial status before attempt to check
            string initialStatus = gameController.Status;

            Assert.Multiple(() =>
            {
                // ACT/ASSERT
                // Check current location and assert that message is as expected
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 5 opponents hiding behind the coat rack"), "message when check");

                // Assert that properties are as expected
                Assert.That(gameController.FoundOpponents.Select((o) => o.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "found opponents after check");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the West" + Environment.NewLine +
                    "Someone could hide behind the coat rack" + Environment.NewLine +
                    "You have found 5 of 5 opponents: Joe, Bob, Ana, Owen, Jimmy"), "status after check");
                Assert.That(gameController.CurrentLocation, Is.SameAs(entryLocationWithHidingPlace), "current location after check");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "move number after check");
                Assert.That(gameController.GameOver, Is.True, "game over after check");
            });
        }
    }
}