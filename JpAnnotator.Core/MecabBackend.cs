using System.IO;
using NMeCab;

namespace JpAnnotator.Core
{
    public class MecabBackend
    {
        readonly MeCabTagger _tagger = MeCabTagger.Create();

        public TextReader ParseText(string text) => new StringReader(_tagger.Parse(text));
    }
}

