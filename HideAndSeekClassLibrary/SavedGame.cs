﻿using System;
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
     * -I marked each property as required to ensure it is set (except House).
     * -I created a private variable for each public property.
     * -I created a body for each property's getter and setter methods.
     * -I added data validation in each setter method.
     * -I added a parameterless constructor for JSON deserialization.
     * -I added a House property (used for property data validation)
     * -I added a HouseFileName property and a backing field.
     * -I added a method to set the HouseFileName property's backing field
     *  without using the property's setter (which calls House.CreateHouse)
     * -I added a parameterized constructor for setting properties upon initialization.
     * -I added comments for easier reading.
     **/

    public class SavedGame
    {
        private House _house;

        /// <summary>
        /// House object associated with game (used for property validation)
        /// Can only use setter when backing field has not already been set
        /// (security measure since other properties' setters rely on this property for data validation)
        /// </summary>
        /// <exception cref="NullReferenceException">Exception thrown if House property has not been set</exception>"
        /// <exception cref="InvalidOperationException">Exception thrown if House property has already been set</exception>
        [JsonIgnore]
        public House House
        {
            get
            {
                // If backing field has not been set
                if(_house == null)
                {
                    throw new NullReferenceException("House has not been set"); // Throw exception
                }

                // Return House
                return _house;
            }
            set
            {
                // If backing field already has a value
                if(_house != null)
                {
                    throw new InvalidOperationException("House property already has a value"); // Throw exception
                }

                // Set backing field to value
                _house = value;
            }
        }

        private string _houseFileName;

        /// <summary>
        /// Set the House file name backing field, bypassing the HouseFileName property setter which calls House's CreateHouse method
        /// </summary>
        /// <param name="fileName">Name of file to set</param>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        private void SetHouseFileName_WithoutCreatingHouse(string fileName)
        {
            // If House file name is invalid
            if ( !(new FileSystem().IsValidName(fileName)) )
            {
                throw new ArgumentException($"House file name \"{fileName}\" is invalid " +
                                             "(is empty or contains illegal characters, e.g. \\, /, or whitespace)", "value"); // Throw exception
            }

            // Set House file name backing field, bypassing the HouseFileName property setter which calls House's CreateHouse method
            _houseFileName = fileName;
        }

        /// <summary>
        /// Name of file storing House object for layout (w/o JSON extension)
        /// Can only use setter when backing field has not already been set
        /// (security measure since other properties' setters rely on House, which this sets, for data validation)
        /// Should only be used by JSON deserializer and tests
        /// CAUTION: setter calls House's CreateHouse method
        /// </summary>
        /// <exception cref="NullReferenceException">Exception thrown if HouseFileName property has not been set</exception>"
        /// <exception cref="InvalidOperationException">Exception thrown if HouseFileName property has already been set</exception>
        [JsonRequired]
        public required string HouseFileName
        {
            get
            {
                // If backing field has not been set
                if (_houseFileName == null)
                {
                    throw new NullReferenceException("House file name has not been set"); // Throw exception
                }

                return _houseFileName;
            }
            set
            {
                // If backing field already has a value
                if (_houseFileName != null)
                {
                    throw new InvalidOperationException("HouseFileName property already has a value"); // Throw exception
                }

                // Set backing field and create House
                _houseFileName = value; // Set backing field
                House = House.CreateHouse(HouseFileName); // Create House (must have this line for JSON deserializer because setters use House for data validation)
            }
        }

        private string _playerLocation;

        /// <summary>
        /// Player's current location
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown if value passed to setter does not exist in House</exception>
        [JsonRequired]
        public required string PlayerLocation {
            get
            {
                return _playerLocation;
            }
            set
            {
                // If location does not exist
                if ( !(House.DoesLocationExist(value)) )
                {
                    throw new InvalidOperationException($"invalid PlayerLocation - location \"{value}\" does not exist in House"); // Throw exception
                }

                // Set backing field
                _playerLocation = value;
            }
        }

        private int _moveNumber;

        /// <summary>
        /// Current move number
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if value passed to setter is not a positive number</exception>
        [JsonRequired]
        public required int MoveNumber
        {
            get
            {
                return _moveNumber;
            }
            set
            {
                // If move number is invalid
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", "MoveNumber is invalid - must be positive number"); // Throw exception
                }

                // Set backing field
                _moveNumber = value;
            }
        }

        private Dictionary<string, string> _opponentsAndHidingLocations;

        /// <summary>
        /// Opponents and their locations with hiding place
        /// (opponent name as key, location with hiding place name as value)
        /// </summary>
        /// <exception cref="ArgumentException">Exception thrown if value passed to setter is empty dictionary</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if value passed to setter has value not existing as location with hiding place in House</exception>
        [JsonRequired]
        public required Dictionary<string, string> OpponentsAndHidingLocations
        {
            get
            {
                return _opponentsAndHidingLocations;
            }
            set
            {
                // If no items
                if (value.Count == 0)
                {
                    throw new ArgumentException("invalid OpponentsAndHidingLocations - no opponents", "value"); // Throw exception
                }

                // If any of the LocationWithHidingPlaces do not exist, throw exception
                foreach (KeyValuePair<string, string> opponentInfo in value)
                {
                    if (!(House.DoesLocationWithHidingPlaceExist(opponentInfo.Value)))
                    {
                        throw new InvalidOperationException($"location with hiding place \"{opponentInfo.Value}\" does not exist in House");
                    }
                }

                // Set backing field
                _opponentsAndHidingLocations = value;
            }
        }

        private IEnumerable<string> _foundOpponents;

        /// <summary>
        /// All found opponents' names
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception thrown if value passed to setter includes name of opponent not in game</exception>
        [JsonRequired]
        public required IEnumerable<string> FoundOpponents
        {
            get
            {
                return _foundOpponents;
            }
            set
            {
                // If any found opponents do not exist in OpponentsAndHidingLocations dictionary keys, throw exception
                foreach (string foundOpponent in value)
                {
                    if( !(OpponentsAndHidingLocations.Keys.Contains(foundOpponent)) )
                    {
                        throw new InvalidOperationException("found opponent is not an opponent");
                    }
                }

                // Set backing field
                _foundOpponents = value;
            }
        }

        /// <summary>
        /// Parameterless constructor for JSON deserializer
        /// </summary>
        public SavedGame() { }

        /// <summary>
        /// Constructor for setting properties (does not load House from file or validate that file exists)
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
            House = house;
            SetHouseFileName_WithoutCreatingHouse(houseFileName); // Set backing field rather than property to get around CreateHouse call in property setter
            PlayerLocation = playerLocation;
            MoveNumber = moveNumber;
            OpponentsAndHidingLocations = opponentsAndHidingLocations;
            FoundOpponents = foundOpponents;
        }
    }
}