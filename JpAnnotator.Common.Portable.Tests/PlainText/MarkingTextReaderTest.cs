using System.IO;
using FluentAssertions;
using JpAnnotator.Common.Portable.PlainText;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests
{
    [TestFixture]
    public class MarkingTextReaderTest
    {
        MarkingTextReader _reader;

        [SetUp]
        public void Setup()
        {
            _reader = new MarkingTextReader(new StringReader("line1\nx\ny\nline2\nline3\nx\nline4\nline5\ny\nline6\n"));
        }

        [TearDown]
        public void TearDown()
        {
            _reader.Dispose();
            _reader = null;
        }

        [Test]
        public void MarkingReaderLinesShouldBeAllRead()
        {
            _reader.Lines.Should().BeEquivalentTo("line1", "x", "y", "line2", "line3", "x", "line4", "line5", "y", "line6");
        }

        [Test]
        public void MarkingReaderCountLinesUntilMarkerShouldCountLinesWhenNotSkipFirstMarker()
        {
            _reader.CountLinesUntilMarker("x", false).Should().Be(1);
        }

        [Test]
        public void MarkingReaderCountLinesUntilMarkerShouldCountLinesWhenSkipFirstMarker()
        {
            _reader.CountLinesUntilMarker("x", true).Should().Be(5);
        }

        [Test]
        public void MarkingReaderCountLinesUntilEndShouldCountAllLines()
        {
            _reader.CountLinesUntilEnd().Should().Be(10);
        }

        [Test]
        public void MarkingReaderCountLinesUntilMarkerShouldCountLinesWhenNotSkipFirstMarkerAfterFirstCount()
        {
            _reader.CountLinesUntilMarker("x", true);
            _reader.CountLinesUntilMarker("y", false).Should().Be(3);
        }

        [Test]
        public void MarkingReaderCountinesUntilEndShouldCountAllLinesAfterFirstCount()
        {
            _reader.CountLinesUntilMarker("x", false);
            _reader.CountLinesUntilEnd().Should().Be(9);
        }
    }
}
