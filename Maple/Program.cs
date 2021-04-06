using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Tomlyn;
using Tomlyn.Syntax;

namespace Maple
{
    
    class Program
    {
        public const string Version = "0.0.1";
        static void Main(string[] args)
        {
           ParseOptions(args); 
            
        }
        
        static void ParseOptions(string[] args)
        {
            var arg1 = args.Length > 0 ? args[0] : default;
            var arg2 = args.Length > 1 ? args[1] : default;
            switch (arg1.ToLower())
            {
                case "build":
                    BuildProj();
                    break;
                case "new":
                    NewProj(arg2);
                    break;
                case "add":
                    AddFile(arg2);
                    break;
            }
        }

        static void BuildProj()
        {
            if (File.Exists("build.maple"))
            {
                var settings = Helper.TomlToObj(File.ReadAllText("build.maple"));
                Console.WriteLine("Building Project \"" + settings.ProjectName + "\"");
                Process p = default;
                List<string> oFiles = new List<string>();
                foreach (var file in settings.CSrc)
                {
                    p = new Process();
                    Console.WriteLine($"Building C Object {file}");
                    p.StartInfo.FileName = "cc";
                    p.StartInfo.Arguments += $"src/{file} ";
                    p.StartInfo.Arguments += $"-c -o working/{settings.ProjectName}_{file}.o";
                    oFiles.Add($"working/{settings.ProjectName}_{file}.o");
                    p.Start();
                    p.WaitForExit();
                }
                
                foreach (var file in settings.CXXSrc)
                {
                    Console.WriteLine($"Building CXX Object {file}");
                    p = new Process();
                    p.StartInfo.FileName = "c++";
                    p.StartInfo.Arguments += $"src/{file} ";
                    p.StartInfo.Arguments += $"-lstdc++ -c -o working/{settings.ProjectName}_{file}.o";
                    oFiles.Add($"working/{settings.ProjectName}_{file}.o");
                    p.Start();
                    p.WaitForExit();
                }
                
                
                
                p = new Process();
                p.StartInfo.FileName = "cc";
                
                
                foreach (var s in oFiles)
                {
                    p.StartInfo.Arguments += $"{s} ";
                }

                if (settings.CXXSrc.Count > 0)
                {
                    p.StartInfo.Arguments += "-lstdc++ ";
                }
                p.StartInfo.Arguments += $"-o build/{settings.ProjectName}";
                p.Start();
                p.WaitForExit();
            }
            else
            {
                Console.WriteLine("Error: Failed to locate build.maple!");
            }
        }
        #region ExampleC
        private static string ExampleC = "#include <stdio.h>\n" +
                                         "\n" +
                                         "int main()\n" +
                                         "{\n" +
                                         "  puts(\"Hello World!\");\n" +
                                         "}\n";
        #endregion
        static void NewProj(string Name)
        {
            var doc = new DocumentSyntax()
            {
                Tables =
                {
                    new TableSyntax("Maple")
                    {
                        Items =
                        {
                            {"ProjectName", "Test"},
                            {"C_SRC", new string[]{"main.c"}},
                            {"CXX_SRCc", new string[]{}},
                            {"Dependencies", new string[]{}}
                        }
                    }
                }
            };
            Directory.CreateDirectory(Name);
            Directory.SetCurrentDirectory(Name);
            Directory.CreateDirectory("src");
            Directory.CreateDirectory("build");
            Directory.CreateDirectory("working");
            Directory.CreateDirectory("lib");
            File.WriteAllText("build.maple", $"#Maple build file generated using Maple v{Version} on {DateTime.Now.ToString()}\n" + doc.ToString());
            File.WriteAllText("src/main.c", ExampleC);
        }

        static void AddFile(string file)
        {
            var f = new FileInfo(file);
            var mapleBuild = GetMapleBuild(f.Directory);
            var settings = Helper.TomlToObj(File.ReadAllText(mapleBuild));
            var MapleDir = new FileInfo(mapleBuild).Directory.FullName + "/src";
            if (f.Name.Split('.')[1] == "cpp")
            {
                settings.CXXSrc.Add(Path.GetRelativePath(MapleDir, f.FullName));
            }
            else
            {
                settings.CSrc.Add(Path.GetRelativePath(MapleDir, f.FullName));
            }

            File.WriteAllText(mapleBuild, Helper.ObjToToml(settings));
            Console.WriteLine($"Added \"{file}\" as source");
        }

        static string GetMapleBuild(DirectoryInfo dir)
        {
            if (dir.GetFiles().Any(t =>
            {
                return t.Name == "build.maple";
            }))
            {
                return dir.GetFiles().First(t =>
                {
                    return t.Name == "build.maple";
                }).FullName;
            }
            else
            {
                return GetMapleBuild(dir.Parent);
            }
        }
    }
}
