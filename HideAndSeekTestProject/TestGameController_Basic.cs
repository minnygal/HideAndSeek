using Moq;
using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for RehideAllOpponents method,
    /// Opponents' hiding locations when Opponents rehidden or game restarted,
    /// constructor and RestartGame with invalid file name or name of nonexistent file
    /// </summary>
    [TestFixture]
    public class TestGameController_Basic
    {
        GameController gameController;

        [SetUp]
        public void SetUp()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
            gameController = new GameController();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController RestartGame HidingLocations Success")]
        public void Test_GameController_RestartGame_AndCheckHidingLocations()
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 1 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 2 in Pantry
                1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, // Hide opponent 3 in Bathroom
                1, 0, 4, 0, 1, 0, 4, 0, 1, 0, 4, // Hide opponent 4 in Kitchen
                0, 1, 2, 0, 1, 2, 0, 1, 2, 0, 1, 2, // Hide opponent 5 in Pantry
            ];

            // Set House random number generator to mock random
            gameController.House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Restart game to rehide Opponents with new MockRandom
            gameController.RestartGame();

            // Assert that hiding places (values) in OpponentsAndHidingLocations dictionary are set correctly
            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(0).Value.ToString, Is.EqualTo("Kitchen"), "Opponent 1 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(1).Value.ToString, Is.EqualTo("Pantry"), "Opponent 2 hiding in Pantry");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(2).Value.ToString, Is.EqualTo("Bathroom"), "Opponent 3 hiding in Bathroom");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(3).Value.ToString, Is.EqualTo("Kitchen"), "Opponent 4 hiding in Kitchen");
                Assert.That(gameController.OpponentsAndHidingLocations.ElementAt(4).Value.ToString, Is.EqualTo("Pantry"), "Opponent 5 hiding in Pantry");
            });
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
        [Category("GameController RehideAllOpponents Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForNonexistentLocation()
        {
            Assert.Multiple(() =>
            {
                // Assert that hiding an Opponent in a location with an invalid name raises an exception
                var exception = Assert.Throws<NullReferenceException>(() =>
                {
                    gameController.RehideAllOpponents(new List<string>() { "Dungeon", "Lavatory", "Eggshells", "Worm Hole", "Zoo" });
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Object reference not set to an instance of an object."));
            });
        }

        [TestCase()]
        [TestCase("Living Room")]
        [TestCase("Living Room", "Kitchen")]
        [TestCase("Living Room", "Kitchen", "Pantry")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic")]
        [TestCase("Living Room", "Kitchen", "Pantry", "Attic", "Master Bedroom", "Kids Room")]
        [Category("GameController RehideAllOpponents Failure")]
        public void Test_GameController_RehideAllOpponents_InSpecificPlaces_AndCheckErrorMessage_ForIncorrectNumberOfHidingPlaces(params string[] hidingPlaces)
        {
            Assert.Multiple(() =>
            {
                // Assert that hiding an Opponent in a location with an invalid name raises an exception
                var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    gameController.RehideAllOpponents(hidingPlaces);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("The number of hiding places must equal the number of opponents. (Parameter 'hidingPlaces')"));
            });
        }

        [TestCaseSource(typeof(TestGameController_Basic_TestCaseData), nameof(TestGameController_Basic_TestCaseData.TestCases_For_Test_GameController_CheckErrorMessage_ForInvalidHouseFileName))]
        [Category("GameController Constructor Failure")]
        public void Test_GameController_CheckErrorMessage_ForInvalidHouseFileName(Action callWithInvalidHouseFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that doing action with name of nonexistent House file raises an exception
                var exception = Assert.Throws<InvalidDataException>(() =>
                {
                    callWithInvalidHouseFileName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo("Cannot perform action because file name \"@eou]} {(/\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_Basic_TestCaseData), nameof(TestGameController_Basic_TestCaseData.TestCases_For_Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist))]
        public void Test_GameController_CheckErrorMessage_ForHouseFileDoesNotExist(Action callWithNonexistentFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that doing action with name of nonexistent House file raises an exception
                var exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    callWithNonexistentFileName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot load game because house layout file MyNonexistentFile does not exist"));
            });
        }
    }
}