using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// Location unit tests for properties and methods GetExit, ExitList, AddExit, and SetExitsDictionary
    /// </summary>
    [TestFixture]
    public class TestLocation
    {
        // Declare Location variables
        private Location center; // Center room (called living room)
        private Location in_closet;
        private Location up_attic;
        private Location southeast_study;
        private Location northeast_pantry;
        private Location east_gameRoom;
        private Location north_kitchen;
        private Location south_office;
        private Location west_bedroom;
        private Location southwest_sensoryRoom;
        private Location northwest_storageRoom;
        private Location down_basement;
        private Location out_yard;

        /// <summary>
        /// Create a center Location and add a room in each direction before each test
        /// 
        /// CREDIT: adapted from HideAndSeek project's TestLocation class's Initialize() method
        ///         � 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/TestLocation.cs
        ///         Link valid as of 02-26-2025
        /// 
        /// CHANGES:
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I added some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Set Location Random number generator to fresh generator
            Location.Random = new Random();

            // Initialize class variables to new Locations
            center = new Location("living room");
            in_closet = new Location("closet");
            up_attic = new Location("attic");
            southeast_study = new Location("study");
            northeast_pantry = new Location("pantry");
            east_gameRoom = new Location("game room");
            north_kitchen = new Location("kitchen");
            south_office = new Location("office");
            west_bedroom = new Location("bedroom");
            southwest_sensoryRoom = new Location("sensory room");
            northwest_storageRoom = new Location("storage room");
            down_basement = new Location("basement");
            out_yard = new Location("yard");

            // Assert that basic properties are as expected and add exits to Center Location
            Assert.Multiple(() =>
            {
                // Assert that center room (living room) is represented as string correctly and has an empty exit list
                Assert.That(center.ToString, Is.EqualTo("living room"), "center room is \"living room\" as string");
                Assert.That(center.ExitList().Count(), Is.EqualTo(0), "center room has empty exit list before exits added");

                // Add non-center locations to center location out of order
                center.AddExit(Direction.North, north_kitchen);
                center.AddExit(Direction.Northeast, northeast_pantry);
                center.AddExit(Direction.East, east_gameRoom);
                center.AddExit(Direction.Southeast, southeast_study);
                center.AddExit(Direction.South, south_office);
                center.AddExit(Direction.Southwest, southwest_sensoryRoom);
                center.AddExit(Direction.West, west_bedroom);
                center.AddExit(Direction.Northwest, northwest_storageRoom);
                center.AddExit(Direction.In, in_closet);
                center.AddExit(Direction.Out, out_yard);
                center.AddExit(Direction.Up, up_attic);
                center.AddExit(Direction.Down, down_basement);

                // Assert that center location's exit list has expected number of items
                Assert.That(center.ExitList().Count(), Is.EqualTo(12), "center room has 12 exits in exit list after exits added");
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Location.Random = new Random(); // Set Location Random number generator to fresh generator
        }

        /// <summary>
        /// Assert that GetRandomExit method returns appropriate Location using mock of Random
        /// 
        /// CREDIT: adapted from HideAndSeek project's TestHouse class's TestRandomExit() test method
        ///         � 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/TestHouse.cs
        ///         Link valid as of 02-26-2025
        /// 
        /// CHANGES:
        /// -I moved the method and property to the Location class since this pertains to Location rather than House.
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I added some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// -I moved the GetLocationByName method call for getting the Kitchen Location to the beginning of the test method.
        /// </summary>
        [Test]
        [Category("GetRandomExit Success")]
        public void Test_Location_GetRandomExit()
        {
            // Set House random number generator to mock
            Location.Random = new MockRandomWithValueList(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

            Assert.Multiple(() =>
            {
                // Assert Center's random exits
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("kitchen"), "exit at index 0");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("pantry"), "exit at index 1");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("game room"), "exit at index 2");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("study"), "exit at index 3");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("office"), "exit at index 4");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("sensory room"), "exit at index 5");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("bedroom"), "exit at index 6");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("storage room"), "exit at index 7");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("closet"), "exit at index 8");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("yard"), "exit at index 9");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("attic"), "exit at index 10");
                Assert.That(center.GetRandomExit().Name, Is.EqualTo("basement"), "exit at index 11");
            });
        }

        [Test]
        [Category("Location GetExit Success")]
        public void Test_Location_GetExit()
        {
            // Initialize array of expected exits (one per direction)
            Location[] expectedExits =
            {
                in_closet,
                up_attic,
                southeast_study,
                northeast_pantry,
                east_gameRoom,
                north_kitchen,
                south_office,
                west_bedroom,
                southwest_sensoryRoom,
                northwest_storageRoom,
                down_basement,
                out_yard
            };

            // Initialize array of actual exits returned
            Location[] actualExits =
            {
                center.GetExit(Direction.In),
                center.GetExit(Direction.Up),
                center.GetExit(Direction.Southeast),
                center.GetExit(Direction.Northeast),
                center.GetExit(Direction.East),
                center.GetExit(Direction.North),
                center.GetExit(Direction.South),
                center.GetExit(Direction.West),
                center.GetExit(Direction.Southwest),
                center.GetExit(Direction.Northwest),
                center.GetExit(Direction.Down),
                center.GetExit(Direction.Out)
            };

            // Assert that the returned exits are as expected (one exit returned per direction)
            Assert.That(actualExits, Is.EquivalentTo(expectedExits));
        }

        [Test]
        [Category("Location GetExit InvalidOperationException Failure")]
        public void Test_Location_GetExit_AndCheckErrorMessage_WhenNoLocationInDirection()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling get exit with direction in which no location exists raises exception
                Exception e = Assert.Throws<InvalidOperationException>(() =>
                {
                    up_attic.GetExit(Direction.Up);
                });

                // Assert that error message is as expected
                Assert.That(e.Message, Is.EqualTo("There is no exit for location \"attic\" in direction \"Up\""));
            });
        }

        [Test]
        [Category("Location ExitList Success")]
        public void Test_Location_ExitList_ForCenterLocation()
        {
            // Initialize array of expected exit descriptions
            List<string> expectedExitList = new List<string>()
            {
                "the closet is In",
                "the attic is Up",
                "the study is to the Southeast",
                "the pantry is to the Northeast",
                "the game room is to the East",
                "the kitchen is to the North",
                "the office is to the South",
                "the bedroom is to the West",
                "the sensory room is to the Southwest",
                "the storage room is to the Northwest",
                "the basement is Down",
                "the yard is Out"
            };

            // Initialize enumerable of actual exits
            IEnumerable<string> actualExitList = center.ExitList();

            // Assert that exit list is correct
            Assert.That(actualExitList, Is.EquivalentTo(expectedExitList));
        }

        [Test]
        [Category("Location ExitList Success")]
        public void Test_Location_ExitList_ForNotCenterLocations()
        {
            // Expected exit lists for not-center locations
            IEnumerable<string> expectedClosetExitList = new List<string>() { "the living room is Out" };
            IEnumerable<string> expectedAtticExitList = new List<string>() { "the living room is Down" };
            IEnumerable<string> expectedStudyExitList = new List<string>() { "the living room is to the Northwest" };
            IEnumerable<string> expectedPantryExitList = new List<string>() { "the living room is to the Southwest" };
            IEnumerable<string> expectedGameRoomExitList = new List<string>() { "the living room is to the West" };
            IEnumerable<string> expectedKitchenExitList = new List<string>() { "the living room is to the South" };
            IEnumerable<string> expectedOfficeExitList = new List<string>() { "the living room is to the North" };
            IEnumerable<string> expectedBedroomExitList = new List<string>() { "the living room is to the East" };
            IEnumerable<string> expectedSensoryRoomExitList = new List<string>() { "the living room is to the Northeast" };
            IEnumerable<string> expectedStorageRoomExitList = new List<string>() { "the living room is to the Southeast" };
            IEnumerable<string> expectedBasementExitList = new List<string>() { "the living room is Up" };
            IEnumerable<string> expectedYardExitList = new List<string>() { "the living room is In" };

            // Assert that exit lists are as expected
            Assert.Multiple(() =>
            {
                Assert.That(in_closet.ExitList(), Is.EquivalentTo(expectedClosetExitList), "closet exit list");
                Assert.That(up_attic.ExitList(), Is.EquivalentTo(expectedAtticExitList), "attic exit list");
                Assert.That(southeast_study.ExitList(), Is.EquivalentTo(expectedStudyExitList), "study exit list");
                Assert.That(northeast_pantry.ExitList(), Is.EquivalentTo(expectedPantryExitList), "pantry exit list");
                Assert.That(east_gameRoom.ExitList(), Is.EquivalentTo(expectedGameRoomExitList), "game room exit list");
                Assert.That(north_kitchen.ExitList(), Is.EquivalentTo(expectedKitchenExitList), "kitchen exit list");
                Assert.That(south_office.ExitList(), Is.EquivalentTo(expectedOfficeExitList), "office exit list");
                Assert.That(west_bedroom.ExitList(), Is.EquivalentTo(expectedBedroomExitList), "bedroom exit list");
                Assert.That(southwest_sensoryRoom.ExitList(), Is.EquivalentTo(expectedSensoryRoomExitList), "sensory room exit list");
                Assert.That(northwest_storageRoom.ExitList(), Is.EquivalentTo(expectedStorageRoomExitList), "storage room exit list");
                Assert.That(down_basement.ExitList(), Is.EquivalentTo(expectedBasementExitList), "basement exit list");
                Assert.That(out_yard.ExitList(), Is.EquivalentTo(expectedYardExitList), "yard exit list");
            });
        }

        /// <summary>
        /// Add a hallway with two Locations to one of the rooms
        /// and make sure the rooms and return exits are created correctly
        /// </summary>
        [Test]
        [Category("Location AddExit ExitList Success")]
        public void Test_Location_AddHall_AndCheckExitLists()
        {
            // Create hallway and add to basement
            Location hallway = new Location("hallway");
            down_basement.AddExit(Direction.East, hallway);

            // Create new rooms
            Location north_bathroom = new Location("bathroom");
            Location south_gym = new Location("gym");

            // Add new rooms to hallway
            hallway.AddExit(Direction.North, north_bathroom);
            hallway.AddExit(Direction.South, south_gym);

            // Get exit lists
            IEnumerable<string> basementExitList = down_basement.ExitList();
            IEnumerable<string> hallwayExitList = hallway.ExitList();
            IEnumerable<string> gymExitList = south_gym.ExitList();
            IEnumerable<string> bathroomExitList = north_bathroom.ExitList();

            // Assert exit lists are correct length and have correct elements
            Assert.Multiple(() =>
            {
                // Exit list for basement
                Assert.That(basementExitList.Count, Is.EqualTo(2), "2 basement exits");
                Assert.That(basementExitList.ElementAt(0), Is.EqualTo("the living room is Up"), "first basement exit is living room");
                Assert.That(basementExitList.ElementAt(1), Is.EqualTo("the hallway is to the East"), "second basement exit is hallway");

                // Exit list for hallway
                Assert.That(hallwayExitList.Count, Is.EqualTo(3), "3 hallway exits");
                Assert.That(hallwayExitList.ElementAt(0), Is.EqualTo("the bathroom is to the North"), "first exit is bathroom");
                Assert.That(hallwayExitList.ElementAt(1), Is.EqualTo("the gym is to the South"), "second exit is gym");
                Assert.That(hallwayExitList.ElementAt(2), Is.EqualTo("the basement is to the West"), "third exit is basement");

                // Exit list for bathroom
                Assert.That(bathroomExitList.Count, Is.EqualTo(1), "1 bathroom exit");
                Assert.That(bathroomExitList.ElementAt(0), Is.EqualTo("the hallway is to the South"), "the bathroom exit");

                // Exit list for gym
                Assert.That(gymExitList.Count, Is.EqualTo(1), "1 gym exit");
                Assert.That(gymExitList.ElementAt(0), Is.EqualTo("the hallway is to the North"), "the gym exit");
            });
        }

        /// <summary>
        /// Call AddExit method with a string name for a new Location and make sure the Location is created and added properly
        /// </summary>
        [Test]
        [Category("Location AddExit GetExit ExitList Success")]
        public void Test_Location_AddExit_WithConstructorAcceptingName()
        {
            // Create expected exit list for yard
            IEnumerable<string> expectedYardExitList = new List<string>() {
                "the living room is In",
                "the shed is to the North"
            };

            // Create expected exit list for north location
            IEnumerable<string> expectedNorthLocationExitList = new List<string>() { "the yard is to the South" };

            // Call method to add location to north of yard
            out_yard.AddExit(Direction.North, "shed");

            // Get the location north of yard
            Location northLocation = out_yard.GetExit(Direction.North);

            // Assert that exit lists and north location name are as expected
            Assert.Multiple(() =>
            {
                Assert.That(out_yard.ExitList, Is.EquivalentTo(expectedYardExitList), "yard exit list");
                Assert.That(northLocation.Name, Is.EqualTo("shed"), "north location name");
                Assert.That(northLocation.ExitList, Is.EqualTo(expectedNorthLocationExitList), "north location exit list");
            });
        }

        /// <summary>
        /// Call AddExit method with a string name and description for a new LocationWithHidingPlace
        /// and make sure the LocationWithHidingPlace is created and added properly
        /// </summary>
        [Test]
        [Category("Location AddExit GetExit ExitList Success")]
        public void Test_Location_AddExit_WithConstructorAcceptingNameAndHidingPlace()
        {
            // Create expected exit list for yard
            IEnumerable<string> expectedYardExitList = new List<string>() {
                "the living room is In",
                "the shed is to the North"
            };

            // Create expected exit list for north location
            IEnumerable<string> expectedNorthLocationExitList = new List<string>() { "the yard is to the South" };

            // Call method to add LocationWithHidingPlace to north of yard
            out_yard.AddExit(Direction.North, "shed", "in the wheelbarrow");

            // Get the location north of yard and convert to LocationWithHidingPlace
            LocationWithHidingPlace northLocation = (LocationWithHidingPlace)out_yard.GetExit(Direction.North);

            // Assert that exit lists and north location properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(out_yard.ExitList, Is.EquivalentTo(expectedYardExitList), "yard exit list");
                Assert.That(northLocation.Name, Is.EqualTo("shed"), "north location name");
                Assert.That(northLocation.HidingPlace, Is.EqualTo("in the wheelbarrow"));
                Assert.That(northLocation.ExitList, Is.EqualTo(expectedNorthLocationExitList), "north location exit list");
            });
        }

        [Test]
        [Category("Location Name Success")]
        public void Test_Location_Set_Name()
        {
            // Create Location
            Location myLocation = new Location("secret attic");

            Assert.Multiple(() =>
            {
                // Assert that Location has expected name
                Assert.That(myLocation.Name, Is.EqualTo("secret attic"));

                // Change Location Name and assert that it was set successfully
                myLocation.Name = "secret basement";
                Assert.That(myLocation.Name, Is.EqualTo("secret basement"));
            });

        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Location Name ArgumentException Failure")]
        public void Test_Location_Set_Name_AndCheckErrorMessage_ForInvalidName(string name)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the Name to an invalid name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    center.Name = name;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"location name \"{name}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("Location Exits Success")]
        public void Test_Location_Set_Exits()
        {
            // Create dictionary of exits
            Dictionary<Direction, Location> exits = new Dictionary<Direction, Location>()
            {
                { Direction.Out, out_yard },
                { Direction.North, north_kitchen }
            };

            // Set exits dictionary for center location
            center.Exits = exits;

            // Check that Exits property was set successfully
            Assert.That(center.Exits, Is.EquivalentTo(exits));
        }

        [Test]
        [Category("Location Exits ArgumentException Failure")]
        public void Test_Location_Set_Exits_AndCheckErrorMessage_ForEmptyDictionary()
        {
            Assert.Multiple(() =>
            {
                // Assert that setting exits dictionary to empty dictionary raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    center.Exits = new Dictionary<Direction, Location>();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("location \"living room\" must be assigned at least one exit"));
            });
        }

        [Test]
        [Category("Location ExitsForSerialization Success")]
        public void Test_Location_Set_ExitsForSerialization()
        {
            // Create dictionary of exits
            Dictionary<Direction, string> exits = new Dictionary<Direction, string>()
            {
                { Direction.Northeast, "Secret Library" },
                { Direction.In, "Secret Snack Corner" }
            };

            // Set ExitsForSerialization property
            northwest_storageRoom.ExitsForSerialization = exits;

            // Assert that property has been set successfully
            Assert.That(northwest_storageRoom.ExitsForSerialization, Is.EquivalentTo(exits));
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Location ExitsForSerialization ArgumentException Failure")]
        public void Test_Location_Set_ExitsForSerialization_AndCheckErrorMessage_ForInvalidExitLocationName(string locationName)
        {
            // Create dictionary of exits
            Dictionary<Direction, string> exits = new Dictionary<Direction, string>()
            {
                { Direction.South, "Game Room" },
                { Direction.North, locationName }
            };

            Assert.Multiple(() =>
            {
                // Assert that setting the ExitsForSerialization property to a dictionary with an invalid location name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    center.ExitsForSerialization = exits;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"location name \"{locationName}\" for exit in direction \"North\" " +
                                                           "is invalid (is empty or contains only whitespace"));
            });
        }

        [Test]
        [Category("Location PrepForSerialization Success")]
        public void Test_Location_PrepForSerialization_WithNoExits()
        {
            // Create Location with no exits
            Location location = new Location("sealed room");

            Assert.Multiple(() =>
            {
                // Assert that ExitsForSerialization is null before prep
                Assert.That(center.ExitsForSerialization, Is.Null, "null before PrepForSerialization");

                // ACT: Prep for serialization
                location.PrepForSerialization();

                // Assert that ExitsForSerialization has been set successfully
                Assert.That(location.ExitsForSerialization, Is.Empty, "set properly by PrepForSerialization");
            });
        }

        [Test]
        [Category("Location PrepForSerialization Success")]
        public void Test_Location_PrepForSerialization_WithExits()
        {
            // Create dictionary of expected exits
            Dictionary<Direction, string> expectedexits = new Dictionary<Direction, string>()
            {
                { Direction.North, "kitchen" },
                { Direction.Northeast, "pantry" },
                { Direction.East, "game room" },
                { Direction.Southeast, "study" },
                { Direction.South, "office" },
                { Direction.Southwest, "sensory room" },
                { Direction.West, "bedroom" },
                { Direction.Northwest, "storage room" },
                { Direction.In, "closet" },
                { Direction.Out, "yard" },
                { Direction.Up, "attic" },
                { Direction.Down, "basement" }
            };
            Assert.Multiple(() =>
            {
                // Assert that ExitsForSerialization is null before prep
                Assert.That(center.ExitsForSerialization, Is.Null, "null before PrepForSerialization");
                
                // ACT: Prep for serialization
                center.PrepForSerialization();

                // Assert that ExitsForSerialization has been set successfully
                Assert.That(center.ExitsForSerialization, Is.EquivalentTo(expectedexits), "set properly by PrepForSerialization");
            });
        }

        [Test]
        [Category("Location Serialization Success")]
        public void Test_Location_Serialization()
        {
            // Set expected serialized Location text
            string expectedSerializedLocation =
                "{\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":" +
                    "{" +
                        "\"North\":\"kitchen\"," +
                        "\"Northeast\":\"pantry\"," +
                        "\"East\":\"game room\"," +
                        "\"Southeast\":\"study\"," +
                        "\"South\":\"office\"," +
                        "\"Southwest\":\"sensory room\"," +
                        "\"West\":\"bedroom\"," +
                        "\"Northwest\":\"storage room\"," +
                        "\"In\":\"closet\"," +
                        "\"Out\":\"yard\"," +
                        "\"Up\":\"attic\"," +
                        "\"Down\":\"basement\"" +
                    "}" +
                "}";

            // Serialize Location
            string serializedLocation = center.Serialize();

            // Assert that serialized Location text is as expected
            Assert.That(serializedLocation, Is.EqualTo(expectedSerializedLocation));
        }

        // Does not test restoring Exits dictionary (this is done in House class)
        [Test]
        [Category("Location Deserialization Success")]
        public void Test_Location_Deserialization()
        {
            // Set expected ExitsForSerialization property value
            IDictionary<Direction, string> expectedExitsForSerialization = new Dictionary<Direction, string>()
            {
                { Direction.North, "kitchen" },
                { Direction.Northeast, "pantry" },
                { Direction.East, "game room" },
                { Direction.Southeast, "study" },
                { Direction.South, "office" },
                { Direction.Southwest, "sensory room" },
                { Direction.West, "bedroom" },
                { Direction.Northwest, "storage room" },
                { Direction.In, "closet" },
                { Direction.Out, "yard" },
                { Direction.Up, "attic" },
                { Direction.Down, "basement" }
            };

            // Text representing serialized location
            string serializedLocation = 
                "{\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":" +
                    "{" +
                        "\"North\":\"kitchen\"," +
                        "\"Northeast\":\"pantry\"," +
                        "\"East\":\"game room\"," +
                        "\"Southeast\":\"study\"," +
                        "\"South\":\"office\"," +
                        "\"Southwest\":\"sensory room\"," +
                        "\"West\":\"bedroom\"," +
                        "\"Northwest\":\"storage room\"," +
                        "\"In\":\"closet\"," +
                        "\"Out\":\"yard\"," +
                        "\"Up\":\"attic\"," +
                        "\"Down\":\"basement\"" +
                    "}" +
                "}";

            // Deserialize Location
            Location deserializedLocation = JsonSerializer.Deserialize<Location>(serializedLocation);

            // Assert that deserialized Location's Name and ExitsForSerialization properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(deserializedLocation.Name, Is.EqualTo("living room"), "name");
                Assert.That(deserializedLocation.ExitsForSerialization, Is.EquivalentTo(expectedExitsForSerialization), "exits for serialization");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Location Deserialization ArgumentException Failure")]
        public void Test_Location_Deserialization_AndCheckErrorMessage_ForInvalidName(string name)
        {
            // Set expected ExitsForSerialization property value
            IDictionary<Direction, string> expectedExitsForSerialization = new Dictionary<Direction, string>() { { Direction.North, "kitchen" } };

            // Text representing serialized Location
            string serializedLocation = 
                "{\"Name\":\"" + name + "\"," +
                "\"ExitsForSerialization\":" +
                    "{" +
                        "\"North\":\"kitchen\"" +
                    "}" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that deserializing raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    JsonSerializer.Deserialize<Location>(serializedLocation);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"location name \"{name}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Location Deserialization ArgumentException Failure")]
        public void Test_Location_Deserialization_AndCheckErrorMessage_ForInvalidExitLocationName(string name)
        {
            // Text representing serialized Location
            string serializedLocation =
                "{\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":" +
                    "{" +
                        "\"North\":\"" + name + "\"" +
                    "}" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that deserializing raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    JsonSerializer.Deserialize<Location>(serializedLocation);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"location name \"{name}\" for exit in direction \"North\" " +
                                                               "is invalid (is empty or contains only whitespace"));
            });
        }
    }
}