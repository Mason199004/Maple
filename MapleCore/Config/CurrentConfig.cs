using System.Collections.Generic;
using Functional.Option;

namespace MapleCore.Config
{
	public class CurrentConfig : IGlobalConfig, ILocalConfig
	{
		public bool AUTO_ADD_SRC { get; set; }
		public bool RECURSE_SRC { get; set; }
		public List<string> C_SRC_EXTENSIONS { get; set; }
		public List<string> CXX_SRC_EXTENSIONS { get; set; }
		public string ProjectName { get; set; }
		public Option<List<string>> CSrc { get; set; }
		public Option<List<string>> CXXSrc { get; set; }
		public Option<List<string>> Dependencies { get; set; }

		public void FromGlobal(IGlobalConfig cfg)
		{
			AUTO_ADD_SRC = cfg.AUTO_ADD_SRC;
			RECURSE_SRC = cfg.RECURSE_SRC;
			C_SRC_EXTENSIONS = cfg.C_SRC_EXTENSIONS;
			CXX_SRC_EXTENSIONS = cfg.CXX_SRC_EXTENSIONS;
		}

		public void FromLocal(ILocalConfig cfg)
		{
			ProjectName = cfg.ProjectName;
			CSrc = cfg.CSrc;
			CXXSrc = cfg.CXXSrc;
			Dependencies = cfg.Dependencies;
		}
	}
}