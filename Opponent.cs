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


        // Contructor accepting opponent's name
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

        public void Hide()
        {
            // Current location of opponent moving through House to find hiding place
            Location currentLocation = House.Entry;

            // Minimum number of moves opponent makes through the house to find a hiding spot
            int minNumberOfMoves = House.Random.Next(10, 51);

            // Move through house via random exits between 10 and 50 times (inclusive)
            for(int currentMove = 0; currentMove < minNumberOfMoves; currentMove++)
            {
               currentLocation = House.RandomExit(currentLocation);
            }

            // Keep moving until a location with hiding place is found
            while(currentLocation.GetType() != typeof(LocationWithHidingPlace))
            {
                currentLocation = House.RandomExit(currentLocation);
            }

            // Initialize hiding place to current location
            LocationWithHidingPlace hidingPlace = currentLocation as LocationWithHidingPlace;

            // Hide in hiding place
            hidingPlace.Hide(this);

            // Set hiding place property
            HidingPlace = hidingPlace;
        }
    }
}
