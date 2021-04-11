using MapleCore.Config;
using Nett;

namespace MapleCore
{
	public class Test
	{
		public static void TestCfgSys()
		{
			var cfg = new Config.CurrentConfig();
			cfg.ProjectName = "text";
			Toml.WriteFile((IGlobalConfig)cfg, "test.toml");
			Toml.WriteFile(cfg, "test2.toml");
			ConfigSystem.WriteGlobal(cfg);
		}
	}
}