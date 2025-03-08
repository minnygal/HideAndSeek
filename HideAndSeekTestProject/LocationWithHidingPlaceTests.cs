namespace HideAndSeek
{
    using HideAndSeek;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// LocationWithHidingPlace tests for testing properties and methods HideOpponent and CheckHidingPlace
    /// </summary>
    [TestFixture]
    public class LocationWithHidingPlaceTests
    {
        [Test]
        [Category("LocationWithHidingPlace")]
        public void Test_LocationWithHidingPlace_Properties()
        {
            // Create a new LocationWithHidingPlace, setting the Name and HidingPlace properties
            var hidingLocation = new LocationWithHidingPlace("Room", "under the bed");

            // Assert that properties have been set correctly
            Assert.Multiple(() =>
            {
                Assert.That(hidingLocation.Name, Is.EqualTo("Room"), "location name");
                Assert.That(hidingLocation.ToString(), Is.EqualTo("Room"), "location as string");
                Assert.That(hidingLocation.HidingPlace, Is.EqualTo("under the bed"), "HidingPlace property");
            });
        }

        [Test]
        [Category("LocationWithHidingPlace HideOpponent CheckHidingPlace")]
        public void Test_LocationWithHidingPlace_HideOpponents_And_CheckHidingPlace()
        {
            // Create a new LocationWithHidingPlace
            var hidingLocation = new LocationWithHidingPlace("Room", "under the bed");

            // Create two opponents
            var opponent1 = new Opponent();
            var opponent2 = new Opponent();

            // Hide opponents in hiding place
            hidingLocation.HideOpponent(opponent1);
            hidingLocation.HideOpponent(opponent2);

            // Initialize list of expected opponents in hiding place
            List<Opponent> expectedOpponentsInHidingPlace = new List<Opponent>() { opponent1, opponent2 };

            // Check for opponents in hiding place
            Assert.Multiple(() =>
            {
                Assert.That(hidingLocation.CheckHidingPlace(), Is.EquivalentTo(expectedOpponentsInHidingPlace), "opponents found when hiding place checked"); // Check hiding place, finding opponents
                Assert.That(hidingLocation.CheckHidingPlace(), Is.Empty, "hiding place empty after being checked"); // Check hiding place again, should be empty because opponents already found
            });
        }
    }
}