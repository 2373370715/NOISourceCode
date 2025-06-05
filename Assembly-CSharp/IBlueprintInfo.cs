using System;
using Database;

// Token: 0x02000996 RID: 2454
public interface IBlueprintInfo : IHasDlcRestrictions
{
	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06002BBB RID: 11195
	// (set) Token: 0x06002BBC RID: 11196
	string id { get; set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06002BBD RID: 11197
	// (set) Token: 0x06002BBE RID: 11198
	string name { get; set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06002BBF RID: 11199
	// (set) Token: 0x06002BC0 RID: 11200
	string desc { get; set; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06002BC1 RID: 11201
	// (set) Token: 0x06002BC2 RID: 11202
	PermitRarity rarity { get; set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06002BC3 RID: 11203
	// (set) Token: 0x06002BC4 RID: 11204
	string animFile { get; set; }
}
