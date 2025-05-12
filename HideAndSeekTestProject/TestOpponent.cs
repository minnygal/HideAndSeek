using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Opponent unit tests for setting Name property and static method returning whether name is valid
    /// </summary>
    [TestFixture]
    public class TestOpponent
    {
        [SetUp]
        [OneTimeTearDown]
        public void SetUpAndOneTearDown()
        {
            Opponent.ResetDefaultNumbersForOpponentNames(); // Reset static default numbers for Opponent names
        }

        [Test]
        [Category("Opponent Constructor Name Success")]
        public void Test_Opponent_Constructor_Parameterized_SetsToNamePassedIn()
        {
            Assert.That(new Opponent("John Doe").Name, Is.EqualTo("John Doe"));
        }

        [Test]
        [Category("Opponent Constructor Name Success")]
        public void Test_Opponent_Constructor_Unparameterized_SetsToDefaultName()
        {
            Assert.Multiple(() =>
            {
                Assert.That(new Opponent().Name, Is.EqualTo("Random Opponent 1"));
                Assert.That(new Opponent().Name, Is.EqualTo("Random Opponent 2"));
            });
        }

        [Test]
        [Category("Opponent ResetDefaultNumbersForOpponentNames Success")]
        public void Test_Opponent_ResetDefaultNumbersForOpponentNames()
        {
            Assert.Multiple(() =>
            {
                Assert.That(new Opponent().Name, Does.EndWith("1"), "first new Opponent name ending before resetting");
                Assert.That(new Opponent().Name, Does.EndWith("2"), "second new Opponent name ending before resetting");
                Opponent.ResetDefaultNumbersForOpponentNames(); // Reset numbers for Opponent names
                Assert.That(new Opponent().Name, Does.EndWith("1"), "new Opponent name ending after resetting");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Opponent Constructor Name ArgumentException Failure")]
        public void Test_Opponent_Constructor_Parameterized_AndCheckErrorMessage_ForInvalidName(string name)
        {
            Assert.Multiple(() =>
            {
                // Assert that creating a new opponent with an invalid name raises exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    new Opponent(name);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith($"opponent name \"{name}\" is invalid (is empty or contains only whitespace)"));
            });
        }

        [TestCase("J")]
        [TestCase("John")]
        [TestCase("Mary Alice")]
        [TestCase("Patrick S O'Sullivan")]
        [TestCase("'oMalley")]
        [Category("Opponent IsValidName True")]
        public void Test_Opponent_IsValidName_ValidName(string name)
        {
            Assert.That(Opponent.IsValidName(name), Is.True);
        }

        [TestCase("")]
        [TestCase(" ")]
        [Category("Opponent IsValidName False")]
        public void Test_Opponent_IsValidName_InvalidName(string name)
        {
            Assert.That(Opponent.IsValidName(name), Is.False);
        }
    }
}