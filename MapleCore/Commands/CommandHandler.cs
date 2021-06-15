using System;
using MapleCore.Tools;

namespace MapleCore.Commands
{
    public class CommandHandler
    {
        public static void HandleCommand(string[] args)
        {
            if (args.Length == 0) throw new ArgumentException("Unable to handle null command");
            switch (args[0])
            {
                case "src":
                    if (args.Length < 2) throw new ArgumentException("no subcommand passed to src");
                    Env.InitEnv();
                    switch (args[1])
                    {
                        case "add":
                            CommandSrc.Add(FilePaths.Normalize(args[2]));
                            break;
                    }
                    break;
                case "build":
                    Env.InitEnv();
                    CommandBuild.Build();
                    break;
            }
        }
    }
}