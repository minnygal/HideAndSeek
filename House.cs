using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    public static class House {
        /// <summary>
        /// Entry of House
        /// </summary>
        public static Location Entry { get; private set; }

        /// <summary>
        /// List of Locations in House
        /// </summary>
        public static List<Location> Locations { get; private set; }

        /// <summary>
        /// Static constructor to create Locations within the house and connect them
        /// </summary>
        static House()
        {
            // Create Entry and connect to new locations: Garage, Hallway
            Entry = new Location("Entry");
            Location garage = Entry.AddExit(Direction.Out, "Garage");
            Location hallway = Entry.AddExit(Direction.East, "Hallway");

            // Connect Hallway to new locations: Kitchen, Bathroom, Living Room, Landing
            Location kitchen = hallway.AddExit(Direction.Northwest, "Kitchen");
            Location bathroom = hallway.AddExit(Direction.North, "Bathroom");
            Location livingRoom = hallway.AddExit(Direction.South, "Living Room");
            Location landing = hallway.AddExit(Direction.Up, "Landing");

            // Connect Landing to new locations: Second Bathroom, Nursery, Pantry, Kids Room, Attic, Master Bedroom
            Location secondBathroom = landing.AddExit(Direction.West, "Second Bathroom");
            Location nursery = landing.AddExit(Direction.Southwest, "Nursery");
            Location pantry = landing.AddExit(Direction.South, "Pantry");
            Location kidsRoom = landing.AddExit(Direction.Southeast, "Kids Room");
            Location attic = landing.AddExit(Direction.Up, "Attic");
            Location masterBedroom = landing.AddExit(Direction.Northwest, "Master Bedroom");

            // Connect Master Bodroom to new location: Master Bath
            Location masterBath = masterBedroom.AddExit(Direction.East, "Master Bath");

            // Add Locations to Locations list
            Locations = new List<Location>()
            {
                attic,
                hallway,
                kidsRoom,
                masterBedroom,
                nursery,
                pantry,
                secondBathroom,
                kitchen,
                hallway,
                masterBath,
                garage,
                landing,
                livingRoom,
                Entry
            };
        }
    }
}
