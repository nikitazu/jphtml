using System.Collections.Generic;
using System.IO;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core;
using JpAnnotator.Core.Format;
using NUnit.Framework;
using JpAnnotator.Common.Portable.PlainText;
using FluentAssertions;
using System;

namespace JpAnnotator.Tests.Core
{
    [TestFixture]
    public class ContentsBreakerTest
    {
        class TestChapterMarkersProvider : IOptionProviderChapterMarkers
        {
            public IReadOnlyList<string> ChapterMarkers => new string[] { "第1章", "第2章", "第3章" };
        }

        const string _text = "Heading\n第1章 a\n第2章 b\n第3章 c\n\n第1章 aa\nfoo\n\n第2章 bb\nbar\n\n第3章 cc\ncux";
        ContentsBreaker _breaker;
        ContentsInfo _contents;

        [SetUp]
        public void Setup()
        {
            _breaker = new ContentsBreaker(new TestChapterMarkersProvider());
            using (var reader = new MarkingTextReader(new StringReader(_text)))
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
            Assert.AreEqual("ch0", _contents.ChapterFiles[0].Name, "FilePath");
            Assert.AreEqual(0, _contents.ChapterFiles[0].StartLine, "StartLine");
            Assert.AreEqual(5, _contents.ChapterFiles[0].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter1()
        {
            Assert.AreEqual("ch1", _contents.ChapterFiles[1].Name, "FilePath");
            Assert.AreEqual(5, _contents.ChapterFiles[1].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[1].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter2()
        {
            Assert.AreEqual("ch2", _contents.ChapterFiles[2].Name, "FilePath");
            Assert.AreEqual(8, _contents.ChapterFiles[2].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[2].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldDetectChapter3()
        {
            Assert.AreEqual("ch3", _contents.ChapterFiles[3].Name, "FilePath");
            Assert.AreEqual(11, _contents.ChapterFiles[3].StartLine, "StartLine");
            Assert.AreEqual(3, _contents.ChapterFiles[3].LengthInLines, "LengthInLines");
        }

        [Test]
        public void AnalyzeShouldAssignPlainTextContent()
        {
            Assert.AreEqual("Heading\n第1章 a\n第2章 b\n第3章 c\n", string.Join("\n", _contents.ChapterFiles[0].PlainTextContent), "ch0");
            Assert.AreEqual("第1章 aa\nfoo\n", string.Join("\n", _contents.ChapterFiles[1].PlainTextContent), "ch1");
            Assert.AreEqual("第2章 bb\nbar\n", string.Join("\n", _contents.ChapterFiles[2].PlainTextContent), "ch2");
            Assert.AreEqual("第3章 cc\ncux", string.Join("\n", _contents.ChapterFiles[3].PlainTextContent), "ch3");
        }

        [Test]
        public void AnalyzeShouldDetectSingleChapter0WhenChapterMarkersAreEmpty()
        {
            ContentsInfo zeroContents;
            var zeroBreaker = new ContentsBreaker(new Options(new string[] { }));
            using (var reader = new MarkingTextReader(new StringReader(_text)))
            {
                zeroContents = zeroBreaker.Analyze(reader);
            }

            Assert.AreEqual(1, zeroContents.ChapterFiles.Count, "ChapterFiles Count");
            Assert.AreEqual("ch0", zeroContents.ChapterFiles[0].Name, "FilePath");
            Assert.AreEqual(0, zeroContents.ChapterFiles[0].StartLine, "StartLine");
            Assert.AreEqual(13, zeroContents.ChapterFiles[0].LengthInLines, "LengthInLines");

            zeroContents.ChapterFiles[0].PlainTextContent.Should().BeEquivalentTo(_text.Split('\n'));
        }
    }
}

