using System;

namespace HideAndSeek
{
    /// <summary>
    /// Class to run a Console interactive game of HideAndSeek
    /// Outputs game information to Console and accepts user input via Console
    /// CREDIT: adapted from Stellman and Greene's code
    /// </summary>

    /** CREDIT
     *  adapted from HideAndSeek project's Program class
     *  © 2023 Andrew Stellman and Jennifer Greene
     *         Published under the MIT License
     *         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_1/HideAndSeek/Program.cs
     *         Link valid as of 02-25-2025
     * **/

    /** CHANGES
     * -I added instructions to be printed for the user when they launch the program.
     * -I allow the user to select the number or names of opponents.
     * -I display available House layout file names and allow the user to enter a House layout name 
     *  or use the default House layout.
     * -I display available SavedGame file names.
     * -I added comments to make the code more readable.
     * -I made code conform to my formatting.
     * -I added code to print an empty line to put space between moves information.
     * **/
    public class HideAndSeekConsoleRunner
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public HideAndSeekConsoleRunner() : this(new GameController(), new ConsoleIOAdapter()) { }

        /// <summary>
        /// Constructor accepting game controller and console IO (for testing)
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="consoleIO">Console in/out to use</param>
        public HideAndSeekConsoleRunner(IGameController gameController, IConsoleIO consoleIO) 
            : this(gameController, consoleIO, new GetFileNamesAdapter()) { }

        /// <summary>
        /// Constructor accepting game controller, console IO, and file names adapter (for testing)
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="consoleInOut">Console in/out to use</param>
        /// <param name="fileNamesAdapter">GetFileNames adapter</param>
        public HideAndSeekConsoleRunner(IGameController gameController, IConsoleIO consoleInOut, IGetFileNamesAdapter fileNamesAdapter) 
            : this(gameController, consoleInOut, fileNamesAdapter, new GameControllerFactory()) { }

        /// <summary>
        /// Constructor accepting game controller, console IO, and game controller factory (for testing)
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="consoleInOut">Console in/out to use</param>
        /// <param name="gcFactory">Game controller factory to use</param>
        public HideAndSeekConsoleRunner(IGameController gameController, IConsoleIO consoleInOut, IGameControllerFactory gcFactory)
            : this(gameController, consoleInOut, new GetFileNamesAdapter(), gcFactory) { }

        /// <summary>
        /// Constructor accepting game controller, console IO, file names adapter, and game controller factory (for testing)
        /// </summary>
        /// <param name="gameController">Game controller to use</param>
        /// <param name="consoleInOut">Console in/out to use</param>
        /// <param name="fileNamesAdapter">GetFileNames adapter</param>
        /// <param name="gcFactory">Game controller factory to use</param>
        public HideAndSeekConsoleRunner(
            IGameController gameController, IConsoleIO consoleInOut, IGetFileNamesAdapter fileNamesAdapter, IGameControllerFactory gcFactory)
        {
            GameController = gameController; // Set game controller to passed in game controller
            consoleIO = consoleInOut; // Set console IO to passed in console IO
            getFileNamesAdapter = fileNamesAdapter; // Set file names adapter to passed in file names adapter
            gameControllerFactory = gcFactory; // Set game controller factory to passed in game controller factory
        }

        /// <summary>
        /// GameController object to control game
        /// </summary>
        public IGameController GameController { get; private set; }

        /// <summary>
        /// Command to quit program
        /// </summary>
        public const string QuitCommand = "exit";

        // Console input/output object
        private readonly IConsoleIO consoleIO;

        // Get file names adapter
        private readonly IGetFileNamesAdapter getFileNamesAdapter;

        // Game controller factory (for GameController creation) when new command called
        private readonly IGameControllerFactory gameControllerFactory;

        /// <summary>
        /// Method to run the game,
        /// outputting game information to Console and accepting user input from Console
        /// </summary>
        public void RunGame()
        {
            // Declare variable to store return value of ParseInput method
            string parseInputReturnValue;

            // Print welcome, basic instructions, and space
            consoleIO.WriteLine( GetWelcomeAndInstructions() + Environment.NewLine );

            // Until the user quits the program
            while(true)
            {
                // Welcome user to the House
                consoleIO.WriteLine(GetWelcomeToHouse());

                // Until game is over
                while( !(GameController.GameOver) )
                {
                    // Print game status and prompt, and get/parse/act on user input
                    consoleIO.WriteLine(GameController.Status); // Print game status
                    consoleIO.Write(GameController.Prompt); // Print game prompt
                    parseInputReturnValue = ParseInput(consoleIO.ReadLine()); // Get user input, parse input, act on input, and set variable to return message

                    // If user entered quit command
                    if(parseInputReturnValue == QuitCommand)
                    {
                        return; // Quit program
                    }

                    // Print update
                    consoleIO.WriteLine(RemoveParamText(parseInputReturnValue)); // Print ParseInput return message without param text
                    consoleIO.WriteLine(); // Print empty line to put space between actions
                }

                // Print post-game info and instructions
                consoleIO.WriteLine($"You won the game in {GameController.MoveNumber} moves!");
                consoleIO.Write("Press P to play again, any other key to quit. ");

                // If user does not want to play again
                if( !(consoleIO.ReadKey().KeyChar.ToString().Equals("P", StringComparison.CurrentCultureIgnoreCase)) )
                {
                    return; // Quit program
                }

                // If user does want to play again, restart game and print space
                GameController.RestartGame();
                consoleIO.WriteLine();
            }
        }

