using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
     * -I moved the random exit method to the Location class.
     * -I added a method to get a random hiding place.
     * -I added methods to hide all opponents to ensure hiding places are cleared before
     *  opponents are rehidden.
     * -I selected LocationWithHidingPlace objects in foreach header instead of using if statement in body
     *  in ClearHidingPlaces method.
     * -I made the class non-static so GameController can have an instance of House
     *  and so there can be different Houses with different layouts.
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
     * -I added a method to tell whether a Location exists in this House for data validation.
     * -I added a method to tell whether a LocationWithHidingPlace exists in this House for data validation.
     * -I used a shorter code approach in GetLocationByName.
     * -I added a static property to store the ending for a House file name.
     * -I added a static method to get the full House file name based on input.
     * -I added a static method to get the names of House files stored in specific directory.
     * -I added/edited comments for easier reading.
     * **/

    public class House {
        /// <summary>
        /// Parameterless constructor for JSON deserialization
        /// </summary>
        public House() { }

        /// <summary>
        /// Constructor to create a new House object and initialize required properties
        /// </summary>
        /// <param name="name">Name of House</param>
        /// <param name="houseFileName">Name of file in which House layout is stored (not including House file ending or JSON extension</param>
        /// <param name="startingPoint">Location from which player should start a new game</param>
        /// <param name="locationsWithoutHidingPlaces">Enumerable of Location objects without hiding places</param>
        /// <param name="locationsWithHidingPlaces">Enumerable of LocationWithHidingPlace objects</param>
        [SetsRequiredMembers]
        public House(string name, string houseFileName, Location startingPoint,
                     IEnumerable<Location> locationsWithoutHidingPlaces,
                     IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces)
        {
            // Set properties which don't require other properties to be set
            Name = name;
            HouseFileName = houseFileName;
            LocationsWithoutHidingPlaces = locationsWithoutHidingPlaces;
            LocationsWithHidingPlaces = locationsWithHidingPlaces;

            // Set StartingPoint (requires Location properties to be set)
            StartingPoint = startingPoint;
        }

        #region Properties and property setting methods

        private string _name;

        /// <summary>
        /// Name of House
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty or only whitespace)</exception>
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
                    throw new ArgumentException($"house name \"{value}\" is invalid (is empty or contains only whitespace)", nameof(value)); // Throw exception
                }

                // Set backing field
                _name = value;
            }
        }

        private string _houseFileName;

        /// <summary>
        /// Name of file from which House is loaded (not including House file ending or JSON extension)
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty, illegal characters, whitespace)</exception>
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
                if( !(FileSystem.IsValidName(value)) )
                {
                    throw new ArgumentException($"house file name \"{value}\" is invalid " +
                                                 "(is empty or contains illegal characters, e.g. \\, /, or whitespace)", nameof(value)); // Throw exception
                }

                // Set backing field
                _houseFileName = value;
            }
        }

        private string _playerStartingPoint;

        /// <summary>
        /// Player starting point in House as string
        /// for serialization/deserialization use only
        /// DO NOT SET MANUALLY
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty of contains whitespace)</exception>
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
                    throw new ArgumentException($"player starting point location name \"{value}\" is invalid " +
                                                 "(is empty or contains only whitespace)", nameof(value)); // Throw exception
                }

                // Set backing field
                _playerStartingPoint = value;
            }
        }

        private Location _startingPoint;

        /// <summary>
        /// Player starting point in House
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown if value passed to setter is not in House</exception>
        [JsonIgnore]
        public Location StartingPoint
        {
            get
            {
                return _startingPoint;
            }
            set
            {
                // If Location is null or does not exist in House
                if ( value == null || !(DoesLocationExist(value.Name)) )
                {
                    throw new InvalidOperationException($"starting point location \"{value}\" does not exist in House"); // Throw exception
                }

                // Set backing field and property for JSON serialization
                _startingPoint = value; // Set property backing field
                PlayerStartingPoint = StartingPoint.Name; // Set player starting point property (for JSON serialization)
            }
        }

        private IEnumerable<Location> _locationsWithoutHidingPlaces;

        /// <summary>
        /// List of all Locations without hiding places in House
        /// Setter does NOT check that StartingPoint is still in House, so
        /// DO NOT MANUALLY CALL SETTER - call SetLocationsWithoutHidingPlaces method instead
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if LocationWithHidingPlace passed in to setter</exception>
        [JsonRequired]
        public IEnumerable<Location> LocationsWithoutHidingPlaces
        {
            get
            {
                return _locationsWithoutHidingPlaces;
            }
            set
            {
                // If any object passed in is a LocationWithHidingPlace
                foreach(Location location in value)
                {
                    if(location.GetType() == typeof(LocationWithHidingPlace))
                    {
                        throw new ArgumentException($"LocationWithHidingPlace \"{location.Name}\" passed to LocationsWithoutHidingPlaces setter", nameof(value)); // Throw exception
                    }
                }

                // Set backing field
                _locationsWithoutHidingPlaces = value;
            }
        }

        /// <summary>
        /// Set LocationsWithoutHidingPlaces property,
        /// checking that StartingPoint is still in the House
        /// </summary>
        /// <param name="locations">New value for LocationsWithoutHidingPlaces property</param>
        /// <exception cref="InvalidOperationException">Exception thrown if StartingPoint is not in locations passed in or LocationsWithHidingPlaces</exception>
        /// <exception cref="ArgumentException">Exception thrown if LocationWithHidingPlace passed in</exception>
        public void SetLocationsWithoutHidingPlaces(IEnumerable<Location> locations)
        {
            // If StartingPoint is not in locations passed in or LocationsWithHidingPlaces
            if( !(locations.Concat(LocationsWithHidingPlaces).Contains(StartingPoint)) )
            {
                throw new InvalidOperationException("StartingPoint is not in LocationsWithHidingPlaces or the locations passed in"); // Throw exception
            }

            // Set property (any exception thrown bubbles up)
            LocationsWithoutHidingPlaces = locations;
        }

        private IEnumerable<LocationWithHidingPlace> _locationsWithHidingPlaces;

        /// <summary>
        /// List of all LocationWithHidingPlace objects in House
        /// Setter does NOT check that StartingPoint is still in House, so
        /// DO NOT MANUALLY CALL SETTER - call SetLocationsWithHidingPlaces method instead
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is empty list</exception>
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
                if( !(value.Any()) )
                {
                    throw new ArgumentException("locations with hiding places list is empty", nameof(value)); // Throw exception
                }

                // Set backing field
                _locationsWithHidingPlaces = value;
            }
        }

        /// <summary>
        /// Set LocationsWithHidingPlaces property,
        /// checking that StartingPoint is still in the House
        /// </summary>
        /// <param name="locationsWithHidingPlaces">New value for LocationsWithHidingPlaces property</param>
        /// <exception cref="InvalidOperationException">Exception thrown if StartingPoint is not in locations passed in or LocationsWithoutHidingPlaces</exception>
        /// <exception cref="ArgumentException">Exception thrown if value passed in is empty list</exception>
        public void SetLocationsWithHidingPlaces(IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces)
        {
            // If StartingPoint is not in locations passed in or LocationsWithoutHidingPlaces
            if (!(locationsWithHidingPlaces.Concat(LocationsWithoutHidingPlaces).Contains(StartingPoint)))
            {
                throw new InvalidOperationException("StartingPoint is not in LocationsWithoutHidingPlaces or the locations passed in"); // Throw exception
            }

            // Set property (any exception thrown bubbles up)
            LocationsWithHidingPlaces = locationsWithHidingPlaces;
        }

        /// <summary>
        /// List of all Locations in House
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Location> Locations => LocationsWithoutHidingPlaces.Concat(LocationsWithHidingPlaces);

        #endregion

        #region Instance methods

        /// <summary>
        /// Clear all LocationWithHidingPlace objects of Opponents
        /// </summary>
        public void ClearHidingPlaces()
        {
            // For each LocationWithHidingPlace
            foreach (LocationWithHidingPlace location in LocationsWithHidingPlaces)
            {
                location.CheckHidingPlace(); // Check hiding place which clears hiding place
            }
        }

        /// <summary>
        /// Get a random location with hiding place
        /// <returns>Random location with hiding place</returns>
        public LocationWithHidingPlace GetRandomLocationWithHidingPlace()
        {
            return LocationsWithHidingPlaces.ElementAt(Random.Next(LocationsWithHidingPlaces.Count()));
        }

        /// <summary>
        /// Set up House after deserialization
        /// (sets StartingPoint property, and sets Exits property for each Location in House)
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown if a Location does not exist in House</exception>
        private void SetUpHouseAfterDeserialization()
        {
            // Set StartingPoint property (requeris Location properties to be set)
            Location startingPoint; // Declare variable to store starting point as Location
            try
            {
                // Attempt to get player starting point location as object
                startingPoint = GetLocationByName(PlayerStartingPoint);

                // Set StartingPoint
                StartingPoint = startingPoint;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"player starting point location \"{PlayerStartingPoint}\" does not exist in House"); // Throw exception
            }
            catch(Exception)
            {
                throw; // Bubble up exception
            }

            // Set Exit list for each Location
            foreach (Location location in Locations)
            {
                // Initialize empty Dictionary to store exits
                Dictionary<Direction, Location> exitsDictionary = new Dictionary<Direction, Location>();

                // Declare variable to store exit location
                Location exitLocation;

                // For each exit
                foreach (KeyValuePair<Direction, string> exit in location.ExitsForSerialization)
                {
                    try
                    {
                        // Get exit Location object
                        exitLocation = GetLocationByName(exit.Value);
                    }
                    catch(InvalidOperationException e)
                    {
                        throw new InvalidOperationException(
                            $"\"{location.Name}\" exit location \"{exit.Value}\" in direction \"{exit.Key}\" does not exist"); // Throw exception
                    }

                    // Add exit location to Dictionary
                    exitsDictionary.Add(exit.Key, exitLocation);
                }

                // Set Location's Exits Dictionary
                location.Exits = exitsDictionary;
            }
        }

        /// <summary>
        /// Serialize House and get text of serialized House
        /// (preps each Location in House appropriately)
        /// </summary>
        /// <returns>Text of serialized House</returns>
        public string Serialize()
        {
            // For each Location in House
            foreach (Location location in Locations)
            {
                location.PrepForSerialization(); // Prep for serialization
            }

            // Return serialized House
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Return whether Location exists
        /// </summary>
        /// <param name="name">Name of Location</param>
        /// <returns>True if Location exists</returns>
        public virtual bool DoesLocationExist(string name)
        {
            return Locations.Select((l) => l.Name).ToList().Contains(name);
        }

        /// <summary>
        /// Return whether LocationWithHidingPlace exists
        /// </summary>
        /// <param name="name">Name of LocationWithHidingPlace</param>
        /// <returns>True if LocationWithHidingPlace exists</returns>
        public virtual bool DoesLocationWithHidingPlaceExist(string name)
        {
            return LocationsWithHidingPlaces.Select((l) => l.Name).ToList().Contains(name);
        }

        /// <summary>
        /// Get Location object by its Name property
        /// </summary>
        /// <param name="name">Name of Location</param>
        /// <returns>Location with specified name</returns>
        /// <exception cref="InvalidOperationException">Exception thrown if no matching location in House</exception>
        public Location GetLocationByName(string name)
        {
            try
            {
                return Locations.Where(l => l.Name == name).First();
            }
            catch(InvalidOperationException e)
            {
                throw new InvalidOperationException($"location \"{name}\" does not exist in House"); // Throw exception
            }
        }

        /// <summary>
        /// Get LocationWithHidingPlace object by its Name property
        /// </summary>
        /// <param name="name">Name of LocationWithHidingPlace</param>
        /// <returns>LocationWithHidingPlace with specified name</returns>
        /// <exception cref="InvalidOperationException">Exception thrown if no matching location with hiding place in House</exception>
        public LocationWithHidingPlace GetLocationWithHidingPlaceByName(string name)
        {
            try
            {
                return LocationsWithHidingPlaces.Where(l => l.Name == name).First();
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"location with hiding place \"{name}\" does not exist in House"); // Throw exception
            }
        }

        #endregion

        #region Static properties and methods

        /// <summary>
        /// Create a House object from a file
        /// Should only be called from GameController and SavedGame, and tests
        /// </summary>
        /// <param name="houseFileNameWithoutEnding">Name of House file without House file ending or JSON extension</param>
        /// <returns>House object created from file</returns>
        /// <exception cref="ArgumentException">Exception thrown if House file name or file value is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if House file not found</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if House file data is corrupt</exception>
        public static House CreateHouse(string houseFileNameWithoutEnding)
        {
            // Get full file name including extension
            string fullFileName = GetFullHouseFileName(houseFileNameWithoutEnding);

            // If file does not exist
            if (!(FileSystem.File.Exists(fullFileName)))
            {
                throw new FileNotFoundException($"Cannot load game because house layout file {houseFileNameWithoutEnding} does not exist"); // Throw exception
            }

            // Get text from House file
            string houseFileText = FileSystem.File.ReadAllText(fullFileName);

            // Create variable to store deserialized House object
            House house;

            // Deserialize text from file into House object and store in private variable, then set up House
            try
            {
                house = JsonSerializer.Deserialize<House>(houseFileText); // Deserialize House
                house.SetUpHouseAfterDeserialization(); // Set up House after deserialization
            }
            catch (JsonException e) // If JSON format is corrupt
            {
                throw new JsonException($"Cannot process because data in house layout file {houseFileNameWithoutEnding} is corrupt - {e.Message}");
            }
            catch (InvalidOperationException e) // If invalid operation attempted
            {
                throw new InvalidOperationException($"Cannot process because data in house layout file {houseFileNameWithoutEnding} is corrupt - {e.Message}");
            }
            catch (ArgumentException e) // If argument is invalid
            {
                throw new ArgumentException($"Cannot process because data in house layout file {houseFileNameWithoutEnding} is invalid - {e.Message}", e.ParamName);
            }
            catch (Exception)
            {
                throw; // Bubble up exception
            }

            // Return House object
            return house;
        }

        [JsonIgnore]
        /// <summary>
        /// Random number generator used for getting random hiding place
        /// (should only be changed for testing purposes)
        /// </summary>
        public static Random Random { get; set; } = new Random();

        /// <summary>
        /// File system to use when creating a new House from a House file
        /// (should only be changed for testing purposes)
        /// </summary>
        public static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// Ending text for House layout file
        /// </summary>
        public static string HouseFileEnding
        {
            get
            {
                return ".house";
            }
        }

        /// <summary>
        /// Get full file name for a House layout file (including House file ending and JSON extension)
        /// </summary>
        /// <param name="fileNameWithoutEnding">Name of House file without ending</param>
        /// <returns>Name of House file with ending and JSON extension</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        public static string GetFullHouseFileName(string fileNameWithoutEnding)
        {
            // If file name without ending is invalid
            if (!(FileSystem.IsValidName(fileNameWithoutEnding)))
            {
                throw new ArgumentException($"Cannot perform action because file name \"{fileNameWithoutEnding}\" is invalid " +
                                             "(is empty or contains illegal characters, e.g. \\, /, or whitespace)", nameof(fileNameWithoutEnding)); // Throw new exception with custom error message
            }

            // Return full file name including ending and extension
            return FileSystem.GetFullFileNameForJson(fileNameWithoutEnding + HouseFileEnding);
        }

        /// <summary>
        /// Get names of all House layout files in directory
        /// (returns names without House file ending or JSON extension)
        /// </summary>
        /// <param name="directoryFullName">Full name of directory</param>
        /// <returns>Enumerable of house layout file names (without House file ending or JSON extension)</returns>
        public static IEnumerable<string> GetHouseFileNames(string directoryFullName = null)
        {
            // If directory name has not been set
            if (directoryFullName == null)
            {
                directoryFullName = FileSystem.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // Set to current directory
            }

            // Return names of House layout files (without House file ending or JSON extension) in directory
            return FileSystem.Directory.GetFiles(directoryFullName) // Get files from specified directory
                    .Where((n) => n.EndsWith($"{HouseFileEnding}{FileExtensions.JsonFileExtension}")) // Whose names end with House file ending and JSON file extension
                    .Select((n) => // Remove House file ending and JSON extension from House file names and return modified names
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(n); // Get file name without JSON extension
                        return fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - HouseFileEnding.Length); // Return House file name without House file ending or JSON extension
                    });
        }

        #endregion
    }
}