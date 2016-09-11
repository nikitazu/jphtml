using JpAnnotator.Core;
using JpAnnotator.Core.Format;
using NUnit.Framework;
using FluentAssertions;

namespace JpAnnotator.Tests.Core
{
    [TestFixture(Category = "Mecab")]
    public class MecabParserAuxillaryVerbTest
    {
        const string _input = "です\t助動詞,*,*,*,特殊・デス,基本形,です,デス,デス";
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
            _result.Text.Should().Be("です");
        }

        [Test]
        public void ParseWordShouldHavePartOfSpeech()
        {
            _result.PartOfSpeech.Should().Be(PartOfSpeech.AuxillaryVerb);
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
            _result.Inflection.Should().Be("特殊・デス");
        }

        [Test]
        public void ParseWordShouldHaveConjugation()
        {
            _result.Conjugation.Should().Be("基本形");
        }

        [Test]
        public void ParseWordShouldHaveRootForm()
        {
            _result.RootForm.Should().Be("です");
        }

        [Test]
        public void ParseWordShouldHaveReading()
        {
            _result.Reading.Should().Be("デス");
        }

        [Test]
        public void ParseWordShouldHavePronunciation()
        {
            _result.Pronunciation.Should().Be("デス");
        }
    }
}

