using System;
using MapleCore.Config;
using Nett;

namespace MapleCore
{
	public class Test
	{
		public static void TestCfgSys()
		{
			Env.InitEnv();
			Console.WriteLine(ConfigSystem.Get<bool>("AUTO_ADD_SRC"));
			
		}
	}
}