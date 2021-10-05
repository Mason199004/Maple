using System;
using System.Collections.Generic;
using System.IO;
using MapleCore.Config;
using MapleCore.Interaction;

namespace MapleCore.Commands
{
    public class CommandSrc
    {
        public static void Add(string file)
        {
            /*
            var f = new FileInfo(file);
            
            if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Ext)
                .Contains(f.Extension[1..]))
            {
                var temp = ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Src);
                if (temp.Contains(file))
                    return;
                temp.Add(file);
                ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.C_Src, temp);
            }
            else if (ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Ext)
                .Contains(f.Extension[1..]))
            {
                var temp = ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Src);
                if (temp.Contains(file))
                    return;
                temp.Add(file);
                ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.Cpp_Src, temp);
            }
            else
            {
                var type = UserInteract.PromptUser("Error: Unknown Extension, what is source file type?", new List<string>() {"c", "cxx"});
                var willsave = UserInteract.PromptUser("Would you like to save this preference?",
                    new List<string>() {"y", "n"});
                if (willsave.ToLower() == "y")
                {
                    var temp = ConfigSystem.Get<List<string>>(type.ToLower() == "c"
                        ? Config.Config.ConfigurableSettings.C_Ext
                        : Config.Config.ConfigurableSettings.Cpp_Ext);
                    temp.Add(f.Extension[1..]);
                    ConfigSystem.UpdateLocal(type.ToLower() == "c" ? Config.Config.ConfigurableSettings.C_Ext : Config.Config.ConfigurableSettings.Cpp_Ext, temp);
                }

                if (type.ToLower() == "c")
                {
                    var temp = ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.C_Src);
                    temp.Add(file);
                    ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.C_Src, temp);
                }
                else
                {
                    var temp = ConfigSystem.Get<List<string>>(Config.Config.ConfigurableSettings.Cpp_Src);
                    temp.Add(file);
                    ConfigSystem.UpdateLocal(Config.Config.ConfigurableSettings.Cpp_Src, (IEnumerable<string>)temp);
                }
            }*/
            
        }
    }
}