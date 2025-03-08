using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent an Opponent who can be hidden in a Location in the House for the user to find
    /// 
    /// CREDIT: adapted from Stellman and Greene's cod
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's Opponent class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/Opponent.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I added a constructor which accepts a hiding place to hide the opponent
     *  at the same time they are created.
     * -I added a property to store the opponent's hiding place
     *  to make it easier to save and restore games.
     * -I moved the random hiding place picking logic from the Hide method to the House class.
     * -I created a Hide method override to allow hiding opponent in specific hiding place
     *   for easier testing and in case the functionality is needed in the future.
     * -I converted lambdas to regular method bodies for easier modification.
     * -I added a feature to allow opponents to be created with default names.
     *      -Allows an opponent to be created without a name passed in.
     *      -New opponent is given a default name with an incrementing number.
     *      -I provided a method to reset the incrementing number.
     * -I added/edited comments for easier reading.
     * **/

    public class Opponent
    {
        public readonly string Name;

        public override string ToString() => Name;

        private static int _randomOpponentNumber = 1; // Number for next opponent created with default name
        
        /// <summary>
        /// Reset numbers for default names for opponents created without specified names
        /// </summary>
        public static void ResetDefaultNumbersForOpponentNames()
        {
            _randomOpponentNumber = 1;
        }

        /// <summary>
        /// Create opponent with default name
        /// </summary>
        public Opponent() : this("Random Opponent " + _randomOpponentNumber++) { }

        /// <summary>
        /// Create opponent with specified name
        /// </summary>
        /// <param name="name"></param>
        public Opponent(string name)
        {
            Name = name;
        }
    }
}
