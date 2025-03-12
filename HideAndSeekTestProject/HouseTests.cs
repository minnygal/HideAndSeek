using System;
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
    /// House tests for testing House layout and functions 
    /// (get Location or LocationWithHidingPlace by name, get random exit, get random LocationWithHidingPlace, 
    /// clear LocationWithHidingPlaces, find out whether Location or LocationWithHidingPlace exists)
    /// </summary>
    [TestFixture]
    public class HouseTests
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            house = new House("my house", "DefaultHouse", "Entry");
        }

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
                // Assert that name of StartingPoint is correct
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"), "name of StartingPoint");

                // Assert that Garage is located "Out" from StartingPoint
                var garage = house.StartingPoint.GetExit(Direction.Out);
                Assert.That(garage.Name, Is.EqualTo("Garage"), "Garage is Out from StartingPoint");

                // Assert that Hallway is located "East" of StartingPoint
                var hallway = house.StartingPoint.GetExit(Direction.East);
                Assert.That(hallway.Name, Is.EqualTo("Hallway"), "Hallway is East of StartingPoint");

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
                Assert.That(house.GetLocationByName("Entry").Name, Is.EqualTo("Entry"), "get StartingPoint");
                Assert.That(house.GetLocationByName("Attic").Name, Is.EqualTo("Attic"), "get Attic");
                Assert.That(house.GetLocationByName("Garage").Name, Is.EqualTo("Garage"), "get Garage");
                Assert.That(house.GetLocationByName("Master Bedroom").Name, Is.EqualTo("Master Bedroom"), "get Master Bedroom");
            });
        }

        [Test]
        [Category("House GetLocationByName Failure")]
        public void Test_House_GetLocationByName_WhenNameNotFound_ReturnNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationByName("Secret Library"), Is.Null, "try \"Secret Library\"");
                Assert.That(house.GetLocationByName("master bedroom"), Is.Null, "try \"master bedroom\"");
                Assert.That(house.GetLocationByName("MasterBedroom"), Is.Null, "try \"MasterBedroom\"");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Success")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenFound_ReturnThatLocation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationWithHidingPlaceByName("Pantry").Name, Is.EqualTo("Pantry"), "get Pantry");
                Assert.That(house.GetLocationWithHidingPlaceByName("Attic").Name, Is.EqualTo("Attic"), "get Attic");
                Assert.That(house.GetLocationWithHidingPlaceByName("Garage").Name, Is.EqualTo("Garage"), "get Garage");
                Assert.That(house.GetLocationWithHidingPlaceByName("Master Bedroom").Name, Is.EqualTo("Master Bedroom"), "get Master Bedroom");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenNotFound_BecauseLocationIsNotHidingPlaceType_ReturnsNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationWithHidingPlaceByName("Entry"), Is.Null, "try \"StartingPoint\"");
                Assert.That(house.GetLocationWithHidingPlaceByName("Hallway"), Is.Null, "try \"Hallway\"");
                Assert.That(house.GetLocationWithHidingPlaceByName("Landing"), Is.Null, "try \"Landing\"");
            });
        }

        [Test]
        [Category("House GetLocationWithHidingPlaceByName Failure")]
        public void Test_House_GetLocationWithHidingPlaceByName_WhenNotFound_BecauseNoLocationWithThatName_ReturnsNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationWithHidingPlaceByName("Secret Library"), Is.Null, "try \"Secret Library\"");
                Assert.That(house.GetLocationWithHidingPlaceByName("master bedroom"), Is.Null, "try \"master bedroom\"");
                Assert.That(house.GetLocationWithHidingPlaceByName("MasterBedroom"), Is.Null, "try \"MasterBedroom\"");
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
        public void Test_House_DoesLocationWithHidingPlaceExist_ReturnsFalse_WhenNoLocationExists()
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
            var landing = house.GetLocationByName("Landing");
            var kitchen = house.GetLocationByName("Kitchen");

            Assert.Multiple(() =>
            {
                // Assert Landing's random exit at index 0 is Attic
                house.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Attic"), "Landing exit at index 0");

                // Assert Landing's random exit at index 1 is Kids Room
                house.Random = new MockRandom() { ValueToReturn = 1 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Kids Room"), "Landing exit at index 1");

                // Assert Landing's random exit at index 2 is Pantry
                house.Random = new MockRandom() { ValueToReturn = 2 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Pantry"), "Landing exit at index 2");

                // Assert Landing's random exit at index 3 is Second Bathroom
                house.Random = new MockRandom() { ValueToReturn = 3 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Second Bathroom"), "Landing exit at index 3");

                // Assert Landing's random exit at index 4 is Nursery
                house.Random = new MockRandom() { ValueToReturn = 4 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Nursery"), "Landing exit at index 4");

                // Assert Landing's random exit at index 5 is Master Bedroom
                house.Random = new MockRandom() { ValueToReturn = 5 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Master Bedroom"), "Landing exit at index 5");

                // Assert Landing's random exit at index 6 is Hallway
                house.Random = new MockRandom() { ValueToReturn = 6 };
                Assert.That(house.GetRandomExit(landing).Name, Is.EqualTo("Hallway"), "Landing exit at index 6");

                // Assert Kitchen's random exit at index 0 is Hallway
                house.Random = new MockRandom() { ValueToReturn = 0 };
                Assert.That(house.GetRandomExit(kitchen).Name, Is.EqualTo("Hallway"), "Kitchen exit at index 0");
            });
        }

        [TestCase("Kitchen", 1, 0, 4, 0)] // no extra moves
        [TestCase("Pantry", 0, 1, 2)] // 1 extra move
        [TestCase("Bathroom", 1, 2, 3)] // 2 extra moves
        [Category("House GetRandomLocationWithHidingPlace")]
        public void Test_House_GetRandomHidingPlace(string hidingLocationName, params int[] mockRandomValueList)
        {
            house.Random = new MockRandomWithValueList(mockRandomValueList); // Set House's Random number generator to mock
            Assert.That(house.GetRandomLocationWithHidingPlace().Name, Is.EqualTo(hidingLocationName)); // Assert that name of LocationWithHidingPlace returned is as expected
        }

        [Test]
        [Category("House LocationTypes")]
        public void Test_House_LocationsAreOfType_LocationWithHidingPlace()
        {
            Assert.Multiple(() =>
            {
                Assert.That(house.GetLocationByName("Garage"), Is.InstanceOf<LocationWithHidingPlace>(), "Garage");
                Assert.That(house.GetLocationByName("Kitchen"), Is.InstanceOf<LocationWithHidingPlace>(), "Kitchen");
                Assert.That(house.GetLocationByName("Living Room"), Is.InstanceOf<LocationWithHidingPlace>(), "Living Room");
                Assert.That(house.GetLocationByName("Bathroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Bathroom");
                Assert.That(house.GetLocationByName("Master Bedroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Master Bedroom");
                Assert.That(house.GetLocationByName("Master Bath"), Is.InstanceOf<LocationWithHidingPlace>(), "Master Bath");
                Assert.That(house.GetLocationByName("Second Bathroom"), Is.InstanceOf<LocationWithHidingPlace>(), "Second Bathroom");
                Assert.That(house.GetLocationByName("Kids Room"), Is.InstanceOf<LocationWithHidingPlace>(), "Kids Room");
                Assert.That(house.GetLocationByName("Nursery"), Is.InstanceOf<LocationWithHidingPlace>(), "Nursery");
                Assert.That(house.GetLocationByName("Pantry"), Is.InstanceOf<LocationWithHidingPlace>(), "Pantry");
                Assert.That(house.GetLocationByName("Attic"), Is.InstanceOf<LocationWithHidingPlace>(), "Attic");
            });
        }

        [Test]
        [Category("House ClearHidingPlaces CheckHidingPlace")]
        public void Test_House_ClearHidingPlaces()
        {
            // Hide opponent in garage
            var garage = house.GetLocationWithHidingPlaceByName("Garage"); // Get garage reference
            garage.HideOpponent(new Opponent());

            // Hide 3 more opponents in attic
            var attic = house.GetLocationWithHidingPlaceByName("Attic"); // Get attic reference
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());
            attic.HideOpponent(new Opponent());

            // Clear hiding places in house
            house.ClearHidingPlaces();

            // Assert that no opponents are in cleared hiding places
            Assert.Multiple(() =>
            {
                Assert.That(garage.CheckHidingPlace(), Is.Empty, "no opponents in garage");
                Assert.That(attic.CheckHidingPlace(), Is.Empty, "no opponents in attic");
            });
        }

        [Test]
        [Category("House CreateHouse Success")]
        public void Test_House_CreateHouse()
        {
            // Set up mock file system and assign to House property
            string textInHouseFile = "{\"Name\":\"my house\",\"HouseFileName\":\"MyHouseFile\",\"PlayerStartingPoint\":\"Entry\"}";
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup((s) => s.File.Exists("MyHouseFile.json")).Returns(true);
            fileSystemMock.Setup((s) => s.File.ReadAllText("MyHouseFile.json")).Returns(textInHouseFile);
            House.FileSystem = fileSystemMock.Object;

            // Call method to create House
            House house = House.CreateHouse("MyHouseFile");

            Assert.Multiple(() =>
            {
                Assert.That(house.Name, Is.EqualTo("my house"));
                Assert.That(house.HouseFileName, Is.EqualTo("MyHouseFile"));
                Assert.That(house.PlayerStartingPoint, Is.EqualTo("Entry"));
            });
        }

        [Test]
        [Category("House CreateHouse Failure")]
        public void Test_House_CreateHouse_WithNameOfNonexistingFile_ThrowsException()
        {
            // Set up mock file system and assign to House property
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup((s) => s.File.Exists("MyNonexistentFile.json")).Returns(false);
            House.FileSystem = fileSystemMock.Object;

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with an invalid file name raises an exception
                var exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    House.CreateHouse("MyNonexistentFile");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot load game because house layout file MyNonexistentFile does not exist"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("ABCDeaoueou[{}}({}")]
        [Category("House CreateHouse Failure")]
        public void Test_House_CreateHouse_FromCorruptFile_ThrowsException(string fileText)
        {
            // Set up mock file system and assign to House property
            Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup((s) => s.File.Exists("MyCorruptFile.json")).Returns(true);
            fileSystemMock.Setup((s) => s.File.ReadAllText("MyCorruptFile.json")).Returns(fileText);
            House.FileSystem = fileSystemMock.Object;

            Assert.Multiple(() =>
            {
                // Assert that creating a SavedGame object with an invalid file name raises an exception
                var exception = Assert.Throws<JsonException>(() =>
                {
                    House.CreateHouse("MyCorruptFile");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot process because data in house layout file MyCorruptFile is corrupt"));
            });
        }

        // Calls properties' setters and setters successfully
        [Test]
        [Category("House Constructor Success")]
        public void Test_House_Constructor()
        {
            // Create House
            house = new House("special house", "SpecialHouse", "Entry");

            // Create list of expected Location names
            IEnumerable<string> expectedLocationNames = new List<string>()
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

            // Create list of expected LocationWithHidingPlace names
            IEnumerable<string> expectedLocationWithHidingPlaceNames = new List<string>()
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

            // Create list of expected Location names not including LocationWithHidingPlace objects
            IEnumerable<string> expectedLocationWithoutHidingPlaceNames = new List<string>()
            {
                "Hallway",
                "Landing",
                "Entry"
            };

            // Assume no exceptions were thrown
            // Assert that properties are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(house.Name, Is.EqualTo("special house"));
                Assert.That(house.HouseFileName, Is.EqualTo("SpecialHouse"));
                Assert.That(house.StartingPoint.Name, Is.EqualTo("Entry"));
                Assert.That(house.PlayerStartingPoint, Is.EqualTo("Entry"));
                Assert.That(house.Locations.Select((l) => l.Name), Is.EquivalentTo(expectedLocationNames));
                Assert.That(house.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(expectedLocationWithHidingPlaceNames));
                Assert.That(house.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(expectedLocationWithoutHidingPlaceNames));
            });
        }
        
        [TestCase("")]
        [TestCase(" ")]
        [Category("House Name Failure")]
        public void Test_House_NameSetter_WithInvalidName(string name)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the house name to an invalid name raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    house.Name = name;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because house name \"{name}\" is invalid (is empty or contains only whitespace"));
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
        [Category("House HouseFileName Failure")]
        public void Test_House_HouseFileNameSetter_WithInvalidHouseFileName(string houseFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the house file name to an invalid file name raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    house.HouseFileName = houseFileName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because House file name \"{houseFileName}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("House PlayerStartingPoint Failure")]
        public void Test_House_PlayerStartingPointSetter_WithInvalidLocationName(string locationName)
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the player starting point location name to an invalid location name raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    house.PlayerStartingPoint = locationName;
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because player starting point location name \"{locationName}\" is invalid (is empty or contains only whitespace"));
            });
        }

        [Test]
        [Category("House StartingPoint Failure")]
        public void Test_House_StartingPointSetter_WithLocationNotExistingInHouse()
        {
            Assert.Multiple(() =>
            {
                // Assert that setting the starting point location to a Location not in the House raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    house.StartingPoint = new Location("not in house");
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot perform action because player starting point location \"not in house\" is not a location in the house"));
            });  
        }

        [Test]
        [Category("House Serialize")]
        public void Test_House_SerializeMethod()
        {
            // Expected serialized House text
            string expectedSerializedHouse =
                "{\"Name\":\"my house\"," +
                "\"HouseFileName\":\"DefaultHouse\"," +
                "\"PlayerStartingPoint\":\"Entry\"," +
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
                    "]," +
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
            
            // Serialize House
            string serializedHouse = house.Serialize();

            // Assert that serialized text is as expected
            Assert.That(serializedHouse, Is.EqualTo(expectedSerializedHouse));
        }
    }
}