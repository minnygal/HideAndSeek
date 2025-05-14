using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for location-related methods GetRandomExit, GetRandomLocationWithHidingPlace, ClearHidingPlaces
    /// GetLocationByName, GetLocationWithHidingPlaceByName, DoesLocationExist, and DoesLocationWithHidingPlaceExist
    /// 
    /// The tests are integration tests using Location and LocationWithHidingPlace
    /// </summary>
    [TestFixture]
    public class TestHouse_LocationMethods
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
            house = null;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        /// <summary>
        /// Get new House object for testing purposes
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        public static House GetDefaultHouse()
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
            return new House("my house", "DefaultHouse", entry, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        [Test]
        [Category("House GetRandomLocationWithHidingPlace Success")]
        public void Test_House_GetRandomLocationWithHidingPlace()
        {
            // Set House
            house = GetDefaultHouse();

            // Set House's Random number generator to mock
            House.Random = new MockRandomWithValueList(new List<int>() { 5, 1, 0, 9, 10 });

            // Assert that method returns locations with expected names
            Assert.Multiple(() =>
            {
                Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo("Pantry"), "1st random LocationWithHidingPlace");
                Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo("Bathroom"), "2nd random LocationWithHidingPlace");
                Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo("Attic"), "3rd random LocationWithHidingPlace");
                Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo("Garage"), "4th random LocationWithHidingPlace");
                Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo("Living Room"), "10th random LocationWithHidingPlace");
            });
        }

        [Test]
        [Category("House ClearHidingPlaces CheckHidingPlace HideOpponent")]
        public void Test_House_ClearHidingPlaces()
        {
            // ARRANGE
            // Set House
            LocationWithHidingPlace garage = new LocationWithHidingPlace("Garage", "behind the car");
            LocationWithHidingPlace attic = new LocationWithHidingPlace("Attic", "behind the trunks");
            house = new House("test house", "TestHouse", garage,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { garage, attic });

            // Hide opponent in Garage
            garage.HideOpponent(new Mock<Opponent>().Object);

            // Hide 3 more Opponents in Attic
            attic.HideOpponent(new Mock<Opponent>().Object);
            attic.HideOpponent(new Mock<Opponent>().Object);
            attic.HideOpponent(new Mock<Opponent>().Object);

            // ACT
            // Clear hiding places in House
            house.ClearHidingPlaces();

            // ASSERT
            // Assert that no Opponents are in cleared hiding places
            Assert.Multiple(() =>
            {
                Assert.That(garage.CheckHidingPlace(), Is.Empty, "no Opponents in Garage");
                Assert.That(attic.CheckHidingPlace(), Is.Empty, "no Opponents in Attic");
            });
        }

        [Test]
        [Category("House GetLocationByName Success")]
        public void Test_House_GetLocationByName_OfLocation_ReturnsLocation()
        {
            // Set House
            Location entryLocation = new Location("Entry");
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Pantry", "behind the shelves");
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation }, new List<LocationWithHidingPlace>() { locationWithHidingPlace });

            // Assert that GetLocationByName method returns the correct Location
            Assert.That(house.GetLocationByName("Entry"), Is.SameAs(entryLocation), "same object");
        }

        [Test]
        [Category("House GetLocationByName Success")]
        public void Test_House_GetLocationByName_OfLocationWithHidingPlace_ReturnsLocation()
        {
            // Set House
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Assert that GetLocationByName method returns the correct Location
            Assert.That(house.GetLocationByName("Entry"), Is.SameAs(entryLocationWithHidingPlace));
        }

        [TestCase("Secret Library")]
        [TestCase("master bedroom")]
        [TestCase("MasterBedroom")]
        [TestCase(" ")]
        [TestCase("")]
        [Category("House GetLocationByName InvalidOperationException Failure")]
        public void Test_House_GetLocationByName_AndCheckErrorMessage_WhenLocationWithName_DoesNotExist(string locationName)
        {
            // Set House
            LocationWithHidingPlace startingLocationWithHidingPlace = new LocationWithHidingPlace("Master Bedroom", "under the bed");
            house = new House("test house", "TestHouse", startingLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { startingLocationWithHidingPlace });

            Assert.Multiple(() =>
            {
                // Assert that exception is thrown when attempt to get location by name for nonexistent location
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.GetLocationByName(locationName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("location \"" + locationName + "\" does not exist in House"));
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Success")]
        public void Test_House_GetLocationWithHidingPlaceByName_ReturnsLocation()
        {
            // Set House
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Assert that GetLocationByName method returns the correct Location
            Assert.That(house.GetLocationWithHidingPlaceByName("Entry"), Is.SameAs(entryLocationWithHidingPlace));
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName InvalidOperationException Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_AndCheckErrorMessage_WhenLocationDoesNotHaveHidingPlace()
        {
            // Set House
            Location entryLocation = new Location("Entry");
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Pantry", "behind the shelves");
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation }, new List<LocationWithHidingPlace>() { locationWithHidingPlace });

            Assert.Multiple(() =>
            {
                // Assert exception thrown when get location with hiding place that does not exist
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.GetLocationWithHidingPlaceByName("Entry");
                });

                // Assert exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"location with hiding place \"Entry\" does not exist in House"));
            });
        }

        [TestCase("Secret Library")]
        [TestCase("master bedroom")]
        [TestCase("MasterBedroom")]
        [TestCase(" ")]
        [TestCase("")]
        [Category("House GetLocationWithHidingPlaceByName InvalidOperationException Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_AndCheckErrorMessage_WhenNoLocationWithNameExistsInHouse(string locationName)
        {
            // Set House
            LocationWithHidingPlace startingLocationWithHidingPlace = new LocationWithHidingPlace("Master Bedroom", "under the bed");
            house = new House("test house", "TestHouse", startingLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { startingLocationWithHidingPlace });

            Assert.Multiple(() =>
            {
                // Assert that exception is thrown when attempt to get location with hiding place by name for nonexistent location
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.GetLocationWithHidingPlaceByName(locationName);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"location with hiding place \"{locationName}\" does not exist in House"));
            });
        }

        [Test]
        [Category("House DoesLocationExist True")]
        public void Test_House_DoesLocationExist_ReturnsTrue()
        {
            // Set House
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Assert that method returns true
            Assert.That(house.DoesLocationExist("Entry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationExist False")]
        public void Test_House_DoesLocationExist_ReturnsFalse()
        {
            // Set House
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Assert that method returns false
            Assert.That(house.DoesLocationExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist True")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsTrue()
        {
            // Set House
            LocationWithHidingPlace startingLocationWithHidingPlace = new LocationWithHidingPlace("Pantry", "behind the food");
            house = new House("test house", "TestHouse", startingLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { startingLocationWithHidingPlace });

            Assert.That(house.DoesLocationWithHidingPlaceExist("Pantry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist False")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenLocationDoesNotExist()
        {
            // Set House
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("test house", "TestHouse", entryLocationWithHidingPlace,
                              Enumerable.Empty<LocationWithHidingPlace>(), new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Assert that method retruns false
            Assert.That(house.DoesLocationWithHidingPlaceExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist False")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenLocationIsNotLocationWithHidingPlace()
        {
            // Set House
            Location entryLocation = new Location("Entry");
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Pantry", "behind the shelves");
            house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation }, new List<LocationWithHidingPlace>() { locationWithHidingPlace });

            // Assert that method returns false
            Assert.That(house.DoesLocationWithHidingPlaceExist("Entry"), Is.False);
        }
    }
}
