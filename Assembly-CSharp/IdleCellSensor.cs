using System;
using UnityEngine;

// Token: 0x0200084D RID: 2125
public class IdleCellSensor : Sensor
{
	// Token: 0x0600257E RID: 9598 RVA: 0x000BCF77 File Offset: 0x000BB177
	public IdleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x001D98D8 File Offset: 0x001D7AD8
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60));
		this.navigator.RunQuery(idleCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = idleCellQuery.GetResultCell();
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000BCFA4 File Offset: 0x000BB1A4
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x040019D8 RID: 6616
	private MinionBrain brain;

	// Token: 0x040019D9 RID: 6617
	private Navigator navigator;

	// Token: 0x040019DA RID: 6618
	private KPrefabID prefabid;

	// Token: 0x040019DB RID: 6619
	private int cell;
}
