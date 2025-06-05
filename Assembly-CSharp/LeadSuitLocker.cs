using System;
using TUNING;
using UnityEngine;

// Token: 0x02000E56 RID: 3670
public class LeadSuitLocker : StateMachineComponent<LeadSuitLocker.StatesInstance>
{
	// Token: 0x060047B8 RID: 18360 RVA: 0x00261688 File Offset: 0x0025F888
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.o2_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_top", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_top"
		});
		this.battery_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target_side", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
		{
			"meter_target_side"
		});
		base.smi.StartSM();
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x000D2EE7 File Offset: 0x000D10E7
	public bool IsSuitFullyCharged()
	{
		return this.suit_locker.IsSuitFullyCharged();
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x000D2EF4 File Offset: 0x000D10F4
	public KPrefabID GetStoredOutfit()
	{
		return this.suit_locker.GetStoredOutfit();
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x00261708 File Offset: 0x0025F908
	private void FillBattery(float dt)
	{
		KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
		if (storedOutfit == null)
		{
			return;
		}
		LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
		if (!component.IsFull())
		{
			component.batteryCharge += dt / this.batteryChargeTime;
		}
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x00261750 File Offset: 0x0025F950
	private void RefreshMeter()
	{
		this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
		this.battery_meter.SetPositionPercent(this.suit_locker.BatteryAvailable);
		this.anim_controller.SetSymbolVisiblity("oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
		this.anim_controller.SetSymbolVisiblity("petrol_yes_bloom", this.IsBatteryAboveMinimumLevel());
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x002617C0 File Offset: 0x0025F9C0
	public bool IsOxygenTankAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			SuitTank component = storedOutfit.GetComponent<SuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x00261804 File Offset: 0x0025FA04
	public bool IsBatteryAboveMinimumLevel()
	{
		KPrefabID storedOutfit = this.GetStoredOutfit();
		if (storedOutfit != null)
		{
			LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
			return component == null || component.PercentFull() >= EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
		}
		return false;
	}

	// Token: 0x04003242 RID: 12866
	[MyCmpReq]
	private Building building;

	// Token: 0x04003243 RID: 12867
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003244 RID: 12868
	[MyCmpReq]
	private SuitLocker suit_locker;

	// Token: 0x04003245 RID: 12869
	[MyCmpReq]
	private KBatchedAnimController anim_controller;

	// Token: 0x04003246 RID: 12870
	private MeterController o2_meter;

	// Token: 0x04003247 RID: 12871
	private MeterController battery_meter;

	// Token: 0x04003248 RID: 12872
	private float batteryChargeTime = 60f;

	// Token: 0x02000E57 RID: 3671
	public class States : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker>
	{
		// Token: 0x060047C0 RID: 18368 RVA: 0x00261848 File Offset: 0x0025FA48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Update("RefreshMeter", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.RefreshMeter();
			}, UpdateRate.RENDER_200ms, false);
			this.empty.EventTransition(GameHashes.OnStorageChange, this.charging, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() != null);
			this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null).Transition(this.charged, (LeadSuitLocker.StatesInstance smi) => smi.master.IsSuitFullyCharged(), UpdateRate.SIM_200ms);
			this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational, false);
			this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Update("FillBattery", delegate(LeadSuitLocker.StatesInstance smi, float dt)
			{
				smi.master.FillBattery(dt);
			}, UpdateRate.SIM_1000ms, false);
			this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (LeadSuitLocker.StatesInstance smi) => smi.master.GetStoredOutfit() == null);
		}

		// Token: 0x04003249 RID: 12873
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State empty;

		// Token: 0x0400324A RID: 12874
		public LeadSuitLocker.States.ChargingStates charging;

		// Token: 0x0400324B RID: 12875
		public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State charged;

		// Token: 0x02000E58 RID: 3672
		public class ChargingStates : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State
		{
			// Token: 0x0400324C RID: 12876
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State notoperational;

			// Token: 0x0400324D RID: 12877
			public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State operational;
		}
	}

	// Token: 0x02000E5A RID: 3674
	public class StatesInstance : GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.GameInstance
	{
		// Token: 0x060047CB RID: 18379 RVA: 0x000D2F7E File Offset: 0x000D117E
		public StatesInstance(LeadSuitLocker lead_suit_locker) : base(lead_suit_locker)
		{
		}
	}
}
