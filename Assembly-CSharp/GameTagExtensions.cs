using System;
using UnityEngine;

// Token: 0x02001375 RID: 4981
public static class GameTagExtensions
{
	// Token: 0x06006601 RID: 26113 RVA: 0x000E705D File Offset: 0x000E525D
	public static GameObject Prefab(this Tag tag)
	{
		return Assets.GetPrefab(tag);
	}

	// Token: 0x06006602 RID: 26114 RVA: 0x000E7065 File Offset: 0x000E5265
	public static string ProperName(this Tag tag)
	{
		return TagManager.GetProperName(tag, false);
	}

	// Token: 0x06006603 RID: 26115 RVA: 0x000E706E File Offset: 0x000E526E
	public static string ProperNameStripLink(this Tag tag)
	{
		return TagManager.GetProperName(tag, true);
	}

	// Token: 0x06006604 RID: 26116 RVA: 0x000E7077 File Offset: 0x000E5277
	public static Tag Create(SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}

	// Token: 0x06006605 RID: 26117 RVA: 0x000E7077 File Offset: 0x000E5277
	public static Tag CreateTag(this SimHashes id)
	{
		return TagManager.Create(id.ToString());
	}
}
