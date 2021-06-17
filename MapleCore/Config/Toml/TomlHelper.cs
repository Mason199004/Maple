using Nett;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MapleCore.Config.Toml
{
	public class TomlHelper
	{
		private TomlTable root = Nett.Toml.Create();


		public void Clear()
		{
			root.Clear();
		}
		
		public T? Get<T>(string key)
		{
			if (root.ContainsKey(key))
			{
				return root.Get<T>(key);
			}

			return (T)(object?)null;
		}

		public void Import(TomlTable from)
		{
			if (from is null)
			{
				return;
			}
			foreach (var pair in from)
			{
				if (root.ContainsKey(pair.Key))
				{
					root[pair.Key] = pair.Value;
				}
				else
				{
					root.Add(pair.Key, pair.Value);
				}
			}
		}
		
	}
}
