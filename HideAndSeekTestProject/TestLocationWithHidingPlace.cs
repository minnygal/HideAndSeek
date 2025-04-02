using HideAndSeek;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// LocationWithHidingPlace tests for properties and methods HideOpponent, CheckHidingPlace, and Serialize
    /// </summary>
    [TestFixture]
    public class TestLocationWithHidingPlace
    {
        [Test]
        [Category("LocationWithHidingPlace Success")]
        public void Test_LocationWithHidingPlace_Constructor()
        {
            // Create a new LocationWithHidingPlace, setting the Name and HidingPlace properties
            var hidingLocation = new LocationWithHidingPlace("Room", "under the bed");

            // Assert that properties have been set correctly
            Assert.Multiple(() =>
            {
                Assert.That(hidingLocation.Name, Is.EqualTo("Room"), "location name");
                Assert.That(hidingLocation.ToString(), Is.EqualTo("Room"), "location as string");
                Assert.That(hidingLocation.HidingPlace, Is.EqualTo("under the bed"), "HidingPlace property");
            });
        }

        [Test]
        [Category("LocationWithHidingPlace HideOpponent CheckHidingPlace Success")]
        public void Test_LocationWithHidingPlace_HideOpponent_And_CheckHidingPlace()
        {
            // Create a new LocationWithHidingPlace
            var hidingLocation = new LocationWithHidingPlace("Room", "under the bed");

            // Create two opponents
            var opponent1 = new Opponent();
            var opponent2 = new Opponent();

            // Hide opponents in hiding place
            hidingLocation.HideOpponent(opponent1);
            hidingLocation.HideOpponent(opponent2);

            // Initialize list of expected opponents in hiding place
            List<Opponent> expectedOpponentsInHidingPlace = new List<Opponent>() { opponent1, opponent2 };

            // Check for opponents in hiding place
            Assert.Multiple(() =>
            {
                Assert.That(hidingLocation.CheckHidingPlace(), Is.EquivalentTo(expectedOpponentsInHidingPlace), "opponents found when hiding place checked"); // Check hiding place, finding opponents
                Assert.That(hidingLocation.CheckHidingPlace(), Is.Empty, "hiding place empty after being checked"); // Check hiding place again, should be empty because opponents already found
            });
        }

        [Test]
        [Category("LocationWithHidingPlace HidingPlace Success")]
        public void Test_LocationWithHidingPlace_Set_HidingPlace()
        {
            // Create LocationWithHidingPlace
            LocationWithHidingPlace myLocation = new LocationWithHidingPlace("garden", "in the bushes");

            Assert.Multiple(() =>
            {
                // Assert that LocationWithHidingPlace has expected hiding place
                Assert.That(myLocation.HidingPlace, Is.EqualTo("in the bushes"));

                // Change LocationWithHidingPlace hiding place and assert that it was set successfully
                myLocation.HidingPlace = "in a tree";
                Assert.That(myLocation.HidingPlace, Is.EqualTo("in a tree"));
            });

        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("LocationWithHidingPlace HidingPlace Failure")]
        public void Test_LocationWithHidingPlace_Set_HidingPlace_AndCheckErrorMessage_ForInvalidDescription(string description)
        {
            // Create LocationWithHidingPlace
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Garden", "in the bushes");
            
            Assert.Multiple(() =>
            {
                // Assert that setting the hiding place to an invalid description throws exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    locationWithHidingPlace.HidingPlace = description;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because hiding place \"{description}\" is invalid " +
                                                           "(is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("LocationWithHidingPlace Serialize Success")]
        public void Test_LocationWithHidingPlace_Serialize()
        {
            // Set expected serialized Location text
            string expectedSerializedLocation =
                "{\"HidingPlace\":\"behind the piano\"," +
                "\"Name\":\"living room\"," +
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
            string serializedLocation = GetLocationForSerializationTests().Serialize();
            
            // Assert that serialized Location text is as expected
            Assert.That(serializedLocation, Is.EqualTo(expectedSerializedLocation));
        }

        // Does not test restoring Exits dictionary (this is done in House class)
        [Test]
        [Category("LocationWithHidingPlace Deserialize Success")]
        public void Test_LocationWithHidingPlace_Deserialize()
        {
            // Initialize to expected ExitsForSerialization property value
            IDictionary<Direction, string> expectedExitsForSerialization = new Dictionary<Direction, string>();
            expectedExitsForSerialization.Add(Direction.North, "kitchen");
            expectedExitsForSerialization.Add(Direction.Northeast, "pantry");
            expectedExitsForSerialization.Add(Direction.East, "game room");
            expectedExitsForSerialization.Add(Direction.Southeast, "study");
            expectedExitsForSerialization.Add(Direction.South, "office");
            expectedExitsForSerialization.Add(Direction.Southwest, "sensory room");
            expectedExitsForSerialization.Add(Direction.West, "bedroom");
            expectedExitsForSerialization.Add(Direction.Northwest, "storage room");
            expectedExitsForSerialization.Add(Direction.In, "closet");
            expectedExitsForSerialization.Add(Direction.Out, "yard");
            expectedExitsForSerialization.Add(Direction.Up, "attic");
            expectedExitsForSerialization.Add(Direction.Down, "basement");

            // Initialize to text representing serialized object
            string serializedLocation =
                "{\"HidingPlace\":\"behind the piano\"," +
                "\"Name\":\"living room\"," +
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

            // Deserialize LocationWithHidingPlace
            LocationWithHidingPlace deserializedLocation = JsonSerializer.Deserialize<LocationWithHidingPlace>(serializedLocation);

            // Assert that deserialized Location's Name and ExitsForSerialization properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(deserializedLocation.Name, Is.EqualTo("living room"));
                Assert.That(deserializedLocation.HidingPlace, Is.EqualTo("behind the piano"));
                Assert.That(deserializedLocation.ExitsForSerialization, Is.EquivalentTo(expectedExitsForSerialization));
            });
        }

        // Tests HidingLocation data validation
        [TestCase("")]
        [TestCase(" ")]
        [Category("LocationWithHidingPlace Deserialize Failure")]
        public void Test_LocationWithHidingPlace_Deserialize_AndCheckErrorMessage_ForInvalidHidingLocation(string hidingPlace)
        {
            // Initialize to expected ExitsForSerialization property value
            IDictionary<Direction, string> expectedExitsForSerialization = new Dictionary<Direction, string>();
            expectedExitsForSerialization.Add(Direction.North, "kitchen");

            // Initialize to text representing serialized object
            string serializedLocation =
                "{\"HidingPlace\":\"" + hidingPlace + "\"," +
                "\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":" +
                    "{" +
                        "\"North\":\"kitchen\"" +
                    "}" +
                "}";

            Assert.Multiple(() =>
            {
                // Assert that deserializing raises exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    JsonSerializer.Deserialize<LocationWithHidingPlace>(serializedLocation);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because hiding place \"{hidingPlace}\" is invalid " +
                                                           "(is empty or contains only whitespace)"));
            });
        }

        /// <summary>
        /// Helper method to get LocationWithHidingPlace for serialization test
        /// </summary>
        /// <returns>LocationWithHidingPlace for serialization test</returns>
        private LocationWithHidingPlace GetLocationForSerializationTests()
        {
            // Initialize variables to new Locations/LocationWithHidingPlaces
            LocationWithHidingPlace center = new LocationWithHidingPlace("living room", "behind the piano");
            LocationWithHidingPlace in_closet = new LocationWithHidingPlace("closet", "between the coats");
            LocationWithHidingPlace up_attic = new LocationWithHidingPlace("attic", "behind a trunk");
            Location southeast_study = new Location("study");
            Location northeast_pantry = new Location("pantry");
            LocationWithHidingPlace east_gameRoom = new LocationWithHidingPlace("game room", "under the bean bags");
            LocationWithHidingPlace north_kitchen = new LocationWithHidingPlace("kitchen", "behind the refrigerator");
            LocationWithHidingPlace south_office = new LocationWithHidingPlace("office", "under the table");
            LocationWithHidingPlace west_bedroom = new LocationWithHidingPlace("bedroom", "uhder the bed");
            LocationWithHidingPlace southwest_sensoryRoom = new LocationWithHidingPlace("sensory room", "under the trampoline");
            LocationWithHidingPlace northwest_storageRoom = new LocationWithHidingPlace("storage room", "behind the boxes");
            LocationWithHidingPlace down_basement = new LocationWithHidingPlace("basement", "behind the pipes");
            LocationWithHidingPlace out_yard = new LocationWithHidingPlace("yard", "is the shed");

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

            // Return center location
            return center;
        }
    }
}