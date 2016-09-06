using NUnit.Framework;
using JpAnnotator.Core;

namespace JpAnnotator.Tests.Core
{
    [TestFixture]
    public class OptionsTest
    {
        Options _options;

        [SetUp]
        public void Setup()
        {
            _options = new Options(new string[] {
                "--inputFile", "path/to/in",
                "--outputFile", "path/to/out",
                "--chapterMarkers", "a,b,c",
                "--author", "murakami",
                "--bookId", "666",
                "--publisher", "ZStudios"
            });
        }

        [Test]
        public void InputFileShouldParse()
        {
            Assert.AreEqual("path/to/in", _options.InputFile);
        }

        [Test]
        public void OutputFileShouldParse()
        {
            Assert.AreEqual("path/to/out", _options.OutputFile);
        }

        [Test]
        public void ChapterMarkersShouldParse()
        {
            Assert.AreEqual(3, _options.ChapterMarkers.Count);
            Assert.AreEqual("a", _options.ChapterMarkers[0]);
            Assert.AreEqual("b", _options.ChapterMarkers[1]);
            Assert.AreEqual("c", _options.ChapterMarkers[2]);
        }

        [Test]
        public void AuthorShouldParse()
        {
            Assert.AreEqual("murakami", _options.Author);
        }

        [Test]
        public void BookIdShouldParse()
        {
            Assert.AreEqual("666", _options.BookId);
        }

        [Test]
        public void PublisherShouldParse()
        {
            Assert.AreEqual("ZStudios", _options.Publisher);
        }
    }
}

