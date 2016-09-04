using System.Collections.Generic;
using System.Xml.Linq;

namespace jphtml.Core.Format
{
    public class ContentsInfo
    {
        public List<ContentsMapping> ChapterFiles { get; set; }
    }

    public class ContentsMapping
    {
        public string FilePath { get; set; }
        public int StartLine { get; set; }
        public int LengthInLines { get; set; }
        public string PlainTextContent { get; set; }
        public XElement XhtmlContent { get; set; }
    }
}

