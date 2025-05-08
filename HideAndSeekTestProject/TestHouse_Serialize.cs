using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for Serialize method
    /// 
    /// These are integration tests using Location and LocationWithHidingPlace
    /// </summary>
    [TestFixture]
    public class TestHouse_Serialize
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            house = null;
        }

        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        private static string DefaultHouse_Serialized
        {
            get
            {
                return
                    "{" +
                        "\"Name\":\"my house\"" + "," +
                        "\"HouseFileName\":\"DefaultHouse\"" + "," +
                        "\"PlayerStartingPoint\":\"Entry\"" + "," +
                        "\"LocationsWithoutHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"Name\":\"Hallway\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Entry\"," +
                                    "\"Northwest\":\"Kitchen\"," +
                                    "\"North\":\"Bathroom\"," +
                                    "\"South\":\"Living Room\"," +
                                    "\"Up\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Landing\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Hallway\"," +
                                    "\"Up\":\"Attic\"," +
                                    "\"Southeast\":\"Kids Room\"," +
                                    "\"Northwest\":\"Master Bedroom\"," +
                                    "\"Southwest\":\"Nursery\"," +
                                    "\"South\":\"Pantry\"," +
                                    "\"West\":\"Second Bathroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Entry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Out\":\"Garage\"," +
                                    "\"East\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" + "," +
                        "\"LocationsWithHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"HidingPlace\":\"in a trunk\"," +
                                "\"Name\":\"Attic\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{\"HidingPlace\":\"behind the door\"," +
                                "\"Name\":\"Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"South\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the bunk beds\"," +
                                "\"Name\":\"Kids Room\"," +
                                "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Northwest\":\"Landing\"" +
                                    "}" +
                                "}," +
                            "{" +
                                "\"HidingPlace\":\"under the bed\"," +
                                "\"Name\":\"Master Bedroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Landing\"," +
                                    "\"East\":\"Master Bath\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the changing table\"," +
                                "\"Name\":\"Nursery\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Northeast\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"inside a cabinet\"," +
                                "\"Name\":\"Pantry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the shower\"," +
                                "\"Name\":\"Second Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"East\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"next to the stove\"," +
                                "\"Name\":\"Kitchen\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the tub\"," +
                                "\"Name\":\"Master Bath\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Master Bedroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the car\"," +
                                "\"Name\":\"Garage\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"In\":\"Entry\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the sofa\"," +
                                "\"Name\":\"Living Room\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" +
                    "}";
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
            return new House("my house", "DefaultHouse", entry, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_DefaultHouse()
        {
            house = GetDefaultHouse();
            Assert.That(house.Serialize(), Is.EqualTo(DefaultHouse_Serialized));
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
    }
}