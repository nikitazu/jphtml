using System;
using System.Collections.Generic;
using System.Linq;

namespace jphtml.Core
{
    public class Options
    {
        public string InputFile { get; }
        public string OutputFile { get; }
        public IReadOnlyList<string> ChapterMarkers { get; }
        public bool Simulation { get; }

        public Options(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                string key = args[i++];
                string value = args[i];

                switch (key)
                {
                    case "--inputFile": InputFile = value; break;
                    case "--outputFile": OutputFile = value; break;
                    case "--chapterMarkers": ChapterMarkers = value.Split(',').ToArray(); break;
                    case "--simulation": Simulation = value == "true"; break;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine($"Input file: {InputFile}");
            Console.WriteLine($"Output file: {OutputFile}");
            Console.WriteLine($"Chapter markers: {string.Join(",", ChapterMarkers)}");
            Console.WriteLine($"Simulation: {Simulation}");
        }
    }
}
