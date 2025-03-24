using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to test layout of GameController non-default House
    /// set using GameController constructor or RestartGame method 
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

                // Check LocationWithHidingPlace exit lists
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

            // Assume no exceptions thrown when getting Location and LocationWithHidingPlace objects from associated properties
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
    }
}
