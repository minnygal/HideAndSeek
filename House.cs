using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public static class House {
        public static Location Entry { get; private set; }

        /// <summary>
        /// Static constructor to create Locations within the house and connect them
        /// </summary>
        static House()
        {
            // Create Entry and connect to new Garage
            Entry = new Location("Entry");
            Entry.AddExit(Direction.Out, "Garage");

            // Create Hallway and connect to Entry and new rooms: Kitchen, Bathroom, and Living Room
            Location hallway = new Location("Hallway");
            hallway.AddExit(Direction.West, Entry);
            hallway.AddExit(Direction.Northwest, "Kitchen");
            hallway.AddExit(Direction.North, "Bathroom");
            hallway.AddExit(Direction.South, "Living Room");

            // Create Landing and connect to Hallway and new rooms: Second Bathroom, Nursery, Pantry, Kids Room, and Attic
            Location landing = new Location("Landing");
            landing.AddExit(Direction.Down, hallway);
            landing.AddExit(Direction.West, "Second Bathroom");
            landing.AddExit(Direction.Southwest, "Nursery");
            landing.AddExit(Direction.South, "Pantry");
            landing.AddExit(Direction.Southeast, "Kids Room");
            landing.AddExit(Direction.Up, "Attic");

            // Create Master Bedroom and connect to Landing and new Master Bath
            Location masterBedroom = new Location("Master Bedroom");
            masterBedroom.AddExit(Direction.Southeast, landing);
            masterBedroom.AddExit(Direction.East, "Master Bath");
        }
    }
}
