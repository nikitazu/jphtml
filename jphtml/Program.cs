using System;
using System.Diagnostics;
using jphtml.Core;
using jphtml.Core.Ipc;

namespace jphtml
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("start");

			var mecabRunner = new MecabRunner();
			var mecabReader = new MecabReader();

			mecabRunner.RunMecab(process =>
			{
				process.StandardInput.WriteLine("ウィキペディアは誰でも編集できるフリー百科事典です");

				var builder = mecabReader.ReadResponse(process.StandardOutput);
				Console.WriteLine(builder);
			});

			Console.WriteLine("end");
		}
	}
}
