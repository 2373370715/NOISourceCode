using System;
using UnityEngine;

// Token: 0x02000B79 RID: 2937
public class BreathabilityTracker : WorldTracker
{
	// Token: 0x0600372D RID: 14125 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public BreathabilityTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x002236B4 File Offset: 0x002218B4
	public override void UpdateData()
	{
		float num = 0f;
		if (Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false).Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false))
		{
			OxygenBreather component = minionIdentity.GetComponent<OxygenBreather>();
			if (!(component == null))
			{
				OxygenBreather.IGasProvider currentGasProvider = component.GetCurrentGasProvider();
				num2++;
				if (!component.IsOutOfOxygen)
				{
					num += 100f;
					if (currentGasProvider.IsLowOxygen())
					{
						num -= 50f;
					}
				}
			}
		}
		num /= (float)num2;
		base.AddPoint((float)Mathf.RoundToInt(num));
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x000C8552 File Offset: 0x000C6752
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
