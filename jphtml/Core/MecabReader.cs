using System;
using System.IO;
using System.Text;

namespace jphtml.Core
{
	public class MecabReader
	{
		public StringBuilder ReadResponse(TextReader reader)
		{
			var builder = new StringBuilder();
			string line;
			while (!(line = reader.ReadLine()).Equals("EOS", StringComparison.InvariantCulture))
			{
				builder.AppendLine(line);
			}
			return builder;
		}
	}
}

