using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JpAnnotator.Common.Portable.PlainText
{
    public class ContentsDetector : IContentsDetector
    {
        readonly Regex _japaneseChapterRegex = new Regex(@"^(第\d+章)", RegexOptions.CultureInvariant);

        IEnumerable<string> IContentsDetector.DetectContents(List<string> textLines)
        {
            var markers = new List<string>();
            foreach (var line in textLines)
            {
                var match = _japaneseChapterRegex.Match(line);
                if (match.Success)
                {
                    markers.Add(match.Groups[1].Value);
                }
            }
            return markers.Distinct();
        }
    }
}

