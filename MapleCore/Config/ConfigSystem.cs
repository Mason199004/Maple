using System;
using System.Collections.Generic;
using System.Text;
using Nett;

namespace MapleCore.Config
{
	class ConfigSystem
	{
		public static T Get<T>(string key)
		{
			var got = Env.Config.Get<T>(key);
			if (got is not null)
			{
				return got;
			}
			else
			{
				return (T)Config.Defaults[key];
			}
		}
		//yes, i know, this looks weird and hacky, but for whatever reason you cant just add type object to a TomlTable without it shitting
		public static void UpdateGlobal(string key, object value)
		{
			if (Config.Global.ContainsKey(key))
			{
				var temp = Config.Global.ToDictionary();
				temp[key] = value;
				Config.Global = Nett.Toml.Create(temp);
			}
			else
			{
				var temp = Config.Global.ToDictionary();
				temp.Add(key, value);
				Config.Global = Nett.Toml.Create(temp);
			}
			Config.WriteGlobal();
			Env.Config.Clear();
			Env.Config.Import(Config.Global);
			Env.Config.Import(Config.Local);
		}
		public static void UpdateLocal(string key, object value)
		{
			if (Config.Local.ContainsKey(key))
			{
				var temp = Config.Local.ToDictionary();
				temp[key] = value;
				Config.Local = Nett.Toml.Create(temp);
			}
			else
			{
				var temp = Config.Local.ToDictionary();
				temp.Add(key, value);
				Config.Local = Nett.Toml.Create(temp);
			}
			Config.WriteLocal();
			Env.Config.Clear();
			Env.Config.Import(Config.Global);
			Env.Config.Import(Config.Local);
		}
	}
}
