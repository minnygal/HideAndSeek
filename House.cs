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
            Location garage = Entry.AddExit(Direction.Out, "Garage");
            Location hallway = Entry.AddExit(Direction.East, "Hallway");

            // Connect Hallway to new locations: Kitchen, Bathroom, Living Room, Landing
            Location kitchen = hallway.AddExit(Direction.Northwest, "Kitchen");
            Location bathroom = hallway.AddExit(Direction.North, "Bathroom");
            Location livingRoom = hallway.AddExit(Direction.South, "Living Room");

            // Create landing
            Location landing = new Location("Landing");

            // Connect Landing to new locations: Attic, Hallway, Kids Room, Master Bedroom, Nursery, Pantry, Second Bathroom
            Location attic = landing.AddExit(Direction.Up, "Attic");
            landing.AddExit(Direction.Down, hallway);
            Location kidsRoom = landing.AddExit(Direction.Southeast, "Kids Room");
            Location masterBedroom = landing.AddExit(Direction.Northwest, "Master Bedroom");
            Location nursery = landing.AddExit(Direction.Southwest, "Nursery");
            Location pantry = landing.AddExit(Direction.South, "Pantry");
            Location secondBathroom = landing.AddExit(Direction.West, "Second Bathroom");
            
            // Connect Master Bodroom to new location: Master Bath
            Location masterBath = masterBedroom.AddExit(Direction.East, "Master Bath");

            // Add Locations to Locations list
            Locations = new List<Location>()
            {
                attic,
                hallway,
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
            IDictionary<Direction, Location> exitList = location.Exits; // Get collection of all exits from Location
            return exitList.ElementAt(Random.Next(exitList.Count)).Value; // Return random Location from exits collection
        }
    }
}
