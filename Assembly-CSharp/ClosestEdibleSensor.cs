using System;
using System.Collections.Generic;

// Token: 0x02000847 RID: 2119
public class ClosestEdibleSensor : Sensor
{
	// Token: 0x0600256A RID: 9578 RVA: 0x000BCE2D File Offset: 0x000BB02D
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x001D94DC File Offset: 0x001D76DC
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, ClosestEdibleSensor.requiredSearchTags);
		bool flag = this.edibleInReachButNotPermitted;
		Edible x = null;
		bool flag2 = false;
		if (pickupable != null)
		{
			x = pickupable.GetComponent<Edible>();
			flag2 = true;
			flag = false;
		}
		else
		{
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), ClosestEdibleSensor.requiredSearchTags) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x000BCE36 File Offset: 0x000BB036
	public Edible GetEdible()
	{
		return this.edible;
	}

	// Token: 0x040019C5 RID: 6597
	private Edible edible;

	// Token: 0x040019C6 RID: 6598
	private bool hasEdible;

	// Token: 0x040019C7 RID: 6599
	public bool edibleInReachButNotPermitted;

	// Token: 0x040019C8 RID: 6600
	public static Tag[] requiredSearchTags = new Tag[]
	{
		GameTags.Edible
	};
}
