using Moq;
using System.Collections; // From TestableIO
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for SaveGame method to save game to file
    /// </summary>
    public static class TestGameController_SaveGame_TestData
    {
        /// <summary>
        /// Array with a mocked Opponents (named)
        /// </summary>
        public static Opponent[] MockedOpponents
        {
            get
            {
                // Create Opponent mocks
                Mock<Opponent> opponent1 = new Mock<Opponent>();
                opponent1.Setup((o) => o.Name).Returns("Joe");
                opponent1.Setup((o) => o.ToString()).Returns("Joe");

                Mock<Opponent> opponent2 = new Mock<Opponent>();
                opponent2.Setup((o) => o.Name).Returns("Bob");
                opponent2.Setup((o) => o.ToString()).Returns("Bob");

                Mock<Opponent> opponent3 = new Mock<Opponent>();
                opponent3.Setup((o) => o.Name).Returns("Ana");
                opponent3.Setup((o) => o.ToString()).Returns("Ana");

                Mock<Opponent> opponent4 = new Mock<Opponent>();
                opponent4.Setup((o) => o.Name).Returns("Owen");
                opponent4.Setup((o) => o.ToString()).Returns("Owen");

                Mock<Opponent> opponent5 = new Mock<Opponent>();
                opponent5.Setup((o) => o.Name).Returns("Jimmy");
                opponent5.Setup((o) => o.ToString()).Returns("Jimmy");

                // Return array of mocked Opponents
                return new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object };
            }
        }

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

        /// <summary>
        /// Get new House object for testing purposes
        /// </summary>
        /// <returns>House object for testing purposes</returns>
        public static House GetCustomTestHouse()
        {
            // Create Landing and connect to new location: Hallway
            Location landing = new Location("Landing");
            Location hallway = landing.AddExit(Direction.North, "Hallway");

            // Connect Hallway to new locations: Bedroom, Sensory Room, Kitchen, Pantry, Bathroom, Living Room, Office, Attic
            LocationWithHidingPlace bedroom = hallway.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace sensoryRoom = hallway.AddExit(Direction.Northeast, "Sensory Room", "under the beanbags");
            LocationWithHidingPlace kitchen = hallway.AddExit(Direction.East, "Kitchen", "beside the stove");
            LocationWithHidingPlace pantry = hallway.AddExit(Direction.Southeast, "Pantry", "behind the food");
            LocationWithHidingPlace bathroom = hallway.AddExit(Direction.Southwest, "Bathroom", "in the tub");
            LocationWithHidingPlace livingRoom = hallway.AddExit(Direction.West, "Living Room", "behind the sofa");
            LocationWithHidingPlace office = hallway.AddExit(Direction.Northwest, "Office", "under the desk");
            LocationWithHidingPlace attic = hallway.AddExit(Direction.Up, "Attic", "behind a trunk");

            // Connect Bedroom to new Closet and existing Sensory Room
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.North, "Closet", "between the coats");
            bedroom.AddExit(Direction.East, sensoryRoom);

            // Connect Kitchen to existing Pantry and new Cellar and Yard
            kitchen.AddExit(Direction.South, pantry);
            LocationWithHidingPlace cellar = kitchen.AddExit(Direction.Down, "Cellar", "behind the canned goods");
            LocationWithHidingPlace yard = kitchen.AddExit(Direction.Out, "Yard", "behind a bush");

            // Connect Living Room to existing Office and Bathroom
            livingRoom.AddExit(Direction.North, office);
            livingRoom.AddExit(Direction.South, bathroom);

            // Create list of Location objects (no hiding places)
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>() { landing, hallway };

            // Create list of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                bedroom,
                closet,
                sensoryRoom,
                kitchen,
                cellar,
                pantry,
                yard,
                bathroom,
                livingRoom,
                office,
                attic
            };

            // Create and return new House
            return new House("test house", "TestHouse", "Landing", locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        public static IEnumerable TestCases_For_Test_GameController_SaveGame_AndCheckTextSavedToFile
        {
            get
            {
                // Default House, no Opponents found
                yield return new TestCaseData(
                        () =>
                        {
                            // Create GameController with default House, hide all Opponents in specified locations, and return GameController
                            return new GameController(MockedOpponents, GetDefaultHouse())
                                       .RehideAllOpponents(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" });
                        },
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - default House - no opponents found")
                    .SetCategory("GameController SaveGame Constructor RehideAllOpponents Success");

                // Default House, 3 Opponents found
                yield return new TestCaseData(
                        () => 
                        {
                            // Create GameController with default House and hide all Opponents in specified locations
                            GameController gameController = new GameController(MockedOpponents, GetDefaultHouse())
                                                                .RehideAllOpponents(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" });

                            // Go to Kitchen and check to find 2 Opponents
                            gameController.Move(Direction.East);
                            gameController.Move(Direction.Northwest);;
                            gameController.CheckCurrentLocation();

                            // Go to Bathroom and check to find 1 Opponent
                            gameController.Move(Direction.Southeast);;
                            gameController.Move(Direction.North);;
                            gameController.CheckCurrentLocation();

                            // Return GameController after 3 Opponents found
                            return gameController;
                        },
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Bathroom\"" + "," +
                            "\"MoveNumber\":7" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Kitchen\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]" +
                        "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - default House - 3 opponents found")
                    .SetCategory("GameController SaveGame Constructor RehideAllOpponents Move CheckCurrentLocation Success");
                
                // Custom House with constructor, no Opponents found
                yield return new TestCaseData(
                        () =>
                        {
                            return new GameController(MockedOpponents, GetCustomTestHouse()) // Create GameController with mocked Opponents and specific House object
                                       .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Landing\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - constructor - no opponents found")
                    .SetCategory("GameController SaveGame CustomHouse Constructor RehideAllOpponents Success");

                // Custom House with RestartGame, no Opponents found
                yield return new TestCaseData(
                    () =>
                    {
                        // Return GameController with restarted game and rehidden Opponents
                        return new GameController(MockedOpponents, GetCustomTestHouse()) // Create GameController with mocked Opponents and specific House
                                    .RestartGame() // and restart game
                                    .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations
                    },
                    "{" +
                        "\"HouseFileName\":\"TestHouse\"" + "," +
                        "\"PlayerLocation\":\"Landing\"" + "," +
                        "\"MoveNumber\":1" + "," +
                        "\"OpponentsAndHidingLocations\":" +
                        "{" +
                            "\"Joe\":\"Closet\"," +
                            "\"Bob\":\"Yard\"," +
                            "\"Ana\":\"Cellar\"," +
                            "\"Owen\":\"Attic\"," +
                            "\"Jimmy\":\"Yard\"" +
                        "}" + "," +
                        "\"FoundOpponents\":[]" +
                    "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - RestartGame - no opponents found")
                    .SetCategory("GameController SaveGame CustomHouse Constructor RestartGame RehideAllOpponents Success");

                // Custom House with constructor, 3 Opponents found
                yield return new TestCaseData(
                    () =>
                    {
                        // Initialize to GameController with restarted game and rehidden Opponents
                        GameController gameController = new GameController(MockedOpponents, GetCustomTestHouse()) // Create GameController with mocked Opponents and specific House
                                                        .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                        // Go to Cellar and find 1 Opponent there
                        gameController.Move(Direction.North);;
                        gameController.Move(Direction.East);
                        gameController.Move(Direction.Down);;
                        gameController.CheckCurrentLocation();

                        // Go to Yard and find 2 Opponents there
                        gameController.Move(Direction.Up);;
                        gameController.Move(Direction.Out);;
                        gameController.CheckCurrentLocation();

                        // Return GameController
                        return gameController;
                    },
                    "{" +
                        "\"HouseFileName\":\"TestHouse\"" + "," +
                        "\"PlayerLocation\":\"Yard\"" + "," +
                        "\"MoveNumber\":8" + "," +
                        "\"OpponentsAndHidingLocations\":" +
                        "{" +
                            "\"Joe\":\"Closet\"," +
                            "\"Bob\":\"Yard\"," +
                            "\"Ana\":\"Cellar\"," +
                            "\"Owen\":\"Attic\"," +
                            "\"Jimmy\":\"Yard\"" +
                        "}" + "," +
                        "\"FoundOpponents\":[\"Ana\",\"Bob\",\"Jimmy\"]" +
                    "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - constructor - 3 opponents found")
                    .SetCategory("GameController SaveGame CustomHouse Constructor RehideAllOpponents Move CheckCurrentLocation Success");

                // Custom House with RestartGame, 3 Opponents found
                yield return new TestCaseData(
                        () =>
                        {
                            // Initialize GameController
                            GameController gameController = new GameController(MockedOpponents, GetCustomTestHouse()) // Create GameController with specified file system and specific House
                                                            .RestartGame() // Restart Game                                    
                                                            .RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" }); // and hide all Opponents in specified locations

                            // Go to Cellar and find 1 Opponent there
                            gameController.Move(Direction.North);;
                            gameController.Move(Direction.East);
                            gameController.Move(Direction.Down);;
                            gameController.CheckCurrentLocation();

                            // Go to Yard and find 2 Opponents there
                            gameController.Move(Direction.Up);;
                            gameController.Move(Direction.Out);;
                            gameController.CheckCurrentLocation();

                            // Return GameController
                            return gameController;
                        },
                        "{" +
                            "\"HouseFileName\":\"TestHouse\"" + "," +
                            "\"PlayerLocation\":\"Yard\"" + "," +
                            "\"MoveNumber\":8" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Closet\"," +
                                "\"Bob\":\"Yard\"," +
                                "\"Ana\":\"Cellar\"," +
                                "\"Owen\":\"Attic\"," +
                                "\"Jimmy\":\"Yard\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[\"Ana\",\"Bob\",\"Jimmy\"]" +
                        "}")
                    .SetName("Test_GameController_SaveGame_AndCheckTextSavedToFile - custom House - RestartGame - 3 opponents found")
                    .SetCategory("GameController SaveGame CustomHouse Constructor RestartGame RehideAllOpponents Move CheckCurrentLocation Success");
            }
        }
    }
}