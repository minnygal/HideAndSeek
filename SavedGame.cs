using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class SavedGame
    {
        // Player's current location
        public string PlayerLocation { get; private set; }

        // Current move number
        public int MoveNumber { get; private set; }

        // All opponents (opponent name and hiding place name)
        public Dictionary<string, string> AllOpponents { get; private set; } = new Dictionary<string, string>();

        // All found opponents' names
        public IEnumerable<string> FoundOpponents { get; private set; }

        // Constructor
        public SavedGame(GameController gameController)
        {
            PlayerLocation = gameController.CurrentLocation.Name; // Set player location
            MoveNumber = gameController.MoveNumber; // Set move number

            // Get opponents
            IEnumerable<Opponent> opponents = gameController.Opponents;

            // Add opponents and their hiding places to collection
            foreach(Opponent opponent in opponents)
            {
                AllOpponents.Add(opponent.Name, opponent.HidingPlace.Name);
            }

            FoundOpponents = gameController.FoundOpponents.Select((x) => x.Name);
        }
    }
}
