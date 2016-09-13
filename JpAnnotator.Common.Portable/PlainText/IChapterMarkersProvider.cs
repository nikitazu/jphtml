using System.Collections.Generic;

namespace JpAnnotator.Common.Portable.PlainText
{
    public interface IChapterMarkersProvider
    {
        IEnumerable<string> ProvideChapterMarkers(List<string> textLines);
    }
}

