using System;
using System.IO;
using System.Linq;
using System.Text;
using jphtml.Core;
using jphtml.Core.Ipc;
using jphtml.Core.Html;

namespace jphtml
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("start");

			var mecabRunner = new MecabRunner();
			var mecabReader = new MecabReader();
			var mecabParser = new MecabParser();
			var htmlPrinter = new HtmlPrinter();

			mecabRunner.RunMecab(process =>
			{
				process.StandardInput.WriteLine("ウィキペディアは誰でも編集できるフリー百科事典です");

				var lines = mecabReader.ReadResponse(process.StandardOutput);
				Console.WriteLine(string.Join("\n", lines));

				var words = lines.Select(s => mecabParser.ParseWord(s)).ToArray();
				using (var fileWriter = new StreamWriter("jp.html", false, Encoding.UTF8))
				{
					int i = 0;
					htmlPrinter.PrintDocument(
						fileWriter,
						() => i < words.Length ? htmlPrinter.FormatWord(words[i++]) : string.Empty);
				}
			});

			Console.WriteLine("end");
		}
	}
}
