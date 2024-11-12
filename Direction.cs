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
        /// Get the opposite Direction of the direction provided
        /// </summary>
        /// <param name="direction">Direction to find the opposite of</param>
        /// <returns>The opposite Direction of the direction provided</returns>
        public static Direction OppositeDirection(this Direction direction)
        {
            return (Direction)((int)(direction) * -1); // direction provided as integer multiplied by -1 and converted back to Direction
        }
    }
}
