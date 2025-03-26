using Moq;
using System.IO.Abstractions;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for saving/loading/deleting games
    /// </summary>
    [TestFixture]
    public class TestGameController_SaveLoadDeleteGame
    {
        private GameController gameController;
        private string message; // Message returned by GameController's ParseInput method
        private Mock<IFileSystem> mockFileSystem; // Mock file system to be passed to GameController upon creation

        [SetUp]
        public void Setup()
        {
            gameController = null;
            message = null;
            mockFileSystem = new Mock<IFileSystem>(); // Set mock file system variable to new file system
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForInvalidFileName(
            [Values("save", "load", "delete")] string commandKeyword,
            [Values(" ", " my saved game", " my\\saved\\game", " my/saved/game", " my/saved\\ game")] string restOfCommand)
        {
            gameController = new GameController();
            message = gameController.ParseInput(commandKeyword + restOfCommand);
            Assert.That(message, Is.EqualTo($"Cannot perform action because file name \"{restOfCommand.Substring(1)}\" is invalid (is empty or contains illegal characters, e.g. \\, /, or whitespace)"));
        }

        [TestCase("save")]
        [TestCase("load")]
        [TestCase("delete")]
        [Category("GameController Save Load Delete Failure")]
        public void Test_GameController_ParseInput_ToSaveLoadOrDeleteGame_AndCheckErrorMessage_ForNoFileName(string commandWord)
        {
            gameController = new GameController();
            message = gameController.ParseInput(commandWord);
            Assert.That(message, Is.EqualTo("Cannot perform action because no file name was entered"));
        }

        [TestCase("a", "Game successfully saved in a"),]
        [TestCase("my_saved_game", "Game successfully saved in my_saved_game")]
        [Category("GameController Save Success")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Set up mock for file system
            mockFileSystem.Setup(system => system.File.WriteAllText($"{fileName}.json", It.IsAny<string>())); // Accept any text written to file

            // Set up game cotroller
            gameController = new GameController(mockFileSystem.Object);

            // Attempt to save game
            message = gameController.ParseInput($"save {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        // Tests default House, and tests custom House set via constructor and via ReloadGame
        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile))]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckTextSavedToFile(string houseFileName, string houseFileText, Func<IFileSystem, Mock<IFileSystem>, GameController> startNewGame, string expectedTextInSavedGameFile)
        {
            // Set House file system
            Mock<IFileSystem> mockHouseFileSystem = MockFileSystemHelper.GetMockOfFileSystem_ToReadAllText(houseFileName, houseFileText);
            House.FileSystem = mockHouseFileSystem.Object;

            // Create variable to store text written to SavedGame file
            string? actualTextInSavedGameFile = null;

            // Set up mock for GameController file system
            mockFileSystem.Setup(system => system.File.WriteAllText("my_saved_game.json", It.IsAny<string>()))
                    .Callback((string path, string text) =>
                    {
                        actualTextInSavedGameFile = text; // Store text written to file in variable
                    });

            // Start and attempt to save game
            gameController = startNewGame(mockFileSystem.Object, mockHouseFileSystem); // Mock House file system is used in test cases calling RestartGame
            gameController.ParseInput("save my_saved_game");

            // Assert that actual text in file is equal to expected text in file
            Assert.That(actualTextInSavedGameFile, Is.EqualTo(expectedTextInSavedGameFile));
        }

        [Test]
        [Category("GameController Save Failure")]
        public void Test_GameController_ParseInput_ToSaveGame_AndCheckErrorMessage_ForAlreadyExistingFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(system => system.File.Exists("fileName.json")).Returns(true); // Mock that file already exists

            // Set up game cotroller
            gameController = new GameController(mockFileSystem.Object);

            // Attempt to save game
            message = gameController.ParseInput($"save fileName");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot perform action because a file named fileName already exists"));
        }

        [TestCase("a", "Game successfully loaded from a")]
        [TestCase("my_saved_game", "Game successfully loaded from my_saved_game")]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckSuccessMessage(string fileName, string expected)
        {
            // Initialize variable to text stored in mock file
            string textInFile = "{\"HouseFileName\":\"DefaultHouse\",\"PlayerLocation\":\"Entry\",\"MoveNumber\":1,\"OpponentsAndHidingLocations\":{\"Joe\":\"Kitchen\",\"Bob\":\"Pantry\",\"Ana\":\"Bathroom\",\"Owen\":\"Kitchen\",\"Jimmy\":\"Pantry\"},\"FoundOpponents\":[]}";

            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists($"{fileName}.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText($"{fileName}.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput($"load {fileName}");

            // Assert that success message is correct
            Assert.That(message, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents))]
        public void Test_GameController_ParseInput_ToLoadGame_WithNoFoundOpponents(
            string savedGameFileText, string houseName, string houseFileName, string houseFileText, string housePlayerStartingPoint,
            IEnumerable<string> locations, IEnumerable<string> locationsWithoutHidingPlaces, IEnumerable<string> locationsWithHidingPlaces,
            string currentLocation, int moveNumber, IEnumerable<string> opponents, IEnumerable<string> opponentHidingLocations)
        {
            // Set mock file system for House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText($"{houseFileName}.json", houseFileText);

            // Set up mock for file system for GameController
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(savedGameFileText); // Mock what file returns

            // Create new game controller with specified House layout (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object, houseFileName);

            // Have game controller parse file name with load command and store message
            message = gameController.ParseInput($"load my_saved_game");

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo(houseName), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo(houseFileName), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo(housePlayerStartingPoint), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(locations), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithoutHidingPlaces), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(locationsWithHidingPlaces), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo(currentLocation), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(moveNumber), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(opponents), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(opponentHidingLocations), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents, Is.Empty, "no opponents found");

                // Assert that locations with hiding places that should not have hidden Opponents do not have any hidden Opponents
                gameController.House.LocationsWithHidingPlaces.Where((l) => !(opponentHidingLocations.Distinct().Contains(l.Name))).ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [Test]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_With1FoundOpponent()
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Bathroom\"" + "," +
                    "\"MoveNumber\":4" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    "\"FoundOpponents\":[\"Ana\"]" +
                "}";

            // Have game controller parse file name with load command
            message = ParseInputToLoadGameSuccessfully(savedGameFileText);
            
            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_Locations_Names), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithoutHidingPlaces_Names), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithHidingPlaces_Names), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(4), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(new List<string>() { "Ana" }), "found opponent");

                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Kitchen").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                            "Joe and Owen are hidden in the Kitchen");

                // Assert that Opponents Bob and Jimmy are hiding in the Pantry
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Pantry").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                            "Bob and Jimmy are hidden in the Pantry");

                // Assert that all the other locations with hiding places do not have any Opponents hidden there
                gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name != "Kitchen" && l.Name != "Pantry").ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [Test]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_With2FoundOpponents()
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Pantry\"" + "," +
                    "\"MoveNumber\":5" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    "\"FoundOpponents\":[\"Bob\",\"Jimmy\"]" +
                "}";

            // Have game controller parse file name with load command
            message = ParseInputToLoadGameSuccessfully(savedGameFileText);

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_Locations_Names), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithoutHidingPlaces_Names), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithHidingPlaces_Names), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Pantry"), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(5), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }), "found opponents");

                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Kitchen").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Joe", "Owen" }),
                            "Joe and Owen are hidden in the Kitchen");

                // Assert that Ana is hiding in the Bathroom
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Bathroom").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Ana" }),
                            "Ana is hidden in the Bathroom");

                // Assert that all the other locations with hiding places do not have any Opponents hidden there
                gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name != "Kitchen" && l.Name != "Bathroom").ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [Test]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_With3FoundOpponents()
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_PlayerLocation + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_MoveNumber + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    MyTestSavedGame.SerializedTestSavedGame_3FoundOpponents_FoundOpponents +
                "}";

            // Have game controller parse file name with load command
            message = ParseInputToLoadGameSuccessfully(savedGameFileText);

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_Locations_Names), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithoutHidingPlaces_Names), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithHidingPlaces_Names), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Bathroom"), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(7), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(new List<string>() { "Joe", "Owen", "Ana" }), "found opponents");

                // Assert that Opponents Joe and Owen are hiding in the Kitchen
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Pantry").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Bob", "Jimmy" }),
                            "Bob and Jimmy are hidden in the Kitchen");

                // Assert that all the other locations with hiding places do not have any Opponents hidden there
                gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name != "Pantry").ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [Test]
        [Category("GameController Load Success")]
        public void Test_GameController_ParseInput_ToLoadGame_With4FoundOpponents()
        {
            // Initialize variable to serialized SavedGame
            string savedGameFileText =
                "{" +
                    "\"HouseFileName\":\"DefaultHouse\"" + "," +
                    "\"PlayerLocation\":\"Pantry\"" + "," +
                    "\"MoveNumber\":10" + "," +
                    MyTestSavedGame.SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                    "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Bob\",\"Jimmy\"]" +
                "}";

            // Have game controller parse file name with load command
            message = ParseInputToLoadGameSuccessfully(savedGameFileText);

            // Assert that message is as expected and game state has been restored successfully
            Assert.Multiple(() =>
            {
                // Assert that ParseInput return message is as expected
                Assert.That(message, Is.EqualTo("Game successfully loaded from my_saved_game"), "ParseInput return message");

                // Assert that House properties are as expected
                Assert.That(gameController.House.Name, Is.EqualTo("test house"), "House name");
                Assert.That(gameController.House.HouseFileName, Is.EqualTo("TestHouse"), "House file name");
                Assert.That(gameController.House.PlayerStartingPoint, Is.EqualTo("Entry"), "House starting point");
                Assert.That(gameController.House.Locations.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_Locations_Names), "House all locations names");
                Assert.That(gameController.House.LocationsWithoutHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithoutHidingPlaces_Names), "House locations without hiding places names");
                Assert.That(gameController.House.LocationsWithHidingPlaces.Select((l) => l.Name), Is.EquivalentTo(MyTestHouse.TestHouseExpectedProperties_LocationsWithHidingPlaces_Names), "House locations with hiding places names");

                // Assert that GameController properties are as expected
                Assert.That(gameController.CurrentLocation.Name, Is.EqualTo("Pantry"), "player location");
                Assert.That(gameController.MoveNumber, Is.EqualTo(10), "move number");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Key.Name), Is.EquivalentTo(new List<string>() { "Joe", "Bob", "Ana", "Owen", "Jimmy" }), "all opponents");
                Assert.That(gameController.OpponentsAndHidingLocations.Select((x) => x.Value.Name), Is.EquivalentTo(new List<string>() { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" }), "opponents' hiding places");
                Assert.That(gameController.FoundOpponents.Select((x) => x.Name), Is.EquivalentTo(new List<string>() { "Joe", "Owen", "Bob", "Jimmy" }), "found opponents");

                // Assert that Ana is hiding in the Bathroom
                Assert.That(gameController.House.LocationsWithHidingPlaces
                                .Where((l) => l.Name == "Bathroom").First()
                                .CheckHidingPlace()
                                .Select((o) => o.Name),
                            Is.EquivalentTo(new List<string>() { "Ana" }),
                            "Ana is hidden in the Bathroom");

                // Assert that all the other locations with hiding places do not have any Opponents hidden there
                gameController.House.LocationsWithHidingPlaces.Where((l) => l.Name != "Bathroom").ToList().ForEach((l) =>
                {
                    Assert.That(l.CheckHidingPlace, Is.Empty, $"no Opponents hiding in the {l.Name}");
                });
            });
        }

        [Test]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForNonexistentSavedGameFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(false); // Mock that file does not exist

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_saved_game");

            // Assert that error message is correct
            Assert.That(message, Is.EqualTo("Cannot load game because file my_saved_game does not exist"));
        }

        [TestCaseSource(typeof(TestGameController_SaveLoadDeleteGame_TestCaseData), nameof(TestGameController_SaveLoadDeleteGame_TestCaseData.TestCases_For_Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData))]
        [Category("GameController Load Failure")]
        public void Test_GameController_ParseInput_ToLoadGame_AndCheckErrorMessage_ForInvalidData(string endOfErrorMessage, string textInFile)
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_corrupt_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_corrupt_game.json")).Returns(textInFile); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command
            message = gameController.ParseInput("load my_corrupt_game");
            
            // Assert that error message is correct
            Assert.That(message, Is.EqualTo($"Cannot process because data is corrupt - {endOfErrorMessage}"));
        }

        [Test]
        [Category("GameController Delete Success")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckSuccessMessage()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_saved_game");

            // Verify that File.Delete was called once
            mockFileSystem.Verify(manager => manager.File.Delete("my_saved_game.json"), Times.Once);

            // Assert that success message is as expected
            Assert.That(message, Is.EqualTo("Game file my_saved_game has been successfully deleted"));
        }

        [Test]
        [Category("GameController Delete Failure")]
        public void Test_GameController_ParseInput_ToDeleteGame_AndCheckErrorMessage_ForNonexistentFile()
        {
            // Set up mock for file system
            mockFileSystem.Setup(manager => manager.File.Exists("my_nonexistent_game.json")).Returns(false); // Mock that file does not exist

            // Create new game controller
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with delete command
            message = gameController.ParseInput("delete my_nonexistent_game");

            // Assert that error message is as expected
            Assert.That(message, Is.EqualTo("Could not delete game because file my_nonexistent_game does not exist"));
        }

        /// <summary>
        /// Helper method to call GameController ParseInput to load game with specific SavedGame file text
        /// </summary>
        /// <param name="savedGamedFileText">Text in SavedGame file</param>
        /// <returns>Message returned by GameController ParseInput</returns>
        private string ParseInputToLoadGameSuccessfully(string savedGamedFileText)
        {
            // Set mock file system for House property
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.json", MyTestHouse.SerializedTestHouse);

            // Set up mock for file system for GameController
            mockFileSystem.Setup(manager => manager.File.Exists("my_saved_game.json")).Returns(true); // Mock that file exists
            mockFileSystem.Setup(manager => manager.File.ReadAllText("my_saved_game.json")).Returns(savedGamedFileText); // Mock what file returns

            // Create new game controller (Random not mocked, so truly random hiding places generated)
            gameController = new GameController(mockFileSystem.Object);

            // Have game controller parse file name with load command and return message
            return gameController.ParseInput($"load my_saved_game");
        }
    }
}
