namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a location in the House
    /// 
    /// CREDIT: adapted from HideAndSeek project's Location class
    ///         © 2023 Andrew Stellman and Jennifer Greene
    ///         Published under the MIT License
    ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Location.cs
    ///         Link valid as of 02-25-2025
    ///         
    /// CHANGES:
    /// -I removed DescribeDirection method and put the logic in Direction file as Extension class
    ///  for separation of concerns.
    /// -I used a loop instead of LINQ in the ExitList method (just my approach).
    /// -I made AddExit return the connecting Location added.
    /// -I created an AddExit overload to allow a location name to be passed in
    ///  to have a new Location created in method (for ease).
    /// -I created an AddExit overload to allow a location name and hiding place description
    ///  to be passed in to have a new LocationWithHidingPlace created in method (for ease).
    /// -I used a different method to find out whether there is an exit 
    ///  in the specified Direction in GetExit method (just my approach).
    /// -I made the GetExit method body multi-line and replaced ternary operator
    ///  for easier reading.
    /// -I modified the Direction changing logic in AddReturnExit (just my approach).
    /// -I used Direction changing in AddExit (just my approach).
    /// -I converted lambdas to regular method bodies for easier modification.
    /// -I added comments for easier reading.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// The name of this location
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The exits out of this location
        /// </summary>
        public IDictionary<Direction, Location> Exits { get; private set; } = new Dictionary<Direction, Location>();

        /// <summary>
        /// The constructor sets the location name
        /// </summary>
        /// <param name="name">Name of the location</param>
        public Location(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        /// <summary>
        /// Returns a sequence of descriptions of the exits, sorted by direction
        /// </summary>
        public IEnumerable<string> ExitList()
        {
            // Initialize a sequence to store descriptions
            List<string> descriptions = new List<string>();

            // Add description to sequence for each exit (sorted by direction)
            foreach(KeyValuePair<Direction, Location> exit in Exits.OrderBy(x => x.Key))
            {
                descriptions.Add($"the {exit.Value.Name} is {exit.Key.DirectionDescription()}");
            }

            // Return descriptions sequence
            return descriptions as IEnumerable<string>;
        }

        /// <summary>
        /// Adds an exit to this location (and adds a reciprocal exit to the other location)
        /// </summary>
        /// <param name="direction">Direction of the connecting location</param>
        /// <param name="connectingLocation">Connecting location to add</param>
        /// <returns>Connecting Location</returns>
        public Location AddExit(Direction direction, Location connectingLocation)
        {
            Exits.Add(direction, connectingLocation);
            connectingLocation.AddReturnExit(direction.OppositeDirection(), this);
            return connectingLocation;
        }

        /// <summary>
        /// Creates a new Location and adds it as an exit to the calling Location 
        /// (and adds a reciprocal exit to the new Location)
        /// </summary>
        /// <param name="direction">Direction of the new location</param>
        /// <param name="connectingLocation">Name of new location to add and connect</param>
        /// <returns>New Location</returns>
        public Location AddExit(Direction direction, string newLocationName)
        {
            return AddExit(direction, new Location(newLocationName));
        }

        /// <summary>
        /// Creates a new LocationWithHidingPlace and adds it as an exit to the calling Location 
        /// (and adds a reciprocal exit to the new LocationWithHidingPlace)
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
        /// Adds a return exit to the specified connecting location
        /// </summary>
        /// <param name="direction">Direction from this location to the connecting location</param>
        /// <param name="connectingLocation">Connecting location linked to this location</param>
        private void AddReturnExit(Direction direction, Location connectingLocation)
        {
            Exits.Add(direction, connectingLocation);
        }

        /// <summary>
        /// Gets the exit location in a direction
        /// </summary>
        /// <param name="direction">Direciton of the exit location</param>
        /// <returns>The exit location, or this if there is no exit in that direction</returns>
        public Location GetExit(Direction direction) {
            // Get exit in direction specified and store in location variable
            Exits.TryGetValue(direction, out Location location);

            // If to location found in direction
            if(location == null)
            {
                location = this; // Return calling location
            }

            // Return location
            return location;
        }
    }
}