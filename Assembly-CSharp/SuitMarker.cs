using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200100F RID: 4111
[AddComponentMenu("KMonoBehaviour/scripts/SuitMarker")]
public class SuitMarker : KMonoBehaviour
{
	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x060052F2 RID: 21234 RVA: 0x000DA7F8 File Offset: 0x000D89F8
	// (set) Token: 0x060052F3 RID: 21235 RVA: 0x000DA818 File Offset: 0x000D8A18
	private bool OnlyTraverseIfUnequipAvailable
	{
		get
		{
			DebugUtil.Assert(this.onlyTraverseIfUnequipAvailable == (this.gridFlags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) > (Grid.SuitMarker.Flags)0);
			return this.onlyTraverseIfUnequipAvailable;
		}
		set
		{
			this.onlyTraverseIfUnequipAvailable = value;
			this.UpdateGridFlag(Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable, this.onlyTraverseIfUnequipAvailable);
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x060052F4 RID: 21236 RVA: 0x000DA82E File Offset: 0x000D8A2E
	// (set) Token: 0x060052F5 RID: 21237 RVA: 0x000DA83B File Offset: 0x000D8A3B
	private bool isRotated
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Rotated, value);
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x060052F6 RID: 21238 RVA: 0x000DA845 File Offset: 0x000D8A45
	// (set) Token: 0x060052F7 RID: 21239 RVA: 0x000DA852 File Offset: 0x000D8A52
	private bool isOperational
	{
		get
		{
			return (this.gridFlags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		}
		set
		{
			this.UpdateGridFlag(Grid.SuitMarker.Flags.Operational, value);
		}
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x002852E4 File Offset: 0x002834E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnlyTraverseIfUnequipAvailable = this.onlyTraverseIfUnequipAvailable;
		global::Debug.Assert(this.interactAnim != null, "interactAnim is null");
		base.Subscribe<SuitMarker>(493375141, SuitMarker.OnRefreshUserMenuDelegate);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<SuitMarker>(-592767678, SuitMarker.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<SuitMarker>(-1643076535, SuitMarker.OnRotatedDelegate);
		this.CreateNewEquipReactable();
		this.CreateNewUnequipReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		base.GetComponent<KAnimControllerBase>().Play("no_suit", KAnim.PlayMode.Once, 1f, 0f);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
		this.RefreshTraverseIfUnequipStatusItem();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
	}

	// Token: 0x060052F9 RID: 21241 RVA: 0x000DA85C File Offset: 0x000D8A5C
	private void CreateNewEquipReactable()
	{
		this.equipReactable = new SuitMarker.EquipSuitReactable(this);
	}

	// Token: 0x060052FA RID: 21242 RVA: 0x000DA86A File Offset: 0x000D8A6A
	private void CreateNewUnequipReactable()
	{
		this.unequipReactable = new SuitMarker.UnequipSuitReactable(this);
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x002853E0 File Offset: 0x002835E0
	public void GetAttachedLockers(List<SuitLocker> suit_lockers)
	{
		int num = this.isRotated ? 1 : -1;
		int num2 = 1;
		for (;;)
		{
			int num3 = Grid.OffsetCell(this.cell, num2 * num, 0);
			GameObject gameObject = Grid.Objects[num3, 1];
			if (gameObject == null)
			{
				break;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!(component == null))
			{
				if (!component.IsAnyPrefabID(this.LockerTags))
				{
					break;
				}
				SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
				if (component2 == null)
				{
					break;
				}
				Operational component3 = gameObject.GetComponent<Operational>();
				if ((!(component3 != null) || component3.GetFlag(BuildingEnabledButton.EnabledFlag)) && !suit_lockers.Contains(component2))
				{
					suit_lockers.Add(component2);
				}
			}
			num2++;
		}
	}

	// Token: 0x060052FC RID: 21244 RVA: 0x000DA878 File Offset: 0x000D8A78
	public static bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell, Grid.SuitMarker.Flags flags)
	{
		return Grid.CellColumn(dest_cell) > Grid.CellColumn(source_cell) == ((flags & Grid.SuitMarker.Flags.Rotated) == (Grid.SuitMarker.Flags)0);
	}

	// Token: 0x060052FD RID: 21245 RVA: 0x000DA890 File Offset: 0x000D8A90
	public bool DoesTraversalDirectionRequireSuit(int source_cell, int dest_cell)
	{
		return SuitMarker.DoesTraversalDirectionRequireSuit(source_cell, dest_cell, this.gridFlags);
	}

	// Token: 0x060052FE RID: 21246 RVA: 0x00285490 File Offset: 0x00283690
	private void Update()
	{
		ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
		this.GetAttachedLockers(pooledList);
		int num = 0;
		int num2 = 0;
		KPrefabID x = null;
		foreach (SuitLocker suitLocker in pooledList)
		{
			if (suitLocker.CanDropOffSuit())
			{
				num++;
			}
			if (suitLocker.GetPartiallyChargedOutfit() != null)
			{
				num2++;
			}
			if (x == null)
			{
				x = suitLocker.GetStoredOutfit();
			}
		}
		pooledList.Recycle();
		bool flag = x != null;
		if (flag != this.hasAvailableSuit)
		{
			base.GetComponent<KAnimControllerBase>().Play(flag ? "off" : "no_suit", KAnim.PlayMode.Once, 1f, 0f);
			this.hasAvailableSuit = flag;
		}
		Grid.UpdateSuitMarker(this.cell, num2, num, this.gridFlags, this.PathFlag);
	}

	// Token: 0x060052FF RID: 21247 RVA: 0x00285584 File Offset: 0x00283784
	private void RefreshTraverseIfUnequipStatusItem()
	{
		if (this.OnlyTraverseIfUnequipAvailable)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, false);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalOnlyWhenRoomAvailable, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerTraversalAnytime, null);
	}

	// Token: 0x06005300 RID: 21248 RVA: 0x000DA89F File Offset: 0x000D8A9F
	private void OnEnableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = true;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x000DA8AE File Offset: 0x000D8AAE
	private void OnDisableTraverseIfUnequipAvailable()
	{
		this.OnlyTraverseIfUnequipAvailable = false;
		this.RefreshTraverseIfUnequipStatusItem();
	}

	// Token: 0x06005302 RID: 21250 RVA: 0x000DA8BD File Offset: 0x000D8ABD
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06005303 RID: 21251 RVA: 0x000DA8E1 File Offset: 0x000D8AE1
	private void OnOperationalChanged(bool isOperational)
	{
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		this.isOperational = isOperational;
	}

	// Token: 0x06005304 RID: 21252 RVA: 0x0028560C File Offset: 0x0028380C
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = (!this.OnlyTraverseIfUnequipAvailable) ? new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.NAME, new System.Action(this.OnEnableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ONLY_WHEN_ROOM_AVAILABLE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_clearance", UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.NAME, new System.Action(this.OnDisableTraverseIfUnequipAvailable), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.SUIT_MARKER_TRAVERSAL.ALWAYS.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005305 RID: 21253 RVA: 0x002856A8 File Offset: 0x002838A8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.equipReactable != null)
		{
			this.equipReactable.Cleanup();
		}
		if (this.unequipReactable != null)
		{
			this.unequipReactable.Cleanup();
		}
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x04003A99 RID: 15001
	[MyCmpGet]
	private Building building;

	// Token: 0x04003A9A RID: 15002
	private SuitMarker.SuitMarkerReactable equipReactable;

	// Token: 0x04003A9B RID: 15003
	private SuitMarker.SuitMarkerReactable unequipReactable;

	// Token: 0x04003A9C RID: 15004
	private bool hasAvailableSuit;

	// Token: 0x04003A9D RID: 15005
	[Serialize]
	private bool onlyTraverseIfUnequipAvailable;

	// Token: 0x04003A9E RID: 15006
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x04003A9F RID: 15007
	private int cell;

	// Token: 0x04003AA0 RID: 15008
	public Tag[] LockerTags;

	// Token: 0x04003AA1 RID: 15009
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x04003AA2 RID: 15010
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x04003AA3 RID: 15011
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003AA4 RID: 15012
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x04003AA5 RID: 15013
	private static readonly EventSystem.IntraObjectHandler<SuitMarker> OnRotatedDelegate = new EventSystem.IntraObjectHandler<SuitMarker>(delegate(SuitMarker component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x02001010 RID: 4112
	private class EquipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x06005308 RID: 21256 RVA: 0x000DA922 File Offset: 0x000D8B22
		public EquipSuitReactable(SuitMarker marker) : base("EquipSuitReactable", marker)
		{
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x000DA935 File Offset: 0x000D8B35
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			return !newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x000DA965 File Offset: 0x000D8B65
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewEquipReactable();
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x00285768 File Offset: 0x00283968
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return this.IsRocketDoorExitEquip(newReactor, transition) || flag == this.suitMarker.isRotated;
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x002857A0 File Offset: 0x002839A0
		private bool IsRocketDoorExitEquip(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.end != NavType.Teleport && transition.start != NavType.Teleport;
			return transition.navGridTransition.x == 0 && new_reactor.GetMyWorld().IsModuleInterior && !flag;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x002857E8 File Offset: 0x002839E8
		protected override void Run()
		{
			ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
			this.suitMarker.GetAttachedLockers(pooledList);
			SuitLocker suitLocker = null;
			for (int i = 0; i < pooledList.Count; i++)
			{
				float suitScore = pooledList[i].GetSuitScore();
				if (suitScore >= 1f)
				{
					suitLocker = pooledList[i];
					break;
				}
				if (suitLocker == null || suitScore > suitLocker.GetSuitScore())
				{
					suitLocker = pooledList[i];
				}
			}
			pooledList.Recycle();
			if (suitLocker != null)
			{
				Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
				SuitWearer.Instance smi = this.reactor.GetSMI<SuitWearer.Instance>();
				suitLocker.EquipTo(equipment);
				smi.UnreserveSuits();
				this.suitMarker.Update();
			}
		}
	}

	// Token: 0x02001011 RID: 4113
	private class UnequipSuitReactable : SuitMarker.SuitMarkerReactable
	{
		// Token: 0x0600530E RID: 21262 RVA: 0x000DA978 File Offset: 0x000D8B78
		public UnequipSuitReactable(SuitMarker marker) : base("UnequipSuitReactable", marker)
		{
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x00285898 File Offset: 0x00283A98
		public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			Navigator component = newReactor.GetComponent<Navigator>();
			return newReactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit) && component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None && base.InternalCanBegin(newReactor, transition);
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x000DA98B File Offset: 0x000D8B8B
		protected override void InternalBegin()
		{
			base.InternalBegin();
			this.suitMarker.CreateNewUnequipReactable();
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x00285900 File Offset: 0x00283B00
		protected override bool MovingTheRightWay(GameObject newReactor, Navigator.ActiveTransition transition)
		{
			bool flag = transition.navGridTransition.x < 0;
			return transition.navGridTransition.x != 0 && flag != this.suitMarker.isRotated;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x0028593C File Offset: 0x00283B3C
		protected override void Run()
		{
			Navigator component = this.reactor.GetComponent<Navigator>();
			Equipment equipment = this.reactor.GetComponent<MinionIdentity>().GetEquipment();
			if (component != null && (component.flags & this.suitMarker.PathFlag) > PathFinder.PotentialPath.Flags.None)
			{
				ListPool<SuitLocker, SuitMarker>.PooledList pooledList = ListPool<SuitLocker, SuitMarker>.Allocate();
				this.suitMarker.GetAttachedLockers(pooledList);
				SuitLocker suitLocker = null;
				int num = 0;
				while (suitLocker == null && num < pooledList.Count)
				{
					if (pooledList[num].CanDropOffSuit())
					{
						suitLocker = pooledList[num];
					}
					num++;
				}
				pooledList.Recycle();
				if (suitLocker != null)
				{
					suitLocker.UnequipFrom(equipment);
					component.GetSMI<SuitWearer.Instance>().UnreserveSuits();
					this.suitMarker.Update();
					return;
				}
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable != null)
			{
				assignable.Unassign();
				Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
				assignable.GetComponent<Notifier>().Add(notification, "");
			}
		}
	}

	// Token: 0x02001013 RID: 4115
	private abstract class SuitMarkerReactable : Reactable
	{
		// Token: 0x06005316 RID: 21270 RVA: 0x00285A7C File Offset: 0x00283C7C
		public SuitMarkerReactable(HashedString id, SuitMarker suit_marker) : base(suit_marker.gameObject, id, Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.suitMarker = suit_marker;
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x000DA9AA File Offset: 0x000D8BAA
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.suitMarker == null)
			{
				base.Cleanup();
				return false;
			}
			return this.suitMarker.isOperational && this.MovingTheRightWay(new_reactor, transition);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x00285AC8 File Offset: 0x00283CC8
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.suitMarker.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.suitMarker.HasTag(GameTags.JetSuitBlocker))
			{
				KBatchedAnimController component2 = this.suitMarker.GetComponent<KBatchedAnimController>();
				component2.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
				component2.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x00285BC0 File Offset: 0x00283DC0
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.suitMarker)
			{
				facing.SetFacing(this.suitMarker.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				if (this.reactor != null && this.suitMarker != null)
				{
					this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
					this.Run();
				}
				base.Cleanup();
			}
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x000DA9E9 File Offset: 0x000D8BE9
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.suitMarker.interactAnim);
			}
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void InternalCleanup()
		{
		}

		// Token: 0x0600531C RID: 21276
		protected abstract bool MovingTheRightWay(GameObject reactor, Navigator.ActiveTransition transition);

		// Token: 0x0600531D RID: 21277
		protected abstract void Run();

		// Token: 0x04003AA8 RID: 15016
		protected SuitMarker suitMarker;

		// Token: 0x04003AA9 RID: 15017
		protected float startTime;
	}
}
