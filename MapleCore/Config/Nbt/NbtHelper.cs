using System;
using System.IO;
using System.Linq;
using fNbt;
using fNbt.Tags;

namespace MapleCore.Config.Nbt
{
	/*
	 *		root
	 *		|
	 *		|-ProjectName(str)
	 *		|-Languages(listStr)
	 *		|-src(comp)
	 *		|	|						//i like how i keep trying to define the structure of shit that i do despite it being inherently fluctuous and
	 *		|	|-listStr foreach lang  //none of the elements are really required to be defined  
	 *		|
	 *		| 
	 * 
	 */
	public class NbtHelper
	{
		public NbtCompound root;

		public NbtTag GetTag(string path)
		{
			var PathBydir = path.Split('/');
			if (PathBydir.Length > 1)
			{
				NbtCompound rootTag = root;
				for (int i = 0; i < PathBydir.Length - 1; i++)
				{
					if (!rootTag.Contains(PathBydir[i]))
					{
						throw new ArgumentException("cum"); // this isnt an arg problem, TODO: fix later
					}
					rootTag = rootTag[PathBydir[i]] as NbtCompound;
				}

				if (!rootTag.Contains(PathBydir[^1]))
				{
					throw new ArgumentException("");
				}

				return rootTag[PathBydir[^1]];
			}
			return root[PathBydir[^1]];
		}

		public void FromFile(string path)
		{
			if (!File.Exists(path))
			{
				var temp = new NbtCompound("root") {};
				new NbtFile(temp).SaveToFile(path, NbtCompression.GZip);
			}

			root = new NbtFile(path).RootTag;
		}

		public void ToFile(string path)
		{
			new NbtFile(root).SaveToFile(path, NbtCompression.GZip);
		}
	}
}