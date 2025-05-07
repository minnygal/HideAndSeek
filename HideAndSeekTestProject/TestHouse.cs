using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Newtonsoft.Json.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for:
    /// - property getters and setters
    /// - parameterized constructor
    /// - Serialize method
    /// - default House layout
    /// 
    /// These are integration tests using Location and LocationWithHidingPlace.
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

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_DefaultHouse()
        {
            house = TestHouse_TestData.GetDefaultHouse();
            Assert.That(house.Serialize(), Is.EqualTo(TestHouse_TestData.DefaultHouse_Serialized));
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_CustomHouse_WithLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Initialize variable to expected serialized House text
            string expectedSerializedHouse =
                #region expected serialized House
                "{" +
                    "\"Name\":\"dream house\"" + "," + 
                    "\"HouseFileName\":\"DreamHouse\"" + "," + 
                    "\"PlayerStartingPoint\":\"Kitchen\"" + "," + 
                    "\"LocationsWithoutHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"Name\":\"Kitchen\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"" + "," + 
                                "\"East\":\"Pantry\"" + "," + 
                                "\"West\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"Name\":\"Exercise Room\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}" +
                    "]" + "," + 
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"" + "," + 
                            "\"Name\":\"Bedroom\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Kitchen\"" + "," + 
                                "\"North\":\"Exercise Room\"" + "," + 
                                "\"East\":\"Closet\"" + "," + 
                                "\"West\":\"Bathroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"in a box\"" + "," + 
                            "\"Name\":\"Pantry\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Kitchen\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"under the desk\"" + "," + 
                            "\"Name\":\"Office\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Kitchen\"" + "," + 
                                "\"Northeast\":\"Bathroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"between the coats\"" + "," + 
                            "\"Name\":\"Closet\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Bedroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"in the tub\"" + "," + 
                            "\"Name\":\"Bathroom\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bedroom\"" + "," + 
                                "\"West\":\"Sensory Room\"" + "," + 
                                "\"Southwest\":\"Office\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"under the bean bags\"" + "," + 
                            "\"Name\":\"Sensory Room\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bathroom\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            #endregion

            #region create House
            // Create starting point (Kitchen) and connect to new locations: Bedroom, Pantry, Office
            Location kitchen = new Location("Kitchen");
            LocationWithHidingPlace bedroom = kitchen.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace pantry = kitchen.AddExit(Direction.East, "Pantry", "in a box");
            LocationWithHidingPlace office = kitchen.AddExit(Direction.West, "Office", "under the desk");

            // Connect Bedroom to new locations: Exercise Room, Closet, Bathroom
            Location exerciseRoom = bedroom.AddExit(Direction.North, "Exercise Room");
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.East, "Closet", "between the coats");
            LocationWithHidingPlace bathroom = bedroom.AddExit(Direction.West, "Bathroom", "in the tub");

            // Connect Office to new location: Sensory Room
            LocationWithHidingPlace sensoryRoom = bathroom.AddExit(Direction.West, "Sensory Room", "under the bean bags");

            // Connect Office to Bathroom
            office.AddExit(Direction.Northeast, bathroom);

            // Create enumerable of Location objects
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>() { kitchen, exerciseRoom };

            // Create enumerable of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                bedroom, pantry, office, closet, bathroom, sensoryRoom
            };

            // Create House
            house = new House("dream house", "DreamHouse", kitchen, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
            #endregion

            // ACT
            string serializedHouse = house.Serialize();

            // Assert that serialized House text is as expected
            Assert.That(serializedHouse, Is.EqualTo(expectedSerializedHouse));
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_CustomHouse_WithoutLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Initialize variable to expected serialized House text
            string expectedSerializedHouse =
            #region expected serialized House
                "{" +
                    "\"Name\":\"dream house\"" + "," +
                    "\"HouseFileName\":\"DreamHouse\"" + "," +
                    "\"PlayerStartingPoint\":\"Kitchen\"" + "," +
                    "\"LocationsWithoutHidingPlaces\":[]" + "," +
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"in a cupboard\"" + "," +
                            "\"Name\":\"Kitchen\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"" + "," +
                                "\"East\":\"Pantry\"" + "," +
                                "\"West\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"" + "," +
                            "\"Name\":\"Bedroom\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Kitchen\"" + "," +
                                "\"North\":\"Exercise Room\"" + "," +
                                "\"East\":\"Closet\"" + "," +
                                "\"West\":\"Bathroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"in a box\"" + "," +
                            "\"Name\":\"Pantry\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Kitchen\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the desk\"" + "," +
                            "\"Name\":\"Office\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Kitchen\"" + "," +
                                "\"Northeast\":\"Bathroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"behind the balls\"" + "," +
                            "\"Name\":\"Exercise Room\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"between the coats\"" + "," +
                            "\"Name\":\"Closet\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Bedroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"in the tub\"" + "," +
                            "\"Name\":\"Bathroom\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bedroom\"" + "," +
                                "\"West\":\"Sensory Room\"" + "," +
                                "\"Southwest\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the bean bags\"" + "," +
                            "\"Name\":\"Sensory Room\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bathroom\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            #endregion

            #region create House
            // Create starting point (Kitchen) and connect to new locations: Bedroom, Pantry, Office
            LocationWithHidingPlace kitchen = new LocationWithHidingPlace("Kitchen", "in a cupboard");
            LocationWithHidingPlace bedroom = kitchen.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace pantry = kitchen.AddExit(Direction.East, "Pantry", "in a box");
            LocationWithHidingPlace office = kitchen.AddExit(Direction.West, "Office", "under the desk");

            // Connect Bedroom to new locations: Exercise Room, Closet, Bathroom
            LocationWithHidingPlace exerciseRoom = bedroom.AddExit(Direction.North, "Exercise Room", "behind the balls");
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.East, "Closet", "between the coats");
            LocationWithHidingPlace bathroom = bedroom.AddExit(Direction.West, "Bathroom", "in the tub");

            // Connect Office to new location: Sensory Room
            LocationWithHidingPlace sensoryRoom = bathroom.AddExit(Direction.West, "Sensory Room", "under the bean bags");

            // Connect Office to Bathroom
            office.AddExit(Direction.Northeast, bathroom);

            // Create enumerable of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                kitchen, bedroom, pantry, office, exerciseRoom, closet, bathroom, sensoryRoom
            };

            // Create House
            house = new House("dream house", "DreamHouse", kitchen, new List<Location>(), locationsWithHidingPlaces);
            #endregion

            // ACT
            string serializedHouse = house.Serialize();

            // Assert that serialized House text is as expected
            Assert.That(serializedHouse, Is.EqualTo(expectedSerializedHouse));
        }

        #region Test default House layout
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
            // Set House
            house = TestHouse_TestData.GetDefaultHouse();

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
            house = TestHouse_TestData.GetDefaultHouse();
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
            house = TestHouse_TestData.GetDefaultHouse();
            Assert.That(house.Locations.Where((l) => l.Name == locationName).First(), Is.InstanceOf<LocationWithHidingPlace>());
        }
        #endregion
    }
}