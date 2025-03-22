using HideAndSeek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Class to test DirectionExtensions methods to get opposite direction and get direction description
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
    }
}
