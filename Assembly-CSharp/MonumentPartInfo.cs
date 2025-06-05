using System;
using Database;

// Token: 0x0200099D RID: 2461
public class MonumentPartInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000C114D File Offset: 0x000BF34D
	// (set) Token: 0x06002C14 RID: 11284 RVA: 0x000C1155 File Offset: 0x000BF355
	public string id { get; set; }

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06002C15 RID: 11285 RVA: 0x000C115E File Offset: 0x000BF35E
	// (set) Token: 0x06002C16 RID: 11286 RVA: 0x000C1166 File Offset: 0x000BF366
	public string name { get; set; }

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06002C17 RID: 11287 RVA: 0x000C116F File Offset: 0x000BF36F
	// (set) Token: 0x06002C18 RID: 11288 RVA: 0x000C1177 File Offset: 0x000BF377
	public string desc { get; set; }

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06002C19 RID: 11289 RVA: 0x000C1180 File Offset: 0x000BF380
	// (set) Token: 0x06002C1A RID: 11290 RVA: 0x000C1188 File Offset: 0x000BF388
	public PermitRarity rarity { get; set; }

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06002C1B RID: 11291 RVA: 0x000C1191 File Offset: 0x000BF391
	// (set) Token: 0x06002C1C RID: 11292 RVA: 0x000C1199 File Offset: 0x000BF399
	public string animFile { get; set; }

	// Token: 0x06002C1D RID: 11293 RVA: 0x001EDDDC File Offset: 0x001EBFDC
	public MonumentPartInfo(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFilename;
		this.state = state;
		this.symbolName = symbolName;
		this.part = part;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x000C11A2 File Offset: 0x000BF3A2
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000C11AA File Offset: 0x000BF3AA
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E34 RID: 7732
	public string state;

	// Token: 0x04001E35 RID: 7733
	public string symbolName;

	// Token: 0x04001E36 RID: 7734
	public MonumentPartResource.Part part;

	// Token: 0x04001E37 RID: 7735
	public string[] requiredDlcIds;

	// Token: 0x04001E38 RID: 7736
	public string[] forbiddenDlcIds;
}
