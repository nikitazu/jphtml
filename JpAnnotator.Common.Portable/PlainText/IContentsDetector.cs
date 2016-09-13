using System.Collections.Generic;

namespace JpAnnotator.Common.Portable.PlainText
{
    public interface IContentsDetector
    {
        IEnumerable<string> DetectContents(List<string> textLines);
    }
}

