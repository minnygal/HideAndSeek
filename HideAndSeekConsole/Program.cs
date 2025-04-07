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
            // Print basic instructions
            Console.WriteLine("Welcome to the Hide And Seek Console App!");
            Console.WriteLine("-Navigate through rooms in a virtual house to find all the hiding opponents " +
                              "in the fewest number of moves possible.");
            Console.WriteLine("-To MOVE, enter the direction in which you want to move.");
            Console.WriteLine("-To CHECK if any opponents are hiding in your current location, enter \"check\".");
            Console.WriteLine("-To TELEPORT to a random location with hiding place, enter \"teleport\".");
            Console.WriteLine("-To SAVE your progress, enter \"save\" followed by a space and a name for your game.");
            Console.WriteLine("-To LOAD a saved game, enter \"load\" followed by a space and the name of your game.");
            Console.WriteLine("-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game.");
            Console.WriteLine();

            // Create new game controller
            GameController gameController = new GameController();

            // Until the user quits the program
            while (true)
            {
                bool houseLayoutChosen = false; // Set flag
                do
                {
                    // Get House file name
                    Console.Write("Type a house layout file name or just press Enter to use the default house layout: "); // Prompt for House layout file name
                    string houseLayoutFileName = Console.ReadLine(); // Get user input

                    try
                    {
                        // If no House file name was entered
                        if (houseLayoutFileName.Trim() == "")
                        {
                            gameController.RestartGame("DefaultHouse"); // Restart game
                        }
                        else // If a House file name was entered
                        {
                            gameController.RestartGame(houseLayoutFileName); // Restart game with House layout file
                        }

                        // Update flag
                        houseLayoutChosen = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message); // Print exception message
                    }
                } while (!(houseLayoutChosen));
                
                // Welcome player to the House
                Console.WriteLine($"{Environment.NewLine}Welcome to {gameController.House.Name}!");

                // Until game is over
                while ( !(gameController.GameOver) )
                {
                    Console.WriteLine(gameController.Status); // Print game status
                    Console.Write(gameController.Prompt); // Print game prompt
                    Console.WriteLine(gameController.ParseInput(Console.ReadLine())); // Get user input, parse input, and print message
                    Console.WriteLine(); // Print empty line to put space between moves
                }

                // Print post game info and instructions
                Console.WriteLine($"You won the game in {gameController.MoveNumber} moves!");
                Console.WriteLine("Press P to play again, any other key to quit.");

                // If user does not want to play again, quit
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() != "P")
                {
                    return;
                }
            }
        }
    }
}