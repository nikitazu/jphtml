using JpAnnotator.Core;
using JpAnnotator.Core.Format;
using NUnit.Framework;
using FluentAssertions;

namespace JpAnnotator.Tests.Core
{
    [TestFixture(Category = "Mecab")]
    public class MecabParserKatakanaNameTest
    {
        const string _input = "ウィキペディア\t名詞,一般,*,*,*,*,*";
        MecabParser _parser;
        WordInfo _result;

        [SetUp]
        public void Setup()
        {
            _parser = new MecabParser();
            _result = _parser.ParseWord(_input);
        }

        [Test]
        public void ParseWordShouldHaveText()
        {
            _result.Text.Should().Be("ウィキペディア");
        }

        [Test]
        public void ParseWordShouldHavePartOfSpeech()
        {
            _result.PartOfSpeech.Should().Be(PartOfSpeech.Noun);
        }

        [Test]
        public void ParseWordShouldHaveSubclass1()
        {
            _result.Subclass1.Should().Be(Subclass1.None);
        }

        [Test]
        public void ParseWordShouldHaveSubclass2()
        {
            _result.Subclass2.Should().Be(Subclass2.None);
        }

        [Test]
        public void ParseWordShouldHaveSubclass3()
        {
            _result.Subclass3.Should().Be(Subclass3.None);
        }

        [Test]
        public void ParseWordShouldHaveInflection()
        {
            _result.Inflection.Should().Be("*");
        }

        [Test]
        public void ParseWordShouldHaveConjugation()
        {
            _result.Conjugation.Should().Be("*");
        }

        [Test]
        public void ParseWordShouldHaveRootForm()
        {
            _result.RootForm.Should().Be("*");
        }

        [Test]
        public void ParseWordShouldHaveReading()
        {
            _result.Reading.Should().BeNull();
        }

        [Test]
        public void ParseWordShouldHavePronunciation()
        {
            _result.Pronunciation.Should().BeNull();
        }
    }
}

