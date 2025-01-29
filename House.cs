using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public static class House {
        /// <summary>
        /// Entry of House
        /// </summary>
        public static Location Entry { get; private set; }

        /// <summary>
        /// List of Locations in House
        /// </summary>
        public static List<Location> Locations { get; private set; }

        public static Random Random { get; set; } = new Random();

        /// <summary>
        /// Static constructor to create Locations within the house and connect them
        /// </summary>
        static House()
        {
            // Create Entry and connect to new locations: Garage, Hallway
            Entry = new Location("Entry");
            LocationWithHidingPlace garage = Entry.AddExit(Direction.Out, "Garage", "behind the car");
            Location hallway = Entry.AddExit(Direction.East, "Hallway");

            // Connect Hallway to new locations: Kitchen, Bathroom, Living Room, Landing
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.Northwest, "Kitchen", "next to the stove");
            LocationWithHidingPlace bathroom = hallway.AddExit(Direction.North, "Bathroom", "behind the door");
            LocationWithHidingPlace livingRoom = hallway.AddExit(Direction.South, "Living Room", "behind the sofa");

            // Create landing
            Location landing = new Location("Landing");

            // Connect Landing to new locations: Attic, Hallway, Kids Room, Master Bedroom, Nursery, Pantry, Second Bathroom
            LocationWithHidingPlace attic = landing.AddExit(Direction.Up, "Attic", "in a trunk");
            landing.AddExit(Direction.Down, hallway);
            LocationWithHidingPlace kidsRoom = landing.AddExit(Direction.Southeast, "Kids Room", "in the bunk beds");
            LocationWithHidingPlace masterBedroom = landing.AddExit(Direction.Northwest, "Master Bedroom", "under the bed");
            LocationWithHidingPlace nursery = landing.AddExit(Direction.Southwest, "Nursery", "behind the changing table");
            LocationWithHidingPlace pantry = landing.AddExit(Direction.South, "Pantry", "inside a cabinet");
            LocationWithHidingPlace secondBathroom = landing.AddExit(Direction.West, "Second Bathroom", "in the shower");

            // Connect Master Bodroom to new location: Master Bath
            LocationWithHidingPlace masterBath = masterBedroom.AddExit(Direction.East, "Master Bath", "in the tub");

            // Add Locations to Locations list
            Locations = new List<Location>()
            {
                attic,
                hallway,
                bathroom,
                kidsRoom,
                masterBedroom,
                nursery,
                pantry,
                secondBathroom,
                kitchen,
                hallway,
                masterBath,
                garage,
                landing,
                livingRoom,
                Entry
            };
        }

        /// <summary>
        /// Return whether Location exists
        /// </summary>
        /// <param name="name">Name of Location</param>
        /// <returns>True if Location exists</returns>
        public static bool DoesLocationExist(string name)
        {
            return Locations.Select((x) => x.Name).Contains(name);
        }

        /// <summary>
        /// Get Location object by its Name property
        /// </summary>
        /// <param name="name">Name of Location</param>
        /// <returns>Location with specified name or Entry if no match found</returns>
        public static Location GetLocationByName(string name)
        {
            return Locations.Where(l => l.Name == name).FirstOrDefault(Entry);
        }

        /// <summary>
        /// Get random exit from specified location
        /// </summary>
        /// <param name="location">Location from which to find random exit</param>
        /// <returns>Random exit Location</returns>
        public static Location RandomExit(Location location)
        {
            IDictionary<Direction, Location> exitList = location.Exits.OrderBy(x => x.Key).ToDictionary(); // Get collection of all exits from Location
            return exitList.ElementAt(Random.Next(exitList.Count)).Value; // Return random Location from exits collection
        }

        /// <summary>
        /// Clear each LocationWithHidingPlace of opponents.
        /// </summary>
        public static void ClearHidingPlaces()
        {
            // For each LocationWithHidingPlace
            foreach(LocationWithHidingPlace location in Locations.Where((l) => l.GetType() == typeof(LocationWithHidingPlace)))
            {
                location.CheckHidingPlace(); // Check and clear hiding place
            }
        }
    }
}
