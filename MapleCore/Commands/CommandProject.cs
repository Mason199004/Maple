using System;
using System.Collections.Generic;
using System.IO;
using MapleCore.Config;
using Nett;

namespace MapleCore.Commands
{
	public class CommandProject
	{
		
		private const string ExampleC = "#include <stdio.h>\n" +
		                                          "\n" +
		                                          "int main()\n" +
		                                          "{\n" +
		                                          "  puts(\"Hello World!\");\n" +
		                                          "}\n";

		
		public static void NewProject(string name)
		{
			if (Directory.Exists(name))
			{
				Console.WriteLine($"Error: Directory {name} already exists.");
				return;
			}

			Directory.CreateDirectory(name);
			Directory.SetCurrentDirectory(name);
			Env.ProjectDir = new DirectoryInfo(Environment.CurrentDirectory); //reset project dir so that config can be properly saved
			Config.Config.Local = Config.Config.Global;
			Config.Config.Local.Remove(Config.Config.ConfigurableSettings.C_Compiler);
			Config.Config.Local.Remove(Config.Config.ConfigurableSettings.CXX_Compiler);
			Config.Config.Local.Add(Config.Config.ConfigurableSettings.ProjectName, name);
			Config.Config.Local.Add(Config.Config.ConfigurableSettings.C_Src, new List<string>() {"src/main.c"});
			Config.Config.WriteLocal();
			Directory.CreateDirectory("build");
			Directory.CreateDirectory("working");
			Directory.CreateDirectory("src");
			File.WriteAllText("src/main.c", ExampleC);
		}
	}
}