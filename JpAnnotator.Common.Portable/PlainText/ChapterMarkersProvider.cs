using System.Collections.Generic;
using JpAnnotator.Common.Portable.Configuration;

namespace JpAnnotator.Common.Portable.PlainText
{
    public class ChapterMarkersProvider : IChapterMarkersProvider
    {
        readonly IOptionProviderChapterMarkers _options;
        readonly IContentsDetector _detector;

        public ChapterMarkersProvider(
            IOptionProviderChapterMarkers options,
            IContentsDetector detector)
        {
            _options = options;
            _detector = detector;
        }

        IEnumerable<string> IChapterMarkersProvider.ProvideChapterMarkers(List<string> textLines)
        {
            return _options.ChapterMarkers.Count > 0 ? _options.ChapterMarkers : _detector.DetectContents(textLines);
        }
    }
}
