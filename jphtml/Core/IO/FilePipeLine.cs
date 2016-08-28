using System;
using System.IO;
using System.Text;

namespace jphtml.Core.IO
{
    public class FilePipeLine
    {
        readonly string _inputFile;
        readonly string _outputFile;

        public FilePipeLine(string inputFile, string outputFile)
        {
            _inputFile = inputFile;
            _outputFile = outputFile;
        }

        public void Run(Action<StreamReader, StreamWriter> processData)
        {
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

