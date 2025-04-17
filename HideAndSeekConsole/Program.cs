using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// Class to run in Console interactive game of HideAndSeek
    /// 
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
     * -I used the RestartGame method I added to GameController so
     *  GameController only has to be created once.
     * -I allow the user to select the number or names of opponents.
     * -I allow the user to enter a House layout name or use the default House layout.
     * -I moved the ParseInput method to this class.
     * -I added comments to make the code more readable.
     * -I made code conform to my formatting.
     * -I added code to print an empty line to put space between moves information.
     * **/
    class Program
    {
        private static GameController gameController; // Game controller used by Main and ParseInput methods

        /// <summary>
        /// Main method to play a game of Hide and Seek
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Print welcome and basic instructions
            PrintWelcomeAndInstructions();
            Console.WriteLine();

            // Create new game controller
            gameController = new GameController();

            // Until the user quits the program
            while (true)
            {
                // Welcome user to the House
                PrintWelcomeToHouse();

                // Until game is over
                while ( !(gameController.GameOver) )
                {
                    Console.WriteLine(gameController.Status); // Print game status
                    Console.Write(gameController.Prompt); // Print game prompt
                    Console.WriteLine( ParseInput( Console.ReadLine()) ); // Get user input, parse input, and print message
                    Console.WriteLine(); // Print empty line to put space between moves
                }

                // Print post-game info and instructions
                Console.WriteLine($"You won the game in {gameController.MoveNumber} moves!");
                Console.WriteLine("Press P to play again, any other key to quit.");

                // If user does not want to play again, quit
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() != "P")
                {
                    return;
                }

                // If user does want to play again, restart game
                gameController.RestartGame();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Helper method to welcome user to the House
        /// </summary>
        private static void PrintWelcomeToHouse()
        {
            Console.WriteLine($"Welcome to {gameController.House.Name}!");
        }

        /// <summary>
        /// Helper method to print app welcome and instructions
        /// </summary>
        private static void PrintWelcomeAndInstructions()
        {
            Console.WriteLine("Welcome to the Hide And Seek Console App!");
            Console.WriteLine("Navigate through rooms in a virtual house to find all the hiding opponents " +
                              "in the fewest number of moves possible.");
            Console.WriteLine("-To MOVE, enter the direction in which you want to move.");
            Console.WriteLine("-To CHECK if any opponents are hiding in your current location, enter \"check\".");
            Console.WriteLine("-To TELEPORT to a random location with a hiding place, enter \"teleport\".");
            Console.WriteLine("-To SAVE your progress, enter \"save\" followed by a space and a name for your game.");
            Console.WriteLine("-To LOAD a saved game, enter \"load\" followed by a space and the name of your game.");
            Console.WriteLine("-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game.");
            Console.WriteLine("-To start a NEW custom game, enter \"new\" and follow the prompts.");
        }

        /// <summary>
        /// Parse input from the player
        /// Accepts case-insensitive commands for directions, 
        /// teleport, check, new, save, load, delete
        /// Modifies class's GameController's state
        /// </summary>
        /// <param name="input">Input to parse</param>
        /// <returns>The results of parsing the input</returns>
        private static string ParseInput(string input)
        {
            // Extract command and make lowercase
            string lowercaseCommand = input.Trim().Split(" ").FirstOrDefault("").ToLower();

            // Evaluate command and act accordingly
            if (lowercaseCommand == "check") // If input requests the current location be checked for hiding opponents
            {
                try
                {
                    return gameController.CheckCurrentLocation(); // Check current location and return results
                }
                catch (Exception e)
                {
                    return e.Message; // Return error message
                }
            }
            else if (lowercaseCommand == "teleport") // If input requests teleportation
            {
                return gameController.Teleport(); // Teleport and return message
            }
            else if ( // If input requests save, load, or delete game
                lowercaseCommand == "save" ||
                lowercaseCommand == "load" ||
                lowercaseCommand == "delete")
            {
                // Get index of first space in input (space after command and before name of file)
                int indexOfSpace = input.IndexOf(' ');

                // If input does not include a space
                if (indexOfSpace == -1)
                {
                    return "Cannot perform action because no file name was entered"; // Return failure message
                }
                else // If input does include a space
                {
                    // Extract file name
                    string fileName = input.Substring(indexOfSpace + 1);

                    // Perform requested action and return message
                    try
                    {
                        switch (lowercaseCommand)
                        {
                            case "save":
                                return gameController.SaveGame(fileName);
                            case "load":
                                PrintWelcomeToHouse(); // Welcome user to the House
                                return gameController.LoadGame(fileName);
                            default:
                                return gameController.DeleteGame(fileName);
                        }
                    }
                    catch (Exception e)
                    {
                        return e.Message; // Return error message
                    }
                }
            }
            else if(lowercaseCommand == "new") // If input requests start new custom game
            {
                Console.WriteLine(); // Add an empty line
                gameController = GetGameControllerForCustomGame(); // Set game controller to game controller for new custom game                            
                PrintWelcomeToHouse(); // Welcome user to the House
                return "New game started";
            }
            else // Try to move in specified Direction
            {
                try
                {
                    return gameController.Move( DirectionExtensions.Parse(lowercaseCommand) );
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        /// <summary>
        /// Helper method to get game controller for new game based on user input; also sets House layout
        /// (allows user to enter number or names of opponents, or empty string for default opponents)
        /// </summary>
        /// <returns>Game controller set up for game</returns>
        private static GameController GetGameControllerForCustomGame()
        {
            // Set game controller to null (used as flag in do-while to create new game controller)
            GameController gameController = null;

            do
            {
                // Obtain user input for setting opponents or loading game
                Console.Write("How many opponents would you like?  Enter a number between 1 and 10, or a comma-separated list of names: "); // Prompt
                string userInput = Console.ReadLine().Trim(); // Get user input and trim it

                try
                {
                    // If user did not enter anything
                    if(userInput == string.Empty)
                    {
                        gameController = new GameController(); // Create a new default game controller
                    }
                    else if( int.TryParse(userInput, out int numberOfOpponents) ) // If user entered a number
                    {
                        gameController = new GameController(numberOfOpponents); // Create new game controller with specified number of opponents
                    }
                    else // if user did not enter empty string or a number, then assume user entered names for opponents
                    {
                        string[] namesOfOpponents = userInput.Split(','); // Extract names from user input
                        for(int i = 0; i < namesOfOpponents.Length; i++)
                        {
                            namesOfOpponents[i] = namesOfOpponents[i].Trim(); // Remove whitespace before or after each name
                        }
                        gameController = new GameController(namesOfOpponents); // Create new game controller with names entered by user as array (without whitespace)
                    }

                    // Get game controller with House layout set
                    gameController = SetHouseLayoutForGameController(gameController);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // Print error message
                    gameController = null; // Make sure game controller variable is not set
                }
            } while (gameController == null);

            // Return game controller
            return gameController;
        }

        /// <summary>
        /// Helper method to set the House layout for a game controller based on user input
        /// (allows user to enter name of file from which to load House layout or empty string to load default layout)
        /// </summary>
        /// <returns>Game controller with House layout loaded</returns>
        private static GameController SetHouseLayoutForGameController(GameController gameController)
        {
            // Set flag
            bool houseLayoutChosen = false;

            do
            {
                // Get House file name from user
                Console.Write("Type a house layout file name or just press Enter to use the default house layout: "); // Prompt for House layout file name
                string houseLayoutFileName = Console.ReadLine(); // Get user input for file name

                try
                {
                    // If any text was entered
                    if (houseLayoutFileName.Trim() != string.Empty)
                    {
                        gameController.RestartGame(houseLayoutFileName); // Restart game with House layout file, throws exception if anything invalid
                    } // else if no text entered, don't change House layout

                    // Update flag
                    houseLayoutChosen = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // Print exception message
                }
            } while( !(houseLayoutChosen) );

            // Return game controller
            return gameController;
        }
    }
}