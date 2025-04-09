using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    [TestFixture]
    public class TestGameController_CustomOpponents
    {
        GameController gameController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set House file system to return text for default House
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", 
                                    TestGameController_CustomOpponents_TestCaseData.DefaultHouse_Serialized);
        }

        [SetUp]
        public void Setup()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.Random = new Random(); // Set static House Random property to new Random number generator
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestCaseData), 
            nameof(TestGameController_CustomOpponents_TestCaseData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNumberOfOpponents))]
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNumberOfOpponents(int numberOfOpponents, string[] expectedNames, string[] expectedHidingPlaces)
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
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController
            gameController = new GameController(numberOfOpponents, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(expectedNames), "Opponents' expectedNames");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(11)]
        [TestCase(500)]
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces Failure")]
        public void Test_GameController_Constructor_WithInvalidNumberOfOpponents_AndCheckErrorMessage(int numberOfOpponents)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with invalid number of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(numberOfOpponents, "DefaultHouse");
                });

                Assert.That(exception.Message, Is.EqualTo("Cannot create a new instance of GameController " +
                                                          "because the number of Opponents specified is invalid (must be between 1 and 10)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestCaseData), 
            nameof(TestGameController_CustomOpponents_TestCaseData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNamesOfOpponents))]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNamesOfOpponents(string[] names, string[] expectedHidingPlaces)
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
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController
            gameController = new GameController(names, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(names), "Opponents' expectedNames");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [Test]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces Failure")]
        public void Test_GameController_Constructor_WithEmptyArrayOfNamesOfOpponents_AndCheckErrorMessage()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty array of names of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(Array.Empty<string>(), "DefaultHouse");
                });

                Assert.That(exception.Message, Is.EqualTo("Cannot create a new instance of GameController because no names for Opponents were passed in"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces Failure")]
        public void Test_GameController_Constructor_WithInvalidOpponentName_AndCheckErrorMessage(string invalidName)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with array with invalid name of Opponent raises an exception
                var exception = Assert.Throws<InvalidDataException>(() => {
                    new GameController(new string[] {invalidName}, "DefaultHouse");
                });

                Assert.That(exception.Message, Is.EqualTo($"Cannot perform action because opponent name \"{invalidName}\" is invalid " +
                                                           "(is empty or contains only whitespace)"));
            });
        }
    }
}
