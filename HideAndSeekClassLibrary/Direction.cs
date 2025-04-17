namespace HideAndSeek
{
    /// <summary>
    /// Enum to represent a direction to describe the relationship between two Locations
    /// 
    /// CREDIT: copied from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  copied from HideAndSeek project's Direction enum class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Direction.cs
     *         Link valid as of 02-25-2025
     * **/

    public enum Direction
    {
        North = -1,
        South = 1,
        East = -2,
        West = 2,
        Northeast = -3,
        Southwest = 3,
        Southeast = -4,
        Northwest = 4,
        Up = -5,
        Down = 5,
        In = -6,
        Out = 6
    }

    /// <summary>
    /// Extension class for Direction enum to describe a direction or get the opposite direction
    /// Also includes static method to convert a string to a direction
    /// 
    /// CREDIT: Direction descriptions and description logic adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  direction descriptions and direction description logic
     *  adapted from HideAndSeek project's Location class's DescribeDirection method
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Direction.cs
     *         Link valid as of 02-25-2025
     * **/
    public static class DirectionExtensions
    {
        /// <summary>
        /// Directions whose descriptions should be output without extra words
        /// </summary>
        private static readonly Direction[] directionsToOutputWithoutExtraText = 
        {   
            Direction.Up, 
            Direction.Down, 
            Direction.In, 
            Direction.Out 
        };

        /// <summary>
        /// Get the opposite Direction of the Direction provided
        /// </summary>
        /// <param name="direction">Direction to find the opposite of</param>
        /// <returns>The opposite Direction of the Direction provided</returns>
        public static Direction OppositeDirection(this Direction direction)
        {
            // Return Direction provided converted to integer, multiplied by -1, and converted back to Direction
            return (Direction)((int)(direction) * -1); 
        }

        /// <summary>
        /// Describes a direction (e.g. "in" vs. "to the North")
        /// </summary>
        /// <param name="direction">Direction to describe</param>
        /// <returns>String describing the direction</returns>
        public static string DirectionDescription(this Direction direction)
        {
            // If the direction should be output without extra words
            if(directionsToOutputWithoutExtraText.Contains(direction))
            {
                return direction.ToString(); // Return just direction as string
            } 
            else
            {
                return $"to the {direction}"; // Return extra words with direction
            }
        }

        /// <summary>
        /// Text values (all lowercase) for Directions
        /// (key is text value written out or shorthand, value is associated Direction)
        /// </summary>
        public static readonly IDictionary<string, Direction> TextsForDirections = new Dictionary<string, Direction>()
        {
            { "north", Direction.North },
            { "n", Direction.North },
            { "south", Direction.South },
            { "s", Direction.South },
            { "east", Direction.East },
            { "e", Direction.East },
            { "west", Direction.West },
            { "w", Direction.West },
            { "northeast", Direction.Northeast },
            { "ne", Direction.Northeast },
            { "southwest", Direction.Southwest },
            { "sw", Direction.Southwest },
            { "southeast", Direction.Southeast },
            { "se", Direction.Southeast },
            { "northwest", Direction.Northwest },
            { "nw", Direction.Northwest },
            { "up", Direction.Up },
            { "u", Direction.Up },
            { "down", Direction.Down },
            { "d", Direction.Down },
            { "in", Direction.In },
            { "i", Direction.In },
            { "out", Direction.Out },
            { "o", Direction.Out }
        };

        /// <summary>
        /// Parse Direction from text (not case sensitive)
        /// (checks for direction as text and shorthand for direction)
        /// </summary>
        /// <param name="directionText">Direction as text or shorthand for direction</param>
        /// <returns>Direction parsed from text</returns>
        /// <exception cref="ArgumentException">Exception thrown if direction invalid</exception>
        public static Direction Parse(string directionText)
        {
            // Trim direction text and convert to lowercase
            directionText = directionText.Trim().ToLower();

            // Attempt to parse direction, if parse unsuccessful
            if( !(TextsForDirections.TryGetValue(directionText, out Direction parsedDirection)) )
            {
                throw new ArgumentException("That's not a valid direction", nameof(directionText)); // Throw new exception with custom error message
            }

            // Return whether parse was successful
            return parsedDirection;
        }
    }
}
