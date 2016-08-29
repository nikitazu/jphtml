using System;
using System.Xml;
using System.Diagnostics;
using jphtml.Logging;

namespace jphtml.Core.Dic
{
    public class JmdicReader
    {
        readonly ILogWriter _log;
        readonly XmlDocument _document;
        readonly IMultiDictionary _dictionary;

        public JmdicReader(ILogWriter log, string path, IMultiDictionary dictionary)
        {
            _log = log;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            _log.Debug($"Loading {path}");
            _dictionary = dictionary;
            _document = new XmlDocument();
            _document.Load(path);
            _log.Debug($"Indexing {path}");
            CreateDictionary();
            sw.Stop();
            _log.Debug($"done in {sw.ElapsedMilliseconds}ms");
        }

        public string Lookup(string kanji)
        {
            return _dictionary.LookupTranslation(kanji);
        }

        void CreateDictionary()
        {
            using (var nodes = _document.SelectNodes($"/JMdict/entry/k_ele/keb[text()]"))
            {
                foreach (XmlElement n in nodes)
                {
                    var gloss = n.ParentNode.ParentNode.SelectSingleNode("sense/gloss");
                    if (gloss != null)
                    {
                        _dictionary.Append(n.InnerXml, gloss.InnerXml);
                    }
                }
            }
        }
    }
}

