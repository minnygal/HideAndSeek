using HideAndSeek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Static class providing House object for testing and serialized House text for testing
    /// </summary>
    public static class TestHouse_Data
    {
        /// <summary>
        /// Enumerable of expected Location names in Locations property in test House
        /// </summary>
        public static readonly IEnumerable<string> TestHouseExpectedProperties_Locations_Names = new List<string>()
        {
            "Attic",
            "Hallway",
            "Bathroom",
            "Kids Room",
            "Master Bedroom",
            "Nursery",
            "Pantry",
            "Second Bathroom",
            "Kitchen",
            "Master Bath",
            "Garage",
            "Landing",
            "Living Room",
            "Entry"
        };

        /// <summary>
        /// Enumerable of expected LocationWithHidingPlace names in LocationsWithHidingPlaces property in test House
        /// </summary>
        public static readonly IEnumerable<string> TestHouseExpectedProperties_LocationsWithHidingPlaces_Names = new List<string>()
        {
            "Attic",
            "Bathroom",
            "Kids Room",
            "Master Bedroom",
            "Nursery",
            "Pantry",
            "Second Bathroom",
            "Kitchen",
            "Master Bath",
            "Garage",
            "Living Room",
        };

        /// <summary>
        /// Enumerable of expected Location names in LocationsWithoutHidingPlaces property in test House
        /// </summary>
        public static readonly IEnumerable<string> TestHouseExpectedProperties_LocationsWithoutHidingPlaces_Names = new List<string>()
        {
            "Hallway",
            "Landing",
            "Entry"
        };

        /// <summary>
        /// Get new House object for testing purposes
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        public static House GetNewTestHouse()
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
                return new House("test house", "TestHouse", "Entry", locationsWithoutHidingPlaces, locationsWithHidingPlaces);
            }

        /// <summary>
        /// Text for serialized test House
        /// </summary>
        public static string SerializedTestHouse
        {
            get
            {
                return "{" +
                        SerializedTestHouse_Name + "," +
                        SerializedHouse_HouseFileName + "," +
                        SerializedHouse_PlayerStartingPoint + "," +
                        SerializedHouse_LocationsWithoutHidingPlaces + "," +
                        SerializedTestHouse_LocationsWithHidingPlaces +
                        "}";
            }
        }   

        /// <summary>
        /// Text for serialized Name property for test House
        /// </summary>
        public static string SerializedTestHouse_Name
        {
            get
            {
                return "\"Name\":\"test house\"";
            }
        }

        /// <summary>
        /// Text for serialized HouseFileName property for test House
        /// </summary>
        public static string SerializedHouse_HouseFileName
        {
            get
            {
                return "\"HouseFileName\":\"TestHouse\"";
            }
        }

        /// <summary>
        /// Text for serialized PlayerStartingPoint property for test House
        /// </summary>
        public static string SerializedHouse_PlayerStartingPoint
        {
            get
            {
                return "\"PlayerStartingPoint\":\"Entry\"";
            }
        }

        /// <summary>
        /// Text for serialized LocationsWithoutHidingPlaces property for test House
        /// </summary>
        public static string SerializedHouse_LocationsWithoutHidingPlaces
        {
            get
            {
                return "\"LocationsWithoutHidingPlaces\":" +
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
                        "]";
            }
        }

        /// <summary>
        /// Text for  serialized LocationsWithHidingPlaces property for test House
        /// </summary>
        public static string SerializedTestHouse_LocationsWithHidingPlaces
        {
            get
            {
                return "\"LocationsWithHidingPlaces\":" +
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
                        "]";
            }
        }
    }
}
