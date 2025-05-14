using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a location with a hiding place where an Opponent can hide in a House
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
     * -I added data validation for the HidingPlace property.
     * -I renamed the parameters in the constructor for easier comprehension.
     * -I renamed the private list to opponentsHiding for easier comprehension.
     * -I renamed the Hide method to HideOpponent for clarity.
     * -I converted lambdas to regular method bodies for easier modification.
     * -I changed the return type for CheckHidingPlace.
     * -I made a local variable in CheckHidingPlace a specific type.
     * -I renamed a local variable in CheckHidingPlace for easier comprehension.
     * -I added a method for JSON serialization.
     * -I added a constructor for JSON deserialization.
     * -I added/edited comments for easier reading.
     * **/

    public class LocationWithHidingPlace : Location
    {
        /// <summary>
        /// Constructor for JSON deserialization
        /// </summary>
        public LocationWithHidingPlace() : base() { }

        /// <summary>
        /// Constructor to set name and hiding place description
        /// </summary>
        /// <param name="locationName">Name of location hiding place is in</param>
        /// <param name="hidingPlaceDescription">Description of hiding place</param>
        [SetsRequiredMembers]
        public LocationWithHidingPlace(string locationName, string hidingPlaceDescription) : base(locationName)
        {
            HidingPlace = hidingPlaceDescription;
        }

        private string _hidingPlace;

        /// <summary>
        /// Description of hiding place
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is invalid (empty or only whitespace)</exception>
        [JsonRequired]
        public required string HidingPlace
        {
            get
            {
                return _hidingPlace;
            }
            set
            {
                // If invalid name is entered
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"hiding place \"{value}\" is invalid (is empty or contains only whitespace)", nameof(value)); // Throw exception
                }

                // Set backing field
                _hidingPlace = value;
            }
        }

        /// <summary>
        /// Opponents hidden in this hiding place
        /// </summary>
        private IList<Opponent> opponentsHiding = new List<Opponent>();
        
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
        public IList<Opponent> CheckHidingPlace()
        {
            IList<Opponent> opponentsFound = new List<Opponent>(opponentsHiding); // Store opponents found in new list
            opponentsHiding.Clear(); // Clear list of opponents hiding
            return opponentsFound; // Return list of opponents found
        }

        /// <summary>
        /// Serialize object and return as string
        /// Calls PrepForSerialization method
        /// which must be called prior to object serialization.
        /// </summary>
        /// <returns>Serialized object as string</returns>
        public override string Serialize()
        {
            PrepForSerialization(); // Prepare object for serialization
            return JsonSerializer.Serialize(this); // Serialize this object as a LocationWithHidingPlace
        }
    }
}