using System;
using Database;

// Token: 0x0200099C RID: 2460
public class EquippableFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000C10E8 File Offset: 0x000BF2E8
	// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000C10F0 File Offset: 0x000BF2F0
	public string id { get; set; }

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000C10F9 File Offset: 0x000BF2F9
	// (set) Token: 0x06002C09 RID: 11273 RVA: 0x000C1101 File Offset: 0x000BF301
	public string name { get; set; }

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000C110A File Offset: 0x000BF30A
	// (set) Token: 0x06002C0B RID: 11275 RVA: 0x000C1112 File Offset: 0x000BF312
	public string desc { get; set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06002C0C RID: 11276 RVA: 0x000C111B File Offset: 0x000BF31B
	// (set) Token: 0x06002C0D RID: 11277 RVA: 0x000C1123 File Offset: 0x000BF323
	public PermitRarity rarity { get; set; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000C112C File Offset: 0x000BF32C
	// (set) Token: 0x06002C0F RID: 11279 RVA: 0x000C1134 File Offset: 0x000BF334
	public string animFile { get; set; }

	// Token: 0x06002C10 RID: 11280 RVA: 0x001EDD84 File Offset: 0x001EBF84
	public EquippableFacadeInfo(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.defID = defID;
		this.buildOverride = buildOverride;
		this.animFile = animFile;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000C113D File Offset: 0x000BF33D
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000C1145 File Offset: 0x000BF345
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E2A RID: 7722
	public string buildOverride;

	// Token: 0x04001E2B RID: 7723
	public string defID;

	// Token: 0x04001E2D RID: 7725
	public string[] requiredDlcIds;

	// Token: 0x04001E2E RID: 7726
	public string[] forbiddenDlcIds;
}
