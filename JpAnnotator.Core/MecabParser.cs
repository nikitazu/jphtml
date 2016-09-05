using System.Text;
using JpAnnotator.Core.Format;

namespace JpAnnotator.Core
{
	public class MecabParser
	{
		public WordInfo ParseWord(string input)
		{
			var data = input.Split(new char[] { '\t' });
			var surface = data[0];
			var feature = data[1].Split(new char[] { ',' });
			return new WordInfo
			{
				Text = surface,
				PartOfSpeech = ParsePartOfSpeech(feature[0]),
				Subclass1 = ParseSubclass1(feature[1]),
				Subclass2 = ParseSubclass2(feature[2]),
				Subclass3 = ParseSubclass3(feature[3]),
				Inflection = feature[4],
				Conjugation = feature[5],
				RootForm = feature[6],
				Reading = feature.Length > 7 ? feature[7] : null,
				Pronunciation = feature.Length > 8 ? feature[8] : null
			};
		}

		PartOfSpeech ParsePartOfSpeech(string input)
		{
			switch (input)
			{
				case "助詞": return PartOfSpeech.Particle;
				case "名詞": return PartOfSpeech.Noun;
				case "助動詞": return PartOfSpeech.AuxillaryVerb;
				default: return PartOfSpeech.Unknown;
			}
		}

		Subclass1 ParseSubclass1(string input)
		{
			switch (input)
			{
				case "係助詞": return Subclass1.ParticleBinding;
				default: return Subclass1.None;
			}
		}

		Subclass2 ParseSubclass2(string input)
		{
			switch (input)
			{
				default: return Subclass2.None;
			}
		}

		Subclass3 ParseSubclass3(string input)
		{
			switch (input)
			{
				default: return Subclass3.None;
			}
		}
	}
}

