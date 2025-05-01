using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for constructors and RestartGame
    /// 
    /// When House is passed in, these are integration tests using Location, LocationWithHidingPlace, and sometimes Opponent.
    /// When Opponents are passed in, these are integration tests using Location, LocationWithHidingPlace, and sometimes House.
    /// Otherwise, these are integration tests using Opponents, House, Location, and LocationWithHidingPlace.
    /// </summary>
    [TestFixture]
    public class TestGameController_ConstructorsAndRestartGame
    {
        [SetUp]
        public void SetUp()
        {
            House.FileSystem = new FileSystem(); // Set House file system to new clean file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set House file system to new clean file system
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault))]
        public void Test_GameController_CheckHouseSetSuccessfully_ViaFileNameOrDefault(
            Func<GameController> GetGameController, string houseName, string houseFileName,
            IEnumerable<string> locationsWithoutHidingPlaces, IEnumerable<string> locationsWithHidingPlaces)
        {
            GameController gameController = GetGameController(); // Get game controller
            Assert.Multiple(() =>
            {
                Assert.That(gameController.House.Name, Is.EqualTo(houseName), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(houseFileName), "House file name");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithoutHidingPlaces), "House locations without hiding places");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithHidingPlaces), "House locations with hiding places");
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject))]
        public void Test_GameController_CheckHouseSetSuccessfully_ViaHouseObject(House house, Func<GameController> GetGameController)
        {
            GameController gameController = GetGameController();
            Assert.That(gameController.House, Is.SameAs(house));
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_CheckOpponentsSetSuccessfully))]
        public void Test_GameController_CheckOpponentsSetSuccessfully(
            Func<GameController> GetGameController, string[] opponentNames)
        {
            Assert.That(GetGameController().OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(opponentNames));
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData), 
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName))]
        public void Test_GameController_SpecifyHouseFileName_AndCheckErrorMessage_ForInvalidHouseFileName(Action CallWithInvalidHouseFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that doing action with name of nonexistent House file raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    CallWithInvalidHouseFileName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot perform action because file name \"@eou]} {(/\" is invalid " +
                                                              "(is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData), 
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist))]
        public void Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileDoesNotExist(
            string fileName, Action CallWithNonexistentFileName)
        {
            Assert.Multiple(() =>
            {
                // Assert that doing action with name of nonexistent House file raises an exception
                var exception = Assert.Throws<FileNotFoundException>(() =>
                {
                    CallWithNonexistentFileName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Is.EqualTo($"Cannot load game because house layout file {fileName} does not exist"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt))]
        public void Test_GameController_SpecifyHouseFileNameOrDefault_AndCheckErrorMessage_WhenHouseFileIsCorrupt(
            string corruptHouseFileName, Action<Mock<IFileSystem>> CallWithNameOfCorruptHouseFile)
        {
            // Set up the mock file system to return a corrupt file
            Mock<IFileSystem> mockFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(
                $"{corruptHouseFileName}.house.json", "{(=}gpioeu345621({=@");

            // Assert that performing action with name of corrupt House file raises an exception
            var exception = Assert.Throws<JsonException>(() =>
            {
                CallWithNameOfCorruptHouseFile(mockFileSystem);
            });
            
            // Assert that exception message is as expected
            Assert.That(exception.Message, Does.StartWith($"Cannot process because data in house layout file {corruptHouseFileName} is corrupt - "));
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber))]
        public void Test_GameController_Constructor_SpecifyNumberOfOpponents_AndCheckErrorMessage_ForInvalidNumber(Action CallWithInvalidNumberOfOpponents)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with invalid number of opponents raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    CallWithInvalidNumberOfOpponents();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController " +
                                                              "because the number of Opponents specified is invalid (must be between 1 and 10)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForInvalidName))]
        public void Test_GameController_Constructor_SpecifyOpponentNames_AndCheckErrorMessage_ForInvalidName(Action CallWithInvalidOpponentName)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with list with invalid name for opponent raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    CallWithInvalidOpponentName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because " +
                                                              "opponent name \" \" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames))]
        public void Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForEmptyListOfNames(Action CallWithEmptyListOfNames)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty list for names raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    CallWithEmptyListOfNames();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no names for Opponents were passed in"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName))]
        public void Test_GameController_Constructor_SpecifyNamesOfOpponents_AndCheckErrorMessage_ForNullName(Action CallWithEmptyListWithNullName)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with list with null name raises exception
                var exception = Assert.Throws<ArgumentNullException>(() =>
                {
                    CallWithEmptyListWithNullName();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because opponent name passed in was null"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents))]
        public void Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForEmptyListOfOpponents(Action CallWithEmptyListOfOpponents)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with empty list for Opponents raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    CallWithEmptyListOfOpponents();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because no Opponents were passed in"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent))]
        public void Test_GameController_Constructor_SpecifyOpponents_AndCheckErrorMessage_ForNullOpponent(Action CallWithListWithNullOpponent)
        {
            Assert.Multiple(() =>
            {
                // Assert that calling constructor with null Opponent raises exception
                var exception = Assert.Throws<ArgumentNullException>(() =>
                {
                    CallWithListWithNullOpponent();
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("Cannot create a new instance of GameController because null Opponent was passed in"));
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData), 
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents))]
        public void Test_GameController_Constructor_FullGame_InDefaultHouse_With2Opponents(
            Func<GameController> GetGameController, string[] opponentNames)
        {
            // Set up mock file system to return valid default House text
            TestGameController_ConstructorsAndRestartGame_TestData.SetUpHouseMockFileSystemToReturnValidDefaultHouseText();

            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                7, // Hide opponent in Kitchen
                                5 // Hide opponent in Pantry
                           ]);

            // Get game controller
            GameController gameController = GetGameController();

            // Play game and assert that messages and properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.House.Name, Is.EqualTo("my house"), "check house name");
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(opponentNames), "check opponent names");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Move to the Garage
                Assert.That(gameController.Move(Direction.Out), Is.EqualTo("Moving Out"), "check string returned when move Out to Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" + Environment.NewLine +
                    " - the Entry is In" + Environment.NewLine +
                    "Someone could hide behind the car" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move Out to Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "check prompt after move Out to Garage");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Out to Garage");

                // Check the Garage but find no opponents
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the car"), "check string returned when check in Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" + Environment.NewLine +
                    " - the Entry is In" + Environment.NewLine +
                    "Someone could hide behind the car" + Environment.NewLine +
                    "You have not found any opponents"), "check status after check in Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "check prompt after check in Garage");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Garage");

                // Move to the Entry
                Assert.That(gameController.Move(Direction.In), Is.EqualTo("Moving In"), "check string returned when move In to Entry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move In to Entry");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go: "), "check prompt after move In to Entry");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Entry");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move East to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go: "), "check prompt after move East to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over move to Hallway");

                // Move to the Kitchen
                Assert.That(gameController.Move(Direction.Northwest), Is.EqualTo("Moving Northwest"), "check string returned when move Northwest to Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the Southeast" + Environment.NewLine +
                    "Someone could hide next to the stove" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move Northwest to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go (or type 'check'): "), "check prompt after move Northwest to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Kitchen");

                // Check the Kitchen for players - opponent 1 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding next to the stove"), "check string returned when check in Kitchen and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the Southeast" + Environment.NewLine +
                    "Someone could hide next to the stove" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status after check in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "check prompt after check in Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Kitchen");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.Southeast), Is.EqualTo("Moving Southeast"), "check string returned when move Southeast to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status when move Southeast to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go: "), "check prompt after move Southeast to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Hallway");

                // Move to the Bathroom
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the South" + Environment.NewLine +
                    "Someone could hide behind the door" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status when move North to Bathroom");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Bathroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Bathroom");

                // Check the Bathroom for players - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the door"), "check string returned when check in Bathroom and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the South" + Environment.NewLine +
                    "Someone could hide behind the door" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status after check in Bathroom");
                Assert.That(gameController.Prompt, Is.EqualTo("10: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bathroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Bathroom");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "check string returned when move South to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status when move South to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go: "), "check prompt after move South to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Hallway");

                // Move to the Landing
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "check string returned when move Up to Landing");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Kids Room is to the Southeast" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Second Bathroom is to the West" + Environment.NewLine +
                    " - the Nursery is to the Southwest" + Environment.NewLine +
                    " - the Master Bedroom is to the Northwest" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status when move Up to Landing");
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go: "), "check prompt after move Up to Landing");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Landing");

                // Move to the Pantry
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "check string returned when move South to Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Landing is to the North" + Environment.NewLine +
                    "Someone could hide inside a cabinet" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[0]}"), "check status when move South to Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("13: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Pantry");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Pantry");

                // Check the Pantry for players - opponent 2 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding inside a cabinet"), "check string returned when check in Pantry and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Landing is to the North" + Environment.NewLine +
                    "Someone could hide inside a cabinet" + Environment.NewLine +
                    $"You have found 2 of 2 opponents: {opponentNames[0]}, {opponentNames[1]}"), "check status after check in Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "check prompt after check in Pantry");
                Assert.That(gameController.GameOver, Is.True, "check game over after check in Pantry");
            });
        }

        [Test]
        [Category("GameController Constructor Parameterless FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success")]
        public void Test_GameController_Constructor_FullGame_InDefaultHouse_WithDefaultNumberOfOpponents()
        {
            // Set up mock file system to return valid default House text
            TestGameController_ConstructorsAndRestartGame_TestData.SetUpHouseMockFileSystemToReturnValidDefaultHouseText();

            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                7, // Hide opponent in Kitchen
                                5, // Hide opponent in Pantry
                                1, // Hide opponent in Bathroom
                                7, // Hide opponent in Kitchen
                                5 // Hide opponent in Pantry
                           ]);

            // Initialize game controller
            GameController gameController = new GameController();

            // Play game and assert that messages and properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.House.Name, Is.EqualTo("my house"), "check house name");
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name),
                    Is.EquivalentTo(new List<string> { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "check opponent names");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Move to the Garage
                Assert.That(gameController.Move(Direction.Out), Is.EqualTo("Moving Out"), "check string returned when move Out to Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" + Environment.NewLine +
                    " - the Entry is In" + Environment.NewLine +
                    "Someone could hide behind the car" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move Out to Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "check prompt after move Out to Garage");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Out to Garage");

                // Check the Garage but find no opponents
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the car"), "check string returned when check in Garage");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Garage. You see the following exit:" + Environment.NewLine +
                    " - the Entry is In" + Environment.NewLine +
                    "Someone could hide behind the car" + Environment.NewLine +
                    "You have not found any opponents"), "check status after check in Garage");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "check prompt after check in Garage");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Garage");

                // Move to the Entry
                Assert.That(gameController.Move(Direction.In), Is.EqualTo("Moving In"), "check string returned when move In to Entry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Entry. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Garage is Out" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move In to Entry");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go: "), "check prompt after move In to Entry");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Entry");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move East to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go: "), "check prompt after move East to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over move East to Hallway");

                // Move to the Kitchen
                Assert.That(gameController.Move(Direction.Northwest), Is.EqualTo("Moving Northwest"), "check string returned when move Northwest to Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the Southeast" + Environment.NewLine +
                    "Someone could hide next to the stove" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move Northwest to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go (or type 'check'): "), "check prompt after move Northwest to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Kitchen");

                // Check the Kitchen for players - opponents 1 and 4 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding next to the stove"), "check string returned when check in Kitchen and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the Southeast" + Environment.NewLine +
                    "Someone could hide next to the stove" + Environment.NewLine +
                    "You have found 2 of 5 opponents: Joe, Owen"), "check status after check in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "check prompt after check in Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Kitchen");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.Southeast), Is.EqualTo("Moving Southeast"), "check string returned when move Southeast to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    "You have found 2 of 5 opponents: Joe, Owen"), "check status when move Southeast to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go: "), "check prompt after move Southeast to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Southeast to Hallway");

                // Move to the Bathroom
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Bathroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the South" + Environment.NewLine +
                    "Someone could hide behind the door" + Environment.NewLine +
                    "You have found 2 of 5 opponents: Joe, Owen"), "check status when move North to Bathroom");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Bathroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Bathroom");

                // Check the Bathroom for players - opponent 3 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind the door"), "check string returned when check in Bathroom and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bathroom. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the South" + Environment.NewLine +
                    "Someone could hide behind the door" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status after check in Bathroom");
                Assert.That(gameController.Prompt, Is.EqualTo("10: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bathroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Bathroom");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "check string returned when move South to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Landing is Up" + Environment.NewLine +
                    " - the Bathroom is to the North" + Environment.NewLine +
                    " - the Living Room is to the South" + Environment.NewLine +
                    " - the Entry is to the West" + Environment.NewLine +
                    " - the Kitchen is to the Northwest" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status when move South to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go: "), "check prompt after move South to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move South to Hallway");

                // Move to the Landing
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "check string returned when move Up to Landing");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Kids Room is to the Southeast" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Second Bathroom is to the West" + Environment.NewLine +
                    " - the Nursery is to the Southwest" + Environment.NewLine +
                    " - the Master Bedroom is to the Northwest" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status when move Up to Landing");
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go: "), "check prompt after move Up to Landing");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Up to Landing");

                // Move to the Attic
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "check string returned when move Up to Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Landing is Down" + Environment.NewLine +
                    "Someone could hide in a trunk" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status when move Up to Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("13: Which direction do you want to go (or type 'check'): "), "check prompt after move Up to Attic");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Attic");

                // Check the Attic for players - no opponents found
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding in a trunk"), "check string returned when check in Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Landing is Down" + Environment.NewLine +
                    "Someone could hide in a trunk" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status after check in Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "check prompt after check in Attic");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Attic");

                // Move Down to Landing
                Assert.That(gameController.Move(Direction.Down), Is.EqualTo("Moving Down"), "check string returned when move Down to Landing");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Kids Room is to the Southeast" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Second Bathroom is to the West" + Environment.NewLine +
                    " - the Nursery is to the Southwest" + Environment.NewLine +
                    " - the Master Bedroom is to the Northwest" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status when move Down to Landing");
                Assert.That(gameController.Prompt, Is.EqualTo("15: Which direction do you want to go: "), "check prompt after move Down to Landing");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Down to Landing");

                // Move to the Pantry
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "check string returned when move South to Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Landing is to the North" + Environment.NewLine +
                    "Someone could hide inside a cabinet" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Joe, Owen, Ana"), "check status when move South to Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("16: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Pantry");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move to Pantry");

                // Check the Pantry for players - opponents 2 and 5 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding inside a cabinet"), "check string returned when check in Pantry and find 1 opponent");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Landing is to the North" + Environment.NewLine +
                    "Someone could hide inside a cabinet" + Environment.NewLine +
                    "You have found 5 of 5 opponents: Joe, Owen, Ana, Bob, Jimmy"), "check status after check in Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("17: Which direction do you want to go (or type 'check'): "), "check prompt after check in Pantry");
                Assert.That(gameController.GameOver, Is.True, "check game over after check in Pantry");
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_FullGame_InCustomHouse_With2Opponents))]
        public void Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByFileName_With2Opponents(
            Func<GameController> GetGameController, string[] opponentNames)
        {
            // Set up mock file system to return valid custom test House text
            TestGameController_ConstructorsAndRestartGame_TestData.SetUpHouseMockFileSystemToReturnValidCustomTestHouseText();

            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                10, // Hide opponent in Attic
                                2 // Hide opponent in Sensory Room
                           ]);

            // Get game controller
            GameController gameController = GetGameController();

            // Play game and assert that messages and properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "check house name");
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(opponentNames), "check opponent names");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the North" + Environment.NewLine +
                    "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move North to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go: "), "check prompt after move North to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move North to Hallway");

                // Move to the Bedroom
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Sensory Room is to the East" + Environment.NewLine +
                    " - the Closet is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move North to Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move North to Bedroom");

                // Check the Bedroom - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding under the bed"), "check string returned when check in Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Sensory Room is to the East" + Environment.NewLine +
                    " - the Closet is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have not found any opponents"), "check status after check in Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Bedroom");

                // Move to the Sensory Room
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Sensory Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Sensory Room. You see the following exits:" + Environment.NewLine +
                    " - the Bedroom is to the West" + Environment.NewLine +
                    " - the Hallway is to the Southwest" + Environment.NewLine +
                    "Someone could hide under the beanbags" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move East to Sensory Room");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Sensory Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Sensory Room");

                // Check the Sensory Room - opponent 2 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding under the beanbags"), "check string returned when check in Sensory Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Sensory Room. You see the following exits:" + Environment.NewLine +
                    " - the Bedroom is to the West" + Environment.NewLine +
                    " - the Hallway is to the Southwest" + Environment.NewLine +
                    "Someone could hide under the beanbags" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status after check in Sensory Room");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go (or type 'check'): "), "check prompt after check in Sensory Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Sensory Room");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.Southwest), Is.EqualTo("Moving Southwest"), "check string returned when move Southwest to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move Southwest to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go: "), "check prompt after move Southwest to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Southwest to Hallway");

                // Move to the Kitchen
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Hallway is to the West" + Environment.NewLine +
                    " - the Cellar is Down" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move East to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Kitchen");

                // Check the Kitchen - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding beside the stove"), "check string returned when check in Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Hallway is to the West" + Environment.NewLine +
                    " - the Cellar is Down" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status after check in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after check in Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Kitchen");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.West), Is.EqualTo("Moving West"), "check string returned when move West to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move West to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("10: Which direction do you want to go: "), "check prompt after move West to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move West to Hallway");

                // Move to the Attic
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "check string returned when move Up to Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "Someone could hide behind a trunk" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move Up to Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go (or type 'check'): "), "check prompt after move Up to Attic");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Up to Attic");

                // Check the Attic - opponent 1 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind a trunk"), "check string returned when check in Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "Someone could hide behind a trunk" + Environment.NewLine +
                    $"You have found 2 of 2 opponents: {opponentNames[1]}, {opponentNames[0]}"), "check status after check in Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "check prompt after check in Attic");
                Assert.That(gameController.GameOver, Is.True, "check game over after check in Attic");
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents))]
        public void Test_GameController_Constructor_FullGame_InCustomHouse_SpecifiedByObject_With2Opponents(
            Func<GameController> GetGameController, string[] opponentNames)
        {
            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                3, // Hide opponent in Pantry
                                1 // Hide opponent in Office
                           ]);

            // Get game controller
            GameController gameController = GetGameController();

            // Play game and assert that messages and properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "check house name");
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), Is.EquivalentTo(opponentNames), "check opponent names");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Start. You see the following exit:" + Environment.NewLine +
                    " - the Bedroom is to the West" + Environment.NewLine +
                    "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Move to the Bedroom
                Assert.That(gameController.Move(Direction.West), Is.EqualTo("Moving West"), "check string returned when move West to Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Start is to the East" + Environment.NewLine +
                    " - the Office is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move West to Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "check prompt after move West to Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move West to Bedroom");

                // Check the Bedroom - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding under the bed"), "check string returned when check in Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Start is to the East" + Environment.NewLine +
                    " - the Office is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have not found any opponents"), "check status after check in Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Bedroom");

                // Move to the Office
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Office");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Office. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Bedroom is to the South" + Environment.NewLine +
                    "Someone could hide under the desk" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move North to Office");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Office");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move North to Office");

                // Check the Office - opponent 2 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding under the desk"), "check string returned when check in Office");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Office. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Bedroom is to the South" + Environment.NewLine +
                    "Someone could hide under the desk" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status after check in Office");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "check prompt after check in Office");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Office");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Kitchen is to the South" + Environment.NewLine +
                    " - the Office is to the West" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move East to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go: "), "check prompt after move East to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Hallway");

                // Move to the Kitchen
                Assert.That(gameController.Move(Direction.South), Is.EqualTo("Moving South"), "check string returned when move South to Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the East" + Environment.NewLine +
                    " - the Hallway is to the North" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move South to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "check prompt after move South to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move South to Kitchen");

                // Check the Kitchen - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding beside the stove"), "check string returned when check in Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the East" + Environment.NewLine +
                    " - the Hallway is to the North" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status after check in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go (or type 'check'): "), "check prompt after check in Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Kitchen");

                // Move to the Pantry
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Kitchen is to the West" + Environment.NewLine +
                    "Someone could hide behind the canned goods" + Environment.NewLine +
                    $"You have found 1 of 2 opponents: {opponentNames[1]}"), "check status when move East to Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Pantry");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Pantry");

                // Check the Pantry - opponent 1 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding behind the canned goods"), "check string returned when check in Pantry");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Pantry. You see the following exit:" + Environment.NewLine +
                    " - the Kitchen is to the West" + Environment.NewLine +
                    "Someone could hide behind the canned goods" + Environment.NewLine +
                    $"You have found 2 of 2 opponents: {opponentNames[1]}, {opponentNames[0]}"), "check status after check in Pantry");
                Assert.That(gameController.Prompt, Is.EqualTo("10: Which direction do you want to go (or type 'check'): "), "check prompt after check in Pantry");
                Assert.That(gameController.GameOver, Is.True, "check game over after check in Pantry");
            });
        }

        [Test]
        [Category("GameController Constructor SpecifyHouseFileName FullGame Move CheckCurrentLocation Message Prompt Status GameOver Success")]
        public void Test_GameController_Constructor_FullGame_InCustomHouse_WithDefaultNumberOfOpponents()
        {
            // Set up mock file system to return valid custom test House text
            TestGameController_ConstructorsAndRestartGame_TestData.SetUpHouseMockFileSystemToReturnValidCustomTestHouseText();

            // Set static House Random number generator property to mock random number generator
            House.Random = new MockRandomWithValueList([
                                10, // Hide opponent in Attic
                                2, // Hide opponent in Sensory Room
                                0, // Hide opponent in Bedroom
                                10, // Hide opponent in Attic
                                2 // Hide opponent in Sensory Room
                           ]);

            // Create game controller
            GameController gameController = new GameController("TestHouse");

            // Play game and assert that messages and properties are as expected
            Assert.Multiple(() =>
            {
                // Assert that properties are as expected when game started
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "check house name");
                Assert.That(gameController.OpponentsAndHidingLocations.Keys.Select((o) => o.Name), 
                    Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "check opponent names");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Landing. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is to the North" + Environment.NewLine +
                    "You have not found any opponents"), "check status when game started");
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "check prompt when game started");
                Assert.That(gameController.GameOver, Is.False, "check game not over at beginning");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move North to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go: "), "check prompt after move North to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move North to Hallway");

                // Move to the Living Room
                Assert.That(gameController.Move(Direction.West), Is.EqualTo("Moving West"), "check string returned when move West to Living Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Living Room. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Office is to the North" + Environment.NewLine +
                    " - the Bathroom is to the South" + Environment.NewLine +
                    "Someone could hide behind the sofa" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move West to Living Room");
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go (or type 'check'): "), "check prompt after move West to Living Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move West to Living Room");

                // Check the Living Room - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding behind the sofa"), "check string returned when check in Living Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Living Room. You see the following exits:" + Environment.NewLine +
                    " - the Hallway is to the East" + Environment.NewLine +
                    " - the Office is to the North" + Environment.NewLine +
                    " - the Bathroom is to the South" + Environment.NewLine +
                    "Someone could hide behind the sofa" + Environment.NewLine +
                    "You have not found any opponents"), "check status after check in Living Room");
                Assert.That(gameController.Prompt, Is.EqualTo("4: Which direction do you want to go (or type 'check'): "), "check prompt after check in Living Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Living Room");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move East to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go: "), "check prompt after move East to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Hallway");

                // Move to the Bedroom
                Assert.That(gameController.Move(Direction.North), Is.EqualTo("Moving North"), "check string returned when move North to Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Sensory Room is to the East" + Environment.NewLine +
                    " - the Closet is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have not found any opponents"), "check status when move North to Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("6: Which direction do you want to go (or type 'check'): "), "check prompt after move North to Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move North to Bedroom");

                // Check the Bedroom - opponent 3 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 1 opponent hiding under the bed"), "check string returned when check in Bedroom");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Bedroom. You see the following exits:" + Environment.NewLine +
                    " - the Sensory Room is to the East" + Environment.NewLine +
                    " - the Closet is to the North" + Environment.NewLine +
                    "Someone could hide under the bed" + Environment.NewLine +
                    "You have found 1 of 5 opponents: Ana"), "check status after check in Bedroom");
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "check prompt after check in Bedroom");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Bedroom");
                
                // Move to the Sensory Room
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Sensory Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Sensory Room. You see the following exits:" + Environment.NewLine +
                    " - the Bedroom is to the West" + Environment.NewLine +
                    " - the Hallway is to the Southwest" + Environment.NewLine +
                    "Someone could hide under the beanbags" + Environment.NewLine +
                    "You have found 1 of 5 opponents: Ana"), "check status when move East to Sensory Room");
                Assert.That(gameController.Prompt, Is.EqualTo("8: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Sensory Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Sensory Room");

                // Check the Sensory Room - opponents 2 and 5 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding under the beanbags"), "check string returned when check in Sensory Room");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Sensory Room. You see the following exits:" + Environment.NewLine +
                    " - the Bedroom is to the West" + Environment.NewLine +
                    " - the Hallway is to the Southwest" + Environment.NewLine +
                    "Someone could hide under the beanbags" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status after check in Sensory Room");
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "check prompt after check in Sensory Room");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Sensory Room");
                
                // Move to the Hallway
                Assert.That(gameController.Move(Direction.Southwest), Is.EqualTo("Moving Southwest"), "check string returned when move Southwest to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status when move Southwest to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("10: Which direction do you want to go: "), "check prompt after move Southwest to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Southwest to Hallway");
                
                // Move to the Kitchen
                Assert.That(gameController.Move(Direction.East), Is.EqualTo("Moving East"), "check string returned when move East to Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Hallway is to the West" + Environment.NewLine +
                    " - the Cellar is Down" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status when move East to Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("11: Which direction do you want to go (or type 'check'): "), "check prompt after move East to Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move East to Kitchen");

                // Check the Kitchen - no opponents hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("Nobody was hiding beside the stove"), "check string returned when check in Kitchen");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Kitchen. You see the following exits:" + Environment.NewLine +
                    " - the Pantry is to the South" + Environment.NewLine +
                    " - the Hallway is to the West" + Environment.NewLine +
                    " - the Cellar is Down" + Environment.NewLine +
                    " - the Yard is Out" + Environment.NewLine +
                    "Someone could hide beside the stove" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status after check in Kitchen");
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "check prompt after check in Kitchen");
                Assert.That(gameController.GameOver, Is.False, "check game not over after check in Kitchen");

                // Move to the Hallway
                Assert.That(gameController.Move(Direction.West), Is.EqualTo("Moving West"), "check string returned when move West to Hallway");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Hallway. You see the following exits:" + Environment.NewLine +
                    " - the Attic is Up" + Environment.NewLine +
                    " - the Pantry is to the Southeast" + Environment.NewLine +
                    " - the Sensory Room is to the Northeast" + Environment.NewLine +
                    " - the Kitchen is to the East" + Environment.NewLine +
                    " - the Bedroom is to the North" + Environment.NewLine +
                    " - the Landing is to the South" + Environment.NewLine +
                    " - the Living Room is to the West" + Environment.NewLine +
                    " - the Bathroom is to the Southwest" + Environment.NewLine +
                    " - the Office is to the Northwest" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status when move West to Hallway");
                Assert.That(gameController.Prompt, Is.EqualTo("13: Which direction do you want to go: "), "check prompt after move West to Hallway");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move West to Hallway");

                // Move to the Attic
                Assert.That(gameController.Move(Direction.Up), Is.EqualTo("Moving Up"), "check string returned when move Up to Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "Someone could hide behind a trunk" + Environment.NewLine +
                    "You have found 3 of 5 opponents: Ana, Bob, Jimmy"), "check status when move Up to Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "check prompt after move Up to Attic");
                Assert.That(gameController.GameOver, Is.False, "check game not over after move Up to Attic");
                
                // Check the Attic - opponents 1 and 4 hiding there
                Assert.That(gameController.CheckCurrentLocation(), Is.EqualTo("You found 2 opponents hiding behind a trunk"), "check string returned when check in Attic");
                Assert.That(gameController.Status, Is.EqualTo(
                    "You are in the Attic. You see the following exit:" + Environment.NewLine +
                    " - the Hallway is Down" + Environment.NewLine +
                    "Someone could hide behind a trunk" + Environment.NewLine +
                    "You have found 5 of 5 opponents: Ana, Bob, Jimmy, Joe, Owen"), "check status after check in Attic");
                Assert.That(gameController.Prompt, Is.EqualTo("15: Which direction do you want to go (or type 'check'): "), "check prompt after check in Attic");
                Assert.That(gameController.GameOver, Is.True, "check game over after check in Attic");
            });
        }

        [TestCaseSource(typeof(TestGameController_ConstructorsAndRestartGame_TestData),
            nameof(TestGameController_ConstructorsAndRestartGame_TestData.TestCases_For_Test_GameController_RestartGame))]
        public void Test_GameController_RestartGame(string startingLocation, IEnumerable<string> hidingPlaces, Func<GameController> GetGameController)
        {
            // Set House random number generator to mock random
            House.Random = new MockRandomWithValueList(new int[] { 0, 1, 2, 3, 0 });

            // Set GameController
            GameController gameController = GetGameController();

            // Assert that properties are as expected
            Assert.Multiple(() =>
            {
                Assert.That(gameController.OpponentsAndHidingLocations.Values.Select((l) => l.Name), Is.EquivalentTo(hidingPlaces), "opponent hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no found opponents");
                Assert.That(gameController.MoveNumber, Is.EqualTo(1), "move number");
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(startingLocation), "current location");
                Assert.That(gameController.GameOver, Is.False, "game not over");
            });
        }
    }
}
