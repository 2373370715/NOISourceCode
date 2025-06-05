using System;

// Token: 0x02000B86 RID: 2950
public abstract class WorldTracker : Tracker
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06003756 RID: 14166 RVA: 0x000C85F9 File Offset: 0x000C67F9
	// (set) Token: 0x06003757 RID: 14167 RVA: 0x000C8601 File Offset: 0x000C6801
	public int WorldID { get; private set; }

	// Token: 0x06003758 RID: 14168 RVA: 0x000C860A File Offset: 0x000C680A
	public WorldTracker(int worldID)
	{
		this.WorldID = worldID;
	}
}
