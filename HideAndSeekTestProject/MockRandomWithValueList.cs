using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HideAndSeek
{
    /// <summary>
    /// Mock Random for testing that uses a list to return values
    /// 
    /// CREDIT: copied directly from HideAndSeek project's MockRandomWithValueList class
    ///         © 2023 Andrew Stellman and Jennifer Greene
    ///         Published under the MIT License
    ///         https://github.com/head-first-csharp/fourth-edition/blob/master/Code/Chapter_10/HideAndSeek_part_3/HideAndSeekTests/MockRandomWithValueList.cs
    ///         Link valid as of 02-26-2025
    /// </summary>
    public class MockRandomWithValueList : System.Random
    {
        private Queue<int> valuesToReturn;
        public MockRandomWithValueList(IEnumerable<int> values) =>
        valuesToReturn = new Queue<int>(values);
        public int NextValue()
        {
            var nextValue = valuesToReturn.Dequeue();
            valuesToReturn.Enqueue(nextValue);
            return nextValue;
        }
        public override int Next() => NextValue();
        public override int Next(int maxValue) => Next(0, maxValue);
        public override int Next(int minValue, int maxValue)
        {
            var next = NextValue();
            return next >= minValue && next < maxValue ? next : minValue;
        }
    }
}
