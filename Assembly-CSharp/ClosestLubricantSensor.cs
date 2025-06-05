using System;

// Token: 0x02000849 RID: 2121
public class ClosestLubricantSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x06002570 RID: 9584 RVA: 0x000BCE8C File Offset: 0x000BB08C
	public ClosestLubricantSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.SolidLubricant, shouldStartActive)
	{
	}
}
