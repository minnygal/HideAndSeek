namespace HideAndSeek
{
    /// <summary>
    /// House tests for layout of default House
    /// 
    /// These are integration tests using Location and LocationWithHidingPlace
    /// </summary>
    [TestFixture]
    public class TestHouse_Layout
    {
        private House house;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            house = GetDefaultHouse(); // Set House to default House (House not changed in tests)
        }

        /// <summary>
        /// Assert that layout of House is as expected using House's GetExit method and Location's Name property
        /// 
        /// CREDIT: adapted from HideAndSeek project's TestHouse class's TestLayout() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/TestHouse.cs
        ///         Link valid as of 02-26-2025
        ///         
        /// CHANGES:
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I added some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [Test]
        [Category("House Layout GetExit Name")]
        public void Test_House_Layout()
        {
            Assert.Multiple(() =>
            {
                // Assert that name of StartingPoint is correct
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"), "name of starting point");

                // Assert that Garage is located "Out" from StartingPoint
                var garage = house.StartingPoint.GetExit(Direction.Out);
                Assert.That(garage.Name, Is.EqualTo("Garage"), "Garage is Out from Entry");

                // Assert that Hallway is located "East" of StartingPoint
                var hallway = house.StartingPoint.GetExit(Direction.East);
                Assert.That(hallway.Name, Is.EqualTo("Hallway"), "Hallway is East of Entry");

                // Assert that Kitchen is located "Northwest" of Hallway
                var kitchen = hallway.GetExit(Direction.Northwest);
                Assert.That(kitchen.Name, Is.EqualTo("Kitchen"), "Kitchen is Northwest of Hallway");

                // Assert that Bathroom is located "North" of Hallway
                var bathroom = hallway.GetExit(Direction.North);
                Assert.That(bathroom.Name, Is.EqualTo("Bathroom"), "Bathroom is North of Hallway");

                // Assert that Living Room is located "South" of Hallway
                var livingRoom = hallway.GetExit(Direction.South);
                Assert.That(livingRoom.Name, Is.EqualTo("Living Room"), "Living Room is South of Hallway");

                // Assert that Landing is located "Up" from Hallway
                var landing = hallway.GetExit(Direction.Up);
                Assert.That(landing.Name, Is.EqualTo("Landing"), "Landing is Up from Hallway");

                // Assert that Master Bedroom is located "Northwest" of Landing
                var masterBedroom = landing.GetExit(Direction.Northwest);
                Assert.That(masterBedroom.Name, Is.EqualTo("Master Bedroom"), "Master Bedroom is Northwest of Landing");

                // Assert that Master Bath is located "East" of Master Bedroom
                var masterBath = masterBedroom.GetExit(Direction.East);
                Assert.That(masterBath.Name, Is.EqualTo("Master Bath"), "Master Bath is East of Master Bedroom");

                // Assert that Second Bathroom is located "West" of Landing
                var secondBathroom = landing.GetExit(Direction.West);
                Assert.That(secondBathroom.Name, Is.EqualTo("Second Bathroom"), "Second Bathroom is West of Landing");

                // Assert that Nursery is located Southwest of Landing
                var nursery = landing.GetExit(Direction.Southwest);
                Assert.That(nursery.Name, Is.EqualTo("Nursery"), "Nursery is Southwest of Landing");

                // Assert that Pantry is located South of Landing
                var pantry = landing.GetExit(Direction.South);
                Assert.That(pantry.Name, Is.EqualTo("Pantry"), "Pantry is South of Landing");

                // Assert that Kids Room is located "Southeast" of Landing
                var kidsRoom = landing.GetExit(Direction.Southeast);
                Assert.That(kidsRoom.Name, Is.EqualTo("Kids Room"), "Kids Room is Southeast of Landing");

                // Assert that Attic is located "Up" from Landing
                var attic = landing.GetExit(Direction.Up);
                Assert.That(attic.Name, Is.EqualTo("Attic"), "Attic is Up from Landing");
            });
        }

        [TestCase("Entry")]
        [TestCase("Hallway")]
        [TestCase("Landing")]
        [Category("House Locations LocationType")]
        public void Test_House_Locations_AreNotOfType_LocationWithHidingPlace(string locationName)
        {
            Assert.That(house.Locations.Where((l) => l.Name == locationName).First(), Is.Not.InstanceOf<LocationWithHidingPlace>());
        }

        [TestCase("Garage")]
        [TestCase("Kitchen")]
        [TestCase("Living Room")]
        [TestCase("Bathroom")]
        [TestCase("Master Bedroom")]
        [TestCase("Master Bath")]
        [TestCase("Second Bathroom")]
        [TestCase("Kids Room")]
        [TestCase("Nursery")]
        [TestCase("Pantry")]
        [TestCase("Attic")]
        [Category("House Locations LocationWithHidingPlaceType")]
        public void Test_House_Locations_AreOfType_LocationWithHidingPlace(string locationName)
        {
            Assert.That(house.Locations.Where((l) => l.Name == locationName).First(), Is.InstanceOf<LocationWithHidingPlace>());
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
            return new House("my house", "DefaultHouse", entry, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }
    }
}