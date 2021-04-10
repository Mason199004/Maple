using System;
using System.Collections.Generic;
using System.Linq;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace Maple
{
	public enum Lang
	{
		C,
		CXX
	}


	public class Settings : GlobalConfig
	{
		[Obsolete] public static Settings _default = new Settings();

		public Settings()
		{
			CSrc = new[] {""}.ToList();
			CXXSrc = new string[] { }.ToList();
			Dependencies = new string[] { }.ToList();
			ProjectName = "";
		}

		public string ProjectName { get; set; }
		public Lang Language { get; set; }
		public List<string> CSrc { get; set; }
		public List<string> CXXSrc { get; set; }
		public List<string> Dependencies { get; set; }
	}

	public class Helper
	{
		public static Settings TomlToObj(string toml)
		{
			var data = Toml.Parse(toml).ToModel();
			var set = new Settings();
			var table = (TomlTable) data["MapleProject"];
			set.ProjectName = GetValueOrDefault<string>(table, "ProjectName");
			set.CSrc = GetValueOrDefault<List<string>>(table, "C_SRC");
			set.CXXSrc = GetValueOrDefault<List<string>>(table, "CXX_SRC");
			set.Dependencies = GetValueOrDefault<List<string>>(table, "Dependencies");
			set.AutoAddSrc = GetValueOrDefault<bool>(table, "AUTO_ADD_SRC");
			set.RecSearchSrc = GetValueOrDefault<bool>(table, "RECURSE_SRC");

			return set;
		}

		public static T GetValueOrDefault<T>(TomlTable table, string KeyName)
		{
			if (typeof(T) == typeof(List<string>))
			{
				if (table.ContainsKey(KeyName))
				{
					var value = table[KeyName];
					return (T) (object) (from i in (TomlArray) value select i as string).ToList();
				}

				return (T) (object) new List<string>();
			}

			return table.ContainsKey(KeyName) ? (T) table[KeyName] : default;
		}

		public static string ObjToToml(Settings set)
		{
			var doc = new DocumentSyntax
			{
				Tables =
				{
					new TableSyntax("MapleProject")
					{
						Items =
						{
							{"ProjectName", set.ProjectName},
							{"C_SRC", set.CSrc.ToArray()},
							{"CXX_SRC", set.CXXSrc.ToArray()},
							{"Dependencies", set.Dependencies.ToArray()},
							{"AUTO_ADD_SRC", set.AutoAddSrc},
							{"RECURSE_SRC", set.RecSearchSrc}
						}
					}
				}
			};
			return $"# Maple build file generated using Maple v{Program.Version} on {DateTime.Now.ToString()}\n" + doc;
		}
	}
}