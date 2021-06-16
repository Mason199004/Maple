using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapleCore.Config.Toml;
using Nett;

namespace MapleCore.Config
{
	public class Config
	{
		public static TomlTable Global;
		public static TomlTable Local;

		public static void LoadGlobal()
		{
			var appdat = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			if (new DirectoryInfo(appdat).GetFiles(".config.maple").Length == 0)
			{
				GenerateGlobal();
			}
			var cfgloc = appdat + "/.config.maple";
			Global = Nett.Toml.ReadFile(cfgloc);
		}

		public static void GenerateGlobal()
		{
			string cc = string.Empty;
			string cpp = string.Empty;
			IEnumerable<FileInfo[]> execpath = default;
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				execpath = from f in Environment.GetEnvironmentVariable("PATH").Split(';')
						.Where(t => Directory.Exists(t))
					select new DirectoryInfo(f).GetFiles();
			}
			else
			{
				execpath = from f in Environment.GetEnvironmentVariable("PATH").Split(':')
						.Where(t => Directory.Exists(t))
					select new DirectoryInfo(f).GetFiles();
			}
			if (execpath.Any(t => t.Any(tt => tt.Name is "cc" or "cc.exe")))
			{
				cc = "cc";
			}
			else if (execpath.Any(t => t.Any(tt => tt.Name is "gcc" or "gcc.exe")))
			{
				cc = "gcc";
			}
			else if (execpath.Any(t => t.Any(tt => tt.Name is "clang" or "clang.exe")))
			{
				cc = "clang";
			}
			if (execpath.Any(t => t.Any(tt => tt.Name is "c++" or "c++.exe")))
			{
				cpp = "c++";
			}
			else if (execpath.Any(t => t.Any(tt => tt.Name is "g++" or "g++.exe")))
			{
				cpp = "g++";
			}
			else if (execpath.Any(t => t.Any(tt => tt.Name is "clang++" or "clang++.exe")))
			{
				cpp = "clang++";
			}

			if (cc == string.Empty)
			{
				Console.WriteLine("WARNING: Unable to locate C Compiler, please manually configure in config");
			}

			if (cpp == string.Empty)
			{
				Console.WriteLine("WARNING: Unable to locate C++ Compiler, please manually configure in config");
			}

			Global = Nett.Toml.Create(Config.Defaults);
			Global.Update(ConfigurableSettings.C_Compiler, cc);
			Global.Update(ConfigurableSettings.CXX_Compiler, cpp);
			Global.Remove(ConfigurableSettings.C_Src);
			Global.Remove(ConfigurableSettings.Cpp_Src);
			Global.Remove(ConfigurableSettings.ProjectName);
			WriteGlobal();
		}

		public static void LoadLocal()
		{
			Local = Nett.Toml.ReadFile(Env.ProjectDir.FullName + "/build.maple");
		}

		public static void WriteGlobal()
		{
			if (!Global.Comments.Any(t => t.Text.Contains(Env.Version)))
			{
				Global.AddComment($"Maple config file generated on {DateTime.Now} using Maple ver {Env.Version}", CommentLocation.Prepend);
			}
			Nett.Toml.WriteFile(Global, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.config.maple");
		}

		public static void WriteLocal()
		{
			if (!Local.Comments.Any(t => t.Text.Contains(Env.Version)))
			{
				Local.AddComment($"Maple config file generated on {DateTime.Now} using Maple ver {Env.Version}", CommentLocation.Prepend);
			}
			Nett.Toml.WriteFile(Local, Env.ProjectDir.FullName + "/build.maple");
		}

		public class ConfigurableSettings
		{
			public const string ProjectName = "ProjectName";
			public const string AutoSrc = "AUTO_ADD_SRC";
			public const string RecurseSrc = "RECURSE_SRC";
			public const string C_Ext = "C_EXTENSIONS";
			public const string Cpp_Ext = "CXX_EXTENSIONS";
			public const string C_Src = "CSrc";
			public const string Cpp_Src = "CXXSrc";
			public const string C_Compiler = "CC";
			public const string CXX_Compiler = "CXXC";
			public const string CCFlags = "CCFlags";
			public const string CXXCFlags = "CXXCFlags";
		}

		

		public static Dictionary<string, object> Defaults = new()
		{
			{ConfigurableSettings.AutoSrc, true},
			{ConfigurableSettings.RecurseSrc, true},
			{ConfigurableSettings.C_Ext, new List<string> {"c"}},
			{ConfigurableSettings.Cpp_Ext, new List<string> {"cpp"}},
			{ConfigurableSettings.C_Src, new List<string> {}},
			{ConfigurableSettings.Cpp_Src, new List<string> {}},
			{ConfigurableSettings.C_Compiler, string.Empty},
			{ConfigurableSettings.CXX_Compiler, string.Empty},
			{ConfigurableSettings.CCFlags, string.Empty},
			{ConfigurableSettings.CXXCFlags, string.Empty},
			{ConfigurableSettings.ProjectName, string.Empty}
		};
	}
}