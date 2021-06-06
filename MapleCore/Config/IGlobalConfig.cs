using System.Collections.Generic;

namespace MapleCore.Config
{
	public interface IGlobalConfig
	{
		public bool AutoAddSrc { get; set; }
		public bool RecSearchSrc { get; set; }
		public List<string> C_SRC_EXTENSIONS { get; set; }
		public List<string> CXX_SRC_EXTENSIONS { get; set; }
	}
}