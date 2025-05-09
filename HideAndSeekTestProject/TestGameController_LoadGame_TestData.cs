using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for LoadGame method to load saved game from file
    /// </summary>
    public static class TestGameController_LoadGame_TestData
    {
        /// <summary>
        /// Get GameController created with minimal House object and mocked Opponent
        /// </summary>
        /// <returns>GameController created with minimal House object and mocked Opponent</returns>
        public static GameController GetMinimalGameController()
        {
            Location entryLocation = new Location("Entry"); // Create entry
            LocationWithHidingPlace locationWithHidingPlace = new LocationWithHidingPlace("Office", "under the table"); // Create a location with hiding place
            House house = new House("test house", "TestHouse", entryLocation,
                              new List<Location>() { entryLocation },
                              new List<LocationWithHidingPlace>() { locationWithHidingPlace }); // Create House
            return new GameController(new Opponent[] { new Mock<Opponent>().Object }, house); // Create GameController with House
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
            return new House("my house", "DefaultHouse", entry, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized
        {
            get
            {
                return
                    "{" +
                        "\"Name\":\"my house\"" + "," +
                        "\"HouseFileName\":\"DefaultHouse\"" + "," +
                        "\"PlayerStartingPoint\":\"Entry\"" + "," +
                        "\"LocationsWithoutHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"Name\":\"Hallway\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Entry\"," +
                                    "\"Northwest\":\"Kitchen\"," +
                                    "\"North\":\"Bathroom\"," +
                                    "\"South\":\"Living Room\"," +
                                    "\"Up\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Landing\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Hallway\"," +
                                    "\"Up\":\"Attic\"," +
                                    "\"Southeast\":\"Kids Room\"," +
                                    "\"Northwest\":\"Master Bedroom\"," +
                                    "\"Southwest\":\"Nursery\"," +
                                    "\"South\":\"Pantry\"," +
                                    "\"West\":\"Second Bathroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"Name\":\"Entry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Out\":\"Garage\"," +
                                    "\"East\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" + "," +
                        "\"LocationsWithHidingPlaces\":" +
                        "[" +
                            "{" +
                                "\"HidingPlace\":\"in a trunk\"," +
                                "\"Name\":\"Attic\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Down\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{\"HidingPlace\":\"behind the door\"," +
                                "\"Name\":\"Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"South\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the bunk beds\"," +
                                "\"Name\":\"Kids Room\"," +
                                "\"ExitsForSerialization\":" +
                                    "{" +
                                        "\"Northwest\":\"Landing\"" +
                                    "}" +
                                "}," +
                            "{" +
                                "\"HidingPlace\":\"under the bed\"," +
                                "\"Name\":\"Master Bedroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Landing\"," +
                                    "\"East\":\"Master Bath\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the changing table\"," +
                                "\"Name\":\"Nursery\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Northeast\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"inside a cabinet\"," +
                                "\"Name\":\"Pantry\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the shower\"," +
                                "\"Name\":\"Second Bathroom\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"East\":\"Landing\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"next to the stove\"," +
                                "\"Name\":\"Kitchen\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southeast\":\"Hallway\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"in the tub\"," +
                                "\"Name\":\"Master Bath\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"West\":\"Master Bedroom\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the car\"," +
                                "\"Name\":\"Garage\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"In\":\"Entry\"" +
                                "}" +
                            "}," +
                            "{" +
                                "\"HidingPlace\":\"behind the sofa\"," +
                                "\"Name\":\"Living Room\"," +
                                "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"North\":\"Hallway\"" +
                                "}" +
                            "}" +
                        "]" +
                    "}";
            }
        }

        /// <summary>
        /// Enumerable of Location names in Locations property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_Locations
        {
            get
            {
                return new List<string>()
                    {
                        "Attic",
                        "Hallway",
                        "Bathroom",
                        "Kids Room",
                        "Master Bedroom",
                        "Nursery",
                        "Pantry",
                        "Second Bathroom",
                        "Kitchen",
                        "Master Bath",
                        "Garage",
                        "Landing",
                        "Living Room",
                        "Entry"
                    };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithoutHidingPlaces property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_LocationsWithoutHidingPlaces
        {
            get
            {
                return new List<string>()
                        {
                            "Hallway",
                            "Landing",
                            "Entry"
                        };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithHidingPlaces property in default House for tests
        /// </summary>
        public static IEnumerable<string> DefaultHouse_LocationsWithHidingPlaces
        {
            get
            {
                return new List<string>()
                    {
                        "Attic",
                        "Bathroom",
                        "Kids Room",
                        "Master Bedroom",
                        "Nursery",
                        "Pantry",
                        "Second Bathroom",
                        "Kitchen",
                        "Master Bath",
                        "Garage",
                        "Living Room",
                    };
            }
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
            return new House("test house", "TestHouse", landing, locationsWithoutHidingPlaces, locationsWithHidingPlaces);
        }

        /// <summary>
        /// Text representing serialized custom House for tests
        /// </summary>
        public static string CustomTestHouse_Serialized
        {
            get
            {
                return
                "{" +
                    "\"Name\":\"test house\"" + "," +
                    "\"HouseFileName\":\"TestHouse\"" + "," +
                    "\"PlayerStartingPoint\":\"Landing\"" + "," +
                    "\"LocationsWithoutHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"Name\":\"Landing\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"Name\":\"Hallway\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"," +
                                "\"Northeast\":\"Sensory Room\"," +
                                "\"East\":\"Kitchen\"," +
                                "\"Southeast\":\"Pantry\"," +
                                "\"South\":\"Landing\"," +
                                "\"Southwest\":\"Bathroom\"," +
                                "\"West\":\"Living Room\"," +
                                "\"Northwest\":\"Office\"," +
                                "\"Up\":\"Attic\"" +
                            "}" +
                        "}" +
                    "]" + "," +
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"," +
                            "\"Name\":\"Bedroom\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Closet\"," +
                                "\"East\":\"Sensory Room\"," +
                                "\"South\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{\"HidingPlace\":\"between the coats\"," +
                            "\"Name\":\"Closet\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"under the beanbags\"," +
                            "\"Name\":\"Sensory Room\"," +
                            "\"ExitsForSerialization\":" +
                                "{" +
                                    "\"Southwest\":\"Hallway\"," +
                                    "\"West\":\"Bedroom\"" +
                                "}" +
                            "}," +
                        "{" +
                            "\"HidingPlace\":\"beside the stove\"," +
                            "\"Name\":\"Kitchen\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Hallway\"," +
                                "\"South\":\"Pantry\"," +
                                "\"Down\":\"Cellar\"," +
                                "\"Out\":\"Yard\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the canned goods\"," +
                            "\"Name\":\"Cellar\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Up\":\"Kitchen\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the food\"," +
                            "\"Name\":\"Pantry\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Kitchen\"," +
                                "\"Northwest\":\"Hallway\"" +
                            "}" +
                        "}," + "{" +
                            "\"HidingPlace\":\"behind a bush\"," +
                            "\"Name\":\"Yard\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"In\":\"Kitchen\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"in the tub\"," +
                            "\"Name\":\"Bathroom\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Living Room\"," +
                                "\"Northeast\":\"Hallway\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind the sofa\"," +
                            "\"Name\":\"Living Room\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Office\"," +
                                "\"East\":\"Hallway\"," +
                                "\"South\":\"Bathroom\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"under the desk\"," +
                            "\"Name\":\"Office\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Southeast\":\"Hallway\"," +
                                "\"South\":\"Living Room\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"behind a trunk\"," +
                            "\"Name\":\"Attic\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"Down\":\"Hallway\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            }
        }

        /// <summary>
        /// Enumerable of Location names in Locations property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_Locations
        {
            get
            {
                return new List<string>()
                    {
                        "Landing",
                        "Hallway",
                        "Bedroom",
                        "Closet",
                        "Sensory Room",
                        "Kitchen",
                        "Cellar",
                        "Pantry",
                        "Yard",
                        "Bathroom",
                        "Living Room",
                        "Office",
                        "Attic"
                };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithoutHidingPlaces property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_LocationsWithoutHidingPlaces
        {
            get
            {
                return new List<string>() { "Landing", "Hallway" };
            }
        }

        /// <summary>
        /// Enumerable of Location names in LocationsWithHidingPlaces property in custom test House for tests
        /// </summary>
        public static IEnumerable<string> CustomTestHouse_LocationsWithHidingPlaces
        {
            get
            {
                return new List<string>()
                    {
                        "Bedroom", 
                        "Closet", 
                        "Sensory Room", 
                        "Kitchen", 
                        "Cellar",
                        "Pantry", 
                        "Yard", 
                        "Bathroom", 
                        "Living Room", 
                        "Office", 
                        "Attic"
                    };
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_WithNoFoundOpponents
        {
            get
            {
                yield return new TestCaseData(() =>
                    {
                        // Set mock file system for House property to return House file text
                        House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                            "TestHouse.house.json", CustomTestHouse_Serialized);

                        // Set variable to text in SavedGame file
                        string savedGameFileText =
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
                            "}";

                        // Set up mock for GameController file system to return SavedGame file text
                        GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.game.json", savedGameFileText);

                        // Get GameController and load game
                        GameController gameController = GetMinimalGameController();
                        gameController.LoadGame("my_saved_game"); // Load game

                        // Return game controller
                        return gameController;
                    })
                    .SetName("Test_GameController_LoadGame_WithNoFoundOpponents - file name")
                    .SetCategory("GameController LoadGame ByFileName House CurrentLocation MoveNumber OpponentsAndHidingLocations Success");

                yield return new TestCaseData(() =>
                    {
                        // Create SavedGame object
                        SavedGame savedGame = new SavedGame(GetCustomTestHouse(), "TestHouse", "Landing", 1,
                                                            new Dictionary<string, string>()
                                                            {
                                                                { "Joe", "Closet" },
                                                                { "Bob", "Yard" },
                                                                { "Ana", "Cellar" },
                                                                { "Owen", "Attic" },
                                                                { "Jimmy", "Yard" }
                                                            }, 
                                                            new List<string>());

                        // Return game controller after load game
                        return GetMinimalGameController().LoadGame(savedGame);
                    })
                    .SetName("Test_GameController_LoadGame_WithNoFoundOpponents - SavedGame")
                    .SetCategory("GameController LoadGame BySavedGame House CurrentLocation MoveNumber OpponentsAndHidingLocations Success");
            }
        }

        private static GameController SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithFileName(
            string playerLocation, int moveNumber, IEnumerable<string> foundOpponents)
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                   $"\"PlayerLocation\":\"{playerLocation}\"" + "," +
                   $"\"MoveNumber\":{moveNumber}" + "," +
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}" + "," +
                   $"\"FoundOpponents\":[{string.Join(",", foundOpponents.Select((o) => $"\"{o}\""))}]" +
                "}";

            // Set up mock for file system for GameController to return SavedGame file text
            GameController.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("my_saved_game.game.json", savedGameFileText);

            // Set up mock file system for House property to return default House file text
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                               "DefaultHouse.house.json", DefaultHouse_Serialized);

            // Get GameController and load game
            GameController gameController = GetMinimalGameController();
            gameController.LoadGame("my_saved_game"); // Load game

            // Return game controller
            return gameController;
        }

        private static GameController SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithSavedGameObject(string playerLocation, int moveNumber, IEnumerable<string> foundOpponents)
        {
            // Create SavedGame object
            SavedGame savedGame = new SavedGame(GetDefaultHouse(), "DefaultHouse", playerLocation, moveNumber,
                                                new Dictionary<string, string>()
                                                {
                                                                    { "Joe", "Kitchen" },
                                                                    { "Bob", "Pantry" },
                                                                    { "Ana", "Bathroom" },
                                                                    { "Owen", "Kitchen" },
                                                                    { "Jimmy", "Pantry" }
                                                },
                                                foundOpponents);

            // Load saved game and return game controller
            return GetMinimalGameController().LoadGame(savedGame);
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_WithFoundOpponents
        {
            get
            {
                // LOADGAME WITH FILE NAME
                // file name - 1 found Opponent
                yield return new TestCaseData("Bathroom", 4, new List<string>() { "Ana" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithFileName(
                                playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Opponents Bob and Jimmy are hiding in the Pantry
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Pantry").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                            "Bob and Jimmy are hidden in the Pantry");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - file name - 1 found opponent")
                    .SetCategory("GameController LoadGame ByFileName House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // file name - 2 found Opponents
                yield return new TestCaseData("Pantry", 10, new List<string>() { "Bob", "Jimmy" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithFileName(
                                playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Ana is hiding in the Bathroom
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Bathroom").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Ana" }),
                                            "Ana is hidden in the Bathroom");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - file name - 2 found opponents")
                    .SetCategory("GameController LoadGame ByFileName House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // file name - 3 found Opponents
                yield return new TestCaseData("Bathroom", 15, new List<string>() { "Joe", "Owen", "Ana" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithFileName(
                                playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Opponents Joe and Owen are hiding in the Kitchen
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Pantry").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                        "Bob and Jimmy are hidden in the Kitchen");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - file name - 3 found opponents")
                    .SetCategory("GameController LoadGame ByFileName House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // file name - 4 found Opponents
                yield return new TestCaseData("Pantry", 20, new List<string>() { "Joe", "Owen", "Bob", "Jimmy" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithFileName(
                                playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Ana is hiding in the Bathroom
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Bathroom").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Ana" }),
                                        "Ana is hidden in the Bathroom");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - file name - 4 found opponents")
                    .SetCategory("GameController LoadGame ByFileName House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // LOADGAME WITH FILE NAME
                // SavedGame - 1 found Opponent
                yield return new TestCaseData("Bathroom", 4, new List<string>() { "Ana" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithSavedGameObject(
                                        playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Opponents Bob and Jimmy are hiding in the Pantry
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Pantry").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                            "Bob and Jimmy are hidden in the Pantry");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - SavedGame - 1 found opponent")
                    .SetCategory("GameController LoadGame BySavedGame House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // SavedGame - 2 found Opponents
                yield return new TestCaseData("Pantry", 10, new List<string>() { "Bob", "Jimmy" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithSavedGameObject(
                                        playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            Assert.Multiple(() =>
                            {
                                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Kitchen").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                                            "Joe and Owen are hidden in the Kitchen");

                                // Assert that Ana is hiding in the Bathroom
                                Assert.That(locationsWithHidingPlaces
                                                .Where((l) => l.Name == "Bathroom").First()
                                                .CheckHidingPlace()
                                                .Select((o) => o.Name),
                                            Is.EquivalentTo(new List<string>() { "Ana" }),
                                            "Ana is hidden in the Bathroom");
                            });
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - SavedGame - 2 found opponents")
                    .SetCategory("GameController LoadGame BySavedGame House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // SavedGame - 3 found Opponents
                yield return new TestCaseData("Bathroom", 15, new List<string>() { "Joe", "Owen", "Ana" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithSavedGameObject(
                                        playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Opponents Joe and Owen are hiding in the Kitchen
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Pantry").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                                        "Bob and Jimmy are hidden in the Kitchen");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - SavedGame - 3 found opponents")
                    .SetCategory("GameController LoadGame BySavedGame House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");

                // SavedGame - 4 found Opponents
                yield return new TestCaseData("Pantry", 20, new List<string>() { "Joe", "Owen", "Bob", "Jimmy" },
                        (string playerLocation, int moveNumber, List<string> foundOpponents) =>
                        {
                            // Return GameController
                            return SetUpAndGetGameController_For_TestCases_For_Test_GameController_LoadGame_WithFoundOpponents_WhenLoadGameWithSavedGameObject(
                                        playerLocation, moveNumber, foundOpponents);
                        },
                        (IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces) =>
                        {
                            // Assert that Ana is hiding in the Bathroom
                            Assert.That(locationsWithHidingPlaces
                                            .Where((l) => l.Name == "Bathroom").First()
                                            .CheckHidingPlace()
                                            .Select((o) => o.Name),
                                        Is.EquivalentTo(new List<string>() { "Ana" }),
                                        "Ana is hidden in the Bathroom");
                        })
                    .SetName("Test_GameController_LoadGame_WithFoundOpponents - SavedGame - 4 found opponents")
                    .SetCategory("GameController LoadGame BySavedGame House CurrentLocation MoveNumber OpponentsAndHidingLocations FoundOpponents Success");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid
        {
            get
            {
                // RANDOM ISSUES
                // No data in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - no data in file");

                // Only whitespace in file
                yield return new TestCaseData("The input does not contain any JSON tokens. Expected the input to start with a valid JSON token, when isFinalBlock is true. Path: $ | LineNumber: 0 | BytePositionInLine: 2.",
                        "  ")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - only whitespace in file");

                // Just characters in file (not JSON)
                yield return new TestCaseData("'A' is an invalid start of a value. Path: $ | LineNumber: 0 | BytePositionInLine: 0.",
                        "ABCDeaoueou[{}}({}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - just characters in file");

                // MISSING KEY/VALUE SET
                // Missing House file name
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: HouseFileName",
                        "{" +
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
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing house file name");

                // Missing player location
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: PlayerLocation",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
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
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing player location");

                // Missing move number
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: MoveNumber",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
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
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing move number");

                // Missing opponents and hiding locations
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: OpponentsAndHidingLocations",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing opponents and hiding locations");

                // Missing found opponents
                yield return new TestCaseData("JSON deserialization for type 'HideAndSeek.SavedGame' was missing required properties, including the following: FoundOpponents",
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
                            "}" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileFormatIsInvalid - missing found opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue
        {
            get
            {
                // INVALID VALUE DATA
                // Invalid player location
                yield return new TestCaseData("invalid PlayerLocation - location \"Tree\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Tree\"," +
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
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid PlayerLocation");

                // Invalid hiding place for Joe (not yet found) because location does not exist
                yield return new TestCaseData("location with hiding place \"Tree\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Tree\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - Location does not exist");

                // Invalid hiding place for Joe (not yet found) because hiding location is not of type LocationWithHidingPlace
                yield return new TestCaseData("location with hiding place \"Hallway\" does not exist in House",
                        "{" +
                            "\"HouseFileName\":\"DefaultHouse\"" + "," +
                            "\"PlayerLocation\":\"Entry\"" + "," +
                            "\"MoveNumber\":1" + "," +
                            "\"OpponentsAndHidingLocations\":" +
                            "{" +
                                "\"Joe\":\"Hallway\"," +
                                "\"Bob\":\"Pantry\"," +
                                "\"Ana\":\"Bathroom\"," +
                                "\"Owen\":\"Kitchen\"," +
                                "\"Jimmy\":\"Pantry\"" +
                            "}" + "," +
                            "\"FoundOpponents\":[]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - invalid hiding place for opponent - not LocationWithHidingPlace");

                // Found opponent is not in all opponents list
                yield return new TestCaseData("found opponent is not an opponent",
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
                            "\"FoundOpponents\":[\"Steve\"]" +
                        "}")
                    .SetName("Test_GameController_LoadGame_AndCheckErrorMessage_WhenFileDataHasInvalidValue - found opponent Steve is not opponent");
            }
        }
    }
}