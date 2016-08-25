using System;
using System.Diagnostics;

namespace jphtml.Core.Ipc
{
	public class MecabRunner
	{
		public void RunMecab(Action<Process> onProcess)
		{
			using (var process = Process.Start(new ProcessStartInfo
			{
				FileName = "/opt/local/bin/mecab",
				RedirectStandardError = true,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				UseShellExecute = false
			}))
			{
				onProcess(process);
				process.Close();
			}
		}
	}
}

