using System;
using System.IO;
using MapleCore.Config;
using MapleCore.Interaction;

namespace MapleCore.Commands
{
    public class CommandSrc
    {
        public static void Add(string file)
        {
            var f = new FileInfo(file);
            if (Env.Config.C_SRC_EXTENSIONS.Contains(f.Extension))
            {
                Env.Config.CSrc.ValueOrDefault.Add(file);
            }
            else if (Env.Config.CXX_SRC_EXTENSIONS.Contains(f.Extension))
            {
                Env.Config.CXXSrc.ValueOrDefault.Add(file);
            }
            else
            {
                bop:
                var type = UserInteract.PromptUser("Could not determine source type from extension, what type of source file is this?", new[] {"C", "CXX"});
                if (type.ToLower() != "c" && type.ToLower() != "cxx")
                {
                    Console.WriteLine("Invalid response!");
                    goto bop;
                }
                
                if (UserInteract.PromptUser("Would you like to save this preference?", new[] {"y", "n"}).ToLower() ==
                    "y")
                {
                    if (type.ToLower() == "c")
                    {
                        Env.Config.C_SRC_EXTENSIONS.Add(f.Extension);
                    }
                    else
                    {
                        Env.Config.CXX_SRC_EXTENSIONS.Add(f.Extension);
                    }
                    ConfigSystem.WriteGlobal(Env.Config);
                }

                if (type.ToLower() == "c")
                {
                    Env.Config.CSrc.ValueOrDefault.Add(file);
                }
                else
                {
                    Env.Config.CXXSrc.ValueOrDefault.Add(file);
                }
            }
        }
    }
}