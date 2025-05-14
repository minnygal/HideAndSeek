using Moq;
using System.IO.Abstractions;
using System.Text.Json;

namespace HideAndSeek
{
    /// <summary>
    /// House tests for CreateHouse method
    /// 
    /// -The failure tests for CreateHouse are unit tests.
    /// -The success tests for CreateHouse are integration tests using Location and LocationWithHidingPlace.
    /// </summary>
    [TestFixture]
    public class TestHouse_CreateHouse
    {
        [SetUp]
        public void SetUp()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("House CreateHouse Name HouseFileName PlayerStartingPoint StartingPoint Locations LocationsWithHidingPlaces Location LocationWithHidingPlace Exits Name Success")]
        public void Test_House_CreateHouse_WithLocationsWithoutHidingPlaces_AndCheckHouseAndLocationProperties()
        {
            // ARRANGE
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText(
                                    "DefaultHouse.house.json", TestHouse_CreateHouse_TestData.DefaultHouse_Serialized);

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
                Assert.That(house.Locations.Select((l) => l.Name), 
                    Is.EquivalentTo(TestHouse_CreateHouse_TestData.DefaultHouse_Locations), "Locations property (check each Location Name)");
                Assert.That(house.LocationsWithoutHidingPlaces.Select((l) => l.Name), 
                    Is.EquivalentTo(TestHouse_CreateHouse_TestData.DefaultHouse_LocationsWithoutHidingPlaces), "LocationsWithoutHidingPlaces property (check each Location Name)");
                Assert.That(house.LocationsWithHidingPlaces.Select((l) => l.Name), 
                    Is.EquivalentTo(TestHouse_CreateHouse_TestData.DefaultHouse_LocationsWithHidingPlaces), "LocationsWithHidingPlaces property (check each LocationWithHidingPlace Name");

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
        public void Test_House_CreateHouse_WithNoLocationsWithoutHidingPlaces_AndCheckEmptyLocationsWithHidingPlacesProperty()
        {
            // ARRANGE
            // Initialize variable to string representing text in House file (with empty LocationsWithoutHidingPlaces)
            string textInHouseFile =
                "{" +
                    TestHouse_CreateHouse_TestData.DefaultHouse_Serialized_Name + "," +
                    TestHouse_CreateHouse_TestData.DefaultHouse_Serialized_HouseFileName + "," +
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
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.house.json", textInHouseFile);

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

        [TestCaseSource(typeof(TestHouse_CreateHouse_TestData),
            nameof(TestHouse_CreateHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid))]
        [Category("House CreateHouse JsonException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileFormatIsInvalid(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyCorruptFile.house.json", fileText);

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

        [TestCaseSource(typeof(TestHouse_CreateHouse_TestData),
            nameof(TestHouse_CreateHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection))]
        [Category("House CreateHouse JsonException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidDirection(string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyCorruptFile.house.json", fileText);

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

        [TestCaseSource(typeof(TestHouse_CreateHouse_TestData),
            nameof(TestHouse_CreateHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue))]
        [Category("House CreateHouse ArgumentException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasWhitespaceValue(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.house.json", fileText);

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

        [TestCaseSource(typeof(TestHouse_CreateHouse_TestData),
            nameof(TestHouse_CreateHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue))]
        [Category("House CreateHouse ArgumentException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.house.json", fileText);

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

        [TestCaseSource(typeof(TestHouse_CreateHouse_TestData),
            nameof(TestHouse_CreateHouse_TestData.TestCases_For_Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation))]
        [Category("House CreateHouse InvalidOperationException Failure")]
        public void Test_House_CreateHouse_AndCheckErrorMessage_WhenFileDataHasInvalidValue_NonexistentLocation(
            string exceptionMessageEnding, string fileText)
        {
            // Assign mock file system to House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("MyInvalidDataFile.house.json", fileText);

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
    }
}