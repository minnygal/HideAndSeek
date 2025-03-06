using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Static class to represent a House with Locations through which the user can navigate
    /// 
    /// CREDIT: adapted from HideAndSeek project's House class
    ///         © 2023 Andrew Stellman and Jennifer Greene
    ///         Published under the MIT License
    ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/House.cs
    ///         Link valid as of 02-25-2025
    ///         
    /// CHANGES:
    /// -In the constructor, I created and added exits to Locations in the same area of code
    ///  and in a different order for easier comprehension (just my approach).
    /// -I added a method to tell whether a Location exists to make restoring a saved game easier.
    /// -I added a method to tell whether a LocationWithHidingPlace exists to make restoring a saved game easier.
    /// -I used a shorter code approach in GetLocationByName.
    /// -I renamed a method to GetRandomExit to follow project method naming convention.
    /// -I used ElementAt instead of Skip in GetRandomExit.
    /// -I added a method to get a random hiding place using logic from 
    ///  Andrew Stellman and Jennifer Greene's HideAndSeek project's Opponent class's Hide method.
    /// -I added methods to hide all opponents to ensure hiding places are cleared before
    ///  opponents are rehidden.
    /// -I selected LocationWithHidingPlace objects in foreach header instead of using if statement in body
    ///  in ClearHidingPlaces method.
    /// -I added/edited comments for easier reading.
    /// </summary>
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
        /// Return whether LocationWithHidingPlace exists
        /// </summary>
        /// <param name="name">Name of LocationWithHidingPlace</param>
        /// <returns>True if LocationWithHidingPlace exists</returns>
        public static bool DoesLocationWithHidingPlaceExist(string name)
        {
            return Locations.Where((x) => x.GetType() == typeof(LocationWithHidingPlace)).Select((x) => x.Name).Contains(name);
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
        public static Location GetRandomExit(Location location)
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

        /// <summary>
        /// Get a random hiding place
        /// 
        /// CREDIT: adapted from HideAndSeek project's Opponent class's Hide method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Opponent.cs
        ///         Link valid as of 02-25-2025
        ///         
        /// CHANGES:
        /// -I renamed variables and made them type specific for easier comprehension.
        /// -I used GetType and typeof instead of "is" in while loop (just my approach).
        /// -I removed the diagnostic debug line of code because it's unnecessary right now.
        /// -I added comments for easier reading.
        /// </summary>
        /// <returns>Random location with hiding place</returns>
        public static LocationWithHidingPlace GetRandomHidingPlace()
        {
            // Current location of opponent moving through House to find hiding place
            Location currentLocation = House.Entry;

            // Minimum number of moves opponent makes through the house to find a hiding spot
            int minNumberOfMoves = House.Random.Next(10, 51);

            // Move through house via random exits between 10 and 50 times (inclusive)
            for (int currentMove = 0; currentMove < minNumberOfMoves; currentMove++)
            {
                currentLocation = House.GetRandomExit(currentLocation);
            }

            // Keep moving until a location with hiding place is found
            while (currentLocation.GetType() != typeof(LocationWithHidingPlace))
            {
                currentLocation = House.GetRandomExit(currentLocation);
            }

            // Return hiding place
            return currentLocation as LocationWithHidingPlace;
        }
    }
}
