using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace jphtml
{
    public class JmdicFastReader
    {
        readonly string _path;
        readonly Jmdictionary _dictionary = new Jmdictionary();

        public JmdicFastReader(string path)
        {
            _path = path;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Indexing {path}");
            using (var stream = new StreamReader(_path))
            using (var reader = new XmlTextReader(stream)
            {
                WhitespaceHandling = WhitespaceHandling.None,
                Namespaces = false
            })
            {
                string result = null;
                if (reader.ReadToDescendant("entry"))
                {
                    while (result == null && reader.ReadToNextSibling("entry"))
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            string word = null;

                            using (var subr = reader.ReadSubtree())
                            {
                                while (subr.Read())
                                {
                                    if (subr.Name == "keb" && subr.NodeType == XmlNodeType.Element)
                                    {
                                        word = subr.ReadElementContentAsString();
                                    }
                                    else if (word != null && subr.Name == "gloss" && subr.NodeType == XmlNodeType.Element)
                                    {
                                        _dictionary.Append(word, subr.ReadElementContentAsString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"done in {sw.ElapsedMilliseconds}ms");
        }

        public string Lookup(string kanji)
        {
            return _dictionary.LookupTranslation(kanji);
        }
    }
}

