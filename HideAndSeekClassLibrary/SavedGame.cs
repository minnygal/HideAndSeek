using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to represent a saved game, stores the state of a game, and is serializable
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's SavedGame class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/SavedGame.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I marked each property as required to ensure it is set.
     * -I created a private variable for each public property.
     * -I created a body for each property's getter and setter methods.
     * -I added data validation in each setter method.
     * -I added a parameterless constructor for JSON deserialization.
     * -I added a House property and a _houseFileName variable.
     * -I added parameterized constructors for 
     *  easily setting properties upon initialization.
     * -I added comments for easier reading.
     **/

    public class SavedGame
    {
        private House house = House.CreateHouse(House.DefaultHouseFileName); // Set associated House to House with default layout (used for location-related data validation)

        private string _houseFileName = House.DefaultHouseFileName; // Set house file name to default house file name

        /// <summary>
        /// Name of file storing House object for layout (w/o JSON extension)
        /// </summary>
        public required string HouseFileName
        {
            get
            {
                return _houseFileName;
            }
            set
            {
                _houseFileName = value; // Set House file name
                house = House.CreateHouse(_houseFileName); // Create House from file and assign to variable
            }
        }

        private string _playerLocation = "";

        /// <summary>
        /// Player's current location
        /// </summary>
        public required string PlayerLocation {
            get
            {
                return _playerLocation;
            }
            set
            {
                // If location does not exist, throw exception
                if (!(house.DoesLocationExist(value)))
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - invalid CurrentLocation");
                }

                // Set player location
                _playerLocation = value;
            }
        }

        private int _moveNumber;

        /// <summary>
        /// Current move number
        /// </summary>
        public required int MoveNumber
        {
            get
            {
                return _moveNumber;
            }
            set
            {
                // If move number is invalid, throw exception
                if (value < 1)
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - invalid MoveNumber");
                }

                // Set move number
                _moveNumber = value;
            }
        }

        private Dictionary<string, string> _opponentsAndHidingLocations;

        /// <summary>
        /// Opponents and their locations with hiding place
        /// (opponent name as key, location with hiding place name as value)
        /// </summary>
        public required Dictionary<string, string> OpponentsAndHidingLocations
        {
            get
            {
                return _opponentsAndHidingLocations;
            }
            set
            {
                // If no opponents, throw exception
                if (value.Count == 0)
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - no opponents");
                }

                // If any of the LocationWithHidingPlaces do not exist, throw exception
                foreach (KeyValuePair<string, string> opponentInfo in value)
                {
                    if (!(house.DoesLocationWithHidingPlaceExist(opponentInfo.Value)))
                    {
                        throw new InvalidDataException("Cannot process because data is corrupt - invalid hiding location for opponent");
                    }
                }

                // Set dictionary of all opponents and their locations
                _opponentsAndHidingLocations = value;
            }
        }

        private IEnumerable<string> _foundOpponents;

        /// <summary>
        /// All found opponents' names
        /// </summary>
        public required IEnumerable<string> FoundOpponents
        {
            get
            {
                return _foundOpponents;
            }
            set
            {
                // If any found opponents do not exist in OpponentsAndHidingLocations dictionary, throw exception
                foreach (string foundOpponent in value)
                {
                    if (!(OpponentsAndHidingLocations.Keys.Contains(foundOpponent)))
                    {
                        throw new InvalidDataException("Cannot process because data is corrupt - found opponent is not an opponent");
                    }
                }

                // Set collection of found opponents
                _foundOpponents = value;
            }
        }

        /// <summary>
        /// Parameterless constructor for JSON deserializer
        /// </summary>
        public SavedGame() { }

        /// <summary>
        /// Constructor for setting properties (Opponents and Locations accepted as strings)
        /// </summary>
        /// <param name="house">House object being used</param>
        /// <param name="playerLocation">Current location of player</param>
        /// <param name="moveNumber">Current move number</param>
        /// <param name="opponentsAndHidingLocations">Opponents and their hiding locations</param>
        /// <param name="foundOpponents">Opponents who have been found</param>
        [SetsRequiredMembers]
        public SavedGame(House house, string playerLocation, int moveNumber, Dictionary<string, string> opponentsAndHidingLocations, IEnumerable<string> foundOpponents)
        {
            this.house = house;
            HouseFileName = house.HouseFileName;
            PlayerLocation = playerLocation;
            MoveNumber = moveNumber;
            OpponentsAndHidingLocations = opponentsAndHidingLocations;
            FoundOpponents = foundOpponents;
        }

        /// <summary>
        /// Constructor for setting properties (Opponents and Locations accepted as objects)
        /// </summary>
        /// <param name="house">House object being used</param>
        /// <param name="playerLocation">Current location of player</param>
        /// <param name="moveNumber">Current move number</param>
        /// <param name="opponentsAndHidingLocations">Opponents and their hiding locations</param>
        /// <param name="foundOpponents">Opponents who have been found</param>
        [SetsRequiredMembers]
        public SavedGame(House house, Location playerLocation, int moveNumber, Dictionary<Opponent, LocationWithHidingPlace> opponentsAndHidingLocations, IEnumerable<Opponent> foundOpponents)
               : this(house, playerLocation.ToString(), moveNumber, ConvertOpponentsAndHidingPlacesToStringDictionary(opponentsAndHidingLocations), foundOpponents.Select((o) => o.Name)) { }

        /// <summary>
        /// Convert a Dictionary of Opponents and LocationWithHidingPlaces to a Dictionary of representative strings
        /// </summary>
        /// <param name="opponentsAndHidingLocations">Dictionary of Opponents and LocationWithHidingPlaces</param>
        /// <returns>Dictionary of strings representing Opponents and LocationWithHidingPlaces</returns>
        private static Dictionary<string, string> ConvertOpponentsAndHidingPlacesToStringDictionary(Dictionary<Opponent, LocationWithHidingPlace> opponentsAndHidingLocations)
        {
            // Initialize variable to store Dictionary of strings
            Dictionary<string, string> opponentsAndHidingLocationsAsStrings = new Dictionary<string, string>();
            
            // For each Dictionary entry, add Opponent and LocationWithHidingPlace represented as string to string Dictionary
            foreach (KeyValuePair<Opponent, LocationWithHidingPlace> kvp in opponentsAndHidingLocations)
            {
                opponentsAndHidingLocationsAsStrings.Add(kvp.Key.Name, kvp.Value.Name);
            }

            // Return Dictionary of string representations of Opponents and LocationWithHidingPlaces
            return opponentsAndHidingLocationsAsStrings;
        }
    }
}