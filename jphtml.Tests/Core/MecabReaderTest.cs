using System.IO;
using JpAnnotator.Core;
using NUnit.Framework;
using FluentAssertions;

namespace JpAnnotator.Tests.Core
{
    [TestFixture(Category = "Mecab")]
    public class MecabReaderTest
    {
        MecabReader _reader;

        [SetUp]
        public void Setup()
        {
            _reader = new MecabReader();
        }

        [Test]
        public void ReadResponseShouldReturnResponseUntioEOS()
        {
            using (var input = new StringReader("abc\nxyz\nEOS\n"))
            {
                _reader.ReadResponse(input).Should().BeEquivalentTo("abc", "xyz");
            }
        }
    }
}

