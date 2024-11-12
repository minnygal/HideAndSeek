namespace HideAndSeek
{
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

    public static class Extensions
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
        /// Get the opposite Direction of the direction provided
        /// </summary>
        /// <param name="direction">Direction to find the opposite of</param>
        /// <returns>The opposite Direction of the direction provided</returns>
        public static Direction OppositeDirection(this Direction direction)
        {
            return (Direction)((int)(direction) * -1); // direction provided as integer multiplied by -1 and converted back to Direction
        }

        /// <summary>
        /// Describes a direction (e.g. "in" vs. "to the North")
        /// </summary>
        /// <param name="direction">Direction to describe</param>
        /// <returns>string describing the direction</returns>
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
    }
}
