using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;

// Token: 0x02000999 RID: 2457
[DebuggerDisplay("{id} - {name}")]
public class BuildingFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06002BDF RID: 11231 RVA: 0x000C0FB9 File Offset: 0x000BF1B9
	// (set) Token: 0x06002BE0 RID: 11232 RVA: 0x000C0FC1 File Offset: 0x000BF1C1
	public string id { get; set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x000C0FCA File Offset: 0x000BF1CA
	// (set) Token: 0x06002BE2 RID: 11234 RVA: 0x000C0FD2 File Offset: 0x000BF1D2
	public string name { get; set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06002BE3 RID: 11235 RVA: 0x000C0FDB File Offset: 0x000BF1DB
	// (set) Token: 0x06002BE4 RID: 11236 RVA: 0x000C0FE3 File Offset: 0x000BF1E3
	public string desc { get; set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06002BE5 RID: 11237 RVA: 0x000C0FEC File Offset: 0x000BF1EC
	// (set) Token: 0x06002BE6 RID: 11238 RVA: 0x000C0FF4 File Offset: 0x000BF1F4
	public PermitRarity rarity { get; set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x000C0FFD File Offset: 0x000BF1FD
	// (set) Token: 0x06002BE8 RID: 11240 RVA: 0x000C1005 File Offset: 0x000BF205
	public string animFile { get; set; }

	// Token: 0x06002BE9 RID: 11241 RVA: 0x001EDC8C File Offset: 0x001EBE8C
	public BuildingFacadeInfo(string id, string name, string desc, PermitRarity rarity, string prefabId, string animFile, Dictionary<string, string> workables = null, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.prefabId = prefabId;
		this.animFile = animFile;
		this.workables = workables;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x000C100E File Offset: 0x000BF20E
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x000C1016 File Offset: 0x000BF216
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E11 RID: 7697
	public string prefabId;

	// Token: 0x04001E13 RID: 7699
	public Dictionary<string, string> workables;

	// Token: 0x04001E14 RID: 7700
	public string[] requiredDlcIds;

	// Token: 0x04001E15 RID: 7701
	public string[] forbiddenDlcIds;
}
