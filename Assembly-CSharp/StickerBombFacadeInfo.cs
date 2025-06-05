using System;
using Database;

// Token: 0x0200099B RID: 2459
public class StickerBombFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x000C1083 File Offset: 0x000BF283
	// (set) Token: 0x06002BFA RID: 11258 RVA: 0x000C108B File Offset: 0x000BF28B
	public string id { get; set; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06002BFB RID: 11259 RVA: 0x000C1094 File Offset: 0x000BF294
	// (set) Token: 0x06002BFC RID: 11260 RVA: 0x000C109C File Offset: 0x000BF29C
	public string name { get; set; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06002BFD RID: 11261 RVA: 0x000C10A5 File Offset: 0x000BF2A5
	// (set) Token: 0x06002BFE RID: 11262 RVA: 0x000C10AD File Offset: 0x000BF2AD
	public string desc { get; set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06002BFF RID: 11263 RVA: 0x000C10B6 File Offset: 0x000BF2B6
	// (set) Token: 0x06002C00 RID: 11264 RVA: 0x000C10BE File Offset: 0x000BF2BE
	public PermitRarity rarity { get; set; }

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06002C01 RID: 11265 RVA: 0x000C10C7 File Offset: 0x000BF2C7
	// (set) Token: 0x06002C02 RID: 11266 RVA: 0x000C10CF File Offset: 0x000BF2CF
	public string animFile { get; set; }

	// Token: 0x06002C03 RID: 11267 RVA: 0x001EDD34 File Offset: 0x001EBF34
	public StickerBombFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string sticker, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.sticker = sticker;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x000C10D8 File Offset: 0x000BF2D8
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x000C10E0 File Offset: 0x000BF2E0
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E23 RID: 7715
	public string sticker;

	// Token: 0x04001E24 RID: 7716
	public string[] requiredDlcIds;

	// Token: 0x04001E25 RID: 7717
	public string[] forbiddenDlcIds;
}
