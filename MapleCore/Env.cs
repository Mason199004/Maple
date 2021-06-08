using System;
using System.IO;
using MapleCore.Config;

namespace MapleCore
{
    public class Env
    {
        public static DirectoryInfo ProjectDir;
        public static DirectoryInfo CallingDir;
        public static CurrentConfig Config;

        public static void InitEnv()
        {
            CallingDir = new DirectoryInfo(Environment.CurrentDirectory);
            //TODO: implement finding of project file from non calling dir
            ProjectDir = CallingDir;
            var proj = ProjectDir.GetFiles("build.maple");
            if (proj.Length == 0) throw new FileNotFoundException("Project file does not exist");
            Config = ConfigSystem.GetCurrentConfig(proj[0].FullName);
        }

        public static void SaveProjFile()
        {
            ConfigSystem.WriteLocal(Config, ProjectDir.GetFiles("build.maple")[0].FullName);
        }
    }
}