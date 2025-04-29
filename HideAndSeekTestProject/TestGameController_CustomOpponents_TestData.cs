using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HideAndSeek
{
    /// <summary>
    /// Test data for GameController tests for custom Opponents
    /// </summary>
    public static class TestGameController_CustomOpponents_TestData
    {
        /// <summary>
        /// Text representing default House for tests serialized
        /// </summary>
        public static string DefaultHouse_Serialized
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

        public static IEnumerable TestCases_For_Test_GameController_Constructor_WithSpecifiedNumberOfOpponents
        {
            get
            {
                yield return new TestCaseData(
                    1,
                    new string[] { "Joe" },
                    new string[] { "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 1 opponent");

                yield return new TestCaseData(
                    2,
                    new string[] { "Joe", "Bob" },
                    new string[] { "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 2 opponents");

                yield return new TestCaseData(
                    3,
                    new string[] { "Joe", "Bob", "Ana" },
                    new string[] { "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 3 opponents");

                yield return new TestCaseData(
                    4,
                    new string[] { "Joe", "Bob", "Ana", "Owen" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 4 opponents");

                yield return new TestCaseData(
                    5,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 5 opponents");

                yield return new TestCaseData(
                    6,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 6 opponents");

                yield return new TestCaseData(
                    7,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 7 opponents");

                yield return new TestCaseData(
                    8,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 8 opponents");

                yield return new TestCaseData(
                    9,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 9 opponents");

                yield return new TestCaseData(
                    10,
                    new string[] { "Joe", "Bob", "Ana", "Owen", "Jimmy", "Mary", "Alice", "Tony", "Andy", "Jill" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNumberOfOpponents - 10 opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_WithSpecifiedNamesOfOpponents
        {
            get
            {
                yield return new TestCaseData(
                    new string[] { "Henry" }, 
                    new string[] { "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 1 opponent");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia" }, 
                    new string[] { "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 2 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul" }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 3 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy" }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 4 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 5 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 6 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 7 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 8 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 9 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn", "Jackie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 10 opponents");

                yield return new TestCaseData(
                    new string[] { "Henry", "Talia", "Paul", "Wendy", "Fred", "Annie", "George", "Betty", "Bjorn", "Jackie", "Alice", "Steve", "Jenn", "Jude", "Jamie" },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedNamesOfOpponents - 15 opponents");
            }
        }

        public static IEnumerable TestCases_For_Test_GameController_Constructor_WithSpecifiedOpponents
        {
            get
            {
                yield return new TestCaseData(new Opponent[] { new Mock<Opponent>().Object }, new string[] { "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 1 opponent");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 2 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 3 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 4 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, 
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 5 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 6 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object }
                    , new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 7 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object },
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 8 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object ,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 9 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object ,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object }, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 10 opponents");

                yield return new TestCaseData(
                    new Opponent[] { new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object ,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,
                        new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object, new Mock<Opponent>().Object,}, 
                    new string[] { "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", 
                                   "Kitchen", "Pantry", "Kitchen", "Pantry", "Bathroom", "Kitchen", "Pantry" })
                .SetName("Test_GameController_Constructor_WithSpecifiedOpponents - 15 opponents");
            }
        }
    }
}
