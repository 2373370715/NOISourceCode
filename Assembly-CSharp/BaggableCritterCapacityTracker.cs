using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009C7 RID: 2503
public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000C1860 File Offset: 0x000BFA60
	// (set) Token: 0x06002CEA RID: 11498 RVA: 0x000C1868 File Offset: 0x000BFA68
	[Serialize]
	public int creatureLimit { get; set; } = 20;

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000C1871 File Offset: 0x000BFA71
	// (set) Token: 0x06002CEC RID: 11500 RVA: 0x000C1879 File Offset: 0x000BFA79
	public int storedCreatureCount { get; private set; }

	// Token: 0x06002CED RID: 11501 RVA: 0x001FAF2C File Offset: 0x001F912C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(this);
		this.cavityCell = Grid.OffsetCell(cell, this.cavityOffset);
		this.filter = base.GetComponent<TreeFilterable>();
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		base.Subscribe(144050788, new Action<object>(this.RefreshCreatureCount));
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x001FAFBC File Offset: 0x001F91BC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (BaggableCritterCapacityTracker.capacityStatusItem == null)
		{
			BaggableCritterCapacityTracker.capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			BaggableCritterCapacityTracker.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{StoredUnits}", ((int)userControlledCapacity.AmountStored == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS).Replace("{Capacity}", newValue2).Replace("{CapacityUnits}", ((int)userControlledCapacity.UserMaxCapacity == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS);
				return str;
			};
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, BaggableCritterCapacityTracker.capacityStatusItem, this);
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x000C1882 File Offset: 0x000BFA82
	protected override void OnCleanUp()
	{
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Unsubscribe(144050788);
		base.OnCleanUp();
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x001FB048 File Offset: 0x001F9248
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
		if (component == null)
		{
			return;
		}
		this.creatureLimit = component.creatureLimit;
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x001FB084 File Offset: 0x001F9284
	public void RefreshCreatureCount(object data = null)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cavityCell);
		int storedCreatureCount = this.storedCreatureCount;
		this.storedCreatureCount = 0;
		if (cavityForCell != null)
		{
			foreach (KPrefabID kprefabID in cavityForCell.creatures)
			{
				if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped) && (!this.filteredCount || this.filter.AcceptedTags.Contains(kprefabID.PrefabTag)))
				{
					int storedCreatureCount2 = this.storedCreatureCount;
					this.storedCreatureCount = storedCreatureCount2 + 1;
				}
			}
		}
		if (this.onCountChanged != null && this.storedCreatureCount != storedCreatureCount)
		{
			this.onCountChanged();
		}
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000C18BC File Offset: 0x000BFABC
	public void Sim1000ms(float dt)
	{
		this.RefreshCreatureCount(null);
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000C18C5 File Offset: 0x000BFAC5
	// (set) Token: 0x06002CF4 RID: 11508 RVA: 0x000C18CE File Offset: 0x000BFACE
	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return (float)this.creatureLimit;
		}
		set
		{
			this.creatureLimit = Mathf.RoundToInt(value);
			if (this.onCountChanged != null)
			{
				this.onCountChanged();
			}
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000C18EF File Offset: 0x000BFAEF
	float IUserControlledCapacity.AmountStored
	{
		get
		{
			return (float)this.storedCreatureCount;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	float IUserControlledCapacity.MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x000C18FF File Offset: 0x000BFAFF
	float IUserControlledCapacity.MaxCapacity
	{
		get
		{
			return (float)this.maximumCreatures;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	bool IUserControlledCapacity.WholeValues
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x000C1908 File Offset: 0x000BFB08
	LocString IUserControlledCapacity.CapacityUnits
	{
		get
		{
			return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
		}
	}

	// Token: 0x04001EC9 RID: 7881
	public int maximumCreatures = 40;

	// Token: 0x04001ECA RID: 7882
	public CellOffset cavityOffset;

	// Token: 0x04001ECB RID: 7883
	public bool filteredCount;

	// Token: 0x04001ECC RID: 7884
	public System.Action onCountChanged;

	// Token: 0x04001ECD RID: 7885
	private int cavityCell;

	// Token: 0x04001ECE RID: 7886
	[MyCmpReq]
	private TreeFilterable filter;

	// Token: 0x04001ECF RID: 7887
	private static StatusItem capacityStatusItem;
}
