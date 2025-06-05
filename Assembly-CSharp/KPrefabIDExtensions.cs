using System;
using UnityEngine;

// Token: 0x020014C3 RID: 5315
public static class KPrefabIDExtensions
{
	// Token: 0x06006DFD RID: 28157 RVA: 0x000EC9F4 File Offset: 0x000EABF4
	public static Tag PrefabID(this Component cmp)
	{
		return cmp.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006DFE RID: 28158 RVA: 0x000ECA01 File Offset: 0x000EAC01
	public static Tag PrefabID(this GameObject go)
	{
		return go.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006DFF RID: 28159 RVA: 0x000ECA0E File Offset: 0x000EAC0E
	public static Tag PrefabID(this StateMachine.Instance smi)
	{
		return smi.GetComponent<KPrefabID>().PrefabID();
	}

	// Token: 0x06006E00 RID: 28160 RVA: 0x000ECA1B File Offset: 0x000EAC1B
	public static bool IsPrefabID(this Component cmp, Tag id)
	{
		return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06006E01 RID: 28161 RVA: 0x000ECA29 File Offset: 0x000EAC29
	public static bool IsPrefabID(this GameObject go, Tag id)
	{
		return go.GetComponent<KPrefabID>().IsPrefabID(id);
	}

	// Token: 0x06006E02 RID: 28162 RVA: 0x000ECA37 File Offset: 0x000EAC37
	public static bool HasTag(this Component cmp, Tag tag)
	{
		return cmp.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06006E03 RID: 28163 RVA: 0x000ECA45 File Offset: 0x000EAC45
	public static bool HasTag(this GameObject go, Tag tag)
	{
		return go.GetComponent<KPrefabID>().HasTag(tag);
	}

	// Token: 0x06006E04 RID: 28164 RVA: 0x000ECA53 File Offset: 0x000EAC53
	public static bool HasAnyTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06006E05 RID: 28165 RVA: 0x000ECA61 File Offset: 0x000EAC61
	public static bool HasAnyTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAnyTags(tags);
	}

	// Token: 0x06006E06 RID: 28166 RVA: 0x000ECA6F File Offset: 0x000EAC6F
	public static bool HasAllTags(this Component cmp, Tag[] tags)
	{
		return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06006E07 RID: 28167 RVA: 0x000ECA7D File Offset: 0x000EAC7D
	public static bool HasAllTags(this GameObject go, Tag[] tags)
	{
		return go.GetComponent<KPrefabID>().HasAllTags(tags);
	}

	// Token: 0x06006E08 RID: 28168 RVA: 0x000ECA8B File Offset: 0x000EAC8B
	public static void AddTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06006E09 RID: 28169 RVA: 0x000ECA9A File Offset: 0x000EAC9A
	public static void AddTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().AddTag(tag, false);
	}

	// Token: 0x06006E0A RID: 28170 RVA: 0x000ECAA9 File Offset: 0x000EACA9
	public static void RemoveTag(this GameObject go, Tag tag)
	{
		go.GetComponent<KPrefabID>().RemoveTag(tag);
	}

	// Token: 0x06006E0B RID: 28171 RVA: 0x000ECAB7 File Offset: 0x000EACB7
	public static void RemoveTag(this Component cmp, Tag tag)
	{
		cmp.GetComponent<KPrefabID>().RemoveTag(tag);
	}
}
