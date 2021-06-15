using System.Linq;
using fNbt;
using fNbt.Tags;

namespace MapleCore.Config.Nbt
{
	public class NbtHelper
	{
		public NbtCompound root;

		public bool CompareHash(string key, byte[] hash)
		{
			if (((NbtCompound) root["Files"]).Contains(key))
			{
				return root["Files"][key].ByteArrayValue.SequenceEqual(hash);
			}

			return false;
		}

		public void UpdateFile(string key, byte[] hash)
		{
			if (((NbtCompound) root["Files"]).Contains(key))
			{
				root["Files"][key] = new NbtByteArray(hash);
			}
			else
			{
				((NbtCompound)root["Files"]).Add(new NbtByteArray(hash));
			}
		}

		public void FromFile(string path)
		{
			root = new NbtFile(path).RootTag;
		}

		public void ToFile(string path)
		{
			new NbtFile(root).SaveToFile(path, NbtCompression.GZip);
		}
	}
}