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
		public void ParseResponseShouldReturnWordInfo()
		{
			var input = new StringBuilder("は\t助詞,係助詞,*,*,*,*,は,ハ,ワ");
			var result = _parser.ParseResponse(input);
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
	}
}

