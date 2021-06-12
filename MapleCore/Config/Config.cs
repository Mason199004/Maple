using System;
using MapleCore.Config.Toml;

namespace MapleCore.Config
{
	public class Config
	{
		private TomlHelper Local { get; set; }
		private TomlHelper Global { get; set; }

		bool GetFirst(string key)
		{
			if (Local.ContainsKey(key))
			{
				return true;
			}
			else if (Global.ContainsKey(key))
			{
				return false;
			}

			throw new ArgumentException("key not found");
		}
		
		T GetValue<T>(string key)
		{
			var table = GetFirst(key);
			if (table)
			{
				return Local.GetLocal<T>(key);
			}
			else
			{
				return Global.GetGlobal<T>(key);
			}
		}
		
		
		
	}
}