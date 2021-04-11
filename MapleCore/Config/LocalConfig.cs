using System.Collections.Generic;
using Functional.Option;

namespace MapleCore.Config
{
	public class LocalConfig : ILocalConfig
	{
		public string ProjectName { get; set; }
		public Option<List<string>> CSrc { get; set; }
		public Option<List<string>> CXXSrc { get; set; }
		public Option<List<string>> Dependencies { get; set; }
	}
}