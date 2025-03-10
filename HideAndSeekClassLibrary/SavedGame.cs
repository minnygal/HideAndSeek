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
     * -I added a House property (used for property data validation)
     * -I added a HouseFileName property and a _houseFileName variable.
     * -I added a parameterized constructor for setting properties upon initialization.
     * -I added comments for easier reading.
     **/

    public class SavedGame
    {
        /// <summary>
        /// House object associated with game (used for property validation)
        /// </summary>
        [JsonIgnore]
        public House House { get; set; }

        private string _houseFileName = House.DefaultHouseFileName; // Set house file name to default house file name; set in both a method and property

        /// <summary>
        /// Set the House file name private variable, bypassing the HouseFileName property setter which calls House's CreateHouse method
        /// </summary>
        /// <param name="fileName">Name of file to set</param>
        /// <exception cref="InvalidDataException">Exception thrown if file name in invalid</exception>
        private void SetHouseFileName_WithoutCreatingHouse(string fileName)
        {
            // If House file name is invalid
            if (!(new FileSystem().IsValidName(fileName)))
            {
                throw new InvalidDataException($"Cannot perform action because file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"); // Throw exception
            }
            _houseFileName = fileName;
        }

        /// <summary>
        /// Name of file storing House object for layout (w/o JSON extension)
        /// Should only be used by JSON deserializer and tests
        /// CAUTION: setter calls House's CreateHouse method
        /// </summary>
        public required string HouseFileName
        {
            get
            {
                return _houseFileName;
            }
            set
            {
                House = House.CreateHouse(value); // Create House from value (must have this line for JSON deserializer)
                _houseFileName = value; // Set house file name
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
                if (!(House.DoesLocationExist(value)))
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
                    if (!(House.DoesLocationWithHidingPlaceExist(opponentInfo.Value)))
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
        /// Constructor for setting properties
        /// </summary>
        /// <param name="house">House object being used</param>
        /// <param name="houseFileName">Name of House file</param>
        /// <param name="playerLocation">Current location of player</param>
        /// <param name="moveNumber">Current move number</param>
        /// <param name="opponentsAndHidingLocations">Opponents and their hiding locations</param>
        /// <param name="foundOpponents">Opponents who have been found</param>
        [SetsRequiredMembers]
        public SavedGame(House house, string houseFileName, string playerLocation, int moveNumber, Dictionary<string, string> opponentsAndHidingLocations, IEnumerable<string> foundOpponents)
        {
            this.House = house;
            SetHouseFileName_WithoutCreatingHouse(houseFileName); // Set private variable rather than property to get around CreateHouse call in property setter
            PlayerLocation = playerLocation;
            MoveNumber = moveNumber;
            OpponentsAndHidingLocations = opponentsAndHidingLocations;
            FoundOpponents = foundOpponents;
        }
    }
}