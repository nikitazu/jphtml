using System.Collections.Generic;
using NUnit.Framework;
using jphtml.Utils;

namespace jphtml.Tests.Utils
{
    [TestFixture]
    public class EnumerableUtilsTest
    {
        class Point
        {
            public int X, Y;

            public override string ToString() => $"[{X},{Y}]";
        }

        List<Point> _items;

        [SetUp]
        public void Setup()
        {
            _items = new List<Point> {
                new Point { X = 1, Y = 10 },
                new Point { X = 2, Y = 20 },
                new Point { X = 2, Y = 30 }
            };
        }

        [Test]
        public void DistinctByShouldProvideDistinctValues()
        {
            Assert.AreEqual("[1,10],[2,20]", string.Join(",", _items.DistinctBy(p => p.X)));
        }
    }
}

