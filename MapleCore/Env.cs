using System;
using System.IO;
using System.Threading;
using MapleCore.Config;
using MapleCore.Config.Toml;

namespace MapleCore
{
    public class Env
    {
        public static DirectoryInfo ProjectDir;
        public static DirectoryInfo CallingDir;
        public static TomlHelper Config;
        public const string Version = "0.0.1";

        public static void InitEnv()
        {
            CallingDir = new DirectoryInfo(Environment.CurrentDirectory);
            var inproj = BringToProject();
            Config = new TomlHelper();
            MapleCore.Config.Config.LoadGlobal();
            Config.Import(MapleCore.Config.Config.Global);
            if (inproj)
            {
                MapleCore.Config.Config.LoadLocal();
                Config.Import(MapleCore.Config.Config.Local);
            }
            
            
        }

        public static bool BringToProject()
        {
            start:
            if (File.Exists("build.maple"))
            {
                ProjectDir = new DirectoryInfo(Environment.CurrentDirectory);
                return true;
            }
            if (new DirectoryInfo(Environment.CurrentDirectory).Parent == null)
            {
                ProjectDir = null;
                Directory.SetCurrentDirectory(CallingDir.FullName);
                return false;
            }
            Directory.SetCurrentDirectory(new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName);
            goto start;
        }
    }
}