namespace HideAndSeek
{
    /// <summary>
    /// Class to represent an Opponent who can be hidden in a Location in a House for the user to find
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's Opponent class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Opponent.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I moved the Hide method out of this class to keep all the
     *  hiding functionality and details in LocationWithHidingPlace
     *  (instead of having the functionality in both classes).
     * -I converted lambdas to regular method bodies for easier modification.
     * -I added a feature to allow opponents to be created with default names.
     *      -Allows an opponent to be created without a name passed in.
     *      -New opponent is given a default name with an incrementing number.
     *      -I provided a method to reset the incrementing number.
     * -I added data validation to the Name property.
     * -I added/edited comments for easier reading.
     * **/

    public class Opponent
    {
        /// <summary>
        /// Check if name is valid for an Opponent
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>True if name is valid</returns>
        public static bool IsValidName(string name)
        {
            return !(name.Trim() == string.Empty);
        }

        private string _name;

        /// <summary>
        /// Name of Opponent
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty or only whitespace)</exception>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                // If not valid name
                if( !(IsValidName(value)) )
                {
                    throw new ArgumentException($"opponent name \"{value}\" is invalid (is empty or contains only whitespace)", nameof(value));
                }

                // Set name
                _name = value;
            }
        }

        public override string ToString() => Name;

        private static int numberForNextDefaultName = 1; // Number for next name for opponent created with default name
        
        /// <summary>
        /// Reset numbers for default names for opponents created without specified names
        /// </summary>
        public static void ResetDefaultNumbersForOpponentNames()
        {
            numberForNextDefaultName = 1;
        }

        /// <summary>
        /// Create opponent with default name
        /// </summary>
        public Opponent() : this("Random Opponent " + numberForNextDefaultName++) { }

        /// <summary>
        /// Create opponent with specified name
        /// </summary>
        /// <param name="name">Name off opponent</param>
        public Opponent(string name)
        {
            Name = name;
        }
    }
}