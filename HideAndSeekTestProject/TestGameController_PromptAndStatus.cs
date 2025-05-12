using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for Prompt and Status properties
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    public class TestGameController_PromptAndStatus
    {
        GameController gameController;
        House house;
        readonly Opponent[] mockedOpponentArray = new Opponent[] { new Mock<Opponent>().Object };

        [SetUp]
        public void SetUp()
        {
            gameController = null;
            house = null;
        }

        [Test]
        [Category("GameController Prompt Location")]
        public void Test_GameController_Prompt_InLocationWithoutHidingPlace()
        {
            // Create GameController with new House object
            Location entryLocation = new Location("Entry"); // Create entry without hiding place
            LocationWithHidingPlace locationWithHidingPlace = entryLocation.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Set GameController

            // Assert that prompt is as expected
            Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "));
        }

        [Test]
        [Category("GameController Prompt LocationWithHidingPlace")]
        public void Test_GameController_Prompt_InLocationWithHidingPlace()
        {
            // Create GameController with new House object
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry with hiding place
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Set GameController

            // Assert that prompt is as expected
            Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go (or type 'check'): "));
        }

        [Test]
        [Category("GameController Status Location AddExit House")]
        public void Test_GameController_Status_InLocation_WithSingleExit()
        {
            // Set up GameController
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = entryLocation.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Create GameController with House

            // Assert that Status is as expected
            Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exit:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    "You have not found any opponents"));
        }

        [Test]
        [Category("GameController Status Location AddExit House")]
        public void Test_GameController_Status_InLocationWithHidingPlace_WithSingleExit()
        {
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            Location location = entryLocationWithHidingPlace.AddExit(Direction.West, "Hallway"); // Add new connecting location
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              new List<Location>() { location },
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Create GameController with House

            // Assert that Status is as expected
            Assert.That(gameController.Status, Is.EqualTo(
                "You are in the Entry. You see the following exit:" + Environment.NewLine +
                " - the Hallway is to the West" + Environment.NewLine +
                "Someone could hide behind the coat rack" + Environment.NewLine +
                "You have not found any opponents"));
        }

        [Test]
        [Category("GameController Status Location AddExit House")]
        public void Test_GameController_Status_InLocation_WithMultipleExits()
        {
            // Set up GameController
            Location entryLocation = new Location("Entry"); // Create entry
            Location otherLocation = entryLocation.AddExit(Direction.Out, "Yard"); // Add new connecting location
            LocationWithHidingPlace locationWithHidingPlace = entryLocation.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation, otherLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Create GameController with House

            // Assert that Status is as expected
            Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "You have not found any opponents"));
        }

        [Test]
        [Category("GameController Status Location AddExit House")]
        public void Test_GameController_Status_InLocationWithHidingPlace_WithMultipleExits()
        {
            // Set up GameController
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry
            Location location = entryLocationWithHidingPlace.AddExit(Direction.Out, "Yard"); // Add new connecting location
            LocationWithHidingPlace otherLocationWithHidingPlace = entryLocationWithHidingPlace.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              new List<Location>() { location },
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace, otherLocationWithHidingPlace }); // Create House
            gameController = new GameController(mockedOpponentArray, house); // Create GameController with House

            // Assert that Status is as expected
            Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Office is to the East" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "Someone could hide behind the coat rack" + Environment.NewLine +
                    "You have not found any opponents"));
        }
    }
}