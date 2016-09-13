using System.Collections.Generic;
using FluentAssertions;
using JpAnnotator.Common.Portable.PlainText;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.PlainText
{
    [TestFixture]
    public class ContentsDetectorTest
    {
        List<string> _textLines = new List<string>()
        {
            "ここは見世物の世界",
            "第1章\u3000青豆\u3000見かけにだまされないように\t11",
            "第24章\u3000天吾\u3000ここではない世界であることの意味はどこにあるのだろう\t532",
            "タクシーのラジオは、FM放送のクラシック音楽番組を流していた。",
            "第1章\u3000青豆",
            "「トヨタのクラウン?ロイヤルサルーン」と運転手は簡潔に答えた。",
            "第24章\u3000天吾",
            "「音楽がきれいに聞こえる」",
        };
        IContentsDetector _detector;

        [SetUp]
        public void Setup()
        {
            _detector = new ContentsDetector();
        }

        [Test]
        public void DetectContentsShouldReturnListOfChapterMarkers()
        {
            _detector.DetectContents(_textLines).Should().BeEquivalentTo("第1章", "第24章");
        }
    }
}

