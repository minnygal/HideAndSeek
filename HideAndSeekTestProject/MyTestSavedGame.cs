using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Static class providing SavedGame object for testing and serialized SavedGame file text for testing
    /// </summary>
    public static class MyTestSavedGame
    {
        /// <summary>
        /// Dictionary of names of opponents and hiding places in test SavedGame
        /// </summary>
        public static Dictionary<string, string> OpponentsAndHidingPlaces
        {
            get
            {
                Dictionary<string, string> opponentsAndHidingPlaces = new Dictionary<string, string>();
                opponentsAndHidingPlaces.Add("Joe", "Kitchen");
                opponentsAndHidingPlaces.Add("Bob", "Pantry");
                opponentsAndHidingPlaces.Add("Ana", "Bathroom");
                opponentsAndHidingPlaces.Add("Owen", "Kitchen");
                opponentsAndHidingPlaces.Add("Jimmy", "Pantry");
                return opponentsAndHidingPlaces;
            }
        }

        /// <summary>
        /// Get new SavedGame object with no found opponents for testing purposes
        /// </summary>
        /// <returns></returns>
        public static SavedGame GetNewTestSavedGame_NoFoundOpponents()
        {
            return new SavedGame(
                MyTestHouse.GetNewTestHouse(), "DefaultHouse", "Entry", 1, OpponentsAndHidingPlaces, new List<string>());
        }

        /// <summary>
        /// Get new SavedGame object with 3 found opponents for testing purposes
        /// </summary>
        /// <returns></returns>
        public static SavedGame GetNewTestSavedGame_3FoundOpponents()
        {
            return new SavedGame(
                MyTestHouse.GetNewTestHouse(), "DefaultHouse", "Bathroom", 7, OpponentsAndHidingPlaces,
                FoundOpponents_3FoundOpponents
            );
        }

        /// <summary>
        /// Names of Opponents found in test SavedGame with 3 Opponents found
        /// </summary>
        public static IEnumerable<string> FoundOpponents_3FoundOpponents 
        {
            get 
            {
                return new List<string>() { "Joe", "Owen", "Ana" };
            }
        }

        /// <summary>
        /// Text for serialized test SavedGame with no found opponents
        /// </summary>
        public static readonly string SerializedTestSavedGame_NoFoundOpponents =
            "{" +
                SerializedTestSavedGame_HouseFileName + "," +
                SerializedTestSavedGame_NoFoundOpponents_PlayerLocation + "," +
                SerializedTestSavedGame_NoFoundOpponents_MoveNumber + "," +
                SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                SerializedTestSavedGame_NoFoundOpponents_FoundOpponents +
            "}";

        /// <summary>
        /// Text for serialized test SavedGame with 3 found opponents
        /// </summary>
        public static readonly string SerializedTestSavedGame_3FoundOpponents =
            "{" +
                SerializedTestSavedGame_HouseFileName + "," +
                SerializedTestSavedGame_3FoundOpponents_PlayerLocation + "," +
                SerializedTestSavedGame_3FoundOpponents_MoveNumber + "," +
                SerializedTestSavedGame_OpponentsAndHidingLocations + "," +
                SerializedTestSavedGame_3FoundOpponents_FoundOpponents +
            "}";

        /// <summary>
        /// Text for serialized HouseFileName property for test SavedGame
        /// </summary>
        public static string SerializedTestSavedGame_HouseFileName
        {
            get
            {
                return "\"HouseFileName\":\"DefaultHouse\"";
            }
        }

        /// <summary>
        /// Text for serialized PlayerLocation property for test SavedGame with no found opponents
        /// </summary>
        public static string SerializedTestSavedGame_NoFoundOpponents_PlayerLocation
        {
            get
            {
                return "\"PlayerLocation\":\"Entry\"";
            }
        }

        /// <summary>
        /// Text for serialized PlayerLocation property for test SavedGame with 3 found opponents
        /// </summary>
        public static string SerializedTestSavedGame_3FoundOpponents_PlayerLocation
        {
            get
            {
                return "\"PlayerLocation\":\"Bathroom\"";
            }
        }

        /// <summary>
        /// Text for serialized MoveNumber property for test SavedGame with no found opponents
        /// </summary>
        public static string SerializedTestSavedGame_NoFoundOpponents_MoveNumber
        {
            get
            {
                return "\"MoveNumber\":1";
            }
        }

        /// <summary>
        /// Text for serialized MoveNumber property for test SavedGame with 3 found opponents
        /// </summary>
        public static string SerializedTestSavedGame_3FoundOpponents_MoveNumber
        {
            get
            {
                return "\"MoveNumber\":7";
            }
        }

        /// <summary>
        /// Text for serialized OpponentsAndHidingLocations property for test SavedGame
        /// </summary>
        public static string SerializedTestSavedGame_OpponentsAndHidingLocations
        {
            get
            {
                return 
                    "\"OpponentsAndHidingLocations\":" +
                    "{" +
                        "\"Joe\":\"Kitchen\"," +
                        "\"Bob\":\"Pantry\"," +
                        "\"Ana\":\"Bathroom\"," +
                        "\"Owen\":\"Kitchen\"," +
                        "\"Jimmy\":\"Pantry\"" +
                    "}";
            }
        }

        /// <summary>
        /// Text for serialized FoundOpponents property for test SavedGame with no found opponents
        /// </summary>
        public static string SerializedTestSavedGame_NoFoundOpponents_FoundOpponents
        {
            get
            {
                return "\"FoundOpponents\":[]";
            }
        }

        /// <summary>
        /// Text for serialized FoundOpponents property for test SavedGame with 3 found opponents
        /// </summary>
        public static string SerializedTestSavedGame_3FoundOpponents_FoundOpponents
        {
            get
            {
                return "\"FoundOpponents\":[\"Joe\",\"Owen\",\"Ana\"]";
            }
        }
    }
}
