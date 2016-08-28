using System.Collections.Generic;

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
    }
}

