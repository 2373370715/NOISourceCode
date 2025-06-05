using System;

// Token: 0x02000B75 RID: 2933
public class AllWorkTimeTracker : WorldTracker
{
	// Token: 0x06003722 RID: 14114 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public AllWorkTimeTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003723 RID: 14115 RVA: 0x002234E8 File Offset: 0x002216E8
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			num += TrackerTool.Instance.GetWorkTimeTracker(base.WorldID, Db.Get().ChoreGroups[i]).GetCurrentValue();
		}
		base.AddPoint(num);
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x000C8511 File Offset: 0x000C6711
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(value, GameUtil.TimeSlice.None).ToString();
	}
}
