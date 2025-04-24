using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// GameController tests for Prompt property
    /// (integration tests using House, Location, and LocationWithHidingPlace)
    /// </summary>
    public class TestGameController_Prompt
    {
        GameController gameController;

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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Set static House file system to mock file system (not changed in any tests)
            House.FileSystem = MockFileSystemHelper.GetMockedFileSystem_ToReadAllText("DefaultHouse.house.json", DefaultHouse_Serialized);
        }

        [SetUp]
        public void SetUp()
        {
            gameController = new GameController(new Opponent[] { new Mock<Opponent>().Object }, "DefaultHouse"); // Create new GameController with mocked Opponent and default House layout
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            House.FileSystem = new FileSystem(); // Set static House file system to new file system
        }

        [Test]
        [Category("GameController Prompt Move")]
        public void Test_GameController_Prompt_InLocationsWithoutHidingPlace()
        {
            Assert.Multiple(() =>
            {
                // Check prompt in Entry
                Assert.That(gameController.Prompt, Is.EqualTo("1: Which direction do you want to go: "), "prompt in Entry");

                // Move to Hallway and check prompt
                gameController.Move(Direction.East); // Move East to Hallway
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go: "), "prompt in Hallway");

                // Move to Landing and check prompt
                gameController.Move(Direction.Up); // Move Up to Landing
                Assert.That(gameController.Prompt, Is.EqualTo("3: Which direction do you want to go: "), "prompt on Landing");
            });
        }

        [Test]
        [Category("GameController Prompt Move")]
        public void Test_GameController_Prompt_InLocationsWithHidingPlace()
        {
            Assert.Multiple(() =>
            {
                // Move to Garage and check prompt
                gameController.Move(Direction.Out); // Move Out to Garage
                Assert.That(gameController.Prompt, Is.EqualTo("2: Which direction do you want to go (or type 'check'): "), "prompt in Garage");

                // Move to Bathroom and check prompt
                gameController.Move(Direction.In); // Move In to Entry
                gameController.Move(Direction.East); // Move East to Hallway
                gameController.Move(Direction.North); // Move North to Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("5: Which direction do you want to go (or type 'check'): "), "prompt in Bathroom");

                // Move to Living Room and check prompt
                gameController.Move(Direction.South); // Move South to Hallway
                gameController.Move(Direction.South); // Move South to Living Room
                Assert.That(gameController.Prompt, Is.EqualTo("7: Which direction do you want to go (or type 'check'): "), "prompt in Living Room");

                // Move to Kitchen and check prompt
                gameController.Move(Direction.North); // Move North to Hallway
                gameController.Move(Direction.Northwest); // Move Northwest to Kitchen
                Assert.That(gameController.Prompt, Is.EqualTo("9: Which direction do you want to go (or type 'check'): "), "prompt in Kitchen");

                // Move to Attic and check prompt
                gameController.Move(Direction.Southeast); // Move Southeast to Hallway
                gameController.Move(Direction.Up); // Move Up to Landing
                gameController.Move(Direction.Up); // Move Up to Attic
                Assert.That(gameController.Prompt, Is.EqualTo("12: Which direction do you want to go (or type 'check'): "), "prompt in Attic");

                // Move to Kids Room and check prompt
                gameController.Move(Direction.Down); // Move Down to Landing
                gameController.Move(Direction.Southeast); // Move Southeast to Kids Room
                Assert.That(gameController.Prompt, Is.EqualTo("14: Which direction do you want to go (or type 'check'): "), "prompt in Kids Room");

                // Move to Pantry and check prompt
                gameController.Move(Direction.Northwest); // Move Northwest to Landing
                gameController.Move(Direction.South); // Move South to Pantry
                Assert.That(gameController.Prompt, Is.EqualTo("16: Which direction do you want to go (or type 'check'): "), "prompt in Pantry");

                // Move to Second Bathroom and check prompt
                gameController.Move(Direction.North); // Move North to Landing
                gameController.Move(Direction.West); // Move West to Second Bathroom
                Assert.That(gameController.Prompt, Is.EqualTo("18: Which direction do you want to go (or type 'check'): "), "prompt in Second Bathroom");

                // Move to Nursery and check prompt
                gameController.Move(Direction.East); // Move East to Landing
                gameController.Move(Direction.Southwest); // Move Southwest to Nursery
                Assert.That(gameController.Prompt, Is.EqualTo("20: Which direction do you want to go (or type 'check'): "), "prompt in Nursery");

                // Move to Master Bedroom and check prompt
                gameController.Move(Direction.Northeast); // Move Northeast to Landing
                gameController.Move(Direction.Northwest); // Move Northwest to Master Bedroom
                Assert.That(gameController.Prompt, Is.EqualTo("22: Which direction do you want to go (or type 'check'): "), "prompt in Master Bedroom");

                // Move to Master Bath and check prompt
                gameController.Move(Direction.East); // Move East to Master Bath
                Assert.That(gameController.Prompt, Is.EqualTo("23: Which direction do you want to go (or type 'check'): "), "prompt in Master Bath");
            });
        }
    }
}