using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for:
    /// - parameterized constructor
    /// - property getters and setters
    /// - LocationsWithoutHidingPlaces and LocationsWithHidingPlaces setter methods
    /// 
    /// These are integration tests using Location and LocationWithHidingPlace
    /// </summary>
    [TestFixture]
    public class TestHouse
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            house = null;
        }

        // Calls properties' setters and getters successfully
        [Test]
        [Category("House Constructor Name HouseFileName StartingPoint PlayerStartingPoint Locations LocationsWithoutHidingPlaces LocationsWithHidingPlaces Success")]
        public void Test_House_Constructor_Parameterized_SetsProperties()
        {
            // Create locations for House
            Location entry = new Location("Entry");
            Location hallway = new Location("Hallway");
            LocationWithHidingPlace bedroom = new LocationWithHidingPlace("Bedroom", "under the bed");
            LocationWithHidingPlace kitchen = new LocationWithHidingPlace("Kitchen", "beside the stove");
            LocationWithHidingPlace office = new LocationWithHidingPlace("Office", "under the desk");

            // Call House constructor and set House
            house = new House("my house", "DefaultHouse", entry,
                new List<Location>() { entry, hallway }, new List<LocationWithHidingPlace>() { bedroom, kitchen, office });
            
            // Assume no exceptions were thrown
            // Assert that properties are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(house.Name, Is.EqualTo("my house"), "name property");
                Assert.That(house.HouseFileName, Is.EqualTo("DefaultHouse"), "House file name property");
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"), "starting point property");
                Assert.That(house.PlayerStartingPoint, Is.EqualTo("Entry"), "player starting point property");
                Assert.That(house.Locations, 
                    Is.EquivalentTo(new List<Location>() { entry, hallway, bedroom, kitchen, office }), "locations property");
                Assert.That(house.LocationsWithoutHidingPlaces, 
                    Is.EquivalentTo(new List<Location>() { entry, hallway }), "locations without hiding places property");
                Assert.That(house.LocationsWithHidingPlaces, 
                    Is.EquivalentTo(new List<LocationWithHidingPlace>() { bedroom, kitchen, office }), "locations with hiding places property");
            });
        }

        [Test]
        [Category("House Constructor Name ArgumentException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForInvalidName()
        {
            LocationWithHidingPlace entryWithHidingPlace = new Mock<LocationWithHidingPlace>().Object;
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with an invalid name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    // Call House constructor and set House
                    house = new House(" ", "DefaultHouse", entryWithHidingPlace, 
                                      Enumerable.Empty<Location>(), new List<LocationWithHidingPlace>() { entryWithHidingPlace });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house name \" \" is invalid (is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("House Constructor HouseFileName ArgumentException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForInvalidHouseFileName()
        {
            LocationWithHidingPlace entryWithHidingPlace = new Mock<LocationWithHidingPlace>().Object;
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with an invalid House file name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    // Call House constructor and set House
                    house = new House("my house", " ", entryWithHidingPlace, 
                                      Enumerable.Empty<Location>(), new List<LocationWithHidingPlace>() { entryWithHidingPlace });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house file name \" \" is invalid " +
                                                               "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("House Constructor PlayerStartingPoint InvalidOperationException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForInvalidStartingPoint_NullValue()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with an invalid starting point name raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    // Call House constructor and set House
                    house = new House("my house", "DefaultHouse", null, Enumerable.Empty<Location>(), 
                                      new List<LocationWithHidingPlace>() { new Mock<LocationWithHidingPlace>().Object });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"starting point location \"\" does not exist in House"));
            });
        }

        [Test]
        [Category("House Constructor PlayerStartingPoint InvalidOperationException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForInvalidStartingPoint_NotInHouse()
        {
            Location notInHouse = new Location("not in house");
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with an invalid starting point (not in house) raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    // Call House constructor and set House
                    house = new House("my house", "DefaultHouse", notInHouse, Enumerable.Empty<Location>(), 
                                      new List<LocationWithHidingPlace>() { new Mock<LocationWithHidingPlace>().Object });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("starting point location \"not in house\" does not exist in House"));
            });
        }

        [Test]
        [Category("House Constructor LocationsWithoutHidingPlaces ArgumentException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForLocationsWithoutHidingPlaces_IncludingLocationWithHidingPlaceObject()
        {
            house = GetBasicHouse(); // Set House
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with list including LocationWithHidingPlace for LocationsWithoutHidingPlaces raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    // Call House constructor and set House
                    house = new House("my house", "DefaultHouse", new Mock<Location>().Object,
                                      new List<Location>() { new LocationWithHidingPlace("Closet", "between the clothes") },
                                      new List<LocationWithHidingPlace>() { new Mock<LocationWithHidingPlace>().Object });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("LocationWithHidingPlace \"Closet\" passed to LocationsWithoutHidingPlaces setter"));
            });
        }

        [Test]
        [Category("House Constructor LocationsWithHidingPlaces ArgumentException Failure")]
        public void Test_House_Constructor_Parameterized_AndCheckErrorMessage_ForEmptyLocationsWithHidingPlaces()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling House constructor with empty locations with hiding places raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    // Call House constructor and set House
                    house = new House("my house", "DefaultHouse", new Mock<Location>().Object, 
                                      Enumerable.Empty<Location>(), Enumerable.Empty<LocationWithHidingPlace>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("locations with hiding places list is empty"));
            });
        }

        [Test]
        [Category("House Name Success")]
        public void Test_House_Set_Name()
        {
            house = GetBasicHouse(); // Set House
            house.Name = "new name"; // Set House name to new value
            Assert.That(house.Name, Is.EqualTo("new name")); // Assert that House name property is set correctly
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("House Name ArgumentException Failure")]
        public void Test_House_Set_Name_AndCheckErrorMessage_ForInvalidName(string name)
        {
            // Set House
            house = GetBasicHouse();

            Assert.Multiple(() =>
            {
                // Assert that setting House name to an invalid name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.Name = name;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house name \"{name}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("House HouseFileName Success")]
        public void Test_House_Set_HouseFileName()
        {
            house = GetBasicHouse(); // Set House
            house.HouseFileName = "NewName"; // Set House file name to new value
            Assert.That(house.HouseFileName, Is.EqualTo("NewName")); // Assert that House file name property is set correctly
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("my file")]
        [TestCase(" myFile")]
        [TestCase("myFile ")]
        [TestCase("\\")]
        [TestCase("\\myFile")]
        [TestCase("myFile\\")]
        [TestCase("my\\File")]
        [TestCase("/")]
        [TestCase("/myFile")]
        [TestCase("myFile/")]
        [TestCase("my/File")]
        [Category("House HouseFileName ArgumentException Failure")]
        public void Test_House_Set_HouseFileName_AndCheckErrorMessage_ForInvalidFileName(string houseFileName)
        {
            // Set House
            house = GetBasicHouse();

            Assert.Multiple(() =>
            {
                // Assert that setting House file name to an invalid file name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.HouseFileName = houseFileName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house file name \"{houseFileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [Test]
        [Category("House PlayerStartingPoint Success")]
        public void Test_House_Set_PlayerStartingPoint()
        {
            house = GetBasicHouse(); // Set House
            house.PlayerStartingPoint = "NewName"; // Set House player starting point to new value
            Assert.That(house.PlayerStartingPoint, Is.EqualTo("NewName")); // Assert that House player starting point property is set correctly
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("House PlayerStartingPoint ArgumentException Failure")]
        public void Test_House_Set_PlayerStartingPoint_AndCheckErrorMessage_ForInvalidLocationName(string locationName)
        {
            // Set House
            house = GetBasicHouse();

            Assert.Multiple(() =>
            {
                // Assert that setting player starting point location name to an invalid location name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.PlayerStartingPoint = locationName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"player starting point location name \"{locationName}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("House StartingPoint Success")]
        public void Test_House_Set_StartingPoint()
        {
            // Create locations for House
            LocationWithHidingPlace bedroom = new LocationWithHidingPlace("Bedroom", "under the bed");
            LocationWithHidingPlace kitchen = new LocationWithHidingPlace("Kitchen", "beside the stove");

            // Set House
            house = new House("my house", "DefaultHouse", bedroom,
                              Enumerable.Empty<Location>(), new List<LocationWithHidingPlace>() { bedroom, kitchen });

            // Set House starting point to new value
            house.StartingPoint = kitchen;
            Assert.That(house.StartingPoint, Is.SameAs(kitchen)); // Assert that House starting point property is set correctly
        }

        [Test]
        [Category("House StartingPoint InvalidOperationException Failure")]
        public void Test_House_Set_StartingPoint_AndCheckErrorMessage_ForLocationNotExistingInHouse()
        {
            // Set House
            house = GetBasicHouse();

            Assert.Multiple(() =>
            {
                // Assert that setting starting point location to a Location not in the House raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.StartingPoint = new Location("not in house");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("starting point location \"not in house\" does not exist in House"));
            });  
        }

        [Test]
        [Category("House LocationsWithoutHidingPlaces Success")]
        public void Test_House_Set_LocationsWithoutHidingPlaces_ToListWithLocation()
        {
            house = GetBasicHouse(); // Set House
            Location location = new Mock<Location>().Object; // Create location
            house.LocationsWithoutHidingPlaces = new List<Location>() { location }; // Set House locations without hiding places to new value
            Assert.That(house.LocationsWithoutHidingPlaces, Is.EquivalentTo(new List<Location>() { location })); // Assert that House locations without hiding places property is set correctly
        }

        [Test]
        [Category("House LocationsWithoutHidingPlaces Success")]
        public void Test_House_Set_LocationsWithoutHidingPlaces_ToEmptyList()
        {
            house = GetBasicHouse(); // Set House
            house.LocationsWithoutHidingPlaces = Enumerable.Empty<Location>(); // Set House locations without hiding places to new value
            Assert.That(house.LocationsWithoutHidingPlaces, Is.Empty); // Assert that House locations without hiding places property is set correctly
        }

        [Test]
        [Category("House LocationsWithoutHidingPlaces ArgumentException Failure")]
        public void Test_House_Set_LocationsWithoutHidingPlaces_AndCheckErrorMessage_ForLocationWithHidingPlaceObject()
        {
            house = GetBasicHouse(); // Set House
            Assert.Multiple(() =>
            {
                // Assert that setting LocationsWithoutHidingPlaces property to list including LocationWithHidingPlace raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.LocationsWithoutHidingPlaces = new List<Location>() { new LocationWithHidingPlace("Closet", "between the clothes") };
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("LocationWithHidingPlace \"Closet\" passed to LocationsWithoutHidingPlaces setter"));
            });
        }

        [Test]
        [Category("House SetLocationsWithoutHidingPlaces Success")]
        public void Test_House_SetLocationsWithoutHidingPlacesMethod_ToListWithLocation()
        {
            // Set House with StartingPoint in LocationsWithHidingPlaces list
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("my house", "TestHouse", entryLocationWithHidingPlace, Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Set House locations without hiding places to new value
            Location location = new Mock<Location>().Object; // Create location
            house.SetLocationsWithoutHidingPlaces(new List<Location>() { location }); // Set House locations without hiding places to new value

            // Assert that House locations without hiding places property is set correctly// Assert that House locations without hiding places property is set correctly
            Assert.That(house.LocationsWithoutHidingPlaces, Is.EquivalentTo(new List<Location>() { location }));
        }

        [Test]
        [Category("House SetLocationsWithoutHidingPlaces Success")]
        public void Test_House_SetLocationsWithoutHidingPlacesMethod_ToEmptyList()
        {
            // Set House with StartingPoint in LocationsWithHidingPlaces list
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("my house", "TestHouse", entryLocationWithHidingPlace, Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            // Set House locations without hiding places to new value
            house.SetLocationsWithoutHidingPlaces(Enumerable.Empty<Location>());

            // Assert that House locations without hiding places property is set correctly
            Assert.That(house.LocationsWithoutHidingPlaces, Is.Empty);
        }

        [Test]
        [Category("House SetLocationsWithoutHidingPlaces ArgumentException Failure")]
        public void Test_House_SetLocationsWithoutHidingPlacesMethod_AndCheckErrorMessage_ForLocationWithHidingPlaceObject()
        {
            // Set House with StartingPoint in LocationsWithHidingPlaces list
            LocationWithHidingPlace entryLocationWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("my house", "TestHouse", entryLocationWithHidingPlace, Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryLocationWithHidingPlace });

            Assert.Multiple(() =>
            {
                // Assert that calling method with list including LocationWithHidingPlace raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.SetLocationsWithoutHidingPlaces(new List<Location>() { new LocationWithHidingPlace("Closet", "between the clothes") });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("LocationWithHidingPlace \"Closet\" passed to LocationsWithoutHidingPlaces setter"));
            });
        }

        [Test]
        [Category("House SetLocationsWithoutHidingPlaces InvalidOperationException Failure")]
        public void Test_House_SetLocationsWithoutHidingPlacesMethod_AndCheckErrorMessage_ForStartingPointWouldNotBeInHouse()
        {
            // Set House with StartingPoint in LocationsWithoutHidingPlaces list
            Location entry = new Location("Entry");
            house = new House("my house", "TestHouse", entry, new List<Location>() { entry }, 
                              new List<LocationWithHidingPlace>() { new LocationWithHidingPlace("Kitchen", "beside the stove") });

            Assert.Multiple(() =>
            {
                // Assert that calling method with list not including StartingPoint raises exception
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.SetLocationsWithoutHidingPlaces(new List<Location>() { new Location("Foyer") });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("StartingPoint is not in LocationsWithHidingPlaces or the locations passed in"));
            });
        }

        [Test]
        [Category("House LocationsWithHidingPlaces Success")]
        public void Test_House_Set_LocationsWithHidingPlaces()
        {
            house = GetBasicHouse(); // Set House
            LocationWithHidingPlace locationWithHidingPlace = new Mock<LocationWithHidingPlace>().Object; // Create location with hiding place
            house.LocationsWithHidingPlaces = new List<LocationWithHidingPlace>() { locationWithHidingPlace }; // Set House locations with hiding places to new value
            Assert.That(house.LocationsWithHidingPlaces, Is.EquivalentTo(new List<LocationWithHidingPlace>() { locationWithHidingPlace })); // Assert that House locations with hiding places property is set correctly
        }

        [Test]
        [Category("House LocationsWithHidingPlaces ArgumentException Failure")]
        public void Test_House_Set_LocationsWithHidingPlaces_AndCheckErrorMessage_ForEmptyEnumerable()
        {
            // Set House
            house = GetBasicHouse();

            Assert.Multiple(() =>
            {
                // Assert that setting locations with hiding places property to an empty list raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.LocationsWithHidingPlaces = new List<LocationWithHidingPlace>();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("locations with hiding places list is empty"));
            });
        }

        [Test]
        [Category("House SetLocationsWithHidingPlaces Success")]
        public void Test_House_SetLocationsWithHidingPlacesMethod()
        {
            // Set House with StartingPoint in LocationsWithoutHidingPlaces list
            Location entryLocation = new Location("Entry");
            house = new House("my house", "TestHouse", entryLocation, new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { new LocationWithHidingPlace("Closet", "between the coats") });

            // Set House locations with hiding places to new value
            LocationWithHidingPlace locationWithHidingPlace = new Mock<LocationWithHidingPlace>().Object; // Create location with hiding place
            house.SetLocationsWithHidingPlaces(new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Set House locations with hiding places to new value

            // Assert that House locations with hiding places property is set correctly
            Assert.That(house.LocationsWithHidingPlaces, Is.EquivalentTo(new List<LocationWithHidingPlace>() { locationWithHidingPlace }));
        }

        [Test]
        [Category("House SetLocationsWithHidingPlaces ArgumentException Failure")]
        public void Test_House_SetLocationsWithHidingPlacesMethod_AndCheckErrorMessage_ForEmptyList()
        {
            // Set House with StartingPoint in LocationsWithoutHidingPlaces list
            Location entryLocation = new Location("Entry");
            house = new House("my house", "TestHouse", entryLocation, new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { new LocationWithHidingPlace("Closet", "between the coats") });


            Assert.Multiple(() =>
            {
                // Assert that calling method with empty list raises exception
                Exception exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.SetLocationsWithHidingPlaces(Enumerable.Empty<LocationWithHidingPlace>());
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("locations with hiding places list is empty"));
            });
        }

        [Test]
        [Category("House SetLocationsWithHidingPlaces InvalidOperationException Failure")]
        public void Test_House_SetLocationsWithHidingPlacesMethod_AndCheckErrorMessage_ForStartingPointWouldNotBeInHouse()
        {
            // Set House with StartingPoint in LocationsWithHidingPlaces list
            LocationWithHidingPlace entryWithHidingPlace = new LocationWithHidingPlace("Entry", "behind the coat rack");
            house = new House("my house", "TestHouse", entryWithHidingPlace, Enumerable.Empty<Location>(),
                              new List<LocationWithHidingPlace>() { entryWithHidingPlace });

            Assert.Multiple(() =>
            {
                // Assert that calling method with list not including StartingPoint raises exception
                Exception exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.SetLocationsWithHidingPlaces(new List<LocationWithHidingPlace>() { new LocationWithHidingPlace("Foyer", "behind the chair") });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("StartingPoint is not in LocationsWithoutHidingPlaces or the locations passed in"));
            });
        }

        /// <summary>
        /// Get basic House (used to set House in failing setter tests)
        /// </summary>
        private static House GetBasicHouse()
        {
            // Create locations for House
            Location entry = new Location("Entry");
            Location hallway = new Location("Hallway");
            LocationWithHidingPlace bedroom = new LocationWithHidingPlace("Bedroom", "under the bed");
            LocationWithHidingPlace kitchen = new LocationWithHidingPlace("Kitchen", "beside the stove");
            LocationWithHidingPlace office = new LocationWithHidingPlace("Office", "under the desk");

            // Call House constructor and return House object
            return new House("my house", "DefaultHouse", entry,
                new List<Location>() { entry, hallway }, new List<LocationWithHidingPlace>() { bedroom, kitchen, office });
        }
    }
}