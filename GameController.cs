using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Status = message; // Set status
        }

        /// <summary>
        /// A prompt to display to the player
        /// </summary>
        public string Prompt => "Which direction do you want to go: ";

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
            // If input cannot be parsed to Direction enum value
            if ( !(Enum.TryParse(input, out Direction direction)) ) 
            {
                return "That's not a valid direction"; // Return invalid direction message
            }
            else if( !(this.Move(direction))) // If cannot move in specified direction
            {
                return "There's no exit in that direction"; // Return no exit in that direction message
            } 
            else // If successfully moved in specified direction
            {
                return "Moving " + direction; // Return the direction you're moving
            }
        }
    }
}
