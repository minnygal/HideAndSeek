using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for custom Opponents
    /// 
    /// The success tests for constructors accepting the number or names of opponents
    /// are integration tests using House, Opponent, Location, and LocationWithHidingPlace.
    /// 
    /// The success tests for the constructor accepting Opponent objects
    /// are integration tests using House, Location, and LocationWithHidingPlace.
    /// 
    /// Failure tests are not integration tests.
    /// </summary>
    [TestFixture]
    public class TestGameController_CustomOpponents
    {
        GameController gameController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set House file system to return text for default House
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.house.json", 
                                    TestGameController_CustomOpponents_TestData.DefaultHouse_Serialized);
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

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData), 
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNumberOfOpponents))]
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNumberOfOpponents(
            int numberOfOpponents, string[] expectedNames, string[] expectedHidingPlaces)
        {
            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                7, // Hide opponent in Kitchen
                                5, // Hide opponent in Pantry
                                1, // Hide opponent in Bathroom
                                7, // Hide opponent in Kitchen
                                5 // Hide opponent in Pantry
                           ]);

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
        [Category("GameController Constructor SpecifiedNumberOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForInvalidNumberOfOpponents(int numberOfOpponents)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with invalid number of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(numberOfOpponents, "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController " +
                                                          "because the number of Opponents specified is invalid (must be between 1 and 10)"));
            });
        }
        
        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData), 
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedNamesOfOpponents))]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedNamesOfOpponents(string[] names, string[] expectedHidingPlaces)
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                7, // Hide opponent in Kitchen
                5, // Hide opponent in Pantry
                1, // Hide opponent in Bathroom
                7, // Hide opponent in Kitchen
                5 // Hide opponent in Pantry
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
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForEmptyArrayOfNamesOfOpponents()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty array of names of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(Array.Empty<string>(), "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no names for Opponents were passed in"));
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("GameController Constructor SpecifiedNamesOfOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForInvalidOpponentName(string invalidName)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with array with invalid name of Opponent raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(new string[] {invalidName}, "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because " +
                                                             $"opponent name \"{invalidName}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_CustomOpponents_TestData),
            nameof(TestGameController_CustomOpponents_TestData.TestCases_For_Test_GameController_Constructor_WithSpecifiedOpponents))]
        [Category("GameController Constructor SpecifiedOpponents OpponentsAndHidingPlaces Success")]
        public void Test_GameController_Constructor_WithSpecifiedOpponents(Opponent[] opponents, string[] expectedHidingPlaces)
        {
            // Create mock random values list for hiding opponents
            int[] mockRandomValuesList = [
                7, // Hide opponent in Kitchen
                5, // Hide opponent in Pantry
                1, // Hide opponent in Bathroom
                7, // Hide opponent in Kitchen
                5 // Hide opponent in Pantry
            ];

            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(mockRandomValuesList);

            // Create GameController with Opponents
            gameController = new GameController(opponents, "DefaultHouse");

            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Keys, Is.EquivalentTo(opponents), "Opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(expectedHidingPlaces), "Opponents' hiding locations");
            });
        }

        [Test]
        [Category("GameController Constructor SpecifiedOpponents OpponentsAndHidingPlaces ArgumentException Failure")]
        public void Test_GameController_Constructor_AndCheckErrorMessage_ForEmptyArrayOfOpponents()
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty array of Opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() => {
                    new GameController(Array.Empty<Opponent>(), "DefaultHouse");
                });

                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no Opponents were passed in"));
            });
        }
    }
}