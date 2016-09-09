using System.Collections.Generic;

namespace JpAnnotator.Common.Portable.Configuration
{
    public interface IOptionProviderChapterMarkers
    {
        IReadOnlyList<string> ChapterMarkers { get; }
    }
}

