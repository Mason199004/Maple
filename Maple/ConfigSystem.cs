using System;
using System.IO;
using System.Linq;

namespace Maple
{
    //TODO Finish
    public class MapleContext
    {
        public MapleContext(DirectoryInfo ProjectDir)
        {
            this.ProjectDir = ProjectDir;
            Settings = ConfigSystem.GetSettings(ProjectDir);
        }
        public DirectoryInfo ProjectDir { get; set; }
        public Settings Config { get; private set; }
    }

    public class ConfigSystem
    {
        public static Settings ParseConfigs(string global, string local)
        {

        }
        public static Settings GetSettings(DirectoryInfo ProjectDir)
        {
            var global = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            var local = File.ReadAllText(ProjectDir.GetFiles().First(t => { return t.Name == "build.maple"; }).FullName);

        }
        
    }

    

    public class GlobalConfig
    {
        public GlobalConfig()
        {
            AutoAddSrc = true;
            RecSearchSrc = true;
        }
        public bool AutoAddSrc { get; set; }
        public bool RecSearchSrc { get; set; }
    }

    public class Target
    {
        public string TargetString { get; set; }
        //TODO Implement later
    }
}
