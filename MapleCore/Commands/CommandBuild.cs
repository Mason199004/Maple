using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MapleCore.Config;
using MapleCore.Config.Nbt;
using MapleCore.Tools;

namespace MapleCore.Commands
{
	public class CommandBuild
	{
		public static NbtHelper nbt;
		public static void Build()
		{
			nbt = new NbtHelper();
			nbt.FromFile(Env.ProjectDir + $"/working/maple.buildData");
			if (ConfigSystem.Get<bool>(Config.Config.ConfigurableSettings.AutoSrc)) DiscoverAndAddSrcFiles();
			if (!Check())
			{
				Console.WriteLine("Build failed");
				return;
			}
			
			var toCompile = ChangedFiles();
			bool fail = false;
			Parallel.ForEach(toCompile, file =>
			{
				if (!BuildFile(file))
				{
					fail = true;
				}
			});
			

			if (fail)
			{
				Console.WriteLine("Error: Build failed");
				nbt.ToFile(Env.ProjectDir + "/working/maple.buildData");
				return;
			}
			
			var p = new Process();
			
			p.StartInfo.FileName = ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.C_Compiler);
			var files = from f in ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Src)
					.Concat(ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Src))
				select "working/" + new FileInfo(f).Name + ".o"; //  :)
			p.StartInfo.Arguments += " " + string.Join(' ', files) + " -lstdc++ -o build/" +
			                         ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.ProjectName);
			p.Start();
			p.WaitForExit();
			if (p.ExitCode != 0)
			{
				Console.WriteLine("Error: Build failed");
			}
			else
			{
				Console.WriteLine("Build Succeeded");
			}
			nbt.ToFile(Env.ProjectDir + "/working/maple.buildData");
			
		}

		public static bool Check()
		{
			bool fail = false;
			if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Src).Count > 0 && 
			    ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.C_Compiler) == string.Empty)
			{
				Console.Error.Write("FATAL: Cannot possibly build project without a C compiler");
				fail = true;
			}

			if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Src).Count > 0 &&
			    ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.CXX_Compiler) == string.Empty)
			{
				Console.Error.Write("FATAL: Cannot possibly build project without a C++ compiler");
				fail = true;
			}

			return !fail;
		}

		public static IEnumerable<string> ChangedFiles()
		{
			var changed = new List<string>();
			var files = ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Src)
				.Concat(ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Src));
			foreach (var file in files)
			{
				if (!nbt.CompareHash(file, System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(Environment.OSVersion.VersionString + File.ReadAllText(file)))))
				{
					changed.Add(file);
				}
			}

			return changed;
		}

		public static bool BuildFile(string ffile)
		{
			var ex = new FileInfo(ffile).Extension[1..];
			var file = new FileInfo(ffile).Name;
			if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Ext)
				.Contains(ex))
			{
				var p = new Process();
				Console.WriteLine($"Building C Object {file}");
				p.StartInfo.FileName = ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.C_Compiler);
				p.StartInfo.Arguments += $"{ffile} ";
				p.StartInfo.Arguments += $"-c {ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.CCFlags)} -o working/{file}.o";
				p.Start();
				p.WaitForExit();
				if (p.ExitCode != 0)
				{
					return false;
				}
			}
			else if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Ext)
				.Contains(ex))
			{
				Console.WriteLine($"Building CXX Object {file}");
				var p = new Process();
				p.StartInfo.FileName = ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.CXX_Compiler);
				p.StartInfo.Arguments += $"{ffile} ";
				p.StartInfo.Arguments += $"-lstdc++ {ConfigSystem.Get<string>(Config.Config.ConfigurableSettings.CXXCFlags)} -c -o working/{file}.o";
				p.Start();
				p.WaitForExit();
				if (p.ExitCode != 0)
				{
					return false;
				}
			}
			nbt.UpdateFile(ffile, System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(Environment.OSVersion.VersionString + File.ReadAllText(ffile))));
			return true;
		}

		public static void DiscoverAndAddSrcFiles()
		{
			var creg = new Regex($"([a-zA-Z0-9\\s_\\\\.\\-\\(\\):])+(\\.{string.Join("|\\.",ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Ext))})$");
			var csrc = new DirectoryInfo("src").GetFiles("*.*", ConfigSystem.Get<bool>(Config.Config.ConfigurableSettings.RecurseSrc) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(t => creg.IsMatch(t.Name));
			var cppreg = new Regex($"([a-zA-Z0-9\\s_\\\\.\\-\\(\\):])+(\\.{string.Join("|\\.",ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Ext))})$");
			var cppsrc = new DirectoryInfo("src").GetFiles("*.*",ConfigSystem.Get<bool>(Config.Config.ConfigurableSettings.RecurseSrc) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(t => cppreg.IsMatch(t.Name));
			ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.C_Src, (from f in csrc select FilePaths.Normalize(f.FullName)).ToList());
			ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.Cpp_Src, (from f in cppsrc select FilePaths.Normalize(f.FullName)).ToList());
		}
	}
}