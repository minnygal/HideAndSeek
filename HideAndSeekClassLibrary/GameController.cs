﻿using System.IO.Abstractions;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// Class to control a game of hide and seek,
    /// allowing external interaction with and direction of game
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
     * -There MUST be a valid DefaultHouse.house.json file available
     *  to call a constructor without specifying the House file name
     *  (with the exception of the constructor accepting SavedGame).
     *  This is because the default value for the House file name is DefaultHouse.house.json.
     * -You can start a new game w/o having to create a new GameController instance.
     * **/

    /** CHANGES
     * -I moved the ParseInput method to Console app.
     * -I added a restart method so the game can be restarted without creating a new GameController.
     * -I added methods to rehide all opponents.
     * -I made the list of found opponents public.
     * -I added a file system class variable for testing purposes.
     * -I added constructors accepting specific names for opponents.
     * -I added constructors accepting the number of opponents (between 1 and 10) to hide.
     * -I added constructors accepting the name of the House file from which to load the House.
     * -I added a constructor accepting a SavedGame object.
     * -I renamed methods to SaveGame and LoadGame for easier comprehension.
     * -I made SaveGame throw an exception instead of overwriting a pre-existing file.
     * -I wrapped JSON deserialization in a try/catch block in the LoadGame method
     *  to return custom feedback/update messages if certain problems occur.
     * -I moved the code for loading a saved game from an acceptable JSON file to another method
     *  to separate it from the code for evaluating whether JSON file is acceptable.
     * -I created a private variable to store the full file name in SaveGame and LoadGame.
     * -I set GameController properties appropriately (based on my class design) in the LoadGame method.
     * -I changed some feedback/update messages for easier reading.
     * -I renamed a variable in Move method for easier comprehension.
     * -I added a method to delete a game.
     * -I allowed direction shorthands to be used in the Move method.
     * -I added a teleport method to take the user to a random location with hiding place.
     * -I created a property to store a House object.
     * -I made constructors create a House object and assign it to the House property.
     * -I added/edited comments for easier reading.
     * **/

    public class GameController : IGameController
    {
        #region Constructors

        /// <summary>
        /// Default number of Opponents to start the game with (unless number, names, or Opponent objects specified in constructor)
        /// </summary>
        public static readonly int DefaultNumberOfOpponents = 5;

        /// <summary>
        /// Create a GameController and load a specific saved game
        /// </summary>
        /// <param name="savedGame">SavedGame object from which to load game</param>
        public GameController(SavedGame savedGame)
        {
            // Load game from SavedGame object
            LoadGame(savedGame);
        }

        /// <summary>
        /// Create a GameController with an optional specified House file name and 5 Opponents with default names
        /// </summary>
        /// <param name="houseFileNameWithoutEnding">Name of House layout file without House file ending or JSON extension</param>
        /// <exception cref="ArgumentException">Exception thrown if file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public GameController(string houseFileNameWithoutEnding = "DefaultHouse") : this(DefaultNumberOfOpponents, houseFileNameWithoutEnding) { }

        /// <summary>
        /// Create a GameController with a specific number of Opponents and an optional specified House file name
        /// </summary>
        /// <param name="numberOfOpponents">Number of Opponents to hide in House</param>
        /// <param name="houseFileNameWithoutEnding">Name of House layout file without House file ending or JSON extension</param>
        /// <exception cref="ArgumentException">Exception thrown when number of Opponents is invalid, or file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public GameController(int numberOfOpponents, string houseFileNameWithoutEnding = "DefaultHouse")
        {
            // Set Opponents and start game with specified House file name
            SetUpOpponentsInitially(numberOfOpponents); // Set Opponents
            RestartGame(houseFileNameWithoutEnding); // Start game with specified House file
        }

        /// <summary>
        /// Create a GameController with Opponents with specific names and an optional specified House file name
        /// </summary>
        /// <param name="opponentNames">Names of Opponents to hide in House</param>
        /// <param name="houseFileNameWithoutEnding">Name of House layout file without House file ending or JSON extension</param>
        /// <exception cref="ArgumentException">Exception thrown when no names for Opponents were passed in, or file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public GameController(string[] opponentNames, string houseFileNameWithoutEnding = "DefaultHouse")
        {
            // Set Opponents and start game with specified House file name
            SetUpOpponentsInitially(opponentNames); // Set Opponents
            RestartGame(houseFileNameWithoutEnding); // Start game with specified House file
        }

        /// <summary>
        /// Create a GameController with specific Opponents and an optional specified House file name
        /// </summary>
        /// <param name="opponents">Opponents to hide in House</param>
        /// <param name="houseFileNameWithoutEnding">Name of House layout file without House file ending or JSON extension</param>
        /// <exception cref="ArgumentException">Exception thrown when no names for Opponents were passed in, or file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public GameController(Opponent[] opponents, string houseFileNameWithoutEnding = "DefaultHouse")
        {
            // Set up initial game with specific Opponents and House file name
            SetUpOpponentsInitially(opponents.ToList()); // Set Opponents
            RestartGame(houseFileNameWithoutEnding); // Start game with specified House file
        }

        /// <summary>
        /// Create a GameController with an optional specified House and 5 Opponents with default names
        /// </summary>
        /// <param name="house">House in which to play game</param>
        /// <exception cref="ArgumentNullException">Exception thrown if House is null</exception>
        public GameController(House house) : this(DefaultNumberOfOpponents, house) { }

        /// <summary>
        /// Create a GameController with a specific number of Opponents and a specific House
        /// </summary>
        /// <param name="numberOfOpponents">Number of Opponents to hide in House</param>
        /// <param name="house">House in which to play game</param>
        /// <exception cref="ArgumentException">Exception thrown when number of Opponents is invalid</exception>
        /// <exception cref="ArgumentNullException">Exception thrown if House is null</exception>
        public GameController(int numberOfOpponents, House house)
        {
            // Set Opponents and start game with specified House
            SetUpOpponentsInitially(numberOfOpponents); // Set Opponents
            RestartGame(house); // Start game with House passed in
        }

        /// <summary>
        /// Create a GameController with Opponents with specific names and a specific House
        /// </summary>
        /// <param name="opponentNames">Names of Opponents to hide in House</param>
        /// <param name="house">House in which to play game</param>
        /// <exception cref="ArgumentException">Exception thrown when no names for Opponents were passed in</exception>
        /// <exception cref="ArgumentNullException">Exception thrown if House is null</exception>
        public GameController(string[] opponentNames, House house)
        {
            // Set Opponents and start game with specified House
            SetUpOpponentsInitially(opponentNames); // Set Opponents
            RestartGame(house); // Start game with House passed in
        }

        /// <summary>
        /// Create a GameController with specific Opponents and a specific House
        /// </summary>
        /// <param name="opponents">Opponents to hide in House</param>
        /// <param name="house">House in which to play game</param>
        /// <exception cref="ArgumentException">Exception thrown when no names for Opponents were passed in</exception>
        /// <exception cref="ArgumentNullException">Exception thrown if House is null</exception>
        public GameController(Opponent[] opponents, House house)
        {
            // Set up initial game with specific Opponent names and specified House
            SetUpOpponentsInitially(opponents.ToList()); // Set Opponents
            RestartGame(house); // Start game with House passed in
        }

        #endregion

        #region Properties

        /// <summary>
        /// File system to use for GameController save/load/delete SavedGame files
        /// (should only be changed for testing purposes)
        /// </summary>
        public static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// Default names used for Opponents 
        /// when number of Opponents but not names specified in constructor call
        /// </summary>
        public static readonly string[] DefaultOpponentNames = ["Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy", "Jill"];

        private House _house;

        /// <summary>
        /// House in which the game is played
        /// </summary>
        public House House
        {
            get
            {
                return _house;
            }
            private set
            {
                // If House is null
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(House), "House cannot be null"); // Throw exception
                }

                // Set backing field
                _house = value;
            }
        }

        /// <summary>
        /// Player's current location in the House
        /// </summary>
        public Location CurrentLocation { get; private set; }

        private int _moveNumber = 1;

        /// <summary>
        /// Number of moves the player has made during this game
        /// </summary>
        public int MoveNumber
        {
            get
            {
                return _moveNumber;
            }
            private set
            {
                // If move number is less than 1
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(MoveNumber), "move number must be greater than or equal to 1"); // Throw exception
                }

                // Set backing field
                _moveNumber = value;
            }
        }

        /// <summary>
        /// Opponents and their hiding locations
        /// </summary>
        public Dictionary<Opponent, LocationWithHidingPlace> OpponentsAndHidingLocations { get; private set; }

        /// <summary>
        /// List of opponents the player has found so far
        /// </summary>
        public List<Opponent> FoundOpponents { get; private set; } = new List<Opponent>();

        /// <summary>
        /// Returns true if the game is over
        /// </summary>
        public bool GameOver => OpponentsAndHidingLocations.Count == FoundOpponents.Count;

        /// <summary>
        /// Status of game
        /// (describing player location, exits, 
        /// hiding place information if applicable, and found opponents)
        /// </summary>
        public string Status
        {
            get
            {
                // Initialize variable to first part of message for status
                string message = $"You are in the {CurrentLocation.Name}. " +
                                 $"You see the following exit{(CurrentLocation.Exits.Count == 1 ? "" : "s")}:";

                // Add each exit's description to the message for status
                foreach (string exitDescription in CurrentLocation.ExitList())
                {
                    message += Environment.NewLine + " - " + exitDescription;
                }

                // Add hiding place info if someone could hide in current location
                if (CurrentLocation.GetType() == typeof(LocationWithHidingPlace))
                {
                    LocationWithHidingPlace location = (LocationWithHidingPlace)CurrentLocation;
                    message += Environment.NewLine + $"Someone could hide {location.HidingPlace}";
                }

                // If no Opponents have been found
                if (FoundOpponents.Count == 0)
                {
                    message += Environment.NewLine + "You have not found any opponents"; // Add info to message for status
                }
                else // if Opponents have been found
                {
                    message += Environment.NewLine + $"You have found {FoundOpponents.Count} of " +
                               $"{OpponentsAndHidingLocations.Count} opponent{(OpponentsAndHidingLocations.Count == 1 ? "" : "s")}" +
                                ": " + String.Join(", ", FoundOpponents.Select((o) => o.Name)); // Add information about found Opponents to message
                }

                // Return status message
                return message;
            }
        }

        /// <summary>
        /// Prompt to display to the player
        /// </summary>
        public string Prompt
        {
            get
            {
                return $"{MoveNumber}: Which direction do you want to go" +
                       $"{((CurrentLocation.GetType() == typeof(LocationWithHidingPlace)) ? " (or type 'check')" : "")}: ";
            }
        }

        #endregion

        #region Initial game setup, restart game, and rehide opponents methods

        /// <summary>
        /// Set Opponents initially with default names
        /// </summary>
        /// <param name="numberOfOpponents">Number of Opponents</param>
        /// <exception cref="ArgumentException">Exception thrown if number of opponents is invalid</exception>
        private void SetUpOpponentsInitially(int numberOfOpponents)
        {
            // If number of Opponents invalid
            if (numberOfOpponents < 1 || numberOfOpponents > DefaultOpponentNames.Length)
            {
                throw new ArgumentException("Cannot create a new instance of GameController " +
                                            "because the number of Opponents specified is invalid (must be between 1 and 10)", nameof(numberOfOpponents)); // Throw exception
            }

            // Set specified number of Opponents with default names
            SetUpOpponentsInitially(DefaultOpponentNames.Take(numberOfOpponents).ToArray()); // Set Opponents
        }

        /// <summary>
        /// Set Opponents initially with custom names
        /// </summary>
        /// <param name="opponentNames">Names of Opponents</param>
        /// <exception cref="ArgumentException">Exception thrown when no names or invalid name for Opponents were passed in</exception>
        private void SetUpOpponentsInitially(string[] opponentNames)
        {
            // If no opponent names in array
            if (opponentNames.Length == 0)
            {
                throw new ArgumentException("Cannot create a new instance of GameController because no names for Opponents were passed in", nameof(opponentNames)); // Throw exception
            }

            // Create list of Opponents with specified names
            List<Opponent> opponents = new List<Opponent>();
            try
            {
                foreach (string name in opponentNames)
                {
                    // If name is null
                    if(name == null)
                    {
                        throw new ArgumentNullException(nameof(opponentNames), 
                                                        "Cannot create a new instance of GameController because opponent name passed in was null");
                    }

                    // Create new Opponent and add to list
                    opponents.Add(new Opponent(name));
                }
            }
            catch(ArgumentNullException)
            {
                throw; // Rethrow exception
            }
            catch(ArgumentException e)
            {
                throw new ArgumentException($"Cannot create a new instance of GameController because {e.Message}", nameof(opponentNames)); // Throw exception
            }
            catch(Exception e)
            {
                throw new Exception($"Cannot create a new instance of GameController because {e.Message}"); // Throw exception
            }

            // Set Opponents with custom names
            SetUpOpponentsInitially(opponents);
        }

        /// <summary>
        /// Set Opponents initially
        /// </summary>
        /// <param name="opponents">Opponents</param>
        private void SetUpOpponentsInitially(List<Opponent> opponents)
        {
            // If no Opponents in list
            if (opponents.Count == 0)
            {
                throw new ArgumentException("Cannot create a new instance of GameController because no Opponents were passed in", nameof(opponents)); // Throw exception
            }

            // Set Opponents and hiding locations property to new Dictionary
            OpponentsAndHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();

            // Add Opponents to dictionary as keys
            foreach (Opponent opponent in opponents)
            {
                // If Opponent is null
                if(opponent == null)
                {
                    throw new ArgumentNullException(nameof(opponents), 
                                                    "Cannot create a new instance of GameController because null Opponent was passed in"); // Throw exception
                }

                // Add Opponent to dictionary with null hiding location
                OpponentsAndHidingLocations.Add(opponent, null);
            }
        }

        /// <summary>
        /// Restart game from beginning in House loaded from specific House layout file
        /// </summary>
        /// <param name="houseFileNameWithoutEnding">Name of House layout file without House file ending or JSON extension</param>
        /// <returns>This GameController</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public IGameController RestartGame(string houseFileNameWithoutEnding)
        {
            House = House.CreateHouse(houseFileNameWithoutEnding); // Set House property to House created from specified House layout file
            return RestartGame(); // Restart the game
        }

        /// <summary>
        /// Restart game from beginning in House passed in
        /// </summary>
        /// <param name="house">House in which to play game</param>
        /// <returns>This GameController</returns>
        public IGameController RestartGame(House house)
        {
            House = house; // Set House property
            return RestartGame(); // Restart game
        }

        /// <summary>
        /// Restart game from beginning with opponents hidden in specified hiding locations
        /// </summary>
        /// <param name="hidingLocations">Names of hiding locations for opponents (1 per opponent)</param>
        /// <returns>This GameController</returns>
        /// <exception cref="ArgumentException">Exception thrown if hiding places is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if the number of hiding places is not equal to the number of Opponents</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if a location with hiding place is not in the House</exception>
        public IGameController RestartGame(IEnumerable<string>hidingLocations)
        {
            // Rehide all Opponents in specified hiding locations
            RehideAllOpponents(hidingLocations);

            // Reset properties
            return ResetPropertiesToRestartGame();
        }

        /// <summary>
        /// Restart game from beginning without changing House layout
        /// </summary>
        /// <returns>This GameController</returns>
        public IGameController RestartGame()
        {
            RehideAllOpponents(); // Hide opponents in random places
            return ResetPropertiesToRestartGame(); // Reset properties
        }

        /// <summary>
        /// Reset properties to restart game (does not hide opponents)
        /// </summary>
        /// <returns>This GameController</returns>
        private GameController ResetPropertiesToRestartGame()
        {
            FoundOpponents = new List<Opponent>(); // Set FoundOpponents to empty list
            MoveNumber = 1; // Reset move number
            CurrentLocation = House.StartingPoint; // Reset current location
            return this;
        }

        /// <summary>
        /// Rehide all Opponents in specified hiding locations
        /// </summary>
        /// <param name="hidingLocations">Names of locations with hiding places to hide Opponents</param>
        /// <returns>GameController after Opponents rehidden</returns>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if the number of hiding locations is not equal to the number of Opponents</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if no matching location with hiding location in House</exception>
        private GameController RehideAllOpponents(IEnumerable<string> hidingLocations)
        {
            // Initialize variable for LocationWithHidingPlace objects to empty list
            List<LocationWithHidingPlace> hidingLocationsAsObjects = new List<LocationWithHidingPlace>();

            // Get each LocationWithHidingPlace object and add it to list
            foreach (string hidingLocation in hidingLocations)
            {
                hidingLocationsAsObjects.Add(House.GetLocationWithHidingPlaceByName(hidingLocation));
            }

            // Rehide all Opponents in LocationWithHidingPlaces
            return RehideAllOpponents(hidingLocationsAsObjects);
        }

        /// <summary>
        /// Rehide all Opponents in specified hiding locations
        /// </summary>
        /// <param name="hidingPlaces">Locations with hiding places to hide Opponents</param>
        /// <returns>GameController after Opponents rehidden</returns>
        /// <exception cref="ArgumentOutOfRangeException">Exception thrown if the number of hiding places is not equal to the number of Opponents</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if a location with hiding place is not in the House</exception>
        private GameController RehideAllOpponents(IEnumerable<LocationWithHidingPlace> hidingLocations)
        {
            // If the number of hiding locations is not equal to the number of Opponents
            if (hidingLocations.Count() != OpponentsAndHidingLocations.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(hidingLocations), "The number of hiding places must equal the number of opponents"); // Throw exception
            }

            // If any hiding location is not in the House
            foreach(LocationWithHidingPlace hidingLocation in hidingLocations)
            {
                // If the hiding place is not in the House
                if( !(House.LocationsWithHidingPlaces.Contains(hidingLocation)) )
                {
                    throw new InvalidOperationException($"Cannot rehide opponents because {hidingLocation} is not in the House"); // Throw exception
                }
            }

            // Clear hiding places
            House.ClearHidingPlaces();

            // Initialize dictionary to store Opponents and new hiding locations
            Dictionary<Opponent, LocationWithHidingPlace> opponentsAndNewHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();

            // Hide Opponents in hiding locations and add hiding locations to dictionary
            for (int i = 0; i < OpponentsAndHidingLocations.Count; i++)
            {
                hidingLocations.ElementAt(i).HideOpponent(OpponentsAndHidingLocations.ElementAt(i).Key); // Hide Opponent in hiding location
                opponentsAndNewHidingLocations.Add(OpponentsAndHidingLocations.ElementAt(i).Key, hidingLocations.ElementAt(i)); // Add hiding location to dictionary
            }

            // Set Opponents and hiding locations dictionary to dictionary with new hiding locations
            OpponentsAndHidingLocations = opponentsAndNewHidingLocations;

            // Return GameController
            return this;
        }

        /// <summary>
        /// Rehide all Opponents in random hiding places
        /// </summary>
        /// <returns>GameController after Opponents rehidden</returns>
        private GameController RehideAllOpponents()
        {
            // Initialize list for locations with hiding places
            List<LocationWithHidingPlace> hidingLocations = new List<LocationWithHidingPlace>();

            // Populate list with random hiding locations (1 per Opponent)
            for (int i = 0; i < OpponentsAndHidingLocations.Count; i++)
            {
                hidingLocations.Add(House.GetRandomLocationWithHidingPlace());
            }

            // Hide Opponents in hiding places
            return RehideAllOpponents(hidingLocations);
        }

        #endregion

        #region Player action methods

        /// <summary>
        /// Move to the Location in a Direction
        /// (increments move number unless there is no exit in specified Direction)
        /// </summary>
        /// <param name="direction">The Direction in which to move</param>
        /// <returns>Description of player action</returns>
        /// <exception cref="InvalidOperationException">Exception thrown when no exit in specified Direction</exception>
        public string Move(Direction direction)
        {
            // Attempt to move in specified direction
            CurrentLocation = CurrentLocation.GetExit(direction); // Throws exception if no exit in specified Direction

            // Increment move number
            MoveNumber++;

            // Return description
            return $"Moving {direction}";
        }

        /// <summary>
        /// Move to the Location in a direction
        /// (increments move number unless there is no exit in specified Direction)
        /// </summary>
        /// <param name="direction">The direction to move</param>
        /// <returns>Description of player action</returns>
        /// <exception cref="InvalidOperationException">Exception thrown when no exit in specified Direction</exception>
        /// <exception cref="ArgumentException">Exception thrown if direction is invalid</exception>
        public string Move(string direction)
        {
            return Move(DirectionExtensions.Parse(direction));
        }

        /// <summary>
        /// Helper method to check current location for opponents
        /// (increments move number unless there is no hiding place)
        /// </summary>
        /// <returns>The results of checking the location</returns>
        /// <exception cref="InvalidOperationException">Exception thrown if no hiding place in current location</exception>
        public string CheckCurrentLocation()
        {
            // If current location does not have a hiding place
            if (!(CurrentLocation.GetType() == typeof(LocationWithHidingPlace)))
            {
                throw new InvalidOperationException($"There is no hiding place in the {CurrentLocation}"); // Throw new exception with custom error message
            }

            // Increment move number
            MoveNumber++;

            // Check hiding place
            LocationWithHidingPlace location = (LocationWithHidingPlace)CurrentLocation; // Convert CurrentLocation to LocationWithHidingPlace
            IList<Opponent> opponentsFound = location.CheckHidingPlace(); // Check hiding place and set variable to Opponents found

            // If any Opponents were found in the hiding place
            if (opponentsFound.Count >= 1)
            {
                FoundOpponents.AddRange(opponentsFound); // Add Opponents found to list of found opponents
                return $"You found {opponentsFound.Count} opponent{(opponentsFound.Count == 1 ? "" : "s")} hiding {location.HidingPlace}"; // Return description
            }
            else // if no Opponents were found in the hiding place
            {
                return $"Nobody was hiding {location.HidingPlace}"; // Return description
            }
        }

        /// <summary>
        /// Teleport to random location with hiding place
        /// (increments move numbber)
        /// </summary>
        /// <returns>Description of player action</returns>
        public string Teleport()
        {
            MoveNumber++; // Increment move number
            CurrentLocation = House.GetRandomLocationWithHidingPlace(); // Set player location to random location with hiding place
            return $"Teleporting to random location with hiding place: {CurrentLocation}"; // Return description
        }

        #endregion

        #region Methods for saving, loading, and deleting games

        /// <summary>
        /// Save game to file
        /// </summary>
        /// <param name="fileNameWithoutEnding">Name of file (without saved game file ending or JSON extension) in which to save game data</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if file already exists</exception>
        public string SaveGame(string fileNameWithoutEnding)
        {
            // Get full file name including saved game file ending and JSON extension
            string fullFileName = SavedGame.GetFullSavedGameFileName(fileNameWithoutEnding); // Throws exception if file name is invalid

            // If file already exists
            if (FileSystem.File.Exists(fullFileName))
            {
                throw new InvalidOperationException($"Cannot perform action because a file named {fileNameWithoutEnding} already exists"); // Throw new exception with custom error message
            }

            // Create dictionary of Opponents and hiding locations as strings
            Dictionary<string, string> opponentsAndHidingPlacesAsStrings = new Dictionary<string, string>();
            foreach (KeyValuePair<Opponent, LocationWithHidingPlace> kvp in OpponentsAndHidingLocations)
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
            return $"Game successfully saved in {fileNameWithoutEnding}"; // Return success message
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="fileNameWithoutEnding">Name of file (without saved game file ending or JSON extension) from which to load game data</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name or value in file is invalid</exception>
        /// <exception cref="JsonException">Exception thrown if JSON formatting issue</exception>
        /// <exception cref="InvalidOperationException">Exception thrown if invalid operation attempted</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if saved game file not found</exception>
        /// <exception cref="NullReferenceException">Exception thrown if a reference is null</exception>
        public string LoadGame(string fileNameWithoutEnding)
        {
            // Get full file name including saved game file ending and JSON extension
            string fullFileName = SavedGame.GetFullSavedGameFileName(fileNameWithoutEnding); // Throws exception if file name is invalid

            // If file does not exist
            if( !(FileSystem.File.Exists(fullFileName)) )
            {
                throw new FileNotFoundException($"Cannot load game because file {fileNameWithoutEnding} does not exist"); // Throw exception with custom message
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
            catch (JsonException e) // If problem due to JSON formatting issue
            {
                throw new JsonException($"Cannot process because data is corrupt - {e.Message}"); // Throw new exception with custom error message
            }
            catch (InvalidOperationException e) // If problem due to attempted invalid operation
            {
                throw new InvalidOperationException($"Cannot process because data is corrupt - {e.Message}"); // Throw new exception with custom error message
            }
            catch (ArgumentOutOfRangeException e) // If problem due to value out of required range (e.g. MoveNumber not positive)
            {
                throw new ArgumentOutOfRangeException(e.ParamName, $"Cannot process because data is corrupt - {e.Message}"); // Throw new exception with custom error message
            }
            catch (ArgumentException e) // If problem due to invalid argument (e.g. House file name is invalid)
            {
                throw new ArgumentException($"Cannot process because data is corrupt - {e.Message}", e.ParamName); // Throw new exception with custom error message
            }
            catch (NullReferenceException e)
            {
                if (e.Message == "House has not been set") // If SavedGame House property has not been set
                {
                    throw new JsonException("Cannot process because data is corrupt - " +
                                            "JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, " +
                                            "including the following: HouseFileName"); // Throw exception with custom error message
                }
                else // if other NullReferenceException
                {
                    throw; // Bubble up exception
                }
            }
            catch (Exception)
            {
                throw; // Bubble up exception
            }

            // Load game from SavedGame object
            LoadGame(savedGame);

            // Return success message
            return $"Game successfully loaded from {fileNameWithoutEnding}";
        }

        /// <summary>
        /// Load game from SavedGame object
        /// </summary>
        /// <param name="savedGame">SavedGame object from which to load game</param>
        /// <returns>This GameController</returns>
        public GameController LoadGame(SavedGame savedGame)
        {
            // Set GameController House
            House = savedGame.House;

            // Set current location
            CurrentLocation = House.GetLocationByName(savedGame.PlayerLocation);

            // Set move number
            MoveNumber = savedGame.MoveNumber;

            // Create new Opponent objects and restore dictionary of Opponents and their hiding locationsn
            OpponentsAndHidingLocations = new Dictionary<Opponent, LocationWithHidingPlace>();
            foreach (KeyValuePair<string, string> kvp in savedGame.OpponentsAndHidingLocations)
            {
                OpponentsAndHidingLocations.Add(new Opponent(kvp.Key), House.GetLocationWithHidingPlaceByName(kvp.Value));
            }

            // Restore list of found opponents
            FoundOpponents.Clear(); // Clear list of found Opponents
            foreach (string opponent in savedGame.FoundOpponents) // For each found Opponent
            {
                FoundOpponents.Add(OpponentsAndHidingLocations.Keys.First((x) => x.Name == opponent)); // Add Opponent object with matching name to FoundOpponents list
            }

            // Clear hiding places in the House
            House.ClearHidingPlaces();

            // Initialize to dictionary of unfound Opponents who should still be hidden and their hiding locations
            IDictionary<Opponent, LocationWithHidingPlace> unfoundOpponentsAndHidingLocations =
                        OpponentsAndHidingLocations.Where((kvp) => !(FoundOpponents.Contains(kvp.Key))).ToDictionary();

            // Hide each unfound Opponent in their hiding location
            foreach (KeyValuePair<Opponent, LocationWithHidingPlace> kvp in unfoundOpponentsAndHidingLocations)
            {
                kvp.Value.HideOpponent(kvp.Key);
            }

            // Return this GameController to allow daisy-chaining
            return this;
        }

        /// <summary>
        /// Delete game file
        /// </summary>
        /// <param name="fileNameWithoutEnding">Name of file to delete without saved game file ending or JSON extension</param>
        /// <returns>String describing what happened</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        /// <exception cref="FileNotFoundException">Exception thrown if file not found</exception>
        public string DeleteGame(string fileNameWithoutEnding)
        {
            // Get full file name including saved game file ending and JSON extension
            string fullFileName = SavedGame.GetFullSavedGameFileName(fileNameWithoutEnding); // Throws exception if file name is invalid

            // If file does not exist
            if( !(FileSystem.File.Exists(fullFileName)) )
            {
                throw new FileNotFoundException($"Could not delete game because file {fileNameWithoutEnding} does not exist"); // Throw new exception with custom error message
            }

            // Delete file
            FileSystem.File.Delete(fullFileName);

            // Return success message
            return $"Game file {fileNameWithoutEnding} has been successfully deleted";
        }

        #endregion
    }
}