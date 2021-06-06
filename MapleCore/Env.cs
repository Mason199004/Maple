using System.IO;
using MapleCore.Config;

namespace MapleCore
{
    public class Env
    {
        public static DirectoryInfo CurrentDir;
        public static CurrentConfig Config;

        public static void SaveProjFile()
        {
            ConfigSystem.WriteLocal(Config, CurrentDir.GetFiles("build.maple")[0].FullName);
        }
    }
}