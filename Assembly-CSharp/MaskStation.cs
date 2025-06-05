using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000EC7 RID: 3783
public class MaskStation : StateMachineComponent<MaskStation.SMInstance>, IBasicBuilding
{
	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06004B99 RID: 19353 RVA: 0x000D54BD File Offset: 0x000D36BD
	// (set) Token: 0x06004B9A RID: 19354 RVA: 0x000D54CA File Offset: 0x000D36CA
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

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06004B9B RID: 19355 RVA: 0x000D54D4 File Offset: 0x000D36D4
	// (set) Token: 0x06004B9C RID: 19356 RVA: 0x000D54E1 File Offset: 0x000D36E1
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

	// Token: 0x06004B9D RID: 19357 RVA: 0x0026D2D0 File Offset: 0x0026B4D0
	public void UpdateOperational()
	{
		bool flag = this.GetTotalOxygenAmount() >= this.oxygenConsumedPerMask * (float)this.maxUses;
		this.shouldPump = this.IsPumpable();
		if (this.operational.IsOperational && this.shouldPump && !flag)
		{
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidMaskStationConsumptionState, this.noElementStatusGuid, !this.shouldPump, null);
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x0026D368 File Offset: 0x0026B568
	private bool IsPumpable()
	{
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool result = false;
		foreach (ElementConsumer elementConsumer in components)
		{
			for (int j = 0; j < (int)elementConsumer.consumptionRadius; j++)
			{
				for (int k = 0; k < (int)elementConsumer.consumptionRadius; k++)
				{
					int num2 = num + k + Grid.WidthInCells * j;
					bool flag = Grid.Element[num2].IsState(Element.State.Gas);
					bool flag2 = Grid.Element[num2].id == elementConsumer.elementToConsume;
					if (flag && flag2)
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x0026D40C File Offset: 0x0026B60C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, null, false, fetch_chore_type);
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x0026D448 File Offset: 0x0026B648
	private List<GameObject> GetPossibleMaterials()
	{
		List<GameObject> result = new List<GameObject>();
		this.materialStorage.Find(this.materialTag, result);
		return result;
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x000D54EB File Offset: 0x000D36EB
	private float GetTotalMaterialAmount()
	{
		return this.materialStorage.GetMassAvailable(this.materialTag);
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x000D54FE File Offset: 0x000D36FE
	private float GetTotalOxygenAmount()
	{
		return this.oxygenStorage.GetMassAvailable(this.oxygenTag);
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x0026D470 File Offset: 0x0026B670
	private void RefreshMeters()
	{
		float num = this.GetTotalMaterialAmount();
		num = Mathf.Clamp01(num / ((float)this.maxUses * this.materialConsumedPerMask));
		float num2 = this.GetTotalOxygenAmount();
		num2 = Mathf.Clamp01(num2 / ((float)this.maxUses * this.oxygenConsumedPerMask));
		this.materialsMeter.SetPositionPercent(num);
		this.oxygenMeter.SetPositionPercent(num2);
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x0026D4D0 File Offset: 0x0026B6D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.CreateNewReactable();
		this.cell = Grid.PosToCell(this);
		Grid.RegisterSuitMarker(this.cell);
		this.isOperational = base.GetComponent<Operational>().IsOperational;
		base.Subscribe<MaskStation>(-592767678, MaskStation.OnOperationalChangedDelegate);
		this.isRotated = base.GetComponent<Rotatable>().IsRotated;
		base.Subscribe<MaskStation>(-1643076535, MaskStation.OnRotatedDelegate);
		this.materialsMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_resources_target", "meter_resources", this.materialsMeterOffset, Grid.SceneLayer.BuildingBack, new string[]
		{
			"meter_resources_target"
		});
		this.oxygenMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_oxygen_target", "meter_oxygen", this.oxygenMeterOffset, Grid.SceneLayer.BuildingFront, new string[]
		{
			"meter_oxygen_target"
		});
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		base.Subscribe<MaskStation>(-1697596308, MaskStation.OnStorageChangeDelegate);
		this.RefreshMeters();
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x0026D5DC File Offset: 0x0026B7DC
	private void Update()
	{
		float a = this.GetTotalMaterialAmount() / this.materialConsumedPerMask;
		float b = this.GetTotalOxygenAmount() / this.oxygenConsumedPerMask;
		int fullLockerCount = (int)Mathf.Min(a, b);
		int emptyLockerCount = 0;
		Grid.UpdateSuitMarker(this.cell, fullLockerCount, emptyLockerCount, this.gridFlags, this.PathFlag);
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x0026D628 File Offset: 0x0026B828
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		if (base.isSpawned)
		{
			Grid.UnregisterSuitMarker(this.cell);
		}
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x000D5511 File Offset: 0x000D3711
	private void OnOperationalChanged(bool isOperational)
	{
		this.isOperational = isOperational;
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x000D551A File Offset: 0x000D371A
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x000D5522 File Offset: 0x000D3722
	private void UpdateGridFlag(Grid.SuitMarker.Flags flag, bool state)
	{
		if (state)
		{
			this.gridFlags |= flag;
			return;
		}
		this.gridFlags &= ~flag;
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x000D5546 File Offset: 0x000D3746
	private void CreateNewReactable()
	{
		this.reactable = new MaskStation.OxygenMaskReactable(this);
	}

	// Token: 0x040034E2 RID: 13538
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040034E3 RID: 13539
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.OnOperationalChanged((bool)data);
	});

	// Token: 0x040034E4 RID: 13540
	private static readonly EventSystem.IntraObjectHandler<MaskStation> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MaskStation>(delegate(MaskStation component, object data)
	{
		component.isRotated = ((Rotatable)data).IsRotated;
	});

	// Token: 0x040034E5 RID: 13541
	public float materialConsumedPerMask = 1f;

	// Token: 0x040034E6 RID: 13542
	public float oxygenConsumedPerMask = 1f;

	// Token: 0x040034E7 RID: 13543
	public Tag materialTag = GameTags.Metal;

	// Token: 0x040034E8 RID: 13544
	public Tag oxygenTag = GameTags.Breathable;

	// Token: 0x040034E9 RID: 13545
	public int maxUses = 10;

	// Token: 0x040034EA RID: 13546
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040034EB RID: 13547
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040034EC RID: 13548
	public Storage materialStorage;

	// Token: 0x040034ED RID: 13549
	public Storage oxygenStorage;

	// Token: 0x040034EE RID: 13550
	private bool shouldPump;

	// Token: 0x040034EF RID: 13551
	private MaskStation.OxygenMaskReactable reactable;

	// Token: 0x040034F0 RID: 13552
	private MeterController materialsMeter;

	// Token: 0x040034F1 RID: 13553
	private MeterController oxygenMeter;

	// Token: 0x040034F2 RID: 13554
	public Meter.Offset materialsMeterOffset = Meter.Offset.Behind;

	// Token: 0x040034F3 RID: 13555
	public Meter.Offset oxygenMeterOffset;

	// Token: 0x040034F4 RID: 13556
	public string choreTypeID;

	// Token: 0x040034F5 RID: 13557
	protected FilteredStorage filteredStorage;

	// Token: 0x040034F6 RID: 13558
	public KAnimFile interactAnim = Assets.GetAnim("anim_equip_clothing_kanim");

	// Token: 0x040034F7 RID: 13559
	private int cell;

	// Token: 0x040034F8 RID: 13560
	public PathFinder.PotentialPath.Flags PathFlag;

	// Token: 0x040034F9 RID: 13561
	private Guid noElementStatusGuid;

	// Token: 0x040034FA RID: 13562
	private Grid.SuitMarker.Flags gridFlags;

	// Token: 0x02000EC8 RID: 3784
	private class OxygenMaskReactable : Reactable
	{
		// Token: 0x06004BAD RID: 19373 RVA: 0x0026D734 File Offset: 0x0026B934
		public OxygenMaskReactable(MaskStation mask_station) : base(mask_station.gameObject, "OxygenMask", Db.Get().ChoreTypes.SuitMarker, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.maskStation = mask_station;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0026D788 File Offset: 0x0026B988
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.maskStation == null)
			{
				base.Cleanup();
				return false;
			}
			bool flag = !new_reactor.GetComponent<MinionIdentity>().GetEquipment().IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			int x = transition.navGridTransition.x;
			if (x == 0)
			{
				return false;
			}
			if (!flag)
			{
				return (x >= 0 || !this.maskStation.isRotated) && (x <= 0 || this.maskStation.isRotated);
			}
			return this.maskStation.smi.IsReady() && (x <= 0 || !this.maskStation.isRotated) && (x >= 0 || this.maskStation.isRotated);
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0026D858 File Offset: 0x0026BA58
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(this.maskStation.interactAnim, 1f);
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.maskStation.CreateNewReactable();
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x0026D8EC File Offset: 0x0026BAEC
		public override void Update(float dt)
		{
			Facing facing = this.reactor ? this.reactor.GetComponent<Facing>() : null;
			if (facing && this.maskStation)
			{
				facing.SetFacing(this.maskStation.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH);
			}
			if (Time.time - this.startTime > 2.8f)
			{
				this.Run();
				base.Cleanup();
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x0026D964 File Offset: 0x0026BB64
		private void Run()
		{
			GameObject reactor = this.reactor;
			Equipment equipment = reactor.GetComponent<MinionIdentity>().GetEquipment();
			bool flag = !equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit);
			Navigator component = reactor.GetComponent<Navigator>();
			bool flag2 = component != null && (component.flags & this.maskStation.PathFlag) > PathFinder.PotentialPath.Flags.None;
			if (flag)
			{
				if (!this.maskStation.smi.IsReady())
				{
					return;
				}
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Oxygen_Mask".ToTag()), null, null);
				gameObject.SetActive(true);
				SimHashes elementID = this.maskStation.GetPossibleMaterials()[0].GetComponent<PrimaryElement>().ElementID;
				gameObject.GetComponent<PrimaryElement>().SetElement(elementID, false);
				SuitTank component2 = gameObject.GetComponent<SuitTank>();
				this.maskStation.materialStorage.ConsumeIgnoringDisease(this.maskStation.materialTag, this.maskStation.materialConsumedPerMask);
				this.maskStation.oxygenStorage.Transfer(component2.storage, component2.elementTag, this.maskStation.oxygenConsumedPerMask, false, true);
				Equippable component3 = gameObject.GetComponent<Equippable>();
				component3.Assign(equipment.GetComponent<IAssignableIdentity>());
				component3.isEquipped = true;
			}
			if (!flag)
			{
				Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
				assignable.Unassign();
				if (!flag2)
				{
					Notification notification = new Notification(MISC.NOTIFICATIONS.SUIT_DROPPED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SUIT_DROPPED.TOOLTIP, null, true, 0f, null, null, null, true, false, false);
					assignable.GetComponent<Notifier>().Add(notification, "");
				}
			}
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x000D5554 File Offset: 0x000D3754
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(this.maskStation.interactAnim);
			}
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040034FB RID: 13563
		private MaskStation maskStation;

		// Token: 0x040034FC RID: 13564
		private float startTime;
	}

	// Token: 0x02000ECA RID: 3786
	public class SMInstance : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.GameInstance
	{
		// Token: 0x06004BB7 RID: 19383 RVA: 0x000D5597 File Offset: 0x000D3797
		public SMInstance(MaskStation master) : base(master)
		{
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x000D55A0 File Offset: 0x000D37A0
		private bool HasSufficientMaterials()
		{
			return base.master.GetTotalMaterialAmount() >= base.master.materialConsumedPerMask;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x000D55BD File Offset: 0x000D37BD
		private bool HasSufficientOxygen()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask;
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x000D55DA File Offset: 0x000D37DA
		public bool OxygenIsFull()
		{
			return base.master.GetTotalOxygenAmount() >= base.master.oxygenConsumedPerMask * (float)base.master.maxUses;
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x000D5604 File Offset: 0x000D3804
		public bool IsReady()
		{
			return this.HasSufficientMaterials() && this.HasSufficientOxygen();
		}
	}

	// Token: 0x02000ECB RID: 3787
	public class States : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation>
	{
		// Token: 0x06004BBC RID: 19388 RVA: 0x0026DB0C File Offset: 0x0026BD0C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notOperational;
			this.notOperational.PlayAnim("off").TagTransition(GameTags.Operational, this.charging, false);
			this.charging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.notCharging, (MaskStation.SMInstance smi) => smi.OxygenIsFull() || !smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (smi.OxygenIsFull() || !smi.master.shouldPump)
				{
					smi.GoTo(this.notCharging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.charging.openChargingPre);
					return;
				}
				smi.GoTo(this.charging.closedChargingPre);
			});
			this.charging.opening.QueueAnim("opening_charging", false, null).OnAnimQueueComplete(this.charging.open);
			this.charging.open.PlayAnim("open_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.charging.closing.QueueAnim("closing_charging", false, null).OnAnimQueueComplete(this.charging.closed);
			this.charging.closed.PlayAnim("closed_charging_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.charging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.charging.openChargingPre.PlayAnim("open_charging_pre").OnAnimQueueComplete(this.charging.open);
			this.charging.closedChargingPre.PlayAnim("closed_charging_pre").OnAnimQueueComplete(this.charging.closed);
			this.notCharging.TagTransition(GameTags.Operational, this.notOperational, true).EventTransition(GameHashes.OnStorageChange, this.charging, (MaskStation.SMInstance smi) => !smi.OxygenIsFull() && smi.master.shouldPump).Update(delegate(MaskStation.SMInstance smi, float dt)
			{
				smi.master.UpdateOperational();
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(MaskStation.SMInstance smi)
			{
				if (!smi.OxygenIsFull() && smi.master.shouldPump)
				{
					smi.GoTo(this.charging);
					return;
				}
				if (smi.IsReady())
				{
					smi.GoTo(this.notCharging.openChargingPst);
					return;
				}
				smi.GoTo(this.notCharging.closedChargingPst);
			});
			this.notCharging.opening.PlayAnim("opening_not_charging").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.open.PlayAnim("open_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.closing, (MaskStation.SMInstance smi) => !smi.IsReady());
			this.notCharging.closing.PlayAnim("closing_not_charging").OnAnimQueueComplete(this.notCharging.closed);
			this.notCharging.closed.PlayAnim("closed_not_charging_loop").EventTransition(GameHashes.OnStorageChange, this.notCharging.opening, (MaskStation.SMInstance smi) => smi.IsReady());
			this.notCharging.openChargingPst.PlayAnim("open_charging_pst").OnAnimQueueComplete(this.notCharging.open);
			this.notCharging.closedChargingPst.PlayAnim("closed_charging_pst").OnAnimQueueComplete(this.notCharging.closed);
		}

		// Token: 0x040034FF RID: 13567
		public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State notOperational;

		// Token: 0x04003500 RID: 13568
		public MaskStation.States.ChargingStates charging;

		// Token: 0x04003501 RID: 13569
		public MaskStation.States.NotChargingStates notCharging;

		// Token: 0x02000ECC RID: 3788
		public class ChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x04003502 RID: 13570
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x04003503 RID: 13571
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x04003504 RID: 13572
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x04003505 RID: 13573
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x04003506 RID: 13574
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPre;

			// Token: 0x04003507 RID: 13575
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPre;
		}

		// Token: 0x02000ECD RID: 3789
		public class NotChargingStates : GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State
		{
			// Token: 0x04003508 RID: 13576
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State opening;

			// Token: 0x04003509 RID: 13577
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State open;

			// Token: 0x0400350A RID: 13578
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closing;

			// Token: 0x0400350B RID: 13579
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closed;

			// Token: 0x0400350C RID: 13580
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State openChargingPst;

			// Token: 0x0400350D RID: 13581
			public GameStateMachine<MaskStation.States, MaskStation.SMInstance, MaskStation, object>.State closedChargingPst;
		}
	}
}
