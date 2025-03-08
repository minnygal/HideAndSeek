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
            Console.WriteLine("-Navigate through rooms in a virtual house to find hiding opponents.");
            Console.WriteLine("-Find all the opponents to win the game.");
            Console.WriteLine("-The goal is to win the game in the fewest number of moves possible.");
            Console.WriteLine("-To MOVE, enter the direction in which you want to move.");
            Console.WriteLine("-To CHECK if any opponents are hiding in your current location, enter \"check\".");
            Console.WriteLine("-To SAVE your progress, enter \"save\" followed by a space and a name for your game.");
            Console.WriteLine("-To LOAD a saved game, enter \"load\" followed by a space and the name of your game.");
            Console.WriteLine("-To DELETE a saved game, enter \"delete\" followed by a space and the name of your game.");
            Console.WriteLine(); // Print empty line to put space between basic instructions and game

            // Create new game controller (also starts new game)
            GameController gameController = new GameController();

            // Until the user quits the program
            while (true)
            {
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

                // Otherwise, restart game
                gameController.RestartGame();
            }
        }
    }
}