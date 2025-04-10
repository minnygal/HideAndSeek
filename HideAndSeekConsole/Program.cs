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
     * -I allow the user to enter a House layout name or use the default House layout.
     * -I added comments to make the code more readable.
     * -I made code conform to my formatting.
     * -I added code to print an empty line to put space between moves information.
     * **/
    class Program
    {
        static void Main(string[] args)
        {
            // Print welcome and basic instructions
            PrintWelcomeAndInstructions();
            Console.WriteLine();

            // Create variable to store game controller
            GameController gameController;

            // Until the user quits the program
            while (true)
            {
                // Get game controller to start game
                gameController = GetGameControllerToStartGame();

                // Welcome player to the House
                Console.WriteLine($"{Environment.NewLine}Welcome to {gameController.House.Name}!");

                // Until game is over
                while ( !(gameController.GameOver) )
                {
                    Console.WriteLine(gameController.Status); // Print game status
                    Console.Write(gameController.Prompt); // Print game prompt
                    Console.WriteLine( gameController.ParseInput(Console.ReadLine()) ); // Get user input, parse input, and print message
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
            }
        }

        /// <summary>
        /// Helper method to print app welcome and instructions
        /// </summary>
        private static void PrintWelcomeAndInstructions()
        {
            Console.WriteLine("Welcome to the Hide And Seek Console App!");
            Console.WriteLine("-Navigate through rooms in a virtual house to find all the hiding opponents " +
                              "in the fewest number of moves possible.");
            Console.WriteLine("-To MOVE, enter the direction in which you want to move.");
            Console.WriteLine("-To CHECK if any opponents are hiding in your current location, enter \"check\".");
            Console.WriteLine("-To TELEPORT to a random location with hiding place, enter \"teleport\".");
            Console.WriteLine("-To SAVE your progress, enter \"save\" followed by a space and a name for your game.");
            Console.WriteLine("-To LOAD a saved game, enter \"load\" followed by a space and the name of your game.");
            Console.WriteLine("-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game.");
        }

        /// <summary>
        /// Helper method to get game controller to start game based on user input; also sets House layout
        /// (allows user to enter number of opponents, names for opponents, load command, or empty string)
        /// </summary>
        /// <returns>Game controller set up for game</returns>
        private static GameController GetGameControllerToStartGame()
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
                    // If user entered load command
                    if (userInput.ToLower().StartsWith("load"))
                    {
                        gameController = LoadGame(userInput); // Load game and set game controller

                        // If game loaded sucessfully
                        if(gameController != null)
                        {
                            return gameController; // Return game controller
                        }
                    }
                    else // If user did not enter load command
                    {
                        // If user did not enter anything
                        if (userInput == string.Empty)
                        {
                            gameController = new GameController(); // Create a new default game controller
                        }
                        else if (int.TryParse(userInput, out int numberOfOpponents)) // If user entered a number
                        {
                            gameController = new GameController(numberOfOpponents); // Create new game controller with user input
                        }
                        else // if user did not enter load command, empty string, or a number, assume user entered names for opponents
                        {
                            gameController = new GameController(Regex.Replace(userInput, @"\s", string.Empty).Split(',')); // Create new game controller with names entered by user as array (without whitespace)
                        }

                        // Get game controller with House layout set
                        gameController = SetHouseLayoutForGameController(gameController);
                    }
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
        /// Helper method to load game from saved game file and return game controller
        /// </summary>
        /// <param name="userInput">User input containing load command and name of saved game file</param>
        /// <returns>Game controller with saved game loaded (or null if not loaded successfully)</returns>
        private static GameController LoadGame(string userInput)
        {
            // Create new GameController
            GameController gameController = new GameController();
            string message = gameController.ParseInput(userInput); // Parse input to load game
            Console.WriteLine(message); // Display message

            // If return message does not indicate success
            if (!(message.StartsWith("Game successfully loaded")))
            {
                gameController = null; // Set game controller to null
            }

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
                Console.Write(Environment.NewLine + "Type a house layout file name or just press Enter to use the default house layout: "); // Prompt for House layout file name
                string houseLayoutFileName = Console.ReadLine(); // Get user input for file name

                try
                {
                    // If no House file name entered
                    if (houseLayoutFileName.Trim() == "")
                    {
                        gameController.RestartGame(); // Restart game without House file specified
                    }
                    else // If House file name entered
                    {
                        gameController.RestartGame(houseLayoutFileName); // Restart game with House layout file, throws exception if anything invalid
                    }

                    // Update flag
                    houseLayoutChosen = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message); // Print exception message
                }
            } while (!(houseLayoutChosen));

            // Return game controller
            return gameController;
        }
    }
}