using System;
using System.Collections.Generic;
using System.Linq;

namespace JpAnnotator.Common.Portable.PlainText
{
    public class SentenceBreaker
    {
        const char _endOfSentence = '。';

        public IEnumerable<string> BreakToSentences(string input)
        {
            return input.Split(new char[] { _endOfSentence }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s + _endOfSentence);
        }
    }
}

