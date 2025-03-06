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
     * -I added comments for easier reading.
     **/

    /** NOTE TO SELF
     * If add parameterized constructor in the future, 
     * remember to add parameterless constructor for JSON deserializer.
     * **/

    public class SavedGame
    {
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
                if( !(House.DoesLocationExist(value)) )
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
                if(value < 1)
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - invalid MoveNumber");
                }

                // Set move number
                _moveNumber = value;
            }
        }

        private Dictionary<string, string> _opponentsAndHidingPlaces;

        /// <summary>
        /// Opponents and their locations with hiding place
        /// (opponent name as key, location with hiding place name as value)
        /// </summary>
        public required Dictionary<string, string> OpponentsAndHidingPlaces 
        { 
            get
            {
                return _opponentsAndHidingPlaces;
            }
            set
            {
                // If no opponents, throw exception
                if ( value.Count == 0 )
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - no opponents");
                }

                // If any of the LocationWithHidingPlaces do not exist, throw exception
                foreach (KeyValuePair<string, string> opponentInfo in value)
                {
                    if( !(House.DoesLocationWithHidingPlaceExist(opponentInfo.Value)) )
                    {
                        throw new InvalidDataException("Cannot process because data is corrupt - invalid hiding place for opponent");
                    }
                }

                // Set dictionary of all opponents and their locations
                _opponentsAndHidingPlaces = value;
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
                // If any found opponents do not exist in OpponentsAndHidingPlaces dictionary, throw exception
                foreach (string foundOpponent in value)
                {
                    if ( !(OpponentsAndHidingPlaces.Keys.Contains(foundOpponent)) )
                    {
                        throw new InvalidDataException("Cannot process because data is corrupt - found opponent is not an opponent");
                    }
                }

                // Set collection of found opponents
                _foundOpponents = value;
            }
        }
    }
}