using System;
using System.Collections.Generic;
using System.IO;
using Nett;
namespace MapleCore.Config
{
	public static class ConfigSystem
	{
		public static IGlobalConfig GetGlobalConfig()
		{
			if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
			                 "/.config.maple"))
			{
				WriteGlobal(new GlobalConfig() {AUTO_ADD_SRC = true, RECURSE_SRC = true, C_SRC_EXTENSIONS = new List<string>(new[] {"c"}), CXX_SRC_EXTENSIONS = new List<string>(new []{"cpp"})});
			}
			var toml = Toml.ReadFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
			                         "/.config.maple");
			var cfg = new GlobalConfig();
			
			cfg.AUTO_ADD_SRC = toml["AUTO_ADD_SRC"].Get<bool>();
			cfg.RECURSE_SRC = toml["RECURSE_SRC"].Get<bool>();
			cfg.C_SRC_EXTENSIONS = toml["C_SRC_EXTENSIONS"].Get<List<string>>();
			cfg.CXX_SRC_EXTENSIONS = toml["CXX_SRC_EXTENSIONS"].Get<List<string>>();
			return cfg;
		}

		public static ILocalConfig GetLocalConfig(string path)
		{
			var cfg = new LocalConfig();
			var toml = Toml.ReadFile(path);
			toml = (TomlTable)toml["Maple"];
			if (toml.ContainsKey("C_SRC")) cfg.CSrc = toml["C_SRC"].Get<List<string>>();
			if (toml.ContainsKey("CXX_SRC")) cfg.CXXSrc = toml["CXX_SRC"].Get<List<string>>();
			if (toml.ContainsKey("Dependencies")) cfg.Dependencies = toml["Dependencies"].Get<List<string>>();
			cfg.ProjectName = toml["ProjectName"].Get<string>();
			return cfg;
		}

		public static CurrentConfig GetCurrentConfig(string Path)
		{
			var global = GetGlobalConfig();
			var local = GetLocalConfig(Path);
			var cfg = new CurrentConfig();
			cfg.FromGlobal(global);
			cfg.FromLocal(local);
			return cfg;
		}

		public static void WriteGlobal(IGlobalConfig cfg)
		{
			var global = new GlobalConfig();
			global.AUTO_ADD_SRC = cfg.AUTO_ADD_SRC;
			global.RECURSE_SRC = cfg.RECURSE_SRC;
			global.C_SRC_EXTENSIONS = cfg.C_SRC_EXTENSIONS;
			global.CXX_SRC_EXTENSIONS = cfg.CXX_SRC_EXTENSIONS;
			Toml.WriteFile(global, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
			                       "/.config.maple");
		}

		public static void WriteLocal(ILocalConfig cfg, string Path)
		{
			
			var table = Toml.Create();
			table.Add("ProjectName", cfg.ProjectName);
			if (cfg.CSrc.HasValue) table.Add("C_SRC", cfg.CSrc.ValueOrDefault);
			if (cfg.CXXSrc.HasValue) table.Add("CXX_SRC", cfg.CXXSrc.ValueOrDefault);
			if (cfg.Dependencies.HasValue) table.Add("Dependencies", cfg.Dependencies.ValueOrDefault);
			Toml.WriteFile(table, Path);
		}
	}
}