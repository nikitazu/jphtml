using System.IO;
using NUnit.Framework;
using jphtml.Core;
using jphtml.Core.Format;

namespace jphtml.Tests
{
    [TestFixture]
    public class ContentsBreakerTest
    {
        const string _text = @"Heading
x1
x2
x3

x1
foo

x2
bar

x3
cux
";
        ContentsBreaker _breaker;
        ContentsInfo _contents;

        [SetUp]
        public void Setup()
        {
            _breaker = new ContentsBreaker(new string[] { "x1", "x2", "x3" });
            using (var reader = new StringReader(_text))
            {
                _contents = _breaker.Analyze(reader);
            }
        }

        [Test]
        public void AnalyzeShouldCountChapters()
        {
            Assert.AreEqual(4, _contents.ChapterFiles.Count);
        }


        [Test]
        public void AnalyzeShouldDetectChapter0()
        {
            Assert.AreEqual("tmp/ch0", _contents.ChapterFiles[0].FilePath, "FilePath");
            Assert.AreEqual(0, _contents.ChapterFiles[0].StartLine, "StartLine");
            Assert.AreEqual(5, _contents.ChapterFiles[0].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter1()
        {
            Assert.AreEqual("tmp/ch1", _contents.ChapterFiles[1].FilePath, "FilePath");
            Assert.AreEqual(5, _contents.ChapterFiles[1].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[1].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter2()
        {
            Assert.AreEqual("tmp/ch2", _contents.ChapterFiles[2].FilePath, "FilePath");
            Assert.AreEqual(8, _contents.ChapterFiles[2].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[2].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter3()
        {
            Assert.AreEqual("tmp/ch3", _contents.ChapterFiles[3].FilePath, "FilePath");
            Assert.AreEqual(11, _contents.ChapterFiles[3].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[3].LengthInLines, "LengthInLines");
        }
    }
}

