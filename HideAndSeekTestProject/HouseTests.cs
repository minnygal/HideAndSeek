using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for testing House layout and functions 
    /// (get Location or LocationWithHidingPlace by name, get random exit, get random LocationWithHidingPlace, 
    /// clear LocationWithHidingPlaces, find out whether Location or LocationWithHidingPlace exists)
    /// </summary>
    [TestFixture]
    public class HouseTests
    {
        /// <summary>
        /// Assert that layout of House is as expected using House's GetExit method and Location's Name property
        /// 
        /// CREDIT: adapted from HideAndSeek project's HouseTests class's TestLayout() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/HouseTests.cs
        ///         Link valid as of 02-26-2025
        ///         
        /// CHANGES:
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I added some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// </summary>
        [Test]
        [Category("House Layout")]
        public void Test_House_Layout()
        {
            Assert.Multiple(() =>
            {
                // Assert that name of Entry is correct
                Assert.That(House.Entry.Name, Is.EqualTo("Entry"), "name of Entry");

                // Assert that Garage is located "Out" from Entry
                var garage = House.Entry.GetExit(Direction.Out);
                Assert.That(garage.Name, Is.EqualTo("Garage"), "Garage is Out from Entry");

                // Assert that Hallway is located "East" of Entry
                var hallway = House.Entry.GetExit(Direction.East);
                Assert.That(hallway.Name, Is.EqualTo("Hallway"), "Hallway is East of Entry");

                // Assert that Kitchen is located "Northwest" of Hallway
                var kitchen = hallway.GetExit(Direction.Northwest);
                Assert.That(kitchen.Name, Is.EqualTo("Kitchen"), "Kitchen is Northwest of Hallway");

                // Assert that Bathroom is located "North" of Hallway
                var bathroom = hallway.GetExit(Direction.North);
                Assert.That(bathroom.Name, Is.EqualTo("Bathroom"), "Bathroom is North of Hallway");

                // Assert that Living Room is located "South" of Hallway
                var livingRoom = hallway.GetExit(Direction.South);
                Assert.That(livingRoom.Name, Is.EqualTo("Living Room"), "Living Room is South of Hallway");

                // Assert that Landing is located "Up" from Hallway
                var landing = hallway.GetExit(Direction.Up);
                Assert.That(landing.Name, Is.EqualTo("Landing"), "Landing is Up from Hallway");

                // Assert that Master Bedroom is located "Northwest" of Landing
                var masterBedroom = landing.GetExit(Direction.Northwest);
                Assert.That(masterBedroom.Name, Is.EqualTo("Master Bedroom"), "Master Bedroom is Northwest of Landing");

                // Assert that Master Bath is located "East" of Master Bedroom
                var masterBath = masterBedroom.GetExit(Direction.East);
                Assert.That(masterBath.Name, Is.EqualTo("Master Bath"), "Master Bath is East of Master Bedroom");

                // Assert that Second Bathroom is located "West" of Landing
                var secondBathroom = landing.GetExit(Direction.West);
                Assert.That(secondBathroom.Name, Is.EqualTo("Second Bathroom"), "Second Bathroom is West of Landing");

                // Assert that Nursery is located Southwest of Landing
                var nursery = landing.GetExit(Direction.Southwest);
                Assert.That(nursery.Name, Is.EqualTo("Nursery"), "Nursery is Southwest of Landing");

                // Assert that Pantry is located South of Landing
                var pantry = landing.GetExit(Direction.South);
                Assert.That(pantry.Name, Is.EqualTo("Pantry"), "Pantry is South of Landing");

                // Assert that Kids Room is located "Southeast" of Landing
                var kidsRoom = landing.GetExit(Direction.Southeast);
                Assert.That(kidsRoom.Name, Is.EqualTo("Kids Room"), "Kids Room is Southeast of Landing");

                // Assert that Attic is located "Up" from Landing
                var attic = landing.GetExit(Direction.Up);
                Assert.That(attic.Name, Is.EqualTo("Attic"), "Attic is Up from Landing");
            });
        }

        [Test]
        [Category("House GetLocationByName Success")]
        public void Test_House_GetLocationByName_WhenNameFound_ReturnThatLocation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationByName("Entry").Name, Is.EqualTo("Entry"), "get Entry");
                Assert.That(House.GetLocationByName("Attic").Name, Is.EqualTo("Attic"), "get Attic");
                Assert.That(House.GetLocationByName("Garage").Name, Is.EqualTo("Garage"), "get Garage");
                Assert.That(House.GetLocationByName("Master Bedroom").Name, Is.EqualTo("Master Bedroom"), "get Master Bedroom");
            });
        }

        [Test]
        [Category("House GetLocationByName Failure")]
        public void Test_House_GetLocationByName_WhenNameNotFound_ReturnNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationByName("Secret Library"), Is.Null, "try \"Secret Library\"");
                Assert.That(House.GetLocationByName("master bedroom"), Is.Null, "try \"master bedroom\"");
                Assert.That(House.GetLocationByName("MasterBedroom"), Is.Null, "try \"MasterBedroom\"");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Success")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenFound_ReturnThatLocation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationWithHidingPlaceByName("Pantry").Name, Is.EqualTo("Pantry"), "get Pantry");
                Assert.That(House.GetLocationWithHidingPlaceByName("Attic").Name, Is.EqualTo("Attic"), "get Attic");
                Assert.That(House.GetLocationWithHidingPlaceByName("Garage").Name, Is.EqualTo("Garage"), "get Garage");
                Assert.That(House.GetLocationWithHidingPlaceByName("Master Bedroom").Name, Is.EqualTo("Master Bedroom"), "get Master Bedroom");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenNotFound_BecauseLocationIsNotHidingPlaceType_ReturnsNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationWithHidingPlaceByName("Entry"), Is.Null, "try \"Entry\"");
                Assert.That(House.GetLocationWithHidingPlaceByName("Hallway"), Is.Null, "try \"Hallway\"");
                Assert.That(House.GetLocationWithHidingPlaceByName("Landing"), Is.Null, "try \"Landing\"");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenNotFound_BecauseNoLocationWithThatName_ReturnsNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationWithHidingPlaceByName("Secret Library"), Is.Null, "try \"Secret Library\"");
                Assert.That(House.GetLocationWithHidingPlaceByName("master bedroom"), Is.Null, "try \"master bedroom\"");
                Assert.That(House.GetLocationWithHidingPlaceByName("MasterBedroom"), Is.Null, "try \"MasterBedroom\"");
            });
        }

        [Test]
        [Category("House DoesLocationExist Success")]
        public void Test_House_DoesLocationExist_ReturnsTrue() 
        {
            Assert.That(House.DoesLocationExist("Entry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationExist Failure")]
        public void Test_House_DoesLocationExist_ReturnsFalse()
        {
            Assert.That(House.DoesLocationExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Success")]
        public  void Test_House_DoesLocationWithHidingPlaceExist_ReturnsTrue()
        {
            Assert.That(House.DoesLocationWithHidingPlaceExist("Pantry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Failure")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenNoLocationExists()
        {
            Assert.That(House.DoesLocationWithHidingPlaceExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Failure")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenLocationIsNotLocationWithHidingPlace()
        {
            Assert.That(House.DoesLocationWithHidingPlaceExist("Landing"), Is.False);
        }

        /// <summary>
        /// Assert that GetRandomExit method returns appropriate Location using mock of Random
        /// 
        /// CREDIT: adapted from HideAndSeek project's HouseTests class's TestRandomExit() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/HouseTests.cs
        ///         Link valid as of 02-26-2025
        /// 
        /// CHANGES:
        /// -I changed the method name to be consistent with the conventions I'm using in this test project.
        /// -I put all the assertions in the body of a multiple assert so all assertions will be run.
        /// -I changed the assertions to use the constraint model to stay up-to-date.
        /// -I added some comments for easier reading.
        /// -I added messages to the assertions to make them easier to debug.
        /// -I moved the GetLocationByName method call for getting the Kitchen Location to the beginning of the test method.
        /// </summary>
        [Test]
        [Category("House GetRandomExit")]
        public void Test_House_GetRandomExit()
        {
            // Get locations
            var landing = House.GetLocationByName("Landing");
            var kitchen = House.GetLocationByName("Kitchen");

            Assert.Multiple(() =>
            {
                // Assert Landing's random exit at index 0 is Attic
                House.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Attic"), "Landing exit at index 0");

                // Assert Landing's random exit at index 1 is Kids Room
                House.Random = new MockRandom() { ValueToReturn = 1 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Kids Room"), "Landing exit at index 1");

                // Assert Landing's random exit at index 2 is Pantry
                House.Random = new MockRandom() { ValueToReturn = 2 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Pantry"), "Landing exit at index 2");

                // Assert Landing's random exit at index 3 is Second Bathroom
                House.Random = new MockRandom() { ValueToReturn = 3 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Second Bathroom"), "Landing exit at index 3");

                // Assert Landing's random exit at index 4 is Nursery
                House.Random = new MockRandom() { ValueToReturn = 4 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Nursery"), "Landing exit at index 4");

                // Assert Landing's random exit at index 5 is Master Bedroom
                House.Random = new MockRandom() { ValueToReturn = 5 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Master Bedroom"), "Landing exit at index 5");

                // Assert Landing's random exit at index 6 is Hallway
                House.Random = new MockRandom() { ValueToReturn = 6 };
                Assert.That(House.GetRandomExit(landing).Name, Is.EqualTo("Hallway"), "Landing exit at index 6");

                // Assert Kitchen's random exit at index 0 is Hallway
                House.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(House.GetRandomExit(kitchen).Name, Is.EqualTo("Hallway"), "Kitchen exit at index 0");
            });
        }

        [TestCase("Kitchen", 1, 0, 4, 0)] // no extra moves
        [TestCase("Pantry", 0, 1, 2)] // 1 extra move
        [TestCase("Bathroom", 1, 2, 3)] // 2 extra moves
        [Category("House GetRandomLocationWithHidingPlace")]
        public void Test_House_GetRandomHidingPlace(string hidingLocationName, params int[] mockRandomValueList)
        {
            House.Random = new MockRandomWithValueList(mockRandomValueList); // Set House's Random number generator to mock
            Assert.That(House.GetRandomLocationWithHidingPlace().Name, Is.EqualTo(hidingLocationName)); // Assert that name of LocationWithHidingPlace returned is as expected
        }

        [Test]
        [Category("House LocationTypes")]
        public void Test_House_LocationsAreOfType_LocationWithHidingPlace()
        {
            Assert.Multiple(() =>
            {
                Assert.That(House.GetLocationByName("Garage"), Is.InstanceOf<LocationWithHidingPlace>(), "Garage");
                Assert.That(House.GetLocationByName("Kitchen"), Is.InstanceOf<LocationWithHidingPlace>(), "Kitchen");
                Assert.That(House.GetLocationByName("Living Room"), Is.InstanceOf<LocationWithHidingPlace>(), "Living Room");
                Assert.That(House.GetLocationByName("Bathroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Bathroom");
                Assert.That(House.GetLocationByName("Master Bedroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Master Bedroom");
                Assert.That(House.GetLocationByName("Master Bath"), Is.InstanceOf<LocationWithHidingPlace>(), "Master Bath");
                Assert.That(House.GetLocationByName("Second Bathroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Second Bathroom");
                Assert.That(House.GetLocationByName("Kids Room"), Is.InstanceOf<LocationWithHidingPlace>(), "Kids Room");
                Assert.That(House.GetLocationByName("Nursery"), Is.InstanceOf<LocationWithHidingPlace>(), "Nursery");
                Assert.That(House.GetLocationByName("Pantry"), Is.InstanceOf<LocationWithHidingPlace>(), "Pantry");
                Assert.That(House.GetLocationByName("Attic"), Is.InstanceOf<LocationWithHidingPlace>(), "Attic");
            });
        }

        [Test]
        [Category("House ClearHidingPlaces CheckHidingPlace")]
        public void Test_House_ClearHidingPlaces()
        {
            // Hide opponent in garage
            var garage = House.GetLocationWithHidingPlaceByName("Garage"); // Get garage reference
            garage.HideOpponent(new Opponent());

            // Hide 3 more opponents in attic
            var attic = House.GetLocationWithHidingPlaceByName("Attic"); // Get attic reference
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());

            // Clear hiding places in house
            House.ClearHidingPlaces();

            // Assert that no opponents are in cleared hiding places
            Assert.Multiple(() =>
            {
                Assert.That(garage.CheckHidingPlace(), Is.Empty, "no opponents in garage");
                Assert.That(attic.CheckHidingPlace(), Is.Empty, "no opponents in attic");
            });
        }
    }
}