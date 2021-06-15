using System;
using System.IO;
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
            //TODO: implement finding of project file from non calling dir
            ProjectDir = CallingDir;
            var proj = ProjectDir.GetFiles("build.maple");
            if (proj.Length == 0) throw new FileNotFoundException("Project file does not exist");
            Config = new TomlHelper();
            MapleCore.Config.Config.LoadGlobal();
            MapleCore.Config.Config.LoadLocal();
            Config.Import(MapleCore.Config.Config.Global);
            Config.Import(MapleCore.Config.Config.Local);
            
        }
    }
}