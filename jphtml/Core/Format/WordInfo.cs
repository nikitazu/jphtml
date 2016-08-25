namespace jphtml.Core.Format
{
	public class WordInfo
	{
		public string Text { get; set; }
		public PartOfSpeech PartOfSpeech { get; set; }
		public Subclass1 Subclass1 { get; set; }
		public Subclass2 Subclass2 { get; set; }
		public Subclass3 Subclass3 { get; set; }
		public string Inflection { get; set; }
		public string Conjugation { get; set; }
		public string RootForm { get; set; }
		public string Reading { get; set; }
		public string Pronunciation { get; set; }
	}

	public enum PartOfSpeech
	{
		Unknown,
		Particle
	}

	public enum Subclass1
	{
		None,

		ParticleBinding
	}

	public enum Subclass2
	{
		None
	}

	public enum Subclass3
	{
		None
	}


}

