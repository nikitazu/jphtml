using System;
using System.Collections.Generic;
using System.IO;

namespace jphtml.Core
{
    public class MecabReader
    {
        public IList<string> ReadResponse(TextReader reader)
        {
            var result = new List<string>();
            string line;
            while (!"EOS".Equals(line = reader.ReadLine(), StringComparison.InvariantCulture))
            {
                result.Add(line);
            }
            return result;
        }
    }
}

