using Nett;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MapleCore.Config.Toml
{
	class TomlHelper
	{
		private TomlTable root = Nett.Toml.Create();

		public bool SetGlobal(string key, object value)
		{
			if (((TomlTable) root["GlobalConfig"]).ContainsKey(key))
			{
				((TomlTable) root["GlobalConfig"])[key] = (TomlObject) value;
				return true;
			}
			((TomlTable)root["GlobalConfig"]).Add(key, value);
			return false;
		}

		public void SetGlobal(string key, object value, string comment)
		{
			if (SetGlobal(key, value)) return;
			((TomlTable)root["GlobalConfig"])[key].AddComment(comment);
		}

		public T GetGlobal<T>(string key)
		{
			TomlObject ret;
			if (((TomlTable)root["GlobalConfig"]).TryGetValue(key, out ret))
			{
				return (T)(object)ret;
			}
			return default;
		}
		public List<T> GetGlobalList<T>(string key)
		{
			TomlObject ret;
			if (((TomlTable)root["GlobalConfig"]).TryGetValue(key, out ret))
			{
				return (List<T>)(object)ret;
			}
			return new List<T>();
		}

		public bool SetLocal(string key, object value)
		{
			if (((TomlTable) root["MapleProject"]).ContainsKey(key))
			{
				((TomlTable) root["MapleProject"])[key] = (TomlObject) value;
				return true;
			}
			((TomlTable) root["MapleProject"]).Add(key, value);
			return false;
		}

		public void SetLocal(string key, object value, string comment)
		{
			if (SetLocal(key, value)) return;
			((TomlTable) root["MapleProject"])[key].AddComment(comment);
		}
		
		public T GetLocal<T>(string key)
		{
			TomlObject ret;
			if (((TomlTable)root["MapleProject"]).TryGetValue(key, out ret))
			{
				return (T)(object)ret;
			}
			return default;
		}
		public List<T> GetLocalList<T>(string key)
		{
			TomlObject ret;
			if (((TomlTable)root["MapleProject"]).TryGetValue(key, out ret))
			{
				return new List<T>((IEnumerable<T>)ret);
			}
			return new List<T>();
		}

		public bool ContainsKey(string key)
		{
			if (root.ContainsKey("GlobalConfig"))
			{
				if (((TomlTable) root["GlobalConfig"]).ContainsKey(key))
				{
					return true;
				}
			}

			if (root.ContainsKey("MapleProject"))
			{
				if (((TomlTable) root["MapleProject"]).ContainsKey(key))
				{
					return true;
				}
			}

			return false;
		}
		public void GlobalInit()
		{
			root.AddComment($"Maple global config file generated on {DateTime.Now} using Version: {Env.Version}", CommentLocation.Prepend);
			root.Add("GlobalConfig", Nett.Toml.Create());
			SetGlobal("AUTO_ADD_SRC", true, "Determines whether or not Maple will automatically add source files");
			SetGlobal("RECURSE_SRC", true, "Should maple search recursively in the src dir");

		}
	}
}
