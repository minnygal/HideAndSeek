using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public class Opponent
    {
        public readonly string Name;

        public override string ToString() => Name;

        public LocationWithHidingPlace HidingPlace { get; private set; }

        private static int _randomOpponentNumber = 1; // Number for next opponent created with default name
        
        /// <summary>
        /// Reset numbers for default names for opponents created without specified names
        /// </summary>
        public static void ResetDefaultNumbersForOpponentNames()
        {
            _randomOpponentNumber = 1;
        }

        /// <summary>
        /// Create opponent with default name
        /// </summary>
        public Opponent() : this("Random Opponent " + _randomOpponentNumber++) { }

        /// <summary>
        /// Create opponent with specified name
        /// </summary>
        /// <param name="name"></param>
        public Opponent(string name)
        {
            Name = name;
        }

        // Constructor accepting opponent's name and hiding place
        public Opponent(string name, LocationWithHidingPlace hidingPlace)
        {
            Name = name;
            HidingPlace = hidingPlace;
        }

        /// <summary>
        /// Hide in random hiding place
        /// </summary>
        public void Hide()
        {
            Hide(House.GetRandomHidingPlace()); // Hide in random hiding place.
        }

        /// <summary>
        /// Hide in specified hiding place
        /// Only called directly from GameController, SavedGame, and tests
        /// In future, could be used to have user hide players manually
        /// </summary>
        /// <param name="hidingPlace">Hiding place for opponent</param>
        public void Hide(LocationWithHidingPlace hidingPlace)
        {
            HidingPlace = hidingPlace; // Set hiding place property
            HidingPlace.HideOpponent(this); // Inform hiding place that this opponent is hidden there
        }
    }
}
