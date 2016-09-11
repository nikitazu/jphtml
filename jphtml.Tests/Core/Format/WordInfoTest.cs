using JpAnnotator.Core.Format;
using NUnit.Framework;

namespace JpAnnotator.Tests.Core.Format
{
    [TestFixture]
    public class WordInfoTest
    {
        [Test]
        public void WordFuriganaShouldContainReadingWhenOnlyReading()
        {
            Assert.AreEqual("foo", new WordInfo { Reading = "foo" }.Furigana);
        }

        [Test]
        public void WordFuriganaShouldContainPronunciationWhenOnlyPronunciation()
        {
            Assert.AreEqual("foo", new WordInfo { Pronunciation = "foo" }.Furigana);
        }

        [Test]
        public void WordFuriganaShouldContainPronunciationWhenReadingAndPronunciation()
        {
            Assert.AreEqual("foo", new WordInfo { Pronunciation = "foo", Reading = "bar" }.Furigana);
        }

        [Test]
        public void WordTextMaybeRootFormShouldContainRootFormWhenItIsPresent()
        {
            Assert.AreEqual("text (root)", new WordInfo { Text = "text", RootForm = "root" }.TextMaybeRootForm);
        }

        [Test]
        public void WordTextMaybeRootFormShouldContainTextOnlyWhenRootFormIsAbsent()
        {
            Assert.AreEqual("text", new WordInfo { Text = "text" }.TextMaybeRootForm);
        }

        [Test]
        public void WordTextMaybeRootFormShouldContainTextOnlyWhenTextIsInRootForm()
        {
            Assert.AreEqual("text", new WordInfo { Text = "text", RootForm = "text" }.TextMaybeRootForm);
        }

        [Test]
        public void WordSpeechInfoByDefaultShouldConcatenatePartOfSpeechWithSubclasses()
        {
            Assert.AreEqual("Unknown,None,None,None", string.Join(",", new WordInfo().SpeechInfo));
        }
    }
}

