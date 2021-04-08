using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tomlyn;
using Tomlyn.Model;

namespace Maple
{
    //TODO Finish
    public class MapleContext
    {
        public MapleContext(DirectoryInfo ProjectDir)
        {
            this.ProjectDir = ProjectDir;
            Config = ConfigSystem.GetSettings(ProjectDir);
        }
        public DirectoryInfo ProjectDir { get; set; }
        public Settings Config { get; private set; }
    }

    public class ConfigSystem
    {
        
        public static Settings TomlToObj(string toml)
        {
            var def = new Settings();
            var data = Toml.Parse(toml).ToModel();
            var set = new Settings();
            var table = (TomlTable)data["MapleProject"];
            set.ProjectName = GetValueOrDefault<string>(table, "ProjectName");
            set.CSrc = GetValueOrDefault<List<string>>(table, "C_SRC");
            set.CXXSrc = GetValueOrDefault<List<string>>(table, "CXX_SRC");
            set.Dependencies = GetValueOrDefault<List<string>>(table, "Dependencies");
            set.AutoAddSrc = GetValueOrDefault<bool>(table, "AUTO_ADD_SRC");
            set.RecSearchSrc = GetValueOrDefault<bool>(table, "RECURSE_SRC");

            return set;
        }

        public static T GetValueOrDefault<T>(TomlTable table, string KeyName)
        {
            if (typeof(T) == typeof(List<string>))
            {
                if (table.ContainsKey(KeyName))
                {
                    var value = table[KeyName];
                    return (T)(object)(from i in ((Tomlyn.Model.TomlArray)value) select i as string).ToList();
                }
                return default;

            }
            return table.ContainsKey(KeyName) ? (T)table[KeyName] : default;
        }
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
