using System.IO;

namespace MapleCore.Core
{
	public class Context
	{
		public FileInfo ProjectFile { get; }
		public DirectoryInfo CallingDirectory { get; }
		public string ProjectName { get; }
		
	}
}