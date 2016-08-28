namespace jphtml.Core
{
    public class Options
    {
        public string InputFile { get; }
        public string OutputFile { get; }

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
                }
            }
        }
    }
}
