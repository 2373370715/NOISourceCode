using System;
using STRINGS;

// Token: 0x020003AB RID: 939
internal static class KeroseneEngineHelper
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x000B0EA6 File Offset: 0x000AF0A6
	public static string ID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "KeroseneEngineCluster";
			}
			return "KeroseneEngine";
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000F30 RID: 3888 RVA: 0x000B0EBA File Offset: 0x000AF0BA
	public static string CODEXID
	{
		get
		{
			return KeroseneEngineHelper.ID.ToUpperInvariant();
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000F31 RID: 3889 RVA: 0x000B0EC6 File Offset: 0x000AF0C6
	public static string NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return BUILDINGS.PREFABS.KEROSENEENGINECLUSTER.NAME;
			}
			return BUILDINGS.PREFABS.KEROSENEENGINE.NAME;
		}
	}
}
