using System;
using System.IO;
using fNbt;
using fNbt.Tags;

namespace MapleCore.Config.Nbt
{
	public class NbtCfg
	{
		public NbtCompound root;

		public enum CfgType
		{
			LOCAL,
			GLOBAL
		}
		
		public NbtCfg(FileInfo cfg)
		{
			root = new NbtFile(cfg.FullName).RootTag;
		}

		public NbtCfg(CfgType type)
		{
			root = new NbtCompound(type.ToString())
			{
				new NbtCompound("GENERIC")
				{
					new NbtByte("AUTO_SRC"),
					new NbtByte("RECURSE_SRC"),
					new NbtList("C_EXT", NbtTagType.String) {new NbtString("c")},
					new NbtList("CXX_EXT", NbtTagType.String) {new NbtString("cxx")},
					new NbtList("CC_FLAGS", NbtTagType.String),
					new NbtList("CXXC_FLAGS", NbtTagType.String),
					new NbtString("CC"),
					new NbtString("CXXC"),
				},
			};
			switch (type)
			{
				case CfgType.LOCAL:
					root.Add(new NbtCompound("SPECIFIC")
					{
						new NbtList("C_SRC", NbtTagType.String),
						new NbtList("CXX_SRC", NbtTagType.String),
						new NbtString("PROJECT_NAME")
					});
					break;
				case CfgType.GLOBAL:
					root.Add(new NbtCompound("SPECIFIC")
					{
						
					});
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}