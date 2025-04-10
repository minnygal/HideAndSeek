﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to control a game of hide and seek and allow external interaction with and direction of game
    /// 
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's GameController class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeek/GameController.cs
     *         Link valid as of 02-25-2025
     * **/

    /** NOTES
     * -There MUST be a valid DefaultHouse.json file for the constructor to be called
     *  without a House file name because the default value is DefaultHouse.
     * -You can start a new game w/o having to create a new GameController instance.
     * -Command keywords cannot have spaces.
     * -Current command keywords are: move, check, save, load, and delete
     * -In ParseInput, file name is extracted from user input
     *  starting after the space following the save, load, or delete keyword.
     * **/

    /** CHANGES
     * -I added a restart method so the game can be restarted without creating a new GameController.
     * -I added methods to rehide all opponents.
     * -I made the list of found opponents public for easier game saving/restoration.
     * -I added a file system class variable for testing purposes.
     * -I added a constructor to accept specific names for opponents.
     * -I added a constructor to accept the number of opponents (between 1 and 10) to hide.
     * -I used my own approach in ParseInput but accomplished the same results.
     * -I renamed methods to SaveGame and LoadGame for easier comprehension.
     * -I made SaveGame not overwrite a pre-existing file.
     * -I prevented overwriting a saved game file in the SaveGame method.
     * -I wrapped JSON deserialization in a try/catch block in the LoadGame method
     *  to return custom feedback/update messages if certain problems occur.
     * -I moved the code for loading a saved game from an acceptable JSON file to another method
     *  to separate it from the code for evaluating whether JSON file is acceptable.
     * -I created a private variable to store the full file name in SaveGame and LoadGame.
     * -I set GameController properties appropriately based on my class design in LoadGame method.
     * -I changed some feedback/update messages for easier reading.
     * -I renamed a variable in Move method for easier comprehension.
     * -I added a method and command to delete a game.
     * -I allowed direction shorthands to be used in the ParseInput method with the move command.
     * -I added a teleport command which can be passed to ParseInput 
     *  to take the user to a random location with hiding place.
     * -I created a property to store a House object.
     * -I made the constructor create a House object and assign it to the House property.
     * -I made the constructor accept a House file name passed in but also provided a default value.
     * -I added/edited comments for easier reading.
     * **/

    public class GameController
    {
        /// <summary>
        /// File system to use for GameController save/load/delete SavedGame files
        /// (should only be changed for testing purposes)
        /// </summary>
        public static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// The player's current location in the house
        /// </summary>
        public Location CurrentLocation { get; private set; }

        public House House { get; private set; }

        /// <summary>
        /// Status of game
        /// </summary>
        public string Status
        {
            get
            {
                // Initialize variable to first part of message for status
                string message = $"You are in the {CurrentLocation.Name}. You see the following exits:";

                // Add each exit's description to the message for status
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

                // If no Opponents have been opponentsFound
                if (FoundOpponents.Count == 0)
                {
                    message += Environment.NewLine + "You have not found any opponents"; // Add info to message for status
                }
                else // if Opponents have been opponentsFound
                {
                    message += Environment.NewLine + $"You have found {FoundOpponents.Count} of {OpponentsAndHidingLocations.Count()} opponent{(OpponentsAndHidingLocations.Count() == 1 ? "" : "s")}"
                            + ": " + String.Join(", ", FoundOpponents); // Add information about Opponents opponentsFound to message
                }

                // Return status message
                return message;
            }
        }

        /// <summary>
        /// A prompt to display to the player
        /// </summary>
        public string Prompt
        {
            get
            {
                return $"{MoveNumber}: Which direction do you want to go{( ( CurrentLocation.GetType() == typeof(LocationWithHidingPlace) ) ? " (or type 'check')" : "")}: ";
            }
        } 

        /// <summary>
        /// The number of moves the player has made
        /// </summary>
        public int MoveNumber { get; private set; } = 1;

        /// <summary>
        /// Opponents and their hiding places
        /// </summary>
        public Dictionary<Opponent, LocationWithHidingPlace> OpponentsAndHidingLocations { get; private set; }
            
        /// <summary>
        /// List of opponents the player has found so far
        /// </summary>
        public List<Opponent> FoundOpponents { get; private set; } = new List<Opponent>();

        /// <summary>
        /// Returns true if the game is over
        /// </summary>
        public bool GameOver => OpponentsAndHidingLocations.Count() == FoundOpponents.Count();

        /// <summary>
        /// Default names used for Opponents when number of Opponents but not names specified in constructor call
        /// </summary>
        public static readonly string[] DefaultOpponentNames = { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy", "Jill" };

        /// <summary>
        /// Create a GameController with an optional specified House file name and 5 Opponents with default names
        /// </summary>
        /// <param name="houseFileName"></param>
        public GameController(string houseFileName = "DefaultHouse")
        {
            SetUpInitialGameWithSpecificOpponentNamesAndHouseFile(DefaultOpponentNames.Take(5).ToArray(), houseFileName);
        }

        /// <summary>
        /// Create a GameController with a specific number of Opponents and an optional specified House file name
        /// </summary>
        /// <param name="numberOfOpponents">Number of Opponents to hide in House</param>
        /// <param name="houseFileName">Name of file from which to load House layout</param>
        public GameController(int numberOfOpponents, string houseFileName = "DefaultHouse")
        {
            // If number of Opponents invalid
            if(numberOfOpponents < 1 || numberOfOpponents > DefaultOpponentNames.Length)
            {
                throw new ArgumentException("Cannot create a new instance of GameController " +
                                                      "because the number of Opponents specified is invalid (must be between 1 and 10)"); // Throw exception
            }

            // Set up initial game with specific Opponent names and House file name
            SetUpInitialGameWithSpecificOpponentNamesAndHouseFile(DefaultOpponentNames.Take(numberOfOpponents).ToArray(), houseFileName);
        }

        /// <summary>
        /// Create a GameController with Opponents with specific names and an optional specified House file name
        /// </summary>
        /// <param name="opponentNames">Names of Opponents to hide in House</param>
        /// <param name="houseFileName">Name of file from which to load House layout</param>
        public GameController(string[] opponentNames, string houseFileName = "DefaultHouse")
        {
            // If no opponent names in array
            if(opponentNames.Length == 0)
            {
                throw new ArgumentException("Cannot create a new instance of GameController because no names for Opponents were passed in"); // Throw exception
            }

            // Set up initial game with specific Opponent names and House file name
            SetUpInitialGameWithSpecificOpponentNamesAndHouseFile(opponentNames, houseFileName);
        }

        /// <summary>
        /// Set up initial game with specific Opponent names and specific House file name
        /// </summary>
        /// <param name="opponentNames">Names of Opponents</param>
        /// <param name="houseFileName">Name of file from which to load House layout</param>
        private void SetUpInitialGameWithSpecificOpponentNamesAndHouseFile(string[] opponentNames, string houseFileName)
        {
            // Set Opponents and hiding locations property to new Dictionary
            OpponentsAndHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();

            // Create Opponents with specified names and add them to dictionary as keys
            foreach (string name in opponentNames)
            {
                OpponentsAndHidingLocations.Add(new Opponent(name), null);
            }

            // Start game with specified House file
            RestartGame(houseFileName);
        }

        /// <summary>
        /// Restart game from beginning in House from specified file
        /// </summary>
        /// <param name="houseFileName">Name of House layout file</param>
        /// <returns>This GameController</returns>
        public GameController RestartGame(string houseFileName)
        {
            House = House.CreateHouse(houseFileName);
            return RestartGame();
        }

        /// <summary>
        /// Restart game from beginning without changing House layout
        /// </summary>
        /// <returns>This GameController</returns>
        public GameController RestartGame()
        {
            FoundOpponents = new List<Opponent>(); // Reset FoundOpponents list
            RehideAllOpponents(); // Hide opponents in random places
            MoveNumber = 1; // Reset move number
            CurrentLocation = House.StartingPoint; // Reset current location
            return this;
        }

        /// <summary>
        /// Rehide all Opponents in specified hiding places
        /// </summary>
        /// <param name="hidingPlaces">Places to hide Opponents</param>
        /// <returns>GameController after Opponents rehidden</returns>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if the number of hiding places is not equal to the number of Opponents</exception>
        private GameController RehideAllOpponents(IEnumerable<LocationWithHidingPlace> hidingPlaces)
        {
            // If the number of hiding places is not equal to the number of Opponents
            if (hidingPlaces.Count() != OpponentsAndHidingLocations.Count())
            {
                throw new ArgumentOutOfRangeException("hidingPlaces", "The number of hiding places must equal the number of opponents."); // Throw exception
            }

            // Clear hiding places
            House.ClearHidingPlaces();

            // Initialize dictionary to store Opponents and new hiding locations
            Dictionary<Opponent, LocationWithHidingPlace> opponentsAndNewHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();

            // Hide Opponents in hiding locations and add hiding locations to dictionary
            for (int i = 0; i < OpponentsAndHidingLocations.Count(); i++)
            {
                hidingPlaces.ElementAt(i).HideOpponent(OpponentsAndHidingLocations.ElementAt(i).Key); // Hide Opponent in hiding location
                opponentsAndNewHidingLocations.Add(OpponentsAndHidingLocations.ElementAt(i).Key, hidingPlaces.ElementAt(i)); // Add hiding location to dictionary
            }

            // Set Opponents and hiding locations dictionary to dictionary with new hiding locations
            OpponentsAndHidingLocations = opponentsAndNewHidingLocations;

            return this;
        }

        /// <summary>
        /// Rehide all Opponents in specified hiding places
        /// Should only be called from GameController and tests
        /// </summary>
        /// <param name="hidingPlaces">Names of hiding places for Opponents</param>
        /// <returns>GameController after Opponents rehidden</returns>
        public GameController RehideAllOpponents(IEnumerable<string> hidingPlaces)
        {
            // Initialize variable for LocationWithHidingPlace objects to empty list
            List<LocationWithHidingPlace> hidingPlacesAsObjects = new List<LocationWithHidingPlace>();

            // Get each LocationWithHidingPlace object and add it to list
            foreach(string hidingPlace in hidingPlaces)
            {
                hidingPlacesAsObjects.Add(House.GetLocationWithHidingPlaceByName(hidingPlace));
            }

            // Rehide all Opponents in LocationWithHidingPlaces
            return RehideAllOpponents(hidingPlacesAsObjects);
        }

        /// <summary>
        /// Rehide all Opponents in random hiding places
        /// </summary>
        /// <returns>GameController after Opponents rehidden</returns>
        private GameController RehideAllOpponents()
        {
            // Initialize list for locations with hiding locations
            List<LocationWithHidingPlace> hidingLocations = new List<LocationWithHidingPlace>();

            // Populate list with random hiding locations (1 per Opponent)
            for(int i = 0; i < OpponentsAndHidingLocations.Count(); i++)
            {
                hidingLocations.Add(House.GetRandomLocationWithHidingPlace());
            }

            // Hide Opponents in hiding places
            return RehideAllOpponents(hidingLocations);
        }

        /// <summary>
        /// Move to the Location in a Direction
        /// </summary>
        /// <param name="direction">The Direction to move</param>
        /// <returns>True if the player can move in that Direction, false otherwise</returns>
        private bool Move(Direction direction)
        {
            Location startLocation = CurrentLocation; // Set start location to current location
            CurrentLocation = CurrentLocation.GetExit(direction); // Set current location to exit returned
            return CurrentLocation != startLocation; // Return whether the current location has changed (whether move was successful)
        }

        /// <summary>
        /// Parse input from the player
        /// </summary>
        /// <param name="input">Input to parse</param>
        /// <returns>The results of parsing the input</returns>
        public string ParseInput(string input)
        {
            // Extract command, one version lowercase and one version original
            string originalCommand = input.Split(" ").FirstOrDefault("");
            string lowercaseCommand = originalCommand.ToLower();

            // If input requests the current location be checked for hiding opponents
            if (lowercaseCommand == "check")
            {
                MoveNumber++; // Increment move number
                return CheckCurrentLocation(); // Check current location and return results
            }
            else if (lowercaseCommand == "teleport")
            {
                MoveNumber++; // Increment move number
                return Teleport(); // Teleport and return message
            }
            else if ( // If input requests save, load, or delete game
                lowercaseCommand == "save" || 
                lowercaseCommand == "load" ||
                lowercaseCommand == "delete" )
            {
                // Get index of first space in input (space after command and before name of file)
                int indexOfSpace = input.IndexOf(' ');

                // If input includes a space
                if (indexOfSpace != -1)
                {
                    // Extract file name
                    string fileName = input.Substring(indexOfSpace + 1);

                    // If file name is valid
                    if(FileSystem.IsValidName(fileName))
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
                            return DeleteGame(fileName); // Delete game and return message
                        }
                    }
                    else // If the file name is invalid
                    {
                        return $"Cannot perform action because file name \"{fileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"; // Return failure message
                    }
                } 
                else // If input does not include a space
                {
                    return "Cannot perform action because no file name was entered"; // Return failure message
                }
            }
            // Try to parse input to Direction
            else if ( !(DirectionExtensions.TryParse(originalCommand, out Direction direction)) ) // If input cannot be parsed to Direction
            {
                return "That's not a valid direction"; // Return invalid direction message
            }
            // Try to move in specified Direction
            else if ( !(Move(direction)) ) // If cannot move in specified Direction
            {
                MoveNumber++; // Increment move number
                return "There's no exit in that direction"; // Return no exit in that direction message
            }
            else // If successfully moved in specified Direction
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
                List<Opponent> opponentsFound = location.CheckHidingPlace(); // Check hiding place and initialize list to Opponents found

                // If any Opponents were found in the hiding place
                if (opponentsFound.Count >= 1)
                {
                    FoundOpponents.AddRange(opponentsFound); // Add Opponents found to list of found opponents
                    return $"You found {opponentsFound.Count} opponent{(opponentsFound.Count == 1 ? "" : "s")} hiding {location.HidingPlace}";
                }
                else // if no Opponents were found in the hiding place
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
        /// Teleport to random location with hiding place
        /// </summary>
        /// <returns>Description</returns>
        private string Teleport()
        {
            CurrentLocation = House.GetRandomLocationWithHidingPlace(); // Set player location to random location with hiding place
            return $"Teleporting to random location with hiding place: {CurrentLocation}"; // Return description
        }

        /// <summary>
        /// Save game to file
        /// </summary>
        /// <param name="fileName">Name of file in which to save game data</param>
        /// <returns>String describing what happened</returns>
        private string SaveGame(string fileName)
        {
            // Get full file name including extension
            string fullFileName = FileSystem.GetFullFileNameForJson(fileName);

            // If file already exists
            if (FileSystem.File.Exists(fullFileName))
            {
                return $"Cannot perform action because a file named {fileName} already exists"; // Return error message
            }

            // Create dictionary of Opponents and hiding locations as strings
            Dictionary<string, string> opponentsAndHidingPlacesAsStrings = new Dictionary<string, string>();
            foreach(KeyValuePair<Opponent, LocationWithHidingPlace> kvp in OpponentsAndHidingLocations)
            {
                opponentsAndHidingPlacesAsStrings.Add(kvp.Key.ToString(), kvp.Value.ToString());
            }

            // Initialize SavedGame object to store game state data
            SavedGame savedGame = new SavedGame(House, House.HouseFileName, CurrentLocation.ToString(), 
                                                MoveNumber, opponentsAndHidingPlacesAsStrings, 
                                                FoundOpponents.Select((o) => o.Name));
            
            // Save game as JSON to file and return success message
            string savedGameAsJSON = JsonSerializer.Serialize(savedGame); // Convert game's state data to JSON
            FileSystem.File.WriteAllText(fullFileName, savedGameAsJSON); // Save game's state data in file
            return $"Game successfully saved in {fileName}"; // Return success message
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="fileName">Name of file from which to load game data</param>
        /// <returns>String describing what happened</returns>
        private string LoadGame(string fileName)
        {
            // Get full file name including extension
            string fullFileName = FileSystem.GetFullFileNameForJson(fileName);

            // If file does not exist
            if( !(FileSystem.File.Exists(fullFileName)) )
            {
                return $"Cannot load game because file {fileName} does not exist"; // Return error message
            }

            // Read text from file
            string fileText = FileSystem.File.ReadAllText(fullFileName);

            // Declare variable to store SavedGame object
            SavedGame savedGame;

            // Deserialize text from file into SavedGame object
            try
            {
                savedGame = JsonSerializer.Deserialize<SavedGame>(fileText);
            } 
            catch(NullReferenceException e)
            {
                // If SavedGame House has not been set
                if(e.Message == "House has not been set")
                {
                    // Return custom message
                    return $"Cannot process because data is corrupt - JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: HouseFileName";
                }
                else // if other NullReferenceException
                {
                    throw; // Bubble up exception
                }
            }
            catch(FileNotFoundException e) // If House file not found
            {
                return e.Message; // Return error message
            }
            catch(Exception e)
            {
                // If problem due to invalid property value, JSON format issue, or an invalid operation
                if(e is InvalidDataException || e is JsonException || e is InvalidOperationException)
                {
                    return $"Cannot process because data is corrupt - {e.Message}"; // Return error message
                }
                else // If any other exception
                {
                    throw; // Bubble up exception
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
            OpponentsAndHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();
            foreach(KeyValuePair<string, string> kvp in savedGame.OpponentsAndHidingLocations)
            {
                OpponentsAndHidingLocations.Add(new Opponent(kvp.Key), House.GetLocationWithHidingPlaceByName(kvp.Value));
            }

            // Restore list of found opponents
            FoundOpponents.Clear(); // Clear list of found Opponents
            foreach(String opponent in savedGame.FoundOpponents) // For each found Opponent
            {
                FoundOpponents.Add(OpponentsAndHidingLocations.Keys.First((x) => x.Name == opponent)); // Add Opponent object with matching name to FoundOpponents list
            }

            // Clear hiding places in the House
            House.ClearHidingPlaces();

            // Initialize to dictionary of unfound Opponents who should still be hidden and their hiding locations
            IDictionary<Opponent, LocationWithHidingPlace> unfoundOpponentsAndHidingLocations = 
                        OpponentsAndHidingLocations.Where((kvp) => !(FoundOpponents.Contains(kvp.Key))).ToDictionary();

            // Hide each unfound Opponent in their hiding location
            foreach(KeyValuePair<Opponent, LocationWithHidingPlace> kvp in unfoundOpponentsAndHidingLocations)
            {
                kvp.Value.HideOpponent(kvp.Key);
            }
        }

        /// <summary>
        /// Delete game file
        /// </summary>
        /// <param name="fileName">Name of file to delete</param>
        /// <returns>String describing what happened</returns>
        private string DeleteGame(string fileName)
        {
            // Get full file name including extension
            string fullFileName = FileSystem.GetFullFileNameForJson(fileName);

            // If file does not exist
            if ( !(FileSystem.File.Exists(fullFileName)) )
            {
                return $"Could not delete game because file {fileName} does not exist"; // Return error message
            }

            // Delete file
            FileSystem.File.Delete(fullFileName);

            // Return success message
            return $"Game file {fileName} has been successfully deleted";
        }
    }
}