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
    /// Class to store a saved game (stores state of game)
    /// 
    /// CREDIT: adapted from HideAndSeek project's SavedGame class
    ///         © 2023 Andrew Stellman and Jennifer Greene
    ///         Published under the MIT License
    ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/SavedGame.cs
    ///         Link valid as of 02-25-2025
    ///         
    /// CHANGES:
    /// -I marked each property as required to ensure it is set.
    /// -I created a private variable for each public property.
    /// -I created a body for each property's getter and setter methods.
    /// -I added data validation in each setter method.
    /// -I added a parameterless constructor for JSON deserialization.
    /// -I added a parameterized constructor to set SavedGame's properties to 
    ///  reflect game state stored in GameController passed into constructor.
    /// -I added comments for easier reading.
    /// </summary>
    public class SavedGame
    {
        private string _playerLocation = "";

        // Player's current location
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

        // Current move number
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

        // All opponents (opponent name and hiding place name)
        public required Dictionary<string, string> OpponentsAndHidingPlaces 
        { 
            get
            {
                return _opponentsAndHidingPlaces;
            }
            set
            {
                // If dictionary is empty, throw exception
                if ( value.Count == 0 )
                {
                    throw new InvalidDataException("Cannot process because data is corrupt - no opponents");
                }

                // If any of the locations do not exist, throw exception
                foreach (KeyValuePair<string, string> opponentInfo in value)
                {
                    if( !(House.DoesLocationExist(opponentInfo.Value)) )
                    {
                        throw new InvalidDataException("Cannot process because data is corrupt - invalid hiding place for opponent");
                    }
                }

                // Set dictionary of all opponents
                _opponentsAndHidingPlaces = value;
            }
        }

        private IEnumerable<string> _foundOpponents;

        // All found opponents' names
        public required IEnumerable<string> FoundOpponents
        {
            get
            {
                return _foundOpponents;
            }
            set
            {
                // If any of the opponents do not exist in OpponentsAndHidingPlaces dictionary, throw exception
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

        // Default constructor for JSON deserializer
        public SavedGame() {}
    }
}
