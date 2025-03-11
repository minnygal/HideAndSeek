namespace HideAndSeek
{
    using HideAndSeek;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    /// <summary>
    /// LocationWithHidingPlace tests for testing properties and methods HideOpponent and CheckHidingPlace
    /// </summary>
    [TestFixture]
    public class LocationWithHidingPlaceTests
    {
        [Test]
        [Category("LocationWithHidingPlace")]
        public void Test_LocationWithHidingPlace_Properties()
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
        [Category("LocationWithHidingPlace HideOpponent CheckHidingPlace")]
        public void Test_LocationWithHidingPlace_HideOpponents_And_CheckHidingPlace()
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
        public void Test_LocationWithHidingPlace_HidingPlaceSetter()
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
        public void Test_LocationWithHidingPlace_HidingPlace_ThrowsException_WhenInvalidDescriptionPassed(string description)
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
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because hiding place \"{description}\" is invalid (is empty or contains only whitespace"));
            });
        }

        [Test]
        [Category("LocationWithHidingPlace Serialization Success")]
        public void Test_LocationWithHidingPlace_Serialization()
        {
            // Set expected serialized Location text
            string expectedSerializedLocation =
                "{\"HidingPlace\":\"behind the piano\"," +
                "\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":{" +
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
                    "\"Down\":\"basement\"}}";

            // Serialize Location
            string serializedLocation = GetLocationWithSerializationTests().Serialize();
            Console.WriteLine(serializedLocation);
            // Assert that serialized Location text is as expected
            Assert.That(serializedLocation, Is.EqualTo(expectedSerializedLocation));
        }

        // Does not test restoring Exits dictionary (this is done in House class)
        [Test]
        [Category("LocationWithHidingPlace Deserialization Success")]
        public void Test_LocationWithHidingPlace_Deserialization()
        {
            // Set expected ExitsForSerialization property value
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

            // Set text representing serialized object
            string serializedLocation =
                "{\"HidingPlace\":\"behind the piano\"," +
                "\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":{" +
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
                    "\"Down\":\"basement\"}}";

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

        [TestCase("")]
        [TestCase(" ")]
        [Category("LocationWithHidingPlace Deserialization Failure")]
        public void Test_LocationWithHidingPlace_Deserialization_ThrowsException_OverInvalidHidingLocation(string hidingPlace)
        {
            // Set expected ExitsForSerialization property value
            IDictionary<Direction, string> expectedExitsForSerialization = new Dictionary<Direction, string>();
            expectedExitsForSerialization.Add(Direction.North, "kitchen");

            // Set text representing serialized object
            string serializedLocation =
                "{\"HidingPlace\":\"" + hidingPlace + "\"," +
                "\"Name\":\"living room\"," +
                "\"ExitsForSerialization\":{" +
                    "\"North\":\"kitchen\"}}";

            Assert.Multiple(() =>
            {
                // Assert that deserializing throws exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    JsonSerializer.Deserialize<LocationWithHidingPlace>(serializedLocation);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because hiding place \"{hidingPlace}\" is invalid (is empty or contains only whitespace"));
            });
        }

        public LocationWithHidingPlace GetLocationWithSerializationTests()
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