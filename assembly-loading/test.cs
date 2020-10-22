using System;
using Microsoft.Build.Utilities;

class Program
{
	private static int Main(string[] args)
	{
		var version = ToolLocationHelper.CurrentToolsVersion;
		Console.WriteLine(version);
		return 0;
	}
}