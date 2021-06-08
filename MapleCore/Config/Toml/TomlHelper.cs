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

		public void SetGlobal(string key, object value)
		{
			((TomlTable)root["GlobalConfig"]).Add(key, value);
		}

		public void SetGlobal(string key, object value, string comment)
		{
			((TomlTable)root["GlobalConfig"]).Add(key, value);
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

		public void GlobalInit()
		{
			root.AddComment($"Maple global config file generated on {DateTime.Now} using Version: {Env.Version}", CommentLocation.Prepend);
			root.Add("GlobalConfig", Nett.Toml.Create());
			SetGlobal("AUTO_ADD_SRC", true, "Determines whether or not Maple will automatically add source files");
			SetGlobal("RECURSE_SRC", true, "Should maple search recursively in the src dir");

		}
	}
}