        /// <summary>
        /// Parse input from the player
        /// Accepts case-insensitive commands for directions to/for move,
        /// teleport, check, new, save, load, delete
        /// </summary>
        /// <param name="userInput">User input to parse</param>
        /// <returns>Message describing action or error OR "quit" if user wants to quit game</returns>
        private string ParseInput(string userInput)
        {
            // Trim input
            userInput = userInput.Trim();

            // Extract command (text before first space) and make lowercase
            string lowercaseCommand = userInput.Split(" ").FirstOrDefault("").ToLower();
            
            // Extract text after command (may include file name)
            string textAfterCommand = userInput.Substring(lowercaseCommand.Length);

            try
            {
                // Evaluate command and act accordingly
                switch(lowercaseCommand)
                {
                    // Check current location and return description
                    case "check":
                        return GameController.CheckCurrentLocation();

                    // Teleport and return description
                    case "teleport": 
                        return GameController.Teleport();

                    // Save game and return description
                    case "save": 
                        return SaveGame(textAfterCommand);

                    // Load game and return description (or return "quit" command if user wants to quit game)
                    case "load":
                        return PerformActionWithSavedGameFileNameOrAcceptQuitCommand(textAfterCommand, lowercaseCommand,
                            (string fileName) =>
                            {
                                return GameController.LoadGame(fileName) +
                                        Environment.NewLine + GetWelcomeToHouse(); // Return results from loading game plus welcome message
                            });

                    // Delete game and return description (or return "quit" command if user wants to quit game)
                    case "delete":
                        return PerformActionWithSavedGameFileNameOrAcceptQuitCommand(textAfterCommand, lowercaseCommand,
                            (string fileName) =>
                            {
                                return GameController.DeleteGame(fileName); // Return results from deleting game
                            });

                    // Start new game and return description (or "quit" command if user wants to quit game)
                    case "new":
                        GameController = GetGameControllerWithUserSpecifiedOpponents(); // Set game controller to new game with opponents specified via user input and default house                                                        
                        GameController = SetHouseLayoutBasedOnUserInput(GameController); // Set game controller House layout based on user input or leave House layout unchanged (returns null if user wants to quit)
                        return GameController == null ? QuitCommand : "New game started" + Environment.NewLine + GetWelcomeToHouse(); // If null, return quit command; otherwise return new game message and welcome

                    // Return quit command since user wants to quit game
                    case QuitCommand:
                        return QuitCommand;

                    // Assume direction entered
                    default:
                        return GameController.Move(DirectionExtensions.Parse(lowercaseCommand)); // Move in specified direction and return results
                }
            }
            catch(Exception e)
            {
                return e.Message; // Return exception message
            }
        }

        /// <summary>
        /// Helper method to save current game (trims file name)
        /// </summary>
        /// <param name="fileName">Name of file in which to save game</param>
        /// <returns>Message describing action or error</returns>
        private string SaveGame(string fileName)
        {
            // Trim file name
            fileName = fileName.Trim();

            // If input is empty
            if(string.IsNullOrEmpty(fileName))
            {
                return "Cannot save game because no file name was entered"; // Return failure message
            }
            else if( !(FileExtensions.IsValidName(fileName)) ) // If input is invalid file name
            {
                return "Cannot save game because file name is invalid"; // Return failure message
            }
            else
            {
                return GameController.SaveGame(fileName); // Save game and return message
            }
        }

