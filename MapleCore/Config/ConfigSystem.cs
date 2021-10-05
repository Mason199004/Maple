using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using fNbt.Tags;
using MapleCore.Config.Nbt;
using Nett;

namespace MapleCore.Config
{
	class ConfigSystem
	{

		private NbtCfg BackingConfig;

		public ConcurrentQueue<NbtTag> UpdateQueue = new();

		private Mutex Mut = new();

		public async Task UpdateLoop()
		{
			BackingConfig = new NbtCfg(NbtCfg.CfgType.LOCAL);
			while (true)
			{
				await Task.Delay(25);
				Mut.WaitOne();
				
				if (UpdateQueue.IsEmpty)
				{
					Mut.ReleaseMutex();
					continue;
				}

				UpdateQueue.TryDequeue(out var temp);

				if (temp.TagType != NbtTagType.Compound)
				{
					if (BackingConfig.root.Contains(temp.Name))
					{
						BackingConfig.root.Remove(temp);
						BackingConfig.root.Add(temp);
					}
					else
					{
						BackingConfig.root.Add(temp);
					}
				}
				else
				{
					RecursiveUpdate(temp as NbtCompound, ref BackingConfig.root);
				}
				Mut.ReleaseMutex();
			}
		}

		static void RecursiveUpdate(NbtCompound from, ref NbtCompound to)
		{
			if (from.Name != to.Name)
				return;

			foreach (var tag in from)
			{
				if (tag.TagType != NbtTagType.Compound)
				{
					if (to.Contains(tag.Name))
					{
						to.Remove(tag);
						to.Add(tag);
					}
					else
					{
						to.Add(tag);
					}
				}
				else
				{
					if (to.Contains(tag.Name))
					{
						var t = from[tag.Name] as NbtCompound;
						RecursiveUpdate(tag as NbtCompound, ref t);
					}
					else
					{
						to.Add(tag);
					}
				}
			}
		}
	}
}
