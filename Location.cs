namespace HideAndSeek
{
    public class Location
    {
        /// <summary>
        /// The name of this location
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The exits out of this location
        /// </summary>
        public IDictionary<Direction, Location> Exits { get; private set; }
        = new Dictionary<Direction, Location>();
        /// <summary>
        /// The constructor sets the location name
        /// </summary>
        /// <param name="name">Name of the location</param>
        public Location(string name) => throw new NotImplementedException();
        public override string ToString() => Name;
        /// <summary>
        /// Returns a sequence of descriptions of the exits, sorted by direction
        /// </summary>
        public IEnumerable<string> ExitList => throw new NotImplementedException();
        /// <summary>
        /// Adds an exit to this location
        /// </summary>
        /// <param name="direction">Direction of the connecting location</param>
        /// <param name="connectingLocation">Connecting location to add</param>
        public void AddExit(Direction direction, Location connectingLocation)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the exit location in a direction
        /// </summary>
        /// <param name="direction">Direciton of the exit location</param>
        /// <returns>The exit location, or this if there is no exit in that direction</returns>
        public Location GetExit(Direction direction) => throw new NotImplementedException();
    }
}