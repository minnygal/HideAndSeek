using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a house with Locations through which the user can navigate
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's House class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/House.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I made the class non-static so GameController can have an instance of House.
     * -I added a static method to create a House object from a file.
     * -I added a static public property for the file system used in CreateHouse for testing purposes.
     * -I added a Name property so a House object can have a name.
     * -I added a name of file property to store the name of the file from which this House was loaded 
     *  (necessary for saving game).
     * -In the constructor, I created and added exits to Locations in the same area of code
     *  and in a different order for easier comprehension (just my approach).
     * -I added a method to tell whether a Location exists to make restoring a saved game easier.
     * -I added a method to tell whether a LocationWithHidingPlace exists to make restoring a saved game easier.
     * -I used a shorter code approach in GetLocationByName.
     * -I added a method to get a LocationWithHidingPlace by name.
     * -I renamed a method to GetRandomExit to follow project method naming convention.
     * -I used ElementAt instead of Skip in GetRandomExit.
     * -I added a method to get a random hiding place using logic from 
     *  Andrew Stellman and Jennifer Greene's HideAndSeek project's Opponent class's Hide method.
     * -I added methods to hide all opponents to ensure hiding places are cleared before
     *  opponents are rehidden.
     * -I selected LocationWithHidingPlace objects in foreach header instead of using if statement in body
     *  in ClearHidingPlaces method.
     * -I added/edited comments for easier reading.
     * **/

    public class House {
        /// <summary>
        /// File name for default House layout (not including .json extension)
        /// </summary>
        public static string DefaultHouseFileName { get { return "DefaultHouse"; } }

        /// <summary>
        /// File system to use when creating a new House from a House file
        /// (should only be changed for testing purposes)
        /// </summary>
        public static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// Create a House object from a file
        /// Should only be called from GameController and SavedGame, and tests
        /// </summary>
        /// <param name="fileName">Name of House file (not including .json extension)</param>
        /// <returns>House object created from file</returns>
        /// <exception cref="FileNotFoundException">Exception thrown if House file not found</exception>
        /// <exception cref="JsonException">Exception thrown if House file data is corrupt</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if House file data is corrupt</exception>
        public static House CreateHouse(string fileName)
        {
            // Get full file name including extension
            string fullFileName = FileSystem.GetFullFileNameForJson(fileName);

            // If file does not exist
            if (!(FileSystem.File.Exists(fullFileName)))
            {
                throw new FileNotFoundException($"Cannot load game because house layout file {fileName} does not exist"); // Return error message
            }

            // Get text from House file
            string houseFileText = FileSystem.File.ReadAllText(fullFileName);

            // Deserialize text from file into House object and set house private variable to it
            try
            {
                return JsonSerializer.Deserialize<House>(houseFileText);
            }
            catch (JsonException e)
            {
                throw new JsonException($"Cannot process because data in house layout file {fileName} is corrupt");
            }
            catch (InvalidOperationException e)
            {
                throw new JsonException($"Cannot process because data in house layout file {fileName} is corrupt");
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Name of House
        /// </summary>
        public string Name { get; set; } = "my house";

        /// <summary>
        /// Name of file from which House is loaded (w/o JSON extension)
        /// </summary>
        public string HouseFileName { get; set; } = "DefaultHouse";

        /// <summary>
        /// Entry of House
        /// </summary>
        public Location Entry { get; private set; }

        /// <summary>
        /// List of all Locations in House
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        /// Random number generator (used for returning random hiding place)
        /// </summary>
        public Random Random { get; set; } = new Random();

        /// <summary>
        /// Parameterless constructor for JSON deserializer
        /// Should only be called directly from HouseHelper unless default house is desired
        /// </summary>
        public House() 
        {
            // Create Entry and connect to new locations: Garage, Hallway
            Entry = new Location("Entry");
            LocationWithHidingPlace garage = Entry.AddExit(Direction.Out, "Garage", "behind the car");
            Location hallway = Entry.AddExit(Direction.East, "Hallway");

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
        public bool DoesLocationExist(string name)
        {
            return GetLocationByName(name) != null;
        }

        /// <summary>
        /// Return whether LocationWithHidingPlace exists
        /// </summary>
        /// <param name="name">Name of LocationWithHidingPlace</param>
        /// <returns>True if LocationWithHidingPlace exists</returns>
        public bool DoesLocationWithHidingPlaceExist(string name)
        {

            return GetLocationWithHidingPlaceByName(name) != null;
        }

        /// <summary>
        /// Get Location object by its Name property
        /// </summary>
        /// <param name="name">Name of Location</param>
        /// <returns>Location with specified name (or null if not found)</returns>
        public Location GetLocationByName(string name)
        {
            return Locations.Where(l => l.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Get LocationWithHidingPlace object by its Name property
        /// </summary>
        /// <param name="name">Name of LocationWithHidingPlace</param>
        /// <returns>LocationWithHidingPlace with specified name (or null if not found)</returns>
        public LocationWithHidingPlace GetLocationWithHidingPlaceByName(string name)
        {
            return (LocationWithHidingPlace) Locations.Where((x) => x.GetType() == typeof(LocationWithHidingPlace) && x.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Get random exit from specified location
        /// </summary>
        /// <param name="location">Location from which to find random exit</param>
        /// <returns>Location to which random exit leads</returns>
        public Location GetRandomExit(Location location)
        {
            IDictionary<Direction, Location> exitList = location.Exits.OrderBy(x => x.Key).ToDictionary(); // Get collection of all exits from Location ordered by name
            return exitList.ElementAt(Random.Next(exitList.Count)).Value; // Return random Location from exits collection
        }

        /// <summary>
        /// Clear all LocationWithHidingPlaces of Opponents.
        /// </summary>
        public void ClearHidingPlaces()
        {
            // For each LocationWithHidingPlace
            foreach(LocationWithHidingPlace location in Locations.Where((l) => l.GetType() == typeof(LocationWithHidingPlace)))
            {
                location.CheckHidingPlace(); // Check hiding place which clears hiding place as well
            }
        }

        /// <summary>
        /// Get a random location with hiding place
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
        /// -I removed House class specifiers before method calls (unnecessary in House class).
        /// -I added comments for easier reading.
        /// </summary>
        /// <returns>Random location with hiding place</returns>
        public LocationWithHidingPlace GetRandomLocationWithHidingPlace()
        {
            // Current Location of Opponent moving through House to find hiding place
            Location opponentCurrentLocation = Entry;

            // Minimum number of moves Opponent must make through House to find a LocationWithHidingPlace
            int minNumberOfMoves = Random.Next(10, 51);

            // Move through Locations in House via random exits between 10 and 50 times (inclusive)
            for (int currentMove = 0; currentMove < minNumberOfMoves; currentMove++)
            {
                opponentCurrentLocation = GetRandomExit(opponentCurrentLocation);
            }

            // Keep moving until Opponent is in LocationWithHidingPlace
            while (opponentCurrentLocation.GetType() != typeof(LocationWithHidingPlace))
            {
                opponentCurrentLocation = GetRandomExit(opponentCurrentLocation);
            }

            // Return LocationWithHidingPlace
            return (LocationWithHidingPlace)opponentCurrentLocation;
        }
    }
}