using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	
	
	public class Settings
	{
		public static Settings _default = new Settings()
		{
			CSrc = new string[] { "main.c" }.ToList(),
			CXXSrc = new string[] { }.ToList(),
			Dependencies = new string[] { }.ToList(),
			ProjectName = "ConsoleApplication",
			AutoAddSrc = true,
			RecSearchSrc = true

		};
		public string ProjectName { get; set; }
		public Lang Language { get; set; }
		public List<string> CSrc { get; set; }
		public List<string> CXXSrc { get; set; }
		public List<string> Dependencies { get; set; }
		public bool AutoAddSrc { get; set; }
		public bool RecSearchSrc { get; set; }

	}
	public class Helper
	{
		
		public static Settings TomlToObj(string toml)
		{
			
			var data = Toml.Parse(toml).ToModel();
			var set = new Settings();
			var table = (TomlTable)data["Maple"];
			set.ProjectName = GetValueOrDefault<string>(table, "ProjectName");
			set.CSrc = GetValueOrDefault<List<string>>(table, "C_SRC");
			set.CXXSrc = GetValueOrDefault<List<string>>(table, "CXX_SRC");
			set.Dependencies = GetValueOrDefault<List<string>>(table, "Dependencies");
			set.AutoAddSrc = GetValueOrDefault<bool>(table,"AUTO_ADD_SRC");
			set.RecSearchSrc = GetValueOrDefault<bool>(table,"RECURSE_SRC");
			
			return set;
		}

		public static T GetValueOrDefault<T>(TomlTable table, string KeyName)
		{
			if (T == typeof(List<string))
            {
				var value = table.ContainsKey(KeyName) ? table[KeyName] : default(List<string>);
				
				return (T)(from i in ((Tomlyn.Model.TomlArray)value) select i as string).ToList();
			}
			return table.ContainsKey(KeyName) ? (T) table[KeyName] : default;
		}

		public static string ObjToToml(Settings set)
		{
			var doc = new DocumentSyntax()
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
			return $"# Maple build file generated using Maple v{Program.Version} on {DateTime.Now.ToString()}\n" +  doc.ToString();
		}
	}
}
