using HideAndSeek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// DirectionExtensions tests for methods to get opposite direction and direction description
    /// </summary>
    [TestFixture]
    public class TestDirectionExtensions
    {
        [TestCase(Direction.North, Direction.South)]
        [TestCase(Direction.South, Direction.North)]
        [TestCase(Direction.East, Direction.West)]
        [TestCase(Direction.West, Direction.East)]
        [TestCase(Direction.Northeast, Direction.Southwest)]
        [TestCase(Direction.Southwest, Direction.Northeast)]
        [TestCase(Direction.Southeast, Direction.Northwest)]
        [TestCase(Direction.Northwest, Direction.Southeast)]
        [TestCase(Direction.Up, Direction.Down)]
        [TestCase(Direction.Down, Direction.Up)]
        [TestCase(Direction.In, Direction.Out)]
        [TestCase(Direction.Out, Direction.In)]
        [Category("DirectionExtensions OppositeDirection")]
        public void Test_DirectionExtensions_OppositeDirection(Direction inputDirection, Direction expectedOutputDirection)
        {
            Assert.That(inputDirection.OppositeDirection(), Is.EqualTo(expectedOutputDirection));
        }

        [TestCase(Direction.North, "to the North")]
        [TestCase(Direction.South, "to the South")]
        [TestCase(Direction.East, "to the East")]
        [TestCase(Direction.West, "to the West")]
        [TestCase(Direction.Northeast, "to the Northeast")]
        [TestCase(Direction.Southwest, "to the Southwest")]
        [TestCase(Direction.Southeast, "to the Southeast")]
        [TestCase(Direction.Northwest, "to the Northwest")]
        [Category("DirectionExtensions DirectionDescription")]
        public void Test_DirectionExtensions_DirectionDescription_WithToTheText(Direction direction, string expectedText)
        {
            Assert.That(direction.DirectionDescription(), Is.EqualTo(expectedText));
        }

        [TestCase(Direction.Up, "Up")]
        [TestCase(Direction.Down, "Down")]
        [TestCase(Direction.In, "In")]
        [TestCase(Direction.Out, "Out")]
        [Category("DirectionExtensions DirectionDescription")]
        public void Test_DirectionExtensions_DirectionDescription_WithJustDirectionAsText(Direction direction, string expectedText)
        {
            Assert.That(direction.DirectionDescription(), Is.EqualTo(expectedText));
        }

        [TestCase("north", Direction.North)]
        [TestCase("n", Direction.North)]
        [TestCase("south", Direction.South)]
        [TestCase("s", Direction.South)]
        [TestCase("east", Direction.East)]
        [TestCase("e", Direction.East)]
        [TestCase("west", Direction.West)]
        [TestCase("w", Direction.West)]
        [TestCase("northeast", Direction.Northeast)]
        [TestCase("ne", Direction.Northeast)]
        [TestCase("southwest", Direction.Southwest)]
        [TestCase("sw", Direction.Southwest)]
        [TestCase("southeast", Direction.Southeast)]
        [TestCase("se", Direction.Southeast)]
        [TestCase("northwest", Direction.Northwest)]
        [TestCase("nw", Direction.Northwest)]
        [TestCase("up", Direction.Up)]
        [TestCase("u", Direction.Up)]
        [TestCase("down", Direction.Down)]
        [TestCase("d", Direction.Down)]
        [TestCase("in", Direction.In)]
        [TestCase("i", Direction.In)]
        [TestCase("out", Direction.Out)]
        [TestCase("o", Direction.Out)]
        public void Test_DirectionExtensions_Parse_Lowercase(string directionText, Direction expectedDirection)
        {
            bool parseSuccessful = DirectionExtensions.TryParse(directionText, out Direction actualDirection);
            Assert.Multiple(() =>
            {
                Assert.That(parseSuccessful, Is.True, "value returned indicating whether parse was successful");
                Assert.That(actualDirection, Is.EqualTo(expectedDirection), "Direction from out variable");
            });
        }

        [TestCase("N")]
        [TestCase("North")]
        [TestCase("nOrth")]
        [TestCase("nOrTh")]
        [TestCase("NORTH")]
        public void Test_DirectionExtensions_Parse_MixedCase(string directionText)
        {
            bool parseSuccessful = DirectionExtensions.TryParse(directionText, out Direction actualDirection);
            Assert.Multiple(() =>
            {
                Assert.That(parseSuccessful, Is.True, "value returned indicating whether parse was successful");
                Assert.That(actualDirection, Is.EqualTo(Direction.North), "Direction from out variable");
            });
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("}{yaeu\\@!//")]
        [TestCase("No")]
        [TestCase("Northuperly")]
        public void Test_DirectionExtensions_Parse_InvalidDirection(string directionText)
        {
            bool parseSuccessful = DirectionExtensions.TryParse(directionText, out Direction direction);
            Assert.Multiple(() =>
            {
                Assert.That(parseSuccessful, Is.False, "value returned indicating whether parse was successful");
                Assert.That( (int)direction, Is.EqualTo(0), "value from out variable cast to int" );
            });
        }
    }
}
