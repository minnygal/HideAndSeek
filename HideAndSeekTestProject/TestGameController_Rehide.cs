using Moq;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for RehideAllOpponents method
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    [TestFixture]
    public class TestGameController_Rehide
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            gameController = new GameController(MockedOpponents, GetDefaultHouse());
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

        /// <summary>
        /// Get new House object for testing purposes
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        private static House GetDefaultHouse()
        {
            // Create Entry and connect to new locations: Garage, Hallway
            Location entry = new Location("Entry");
            LocationWithHidingPlace garage = entry.AddExit(Direction.Out, "Garage", "behind the car");
            Location hallway = entry.AddExit(Direction.East, "Hallway");

            // Connect Hallway to new locations: Kitchen, Bathroom, Living Room, Landing
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.Northwest, "Kitchen", "next to the stove");
            LocationWithHidingPlace bathroom = hallway.AddExit(Direction.North, "Bathroom", "behind the door");
            LocationWithHidingPlace livingRoom = hallway.AddExit(Direction.South, "Living Room", "behind the sofa");
            Location landing = hallway.AddExit(Direction.Up, "Landing");

            // Connect Landing to new locations: Attic, Kids Room, Master Bedroom, Nursery, Pantry, Second Bathroom
            LocationWithHidingPlace attic = landing.AddExit(Direction.Up, "Attic", "in a trunk");
            LocationWithHidingPlace kidsRoom = landing.AddExit(Direction.Southeast, "Kids Room", "in the bunk beds");
            LocationWithHidingPlace masterBedroom = landing.AddExit(Direction.Northwest, "Master Bedroom", "under the bed");
            LocationWithHidingPlace nursery = landing.AddExit(Direction.Southwest, "Nursery", "behind the changing table");
            LocationWithHidingPlace pantry = landing.AddExit(Direction.South, "Pantry", "inside a cabinet");
            LocationWithHidingPlace secondBathroom = landing.AddExit(Direction.West, "Second Bathroom", "in the shower");

            // Connect Master Bedroom to new location: Master Bath
            LocationWithHidingPlace masterBath = masterBedroom.AddExit(Direction.East, "Master Bath", "in the tub");

            // Create list of Location objects (no hiding places)
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>()
            {
                hallway, landing, entry
            };

            // Create list of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                attic,
                bathroom,
                kidsRoom,
                masterBedroom,
                nursery,
                pantry,
                secondBathroom,
                kitchen,
                masterBath,
                garage,
                livingRoom
            };

            // Create and return new House
            return new House("my house", "DefaultHouse", "Entry", locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        [Test]
        [Category("GameController RehideAllOpponents HidingLocations Success")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces()
        {
            // Create enumerable of hiding locations for opponents to hide
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
                    gameController.RehideAllOpponents(new List<string>() { "Kitchen", "Pantry", "Dungeon", "Worm Hole", "Bathroom" });
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