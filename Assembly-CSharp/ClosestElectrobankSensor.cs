using System;
using System.Collections.Generic;

// Token: 0x02000848 RID: 2120
public class ClosestElectrobankSensor : ClosestPickupableSensor<Electrobank>
{
	// Token: 0x0600256E RID: 9582 RVA: 0x000BCE57 File Offset: 0x000BB057
	public ClosestElectrobankSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.ChargedPortableBattery, shouldStartActive)
	{
		this.bionicIncompatiobleElectrobankTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
		GameTags.BionicIncompatibleBatteries.CopyTo(this.bionicIncompatiobleElectrobankTags, 0);
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x001D959C File Offset: 0x001D779C
	public override HashSet<Tag> GetForbbidenTags()
	{
		HashSet<Tag> forbbidenTags = base.GetForbbidenTags();
		if (this.bionicIncompatiobleElectrobankTags != null && this.bionicIncompatiobleElectrobankTags.Length != 0)
		{
			HashSet<Tag> hashSet = forbbidenTags;
			foreach (Tag item in this.bionicIncompatiobleElectrobankTags)
			{
				if (!forbbidenTags.Contains(item))
				{
					hashSet.Add(item);
				}
			}
			return hashSet;
		}
		return forbbidenTags;
	}

	// Token: 0x040019C9 RID: 6601
	private Tag[] bionicIncompatiobleElectrobankTags;
}
