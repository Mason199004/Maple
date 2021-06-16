using System;
using MapleCore.Tools;

namespace MapleCore.Commands
{
    public class CommandHandler
    {
        public static void HandleCommand(string[] args)
        {
            Env.InitEnv(); //always do this, its just a good thing
            if (args.Length == 0) throw new ArgumentException("Unable to handle null command");
            switch (args[0])
            {
                case "src":
                    if (args.Length < 2) throw new ArgumentException("no subcommand passed to src");
                    switch (args[1])
                    {
                        case "add":
                            CommandSrc.Add(FilePaths.Normalize( Env.CallingDir.FullName + "/" + args[2]));
                            break;
                    }
                    break;
                case "build":
                    CommandBuild.Build();
                    break;
                case "project":
                case "proj":
                    switch (args[1])
                    {
                        case "new":
                            CommandProject.NewProject(args[2]);
                            break;
                    }
                    break;
            }
        }
    }
}