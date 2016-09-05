using System;
using System.Collections.Generic;

namespace JpAnnotator.Core.Format
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
        public string Translation { get; set; }

        public string Furigana => string.IsNullOrWhiteSpace(Pronunciation) ? Reading : Pronunciation;

        public string TextMaybeRootForm => string.IsNullOrWhiteSpace(RootForm) || Text == RootForm ? Text : $"{Text} ({RootForm})";

        public IEnumerable<Enum> SpeechInfo
        {
            get
            {
                yield return PartOfSpeech;
                yield return Subclass1;
                yield return Subclass2;
                yield return Subclass3;
            }
        }
    }

    public enum PartOfSpeech
    {
        Unknown,
        Particle,
        Noun,
        AuxillaryVerb
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

