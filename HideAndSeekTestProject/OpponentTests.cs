using HideAndSeekTestProject;
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
        [Test]
        [Category("Opponent Name")]
        public void Test_Opponent_Constructor_Parameterized_SetsToNamePassedIn()
        {
            Opponent opponent = new Opponent("John Doe");
            Assert.That(opponent.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        [Category("Opponent Name")]
        public void Test_Opponent_Constructor_Unparameterized_SetsToDefaultName()
        {
            // Reset default numbers for Opponent names
            Opponent.ResetDefaultNumbersForOpponentNames();

            // Create two new Opponents
            Opponent opponent1 = new Opponent();
            Opponent opponent2 = new Opponent();

            // Assert that Opponent names are as expected
            Assert.Multiple(() =>
            {
                Assert.That(opponent1.Name, Is.EqualTo("Random Opponent 1"));
                Assert.That(opponent2.Name, Is.EqualTo("Random Opponent 2"));
            });
        }
    }
}