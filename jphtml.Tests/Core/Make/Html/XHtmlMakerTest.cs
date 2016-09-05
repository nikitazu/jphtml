using System.Collections.Generic;
using System.Xml.Linq;
using JpAnnotator.Core.Make.Html;
using JpAnnotator.Core.Format;
using NUnit.Framework;

namespace JpAnnotator.Tests.Core.Make.Html
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
            _kanjiWithReading = new WordInfo { Text = "見世物", Reading = "ミセモノ", Translation = "Unreal" };
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
            AssertEqualNodes(
                "<p xmlns=\"http://www.w3.org/1999/xhtml\">foo</p>",
                _maker.MakeParagraph(new List<XText> { new XText("foo") })
            );
        }

        [Test]
        public void MakeParagraphWithManyWordsShouldNotLooseSpacebars()
        {
            AssertEqualNodes(
                "<p xmlns=\"http://www.w3.org/1999/xhtml\">foo bar</p>",
                _maker.MakeParagraph(new List<XText> { new XText("foo"), new XText("bar") })
            );
        }

        [Test]
        public void MakeParagraphWithManyWordsShouldNotLooseSpacebarsWhenMixedWithHtml()
        {
            AssertEqualNodes(
                "<p xmlns=\"http://www.w3.org/1999/xhtml\">foo bar<p>yo</p></p>",
                _maker.MakeParagraph(new List<XNode> { new XText("foo"), new XText("bar"), _maker.MakeParagraph(new List<XText> { new XText("yo") }) })
            );
        }

        [Test]
        public void MakeSpanShouldReturnSpan()
        {
            AssertEqualNodes(
                "<span xmlns=\"http://www.w3.org/1999/xhtml\">foo</span>",
                _maker.MakeSpan(new List<XText> { new XText("foo") })
            );
        }

        [Test]
        public void MakeSpanWithManyWordsShouldNotLooseSpacebars()
        {
            AssertEqualNodes(
                "<span xmlns=\"http://www.w3.org/1999/xhtml\">foo bar</span>",
                _maker.MakeSpan(new List<XText> { new XText("foo"), new XText("bar") })
            );
        }

        [Test]
        public void MakeSpanWithManyWordsShouldNotLooseSpacebarsWhenMixedWithHtml()
        {
            AssertEqualNodes(
                "<span xmlns=\"http://www.w3.org/1999/xhtml\">foo bar<span>yo</span></span>",
                _maker.MakeSpan(new List<XNode> { new XText("foo"), new XText("bar"), _maker.MakeSpan(new List<XText> { new XText("yo") }) })
            );
        }

        [Test]
        public void MakeLineBreakShouldBeValid()
        {
            AssertEqualNodes("<br xmlns=\"http://www.w3.org/1999/xhtml\" />", _maker.MakeLineBreak());
        }

        [Test]
        public void MakeRubyShouldReturnRuby()
        {
            AssertEqualNodes(
                "<ruby xmlns=\"http://www.w3.org/1999/xhtml\">foo<rt>bar</rt></ruby>",
                _maker.MakeRuby("foo", "bar")
            );
        }

        [Test]
        public void MakeWordShouldContainFuriganaWhenKanjiAndReading()
        {
            AssertEqualNodes(
                "<ruby xmlns=\"http://www.w3.org/1999/xhtml\">見世物<rt>ミセモノ</rt></ruby>",
                _maker.MakeWord(_kanjiWithReading)
            );
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

        [Test]
        public void MakeContextHelpShouldAddTranslationIfPresent()
        {
            AssertEqualNodes("見世物 [ミセモノ] - Unreal", _maker.MakeContextHelp(_kanjiWithReading));
        }

        [Test]
        public void MakeContextHelpParagraphShouldHasTextWithTranslationOnly()
        {
            AssertEqualNodes(
                "<p class=\"jp-contexthelp\" xmlns=\"http://www.w3.org/1999/xhtml\">見世物 [ミセモノ] - Unreal<br />見世物 [ミセモノ] - Unreal<br /></p>",
                _maker.MakeContextHelpParagraph(new List<WordInfo> { _kanjiWithReading, _kanjiWithReading, _kanjiWithoutReading }));
        }

        void AssertEqualNodes(XNode expected, XNode actual) =>
            Assert.AreEqual(expected.ToString(), actual.ToString());

        void AssertEqualNodes(string expected, XNode actual) =>
            Assert.AreEqual(expected, actual.ToString());
    }
}
