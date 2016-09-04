using System;
using System.Collections.Generic;
using System.Linq;

namespace jphtml.Core
{
    public class Options
    {
        public string InputFile { get; } = string.Empty;
        public string OutputDir { get; } = string.Empty;
        public IReadOnlyList<string> ChapterMarkers { get; } = new List<string>();
        public bool Simulation { get; } = false;

        public Options(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                string key = args[i++];
                string value = args[i];

                switch (key)
                {
                    case "--inputFile": InputFile = value; break;
                    case "--outputDir": OutputDir = value; break;
                    case "--chapterMarkers": ChapterMarkers = value.Split(',').ToArray(); break;
                    case "--simulation": Simulation = value == "true"; break;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine($"Input file: {InputFile}");
            Console.WriteLine($"Output dir: {OutputDir}");
            Console.WriteLine($"Chapter markers: {string.Join(",", ChapterMarkers)}");
            Console.WriteLine($"Simulation: {Simulation}");
        }
    }
}
