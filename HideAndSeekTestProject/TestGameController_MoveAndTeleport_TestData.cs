using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for Move and Teleport methods called in default House,
    /// checking CurrentLocation and Move properties along the way
    /// </summary>
    public static class TestGameController_MoveAndTeleport_TestData
    {
        /// <summary>
        /// Get new House object for testing purposes
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        public static House GetDefaultHouse()
        {
            // Create Entry and connect to new locations: Garage, Hallway
            Location entry = new Location("Entry");
            LocationWithHidingPlace garage = entry.AddExit(Direction.Out, "Garage", "behind the car");
            Location hallway = entry.AddExit(Direction.East, "Hallway");

            // Connect Hallway to new locations: Kitchen, Bathroom, Living Room, Landing
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.Northwest, "Kitchen", "next to the stove");
            LocationWithHidingPlace bathroom = hallway.AddExit(Direction.North, "Bathroom", "behind the door");
            LocationWithHidingPlace livingRoom = hallway.AddExit(Direction.South, "Living Room", "behind the sofa");
            Location landing = hallway.AddExit(Direction.Up, "Landing");

            // Connect Landing to new locations: Attic, Kids Room, Master Bedroom, Nursery, Pantry, Second Bathroom
            LocationWithHidingPlace attic = landing.AddExit(Direction.Up, "Attic", "in a trunk");
            LocationWithHidingPlace kidsRoom = landing.AddExit(Direction.Southeast, "Kids Room", "in the bunk beds");
            LocationWithHidingPlace masterBedroom = landing.AddExit(Direction.Northwest, "Master Bedroom", "under the bed");
            LocationWithHidingPlace nursery = landing.AddExit(Direction.Southwest, "Nursery", "behind the changing table");
            LocationWithHidingPlace pantry = landing.AddExit(Direction.South, "Pantry", "inside a cabinet");
            LocationWithHidingPlace secondBathroom = landing.AddExit(Direction.West, "Second Bathroom", "in the shower");

            // Connect Master Bedroom to new location: Master Bath
            LocationWithHidingPlace masterBath = masterBedroom.AddExit(Direction.East, "Master Bath", "in the tub");

            // Create list of Location objects (no hiding places)
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>()
            {
                hallway, landing, entry
            };

            // Create list of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                attic,
                bathroom,
                kidsRoom,
                masterBedroom,
                nursery,
                pantry,
                secondBathroom,
                kitchen,
                masterBath,
                garage,
                livingRoom
            };

            // Create and return new House
            return new House("my house", "DefaultHouse", "Entry", locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        public static IEnumerable TestCases_For_Test_GameController_Move_InAllDirectionsAsStrings
        {
            get
            {
                // Full direction names
                yield return new TestCaseData("north", "south", "east", "west", "northeast", "southwest",
                                              "southeast", "northwest", "up", "down", "in", "out")
                .SetName("Test_GameController_Move_InAllDirectionsAsStrings - full names");

                // Direction shorthands
                yield return new TestCaseData("n", "s", "e", "w", "ne", "sw", "se", "nw", "u", "d", "i", "o")
                .SetName("Test_GameController_Move_InAllDirectionsAsStrings - shorthands");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Move_InDirectionWithNoLocation_AndCheckErrorMessageAndProperties
        {
            get
            {
                // Full direction names
                yield return new TestCaseData(
                    (GameController gameController) => 
                    {
                        try
                        {
                            gameController.Move(Direction.Up);
                        }
                        catch(Exception e)
                        {
                            return e;
                        }
                        return null;
                    })
                .SetName("Test_GameController_Move_InDirectionWithNoLocation_AndCheckErrorMessageAndProperties - Direction");

                // Direction shorthands
                yield return new TestCaseData(
                    (GameController gameController) =>
                    {
                        try
                        {
                            gameController.Move("Up");
                        }
                        catch (Exception e)
                        {
                            return e;
                        }
                        return null;
                    })
                .SetName("Test_GameController_Move_InDirectionWithNoLocation_AndCheckErrorMessageAndProperties - string");
            }
        }
    }
}