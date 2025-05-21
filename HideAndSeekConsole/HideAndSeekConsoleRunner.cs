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
     * -I used the RestartGame method I added to GameController so
     *  GameController only has to be created once.
     * -I display available House layouts and allow the user to enter a House layout name 
     *  or use the default House layout.
     * -I display available SavedGame file names.
     * -I moved the ParseInput method to this class.
     * -I added comments to make the code more readable.
     * -I made code conform to my formatting.
     * -I added code to print an empty line to put space between moves information.
     * **/
    public class HideAndSeekConsoleRunner
    {
        public HideAndSeekConsoleRunner() : this(new GameController(), new ConsoleIOAdapter()) { }

        public HideAndSeekConsoleRunner(GameController gameController, IConsoleIO console)
        {
            GameController = gameController; // Set game controller to passed in game controller
            consoleIO = console; // Set console IO to passed in console IO
        }

        /// <summary>
        /// GameController object to control game
        /// </summary>
        public GameController GameController { get; private set; }

        /// <summary>
        /// Command to quit program
        /// </summary>
        public const string QuitCommand = "exit";

        // Console input/output object
        private readonly IConsoleIO consoleIO;

        /// <summary>
        /// Method to run the game
        /// Outputs game information to Console and accepts user input from Console
        /// </summary>
        public void RunGame()
        {
            // Create variable to store return value of ParseInput method
            string parseInputReturnValue;

            // Print welcome, basic instructions, and space
            consoleIO.WriteLine( GetWelcomeAndInstructions() + Environment.NewLine );

            // Until the user quits the program
            while (true)
            {
                // Welcome user to the House
                consoleIO.WriteLine(GetWelcomeToHouse());

                // Until game is over
                while( !(GameController.GameOver) )
                {
                    // Print game status and prompt, and get/parse/act on user input
                    consoleIO.WriteLine(GameController.Status); // Print game status
                    consoleIO.Write(GameController.Prompt); // Print game prompt
                    parseInputReturnValue = RemoveParamText(ParseInput(consoleIO.ReadLine())); // Get user input, parse input, act on input, and return message

                    // If requested, quit program
                    if (parseInputReturnValue == QuitCommand)
                    {
                        return;
                    }

                    // Print update
                    consoleIO.WriteLine(parseInputReturnValue); // Print ParseInput return message
                    consoleIO.WriteLine(); // Print empty line to put space between moves
                }

                // Print post-game info and instructions
                consoleIO.WriteLine($"You won the game in {GameController.MoveNumber} moves!");
                consoleIO.WriteLine("Press P to play again, any other key to quit.");

                // If user does not want to play again, quit
                if( !(consoleIO.ReadKey().KeyChar.ToString().Equals("P", StringComparison.CurrentCultureIgnoreCase)) )
                {
                    return;
                }

                // If user does want to play again, restart game and print space
                GameController.RestartGame();
                consoleIO.WriteLine();
            }
        }

        /// <summary>
        /// Parse input from the player
        /// Accepts case-insensitive commands for directions to move,
        /// teleport, check, new, save, load, delete
        /// </summary>
        /// <param name="userInput">User input to parse</param>
        /// <returns>Return message from parsing input / doing action OR "quit" if user wants to quit game</returns>
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
                switch (lowercaseCommand)
                {
                    // Check current location and return description
                    case "check":
                        return GameController.CheckCurrentLocation();

                    // Teleport and return description
                    case "teleport":
                        return GameController.Teleport();

                    // Save game and return description
                    case "save":
                        consoleIO.WriteLine();
                        return SaveGame(textAfterCommand);

                    // Load game and return description (or "quit" command if user wants to quit game)
                    case "load":
                        consoleIO.WriteLine();
                        return ReturnActionResultsOrQuitCommand(GetNameOfExistingSavedGameFile(textAfterCommand, lowercaseCommand), 
                            () => {
                                return GameController.LoadGame(userInput) +
                                       Environment.NewLine + GetWelcomeToHouse(); // Return results from loading game plus welcome message
                            });

                    // Delete game and return description (or "quit" command if user wants to quit game)
                    case "delete":
                        return ReturnActionResultsOrQuitCommand(GetNameOfExistingSavedGameFile(textAfterCommand, lowercaseCommand), 
                            () => {
                                return GameController.DeleteGame(userInput); // Return results from deleting game
                            });

                    // Start new game and return description (or "quit" command if user wants to quit game)
                    case "new":
                        // Set game controller for new custom game based on user input
                        consoleIO.WriteLine();
                        GameController = GetGameControllerForCustomGame();   
                        
                        // If user entered command to quit
                        if(GameController == null)
                        {
                            return QuitCommand; // Return quit command
                        }

                        // Return results and welcome message
                        return "New game started" + Environment.NewLine + GetWelcomeToHouse();

                    // Quit (exit) program
                    case QuitCommand:
                        return QuitCommand; // Return quit command

                    // Move in specified direction and return results
                    default:
                        return GameController.Move(DirectionExtensions.Parse(lowercaseCommand));
                }
            }
            catch(Exception e)
            {
                return RemoveParamText(e.Message); // Return exception message without param text
            }
        }

        /// <summary>
        /// Helper method to save current game (trims file name)
        /// </summary>
        /// <param name="fileName">Name of file in which to save game</param>
        /// <returns></returns>
        private string SaveGame(string fileName)
        {
            // Trim file name
            fileName = fileName.Trim();

            // If input is empty
            if (string.IsNullOrEmpty(fileName))
            {
                return "Cannot perform action because no file name was entered"; // Return failure message
            }
            else // If input is not empty
            {
                return GameController.SaveGame(fileName); // Save game and return message
            }
        }

        /// <summary>
        /// Get name of existing SavedGame file stored in current directory from user
        /// </summary>
        /// <param name="userInput">User input of file name (null, whitespace, or empty if should display list of existing SavedGame files)</param>
        /// <param name="command">Description of action to be done with file (e.g. "load", "delete")</param>
        /// <returns>User input for name of SavedGame file OR quit command if user wants to quit game</returns>
        private string GetNameOfExistingSavedGameFile(string userInput, string command)
        {
            // Trim user input
            userInput = userInput.Trim();

            // If input is empty
            if (string.IsNullOrEmpty(userInput))
            {
                return DisplayListOfSavedGamesAndGetUserInput(command); // Display list of saved game files and return input for file name
            }
            else if(userInput == QuitCommand) // If user entered quit command
            {
                return QuitCommand; // Return quit command
            }
            else
            {
                return userInput; // Return user input
            }
        }

        /// <summary>
        /// Helper method to display list of names of saved game files
        /// and get user input for name of file
        /// </summary>
        /// <param name="command">Description of action to be performed with file</param>
        /// <returns>User input for name of SavedGame file OR quit command</returns>
        /// <exception cref="InvalidOperationException">Exception thrown if no saved game files found</exception>
        private string DisplayListOfSavedGamesAndGetUserInput(string command)
        {
            // Get list of names of SavedGame files available
            IEnumerable<string> allSavedGameFileNames = SavedGame.GetSavedGameFileNames();

            // If no SavedGame files available
            if ( !(allSavedGameFileNames.Any()) )
            {
                throw new InvalidOperationException($"Cannot perform action because no saved game files are available"); // Throw exception
            }

            // Display list of names of SavedGame files available
            consoleIO.WriteLine("Here are the names of the saved game files available:" + Environment.NewLine +
                              GetTextListOfFileNames(allSavedGameFileNames)); // Display names of SavedGame files available

            // Get name of SavedGame file to load from user
            consoleIO.Write($"Enter the name of the saved game file to {command}: "); // Prompt
            return consoleIO.ReadLine().Trim(); // Set file name to trimmed user input
        }

        /// <summary>
        /// Helper method to get game controller for new game based on user input;
        /// also sets GameController House layout based on user input
        /// </summary>
        /// <returns>Game controller set up for game OR null if user wants to quit program</returns>
        private GameController GetGameControllerForCustomGame()
        {
            // Create local game controller variable (used as flag)
            GameController gameController = null;

            do
            {
                // Obtain user input for setting opponents or loading game
                consoleIO.Write("How many opponents would you like?  Enter a number between 1 and 10, or a comma-separated list of names: "); // Prompt
                string userInput = consoleIO.ReadLine().Trim(); // Get user input and trim it

                try
                {
                    // Create game controller with Opponents set
                    if (userInput == string.Empty) // If user did not enter anything
                    {
                        gameController = new GameController(); // Create a new default game controller
                    }
                    else if(userInput == QuitCommand) // If user entered command to quit
                    {
                        return null; // Return null to indicate user wants to quit
                    }
                    else if (int.TryParse(userInput, out int numberOfOpponents)) // If user entered a number
                    {
                        gameController = new GameController(numberOfOpponents); // Create new game controller with specified number of opponents
                    }
                    else // if user did not enter empty string or a number, then assume user entered names for opponents
                    {
                        string[] namesOfOpponents = userInput.Split(','); // Extract names from user input
                        for (int i = 0; i < namesOfOpponents.Length; i++)
                        {
                            namesOfOpponents[i] = namesOfOpponents[i].Trim(); // Remove whitespace before or after each name
                        }
                        gameController = new GameController(namesOfOpponents); // Create new game controller with names entered by user as array (without whitespace)
                    }

                    // Set game controller House layout based on user input
                   gameController = GetUserInputAndSetHouseLayout(gameController);
                }
                catch (Exception e)
                {
                    consoleIO.WriteLine(e.Message); // Print error message
                }
            } while (gameController == null);

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method to set the House layout for a game controller based on user input
        /// (allows user to enter name of file from which to load House layout or empty string to load default layout)
        /// </summary>
        /// <returns>Game controller with House layout loaded OR null if user wants to quit program</returns>
        private GameController GetUserInputAndSetHouseLayout(GameController gameController)
        {
            // Set flag
            bool houseLayoutChosen = false;

            do
            {
                // Display list of names of House layout files available
                consoleIO.WriteLine("Here are the names of the house layout files available:" + Environment.NewLine +
                                  GetTextListOfFileNames(House.GetHouseFileNames())); // Display names of House layout files available

                // Get House file name from user
                consoleIO.Write("Type a house layout file name or just press Enter to use the default house layout: "); // Prompt for House layout file name
                string userInputForFileName = consoleIO.ReadLine().Trim(); // Get user input for file name and trim

                // If user entered quit command, return null
                if(userInputForFileName == QuitCommand)
                {
                    return null;
                }

                try
                {
                    // If any text was entered
                    if (userInputForFileName != string.Empty)
                    {
                        gameController.RestartGame(userInputForFileName); // Restart game with House layout file, throws exception if anything invalid
                    } // else if no text entered, don't change House layout

                    // Update flag
                    houseLayoutChosen = true;
                }
                catch (Exception e)
                {
                    consoleIO.WriteLine( RemoveParamText(e.Message) ); // Print exception message without param text
                }
            } while( !(houseLayoutChosen) );

            // Return game controller
            return gameController;
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
        /// Helper method to print app welcome and instructions
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
        /// Remove parameter text in error messages
        /// </summary>
        /// <param name="text">Text to remove parameter text from</param>
        /// <returns>Text without parameter text</returns>
        private static string RemoveParamText(string text)
        {
            int indexOfParam = text.IndexOf("(Param");
            if (indexOfParam == -1)
            {
                return text;
            }

            // Remove parameter text
            return text.Substring(0, indexOfParam - 1);
        }

        /// <summary>
        /// Get text list of file names
        /// </summary>
        /// <param name="fileNames"></param>
        private static string GetTextListOfFileNames(IEnumerable<string> fileNames)
        {
            return String.Join(Environment.NewLine, fileNames.Select((name) => $" - {name}"));
        }

        /// <summary>
        /// Helper method to return quit command if user wants to quit game
        /// OR results of action performed if user does not want to quit game
        /// </summary>
        /// <param name="textInput">Text which could indicate user wants to quit game</param>
        /// <param name="ActionToPerform">Action to perform if user does not want to quit game</param>
        /// <returns>Return message from performing action OR "quit" if user wants to quit game</returns>
        private static string ReturnActionResultsOrQuitCommand(string textInput, Func<string> ActionToPerform)
        {
            // If text input is quit command
            if (textInput == QuitCommand)
            {
                return QuitCommand; // Return quit command
            }

            // Perform action and return message
            return ActionToPerform();
        }
    }
}