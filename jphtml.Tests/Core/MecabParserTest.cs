using NUnit.Framework;
using System.Text;
using jphtml.Core;
using jphtml.Core.Format;

namespace jphtml.Tests.Core
{
	[TestFixture]
	public class MecabParserTest
	{
		MecabParser _parser;

		[SetUp]
		public void Setup()
		{
			_parser = new MecabParser();
		}

		[Test]
		public void ParseWordForParticle()
		{
			var input = "は\t助詞,係助詞,*,*,*,*,は,ハ,ワ";
			var result = _parser.ParseWord(input);
			Assert.AreEqual("は", result.Text, "Text should be parsed");
			Assert.AreEqual(PartOfSpeech.Particle, result.PartOfSpeech, "Part of speech should be parsed");
			Assert.AreEqual(Subclass1.ParticleBinding, result.Subclass1, "Subclass1 should be parsed");
			Assert.AreEqual(Subclass2.None, result.Subclass2, "Subclass2 should be parsed");
			Assert.AreEqual(Subclass3.None, result.Subclass3, "Subclass3 should be parsed");
			Assert.AreEqual("*", result.Inflection, "Inflection should be parsed");
			Assert.AreEqual("*", result.Conjugation, "Conjugation should be parsed");
			Assert.AreEqual("は", result.RootForm, "RootForm should be parsed");
			Assert.AreEqual("ハ", result.Reading, "Reading should be parsed");
			Assert.AreEqual("ワ", result.Pronunciation, "Pronunciation should be parsed");
		}

		[Test]
		public void ParseWordForKatakanaName()
		{
			var input = "ウィキペディア\t名詞,一般,*,*,*,*,*";
			var result = _parser.ParseWord(input);
			Assert.AreEqual("ウィキペディア", result.Text, "Text should be parsed");
			Assert.AreEqual(PartOfSpeech.Noun, result.PartOfSpeech, "Part of speech should be parsed");
			Assert.AreEqual(Subclass1.None, result.Subclass1, "Subclass1 should be parsed"); // TODO 一般
			Assert.AreEqual(Subclass2.None, result.Subclass2, "Subclass2 should be parsed");
			Assert.AreEqual(Subclass3.None, result.Subclass3, "Subclass3 should be parsed");
			Assert.AreEqual("*", result.Inflection, "Inflection should be parsed");
			Assert.AreEqual("*", result.Conjugation, "Conjugation should be parsed");
			Assert.AreEqual("*", result.RootForm, "RootForm should be parsed");
			Assert.AreEqual(null, result.Reading, "Reading should be parsed");
			Assert.AreEqual(null, result.Pronunciation, "Pronunciation should be parsed");
		}

		[Test]
		public void ParseWordForAuxillaryVerb()
		{
			var input = "です\t助動詞,*,*,*,特殊・デス,基本形,です,デス,デス";
			var result = _parser.ParseWord(input);
			Assert.AreEqual("です", result.Text, "Text should be parsed");
			Assert.AreEqual(PartOfSpeech.AuxillaryVerb, result.PartOfSpeech, "Part of speech should be parsed");
			Assert.AreEqual(Subclass1.None, result.Subclass1, "Subclass1 should be parsed");
			Assert.AreEqual(Subclass2.None, result.Subclass2, "Subclass2 should be parsed");
			Assert.AreEqual(Subclass3.None, result.Subclass3, "Subclass3 should be parsed");
			Assert.AreEqual("特殊・デス", result.Inflection, "Inflection should be parsed");
			Assert.AreEqual("基本形", result.Conjugation, "Conjugation should be parsed");
			Assert.AreEqual("です", result.RootForm, "RootForm should be parsed");
			Assert.AreEqual("デス", result.Reading, "Reading should be parsed");
			Assert.AreEqual("デス", result.Pronunciation, "Pronunciation should be parsed");
		}
	}
}

