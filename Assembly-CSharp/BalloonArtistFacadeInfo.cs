using System;
using Database;

// Token: 0x0200099A RID: 2458
public class BalloonArtistFacadeInfo : IBlueprintInfo, IHasDlcRestrictions
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06002BEC RID: 11244 RVA: 0x000C101E File Offset: 0x000BF21E
	// (set) Token: 0x06002BED RID: 11245 RVA: 0x000C1026 File Offset: 0x000BF226
	public string id { get; set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06002BEE RID: 11246 RVA: 0x000C102F File Offset: 0x000BF22F
	// (set) Token: 0x06002BEF RID: 11247 RVA: 0x000C1037 File Offset: 0x000BF237
	public string name { get; set; }

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x000C1040 File Offset: 0x000BF240
	// (set) Token: 0x06002BF1 RID: 11249 RVA: 0x000C1048 File Offset: 0x000BF248
	public string desc { get; set; }

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x000C1051 File Offset: 0x000BF251
	// (set) Token: 0x06002BF3 RID: 11251 RVA: 0x000C1059 File Offset: 0x000BF259
	public PermitRarity rarity { get; set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000C1062 File Offset: 0x000BF262
	// (set) Token: 0x06002BF5 RID: 11253 RVA: 0x000C106A File Offset: 0x000BF26A
	public string animFile { get; set; }

	// Token: 0x06002BF6 RID: 11254 RVA: 0x001EDCE4 File Offset: 0x001EBEE4
	public BalloonArtistFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.balloonFacadeType = balloonFacadeType;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x000C1073 File Offset: 0x000BF273
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x000C107B File Offset: 0x000BF27B
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x04001E1B RID: 7707
	public BalloonArtistFacadeType balloonFacadeType;

	// Token: 0x04001E1C RID: 7708
	public string[] requiredDlcIds;

	// Token: 0x04001E1D RID: 7709
	public string[] forbiddenDlcIds;
}
