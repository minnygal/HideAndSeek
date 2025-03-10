using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Opponent tests for setting name
    /// </summary>
    [TestFixture]
    public class OpponentTests
    {
        private House house;

        [SetUp]
        public void SetUp()
        {
            house = new House();
            house.ClearHidingPlaces();
        }

        [Test]
        [Category("Opponent Name")]
        public void Test_Opponent_SpecifiedName_IsSetCorrectly()
        {
            Opponent opponent = new Opponent("John Doe");
            Assert.That(opponent.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        [Category("Opponent Name")]
        public void Test_Opponent_UnspecifiedName_IsSetCorrectly()
        {
            Opponent.ResetDefaultNumbersForOpponentNames();
            Opponent opponent1 = new Opponent();
            Opponent opponent2 = new Opponent();

            Assert.Multiple(() =>
            {
                Assert.That(opponent1.Name, Is.EqualTo("Random Opponent 1"));
                Assert.That(opponent2.Name, Is.EqualTo("Random Opponent 2"));
            });
        }
    }
}