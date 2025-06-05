using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001003 RID: 4099
public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x060052A5 RID: 21157 RVA: 0x00283BE0 File Offset: 0x00281DE0
	public float OxygenAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<SuitTank>().PercentFull();
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x060052A6 RID: 21158 RVA: 0x00283C10 File Offset: 0x00281E10
	public float BatteryAvailable
	{
		get
		{
			KPrefabID storedOutfit = this.GetStoredOutfit();
			if (storedOutfit == null)
			{
				return 0f;
			}
			return storedOutfit.GetComponent<LeadSuitTank>().batteryCharge;
		}
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x00283C40 File Offset: 0x00281E40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), base.gameObject);
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits, true);
	}

	// Token: 0x060052A8 RID: 21160 RVA: 0x00283CC4 File Offset: 0x00281EC4
	public KPrefabID GetStoredOutfit()
	{
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!(component == null) && component.IsAnyPrefabID(this.OutfitTags))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x060052A9 RID: 21161 RVA: 0x00283D44 File Offset: 0x00281F44
	public float GetSuitScore()
	{
		float num = -1f;
		KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
		if (partiallyChargedOutfit)
		{
			num = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
			JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
			if (component && component.PercentFull() < num)
			{
				num = component.PercentFull();
			}
		}
		return num;
	}

	// Token: 0x060052AA RID: 21162 RVA: 0x00283D94 File Offset: 0x00281F94
	public KPrefabID GetPartiallyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (storedOutfit.GetComponent<SuitTank>().PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && component.PercentFull() < TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x060052AB RID: 21163 RVA: 0x00283DE8 File Offset: 0x00281FE8
	public KPrefabID GetFullyChargedOutfit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!storedOutfit)
		{
			return null;
		}
		if (!storedOutfit.GetComponent<SuitTank>().IsFull())
		{
			return null;
		}
		JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
		if (component && !component.IsFull())
		{
			return null;
		}
		return storedOutfit;
	}

	// Token: 0x060052AC RID: 21164 RVA: 0x00283E30 File Offset: 0x00282030
	private void CreateFetchChore()
	{
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.EquipmentFetch, base.GetComponent<Storage>(), 1f, new HashSet<Tag>(this.OutfitTags), FetchChore.MatchCriteria.MatchID, Tag.Invalid, new Tag[]
		{
			GameTags.Assigned
		}, null, true, null, null, null, Operational.State.None, 0);
		this.fetchChore.allowMultifetch = false;
	}

	// Token: 0x060052AD RID: 21165 RVA: 0x000DA510 File Offset: 0x000D8710
	private void CancelFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
			this.fetchChore = null;
		}
	}

	// Token: 0x060052AE RID: 21166 RVA: 0x00283E98 File Offset: 0x00282098
	public bool HasOxygen()
	{
		GameObject oxygen = this.GetOxygen();
		return oxygen != null && oxygen.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x00283ECC File Offset: 0x002820CC
	private void RefreshMeter()
	{
		GameObject oxygen = this.GetOxygen();
		float num = 0f;
		if (oxygen != null)
		{
			num = oxygen.GetComponent<PrimaryElement>().Mass / base.GetComponent<ConduitConsumer>().capacityKG;
			num = Math.Min(num, 1f);
		}
		this.meter.SetPositionPercent(num);
	}

	// Token: 0x060052B0 RID: 21168 RVA: 0x00283F20 File Offset: 0x00282120
	public bool IsSuitFullyCharged()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (!(storedOutfit != null))
		{
			return false;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		if (component != null && component.PercentFull() < 1f)
		{
			return false;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		if (component2 != null && component2.PercentFull() < 1f)
		{
			return false;
		}
		LeadSuitTank leadSuitTank = (storedOutfit != null) ? storedOutfit.GetComponent<LeadSuitTank>() : null;
		return !(leadSuitTank != null) || leadSuitTank.PercentFull() >= 1f;
	}

	// Token: 0x060052B1 RID: 21169 RVA: 0x00283FAC File Offset: 0x002821AC
	public bool IsOxygenTankFull()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= 1f;
		}
		return false;
	}

	// Token: 0x060052B2 RID: 21170 RVA: 0x000DA531 File Offset: 0x000D8731
	private void OnRequestOutfit()
	{
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x000DA551 File Offset: 0x000D8751
	private void OnCancelRequest()
	{
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x060052B4 RID: 21172 RVA: 0x00283FF0 File Offset: 0x002821F0
	public void DropSuit()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
	}

	// Token: 0x060052B5 RID: 21173 RVA: 0x00284024 File Offset: 0x00282224
	public void EquipTo(Equipment equipment)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		base.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
		Prioritizable component = storedOutfit.GetComponent<Prioritizable>();
		PrioritySetting masterPriority = component.GetMasterPriority();
		PrioritySetting masterPriority2 = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		if (component != null && component.GetMasterPriority().priority_class == PriorityScreen.PriorityClass.topPriority)
		{
			component.SetMasterPriority(masterPriority2);
		}
		storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
		storedOutfit.GetComponent<EquippableWorkable>().CancelChore("Manual equip");
		if (component != null && component.GetMasterPriority() != masterPriority)
		{
			component.SetMasterPriority(masterPriority);
		}
		equipment.Equip(storedOutfit.GetComponent<Equippable>());
		this.returnSuitWorkable.CreateChore();
	}

	// Token: 0x060052B6 RID: 21174 RVA: 0x002840E0 File Offset: 0x002822E0
	public void UnequipFrom(Equipment equipment)
	{
		Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
		assignable.Unassign();
		Durability component = assignable.GetComponent<Durability>();
		if (component != null && component.IsWornOut())
		{
			this.ConfigRequestSuit();
			return;
		}
		base.GetComponent<Storage>().Store(assignable.gameObject, false, false, true, false);
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x000DA571 File Offset: 0x000D8771
	public void ConfigRequestSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(true, base.smi, false);
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x000DA5AF File Offset: 0x000D87AF
	public void ConfigNoSuit()
	{
		base.smi.sm.isConfigured.Set(true, base.smi, false);
		base.smi.sm.isWaitingForSuit.Set(false, base.smi, false);
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x00284140 File Offset: 0x00282340
	public bool CanDropOffSuit()
	{
		return base.smi.sm.isConfigured.Get(base.smi) && !base.smi.sm.isWaitingForSuit.Get(base.smi) && this.GetStoredOutfit() == null;
	}

	// Token: 0x060052BA RID: 21178 RVA: 0x000DA5ED File Offset: 0x000D87ED
	private GameObject GetOxygen()
	{
		return base.GetComponent<Storage>().FindFirst(GameTags.Oxygen);
	}

	// Token: 0x060052BB RID: 21179 RVA: 0x00284198 File Offset: 0x00282398
	private void ChargeSuit(float dt)
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject oxygen = this.GetOxygen();
		if (oxygen == null)
		{
			return;
		}
		SuitTank component = storedOutfit.GetComponent<SuitTank>();
		float num = component.capacity * 15f * dt / 600f;
		num = Mathf.Min(num, component.capacity - component.GetTankAmount());
		num = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, num);
		if (num > 0f)
		{
			base.GetComponent<Storage>().Transfer(component.storage, component.elementTag, num, false, true);
		}
	}

	// Token: 0x060052BC RID: 21180 RVA: 0x0028422C File Offset: 0x0028242C
	public void SetSuitMarker(SuitMarker suit_marker)
	{
		SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
		if (suit_marker == null)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
		}
		else if (suit_marker.transform.GetPosition().x > base.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (suit_marker.transform.GetPosition().x < base.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
		}
		else if (!suit_marker.GetComponent<Operational>().IsOperational)
		{
			suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
		}
		if (suitMarkerState != this.suitMarkerState)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, false);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, false);
			switch (suitMarkerState)
			{
			case SuitLocker.SuitMarkerState.NoMarker:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker, null);
				break;
			case SuitLocker.SuitMarkerState.WrongSide:
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide, null);
				break;
			}
			this.suitMarkerState = suitMarkerState;
		}
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x000DA5FF File Offset: 0x000D87FF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(base.transform.position), null);
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x00284358 File Offset: 0x00282558
	private static void GatherSuitBuildings(int cell, int dir, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		int num = dir;
		for (;;)
		{
			int cell2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(cell2) && !SuitLocker.GatherSuitBuildingsOnCell(cell2, suit_lockers, suit_markers))
			{
				break;
			}
			num += dir;
		}
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x00284388 File Offset: 0x00282588
	private static bool GatherSuitBuildingsOnCell(int cell, List<SuitLocker.SuitLockerEntry> suit_lockers, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return false;
		}
		SuitMarker component = gameObject.GetComponent<SuitMarker>();
		if (component != null)
		{
			suit_markers.Add(new SuitLocker.SuitMarkerEntry
			{
				suitMarker = component,
				cell = cell
			});
			return true;
		}
		SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
		if (component2 != null)
		{
			suit_lockers.Add(new SuitLocker.SuitLockerEntry
			{
				suitLocker = component2,
				cell = cell
			});
			return true;
		}
		return false;
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x00284414 File Offset: 0x00282614
	private static SuitMarker FindSuitMarker(int cell, List<SuitLocker.SuitMarkerEntry> suit_markers)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		foreach (SuitLocker.SuitMarkerEntry suitMarkerEntry in suit_markers)
		{
			if (suitMarkerEntry.cell == cell)
			{
				return suitMarkerEntry.suitMarker;
			}
		}
		return null;
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x0028447C File Offset: 0x0028267C
	public static void UpdateSuitMarkerStates(int cell, GameObject self)
	{
		ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
		ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList pooledList2 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
		if (self != null)
		{
			SuitLocker component = self.GetComponent<SuitLocker>();
			if (component != null)
			{
				pooledList.Add(new SuitLocker.SuitLockerEntry
				{
					suitLocker = component,
					cell = cell
				});
			}
			SuitMarker component2 = self.GetComponent<SuitMarker>();
			if (component2 != null)
			{
				pooledList2.Add(new SuitLocker.SuitMarkerEntry
				{
					suitMarker = component2,
					cell = cell
				});
			}
		}
		SuitLocker.GatherSuitBuildings(cell, 1, pooledList, pooledList2);
		SuitLocker.GatherSuitBuildings(cell, -1, pooledList, pooledList2);
		pooledList.Sort(SuitLocker.SuitLockerEntry.comparer);
		for (int i = 0; i < pooledList.Count; i++)
		{
			SuitLocker.SuitLockerEntry suitLockerEntry = pooledList[i];
			SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry;
			ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList3 = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
			pooledList3.Add(suitLockerEntry);
			for (int j = i + 1; j < pooledList.Count; j++)
			{
				SuitLocker.SuitLockerEntry suitLockerEntry3 = pooledList[j];
				if (Grid.CellRight(suitLockerEntry2.cell) != suitLockerEntry3.cell)
				{
					break;
				}
				i++;
				suitLockerEntry2 = suitLockerEntry3;
				pooledList3.Add(suitLockerEntry3);
			}
			int cell2 = Grid.CellLeft(suitLockerEntry.cell);
			int cell3 = Grid.CellRight(suitLockerEntry2.cell);
			SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell2, pooledList2);
			if (suitMarker == null)
			{
				suitMarker = SuitLocker.FindSuitMarker(cell3, pooledList2);
			}
			foreach (SuitLocker.SuitLockerEntry suitLockerEntry4 in pooledList3)
			{
				suitLockerEntry4.suitLocker.SetSuitMarker(suitMarker);
			}
			pooledList3.Recycle();
		}
		pooledList.Recycle();
		pooledList2.Recycle();
	}

	// Token: 0x04003A5D RID: 14941
	[MyCmpGet]
	private Building building;

	// Token: 0x04003A5E RID: 14942
	public Tag[] OutfitTags;

	// Token: 0x04003A5F RID: 14943
	private FetchChore fetchChore;

	// Token: 0x04003A60 RID: 14944
	[MyCmpAdd]
	public SuitLocker.ReturnSuitWorkable returnSuitWorkable;

	// Token: 0x04003A61 RID: 14945
	private MeterController meter;

	// Token: 0x04003A62 RID: 14946
	private SuitLocker.SuitMarkerState suitMarkerState;

	// Token: 0x02001004 RID: 4100
	[AddComponentMenu("KMonoBehaviour/Workable/ReturnSuitWorkable")]
	public class ReturnSuitWorkable : Workable
	{
		// Token: 0x060052C3 RID: 21187 RVA: 0x000DA625 File Offset: 0x000D8825
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.workTime = 0.25f;
			this.synchronizeAnims = false;
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x00284630 File Offset: 0x00282830
		public void CreateChore()
		{
			if (this.urgentChore == null)
			{
				SuitLocker component = base.GetComponent<SuitLocker>();
				this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
				this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent, null);
				this.urgentChore.AddPrecondition(this.HasSuitMarker, component);
				this.urgentChore.AddPrecondition(this.SuitTypeMatchesLocker, component);
				this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, this, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.idle, 5, false, false);
				this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle, null);
				this.idleChore.AddPrecondition(this.HasSuitMarker, component);
				this.idleChore.AddPrecondition(this.SuitTypeMatchesLocker, component);
			}
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x000DA646 File Offset: 0x000D8846
		public void CancelChore()
		{
			if (this.urgentChore != null)
			{
				this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.urgentChore = null;
			}
			if (this.idleChore != null)
			{
				this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
				this.idleChore = null;
			}
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x000DA686 File Offset: 0x000D8886
		protected override void OnStartWork(WorkerBase worker)
		{
			base.ShowProgressBar(false);
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			return true;
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x00284714 File Offset: 0x00282914
		protected override void OnCompleteWork(WorkerBase worker)
		{
			Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
			if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
			{
				if (base.GetComponent<SuitLocker>().CanDropOffSuit())
				{
					base.GetComponent<SuitLocker>().UnequipFrom(equipment);
				}
				else
				{
					equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
				}
			}
			if (this.urgentChore != null)
			{
				this.CancelChore();
				this.CreateChore();
			}
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x000DA68F File Offset: 0x000D888F
		public override HashedString[] GetWorkAnims(WorkerBase worker)
		{
			return new HashedString[]
			{
				new HashedString("none")
			};
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x00284790 File Offset: 0x00282990
		public ReturnSuitWorkable()
		{
			Chore.Precondition precondition = default(Chore.Precondition);
			precondition.id = "IsValid";
			precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
			precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return ((SuitLocker)data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker;
			};
			this.HasSuitMarker = precondition;
			precondition = default(Chore.Precondition);
			precondition.id = "IsValid";
			precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER;
			precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				SuitLocker suitLocker = (SuitLocker)data;
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				return !(slot.assignable == null) && slot.assignable.GetComponent<KPrefabID>().IsAnyPrefabID(suitLocker.OutfitTags);
			};
			this.SuitTypeMatchesLocker = precondition;
			base..ctor();
		}

		// Token: 0x04003A63 RID: 14947
		public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingUrgent",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				if (component == null || !component.isEquipped)
				{
					return false;
				}
				SuitTank component2 = slot.assignable.GetComponent<SuitTank>();
				if (component2 != null && component2.NeedsRecharging())
				{
					return true;
				}
				JetSuitTank component3 = slot.assignable.GetComponent<JetSuitTank>();
				if (component3 != null && component3.NeedsRecharging())
				{
					return true;
				}
				LeadSuitTank component4 = slot.assignable.GetComponent<LeadSuitTank>();
				return component4 != null && component4.NeedsRecharging();
			}
		};

		// Token: 0x04003A64 RID: 14948
		public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition
		{
			id = "DoesSuitNeedRechargingIdle",
			description = DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Equipment equipment = context.consumerState.equipment;
				if (equipment == null)
				{
					return false;
				}
				AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
				if (slot.assignable == null)
				{
					return false;
				}
				Equippable component = slot.assignable.GetComponent<Equippable>();
				return !(component == null) && component.isEquipped && (slot.assignable.GetComponent<SuitTank>() != null || slot.assignable.GetComponent<JetSuitTank>() != null || slot.assignable.GetComponent<LeadSuitTank>() != null);
			}
		};

		// Token: 0x04003A65 RID: 14949
		public Chore.Precondition HasSuitMarker;

		// Token: 0x04003A66 RID: 14950
		public Chore.Precondition SuitTypeMatchesLocker;

		// Token: 0x04003A67 RID: 14951
		private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;

		// Token: 0x04003A68 RID: 14952
		private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;
	}

	// Token: 0x02001006 RID: 4102
	public class StatesInstance : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance
	{
		// Token: 0x060052D2 RID: 21202 RVA: 0x000DA6C4 File Offset: 0x000D88C4
		public StatesInstance(SuitLocker suit_locker) : base(suit_locker)
		{
		}
	}

	// Token: 0x02001007 RID: 4103
	public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
	{
		// Token: 0x060052D3 RID: 21203 RVA: 0x00284AC4 File Offset: 0x00282CC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).ParamTransition<bool>(this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CreateChore();
			}).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.returnSuitWorkable.CancelChore();
			}).PlayAnim("no_suit_pre").QueueAnim("no_suit", false, null);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state = this.empty.notconfigured.ParamTransition<bool>(this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue);
			string name = BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME;
			string tooltip = BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP;
			string icon = "status_item_no_filter_set";
			StatusItem.IconType icon_type = StatusItem.IconType.Custom;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state2 = this.empty.configured.RefreshUserMenuOnEnter();
			string name2 = BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state3 = this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, this.charging, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null).Enter("CreateFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).ParamTransition<bool>(this.isWaitingForSuit, this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit", false, null).Exit("ClearIsWaitingForSuit", delegate(SuitLocker.StatesInstance smi)
			{
				this.isWaitingForSuit.Set(false, smi, false);
			}).Exit("CancelFetchChore", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.CancelFetchChore();
			});
			string name3 = BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME;
			string tooltip3 = BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP;
			string icon3 = "";
			StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
			NotificationType notification_type3 = NotificationType.Neutral;
			bool allow_multiples3 = false;
			main = Db.Get().StatusItemCategories.Main;
			state3.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
			this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject).Enter(delegate(SuitLocker.StatesInstance smi)
			{
				KAnim.Build.Symbol symbol = smi.master.GetStoredOutfit().GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("suit");
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride("suit_swap", 0);
				if (symbol != null)
				{
					component.AddSymbolOverride("suit_swap", symbol, 0);
				}
			});
			this.charging.pre.Enter(delegate(SuitLocker.StatesInstance smi)
			{
				if (smi.master.IsSuitFullyCharged())
				{
					smi.GoTo(this.suitfullycharged);
					return;
				}
				smi.GetComponent<KBatchedAnimController>().Play("no_suit_pst", KAnim.PlayMode.Once, 1f, 0f);
				smi.GetComponent<KBatchedAnimController>().Queue("charging_pre", KAnim.PlayMode.Once, 1f, 0f);
			}).OnAnimQueueComplete(this.charging.operational);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state4 = this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (SuitLocker.StatesInstance smi) => !smi.master.HasOxygen(), UpdateRate.SIM_200ms).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).Update("ChargeSuit", delegate(SuitLocker.StatesInstance smi, float dt)
			{
				smi.master.ChargeSuit(dt);
			}, UpdateRate.SIM_200ms, false).Exit("ClearActive", delegate(SuitLocker.StatesInstance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			string name4 = BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME;
			string tooltip4 = BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP;
			string icon4 = "";
			StatusItem.IconType icon_type4 = StatusItem.IconType.Info;
			NotificationType notification_type4 = NotificationType.Neutral;
			bool allow_multiples4 = false;
			main = Db.Get().StatusItemCategories.Main;
			state4.ToggleStatusItem(name4, tooltip4, icon4, icon_type4, notification_type4, allow_multiples4, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state5 = this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (SuitLocker.StatesInstance smi) => smi.master.HasOxygen(), UpdateRate.SIM_200ms).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop);
			string name5 = BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME;
			string tooltip5 = BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP;
			string icon5 = "status_item_suit_locker_no_oxygen";
			StatusItem.IconType icon_type5 = StatusItem.IconType.Custom;
			NotificationType notification_type5 = NotificationType.BadMinor;
			bool allow_multiples5 = false;
			main = Db.Get().StatusItemCategories.Main;
			state5.ToggleStatusItem(name5, tooltip5, icon5, icon_type5, notification_type5, allow_multiples5, default(HashedString), 129022, null, null, main);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state6 = this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (SuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			string name6 = BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME;
			string tooltip6 = BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP;
			string icon6 = "";
			StatusItem.IconType icon_type6 = StatusItem.IconType.Info;
			NotificationType notification_type6 = NotificationType.Neutral;
			bool allow_multiples6 = false;
			main = Db.Get().StatusItemCategories.Main;
			state6.ToggleStatusItem(name6, tooltip6, icon6, icon_type6, notification_type6, allow_multiples6, default(HashedString), 129022, null, null, main);
			this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
			GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state7 = this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, this.empty, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).PlayAnim("has_suit").RefreshUserMenuOnEnter().ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (SuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit().gameObject);
			string name7 = BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME;
			string tooltip7 = BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP;
			string icon7 = "";
			StatusItem.IconType icon_type7 = StatusItem.IconType.Info;
			NotificationType notification_type7 = NotificationType.Neutral;
			bool allow_multiples7 = false;
			main = Db.Get().StatusItemCategories.Main;
			state7.ToggleStatusItem(name7, tooltip7, icon7, icon_type7, notification_type7, allow_multiples7, default(HashedString), 129022, null, null, main);
		}

		// Token: 0x04003A6C RID: 14956
		public SuitLocker.States.EmptyStates empty;

		// Token: 0x04003A6D RID: 14957
		public SuitLocker.States.ChargingStates charging;

		// Token: 0x04003A6E RID: 14958
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;

		// Token: 0x04003A6F RID: 14959
		public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;

		// Token: 0x04003A70 RID: 14960
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;

		// Token: 0x04003A71 RID: 14961
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;

		// Token: 0x04003A72 RID: 14962
		public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

		// Token: 0x02001008 RID: 4104
		public class ChargingStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x04003A73 RID: 14963
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;

			// Token: 0x04003A74 RID: 14964
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;

			// Token: 0x04003A75 RID: 14965
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;

			// Token: 0x04003A76 RID: 14966
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;

			// Token: 0x04003A77 RID: 14967
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
		}

		// Token: 0x02001009 RID: 4105
		public class EmptyStates : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
		{
			// Token: 0x04003A78 RID: 14968
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;

			// Token: 0x04003A79 RID: 14969
			public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
		}
	}

	// Token: 0x0200100B RID: 4107
	private enum SuitMarkerState
	{
		// Token: 0x04003A90 RID: 14992
		HasMarker,
		// Token: 0x04003A91 RID: 14993
		NoMarker,
		// Token: 0x04003A92 RID: 14994
		WrongSide,
		// Token: 0x04003A93 RID: 14995
		NotOperational
	}

	// Token: 0x0200100C RID: 4108
	private struct SuitLockerEntry
	{
		// Token: 0x04003A94 RID: 14996
		public SuitLocker suitLocker;

		// Token: 0x04003A95 RID: 14997
		public int cell;

		// Token: 0x04003A96 RID: 14998
		public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();

		// Token: 0x0200100D RID: 4109
		public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
		{
			// Token: 0x060052F0 RID: 21232 RVA: 0x000DA7E9 File Offset: 0x000D89E9
			public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b)
			{
				return a.cell - b.cell;
			}
		}
	}

	// Token: 0x0200100E RID: 4110
	private struct SuitMarkerEntry
	{
		// Token: 0x04003A97 RID: 14999
		public SuitMarker suitMarker;

		// Token: 0x04003A98 RID: 15000
		public int cell;
	}
}
