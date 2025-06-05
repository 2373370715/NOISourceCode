using System;
using Database;

// Token: 0x02000997 RID: 2455
public class ArtableInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x000C0EEF File Offset: 0x000BF0EF
	// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x000C0EF7 File Offset: 0x000BF0F7
	public string id { get; set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000C0F00 File Offset: 0x000BF100
	// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x000C0F08 File Offset: 0x000BF108
	public string name { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000C0F11 File Offset: 0x000BF111
	// (set) Token: 0x06002BCA RID: 11210 RVA: 0x000C0F19 File Offset: 0x000BF119
	public string desc { get; set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000C0F22 File Offset: 0x000BF122
	// (set) Token: 0x06002BCC RID: 11212 RVA: 0x000C0F2A File Offset: 0x000BF12A
	public PermitRarity rarity { get; set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06002BCD RID: 11213 RVA: 0x000C0F33 File Offset: 0x000BF133
	// (set) Token: 0x06002BCE RID: 11214 RVA: 0x000C0F3B File Offset: 0x000BF13B
	public string animFile { get; set; }

	// Token: 0x06002BCF RID: 11215 RVA: 0x001EDB88 File Offset: 0x001EBD88
	public ArtableInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.anim = anim;
		this.decor_value = decor_value;
		this.cheer_on_complete = cheer_on_complete;
		this.status_id = status_id;
		this.prefabId = prefabId;
		this.symbolname = symbolname;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000C0F44 File Offset: 0x000BF144
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000C0F4C File Offset: 0x000BF14C
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001DFC RID: 7676
	public string anim;

	// Token: 0x04001DFD RID: 7677
	public int decor_value;

	// Token: 0x04001DFE RID: 7678
	public bool cheer_on_complete;

	// Token: 0x04001DFF RID: 7679
	public string status_id;

	// Token: 0x04001E00 RID: 7680
	public string prefabId;

	// Token: 0x04001E01 RID: 7681
	public string symbolname;

	// Token: 0x04001E02 RID: 7682
	public string[] requiredDlcIds;

	// Token: 0x04001E03 RID: 7683
	public string[] forbiddenDlcIds;
}
