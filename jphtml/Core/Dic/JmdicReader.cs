using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace jphtml.Core.Dic
{
    public class JmdicReader
    {
        readonly XmlDocument _document;
        readonly Jmdictionary _dictionary;

        public JmdicReader(string path)
        {
            Console.WriteLine($"Loading {path}");
            _document = new XmlDocument();
            _document.Load(path);
            Console.WriteLine($"Indexing {path}");
            _dictionary = CreateDictionary();
            Console.WriteLine("done");
        }

        public string Lookup(string kanji)
        {
            return _dictionary.LookupTranslation(kanji);
        }

        Jmdictionary CreateDictionary()
        {
            var index = new Jmdictionary();
            using (var nodes = _document.SelectNodes($"/JMdict/entry/k_ele/keb[text()]"))
            {
                foreach (XmlElement n in nodes)
                {
                    var gloss = n.ParentNode.ParentNode.SelectSingleNode("sense/gloss");
                    if (gloss != null)
                    {
                        index.Append(n.InnerXml, gloss.InnerXml);
                    }
                }
            }
            return index;
        }
    }
}

