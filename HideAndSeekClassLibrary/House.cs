using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
     * -I renamed Entry to StartingPoint since the starting point may not always be the entry.
     * -I added a method to get a LocationWithHidingPlace by name.
     * -I renamed a method to GetRandomExit to follow project method naming convention.
     * -I used ElementAt instead of Skip in GetRandomExit.
     * -I added a method to get a random hiding place using logic from 
     *  Andrew Stellman and Jennifer Greene's HideAndSeek project's Opponent class's Hide method.
     * -I added methods to hide all opponents to ensure hiding places are cleared before
     *  opponents are rehidden.
     * -I selected LocationWithHidingPlace objects in foreach header instead of using if statement in body
     *  in ClearHidingPlaces method.
     * -I made the class non-static so GameController can have an instance of House.
     * -I added a static method to create a House object from a file.
     * -I added a static public property for the file system used in CreateHouse for testing purposes.
     * -I added a Name property so a House object can have a name.
     * -I added a name of file property to store the name of the file from which this House was loaded 
     *  (necessary for saving game).
     * -I added a player starting point property for JSON deserialization.
     * -I added a property storing locations without hiding places for JSON deserialization.
     * -I added a property storing locations with hiding places for JSON deserialization.
     * -I added a parameterless constructor for JSON deserialization.
     * -I added a parameterized constructor for creating a new House with specific property values.
     * -I added a method to set up House after deserialization.
     * -I added a method to set StartingPoint and Locations properties.
     *  (called from parameterized constructor and method to set up House after deserialization).
     * -I added a method to tell whether a Location exists to make restoring a saved game easier.
     * -I added a method to tell whether a LocationWithHidingPlace exists to make restoring a saved game easier.
     * -I used a shorter code approach in GetLocationByName.
     * -I added/edited comments for easier reading.
     * **/

    public class House {
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
        /// <exception cref="ArgumentException">Exception thrown if House file name is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if House file not found</exception>
        /// <exception cref="JsonException">Exception thrown if House file data is corrupt</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if House file data is corrupt</exception>
        public static House CreateHouse(string fileName)
        {
            // Create variable to store deserialized House
            House house;

            // Get full file name including extension
            string fullFileName = FileSystem.GetFullFileNameForJson(fileName);

            // If file does not exist
            if ( !(FileSystem.File.Exists(fullFileName)) )
            {
                throw new FileNotFoundException($"Cannot load game because house layout file {fileName} does not exist"); // Throw exception
            }

            // Get text from House file
            string houseFileText = FileSystem.File.ReadAllText(fullFileName);

            // Deserialize text from file into House object, set house private variable to it, and set up House
            try
            {
                house = JsonSerializer.Deserialize<House>(houseFileText); // Deserialize House
                house.SetUpHouseAfterDeserialization(); // Set up House after deserialization
            }
            catch (JsonException e)
            {
                throw new JsonException($"Cannot process because data in house layout file {fileName} is corrupt - {e.Message}");
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Cannot process because data in house layout file {fileName} is corrupt - {e.Message}");
            }
            catch (InvalidDataException e)
            {
                throw new InvalidDataException($"Cannot process because data in house layout file {fileName} is invalid - {e.Message}");
            }
            catch (Exception)
            {
                throw; // Bubble up exception
            }

            // Return House object
            return house; 
        }

        private string _name;

        /// <summary>
        /// Name of House
        /// </summary>
        [JsonRequired]
        public required string Name
        {
            get
            {
                return _name;
            }
            set
            {
                // If invalid name is entered
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidDataException($"Cannot perform action because house name \"{value}\" is invalid (is empty or contains only whitespace)"); // Throw exception
                }

                // Set backing field
                _name = value;
            }
        }

        private string _houseFileName;

        /// <summary>
        /// Name of file from which House is loaded (not including JSON extension)
        /// </summary>
        [JsonRequired]
        public required string HouseFileName
        {
            get
            {
                return _houseFileName;
            }
            set
            {
                // If file name is invalid
                if (!(FileSystem.IsValidName(value)))
                {
                    throw new InvalidDataException($"Cannot perform action because house file name \"{value}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"); // Throw exception
                }

                // Set backing field
                _houseFileName = value;
            }
        }

        private string _playerStartingPoint;

        /// <summary>
        /// Player starting point in House as string
        /// </summary>
        [JsonRequired]
        public required string PlayerStartingPoint
        {
            get
            {
                return _playerStartingPoint;
            }
            set
            {
                // If invalid Location name is entered
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidDataException($"Cannot perform action because player starting point location name \"{value}\" is invalid (is empty or contains only whitespace)"); // Throw exception
                }

                // Set backing field
                _playerStartingPoint = value;
            }
        }

        private Location _startingPoint;

        /// <summary>
        /// Player starting point in House
        /// </summary>
        [JsonIgnore]
        public Location StartingPoint
        {
            get
            {
                return _startingPoint;
            }
            set
            {
                // If Location does not exist in House
                if ( !(DoesLocationExist(value.Name)) )
                {
                    throw new InvalidDataException($"Cannot perform action because player starting point location \"{value}\" is not a location in the house"); // Throw exception
                }

                // Set backing field and property for JSON serialization
                _startingPoint = value; // Set starting point private variable
                PlayerStartingPoint = StartingPoint.Name; // Set player starting point property (for JSON serialization)
            }
        }

        /// <summary>
        /// List of all Locations without hiding places in House
        /// </summary>
        [JsonRequired]
        public IEnumerable<Location> LocationsWithoutHidingPlaces { get; set; }

        private IEnumerable<LocationWithHidingPlace> _locationsWithHidingPlaces;

        /// <summary>
        /// List of all LocationWithHidingPlace objects in House
        /// </summary>
        [JsonRequired]
        public IEnumerable<LocationWithHidingPlace> LocationsWithHidingPlaces
        {
            get
            {
                return _locationsWithHidingPlaces;
            }
            set
            {
                // If enumerable is empty
                if(value.Count() == 0)
                {
                    throw new InvalidDataException("Cannot perform action because locations with hiding places list is empty"); // Throw exception
                }

                // Set backing field
                _locationsWithHidingPlaces = value;
            }
        }

        private IEnumerable<Location> _locations;

        /// <summary>
        /// List of all Locations in House
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Location> Locations
        {
            get
            {
                return _locations;
            }
            set
            {
                // If enumerable is empty
                if(value.Count() == 0)
                {
                    throw new InvalidDataException("Cannot perform action because locations list is empty"); // Throw exception
                }

                // Set backing field
                _locations = value;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// Random number generator (used for returning random hiding place)
        /// </summary>
        public static Random Random { get; set; } = new Random();

        /// <summary>
        /// Parameterless constructor for JSON deserialization
        /// </summary>
        public House() { }

        /// <summary>
        /// Constructor to create a new House object and initialize required properties
        /// </summary>
        /// <param name="name">Name of House</param>
        /// <param name="houseFileName">Name of file in which House layout is stored</param>
        /// <param name="playerStartingPoint">Name of Location where player should start a new game</param>
        /// <param name="locationsWithoutHidingPlaces">Enumerable of Location objects (without hiding places)</param>
        /// <param name="locationsWithHidingPlaces">Enumerable of LocationWithHidingPlace objects</param>
        [SetsRequiredMembers]
        public House(string name, string houseFileName, string playerStartingPoint, 
                     IEnumerable<Location> locationsWithoutHidingPlaces, 
                     IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces)
        {
            // Set all properties except Locations and StartingPoint
            Name = name;
            HouseFileName = houseFileName;
            PlayerStartingPoint = playerStartingPoint;
            LocationsWithoutHidingPlaces = locationsWithoutHidingPlaces;
            LocationsWithHidingPlaces = locationsWithHidingPlaces;

            // Set Locations and StartingPoint properties
            SetLocationsAndStartingPoint();
        }

        /// <summary>
        /// Helper method to set Locations and StartingPoint properties
        /// after LocationsWithoutHidingPlaces and LocationsWithHidingPlaces are set
        /// </summary>
        private void SetLocationsAndStartingPoint()
        {
            // Set list of all Locations in House
            Locations = LocationsWithHidingPlaces.Concat(LocationsWithoutHidingPlaces).ToList();

            // Attempt to get player starting point location
            Location startingPoint = GetLocationByName(PlayerStartingPoint);

            // If starting point is not in House
            if(startingPoint == null)
            {
                throw new InvalidDataException($"Cannot perform action because player starting point location \"{PlayerStartingPoint}\" is not a location in the house"); // Throw exception
            }

            // Set StartingPoint
            StartingPoint = startingPoint;
        }

        /// <summary>
        /// Set up House after deserialization
        /// (set Locations, StartingPoint, and Exits property for each Location in House)
        /// </summary>
        /// <exception cref="InvalidDataException">Exception thrown if exit Location does not exist</exception>
        private void SetUpHouseAfterDeserialization()
        {
            // Set Locations and StartingPoint properties
            SetLocationsAndStartingPoint();

            // Set Exit list for each Location
            foreach (Location location in Locations)
            {
                // Initialize empty Dictionary to store exits
                IDictionary<Direction, Location> exitsDictionary = new Dictionary<Direction, Location>();

                // For each exit
                foreach (KeyValuePair<Direction, string> exit in location.ExitsForSerialization)
                {
                    // Get exit Location object
                    Location exitLocation = GetLocationByName(exit.Value);
                    
                    // If exit Location does not exist
                    if(exitLocation == null)
                    {
                        throw new InvalidDataException(
                            $"Cannot perform action because \"{location.Name}\" exit location \"{exit.Value}\" " +
                            $"in direction \"{exit.Key}\" does not exist"); // Throw exception
                    }

                    // Add exit location to Dictionary
                    exitsDictionary.Add(exit.Key, exitLocation);
                }

                // Set Location's Exits Dictionary
                location.SetExitsDictionary(exitsDictionary);
            }
        }

        /// <summary>
        /// Serialize House and get text
        /// (preps each Location in House appropriately)
        /// </summary>
        /// <returns>Text for serialized House</returns>
        public string Serialize()
        {
            // For each Location in House
            foreach (Location location in Locations)
            {
                location.Serialize(); // Prep for serialization
            }

            // Return serialized House
            return JsonSerializer.Serialize(this);
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
            return LocationsWithHidingPlaces.Where((x) => x.Name == name).FirstOrDefault();
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
        /// Clear all LocationWithHidingPlace objects of Opponents
        /// </summary>
        public void ClearHidingPlaces()
        {
            // For each LocationWithHidingPlace
            foreach(LocationWithHidingPlace location in LocationsWithHidingPlaces)
            {
                location.CheckHidingPlace(); // Check hiding place which clears hiding place
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
            Location opponentCurrentLocation = StartingPoint;

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