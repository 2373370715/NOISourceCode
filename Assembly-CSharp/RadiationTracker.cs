using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000B82 RID: 2946
public class RadiationTracker : WorldTracker
{
	// Token: 0x0600374A RID: 14154 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x00223A9C File Offset: 0x00221C9C
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(base.WorldID, false);
		if (worldItems.Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity cmp in worldItems)
		{
			num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
		}
		float value = num / (float)worldItems.Count;
		base.AddPoint(value);
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x000C85F0 File Offset: 0x000C67F0
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
