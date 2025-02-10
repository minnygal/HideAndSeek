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
        /// Create opponent with default name
        /// </summary>
        public Opponent() : this("RandomOpponent" + _randomOpponentNumber++) { }

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
            HidingPlace = House.GetRandomHidingPlace(); // Set hiding place property to random hiding place
            HidingPlace.Hide(this); // Inform hiding place that this opponent is hidden there
        }
    }
}
