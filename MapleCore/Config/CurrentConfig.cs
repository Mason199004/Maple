using System.Collections.Generic;
using Functional.Option;

namespace MapleCore.Config
{
	public class CurrentConfig : IGlobalConfig, ILocalConfig
	{
		public bool AutoAddSrc { get; set; }
		public bool RecSearchSrc { get; set; }
		public List<string> C_SRC_EXTENSIONS { get; set; }
		public List<string> CXX_SRC_EXTENSIONS { get; set; }
		public string ProjectName { get; set; }
		public Option<List<string>> CSrc { get; set; }
		public Option<List<string>> CXXSrc { get; set; }
		public Option<List<string>> Dependencies { get; set; }

		public void FromGlobal(IGlobalConfig cfg)
		{
			AutoAddSrc = cfg.AutoAddSrc;
			RecSearchSrc = cfg.RecSearchSrc;
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