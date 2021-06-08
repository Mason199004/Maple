using System.Collections.Generic;

namespace MapleCore.Config
{
	public class GlobalConfig : IGlobalConfig
	{
		public bool AUTO_ADD_SRC { get; set; }
		public bool RECURSE_SRC { get; set; }
		public List<string> C_SRC_EXTENSIONS { get; set; }
		public List<string> CXX_SRC_EXTENSIONS { get; set; }
	}
}