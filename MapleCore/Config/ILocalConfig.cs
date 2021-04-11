using System.Collections.Generic;
using Functional.Option;

namespace MapleCore.Config
{
	public interface ILocalConfig
	{
		public string ProjectName { get; set; }
		public Option<List<string>> CSrc { get; set; }
		public Option<List<string>> CXXSrc { get; set; }
		public Option<List<string>> Dependencies { get; set; }
	}
}