using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HideAndSeek
{
    /// <summary>
    /// Class to control a game and allow external interaction with and direction of game
    /// Only 1 game can be going on at a time.
    /// GameController can be used to restart the game.
    /// 
    /// CREDIT: adapted from HideAndSeek project's GameController class
    ///         © 2023 Andrew Stellman and Jennifer Greene
    ///         Published under the MIT License
    ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/GameController.cs
    ///         Link valid as of 02-25-2025
    ///         
    /// CHANGES:
    /// -I added a restart method so the game can be restarted without creating a new GameController.
    /// -I added methods to rehide all opponents.
    /// -I created a method to update the status.
    /// -I added a private location variable so I could define a setter for CurrentLocation.
    /// -I created a setter method for Currentlocation so it could automatically update the status.
    /// -I made the list of found opponents public for easier game saving/restoration.
    /// -I added a file system class variable for testing purposes.
    /// -I used my own approach in ParseInput but accomplished the same results.
    /// -I renamed methods to SaveGame and LoadGame for easier comprehension.
    /// -I prevented overwriting a saved game file in the SaveGame method.
    /// -I had the SavedGame class create itself in SaveGame method 
    ///  by passing in GameController for separation of concerns.
    /// -I wrapped JSON deserialization in a try/catch block in the LoadGame method
    ///  to return custom feedback/update messages if certain problems occur.
    /// -I moved the code for loading a game from an acceptable JSON file to another method
    ///  to separate it from the code for evaluating whether JSON file is acceptable.
    /// -I created a private variable to store the full file name in SaveGame and LoadGame.
    /// -I created a class variable to store the serialized file extension text.
    /// -I added a method to return a full file name with the serialized file extension.
    /// -I set GameController properties appropriately based on my class design in LoadGame method.
    /// -I changed some feedback/update messages for easier reading.
    /// -I extracted the file name validation logic to its own method for code reuse.
    /// -I renamed a variable in Move method for easier comprehension.
    /// -I added a method to delete a game.
    /// -I added/edited comments for easier reading.
    /// </summary>
    public class GameController
    {
        private IFileSystem _fileSystem;
        private string _fileNameExtension = "json";

        /// <summary>
        /// Get full name for file (including extension)
        /// </summary>
        /// <param name="fileName">Name of file not including extension</param>
        /// <returns>Full name of file including extension</returns>
        private string GetFullFileName(string fileName)
        {
            return $"{fileName}.{_fileNameExtension}";
        }
        
        /// <summary>
        /// The player's current location in the house
        /// </summary>
        public Location CurrentLocation { get; private set; }

        /// <summary>
        /// Status of game
        /// </summary>
        public string Status
        {
            get
            {
                // Initialize variable to first part of message for status
                string message = $"You are in the {CurrentLocation.Name}. You see the following exits:";

                // Add each exit description to the message for status
                foreach (string exitDescription in CurrentLocation.ExitList())
                {
                    message += Environment.NewLine + " - " + exitDescription;
                }

                // Add hiding place info if someone could hide in CurrentLocation
                if (CurrentLocation.GetType() == typeof(LocationWithHidingPlace))
                {
                    LocationWithHidingPlace location = (LocationWithHidingPlace)CurrentLocation;
                    message += Environment.NewLine + $"Someone could hide {location.HidingPlace}";
                }

                // If no opponents have been found
                if (FoundOpponents.Count == 0)
                {
                    message += Environment.NewLine + "You have not found any opponents";
                }
                else // if opponents have been found
                {
                    // Add information about opponents found to message
                    message += Environment.NewLine + $"You have found {FoundOpponents.Count} of {OpponentsAndHidingPlaces.Count()} opponent{(OpponentsAndHidingPlaces.Count() == 1 ? "" : "s")}"
                            + ": " + String.Join(", ", FoundOpponents);
                }

                // Return status message
                return message;
            }
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
        /// Opponents and their hiding places
        /// </summary>
        public Dictionary<Opponent, LocationWithHidingPlace> OpponentsAndHidingPlaces { get; private set; } = new Dictionary<Opponent, LocationWithHidingPlace>();
            
        /// <summary>
        /// List of opponents the player has found so far
        /// </summary>
        public List<Opponent> FoundOpponents { get; private set; } = new List<Opponent>();

        /// <summary>
        /// Returns true if the game is over
        /// </summary>
        public bool GameOver => OpponentsAndHidingPlaces.Count() == FoundOpponents.Count();

        /// <summary>
        /// Constructor to start game
        /// </summary>
        public GameController() : this(new FileSystem()) {}

        /// <summary>
        /// Constructor to start game with specific file system (used for testing)
        /// </summary>
        /// <param name="fileSystem">File system</param>
        public GameController(IFileSystem fileSystem)
        {
            // Set file system
            _fileSystem = fileSystem;

            // Create Opponents and store them in dictionary as keys
            OpponentsAndHidingPlaces.Add(new Opponent("Joe"), null);
            OpponentsAndHidingPlaces.Add(new Opponent("Bob"), null);
            OpponentsAndHidingPlaces.Add(new Opponent("Ana"), null);
            OpponentsAndHidingPlaces.Add(new Opponent("Owen"), null);
            OpponentsAndHidingPlaces.Add(new Opponent("Jimmy"), null);

            // Start game
            RestartGame();
        }

        /// <summary>
        /// Restart game from beginning (Entry)
        /// </summary>
        public void RestartGame()
        {
            // Hide opponents in random places
            RehideAllOpponents();

            // Reset properties
            MoveNumber = 1; // Reset move number
            CurrentLocation = House.Entry; // Set current location to Entry
        }

        /// <summary>
        /// Rehide all opponents in specified hiding places
        /// </summary>
        /// <param name="hidingPlaces">Places to hide opponents</param>
        public void RehideAllOpponents(IEnumerable<LocationWithHidingPlace> hidingPlaces)
        {
            // Clear hiding places
            House.ClearHidingPlaces();

            // Initialize dictionary to store opponents and new hiding places
            Dictionary<Opponent, LocationWithHidingPlace> opponentsAndNewHidingPlaces = new Dictionary<Opponent, LocationWithHidingPlace>();

            // Hide Opponents in hiding places and add hiding places to dictionary
            for (int i = 0; i < OpponentsAndHidingPlaces.Count(); i++)
            {
                hidingPlaces.ElementAt(i).HideOpponent(OpponentsAndHidingPlaces.ElementAt(i).Key);
                opponentsAndNewHidingPlaces.Add(OpponentsAndHidingPlaces.ElementAt(i).Key, hidingPlaces.ElementAt(i));
            }

            // Set opponents and hiding places dictionary to dictionary with new hiding places
            OpponentsAndHidingPlaces = opponentsAndNewHidingPlaces;
        }

        /// <summary>
        /// Rehide all opponents in random hiding places
        /// </summary>
        public void RehideAllOpponents()
        {
            // Initialize list of locations with hiding places
            List<LocationWithHidingPlace> hidingPlaces = new List<LocationWithHidingPlace>();

            // Populate list with random hiding places (1 per Opponent)
            for(int i = 0; i < OpponentsAndHidingPlaces.Count(); i++)
            {
                hidingPlaces.Add(House.GetRandomHidingPlace());
            }

            // Hide Opponents in hiding places
            RehideAllOpponents(hidingPlaces);
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
            // Extract lowercaseCommand, one version lowercase and one version original
            string originalCommand = input.Split(" ").FirstOrDefault("");
            string lowercaseCommand = originalCommand.ToLower();

            // If input requests the current location be checked for hiding opponents
            if (lowercaseCommand == "check")
            {
                MoveNumber++; // Increment move number
                return CheckCurrentLocation(); // Check current location and return results
            }
            else if ( // If input requests save or load game
                lowercaseCommand == "save" || 
                lowercaseCommand == "load" ||
                lowercaseCommand == "delete" )
            {
                // Get index of space in input (space before name of file)
                int indexOfSpace = input.IndexOf(' ');

                // If input includes a space
                if (indexOfSpace != -1)
                {
                    // Extract file name
                    string fileName = input.Substring(indexOfSpace + 1);

                    // If file name is valid
                    if (IsValidFileName(fileName))
                    {
                        // If input requests save game
                        if (lowercaseCommand == "save")
                        {
                            return SaveGame(fileName); // Save game and return message
                        }
                        else if (lowercaseCommand == "load") // If input requests load game
                        {
                            return LoadGame(fileName); // Load game and return message
                        }
                        else // If input requests delete game
                        {
                            return DeleteGame(fileName);
                        }
                    }
                }

                // If any of the requirements for input are not met, return failure message
                return "Cannot perform action because file name is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)";
            }
            else if (!(Enum.TryParse(originalCommand, out Direction direction))) // If input cannot be parsed to Direction enum value
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
            // Set variable to full file name including extension
            string fullFileName = GetFullFileName(fileName);

            // If file already exists
            if (_fileSystem.File.Exists(fullFileName))
            {
                return $"Cannot perform action because a file named {fileName} already exists";
            }

            // Create dictionary of opponents and hiding places as strings
            Dictionary<string, string> opponentsAndHidingPlacesAsStrings = new Dictionary<string, string>();
            foreach(KeyValuePair<Opponent, LocationWithHidingPlace> kvp in OpponentsAndHidingPlaces)
            {
                opponentsAndHidingPlacesAsStrings.Add(kvp.Key.ToString(), kvp.Value.ToString());
            }

            // Initialize object to store game state data
            SavedGame savedGame = new SavedGame()
            {
                PlayerLocation = CurrentLocation.ToString(),
                MoveNumber = this.MoveNumber,
                OpponentsAndHidingPlaces = opponentsAndHidingPlacesAsStrings,
                FoundOpponents = this.FoundOpponents.Select((x) => x.ToString())
            };

            // Save game as JSON to file and return success message
            string savedGameAsJSON = JsonSerializer.Serialize(savedGame); // Convert game's state data to JSON
            _fileSystem.File.WriteAllText(fullFileName, savedGameAsJSON); // Save game's state data in file
            return $"Game successfully saved in {fileName}"; // Return success message
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="fileName">Name of file from which to load game data</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string LoadGame(string fileName)
        {
            // Set variable to full file name including extension
            string fullFileName = GetFullFileName(fileName);

            // If file does not exist
            if( !(_fileSystem.File.Exists(fullFileName)) )
            {
                return "Cannot load game because file does not exist";
            }

            // Read text from file
            string fileText = _fileSystem.File.ReadAllText(fullFileName);

            // Declare variable to store SavedGame object
            SavedGame savedGame;

            // Deserialize text from file into SavedGame object
            try
            {
                savedGame = JsonSerializer.Deserialize<SavedGame>(fileText);
            } 
            catch(InvalidDataException e) // if data for specific field (evauluated in property setter) is invalid
            {
                return e.Message;
            }
            catch(Exception e)
            {
                // If problem due to JSON or an invalid error, return failure message
                if(e is JsonException || e is InvalidOperationException)
                {
                    return "Cannot load game because data in file is corrupt";
                }
                else
                {
                    throw;
                }
            }

            // Load game from SavedGame object.
            LoadGame(savedGame);

            // Return success message
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

            // Restore dictionary of Opponents and their hiding places, creating new Opponent objects
            OpponentsAndHidingPlaces = new Dictionary<Opponent, LocationWithHidingPlace>();
            foreach(KeyValuePair<string, string> kvp in savedGame.OpponentsAndHidingPlaces)
            {
                OpponentsAndHidingPlaces.Add(new Opponent(kvp.Key), (LocationWithHidingPlace)House.GetLocationByName(kvp.Value));
            }

            // Restore list of found opponents
            FoundOpponents.Clear(); // Clear list of found opponents
            foreach(String opponent in savedGame.FoundOpponents)
            {
                FoundOpponents.Add(OpponentsAndHidingPlaces.Keys.First((x) => x.Name == opponent)); // Add Opponent object with matching name to FoundOpponents list
            }
        }

        /// <summary>
        /// Delete game file
        /// </summary>
        /// <param name="fileName">Name of file to delete</param>
        /// <returns>String describing what happened</returns>
        private string DeleteGame(string fileName)
        {
            // Set variable to full file name including extension
            string fullFileName = GetFullFileName(fileName);

            // If file does not exist
            if ( !(_fileSystem.File.Exists(fullFileName)) )
            {
                return $"Could not delete game because file {fileName} does not exist";
            }

            // Delete file
            _fileSystem.File.Delete(fullFileName);

            // Return success message
            return $"Game file {fileName} has been successfully deleted";
        }
    }
}