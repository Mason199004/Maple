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
			var cfgloc = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.config.maple";
			Global = Nett.Toml.ReadFile(cfgloc);
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
			public const string AutoSrc = "AUTO_ADD_SRC";
			public const string RecurseSrc = "RECURSE_SRC";
			public const string C_Ext = "C_EXTENSIONS";
			public const string Cpp_Ext = "CXX_EXTENSIONS";
			public const string C_Src = "CSrc";
			public const string Cpp_Src = "CXXSrc";
		}

		

		public static Dictionary<string, object> Defaults = new()
		{
			{ConfigurableSettings.AutoSrc, true},
			{ConfigurableSettings.RecurseSrc, true},
			{ConfigurableSettings.C_Ext, new List<string> {"c"}},
			{ConfigurableSettings.Cpp_Ext, new List<string> {"cpp"}},
			{ConfigurableSettings.C_Src, new List<string> {""}},
			{ConfigurableSettings.Cpp_Src, new List<string> {""}},
		};
	}
}