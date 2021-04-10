using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Maple
{
	internal class Program
	{
		public const string Version = "0.0.1";

		#region ExampleC

		private static readonly string ExampleC = "#include <stdio.h>\n" +
		                                          "\n" +
		                                          "int main()\n" +
		                                          "{\n" +
		                                          "  puts(\"Hello World!\");\n" +
		                                          "}\n";

		#endregion

		private static void Main(string[] args)
		{
			try
			{
				AddFiles();
			}
			catch (Exception e)
			{
			}

			ParseOptions(args);
		}

		private static void AddFiles()
		{
			var mapleBuild = GetMapleBuild(new DirectoryInfo(Directory.GetCurrentDirectory()));
			var settings = Helper.TomlToObj(File.ReadAllText(mapleBuild));
			if (settings.AutoAddSrc)
			{
				if (settings.RecSearchSrc)
				{
					var dir = new FileInfo(mapleBuild).Directory.FullName + "/src";
					var cfiles = Directory.GetFiles(dir,
						"*.c", SearchOption.AllDirectories);
					var cxxfiles = Directory.GetFiles(dir,
						"*.cpp", SearchOption.AllDirectories);
					var chash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(cfiles)));
					var cxxhash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(cxxfiles)));
					var projchash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(settings.CSrc)));
					var projcxxhash =
						SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(settings.CXXSrc)));
					if (!projchash.Equals(chash))
						settings.CSrc =
							(from i in cfiles
								select Path.GetRelativePath(dir, i))
							.ToList();
					if (!projcxxhash.Equals(cxxhash))
						settings.CXXSrc =
							(from i in cxxfiles
								select Path.GetRelativePath(dir, i))
							.ToList();
				}
				else
				{
					var dir = new FileInfo(mapleBuild).Directory.FullName + "/src";
					var cfiles = Directory.GetFiles(dir,
						"*.c", SearchOption.TopDirectoryOnly);
					var cxxfiles = Directory.GetFiles(dir,
						"*.cpp", SearchOption.TopDirectoryOnly);
					Array.Sort(cfiles);
					Array.Sort(cxxfiles);
					settings.CSrc.Sort();
					settings.CXXSrc.Sort();
					var chash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(cfiles)));
					var cxxhash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(cxxfiles)));
					var projchash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(settings.CSrc)));
					var projcxxhash =
						SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(settings.CXXSrc)));
					if (!projchash.Equals(chash))
						settings.CSrc =
							(from i in cfiles
								select Path.GetRelativePath(dir, i))
							.ToList();
					if (!projcxxhash.Equals(cxxhash))
						settings.CXXSrc =
							(from i in cxxfiles
								select Path.GetRelativePath(dir, i))
							.ToList();
				}

				File.WriteAllText(mapleBuild, Helper.ObjToToml(settings));
			}
		}

		private static void ParseOptions(string[] args)
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

		private static void BuildProj()
		{
			if (true)
			{
				var mapleBuild = GetMapleBuild(new DirectoryInfo(Directory.GetCurrentDirectory()));
				var settings = Helper.TomlToObj(File.ReadAllText(mapleBuild));
				var CWD = Directory.GetCurrentDirectory();
				var mapleDir = mapleBuild[..^11];
				Directory.SetCurrentDirectory(mapleDir);
				Console.WriteLine("Building Project \"" + settings.ProjectName + "\"");
				Process p = default;
				var oFiles = new List<string>();
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


				foreach (var s in oFiles) p.StartInfo.Arguments += $"{s} ";

				if (settings.CXXSrc.Count > 0) p.StartInfo.Arguments += "-lstdc++ ";
				p.StartInfo.Arguments += $"-o build/{settings.ProjectName}";
				p.Start();
				p.WaitForExit();
				Directory.SetCurrentDirectory(CWD);
			}
		}

		private static void NewProj(string Name)
		{
			var def = Settings._default;
			def.ProjectName = Name;
			Directory.CreateDirectory(Name);
			Directory.SetCurrentDirectory(Name);
			Directory.CreateDirectory("src");
			Directory.CreateDirectory("build");
			Directory.CreateDirectory("working");
			Directory.CreateDirectory("lib");
			File.WriteAllText("build.maple", Helper.ObjToToml(def));
			File.WriteAllText("src/main.c", ExampleC);
		}

		private static void AddFile(string file)
		{
			var f = new FileInfo(file);
			var mapleBuild = GetMapleBuild(f.Directory);
			var settings = Helper.TomlToObj(File.ReadAllText(mapleBuild));
			var MapleDir = new FileInfo(mapleBuild).Directory.FullName + "/src";
			if (f.Name.Split('.')[1] == "cpp")
				settings.CXXSrc.Add(Path.GetRelativePath(MapleDir, f.FullName));
			else
				settings.CSrc.Add(Path.GetRelativePath(MapleDir, f.FullName));

			File.WriteAllText(mapleBuild, Helper.ObjToToml(settings));
			Console.WriteLine($"Added \"{file}\" as source");
		}

		private static string GetMapleBuild(DirectoryInfo dir)
		{
			if (dir.GetFiles().Any(t => { return t.Name == "build.maple"; }))
				return dir.GetFiles().First(t => { return t.Name == "build.maple"; }).FullName;
			return GetMapleBuild(dir.Parent);
		}
	}
}