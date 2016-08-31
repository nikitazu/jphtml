using System.Collections.Generic;
using System.Xml.Linq;
using jphtml.Core.Format;
using jphtml.Core.Html;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Xml;

namespace jphtml.Tests.Core.Html
{
    [TestFixture]
    public class XHtmlMakerTest
    {
        XHtmlMaker _maker;
        WordInfo _kanjiWithReading;
        WordInfo _kanjiWithoutReading;
        WordInfo _katakanaWord;
        WordInfo _hiraganaWord;
        WordInfo _hiraganaWordWithDifferentPronunciation;

        [SetUp]
        public void Setup()
        {
            _maker = new XHtmlMaker();
            _kanjiWithReading = new WordInfo { Text = "見世物", Reading = "ミセモノ" };
            _kanjiWithoutReading = new WordInfo { Text = "見世物" };
            _hiraganaWord = new WordInfo { Text = "すべて", Reading = "スベテ" };
            _katakanaWord = new WordInfo { Text = "スベテ", Reading = "スベテ" };
            _hiraganaWordWithDifferentPronunciation = new WordInfo { Text = "は", Reading = "ハ", Pronunciation = "ワ" };
        }

        [Test]
        public void MakeDocumentShouldReturnXHtml()
        {
            var expected = @"<html xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:m=""http://www.w3.org/1998/Math/MathML"" xmlns:epub=""http://www.ipdf.org/2007/ops"" xml:lang=""ru"" xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <meta http-equiv=""Content-Type"" content=""application/xhtml+xml; charset=utf-8"" />
    <link href=""style.css"" rel=""stylesheet"" type=""text/css"" />
    <title>JpHtml</title>
  </head>
  <body>foo</body>
</html>";
            var actual = _maker.MakeRootNode(new List<XText> { new XText("foo") });
            AssertEqualNodes(expected, actual);
        }

        [Test]
        public void MetaShouldBeValid()
        {
            AssertEqualNodes(
                "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" xmlns=\"http://www.w3.org/1999/xhtml\" />",
                _maker.MakeMeta()
            );
        }

        [Test]
        public void MakeCssLinkShouldReturnLink()
        {
            AssertEqualNodes(
                "<link href=\"style.css\" rel=\"stylesheet\" type=\"text/css\" xmlns=\"http://www.w3.org/1999/xhtml\" />",
                _maker.MakeCssLink("style.css")
            );
        }

        [Test]
        public void MakeTitleShouldReturnTitle()
        {
            AssertEqualNodes("<title xmlns=\"http://www.w3.org/1999/xhtml\">foo</title>", _maker.MakeTitle("foo"));
        }

        [Test]
        public void MakeParagraphShouldReturnParagraph()
        {
            AssertEqualNodes(new XElement("p", "foo"), _maker.MakeParagraph(new List<XText> { new XText("foo") }));
        }

        [Test]
        public void MakeWordShouldContainFuriganaWhenKanjiAndReading()
        {
            var expected = new XElement(
                "ruby",
                "見世物",
                new XElement("rt", "ミセモノ"));

            AssertEqualNodes(expected, _maker.MakeWord(_kanjiWithReading));
        }

        [Test]
        public void MakeWordShouldNotContainFuriganaWhenOnlyKanji()
        {
            AssertEqualNodes("見世物", _maker.MakeWord(_kanjiWithoutReading));
        }

        [Test]
        public void MakeWordShouldNotContainFuriganaWhenTextIsKatakana()
        {
            AssertEqualNodes("スベテ", _maker.MakeWord(_katakanaWord));
        }

        [Test]
        public void MakeWordShouldNotContainFuriganaWhenTextIsHiragana()
        {
            AssertEqualNodes("すべて", _maker.MakeWord(_hiraganaWord));
        }

        [Test]
        public void MakeWordShouldNotContainFuriganaWhenTextIsHiraganaWithDifferentPronunciation()
        {
            AssertEqualNodes("は", _maker.MakeWord(_hiraganaWordWithDifferentPronunciation));
        }

        void AssertEqualNodes(XNode expected, XNode actual) =>
            Assert.AreEqual(expected.ToString(), actual.ToString());

        void AssertEqualNodes(string expected, XNode actual) =>
            Assert.AreEqual(expected, actual.ToString());
    }
}
