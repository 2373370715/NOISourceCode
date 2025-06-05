using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000851 RID: 2129
public class SafeCellSensor : Sensor
{
	// Token: 0x06002588 RID: 9608 RVA: 0x001D9A34 File Offset: 0x001D7C34
	private SafeCellQuery.SafeFlags GetIgnoredFlags()
	{
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		foreach (string key in this.ignoredFlagsSets.Keys)
		{
			SafeCellQuery.SafeFlags safeFlags2 = this.ignoredFlagsSets[key];
			safeFlags |= safeFlags2;
		}
		return safeFlags;
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000BD046 File Offset: 0x000BB246
	public void AddIgnoredFlagsSet(string setID, SafeCellQuery.SafeFlags flagsToIgnore)
	{
		if (this.ignoredFlagsSets.ContainsKey(setID))
		{
			this.ignoredFlagsSets[setID] = flagsToIgnore;
			return;
		}
		this.ignoredFlagsSets.Add(setID, flagsToIgnore);
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000BD071 File Offset: 0x000BB271
	public void RemoveIgnoredFlagsSet(string setID)
	{
		if (this.ignoredFlagsSets.ContainsKey(setID))
		{
			this.ignoredFlagsSets.Remove(setID);
		}
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x001D9A9C File Offset: 0x001D7C9C
	public SafeCellSensor(Sensors sensors, bool startEnabled = true) : base(sensors, startEnabled)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
		this.traits = base.GetComponent<Traits>();
	}

	// Token: 0x0600258C RID: 9612 RVA: 0x001D9AF8 File Offset: 0x001D7CF8
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		bool flag = this.HasSafeCell();
		this.RunSafeCellQuery(false);
		bool flag2 = this.HasSafeCell();
		if (flag2 != flag)
		{
			if (flag2)
			{
				this.sensors.Trigger(982561777, null);
				return;
			}
			this.sensors.Trigger(506919987, null);
		}
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000BD08E File Offset: 0x000BB28E
	public void RunSafeCellQuery(bool avoid_light)
	{
		this.cell = this.RunAndGetSafeCellQueryResult(avoid_light);
		if (this.cell == Grid.PosToCell(this.navigator))
		{
			this.cell = Grid.InvalidCell;
		}
	}

	// Token: 0x0600258E RID: 9614 RVA: 0x001D9B64 File Offset: 0x001D7D64
	public int RunAndGetSafeCellQueryResult(bool avoid_light)
	{
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light, this.GetIgnoredFlags());
		this.navigator.RunQuery(safeCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = safeCellQuery.GetResultCell();
		return this.cell;
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000BD0BB File Offset: 0x000BB2BB
	public int GetSensorCell()
	{
		return this.cell;
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x000BD0C3 File Offset: 0x000BB2C3
	public int GetCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(false);
		}
		return this.cell;
	}

	// Token: 0x06002591 RID: 9617 RVA: 0x000BD0DF File Offset: 0x000BB2DF
	public int GetSleepCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
		}
		return this.cell;
	}

	// Token: 0x06002592 RID: 9618 RVA: 0x000BD110 File Offset: 0x000BB310
	public bool HasSafeCell()
	{
		return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell(this.sensors);
	}

	// Token: 0x040019E2 RID: 6626
	private MinionBrain brain;

	// Token: 0x040019E3 RID: 6627
	private Navigator navigator;

	// Token: 0x040019E4 RID: 6628
	private KPrefabID prefabid;

	// Token: 0x040019E5 RID: 6629
	private Traits traits;

	// Token: 0x040019E6 RID: 6630
	private int cell = Grid.InvalidCell;

	// Token: 0x040019E7 RID: 6631
	private Dictionary<string, SafeCellQuery.SafeFlags> ignoredFlagsSets = new Dictionary<string, SafeCellQuery.SafeFlags>();
}
