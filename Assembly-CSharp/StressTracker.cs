using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B7E RID: 2942
public class StressTracker : WorldTracker
{
	// Token: 0x0600373E RID: 14142 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public StressTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x002239A4 File Offset: 0x00221BA4
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i].GetMyWorldId() == base.WorldID)
			{
				num = Mathf.Max(num, Components.LiveMinionIdentities[i].gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id));
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x000C8552 File Offset: 0x000C6752
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
