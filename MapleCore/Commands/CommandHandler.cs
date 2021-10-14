using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MapleCore.Tools;
using Newtonsoft.Json;

namespace MapleCore.Commands
{
    public class CommandHandler
    {
        public static void HandleCommand(string[] args)
        {
            Stack<string> ArgStack = new();
            for (var i = args.Length - 1; i >= 0; i--)
            {
                ArgStack.Push(args[i]);
            }

            var t = new ExecutionContext();
            t.Command = new Command();
            t.Command.Args = new();
            t.Options = new();
            ProcessArgs(ArgStack, t);
            Console.WriteLine(JsonConvert.SerializeObject(t, Formatting.Indented));

        }

        public static ExecutionContext ProcessArgs(Stack<string> ArgStack, ExecutionContext Context)
        {
            var top = ArgStack.Pop();
            PrimaryCommand.TryParse(top, out PrimaryCommand primaryCommand);
            Context.Command.Primary = primaryCommand;
            Context = SetArgs(ArgStack, Context);
            Context = SetOptions(ArgStack, Context);
            return Context;
        }

        private static ExecutionContext SetOptions(Stack<string> argStack, ExecutionContext context)
        {
            if (argStack.Count == 0)
            {
                return context;
            }

            var singles = argStack.Where(t => !t.StartsWith("--")).Where(t => t.StartsWith('-'));
            foreach (var single in singles)
            {
                foreach (var c in single[1..].ToCharArray())
                {
                    if (SingleToFullMap.Map.ContainsKey(c))
                    {
                        context.Options.Add(SingleToFullMap.Map[c]);
                    }
                }
            }

            return context;
        }

        public static ExecutionContext SetArgs(Stack<string> ArgStack, ExecutionContext Context)
        {
            if (ArgStack.Count == 0)
            {
                return Context;
            }

            while (!ArgStack.Peek().StartsWith('-'))
            {
                Context.Command.Args.Add(ArgStack.Pop());
            }

            return Context;
        }
    }

    public class ExecutionContext
    {
        public FileInfo Project { get; set; }
        
        public Command Command { get; set; }
        
        public List<Option> Options { get; set; }
        
        public Dictionary<string, string> Overrides { get; set; }
    }

    public class Command
    {
        public PrimaryCommand Primary { get; set; }
        
        public List<string> Args { get; set; }
        
    }

    public enum PrimaryCommand
    {
        Build,
        Project,
        Run,
        Settings
    }
    
    public enum Option
    {
        Verbose,
        Headless,
    }

    public static class SingleToFullMap
    {
        public static Dictionary<char, Option> Map = new Dictionary<char, Option>()
            {{'v', Option.Verbose}, {'h', Option.Headless}};
    }
}