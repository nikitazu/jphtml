using System.Collections.Generic;

namespace jphtml
{
    public class Jmdictionary
    {
        readonly Dictionary<string, List<string>> _dictionary = new Dictionary<string, List<string>>();

        public void Append(string word, string translation)
        {
            List<string> entries;
            if (_dictionary.TryGetValue(word, out entries))
            {
                entries.Add(translation);
            }
            else
            {
                _dictionary.Add(word, new List<string> { translation });
            }
        }

        public string LookupTranslation(string word)
        {
            string result = null;
            List<string> translation;
            if (_dictionary.TryGetValue(word, out translation))
            {
                result = string.Join(",", translation);
            }
            return result;
        }
    }
}

