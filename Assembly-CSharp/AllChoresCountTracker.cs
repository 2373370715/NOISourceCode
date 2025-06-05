using System;

// Token: 0x02000B73 RID: 2931
public class AllChoresCountTracker : WorldTracker
{
	// Token: 0x0600371C RID: 14108 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public AllChoresCountTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x00223340 File Offset: 0x00221540
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			Tracker choreGroupTracker = TrackerTool.Instance.GetChoreGroupTracker(base.WorldID, Db.Get().ChoreGroups[i]);
			num += ((choreGroupTracker == null) ? 0f : choreGroupTracker.GetCurrentValue());
		}
		base.AddPoint(num);
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x000C6C93 File Offset: 0x000C4E93
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
