using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace jphtml.Core.Dic
{
    public class JmdicFastReader
    {
        readonly Options _options;
        readonly string _path;
        readonly IMultiDictionary _dictionary;

        public JmdicFastReader(Options options, string path, IMultiDictionary dictionary)
        {
            _options = options;
            _path = path;
            _dictionary = dictionary;


            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine($"Indexing {path}");

            if (!_options.Simulation)
            {
                ReadDictionary();
            }

            sw.Stop();
            Console.WriteLine($"done in {sw.ElapsedMilliseconds}ms");
        }

        void ReadDictionary()
        {
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
        }

        public string Lookup(string kanji)
        {
            return _dictionary.LookupTranslation(kanji);
        }
    }
}

