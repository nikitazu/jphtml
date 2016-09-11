﻿using System.Collections.Generic;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Core.Format;
using JpAnnotator.Common.Portable.PlainText;

namespace JpAnnotator.Core
{
    public class ContentsBreaker
    {
        readonly IReadOnlyList<string> _chapterMarkers;

        public ContentsBreaker(IOptionProviderChapterMarkers options)
        {
            _chapterMarkers = options.ChapterMarkers;
        }

        public ContentsInfo Analyze(MarkingTextReader reader)
        {
            var contents = new ContentsInfo
            {
                ChapterFiles = new List<ContentsMapping>(_chapterMarkers.Count + 1)
            };

            int startLine = 0;
            int chapterIndex = 0;
            for (chapterIndex = 0; chapterIndex < _chapterMarkers.Count; chapterIndex++)
            {
                var count = reader.CountLinesUntilMarker(_chapterMarkers[chapterIndex], chapterIndex == 0);
                contents.ChapterFiles.Add(new ContentsMapping()
                {
                    Name = $"ch{chapterIndex}",
                    StartLine = startLine,
                    LengthInLines = count,
                    PlainTextContent = new List<string>(count),
                });
                Copy(reader.Lines, contents.ChapterFiles[chapterIndex]);
                startLine += count;
            }

            contents.ChapterFiles.Add(new ContentsMapping()
            {
                Name = $"ch{chapterIndex}",
                StartLine = startLine,
                LengthInLines = reader.CountLinesUntilEnd() + (startLine > 0 ? 1 : 0),
                PlainTextContent = new List<string>(),
            });
            Copy(reader.Lines, contents.ChapterFiles[chapterIndex]);

            return contents;
        }

        void Copy(List<string> source, ContentsMapping target)
        {
            for (int i = 0; i < target.LengthInLines && target.StartLine + i < source.Count; i++)
            {
                target.PlainTextContent.Add(source[target.StartLine + i]);
            }
        }
    }
}
