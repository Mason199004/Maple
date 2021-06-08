using System;
using System.IO;

namespace MapleCore.Tools
{
	public class FilePaths
	{
		public static string Normalize(string PathOrFile)
		{
			return Path.GetRelativePath(Env.ProjectDir.FullName, new FileInfo(PathOrFile).FullName);
		}
	}
}