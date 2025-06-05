using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B76 RID: 2934
public class WorkTimeTracker : WorldTracker
{
	// Token: 0x06003725 RID: 14117 RVA: 0x000C851F File Offset: 0x000C671F
	public WorkTimeTracker(int worldID, ChoreGroup group) : base(worldID)
	{
		this.choreGroup = group;
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x00223544 File Offset: 0x00221744
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		Chore chore;
		Predicate<ChoreType> <>9__0;
		foreach (MinionIdentity minionIdentity in worldItems)
		{
			chore = minionIdentity.GetComponent<ChoreConsumer>().choreDriver.GetCurrentChore();
			if (chore != null)
			{
				List<ChoreType> choreTypes = this.choreGroup.choreTypes;
				Predicate<ChoreType> match2;
				if ((match2 = <>9__0) == null)
				{
					match2 = (<>9__0 = ((ChoreType match) => match == chore.choreType));
				}
				if (choreTypes.Find(match2) != null)
				{
					num += 1f;
				}
			}
		}
		base.AddPoint(num / (float)worldItems.Count * 100f);
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x000C852F File Offset: 0x000C672F
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(Mathf.Round(value), GameUtil.TimeSlice.None).ToString();
	}

	// Token: 0x04002612 RID: 9746
	public ChoreGroup choreGroup;
}
