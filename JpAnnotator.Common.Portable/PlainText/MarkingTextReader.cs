using System;
using System.Collections.Generic;
using System.IO;

namespace JpAnnotator.Common.Portable.PlainText
{
    public class MarkingTextReader : IDisposable
    {
        readonly TextReader _reader;
        public List<string> Lines { get; } = new List<string>();
        int _position;

        public MarkingTextReader(TextReader reader)
        {
            _reader = reader;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Lines.Add(line);
            }
        }

        public int CountLinesUntilMarker(string marker, bool skipFirstMarker)
        {
            bool done = false;
            bool hasSkipped = !skipFirstMarker;
            int lastPosition = _position;
            while (!done)
            {
                if (!hasSkipped && _position < Lines.Count && isLineMarked(marker, Lines[_position]))
                {
                    hasSkipped = true;
                    _position++;
                    continue;
                }
                done = _position >= Lines.Count || hasSkipped && isLineMarked(marker, Lines[_position]);
                _position++;
            }
            if (marker != null && _position < Lines.Count)
            {
                _position--;
            }
            return _position - lastPosition;
        }

        public int CountLinesUntilEnd()
        {
            return CountLinesUntilMarker(null, false) - 1;
        }

        bool isLineMarked(string marker, string line) =>
            marker != null && line != null && line.StartsWith(marker, StringComparison.Ordinal);

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
