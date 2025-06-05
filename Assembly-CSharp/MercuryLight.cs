using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000EDB RID: 3803
public class MercuryLight : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
	// Token: 0x06004C32 RID: 19506 RVA: 0x0026F5C4 File Offset: 0x0026D7C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<float>(this.Charge, this.noOperational.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.noOperational.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.noOperational.depleating.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.noOperational.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.noOperational.depleated.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.idle.TagTransition(GameTags.Operational, this.noOperational.exit, false).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null);
		this.noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.darkness).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ConsumeFuelUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<bool>(this.HasEnoughFuel, this.operational.light, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsTrue).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>(this.Charge, this.operational.darkness.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
		this.operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate), UpdateRate.SIM_200ms, false);
		this.operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.darkness.idle);
		this.operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated, null).ParamTransition<float>(this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero);
		this.operational.light.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOn)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.HasEnoughFuel, this.operational.darkness, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).DefaultState(this.operational.light.charging);
		this.operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging, null).ParamTransition<float>(this.Charge, this.operational.light.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTEOne).Update(new Action<MercuryLight.Instance, float>(MercuryLight.ChargeUpdate), UpdateRate.SIM_200ms, false);
		this.operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged, null).ParamTransition<float>(this.Charge, this.operational.light.charging, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTOne);
	}

	// Token: 0x06004C33 RID: 19507 RVA: 0x000D59A7 File Offset: 0x000D3BA7
	public static void SetOperationalActiveFlagOn(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06004C34 RID: 19508 RVA: 0x000D59B6 File Offset: 0x000D3BB6
	public static void SetOperationalActiveFlagOff(MercuryLight.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06004C35 RID: 19509 RVA: 0x000D59C5 File Offset: 0x000D3BC5
	public static void DepleteUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.DepleteUpdate(dt);
	}

	// Token: 0x06004C36 RID: 19510 RVA: 0x000D59CE File Offset: 0x000D3BCE
	public static void ChargeUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ChargeUpdate(dt);
	}

	// Token: 0x06004C37 RID: 19511 RVA: 0x000D59D7 File Offset: 0x000D3BD7
	public static void ConsumeFuelUpdate(MercuryLight.Instance smi, float dt)
	{
		smi.ConsumeFuelUpdate(dt);
	}

	// Token: 0x04003552 RID: 13650
	private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();

	// Token: 0x04003553 RID: 13651
	private const string ON_ANIM_NAME = "on";

	// Token: 0x04003554 RID: 13652
	private const string ON_PRE_ANIM_NAME = "on_pre";

	// Token: 0x04003555 RID: 13653
	private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";

	// Token: 0x04003556 RID: 13654
	private const string DEPLEATING_ANIM_NAME = "depleating";

	// Token: 0x04003557 RID: 13655
	private const string OFF_ANIM_NAME = "off";

	// Token: 0x04003558 RID: 13656
	private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";

	// Token: 0x04003559 RID: 13657
	private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";

	// Token: 0x0400355A RID: 13658
	public MercuryLight.Darknesstates noOperational;

	// Token: 0x0400355B RID: 13659
	public MercuryLight.OperationalStates operational;

	// Token: 0x0400355C RID: 13660
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.FloatParameter Charge;

	// Token: 0x0400355D RID: 13661
	public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.BoolParameter HasEnoughFuel;

	// Token: 0x02000EDC RID: 3804
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004C3A RID: 19514 RVA: 0x0026FA04 File Offset: 0x0026DC04
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			string arg = MercuryLight.ELEMENT_TAG.ProperName();
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
			return list;
		}

		// Token: 0x0400355E RID: 13662
		public float MAX_LUX;

		// Token: 0x0400355F RID: 13663
		public float TURN_ON_DELAY;

		// Token: 0x04003560 RID: 13664
		public float FUEL_MASS_PER_SECOND;
	}

	// Token: 0x02000EDD RID: 3805
	public class LightStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04003561 RID: 13665
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State charging;

		// Token: 0x04003562 RID: 13666
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
	}

	// Token: 0x02000EDE RID: 3806
	public class Darknesstates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04003563 RID: 13667
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleating;

		// Token: 0x04003564 RID: 13668
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleated;

		// Token: 0x04003565 RID: 13669
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;

		// Token: 0x04003566 RID: 13670
		public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State exit;
	}

	// Token: 0x02000EDF RID: 3807
	public class OperationalStates : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
	{
		// Token: 0x04003567 RID: 13671
		public MercuryLight.LightStates light;

		// Token: 0x04003568 RID: 13672
		public MercuryLight.Darknesstates darkness;
	}

	// Token: 0x02000EE0 RID: 3808
	public new class Instance : GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.GameInstance
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x000D5A01 File Offset: 0x000D3C01
		public bool HasEnoughFuel
		{
			get
			{
				return base.sm.HasEnoughFuel.Get(this);
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x000D5A14 File Offset: 0x000D3C14
		public int LuxLevel
		{
			get
			{
				return Mathf.FloorToInt(base.smi.ChargeLevel * base.def.MAX_LUX);
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x000D5A33 File Offset: 0x000D3C33
		public float ChargeLevel
		{
			get
			{
				return base.smi.sm.Charge.Get(this);
			}
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0026FA78 File Offset: 0x0026DC78
		public Instance(IStateMachineTarget master, MercuryLight.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.lightIntensityMeterController = new MeterController(component, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x000D5A4B File Offset: 0x000D3C4B
		public override void StartSM()
		{
			base.StartSM();
			this.SetChargeLevel(this.ChargeLevel);
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0026FAB4 File Offset: 0x0026DCB4
		public void DepleteUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel - dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x0026FAEC File Offset: 0x0026DCEC
		public void ChargeUpdate(float dt)
		{
			float chargeLevel = Mathf.Clamp(this.ChargeLevel + dt / base.def.TURN_ON_DELAY, 0f, 1f);
			this.SetChargeLevel(chargeLevel);
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0026FB24 File Offset: 0x0026DD24
		public void SetChargeLevel(float value)
		{
			base.sm.Charge.Set(value, this, false);
			this.light.Lux = this.LuxLevel;
			this.light.FullRefresh();
			bool flag = this.ChargeLevel > 0f;
			if (this.light.enabled != flag)
			{
				this.light.enabled = flag;
			}
			this.lightIntensityMeterController.SetPositionPercent(value);
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x0026FB98 File Offset: 0x0026DD98
		public void ConsumeFuelUpdate(float dt)
		{
			float num = base.def.FUEL_MASS_PER_SECOND * dt;
			if (this.storage.MassStored() < num)
			{
				base.sm.HasEnoughFuel.Set(false, this, false);
				return;
			}
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float num3;
			this.storage.ConsumeAndGetDisease(MercuryLight.ELEMENT_TAG, num, out num2, out diseaseInfo, out num3);
			base.sm.HasEnoughFuel.Set(true, this, false);
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanRun()
		{
			return true;
		}

		// Token: 0x04003569 RID: 13673
		[MyCmpGet]
		public Operational operational;

		// Token: 0x0400356A RID: 13674
		[MyCmpGet]
		private Light2D light;

		// Token: 0x0400356B RID: 13675
		[MyCmpGet]
		private Storage storage;

		// Token: 0x0400356C RID: 13676
		[MyCmpGet]
		private ConduitConsumer conduitConsumer;

		// Token: 0x0400356D RID: 13677
		private MeterController lightIntensityMeterController;
	}
}
