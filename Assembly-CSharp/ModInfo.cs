using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x020004AD RID: 1197
[Serializable]
public struct ModInfo
{
	// Token: 0x04000DF9 RID: 3577
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.Source source;

	// Token: 0x04000DFA RID: 3578
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.ModType type;

	// Token: 0x04000DFB RID: 3579
	public string assetID;

	// Token: 0x04000DFC RID: 3580
	public string assetPath;

	// Token: 0x04000DFD RID: 3581
	public bool enabled;

	// Token: 0x04000DFE RID: 3582
	public bool markedForDelete;

	// Token: 0x04000DFF RID: 3583
	public bool markedForUpdate;

	// Token: 0x04000E00 RID: 3584
	public string description;

	// Token: 0x04000E01 RID: 3585
	public ulong lastModifiedTime;

	// Token: 0x020004AE RID: 1198
	public enum Source
	{
		// Token: 0x04000E03 RID: 3587
		Local,
		// Token: 0x04000E04 RID: 3588
		Steam,
		// Token: 0x04000E05 RID: 3589
		Rail
	}

	// Token: 0x020004AF RID: 1199
	public enum ModType
	{
		// Token: 0x04000E07 RID: 3591
		WorldGen,
		// Token: 0x04000E08 RID: 3592
		Scenario,
		// Token: 0x04000E09 RID: 3593
		Mod
	}
}
