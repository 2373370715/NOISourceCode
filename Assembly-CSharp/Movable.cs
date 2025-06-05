using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AD1 RID: 2769
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Movable")]
public class Movable : Workable
{
	// Token: 0x17000210 RID: 528
	// (get) Token: 0x060032B8 RID: 12984 RVA: 0x000C55DC File Offset: 0x000C37DC
	public bool IsMarkedForMove
	{
		get
		{
			return this.isMarkedForMove;
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x060032B9 RID: 12985 RVA: 0x000C55E4 File Offset: 0x000C37E4
	public Storage StorageProxy
	{
		get
		{
			if (this.storageProxy == null)
			{
				return null;
			}
			return this.storageProxy.Get();
		}
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000C55FB File Offset: 0x000C37FB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x00211CD4 File Offset: 0x0020FED4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				if (this.reachableChangedHandle < 0)
				{
					this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.storageReachableChangedHandle < 0)
				{
					this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				}
				if (this.cancelHandle < 0)
				{
					this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
				}
				if (this.tagsChangedHandle < 0)
				{
					this.tagsChangedHandle = base.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
				}
				base.gameObject.AddTag(GameTags.MarkedForMove);
			}
			else
			{
				this.isMarkedForMove = false;
			}
		}
		if (Movable.IsCritterPickupable(base.gameObject))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
			this.UpdateStatusItem();
		}
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x00211E14 File Offset: 0x00210014
	private void OnReachableChanged(object data)
	{
		if (this.isMarkedForMove)
		{
			if (this.StorageProxy != null)
			{
				int num = Grid.PosToCell(this.pickupable);
				int num2 = Grid.PosToCell(this.StorageProxy);
				if (num != num2)
				{
					bool flag = MinionGroupProber.Get().IsReachable(num, OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(num2, OffsetGroups.Standard);
					if (this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Confined))
					{
						flag = false;
					}
					KSelectable component = base.GetComponent<KSelectable>();
					this.pendingMoveGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MarkedForMove, this.pendingMoveGuid, flag, this);
					this.storageUnreachableGuid = component.ToggleStatusItem(Db.Get().MiscStatusItems.MoveStorageUnreachable, this.storageUnreachableGuid, !flag, this);
					return;
				}
			}
			else
			{
				this.ClearMove();
			}
		}
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x00211EF4 File Offset: 0x002100F4
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable != null)
		{
			Movable component = pickupable.GetComponent<Movable>();
			if (component.isMarkedForMove)
			{
				this.storageProxy = new Ref<Storage>(component.StorageProxy);
				this.MarkForMove();
			}
		}
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x000C5633 File Offset: 0x000C3833
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isMarkedForMove && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().RemoveMovable(this);
			this.ClearStorageProxy();
		}
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x000C5668 File Offset: 0x000C3868
	private void CleanupMove(object data)
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x000C5689 File Offset: 0x000C3889
	private void OnTagsChanged(object data)
	{
		if (this.isMarkedForMove && !this.HasTagRequiredToMove() && this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x00211F38 File Offset: 0x00210138
	public void ClearMove()
	{
		if (this.isMarkedForMove)
		{
			this.isMarkedForMove = false;
			KSelectable component = base.GetComponent<KSelectable>();
			this.pendingMoveGuid = component.RemoveStatusItem(this.pendingMoveGuid, false);
			this.storageUnreachableGuid = component.RemoveStatusItem(this.storageUnreachableGuid, false);
			this.ClearStorageProxy();
			base.gameObject.RemoveTag(GameTags.MarkedForMove);
			if (this.reachableChangedHandle != -1)
			{
				base.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
				this.reachableChangedHandle = -1;
			}
			if (this.cancelHandle != -1)
			{
				base.Unsubscribe(2127324410, new Action<object>(this.CleanupMove));
				this.cancelHandle = -1;
			}
			if (this.tagsChangedHandle != -1)
			{
				base.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
				this.tagsChangedHandle = -1;
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x000C56BA File Offset: 0x000C38BA
	private void ClearStorageProxy()
	{
		if (this.storageReachableChangedHandle != -1)
		{
			this.StorageProxy.Unsubscribe(-1432940121, new Action<object>(this.OnReachableChanged));
			this.storageReachableChangedHandle = -1;
		}
		this.storageProxy = null;
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x000C56EF File Offset: 0x000C38EF
	private void OnClickMove()
	{
		MoveToLocationTool.Instance.Activate(this);
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x000C5668 File Offset: 0x000C3868
	private void OnClickCancel()
	{
		if (this.StorageProxy != null)
		{
			this.StorageProxy.GetComponent<CancellableMove>().OnCancel(this);
		}
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x00212018 File Offset: 0x00210218
	private void OnRefreshUserMenu(object data)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored) || !this.HasTagRequiredToMove())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForMove ? new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME, new System.Action(this.OnClickMove), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060032C6 RID: 12998 RVA: 0x000C56FC File Offset: 0x000C38FC
	private bool HasTagRequiredToMove()
	{
		return this.tagRequiredForMove == Tag.Invalid || this.pickupable.KPrefabID.HasTag(this.tagRequiredForMove);
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x000C5728 File Offset: 0x000C3928
	public void MoveToLocation(int cell)
	{
		this.CreateStorageProxy(cell);
		this.MarkForMove();
		base.gameObject.Trigger(1122777325, base.gameObject);
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x002120D4 File Offset: 0x002102D4
	private void MarkForMove()
	{
		base.Trigger(2127324410, null);
		this.isMarkedForMove = true;
		this.OnReachableChanged(null);
		this.storageReachableChangedHandle = this.StorageProxy.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.reachableChangedHandle = base.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.StorageProxy.GetComponent<CancellableMove>().SetMovable(this);
		base.gameObject.AddTag(GameTags.MarkedForMove);
		this.cancelHandle = base.Subscribe(2127324410, new Action<object>(this.CleanupMove));
		this.tagsChangedHandle = base.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		this.UpdateStatusItem();
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x000C574D File Offset: 0x000C394D
	private void UpdateStatusItem()
	{
		if (Movable.IsCritterPickupable(base.gameObject))
		{
			this.shouldShowSkillPerkStatusItem = this.isMarkedForMove;
			base.UpdateStatusItem(null);
		}
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x000C576F File Offset: 0x000C396F
	public bool CanMoveTo(int cell)
	{
		return !Grid.IsSolidCell(cell) && Grid.IsWorldValidCell(cell) && base.gameObject.IsMyParentWorld(cell);
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x0021219C File Offset: 0x0021039C
	private void CreateStorageProxy(int cell)
	{
		if (this.storageProxy == null || this.storageProxy.Get() == null)
		{
			if (Grid.Objects[cell, 44] != null)
			{
				Storage component = Grid.Objects[cell, 44].GetComponent<Storage>();
				this.storageProxy = new Ref<Storage>(component);
				return;
			}
			Vector3 position = Grid.CellToPosCBC(cell, MoveToLocationTool.Instance.visualizerLayer);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MovePickupablePlacerConfig.ID), position);
			Storage component2 = gameObject.GetComponent<Storage>();
			gameObject.SetActive(true);
			this.storageProxy = new Ref<Storage>(component2);
		}
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x000C578F File Offset: 0x000C398F
	public static bool IsCritterPickupable(GameObject pickupable_go)
	{
		return pickupable_go.GetComponent<Capturable>();
	}

	// Token: 0x040022AE RID: 8878
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x040022AF RID: 8879
	public Tag tagRequiredForMove = Tag.Invalid;

	// Token: 0x040022B0 RID: 8880
	[Serialize]
	private bool isMarkedForMove;

	// Token: 0x040022B1 RID: 8881
	[Serialize]
	private Ref<Storage> storageProxy;

	// Token: 0x040022B2 RID: 8882
	private int storageReachableChangedHandle = -1;

	// Token: 0x040022B3 RID: 8883
	private int reachableChangedHandle = -1;

	// Token: 0x040022B4 RID: 8884
	private int cancelHandle = -1;

	// Token: 0x040022B5 RID: 8885
	private int tagsChangedHandle = -1;

	// Token: 0x040022B6 RID: 8886
	private Guid pendingMoveGuid;

	// Token: 0x040022B7 RID: 8887
	private Guid storageUnreachableGuid;

	// Token: 0x040022B8 RID: 8888
	public Action<GameObject> onDeliveryComplete;

	// Token: 0x040022B9 RID: 8889
	public Action<GameObject> onPickupComplete;
}
