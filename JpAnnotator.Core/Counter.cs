using System;
using System.Diagnostics;
using JpAnnotator.Logging;

namespace JpAnnotator.Core
{
    public class Counter
    {
        readonly Stopwatch _watch = new Stopwatch();
        readonly ILogWriter _log;

        public Counter(ILogWriter log)
        {
            _log = log;
        }

        public void Start()
        {
            _watch.Start();
            _log.Debug($"Counter start at {DateTime.Now}");
        }

        public void Stop()
        {
            _watch.Stop();
            _log.Debug($"Counter stop at {DateTime.Now}, Elapsed: {_watch.ElapsedMilliseconds} ms");
        }
    }
}

