﻿using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Newtonsoft.Json.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for layout and functions 
    /// (get Location or LocationWithHidingPlace by name, get random exit, get random LocationWithHidingPlace, 
    /// clear LocationWithHidingPlace, find out whether Location or LocationWithHidingPlace exists,
    /// [de]serialization, property setters, etc.)
    /// </summary>
    [TestFixture]
    public class TestHouse
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
            house = TestHouse_TestData.GetDefaultHouse();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        /// <summary>
        /// Assert that layout of House is as expected using House's GetExit method and Location's Name property
        /// 
        /// CREDIT: adapted from HideAndSeek project's TestHouse class's TestLayout() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/TestHouse.cs
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
        [Category("House Layout GetExit")]
        public void Test_House_Layout()
        {
            Assert.Multiple(() =>
            {
                // Assert that name of StartingPoint is correct
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"), "name of starting point");

                // Assert that Garage is located "Out" from StartingPoint
                var garage = house.StartingPoint.GetExit(Direction.Out);
                Assert.That(garage.Name, Is.EqualTo("Garage"), "Garage is Out from Entry");

                // Assert that Hallway is located "East" of StartingPoint
                var hallway = house.StartingPoint.GetExit(Direction.East);
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

        [TestCase("Entry")]
        [TestCase("Hallway")]
        [TestCase("Landing")]
        [Category("House LocationType")]
        public void Test_House_Locations_AreNotOfType_LocationWithHidingPlace(string locationName)
        {
            Assert.That(house.Locations.Where((l) => l.Name == locationName).First(), Is.Not.InstanceOf<LocationWithHidingPlace>());
        }

        [TestCase("Garage")]
        [TestCase("Kitchen")]
        [TestCase("Living Room")]
        [TestCase("Bathroom")]
        [TestCase("Master Bedroom")]
        [TestCase("Master Bath")]
        [TestCase("Second Bathroom")]
        [TestCase("Kids Room")]
        [TestCase("Nursery")]
        [TestCase("Pantry")]
        [TestCase("Attic")]
        [Category("House LocationType")]
        public void Test_House_Locations_AreOfType_LocationWithHidingPlace(string locationName)
        {
            Assert.That(house.Locations.Where((l) => l.Name == locationName).First(), Is.InstanceOf<LocationWithHidingPlace>());
        }

        [Category("House GetLocationByName Success")]
        public void Test_House_GetLocationByName_ReturnsLocation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationByName("Entry").Name, Is.EqualTo("Entry"));
            });
        }

        [TestCase("Secret Library")]
        [TestCase("master bedroom")]
        [TestCase("MasterBedroom")]
        [TestCase(" ")]
        [TestCase("")]
        [Category("House GetLocationByName InvalidOperationException Failure")]
        public void Test_House_GetLocationByName_AndCheckErrorMessage_WhenLocationWithName_DoesNotExist(string locationName)
        {
            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() => house.GetLocationByName(locationName));
                Assert.That(exception.Message, Is.EqualTo("location \"" + locationName + "\" does not exist in House"));
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Success")]
        public void Test_House_GetLocationWithHidingPlaceByName_ReturnsLocation()
        {
            Assert.That(house.GetLocationWithHidingPlaceByName("Pantry").Name, Is.EqualTo("Pantry"));
        }

        [TestCase("Entry")]
        [TestCase("Hallway")]
        [TestCase("Landing")]
        [Category("House GetLocationWithHidingPlaceByName InvalidOperationException Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_AndCheckErrorMessage_WhenLocationDoesNotHaveHidingPlace(string locationName)
        {
            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() => house.GetLocationWithHidingPlaceByName(locationName));
                Assert.That(exception.Message, Is.EqualTo($"location with hiding place \"{locationName}\" does not exist in House"));
            });
        }

        [TestCase("Secret Library")]
        [TestCase("master bedroom")]
        [TestCase("MasterBedroom")]
        [TestCase(" ")]
        [TestCase("")]
        [Category("House GetLocationWithHidingPlaceByName InvalidOperationException Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_AndCheckErrorMessage_WhenNoLocationWithNameExistsInHouse(string locationName)
        {
            Assert.Multiple(() =>
            {
                Exception exception = Assert.Throws<InvalidOperationException>(() => house.GetLocationWithHidingPlaceByName(locationName));
                Assert.That(exception.Message, Is.EqualTo($"location with hiding place \"{locationName}\" does not exist in House"));
            });
        }

        [Test]
        [Category("House DoesLocationExist Success")]
        public void Test_House_DoesLocationExist_ReturnsTrue()
        {
            Assert.That(house.DoesLocationExist("Entry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationExist Failure")]
        public void Test_House_DoesLocationExist_ReturnsFalse()
        {
            Assert.That(house.DoesLocationExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Success")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsTrue()
        {
            Assert.That(house.DoesLocationWithHidingPlaceExist("Pantry"), Is.True);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Failure")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenLocationDoesNotExist()
        {
            Assert.That(house.DoesLocationWithHidingPlaceExist("Dungeon"), Is.False);
        }

        [Test]
        [Category("House DoesLocationWithHidingPlaceExist Failure")]
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenLocationIsNotLocationWithHidingPlace()
        {
            Assert.That(house.DoesLocationWithHidingPlaceExist("Landing"), Is.False);
        }

        /// <summary>
        /// Assert that GetRandomExit method returns appropriate Location using mock of Random
        /// 
        /// CREDIT: adapted from HideAndSeek project's TestHouse class's TestRandomExit() test method
        ///         © 2023 Andrew Stellman and Jennifer Greene
        ///         Published under the MIT License
        ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/TestHouse.cs
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
        [Category("House GetRandomExit Success")]
        public void Test_House_GetRandomExit()
        {
            // Get locations
            Location landing = house.Locations.Where((l) => l.Name == "Landing").First();
            Location kitchen = house.Locations.Where((l) => l.Name == "Kitchen").First();

            Assert.Multiple(() =>
            {
                // Assert Landing's random exit at index 0 is Attic
                House.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Attic"), "Landing exit at index 0");

                // Assert Landing's random exit at index 1 is Kids Room
                House.Random = new MockRandom() { ValueToReturn = 1 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Kids Room"), "Landing exit at index 1");

                // Assert Landing's random exit at index 2 is Pantry
                House.Random = new MockRandom() { ValueToReturn = 2 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Pantry"), "Landing exit at index 2");

                // Assert Landing's random exit at index 3 is Second Bathroom
                House.Random = new MockRandom() { ValueToReturn = 3 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Second Bathroom"), "Landing exit at index 3");

                // Assert Landing's random exit at index 4 is Nursery
                House.Random = new MockRandom() { ValueToReturn = 4 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Nursery"), "Landing exit at index 4");

                // Assert Landing's random exit at index 5 is Master Bedroom
                House.Random = new MockRandom() { ValueToReturn = 5 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Master Bedroom"), "Landing exit at index 5");

                // Assert Landing's random exit at index 6 is Hallway
                House.Random = new MockRandom() { ValueToReturn = 6 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Hallway"), "Landing exit at index 6");

                // Assert Kitchen's random exit at index 0 is Hallway
                House.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(house.GetRandomExit(kitchen).Name, Is.EqualTo("Hallway"), "Kitchen exit at index 0");
            });
        }

        [TestCase("Kitchen", 1, 0, 4, 0)] // no extra moves
        [TestCase("Pantry", 0, 1, 2)] // 1 extra move
        [TestCase("Bathroom", 1, 2, 3)] // 2 extra moves
        [Category("House GetRandomLocationWithHidingPlace Success")]
        public void Test_House_GetRandomLocationWithHidingPlace(string hidingLocationName, params int[] mockRandomValueList)
        {
            House.Random = new MockRandomWithValueList(mockRandomValueList); // Set House's Random number generator to mock
            Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo(hidingLocationName)); // Assert that name of LocationWithHidingPlace returned is as expected
        }

        [Test]
        [Category("House ClearHidingPlaces CheckHidingPlace HideOpponent")]
        public void Test_House_ClearHidingPlaces()
        {
            // ARRANGE
            // Hide opponent in Garage
            LocationWithHidingPlace garage = (LocationWithHidingPlace)house.Locations.Where((l) => l.Name == "Garage").First(); // Get Garage reference
            garage.HideOpponent(new Opponent());

            // Hide 3 more Opponents in Attic
            LocationWithHidingPlace attic = (LocationWithHidingPlace)house.Locations.Where((l) => l.Name == "Attic").First(); // Get Attic reference
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());

            // ACT
            // Clear hiding places in House
            house.ClearHidingPlaces();

            // ASSERT
            // Assert that no Opponents are in cleared hiding places
            Assert.Multiple(() =>
            {
                Assert.That(garage.CheckHidingPlace(), Is.Empty, "no Opponents in Garage");
                Assert.That(attic.CheckHidingPlace(), Is.Empty, "no Opponents in Attic");
            });
        }

        [Test]
        [Category("House CreateHouse Success")]
        public void Test_House_CreateHouse_WithLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                               "DefaultHouse.json", TestHouse_TestData.DefaultHouse_Serialized);

            // ACT
            // Call method to create House
            House house = House.CreateHouse("DefaultHouse");

            // PRE-ASSERT ARRANGE
            // Set variable for Landing from LocationsWithoutHidingPlaces
            Location landingFromLocationsWithoutHidingPlaces = house.LocationsWithoutHidingPlaces.Where((l) => l.Name == "Landing").First();
            
            // Set variables for expected Exits for Landing (taking objects from House Locations property)
            Location hallwayFromLocations = house.Locations.Where((l) => l.Name == "Hallway").First();
            Location atticFromLocations = house.Locations.Where((l) => l.Name == "Attic").First();
            Location kidsRoomFromLocations = house.Locations.Where((l) => l.Name == "Kids Room").First();
            Location masterBedroomFromLocations = house.Locations.Where((l) => l.Name == "Master Bedroom").First();
            Location nurseryFromLocations = house.Locations.Where((l) => l.Name == "Nursery").First();
            Location pantryFromLocations = house.Locations.Where((l) => l.Name == "Pantry").First();
            Location secondBathroomFromLocations = house.Locations.Where((l) => l.Name == "Second Bathroom").First();
            
            // Set variable for Master Bedroom from LocationsWithHidingPlaces
            LocationWithHidingPlace masterBedroomFromLocationsWithHidingPlaces = house.LocationsWithHidingPlaces.Where((l) => l.Name == "Master Bedroom").First();
            
            // Set variables for expected Exits for Master Bedroom (taking objects from House Locations property)
            Location landingFromLocations = house.Locations.Where((l) => l.Name == "Landing").First();
            Location masterBathFromLocations = house.Locations.Where((l) => l.Name == "Master Bath").First();

            // ASSERT
            Assert.Multiple(() =>
            {
                // Assert that House properties are as expected
                Assert.That(house.Name, Is.EqualTo("my house"), "Name property");
                Assert.That(house.HouseFileName, Is.EqualTo("DefaultHouse"), "HouseFileName property");
                Assert.That(house.PlayerStartingPoint, Is.EqualTo("Entry"), "PlayerStartingPoint property");
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"), "StartingPoint property Location Name");
                Assert.That(house.Locations.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_Locations), "Locations property (check each Location Name)");
                Assert.That(house.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_LocationsWithoutHidingPlaces), "LocationsWithoutHidingPlaces property (check each Location Name)");
                Assert.That(house.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_LocationsWithHidingPlaces), "LocationsWithHidingPlaces property (check each LocationWithHidingPlace Name");

                //Assert that Landing Location Name property is as expected
                Assert.That(landingFromLocationsWithoutHidingPlaces.Name, Is.EqualTo("Landing"), "Landing Location Name property");
                
                // Assert that Landing Location Exits properties' keys and values are as expected
                // (and values are the same objects as those stored in the House Locations property)
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(0).Key, Is.EqualTo(Direction.Down), "1st Landing Exits key is Down");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(0).Value, Is.SameAs(hallwayFromLocations), "1st Landing Exits value same as Hallway from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(1).Key, Is.EqualTo(Direction.Up), "2nd Landing Exits key is Up");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(1).Value, Is.SameAs(atticFromLocations), "2nd Landing Exits value same as Attic from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(2).Key, Is.EqualTo(Direction.Southeast), "3rd Landing Exits Key is Southeast");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(2).Value, Is.SameAs(kidsRoomFromLocations), "3rd Landing Exits value same as Kids Room from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(3).Key, Is.EqualTo(Direction.Northwest), "4th Landing Exits Key is Northwest");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(3).Value, Is.SameAs(masterBedroomFromLocations), "4th Landing Exits value same as Master Bedroom from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(4).Key, Is.EqualTo(Direction.Southwest), "5th Landing Exits Key is Southwest");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(4).Value, Is.SameAs(nurseryFromLocations), "5th Landing Exits value same as Nursery from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(5).Key, Is.EqualTo(Direction.South), "6th Landing Exits Key is South");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(5).Value, Is.SameAs(pantryFromLocations), "6th Landing Exits value same as Pantry from Locations");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(6).Key, Is.EqualTo(Direction.West), "6th Landing Exits Key is West");
                Assert.That(landingFromLocationsWithoutHidingPlaces.Exits.ElementAt(6).Value, Is.SameAs(secondBathroomFromLocations), "6th Landing Exits value same as Second Bathroom from Locations");

                //Assert that Master Bedroom LocationWithHidingPlace Name and HidingPlace properties are as expected
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.Name, Is.EqualTo("Master Bedroom"), "Master Bedroom LocationWithHidingPlace Name property");
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.HidingPlace, Is.EqualTo("under the bed"), "Master Bedroom LocationWithHidingPlace property");

                // Assert that Master Bedroom LocationWithHidingPlace Exits properties' keys and values are as expected
                // (and values are the same objects as those stored in the House Locations property)
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.Exits.ElementAt(0).Key, Is.EqualTo(Direction.Southeast), "1st Master Bedroom Exits key is Southeast");
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.Exits.ElementAt(0).Value, Is.SameAs(landingFromLocationsWithoutHidingPlaces), "1st Master Bedroom Exits value same as Landing from Locations");
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.Exits.ElementAt(1).Key, Is.EqualTo(Direction.East), "2nd Master Bedroom Exits key is Southeast");
                Assert.That(masterBedroomFromLocationsWithHidingPlaces.Exits.ElementAt(1).Value, Is.SameAs(masterBathFromLocations), "2nd Master Bedroom Exits value same as Landing from Locations");
            });
        }

        [Test]
        [Category("House CreateHouse Success")]
        public void Test_House_CreateHouse_WithNoLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Initialize variable to string representing text in House file (with empty LocationsWithoutHidingPlaces)
            string textInHouseFile = 
                "{" +
                    TestHouse_TestData.DefaultHouse_Serialized_Name + "," +
                    TestHouse_TestData.DefaultHouse_Serialized_HouseFileName + "," +
                    "\"PlayerStartingPoint\":\"Master Bedroom\"" + "," +
                    "\"LocationsWithoutHidingPlaces\":[]" + "," +
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"," +
                            "\"Name\":\"Master Bedroom\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Master Bath\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"HidingPlace\":\"in the tub\"," +
                            "\"Name\":\"Master Bath\"," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Master Bedroom\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";

            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", textInHouseFile);

            // ACT
            // Call method to create House
            House house = House.CreateHouse("DefaultHouse");

            // ASSERT
            // Assume no exceptions were thrown
            // Assert that there are no Locations in LocationsWithoutHidingPlaces
            Assert.That(house.LocationsWithoutHidingPlaces.Count(), Is.EqualTo(0));
        }

        [Test]
        [Category("House CreateHouse FileNotFoundException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDoesNotExist()
        {
            // Set up mock file system and assign to House property
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup((s) => s.File.Exists("MyNonexistentFile.json")).Returns(false);
            House.FileSystem = fileSystemMock.Object;

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a name of a file that does not exist raises an exception
                var exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    House.CreateHouse("MyNonexistentFile");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot load game because house layout file MyNonexistentFile does not exist"));
            });
        }

        [TestCaseSource(typeof(TestHouse_TestData), 
            nameof(TestHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid))]
        [Category("House CreateHouse JsonException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyCorruptFile.json", fileText);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a file with an invalid format throws an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    House.CreateHouse("MyCorruptFile");
                });
                
                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file MyCorruptFile is corrupt - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestHouse_TestData), 
            nameof(TestHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection))]
        [Category("House CreateHouse JsonException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection(string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyCorruptFile.json", fileText);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a file with an invalid Direction value throws an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    House.CreateHouse("MyCorruptFile");
                });

                // Assert that exception message starts with the expected string
                Assert.That(exception.Message, Does.StartWith("Cannot process because data in house layout file MyCorruptFile is corrupt" +
                                                              " - The JSON value could not be converted to HideAndSeek.Direction."));
            });
        }

        [TestCaseSource(typeof(TestHouse_TestData), 
            nameof(TestHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue))]
        [Category("House CreateHouse ArgumentException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.json", fileText);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a file with a whitespace value for a property throws an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    House.CreateHouse("MyInvalidDataFile");
                });
                
                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot process because data in house layout file MyInvalidDataFile is invalid - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestHouse_TestData), 
            nameof(TestHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue))]
        [Category("House CreateHouse ArgumentException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.json", fileText);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a file with an invalid value for a property throws an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    House.CreateHouse("MyInvalidDataFile");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"Cannot process because data in house layout file MyInvalidDataFile is invalid - {exceptionMessageEnding}"));
            });
        }

        [TestCaseSource(typeof(TestHouse_TestData),
            nameof(TestHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation))]
        [Category("House CreateHouse InvalidOperationException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.json", fileText);

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with a file with an invalid value for a property throws an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    House.CreateHouse("MyInvalidDataFile");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot process because data in house layout file MyInvalidDataFile is corrupt - {exceptionMessageEnding}"));
            });
        }

        // Calls properties' setters successfully
        [Test]
        [Category("House Constructor Success")]
        public void Test_House_Constructor_Parameterized_SetsProperties()
        {
            // Assume no exceptions were thrown
            // Assert that properties are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(house.Name, Is.EqualTo("my house"));
                Assert.That(house.HouseFileName, Is.EqualTo("DefaultHouse"));
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"));
                Assert.That(house.PlayerStartingPoint, Is.EqualTo("Entry"));
                Assert.That(house.Locations.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_Locations));
                Assert.That(house.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_LocationsWithoutHidingPlaces));
                Assert.That(house.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(TestHouse_TestData.DefaultHouse_LocationsWithHidingPlaces));
            });
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [Category("House Name ArgumentException Failure")]
        public void Test_House_Set_Name_AndCheckErrorMessage_ForInvalidName(string name)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting House name to an invalid name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.Name = name;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house name \"{name}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("my file")]
        [TestCase(" myFile")]
        [TestCase("myFile ")]
        [TestCase("\\")]
        [TestCase("\\myFile")]
        [TestCase("myFile\\")]
        [TestCase("my\\File")]
        [TestCase("/")]
        [TestCase("/myFile")]
        [TestCase("myFile/")]
        [TestCase("my/File")]
        [Category("House HouseFileName ArgumentException Failure")]
        public void Test_House_Set_HouseFileName_AndCheckErrorMessage_ForInvalidFileName(string houseFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting House file name to an invalid file name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.HouseFileName = houseFileName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"house file name \"{houseFileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("House PlayerStartingPoint ArgumentException Failure")]
        public void Test_House_Set_PlayerStartingPoint_AndCheckErrorMessage_ForInvalidLocationName(string locationName)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting player starting point location name to an invalid location name raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.PlayerStartingPoint = locationName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"player starting point location name \"{locationName}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [Test]
        [Category("House StartingPoint InvalidOperationException Failure")]
        public void Test_House_Set_StartingPoint_AndCheckErrorMessage_ForLocationNotExistingInHouse()
        {
            Assert.Multiple(() =>
            {
                // Assert that setting starting point location to a Location not in the House raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    house.StartingPoint = new Location("not in house");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("player starting point location \"not in house\" does not exist in House"));
            });  
        }

        [Test]
        [Category("House LocationsWithHidingPlaces ArgumentException Failure")]
        public void Test_House_Set_LocationsWithHidingPlaces_AndCheckErrorMessage_ForEmptyEnumerable()
        {
            Assert.Multiple(() =>
            {
                // Assert that setting locations with hiding places property to an empty list raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.LocationsWithHidingPlaces = new List<LocationWithHidingPlace>();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("locations with hiding places list is empty"));
            });
        }

        [Test]
        [Category("House Locations ArgumentException Failure")]
        public void Test_House_Set_Locations_AndCheckErrorMessage_ForEmptyEnumerable()
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the locations property to an empty list raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    house.Locations = new List<Location>();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("locations list is empty"));
            });
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_DefaultHouse()
        {
            Assert.That(house.Serialize(), Is.EqualTo(TestHouse_TestData.DefaultHouse_Serialized));
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_CustomHouse_WithLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Initialize variable to expected serialized House text
            string expectedSerializedHouse =
                #region expected serialized House
                "{" +
                    "\"Name\":\"dream house\"" + "," + 
                    "\"HouseFileName\":\"DreamHouse\"" + "," + 
                    "\"PlayerStartingPoint\":\"Kitchen\"" + "," + 
                    "\"LocationsWithoutHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"Name\":\"Kitchen\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"" + "," + 
                                "\"East\":\"Pantry\"" + "," + 
                                "\"West\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"Name\":\"Exercise Room\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}" +
                    "]" + "," + 
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"" + "," + 
                            "\"Name\":\"Bedroom\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Kitchen\"" + "," + 
                                "\"North\":\"Exercise Room\"" + "," + 
                                "\"East\":\"Closet\"" + "," + 
                                "\"West\":\"Bathroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"in a box\"" + "," + 
                            "\"Name\":\"Pantry\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Kitchen\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"under the desk\"" + "," + 
                            "\"Name\":\"Office\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Kitchen\"" + "," + 
                                "\"Northeast\":\"Bathroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"between the coats\"" + "," + 
                            "\"Name\":\"Closet\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Bedroom\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"in the tub\"" + "," + 
                            "\"Name\":\"Bathroom\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bedroom\"" + "," + 
                                "\"West\":\"Sensory Room\"" + "," + 
                                "\"Southwest\":\"Office\"" +
                            "}" +
                        "}" + "," + 
                        "{" +
                            "\"HidingPlace\":\"under the bean bags\"" + "," + 
                            "\"Name\":\"Sensory Room\"" + "," + 
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bathroom\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            #endregion

            #region create House
            // Create starting point (Kitchen) and connect to new locations: Bedroom, Pantry, Office
            Location kitchen = new Location("Kitchen");
            LocationWithHidingPlace bedroom = kitchen.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace pantry = kitchen.AddExit(Direction.East, "Pantry", "in a box");
            LocationWithHidingPlace office = kitchen.AddExit(Direction.West, "Office", "under the desk");

            // Connect Bedroom to new locations: Exercise Room, Closet, Bathroom
            Location exerciseRoom = bedroom.AddExit(Direction.North, "Exercise Room");
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.East, "Closet", "between the coats");
            LocationWithHidingPlace bathroom = bedroom.AddExit(Direction.West, "Bathroom", "in the tub");

            // Connect Office to new location: Sensory Room
            LocationWithHidingPlace sensoryRoom = bathroom.AddExit(Direction.West, "Sensory Room", "under the bean bags");

            // Connect Office to Bathroom
            office.AddExit(Direction.Northeast, bathroom);

            // Create enumerable of Location objects
            IEnumerable<Location> locationsWithoutHidingPlaces = new List<Location>() { kitchen, exerciseRoom };

            // Create enumerable of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                bedroom, pantry, office, closet, bathroom, sensoryRoom
            };

            // Create House
            house = new House("dream house", "DreamHouse", "Kitchen", locationsWithoutHidingPlaces, locationsWithHidingPlaces);
            #endregion

            // ACT
            string serializedHouse = house.Serialize();

            // Assert that serialized House text is as expected
            Assert.That(serializedHouse, Is.EqualTo(expectedSerializedHouse));
        }

        [Test]
        [Category("House Serialize Success")]
        public void Test_House_SerializeMethod_CustomHouse_WithoutLocationsWithoutHidingPlaces()
        {
            // ARRANGE
            // Initialize variable to expected serialized House text
            string expectedSerializedHouse =
            #region expected serialized House
                "{" +
                    "\"Name\":\"dream house\"" + "," +
                    "\"HouseFileName\":\"DreamHouse\"" + "," +
                    "\"PlayerStartingPoint\":\"Kitchen\"" + "," +
                    "\"LocationsWithoutHidingPlaces\":[]" + "," +
                    "\"LocationsWithHidingPlaces\":" +
                    "[" +
                        "{" +
                            "\"HidingPlace\":\"in a cupboard\"" + "," +
                            "\"Name\":\"Kitchen\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"North\":\"Bedroom\"" + "," +
                                "\"East\":\"Pantry\"" + "," +
                                "\"West\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the bed\"" + "," +
                            "\"Name\":\"Bedroom\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Kitchen\"" + "," +
                                "\"North\":\"Exercise Room\"" + "," +
                                "\"East\":\"Closet\"" + "," +
                                "\"West\":\"Bathroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"in a box\"" + "," +
                            "\"Name\":\"Pantry\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Kitchen\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the desk\"" + "," +
                            "\"Name\":\"Office\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Kitchen\"" + "," +
                                "\"Northeast\":\"Bathroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"behind the balls\"" + "," +
                            "\"Name\":\"Exercise Room\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"South\":\"Bedroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"between the coats\"" + "," +
                            "\"Name\":\"Closet\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"West\":\"Bedroom\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"in the tub\"" + "," +
                            "\"Name\":\"Bathroom\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bedroom\"" + "," +
                                "\"West\":\"Sensory Room\"" + "," +
                                "\"Southwest\":\"Office\"" +
                            "}" +
                        "}" + "," +
                        "{" +
                            "\"HidingPlace\":\"under the bean bags\"" + "," +
                            "\"Name\":\"Sensory Room\"" + "," +
                            "\"ExitsForSerialization\":" +
                            "{" +
                                "\"East\":\"Bathroom\"" +
                            "}" +
                        "}" +
                    "]" +
                "}";
            #endregion

            #region create House
            // Create starting point (Kitchen) and connect to new locations: Bedroom, Pantry, Office
            LocationWithHidingPlace kitchen = new LocationWithHidingPlace("Kitchen", "in a cupboard");
            LocationWithHidingPlace bedroom = kitchen.AddExit(Direction.North, "Bedroom", "under the bed");
            LocationWithHidingPlace pantry = kitchen.AddExit(Direction.East, "Pantry", "in a box");
            LocationWithHidingPlace office = kitchen.AddExit(Direction.West, "Office", "under the desk");

            // Connect Bedroom to new locations: Exercise Room, Closet, Bathroom
            LocationWithHidingPlace exerciseRoom = bedroom.AddExit(Direction.North, "Exercise Room", "behind the balls");
            LocationWithHidingPlace closet = bedroom.AddExit(Direction.East, "Closet", "between the coats");
            LocationWithHidingPlace bathroom = bedroom.AddExit(Direction.West, "Bathroom", "in the tub");

            // Connect Office to new location: Sensory Room
            LocationWithHidingPlace sensoryRoom = bathroom.AddExit(Direction.West, "Sensory Room", "under the bean bags");

            // Connect Office to Bathroom
            office.AddExit(Direction.Northeast, bathroom);

            // Create enumerable of LocationWithHidingPlace objects
            IEnumerable<LocationWithHidingPlace> locationsWithHidingPlaces = new List<LocationWithHidingPlace>()
            {
                kitchen, bedroom, pantry, office, exerciseRoom, closet, bathroom, sensoryRoom
            };

            // Create House
            house = new House("dream house", "DreamHouse", "Kitchen", new List<Location>(), locationsWithHidingPlaces);
            #endregion

            // ACT
            string serializedHouse = house.Serialize();

            // Assert that serialized House text is as expected
            Assert.That(serializedHouse, Is.EqualTo(expectedSerializedHouse));
        }
    }
}