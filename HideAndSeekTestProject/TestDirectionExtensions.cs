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
        [Category("DirectionExtensions Parse Success")]
        public void Test_DirectionExtensions_Parse_Lowercase(string directionText, Direction expectedDirection)
        {
            Assert.That(DirectionExtensions.Parse(directionText), Is.EqualTo(expectedDirection));
        }

        [TestCase("N")]
        [TestCase("North")]
        [TestCase("nOrth")]
        [TestCase("nOrTh")]
        [TestCase("NORTH")]
        [Category("DirectionExtensions Parse Success")]
        public void Test_DirectionExtensions_Parse_MixedCase(string directionText)
        {
            Assert.That(DirectionExtensions.Parse(directionText), Is.EqualTo(Direction.North));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("}{yaeu\\@!//")]
        [TestCase("No")]
        [TestCase("Northuperly")]
        [Category("DirectionExtensions Parse ArgumentException Failure")]
        public void Test_DirectionExtensions_Parse_AndCatchErrorMessage_ForInvalidDirection(string directionText)
        {
            Assert.Multiple(() =>
            {
                // Assert that parsing an invalid direction raises an exception
                var exception = Assert.Throws<ArgumentException>(() =>
                {
                    DirectionExtensions.Parse(directionText);
                });

                // Assert that exception message is as expected
                Assert.That(exception.Message, Does.StartWith("That's not a valid direction"));
            });
        }
    }
}