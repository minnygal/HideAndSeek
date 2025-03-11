using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a location with a hiding place where an Opponent can hide in the House
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's LocationWithHidingPlace class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/LocationWithHidingPlace.cs
     *         Link valid as of 02-25-2025
     **/

    /** CHANGES
     * -I renamed the parameters in the constructor for easier comprehension.
     * -I renamed the private list to opponentsHiding for easier comprehension.
     * -I renamed the Hide method to HideOpponent so it's easier to 
        differentiate between this class's hide method and Opponent's Hide method.
     * -I converted lambdas to regular method bodies for easier modification.
     * -I changed the return type for CheckHidingPlace.
     * -I made a local variable in CheckHidingPlace a specific type.
     * -I renamed a local variable in CheckHidingPlace for easier comprehension.
     * -I added/edited comments for easier reading.
     * **/

    public class LocationWithHidingPlace : Location
    {
        /// <summary>
        /// Name of hiding place
        /// </summary>
        [JsonRequired]
        public required string HidingPlace { get; set; }

        /// <summary>
        /// Opponents hidden in this hiding place
        /// </summary>
        private List<Opponent> opponentsHiding = new List<Opponent>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locationName">Name of location hiding place is in</param>
        /// <param name="hidingPlaceDescription">Name of hiding place</param>
        [SetsRequiredMembers]
        public LocationWithHidingPlace(string locationName, string hidingPlaceDescription) : base(locationName)
        {
            HidingPlace = hidingPlaceDescription;
        }

        /// <summary>
        /// Hide opponent in hiding place
        /// </summary>
        /// <param name="opponent">Opponent to hide</param>
        public void HideOpponent(Opponent opponent)
        {
            opponentsHiding.Add(opponent); // Add opponent to list of opponents hiding in this location
        }

        /// <summary>
        /// Check hiding place for opponents (clear hiding place of all opponents)
        /// </summary>
        /// <returns>List of opponents found in hiding place</returns>
        public List<Opponent> CheckHidingPlace()
        {
            List<Opponent> opponentsFound = new List<Opponent>(opponentsHiding); // Store opponents found in new list
            opponentsHiding.Clear(); // Clear list of opponents hiding
            return opponentsFound; // Return list of opponents found
        }
    }
}