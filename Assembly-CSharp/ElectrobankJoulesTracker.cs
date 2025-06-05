using System;

// Token: 0x02000B80 RID: 2944
public class ElectrobankJoulesTracker : WorldTracker
{
	// Token: 0x06003744 RID: 14148 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public ElectrobankJoulesTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x000C85C7 File Offset: 0x000C67C7
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x000C8590 File Offset: 0x000C6790
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
