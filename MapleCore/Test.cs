using System;
using fNbt.Tags;
using MapleCore.Config;
using Nett;

namespace MapleCore
{
	public class Test
	{
		public static void TestCfgSys()
		{
			//var configSystem = new ConfigSystem();
			//configSystem.UpdateQueue.Enqueue(new NbtCompound("root") {new NbtString("pp", "bean")});
			//configSystem.UpdateLoop().Wait();
			//Console.WriteLine(ConfigSystem.Get<bool>("AUTO_ADD_SRC"));
			
			Commands.CommandHandler.HandleCommand("Build Test Moment -vh --test".Split(' '));
		}
	}
}