using System.IO;
using NUnit.Framework;
using jphtml.Core;
using jphtml.Core.Format;

namespace jphtml.Tests.Core
{
    [TestFixture]
    public class ContentsBreakerTest
    {
        const string _text = "Heading\n第1章 a\n第2章 b\n第3章 c\n\n第1章 aa\nfoo\n\n第2章 bb\nbar\n\n第3章 cc\ncux\n";
        ContentsBreaker _breaker;
        ContentsInfo _contents;

        [SetUp]
        public void Setup()
        {
            _breaker = new ContentsBreaker("tmp", new string[] { "第1章", "第2章", "第3章" });
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

        [Test]
        public void BreakInMemoryShouldAssignPlainTextContent()
        {
            File.WriteAllText("tmp_input.txt", _text);
            _breaker.BreakInMemory("tmp_input.txt", _contents);
            Assert.AreEqual("Heading\n第1章 a\n第2章 b\n第3章 c\n", string.Join("\n", _contents.ChapterFiles[0].PlainTextContent));
            Assert.AreEqual("第1章 aa\nfoo\n", string.Join("\n", _contents.ChapterFiles[1].PlainTextContent));
            Assert.AreEqual("第2章 bb\nbar\n", string.Join("\n", _contents.ChapterFiles[2].PlainTextContent));
            Assert.AreEqual("第3章 cc\ncux", string.Join("\n", _contents.ChapterFiles[3].PlainTextContent));
        }

        [Test]
        public void AnalyzeShouldDetectSingleChapter0WhenChapterMarkersAreEmpty()
        {
            ContentsInfo zeroContents;
            var zeroBreaker = new ContentsBreaker("tmp", null);
            using (var reader = new StringReader(_text))
            {
                zeroContents = zeroBreaker.Analyze(reader);
            }

            Assert.AreEqual(1, zeroContents.ChapterFiles.Count, "ChapterFiles Count");
            Assert.AreEqual("tmp/ch0", zeroContents.ChapterFiles[0].FilePath, "FilePath");
            Assert.AreEqual(0, zeroContents.ChapterFiles[0].StartLine, "StartLine");
            Assert.AreEqual(14, zeroContents.ChapterFiles[0].LengthInLines, "LengthInLines");
        }
    }
}

