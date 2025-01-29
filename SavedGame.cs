using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class SavedGame
    {
        private string _playerLocation = "";

        // Player's current location
        [JsonInclude]
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
                    throw new InvalidDataException("Cannot load game because data in file is corrupt - invalid CurrentLocation");
                }

                // Set player location
                _playerLocation = value;
            }
        }

        private int _moveNumber;

        // Current move number
        [JsonInclude]
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
                    throw new InvalidDataException("Cannot load game because data in file is corrupt - invalid MoveNumber");
                }

                // Set move number
                _moveNumber = value;
            }
        }

        private Dictionary<string, string> _allOpponents;

        // All opponents (opponent name and hiding place name)
        [JsonInclude]
        public required Dictionary<string, string> AllOpponents 
        { 
            get
            {
                return _allOpponents;
            }
            set
            {
                // If dictionary is empty, throw exception
                if ( value.Count == 0 )
                {
                    throw new InvalidDataException("Cannot load game because data in file is corrupt - no opponents");
                }

                // If any of the locations do not exist, throw exception
                foreach (KeyValuePair<string, string> opponentInfo in value)
                {
                    if( !(House.DoesLocationExist(opponentInfo.Value)) )
                    {
                        throw new InvalidDataException("Cannot load game because data in file is corrupt - invalid hiding place for opponent");
                    }
                }

                // Set dictionary of all opponents
                _allOpponents = value;
            }
        }

        private IEnumerable<string> _foundOpponents;

        // All found opponents' names
        [JsonInclude]
        public required IEnumerable<string> FoundOpponents
        {
            get
            {
                return _foundOpponents;
            }
            set
            {
                // If any of the opponents do not exist in AllOpponents dictionary, throw exception
                foreach (string foundOpponent in value)
                {
                    if ( !(AllOpponents.Keys.Contains(foundOpponent)) )
                    {
                        throw new InvalidDataException("Cannot load game because data in file is corrupt - found opponent is not an opponent");
                    }
                }

                // Set collection of found opponents
                _foundOpponents = value;
            }
        }

        // Default constructor for JSON deserializer
        public SavedGame() {}

        // Constructor to create SavedGame from game state stored in GameController
        [SetsRequiredMembers]
        public SavedGame(GameController gameController)
        {
            PlayerLocation = gameController.CurrentLocation.Name; // Set player location
            MoveNumber = gameController.MoveNumber; // Set move number

            // Get opponents
            IEnumerable<Opponent> opponents = gameController.Opponents;

            // Add opponents and their hiding places to a collection
            Dictionary<string, string> allOpponents = new Dictionary<string, string>();
            foreach (Opponent opponent in opponents)
            {
                allOpponents.Add(opponent.Name, opponent.HidingPlace.Name);
            }

            // Set property to collection of all opponents
            AllOpponents = allOpponents;

            // Set property to names of found opponents
            FoundOpponents = gameController.FoundOpponents.Select((x) => x.Name);
        }
    }
}
