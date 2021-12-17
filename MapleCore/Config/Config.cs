using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapleCore.Config.Nbt;
using MapleCore.Config.Toml;
using MapleCore.Core;
using Nett;

namespace MapleCore.Config
{
	//should contain static methods for each individual setting that can be accessed, i dont care if you think this is dumb
	//it makes the code more concise even if it requires writing a bit more code to serve the requests
	//backend can be fugly as long as the acessible part is simple
	//me no like ConfigSystem.Get<string>(Configlookup[ConfigSystem.CONST.PROJECT_NAME]);
	//me do like Config.GetProjectName();
	public class Config
	{
		private static NbtHelper Global;
		private static NbtHelper Local;

		public static void Init(Context con)
		{
			Local.FromFile(con.ProjectFile.FullName);
			//TODO: THIS WILL EXPLODE IF THE CONFIG FILE DOESNT EXIST
			Global.FromFile(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).GetFiles().First(t => t.Name == "global.maple").FullName);
		}
		
		public static string GetProjectName()
		{
			return Local.GetTag("ProjectName").StringValue;
		}
	}
}