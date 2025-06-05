using System;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
public static class KSelectableExtensions
{
	// Token: 0x060031CA RID: 12746 RVA: 0x000C4CEE File Offset: 0x000C2EEE
	public static string GetProperName(this Component cmp)
	{
		if (cmp != null && cmp.gameObject != null)
		{
			return cmp.gameObject.GetProperName();
		}
		return "";
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x0020DBB8 File Offset: 0x0020BDB8
	public static string GetProperName(this GameObject go)
	{
		if (go != null)
		{
			KSelectable component = go.GetComponent<KSelectable>();
			if (component != null)
			{
				return component.GetName();
			}
		}
		return "";
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x000C4D18 File Offset: 0x000C2F18
	public static string GetProperName(this KSelectable cmp)
	{
		if (cmp != null)
		{
			return cmp.GetName();
		}
		return "";
	}
}
