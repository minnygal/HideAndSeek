using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HideAndSeek
{
    public class GameController
    {
        private Location _location;

        /// <summary>
        /// The player's current location in the house
        /// </summary>
        public Location CurrentLocation { 
            get 
            { 
                return _location; 
            } 
            
            // Set the location and update status
            private set {
                _location = value;
                UpdateStatus();
            } 
        }

        public string Status { get; private set; }

        /// <summary>
        /// Returns the the current status to show to the player
        /// Called by CurrentLocation's setter
        /// </summary>
        public void UpdateStatus()
        {
            // Initialize variable to first part of message for status
            string message = $"You are in the {CurrentLocation.Name}. You see the following exits:";
            
            // Add each exit description to the message for status
            foreach(string exitDescription in CurrentLocation.ExitList())
            {
                message += Environment.NewLine + " - " + exitDescription;
            }

            // Add hiding place info if someone could hide in CurrentLocation
            if(CurrentLocation.GetType() ==  typeof(LocationWithHidingPlace))
            {
                LocationWithHidingPlace location = (LocationWithHidingPlace) CurrentLocation;
                message += Environment.NewLine + $"Someone could hide {location.HidingPlace}";
            }

            // If no opponents have been found
            if(FoundOpponents.Count == 0)
            {
                message += Environment.NewLine + "You have not found any opponents";
            }
            else // if opponents have been found
            {
                // Add information about opponents found to message
                message += Environment.NewLine + $"You have found {FoundOpponents.Count} of {Opponents.Count()} opponent{(Opponents.Count() == 1 ? "" : "s")}"
                        + ": " + String.Join(", ", FoundOpponents);
            }

            Status = message; // Set status
        }

        /// <summary>
        /// A prompt to display to the player
        /// </summary>
        public string Prompt => $"{MoveNumber}: Which direction do you want to go (or type 'check'): ";

        /// <summary>
        /// The number of moves the player has made
        /// </summary>
        public int MoveNumber { get; private set; } = 1;

        /// <summary>
        /// Private list of opponents the player needs to find
        /// </summary>
        public IEnumerable<Opponent> Opponents { get; private set; } = new List<Opponent>()
        {
            new Opponent("Joe"),
            new Opponent("Bob"),
            new Opponent("Ana"),
            new Opponent("Owen"),
            new Opponent("Jimmy")
        };

        /// <summary>
        /// List of opponents the player has found so far
        /// </summary>
        public List<Opponent> FoundOpponents { get; private set; } = new List<Opponent>();

        /// <summary>
        /// Returns true if the game is over
        /// </summary>
        public bool GameOver => Opponents.Count() == FoundOpponents.Count();

        /// <summary>
        /// Constructor to start game
        /// </summary>
        public GameController()
        {
            RestartGame(); // Restart game
        }

        /// <summary>
        /// Restart game from beginning (Entry)
        /// </summary>
        public void RestartGame()
        {
            // Clear hiding places
            House.ClearHidingPlaces(); 

            // Hide opponents
            foreach (var opponent in Opponents)
            {
                opponent.Hide();
            }

            // Reset properties
            MoveNumber = 1; // Reset move number
            CurrentLocation = House.Entry; // Set current location to Entry
        }

        /// <summary>
        /// Move to the location in a direction
        /// </summary>
        /// <param name="direction">The direction to move</param>
        /// <returns>True if the player can move in that direction, false oterwise</returns>
        public bool Move(Direction direction)
        {
            Location startLocation = CurrentLocation; // Set start location to current location
            CurrentLocation = CurrentLocation.GetExit(direction); // Set current location to exit returned
            return CurrentLocation != startLocation; // Return whether the current location has changed (move was successful)
        }

        /// <summary>
        /// Parses input from the player and updates the status
        /// </summary>
        /// <param name="input">Input to parse</param>
        /// <returns>The results of parsing the input</returns>
        public string ParseInput(string input)
        {
            // If input requests the current location be checked for hiding opponents
            if (input.ToLower() == "check")
            {
                MoveNumber++; // Increment move number
                return CheckCurrentLocation(); // Check current location and return results
            }
            else if(input.ToLower().StartsWith("save") || input.ToLower().StartsWith("load")) // If input requests save or load game
            {
                // If input is acceptable length (containing a file name of at least 1 character)
                if(input.Length > 5)
                {
                    string fileName = input.Substring(5); // Extract file name

                    // If file name is valid
                    if(IsValidFileName(fileName)) 
                    {
                        // If input requests save game
                        if(input.ToLower().StartsWith("save"))
                        {
                            return SaveGame(fileName); // Save game and return message
                        }
                        else // if input requests load game
                        {
                            return LoadGame(fileName); // Load game and return message
                        }
                    }
                }

                // If any of the requirements for input are not met, return failure message
                return "Cannot perform action because file name is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)";
            }
            else if (!(Enum.TryParse(input, out Direction direction))) // If input cannot be parsed to Direction enum value
            {
                return "That's not a valid direction"; // Return invalid direction message
            }
            else if (!(this.Move(direction))) // If cannot move in specified direction
            {
                MoveNumber++; // Increment move number
                return "There's no exit in that direction"; // Return no exit in that direction message
            }
            else // If successfully moved in specified direction
            {
                MoveNumber++; // Increment move number
                return "Moving " + direction; // Return the direction you're moving
            }
        }

        /// <summary>
        /// Helper method to check current location for opponents
        /// </summary>
        /// <returns>The results of checking the location</returns>
        private string CheckCurrentLocation()
        {
            // If current location has a hiding place
            if (CurrentLocation.GetType() == typeof(LocationWithHidingPlace))
            {
                LocationWithHidingPlace location = (LocationWithHidingPlace)CurrentLocation; // Convert CurrentLocation to LocationWithHidingPlace
                List<Opponent> found = location.CheckHidingPlace(); // Check hiding place and initialize list to opponents found

                // If any opponents were found in the hiding place
                if (found.Count >= 1)
                {
                    FoundOpponents.AddRange(found); // Add opponents found to list
                    UpdateStatus(); // Update status
                    return $"You found {found.Count} opponent{(found.Count == 1 ? "" : "s")} hiding {location.HidingPlace}";
                }
                else // if no opponents were found in the hiding place
                {
                    return $"Nobody was hiding {location.HidingPlace}";
                }
            }
            else // If current location does not have a hiding place
            {
                return $"There is no hiding place in the {CurrentLocation}";
            }
        }

        /// <summary>
        /// Return whether file name is valid (not empty and no illegal characters)
        /// </summary>
        /// <param name="fileName">File name to examine</param>
        /// <returns>Whether file name is valid</returns>
        private bool IsValidFileName(string fileName)
        {
            // Return true if file name doen NOT contain whitespace, a backslash, or a forward slash
            return !(fileName.Contains(" ") || fileName.Contains("\\") || fileName.Contains("/") || fileName.Equals(""));
        }

        /// <summary>
        /// Save game to file
        /// </summary>
        /// <param name="fileName">Name of file in which to save game data</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string SaveGame(string fileName)
        {
            SavedGame savedGame = new SavedGame(this);
            // TODO
            // Convert to JSON
            // Create file
            // Store text
            return $"Game successfully saved in {fileName}";
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="fileName">Name of file from which to load game data</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string LoadGame(string fileName)
        {
            // TODO
            // Read text from file
            // Convert to SavedGame object
            // Call LoadGame with SavedGame object
            return $"Game successfully loaded from {fileName}";
        }

        /// <summary>
        /// Load game from SavedGame object
        /// </summary>
        /// <param name="savedGame">SavedGame object from which to load game</param>
        private void LoadGame(SavedGame savedGame)
        {
            // Set current location
            CurrentLocation = House.GetLocationByName(savedGame.PlayerLocation);

            // Set move number
            MoveNumber = savedGame.MoveNumber;

            // Restore list of opponents, creating new Opponent objects
            Opponents = savedGame.AllOpponents.Select((x) => new Opponent(x.Key, (LocationWithHidingPlace)House.GetLocationByName(x.Value)));

            // Restore list of found opponents
            FoundOpponents.Clear(); // Clear list of found opponents
            foreach(String opponent in savedGame.FoundOpponents)
            {
                FoundOpponents.Add(Opponents.First((x) => x.Name == opponent)); // Add Opponent object with matching name to FoundOpponents list
            }
        }
    }
}
