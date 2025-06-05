using System;

// Token: 0x02000B7F RID: 2943
public class KCalTracker : WorldTracker
{
	// Token: 0x06003741 RID: 14145 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x000C859E File Offset: 0x000C679E
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x000C6E15 File Offset: 0x000C5015
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
