using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a location in a House
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's Location class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Location.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I added data validation for the Name property.
     * -I removed DescribeDirection method and put the logic in Direction file as Extension class
     *  for separation of concerns.
     * -I used a loop instead of LINQ in the ExitList method (just my approach).
     * -I made AddExit return the connecting Location added to allow daisy-chaining.
     * -I created an AddExit overload to allow a location name to be passed in
     *  to have a new Location created in method (for ease).
     * -I created an AddExit overload to allow a location name and hiding place description
     *  to be passed in to have a new LocationWithHidingPlace created in method (for ease).
     * -I used a different method to find out whether there is an exit 
     *  in the specified Direction in GetExit method (just my approach).
     * -I made the GetExit method body multi-line and replaced ternary operator
     *  for easier reading.
     * -I modified the Direction changing logic in AddReturnExit (just my approach).
     * -I used Direction changing in AddExit (just my approach).
     * -I converted lambdas to regular method bodies for easier modification.
     * -I added a property and method for JSON serialization of exits.
     * -I added a method to prepare the object for JSON serialization.
     * -I added a method to serialize the object and return the text.
     * -I added a method to set Exits property (called from House after JSON deserialization).
     * -I added comments for easier reading.
     * **/

    public class Location
    {
        private string _name;

        /// <summary>
        /// The name of this location
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty or only whitespace)</exception>
        [JsonRequired]
        public virtual required string Name
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
                    throw new ArgumentException($"location name \"{value}\" is invalid (is empty or contains only whitespace)", nameof(value)); // Throw exception
                }

                // Set backing field
                _name = value;
            }
        }

        private IDictionary<Direction, string> _exitsForSerialization;

        /// <summary>
        /// Exits for JSON serialization 
        /// (Locations as strings so duplicate Locations 
        /// are not created when multiple linked Locations are restored)
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter contains invalid value (empty or only whitespace)</exception>
        [JsonRequired]
        public IDictionary<Direction, string> ExitsForSerialization
        {
            get
            {
                return _exitsForSerialization;
            }
            set
            {
                // Check each location name in Dictionary
                foreach(KeyValuePair<Direction, string> kvp in value)
                {
                    // If name is invalid
                    if(string.IsNullOrWhiteSpace(kvp.Value))
                    {
                        throw new ArgumentException($"location name \"{kvp.Value}\" for exit in direction \"{kvp.Key}\" is invalid " +
                                                    "(is empty or contains only whitespace", nameof(value)); // Throw exception
                    }
                }

                // Set Dictionary backing field
                _exitsForSerialization = value;
            }
        }

        /// <summary>
        /// Prepare object for serialization by setting ExitsForSerialization property
        /// </summary>
        private void PrepForSerialization()
        {
            // Create Dictionary of directions and Location names
            IDictionary<Direction, string> exitsForSerialization = new Dictionary<Direction, string>();
            foreach (KeyValuePair<Direction, Location> kvp in Exits)
            {
                exitsForSerialization.Add(kvp.Key, kvp.Value.Name);
            }

            // Set exits for serialization property to Dictionary
            ExitsForSerialization = exitsForSerialization;
        }

        /// <summary>
        /// Serialize object and return as string
        /// Calls private PrepForSerialization method
        /// which must be called prior to object serialization.
        /// </summary>
        /// <returns>Serialized object as string</returns>
        public virtual string Serialize()
        {
            PrepForSerialization();
            return JsonSerializer.Serialize(this);
        }

        [JsonIgnore]
        /// <summary>
        /// The exits out of this location
        /// </summary>
        public IDictionary<Direction, Location> Exits { get; private set; } = new Dictionary<Direction, Location>();
        
        /// <summary>
        /// Set the Exits dictionary
        /// Should only be called by House method
        /// </summary>
        /// <param name="exits">Dictionary of exits</param>
        /// <exception cref="ArgumentException">Exception thrown if exits dictionary is empty</exception>
        public void SetExitsDictionary(IDictionary<Direction, Location> exits)
        {
            // If dictionary is empty
            if(exits.Count() == 0)
            {
                throw new ArgumentException($"location \"{Name}\" must be assigned at least one exit", nameof(exits)); // Throw exception
            }

            // Set Exits property
            Exits = exits;
        }

        /// <summary>
        /// Constructor for JSON deserialization
        /// </summary>
        public Location() { }

        /// <summary>
        /// Constructor setting location name
        /// </summary>
        /// <param name="name">Name of the location</param>
        [SetsRequiredMembers]
        public Location(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        /// <summary>
        /// Get an enumerable of descriptions of the exits, sorted by direction
        /// </summary>
        public IEnumerable<string> ExitList()
        {
            // Initialize a list to store descriptions of exits
            List<string> descriptions = new List<string>();

            // Add description to list for each exit (exits sorted by direction)
            foreach(KeyValuePair<Direction, Location> exit in Exits.OrderBy(x => x.Key))
            {
                descriptions.Add($"the {exit.Value.Name} is {exit.Key.DirectionDescription()}");
            }

            // Return enumerable of descriptions
            return descriptions;
        }

        /// <summary>
        /// Add an exit to this location (and add a reciprocal exit to the other location)
        /// </summary>
        /// <param name="direction">Direction of the connecting location</param>
        /// <param name="connectingLocation">Connecting location to add</param>
        /// <returns>Connecting Location</returns>
        public Location AddExit(Direction direction, Location connectingLocation)
        {
            Exits.Add(direction, connectingLocation); // Add the connecting location as an exit
            connectingLocation.AddReturnExit(direction.OppositeDirection(), this); // Have the connecting location add a reciprocal exit
            return connectingLocation; // Return the connecting location
        }

        /// <summary>
        /// Create a new Location and add it as an exit to the calling Location 
        /// (and add a reciprocal exit to the new Location)
        /// </summary>
        /// <param name="direction">Direction of the new location</param>
        /// <param name="connectingLocation">Name of new location to add and connect</param>
        /// <returns>New Location</returns>
        public Location AddExit(Direction direction, string newLocationName)
        {
            return AddExit(direction, new Location(newLocationName));
        }

        /// <summary>
        /// Create a new LocationWithHidingPlace and add it as an exit to the calling Location 
        /// (and add a reciprocal exit to the new LocationWithHidingPlace)
        /// </summary>
        /// <param name="direction">Direction of the new location</param>
        /// <param name="newLocationName">Name of new location to add and connect</param>
        /// <param name="hidingPlaceDescription">Description of hiding place in new location</param>
        /// <returns>New LocationWithHidingPlace</returns>
        public LocationWithHidingPlace AddExit(Direction direction, string newLocationName, string hidingPlaceDescription)
        {
            return (LocationWithHidingPlace) AddExit(direction, new LocationWithHidingPlace(newLocationName, hidingPlaceDescription));
        }

        /// <summary>
        /// Add a return exit to the specified connecting location
        /// </summary>
        /// <param name="direction">Direction from this location to the connecting location</param>
        /// <param name="connectingLocation">Connecting location linked to this location</param>
        private void AddReturnExit(Direction direction, Location connectingLocation)
        {
            Exits.Add(direction, connectingLocation);
        }

        /// <summary>
        /// Get the exit location in a direction
        /// </summary>
        /// <param name="direction">Direction of exit location</param>
        /// <returns>The exit location</returns>
        /// <exception cref="InvalidOperationException">Exception thrown if no exit in specified direction</exception>
        public Location GetExit(Direction direction) {
            // Get exit in direction specified and store in location variable
            Exits.TryGetValue(direction, out Location location);

            // If no location found in direction
            if(location == null)
            {
                throw new InvalidOperationException($"There is no exit for location \"{Name}\" in direction \"{direction}\""); // Throw exception
            }

            // Return location
            return location;
        }
    }
}