using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200039D RID: 925
public abstract class DatabankHelper
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x000B0CDD File Offset: 0x000AEEDD
	public static string ID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "OrbitalResearchDatabank";
			}
			return "ResearchDatabank";
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x000B0CF1 File Offset: 0x000AEEF1
	public static Tag TAG
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return OrbitalResearchDatabankConfig.TAG;
			}
			return ResearchDatabankConfig.TAG;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000EEA RID: 3818 RVA: 0x000B0D05 File Offset: 0x000AEF05
	public static string RESEARCH_NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return RESEARCH.TYPES.ORBITAL.NAME;
			}
			return RESEARCH.TYPES.GAMMA.NAME;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000EEB RID: 3819 RVA: 0x000B0D23 File Offset: 0x000AEF23
	public static string RESEARCH_CODEXID
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return "RESEARCHDLC1";
			}
			return "RESEARCH";
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000EEC RID: 3820 RVA: 0x000B0D37 File Offset: 0x000AEF37
	public static string NAME
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000EED RID: 3821 RVA: 0x000B0D55 File Offset: 0x000AEF55
	public static string NAME_PLURAL
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME_PLURAL;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME_PLURAL;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000EEE RID: 3822 RVA: 0x000B0D73 File Offset: 0x000AEF73
	public static string DESC
	{
		get
		{
			if (DlcManager.IsExpansion1Active())
			{
				return ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC;
			}
			return ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.DESC;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000EEF RID: 3823 RVA: 0x000B0D91 File Offset: 0x000AEF91
	public static Sprite SPRITE
	{
		get
		{
			return Assets.GetSprite("ui_databank");
		}
	}

	// Token: 0x04000B05 RID: 2821
	public const string CODEXID = "Databank";
}