        /// <summary>
        /// Perform action with SavedGame file (or accept quit command)
        /// If text after command is empty, display list of existing SavedGame files
        /// and get user input for file name.
        /// </summary>
        /// <param name="textAfterCommand">Text after command (if not empty/whitespace, used as file name)</param>
        /// <param name="lowercaseCommand">Action command in lowercase (e.g. "load", "delete")</param>
        /// <param name="ActionWithSavedGameFile">Action to perform with SavedGame file name</param>
        /// <returns>Message describing action or error OR quit command</returns>
        private string PerformActionWithSavedGameFileNameOrAcceptQuitCommand(
            string textAfterCommand, string lowercaseCommand, Func<string, string> ActionWithSavedGameFile)
        {
            // Get list of names of SavedGame files available
            IEnumerable<string> allSavedGameFileNames = getFileNamesAdapter.GetSavedGameFileNames();

            // If no SavedGame files available
            if( !(allSavedGameFileNames.Any()) )
            {
                return $"Cannot {lowercaseCommand} game because no saved game files exist"; // Return error message
            }

            // Set variable for file name to trimmed text after command
            string userInputForFileName = textAfterCommand.Trim();

            // If user input for file name is empty
            if (string.IsNullOrEmpty(userInputForFileName))
            {
                // Display list of names of SavedGame files available
                consoleIO.WriteLine("Here are the names of the saved game files available:" + Environment.NewLine +
                                    GetTextListOfFileNames(allSavedGameFileNames));

                // Get name of SavedGame file to load from user
                consoleIO.Write($"Enter the name of the saved game file to {lowercaseCommand}: "); // Prompt
                userInputForFileName = consoleIO.ReadLine().Trim(); // Set variable for file name to trimmed user input
            }

            // Return quit command, error message, or action results
            if(userInputForFileName == QuitCommand) // If user entered quit command
            {
                return QuitCommand; // Return quit command
            }
            else if( !(FileExtensions.IsValidName(userInputForFileName))) // If file name is invalid
            {
                return $"Cannot {lowercaseCommand} game because file name is invalid"; // Return failure message
            }
            else if( !(allSavedGameFileNames.Contains(userInputForFileName)) ) // If no saved game file with that name exists
            {
                return $"Cannot {lowercaseCommand} game because file does not exist"; // Return failure message
            }
            else
            {
                return ActionWithSavedGameFile(userInputForFileName); // Perform action and return results
            }
        }

        /// <summary>
        /// Helper method to get game controller for new game with opponents specified by user input
        /// and with default house
        /// </summary>
        /// <returns>Game controller set up for game OR null if user wants to quit program</returns>
        private IGameController GetGameControllerWithUserSpecifiedOpponents()
        {
            // Add empty space
            consoleIO.WriteLine();

            while(true) // Continue until return statement
            {
                // Obtain user input for setting opponents
                consoleIO.Write("How many opponents would you like? Enter a number between 1 and 10, or a comma-separated list of names: "); // Prompt
                string userInput = consoleIO.ReadLine().Trim(); // Get user input and trim it

                // If user entered command to quit
                if (userInput == QuitCommand)
                {
                    return null; // Return null to indicate user wants to quit
                }

                // Create local variable to store created game controller
                IGameController gameController;

                try
                {
                    // Create game controller with Opponents set
                    if (userInput == string.Empty) // If user did not enter anything
                    {
                        gameController = gameControllerFactory.GetDefaultGameController(); // Create a new default game controller
                    }
                    else if (int.TryParse(userInput, out int numberOfOpponents)) // If user entered a number
                    {
                        // If number of opponents is out of range
                        if (numberOfOpponents < 1 || numberOfOpponents > gameControllerFactory.MaximumNumberOfOpponentsWithDefaultNames)
                        {
                            throw new ArgumentOutOfRangeException(nameof(numberOfOpponents),
                                $"Cannot create a new game because the number of opponents specified is invalid (must be between 1 and {gameControllerFactory.MaximumNumberOfOpponentsWithDefaultNames})"); // Throw exception
                        }

                        // Create new game controller with specified number of opponents
                        gameController = gameControllerFactory.GetGameControllerWithSpecificNumberOfOpponents(numberOfOpponents);
                    }
                    else // if user did not enter empty string or a number, then assume user entered names for opponents
                    {
                        // Split user input into array of names
                        string[] namesOfOpponents = userInput.Split(',');
                        
                        // For each name
                        for (int i = 0; i < namesOfOpponents.Length; i++)
                        {
                            // Trim name
                            namesOfOpponents[i] = namesOfOpponents[i].Trim();

                            // If name is invalid name for an opponent
                            if( !(gameControllerFactory.IsValidOpponentName(namesOfOpponents[i])) )
                            {
                                throw new ArgumentException($"Cannot create a new game because opponent name \"{namesOfOpponents[i]}\" is invalid (is empty or contains only whitespace)"); // Throw exception
                            }
                        }

                        // Create new game controller with trimmed names
                        gameController = gameControllerFactory.GetGameControllerWithSpecificNamesOfOpponents(namesOfOpponents);
                    }

                    // If no exceptions thrown, return game controller
                    return gameController;
                }
                catch (Exception e)
                {
                    consoleIO.WriteLine(RemoveParamText(e.Message)); // Print error message
                }
            }
        }

