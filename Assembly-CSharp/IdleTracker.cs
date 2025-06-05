using System;
using System.Collections.Generic;

// Token: 0x02000B81 RID: 2945
public class IdleTracker : WorldTracker
{
	// Token: 0x06003747 RID: 14151 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public IdleTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x00223A28 File Offset: 0x00221C28
	public override void UpdateData()
	{
		this.objectsOfInterest.Clear();
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
				this.objectsOfInterest.Add(worldItems[i].gameObject);
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x000C6C93 File Offset: 0x000C4E93
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
