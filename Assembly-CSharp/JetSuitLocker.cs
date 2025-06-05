using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000E48 RID: 3656
public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x0600477E RID: 18302 RVA: 0x002609E8 File Offset: 0x0025EBE8
	public float FuelAvailable
	{
		get
		{
			GameObject fuel = this.GetFuel();
			float num = 0f;
			if (fuel != null)
			{
				num = fuel.GetComponent<PrimaryElement>().Mass / 100f;
				num = Math.Min(num, 1f);
			}
			return num;
		}
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x00260A2C File Offset: 0x0025EC2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.fuel_tag = SimHashes.Petroleum.CreateTag();
		this.fuel_consumer = base.gameObject.AddComponent<ConduitConsumer>();
		this.fuel_consumer.conduitType = this.portInfo.conduitType;
		this.fuel_consumer.consumptionRate = 10f;
		this.fuel_consumer.capacityTag = this.fuel_tag;
		this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		this.fuel_consumer.forceAlwaysSatisfied = true;
		this.fuel_consumer.capacityKG = 100f;
		this.fuel_consumer.useSecondaryInput = true;
		RequireInputs requireInputs = base.gameObject.AddComponent<RequireInputs>();
		requireInputs.conduitConsumer = this.fuel_consumer;
		requireInputs.SetRequirements(false, true);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.secondaryInputCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		this.fuel_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_1"
		});
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_2"
		});
		base.smi.StartSM();
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x000D2C30 File Offset: 0x000D0E30
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, this.flowNetworkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x000D2C5A File Offset: 0x000D0E5A
	private GameObject GetFuel()
	{
		return this.storage.FindFirst(this.fuel_tag);
	}

	// Token: 0x06004782 RID: 18306 RVA: 0x000D2C6D File Offset: 0x000D0E6D
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x000D2C7A File Offset: 0x000D0E7A
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x00260BD0 File Offset: 0x0025EDD0
	private void FuelSuit(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		GameObject fuel = this.GetFuel();
		if (fuel == null)
		{
			return;
		}
		PrimaryElement component = fuel.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
		float num = 375f * dt / 600f;
		num = Mathf.Min(num, 25f - component2.amount);
		num = Mathf.Min(component.Mass, num);
		component.Mass -= num;
		component2.amount += num;
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x000D2C87 File Offset: 0x000D0E87
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x000D2C97 File Offset: 0x000D0E97
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x00260C70 File Offset: 0x0025EE70
	public bool HasFuel()
	{
		GameObject fuel = this.GetFuel();
		return fuel != null && fuel.GetComponent<PrimaryElement>().Mass > 0f;
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x00260CA4 File Offset: 0x0025EEA4
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.fuel_meter.SetPositionPercent(this.FuelAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x00260D10 File Offset: 0x0025EF10
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x00260D54 File Offset: 0x0025EF54
	public bool IsFuelTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
			return component == null || component.PercentFull() >= TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x0400320B RID: 12811
	[MyCmpReq]
	private Building building;

	// Token: 0x0400320C RID: 12812
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400320D RID: 12813
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x0400320E RID: 12814
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x0400320F RID: 12815
	public const float FUEL_CAPACITY = 100f;

	// Token: 0x04003210 RID: 12816
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04003211 RID: 12817
	private int secondaryInputCell = -1;

	// Token: 0x04003212 RID: 12818
	private FlowUtilityNetwork.NetworkItem flowNetworkItem;

	// Token: 0x04003213 RID: 12819
	private ConduitConsumer fuel_consumer;

	// Token: 0x04003214 RID: 12820
	private Tag fuel_tag;

	// Token: 0x04003215 RID: 12821
	private MeterController o2_meter;

	// Token: 0x04003216 RID: 12822
	private MeterController fuel_meter;

	// Token: 0x02000E49 RID: 3657
	public class States : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
	{
		// Token: 0x0600478C RID: 18316 RVA: 0x00260D98 File Offset: 0x0025EF98
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (JetSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (JetSuitLocker.StatesInstance smi) => !smi.master.HasFuel(), UpdateRate.SIM_200ms).Update("FuelSuit", delegate(JetSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FuelSuit(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (JetSuitLocker.StatesInstance smi) => smi.master.HasFuel(), UpdateRate.SIM_200ms).ToggleStatusItem(BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, default(HashedString), 129022, null, null, null);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (JetSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04003217 RID: 12823
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;

		// Token: 0x04003218 RID: 12824
		public JetSuitLocker.States.ChargingStates charging;

		// Token: 0x04003219 RID: 12825
		public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

		// Token: 0x02000E4A RID: 3658
		public class ChargingStates : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
		{
			// Token: 0x0400321A RID: 12826
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;

			// Token: 0x0400321B RID: 12827
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;

			// Token: 0x0400321C RID: 12828
			public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
		}
	}

	// Token: 0x02000E4C RID: 3660
	public class StatesInstance : GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance
	{
		// Token: 0x06004799 RID: 18329 RVA: 0x000D2D4E File Offset: 0x000D0F4E
		public StatesInstance(JetSuitLocker jet_suit_locker) : base(jet_suit_locker)
		{
		}
	}
}
