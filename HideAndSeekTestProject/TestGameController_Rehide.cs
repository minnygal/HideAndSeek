using Moq;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for RehideAllOpponents method
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    [TestFixture]
    public class TestGameController_Rehide
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            // Set static House file system to mock file system
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.house.json", DefaultHouse_Serialized);

            // Create Opponent mocks
            Mock<Opponent> opponent1 = new Mock<Opponent>();
            opponent1.Setup((o) => o.Name).Returns("Joe");

            Mock<Opponent> opponent2 = new Mock<Opponent>();
            opponent2.Setup((o) => o.Name).Returns("Bob");

            Mock<Opponent> opponent3 = new Mock<Opponent>();
            opponent3.Setup((o) => o.Name).Returns("Ana");

            Mock<Opponent> opponent4 = new Mock<Opponent>();
            opponent4.Setup((o) => o.Name).Returns("Owen");

            Mock<Opponent> opponent5 = new Mock<Opponent>();
            opponent5.Setup((o) => o.Name).Returns("Jimmy");

            // Create new GameController with mocked Opponents and default House layout
            gameController = new GameController(
                new Opponent[] { opponent1.Object, opponent2.Object, opponent3.Object, opponent4.Object, opponent5.Object },
                "DefaultHouse");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController RehideAllOpponents HidingLocations Success")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces()
        {
            // Create enumerable of hiding places for opponents to hide
            IEnumerable<string> hidingPlaces = new List<string>()
            {
                "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry"
            };

            // Hide all opponents in specified locations
            gameController.RehideAllOpponents(hidingPlaces);

            // Assert that hiding places (values) in OpponentsAndHidingLocations dictionary are set correctly
            Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(hidingPlaces));
        }

        [Test]
        [Category("GameController RehideAllOpponents InvalidOperationException Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForNonexistentLocation()
        {
            Assert.Multiple(() =>
            {
                // Assert that hiding an Opponent in a location with an invalid name raises an exception
                var exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    gameController.RehideAllOpponents(new List<string>() { "Dungeon", "Lavatory", "Eggshells", "Worm Hole", "Zoo" });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("location with hiding place \"Dungeon\" does not exist in House"));
            });
        }

        [TestCase()]
        [TestCase("Living Room")]
        [TestCase("Living Room", "Kitchen")]
        [TestCase("Living Room", "Kitchen", "Pantry")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic", "Master Bedroom", "Kids Room")]
        [Category("GameController RehideAllOpponents ArgumentOutOfRangeException Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForIncorrectNumberOfHidingPlaces(params string[] hidingPlaces)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling method with an enumerable with an incorrect number of hiding places raises an exception
                var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    gameController.RehideAllOpponents(hidingPlaces);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("The number of hiding places must equal the number of opponents. (Parameter 'hidingPlaces')"));
            });
        }

        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        private static string DefaultHouse_Serialized
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
    }
}