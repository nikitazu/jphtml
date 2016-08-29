using System;
using System.IO;
using System.Text;
using jphtml.Logging;

namespace jphtml.Core.IO
{
    public class FilePipeLine
    {
        readonly ILogWriter _log;
        readonly string _inputFile;
        readonly string _outputFile;

        public FilePipeLine(ILogWriter log, string inputFile, string outputFile)
        {
            _log = log;
            _inputFile = inputFile;
            _outputFile = outputFile;
        }

        public void Run(Action<StreamReader, StreamWriter> processData)
        {
            _log.Debug($"Running file pipeline: {_inputFile} -> {_outputFile}");
            using (var reader = new StreamReader(_inputFile))
            using (var writer = new StreamWriter(_outputFile, false, Encoding.UTF8))
            {
                do
                {
                    processData(reader, writer);
                }
                while (!reader.EndOfStream);
            }
        }
    }
}