        /// <summary>
        /// Helper method to set the House layout for a game controller based on user input
        /// (allows user to enter name of file from which to load House layout;
        /// if user input is empty, does not change game controller's House layout)
        /// </summary>
        /// <returns>Game controller with House layout loaded OR null if user wants to quit program</returns>
        private IGameController SetHouseLayoutBasedOnUserInput(IGameController gameController)
        {
            // If game controller is null (indicating user already wants to quit)
            if(gameController == null)
            {
                return null; // Return null to indicate user wants to quit
            }

            // Set variable to existing house file names
            IEnumerable<string> houseFileNames = getFileNamesAdapter.GetHouseFileNames();

            // Repeat until user input determines House layout or user wants to quit
            while(true)
            {
                // Display names of House layout files available
                consoleIO.WriteLine("Here are the names of the house layout files available:" + Environment.NewLine +
                                  GetTextListOfFileNames(houseFileNames));

                // Get House file name from user
                consoleIO.Write("Type a house layout file name or just press Enter to use the default house layout: "); // Prompt for House layout file name
                string userInputForFileName = consoleIO.ReadLine().Trim(); // Get user input for file name and trim

                // If user entered quit command
                if(userInputForFileName == QuitCommand)
                {
                    return null; // Return null to indicate user wants to quit program
                }
                else if(userInputForFileName == string.Empty) // If user did not enter any text
                {
                    return gameController; // Return game controller without changing House layout
                }
                else if( !(FileExtensions.IsValidName(userInputForFileName)) ) // If user entered invalid file name
                {
                    consoleIO.WriteLine("Cannot load house layout because file name is invalid"); // Print error message
                }
                else if( !(houseFileNames.Contains(userInputForFileName)) ) // If user entered nonexisting file name
                {
                    consoleIO.WriteLine("Cannot load house layout because file does not exist"); // Print error message
                }
                else // If no forseeable issues
                {
                    try
                    {
                        // Restart game with House layout file, throws exception if anything invalid
                        gameController.RestartGame(userInputForFileName);

                        // If no exceptions thrown, return game controller
                        return gameController;
                    }
                    catch (Exception e)
                    {
                        consoleIO.WriteLine(RemoveParamText(e.Message)); // Print exception message without param text
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to return welcome to house message
        /// </summary>
        /// <returns>Welcome to house message</returns>
        private string GetWelcomeToHouse()
        {
            return $"Welcome to {GameController.House.Name}!";
        }

        /// <summary>
        /// Helper method to get app welcome and instructions text
        /// </summary>
        /// <returns>Welcome and instructions text</returns>
        private static string GetWelcomeAndInstructions()
        {
            return "Welcome to the Hide And Seek Console App!" + Environment.NewLine +
                   "Navigate through rooms in a virtual house to find all the hiding opponents in the fewest number of moves possible." + Environment.NewLine +
                   "-To MOVE, enter the direction in which you want to move." + Environment.NewLine +
                   "-To CHECK if any opponents are hiding in your current location, enter \"check\"." + Environment.NewLine +
                   "-To TELEPORT to a random location with a hiding place, enter \"teleport\"." + Environment.NewLine +
                   "-For a NEW custom game, enter \"new\" and follow the prompts." + Environment.NewLine +
                   "-To SAVE your progress, enter \"save\" followed by a space and a name for your game." + Environment.NewLine +
                   "-To LOAD a saved game, enter \"load\" followed by a space and the name of your game." + Environment.NewLine +
                   "-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game." + Environment.NewLine +
                   "-To EXIT the program, enter \"exit\"";
        }

        /// <summary>
        /// Remove param text from error message
        /// </summary>
        /// <param name="errorMessage">Error message from which to remove param text</param>
        /// <returns>Error message without param text</returns>
        private static string RemoveParamText(string errorMessage)
        {
            // Get index of start of param text
            int indexOfParam = errorMessage.IndexOf("(Param");

            // If no param text
            if (indexOfParam == -1)
            {
                return errorMessage; // Return error message
            }

            // Remove error message with param text removed
            return errorMessage.Substring(0, indexOfParam - 1);
        }

        /// <summary>
        /// Get list of file names as text (each file name on new line preceded by dash)
        /// </summary>
        /// <param name="fileNames">Enumerable of file names</param>
        /// <returns>Text list of file names</returns>
        private static string GetTextListOfFileNames(IEnumerable<string> fileNames)
        {
            return String.Join(Environment.NewLine, fileNames.Select((name) => $" - {name}"));
        }
    }
}