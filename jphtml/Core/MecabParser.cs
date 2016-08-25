using System.Text;
using jphtml.Core.Format;

namespace jphtml.Core
{
	public class MecabParser
	{
		public WordInfo ParseResponse(StringBuilder input)
		{
			var data = input.ToString().Split(new char[] { '\t' });
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
				Reading = feature[7],
				Pronunciation = feature[8]
			};
		}

		PartOfSpeech ParsePartOfSpeech(string input)
		{
			switch (input)
			{
				case "助詞": return PartOfSpeech.Particle;
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

