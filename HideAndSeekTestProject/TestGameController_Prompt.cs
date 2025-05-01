using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for Prompt property
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    public class TestGameController_Prompt
    {
        GameController gameController;
        House house;

        [SetUp]
        public void SetUp()
        {
            gameController = null;
        }

        [Test]
        [Category("GameController Prompt Location")]
        public void Test_GameController_Prompt_InLocationWithoutHidingPlace()
        {
            // Create GameController with new House object
            Location entryLocation = new Location("Entry"); // Create entry without hiding place
            LocationWithHidingPlace locationWithHidingPlace = entryLocation.AddExit(Direction.East, "Office", "under the table"); // Add new connecting location with hiding place
            house = new House("test house", "TestHouse", "Entry",
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, house); // Set GameController

            // Assert that prompt is as expected
            Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "));
        }

        [Test]
        [Category("GameController Prompt LocationWithHidingPlace")]
        public void Test_GameController_Prompt_InLocationWithHidingPlace()
        {
            // Create GameController with new House object
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack"); // Create entry with hiding place
            house = new House("test house", "TestHouse", "Entry",
                              Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace }); // Create House
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, house); // Set GameController

            // Assert that prompt is as expected
            Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go (or type 'check'): "));
        }
    }
}