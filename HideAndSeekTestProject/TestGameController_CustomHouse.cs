using Moq;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for layout of non-default House
    /// set using GameController constructor or RestartGame method,
    /// and playing full game in custom House
    /// </summary>
    [TestFixture]
    public class TestGameController_CustomHouse
    {
        [TearDown]
        public void TearDown()
        {
            House.FileSystem = new FileSystem(); // Set House file system to new clean file system
        }

        [TestCaseSource(typeof(TestGameController_CustomHouse_TestCaseData), nameof(TestGameController_CustomHouse_TestCaseData.TestCases_For_Test_GameController_CustomHouse_NameAndFileNameProperties))]
        public void Test_GameController_CustomHouse_NameAndFileNameProperties(GameController gameController)
        {
            Assert.Multiple(() =>
            {
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomHouse_TestCaseData), nameof(TestGameController_CustomHouse_TestCaseData.TestCases_For_Test_GameController_CustomHouse_LocationNames_AndExits))]
        public void Test_GameController_CustomHouse_LocationNames_AndExits(GameController gameController)
        {
            // Initialize variables to Location objects by names
            Location landing = gameController.House.LocationsWithoutHidingPlaces.Where((l) => l.Name == "Landing").First();
            Location hallway = gameController.House.LocationsWithoutHidingPlaces.Where((l) => l.Name == "Hallway").First();

            // Initialize variables to LocationWithHidingPlace objects by names
            LocationWithHidingPlace bedroom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Bedroom").First();
            LocationWithHidingPlace closet = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Closet").First();
            LocationWithHidingPlace sensoryRoom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Sensory Room").First();
            LocationWithHidingPlace kitchen = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Kitchen").First();
            LocationWithHidingPlace cellar = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Cellar").First();
            LocationWithHidingPlace pantry = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Pantry").First();
            LocationWithHidingPlace yard = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Yard").First();
            LocationWithHidingPlace bathroom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Bathroom").First();
            LocationWithHidingPlace livingRoom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Living Room").First();
            LocationWithHidingPlace office = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Office").First();
            LocationWithHidingPlace attic = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Attic").First();

            // Assume no exceptions thrown when getting Location and LocationWithHidingPlace objects from associated properties
            // Check that Exits are as expected
            Assert.Multiple(() =>
            {
                // Check Location Exits
                Assert.That(landing.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(), 
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Hallway"
                    }), 
                    "Landing Exits");

                Assert.That(hallway.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Bedroom",
                        "Northeast:Sensory Room",
                        "East:Kitchen",
                        "Southeast:Pantry",
                        "South:Landing",
                        "Southwest:Bathroom",
                        "West:Living Room",
                        "Northwest:Office",
                        "Up:Attic"
                    }), 
                    "Hallway Exits");

                // Check LocationWithHidingPlace Exits
                Assert.That(bedroom.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Closet",
                        "East:Sensory Room"
                    }),
                    "Bedroom Exits");

                Assert.That(closet.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                         "South:Bedroom"
                    }),
                    "Closet Exits");

                Assert.That(sensoryRoom.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "Southwest:Hallway",
                        "West:Bedroom"
                    }),
                    "Sensory Room Exits");

                Assert.That(kitchen.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "West:Hallway",
                        "South:Pantry",
                        "Down:Cellar",
                        "Out:Yard"
                    }),
                    "Kitchen Exits");

                Assert.That(cellar.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "Up:Kitchen"
                    }),
                    "Cellar Exits");

                Assert.That(pantry.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Kitchen",
                        "Northwest:Hallway"
                    }),
                    "Pantry Exits");

                Assert.That(yard.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "In:Kitchen"
                    }),
                    "Yard Exits");

                Assert.That(bathroom.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Living Room",
                        "Northeast:Hallway"
                    }),
                    "Bathroom Exits");

                Assert.That(livingRoom.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "North:Office",
                        "East:Hallway",
                        "South:Bathroom"
                    }),
                    "Living Room Exits");

                Assert.That(office.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "Southeast:Hallway",
                        "South:Living Room"
                    }),
                    "Office Exits");

                Assert.That(attic.Exits.Select((kvp) => $"{kvp.Key}:{kvp.Value.Name}").ToList(),
                    Is.EquivalentTo(new List<string>()
                    {
                        "Down:Hallway"
                    }),
                    "Attic Exits");
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomHouse_TestCaseData), nameof(TestGameController_CustomHouse_TestCaseData.TestCases_For_Test_GameController_CustomHouse_LocationsWithHidingPlaces_Names_And_HidingPlaces))]
        public void Test_GameController_CustomHouse_LocationsWithHidingPlaces_Names_And_HidingPlaces(GameController gameController)
        {
            // Initialize variables to LocationWithHidingPlace objects by names
            LocationWithHidingPlace bedroom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Bedroom").First();
            LocationWithHidingPlace closet = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Closet").First();
            LocationWithHidingPlace sensoryRoom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Sensory Room").First();
            LocationWithHidingPlace kitchen = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Kitchen").First();
            LocationWithHidingPlace cellar = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Cellar").First();
            LocationWithHidingPlace pantry = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Pantry").First();
            LocationWithHidingPlace yard = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Yard").First();
            LocationWithHidingPlace bathroom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Bathroom").First();
            LocationWithHidingPlace livingRoom = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Living Room").First();
            LocationWithHidingPlace office = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Office").First();
            LocationWithHidingPlace attic = gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name == "Attic").First();

            // Assume no exceptions thrown when getting LocationWithHidingPlace objects from associated properties
            // Check LocationWithHidingPlace hiding place names
            Assert.Multiple(() =>
            {
                Assert.That(bedroom.HidingPlace, Is.EqualTo("under the bed"), "Bedroom hiding place");
                Assert.That(closet.HidingPlace, Is.EqualTo("between the coats"), "Closet hiding place");
                Assert.That(sensoryRoom.HidingPlace, Is.EqualTo("under the beanbags"), "Sensory Room hiding place");
                Assert.That(kitchen.HidingPlace, Is.EqualTo("beside the stove"), "Kitchen hiding place");
                Assert.That(cellar.HidingPlace, Is.EqualTo("behind the canned goods"), "Cellar hiding place");
                Assert.That(pantry.HidingPlace, Is.EqualTo("behind the food"), "Pantry hiding place");
                Assert.That(yard.HidingPlace, Is.EqualTo("behind a bush"), "Yard hiding place");
                Assert.That(bathroom.HidingPlace, Is.EqualTo("in the tub"), "Bathroom hiding place");
                Assert.That(livingRoom.HidingPlace, Is.EqualTo("behind the sofa"), "Living Room hiding place");
                Assert.That(office.HidingPlace, Is.EqualTo("under the desk"), "Office hiding place");
                Assert.That(attic.HidingPlace, Is.EqualTo("behind a trunk"), "Attic hiding place");
            });
        }

        /// <summary>
        /// Using ParseInput method, mimic full game and check ParseInput return message and GameController's public properties:
        /// Prompt, Status, MoveNumber, GameOver
        /// 
        /// CREDIT: adapted from HideAndSeek project's GameControllerTests class's TestParseCheck() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/GameControllerTests.cs
        ///         Link valid as of 02-25-2025
        ///         
        /// CHANGES:
        /// -I am using a different House layout.
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I used a custom method to hide Opponents.
        /// -I added/edited some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [TestCaseSource(typeof(TestGameController_CustomHouse_TestCaseData), nameof(TestGameController_CustomHouse_TestCaseData.TestCases_For_Test_GameController_CustomHouse_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties))]
        public void Test_GameController_CustomHouse_ParseInput_ForFullGame_WithOpponentsHiding_AndCheckMessageAndProperties(GameController gameController)
        {
            // Hide Opponents is specific hiding places
            gameController.RehideAllOpponents(new List<string>() { "Closet", "Yard", "Cellar", "Attic", "Yard" });

            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the North" +
                    Environment.NewLine + "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "check game move number");

                // Attempt to check the starting point (Landing) for players
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("There is no hiding place in the Landing"), "check string returned when check in StartingPoint");
                Assert.That(gameController.MoveNumber, Is.EqualTo(2), "check game move number");

                // Move to the Hallway
                gameController.ParseInput("North");
                Assert.That(gameController.MoveNumber, Is.EqualTo(3), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" +
                    Environment.NewLine + " - the Attic is Up" +
                    Environment.NewLine + " - the Pantry is to the Southeast" +
                    Environment.NewLine + " - the Sensory Room is to the Northeast" +
                    Environment.NewLine + " - the Kitchen is to the East" +
                    Environment.NewLine + " - the Bedroom is to the North" +
                    Environment.NewLine + " - the Landing is to the South" +
                    Environment.NewLine + " - the Living Room is to the West" +
                    Environment.NewLine + " - the Bathroom is to the Southwest" +
                    Environment.NewLine + " - the Office is to the Northwest" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move North to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go: "), "check prompt after move North to Hallway");

                // Attempt to check the Hallway for players
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("There is no hiding place in the Hallway"), "check string returned when check in Hallway");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "check game move number");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go: "), "check prompt after move North to Hallway");

                // Move to the Bathroom
                gameController.ParseInput("Southwest");
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Northeast" +
                    Environment.NewLine + " - the Living Room is to the North" +
                    Environment.NewLine + "Someone could hide in the tub" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move to Bathroom");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "check prompt after move to Bathroom");

                // Check the Bathroom for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding in the tub"), "check string returned when check in Bathroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(6), "check game move number");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bathroom");

                // Move to the Living Room
                gameController.ParseInput("North");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Living Room. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the East" +
                    Environment.NewLine + " - the Office is to the North" +
                    Environment.NewLine + " - the Bathroom is to the South" +
                    Environment.NewLine + "Someone could hide behind the sofa" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move to Living Room");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "check prompt after move to Living Room");

                // Check the Living Room for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding behind the sofa"), "check string returned when check in Living Room");
                Assert.That(gameController.MoveNumber, Is.EqualTo(8), "check game move number");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go (or type 'check'): "), "check prompt after check in Living Room");

                // Move to the Office
                gameController.ParseInput("North");
                Assert.That(gameController.MoveNumber, Is.EqualTo(9), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Office. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is to the Southeast" +
                    Environment.NewLine + " - the Living Room is to the South" +
                    Environment.NewLine + "Someone could hide under the desk" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move to Office");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after move to Office");

                // Check the Office for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding under the desk"), "check string returned when check in Office");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "check game move number");

                // Move to the Hallway
                gameController.ParseInput("Southeast");
                Assert.That(gameController.MoveNumber, Is.EqualTo(11), "check game move number");

                // Move to the Bedroom
                gameController.ParseInput("North");
                Assert.That(gameController.MoveNumber, Is.EqualTo(12), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" +
                    Environment.NewLine + " - the Sensory Room is to the East" +
                    Environment.NewLine + " - the Closet is to the North" +
                    Environment.NewLine + "Someone could hide under the bed" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move to Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "check prompt after move to Bedroom");

                // Check the Bedroom for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding under the bed"), "check string returned when check in Bedroom");
                Assert.That(gameController.MoveNumber, Is.EqualTo(13), "check game move number");
                
                // Move to the Closet
                gameController.ParseInput("North");
                Assert.That(gameController.MoveNumber, Is.EqualTo(14), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Closet. You see the following exits:" +
                    Environment.NewLine + " - the Bedroom is to the South" +
                    Environment.NewLine + "Someone could hide between the coats" +
                    Environment.NewLine + "You have not found any opponents"), "check status when move to Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "check prompt after move to Closet");

                // Check the Closet for players - 1 (Joe) hiding there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding between the coats"), "check string returned when check in Closet and find 1 opponent");
                Assert.That(gameController.MoveNumber, Is.EqualTo(15), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Closet. You see the following exits:" +
                    Environment.NewLine + " - the Bedroom is to the South" +
                    Environment.NewLine + "Someone could hide between the coats" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status after check Closet");
                Assert.That(gameController.Prompt, Is.EqualTo("15: Which direction do you want to go (or type 'check'): "), "check prompt after check Closet");

                // Move to the Bedroom
                gameController.ParseInput("South");
                Assert.That(gameController.MoveNumber, Is.EqualTo(16), "check game move number");

                // Move to the Sensory Room
                gameController.ParseInput("East");
                Assert.That(gameController.MoveNumber, Is.EqualTo(17), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Sensory Room. You see the following exits:" +
                    Environment.NewLine + " - the Bedroom is to the West" +
                    Environment.NewLine + " - the Hallway is to the Southwest" +
                    Environment.NewLine + "Someone could hide under the beanbags" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status when move to Sensory Room");
                Assert.That(gameController.Prompt, Is.EqualTo("17: Which direction do you want to go (or type 'check'): "), "check prompt after move to Sensory Room");

                // Check the Sensory Room for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding under the beanbags"), "check string returned when check in Sensory Room");
                Assert.That(gameController.MoveNumber, Is.EqualTo(18), "check game move number");

                // Move to the Hallway
                gameController.ParseInput("Southwest");
                Assert.That(gameController.MoveNumber, Is.EqualTo(19), "check game move number");

                // Move to the Kitchen
                gameController.ParseInput("East");
                Assert.That(gameController.MoveNumber, Is.EqualTo(20), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" +
                    Environment.NewLine + " - the Pantry is to the South" +
                    Environment.NewLine + " - the Hallway is to the West" +
                    Environment.NewLine + " - the Cellar is Down" +
                    Environment.NewLine + " - the Yard is Out" +
                    Environment.NewLine + "Someone could hide beside the stove" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status when move East to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("20: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Kitchen");

                // Check the Kitchen for players - none there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("Nobody was hiding beside the stove"), "check string returned when check in Kitchen");
                Assert.That(gameController.MoveNumber, Is.EqualTo(21), "check game move number");

                // Move to the Yard
                gameController.ParseInput("Out");
                Assert.That(gameController.MoveNumber, Is.EqualTo(22), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Yard. You see the following exits:" +
                    Environment.NewLine + " - the Kitchen is In" +
                    Environment.NewLine + "Someone could hide behind a bush" +
                    Environment.NewLine + "You have found 1 of 5 opponents: Joe"), "check status when move to Yard");
                Assert.That(gameController.Prompt, Is.EqualTo("22: Which direction do you want to go (or type 'check'): "), "check prompt after move to Yard");

                // Check the Yard for players - 2 (Bob and Jimmy) hiding there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 2 opponents hiding behind a bush"), "check string returned when check in Yard and find 2 opponents");
                Assert.That(gameController.MoveNumber, Is.EqualTo(23), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Yard. You see the following exits:" +
                    Environment.NewLine + " - the Kitchen is In" +
                    Environment.NewLine + "Someone could hide behind a bush" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Joe, Bob, Jimmy"), "check status after check Yard");
                Assert.That(gameController.Prompt, Is.EqualTo("23: Which direction do you want to go (or type 'check'): "), "check prompt after check Yard");

                // Move to the Kitchen
                gameController.ParseInput("In");
                Assert.That(gameController.MoveNumber, Is.EqualTo(24), "check game move number");

                // Move to the Cellar
                gameController.ParseInput("Down");
                Assert.That(gameController.MoveNumber, Is.EqualTo(25), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Cellar. You see the following exits:" +
                    Environment.NewLine + " - the Kitchen is Up" +
                    Environment.NewLine + "Someone could hide behind the canned goods" +
                    Environment.NewLine + "You have found 3 of 5 opponents: Joe, Bob, Jimmy"), "check status when move to Cellar");
                Assert.That(gameController.Prompt, Is.EqualTo("25: Which direction do you want to go (or type 'check'): "), "check prompt after move to Cellar");

                // Check the Cellar for players - 1 (Ana) hiding there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding behind the canned goods"), "check string returned when check in Cellar and find 1 opponent");
                Assert.That(gameController.MoveNumber, Is.EqualTo(26), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Cellar. You see the following exits:" +
                    Environment.NewLine + " - the Kitchen is Up" +
                    Environment.NewLine + "Someone could hide behind the canned goods" +
                    Environment.NewLine + "You have found 4 of 5 opponents: Joe, Bob, Jimmy, Ana"), "check status after check Cellar");
                Assert.That(gameController.Prompt, Is.EqualTo("26: Which direction do you want to go (or type 'check'): "), "check prompt after check Cellar");

                // Move to the Kitchen
                gameController.ParseInput("Up");
                Assert.That(gameController.MoveNumber, Is.EqualTo(27), "check game move number");

                // Move to the Hallway
                gameController.ParseInput("West");
                Assert.That(gameController.MoveNumber, Is.EqualTo(28), "check game move number");

                // Move to the Attic
                gameController.ParseInput("Up");
                Assert.That(gameController.MoveNumber, Is.EqualTo(29), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is Down" +
                    Environment.NewLine + "Someone could hide behind a trunk" +
                    Environment.NewLine + "You have found 4 of 5 opponents: Joe, Bob, Jimmy, Ana"), "check status when move to Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("29: Which direction do you want to go (or type 'check'): "), "check prompt after move to Attic");

                // Check the Attic for players - 1 (Owen) hiding there
                Assert.That(gameController.ParseInput("Check"), Is.EqualTo("You found 1 opponent hiding behind a trunk"), "check string returned when check in Attic and find 1 opponent");
                Assert.That(gameController.MoveNumber, Is.EqualTo(30), "check game move number");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exits:" +
                    Environment.NewLine + " - the Hallway is Down" +
                    Environment.NewLine + "Someone could hide behind a trunk" +
                    Environment.NewLine + "You have found 5 of 5 opponents: Joe, Bob, Jimmy, Ana, Owen"), "check status after check Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("30: Which direction do you want to go (or type 'check'): "), "check prompt after check Attic");
                
                // Check that game is over
                Assert.That(gameController.GameOver, Is.True, "check game over after all opponents found");
            });
        }
    }
}
