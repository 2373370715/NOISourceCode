using System;
using Database;

// Token: 0x02000998 RID: 2456
public class ClothingItemInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000C0F54 File Offset: 0x000BF154
	// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x000C0F5C File Offset: 0x000BF15C
	public string id { get; set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000C0F65 File Offset: 0x000BF165
	// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x000C0F6D File Offset: 0x000BF16D
	public string name { get; set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000C0F76 File Offset: 0x000BF176
	// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x000C0F7E File Offset: 0x000BF17E
	public string desc { get; set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x000C0F87 File Offset: 0x000BF187
	// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x000C0F8F File Offset: 0x000BF18F
	public PermitRarity rarity { get; set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06002BDA RID: 11226 RVA: 0x000C0F98 File Offset: 0x000BF198
	// (set) Token: 0x06002BDB RID: 11227 RVA: 0x000C0FA0 File Offset: 0x000BF1A0
	public string animFile { get; set; }

	// Token: 0x06002BDC RID: 11228 RVA: 0x001EDC00 File Offset: 0x001EBE00
	public ClothingItemInfo(string id, string name, string desc, PermitCategory category, PermitRarity rarity, string animFile, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		Option<ClothingOutfitUtility.OutfitType> outfitTypeFor = PermitCategories.GetOutfitTypeFor(category);
		if (outfitTypeFor.IsNone())
		{
			throw new Exception(string.Format("Expected permit category {0} on ClothingItemResource \"{1}\" to have an {2} but none found.", category, id, "OutfitType"));
		}
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.outfitType = outfitTypeFor.Unwrap();
		this.category = category;
		this.rarity = rarity;
		this.animFile = animFile;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000C0FA9 File Offset: 0x000BF1A9
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x000C0FB1 File Offset: 0x000BF1B1
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E07 RID: 7687
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x04001E08 RID: 7688
	public PermitCategory category;

	// Token: 0x04001E0B RID: 7691
	private string[] requiredDlcIds;

	// Token: 0x04001E0C RID: 7692
	private string[] forbiddenDlcIds;
}
