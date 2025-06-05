using System;
using KSerialization;
using STRINGS;

// Token: 0x020011B8 RID: 4536
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06005C31 RID: 23601 RVA: 0x000E085E File Offset: 0x000DEA5E
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06005C32 RID: 23602 RVA: 0x000E0880 File Offset: 0x000DEA80
	public bool GetEntombed
	{
		get
		{
			return this.isEntombed;
		}
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x002A84D4 File Offset: 0x002A66D4
	public void SetStatusItem(StatusItem si)
	{
		bool flag = this.showStatusItemOnEntombed;
		this.SetShowStatusItemOnEntombed(false);
		this.EntombedStatusItem = si;
		this.SetShowStatusItemOnEntombed(flag);
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x002A8500 File Offset: 0x002A6700
	public void SetShowStatusItemOnEntombed(bool val)
	{
		this.showStatusItemOnEntombed = val;
		if (this.isEntombed && this.EntombedStatusItem != null)
		{
			if (this.showStatusItemOnEntombed)
			{
				this.selectable.AddStatusItem(this.EntombedStatusItem, null);
				return;
			}
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06005C35 RID: 23605 RVA: 0x000E0888 File Offset: 0x000DEA88
	public string WiltStateString
	{
		get
		{
			return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06005C36 RID: 23606 RVA: 0x000E08B3 File Offset: 0x000DEAB3
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Entombed
			};
		}
	}

	// Token: 0x06005C37 RID: 23607 RVA: 0x002A8554 File Offset: 0x002A6754
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.EntombedStatusItem == null)
		{
			this.EntombedStatusItem = this.DefaultEntombedStatusItem;
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.CheckEntombed();
		if (this.isEntombed)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			base.Trigger(-1089732772, true);
		}
	}

	// Token: 0x06005C38 RID: 23608 RVA: 0x000E08C0 File Offset: 0x000DEAC0
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005C39 RID: 23609 RVA: 0x000E08D8 File Offset: 0x000DEAD8
	private void OnSolidChanged(object data)
	{
		this.CheckEntombed();
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x002A85E8 File Offset: 0x002A67E8
	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!this.IsCellSafe(cell))
		{
			if (!this.isEntombed)
			{
				this.isEntombed = true;
				if (this.showStatusItemOnEntombed)
				{
					this.selectable.AddStatusItem(this.EntombedStatusItem, base.gameObject);
				}
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				base.Trigger(-1089732772, true);
			}
		}
		else if (this.isEntombed)
		{
			this.isEntombed = false;
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			base.Trigger(-1089732772, false);
		}
		if (this.operational != null)
		{
			this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
		}
	}

	// Token: 0x06005C3B RID: 23611 RVA: 0x000E08E0 File Offset: 0x000DEAE0
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, null, EntombVulnerable.IsCellSafeCBDelegate);
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x000E08F4 File Offset: 0x000DEAF4
	private static bool IsCellSafeCB(int cell, object data)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell];
	}

	// Token: 0x040041A9 RID: 16809
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040041AA RID: 16810
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040041AB RID: 16811
	private OccupyArea _occupyArea;

	// Token: 0x040041AC RID: 16812
	[Serialize]
	private bool isEntombed;

	// Token: 0x040041AD RID: 16813
	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	// Token: 0x040041AE RID: 16814
	[NonSerialized]
	private StatusItem EntombedStatusItem;

	// Token: 0x040041AF RID: 16815
	private bool showStatusItemOnEntombed = true;

	// Token: 0x040041B0 RID: 16816
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x040041B1 RID: 16817
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040041B2 RID: 16818
	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => EntombVulnerable.IsCellSafeCB(cell, data);
}
