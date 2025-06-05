using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000E3D RID: 3645
public class IceKettle : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
	// Token: 0x0600473F RID: 18239 RVA: 0x0025FDC4 File Offset: 0x0025DFC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.root.EventHandlerTransition(GameHashes.WorkableStartWork, this.inUse, (IceKettle.Instance smi, object obj) => true).EventHandler(GameHashes.OnStorageChange, delegate(IceKettle.Instance smi)
		{
			smi.UpdateMeter();
		});
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.idle);
		this.operational.idle.PlayAnim(IceKettle.IDEL_ANIM_STATE).DefaultState(this.operational.idle.waitingForSolids);
		this.operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.waitingForSpaceInLiquidTank, (IceKettle.Instance smi) => IceKettle.HasEnoughSolidsToMelt(smi));
		this.operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace, null).EventTransition(GameHashes.OnStorageChange, this.operational.idle.notEnoughFuel, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.LiquidTankHasCapacityForNextBatch));
		this.operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel, null).EventTransition(GameHashes.OnStorageChange, this.operational.melting, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch));
		this.operational.melting.Toggle("Operational Active State", new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesTrue), new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesFalse)).DefaultState(this.operational.melting.entering);
		this.operational.melting.entering.PlayAnim(IceKettle.BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.melting.working);
		this.operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting, null).DefaultState(this.operational.melting.working.idle).PlayAnim(IceKettle.BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
		this.operational.melting.working.idle.ParamTransition<float>(this.MeltingTimer, this.operational.melting.working.complete, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>.Callback(IceKettle.IsDoneMelting)).Update(new Action<IceKettle.Instance, float>(IceKettle.MeltingTimerUpdate), UpdateRate.SIM_200ms, false);
		this.operational.melting.working.complete.Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.ResetMeltingTimer)).Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.MeltNextBatch)).EnterTransition(this.operational.melting.working.idle, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)).EnterTransition(this.operational.melting.exit, GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Not(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)));
		this.operational.melting.exit.PlayAnim(IceKettle.BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.idle);
		this.inUse.EventHandlerTransition(GameHashes.WorkableStopWork, this.noOperational, (IceKettle.Instance smi, object obj) => true).ScheduleGoTo(new Func<IceKettle.Instance, float>(IceKettle.GetInUseTimeout), this.noOperational);
	}

	// Token: 0x06004740 RID: 18240 RVA: 0x000D293F File Offset: 0x000D0B3F
	public static void SetOperationalActiveStatesTrue(IceKettle.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x000D294E File Offset: 0x000D0B4E
	public static void SetOperationalActiveStatesFalse(IceKettle.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x000D295D File Offset: 0x000D0B5D
	public static float GetInUseTimeout(IceKettle.Instance smi)
	{
		return smi.InUseWorkableDuration + 1f;
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x000D296B File Offset: 0x000D0B6B
	public static void ResetMeltingTimer(IceKettle.Instance smi)
	{
		smi.sm.MeltingTimer.Set(0f, smi, false);
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x000D2985 File Offset: 0x000D0B85
	public static bool HasEnoughSolidsToMelt(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x000D298D File Offset: 0x000D0B8D
	public static bool LiquidTankHasCapacityForNextBatch(IceKettle.Instance smi)
	{
		return smi.LiquidTankHasCapacityForNextBatch;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x000D2995 File Offset: 0x000D0B95
	public static bool HasEnoughFuelForNextBacth(IceKettle.Instance smi)
	{
		return smi.HasEnoughFuelUnitsToMeltNextBatch;
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x000D299D File Offset: 0x000D0B9D
	public static bool CanMeltNextBatch(IceKettle.Instance smi)
	{
		return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && IceKettle.LiquidTankHasCapacityForNextBatch(smi) && IceKettle.HasEnoughFuelForNextBacth(smi);
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x000D29B7 File Offset: 0x000D0BB7
	public static bool IsDoneMelting(IceKettle.Instance smi, float timePassed)
	{
		return timePassed >= smi.MeltDurationPerBatch;
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x002601B0 File Offset: 0x0025E3B0
	public static void MeltingTimerUpdate(IceKettle.Instance smi, float dt)
	{
		float num = smi.sm.MeltingTimer.Get(smi);
		smi.sm.MeltingTimer.Set(num + dt, smi, false);
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x000D29C5 File Offset: 0x000D0BC5
	public static void MeltNextBatch(IceKettle.Instance smi)
	{
		smi.MeltNextBatch();
	}

	// Token: 0x040031D4 RID: 12756
	public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";

	// Token: 0x040031D5 RID: 12757
	public static string LIQUID_METER_ANIM_NAME = "meter_kettle";

	// Token: 0x040031D6 RID: 12758
	public static string IDEL_ANIM_STATE = "on";

	// Token: 0x040031D7 RID: 12759
	public static string BOILING_PRE_ANIM_NAME = "boiling_pre";

	// Token: 0x040031D8 RID: 12760
	public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";

	// Token: 0x040031D9 RID: 12761
	public static string BOILING_PST_ANIM_NAME = "boiling_pst";

	// Token: 0x040031DA RID: 12762
	private const float InUseTimeout = 5f;

	// Token: 0x040031DB RID: 12763
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State noOperational;

	// Token: 0x040031DC RID: 12764
	public IceKettle.OperationalStates operational;

	// Token: 0x040031DD RID: 12765
	public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State inUse;

	// Token: 0x040031DE RID: 12766
	public StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.FloatParameter MeltingTimer;

	// Token: 0x02000E3E RID: 3646
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600474D RID: 18253 RVA: 0x002601E8 File Offset: 0x0025E3E8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			string txt = string.Format(UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string tooltip = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, GameUtil.GetFormattedMass(this.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			Descriptor item = new Descriptor(txt, tooltip, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}

		// Token: 0x040031DF RID: 12767
		public SimHashes exhaust_tag;

		// Token: 0x040031E0 RID: 12768
		public Tag targetElementTag;

		// Token: 0x040031E1 RID: 12769
		public Tag fuelElementTag;

		// Token: 0x040031E2 RID: 12770
		public float KGToMeltPerBatch;

		// Token: 0x040031E3 RID: 12771
		public float KGMeltedPerSecond;

		// Token: 0x040031E4 RID: 12772
		public float TargetTemperature;

		// Token: 0x040031E5 RID: 12773
		public float EnergyPerUnitOfLumber;

		// Token: 0x040031E6 RID: 12774
		public float ExhaustMassPerUnitOfLumber;
	}

	// Token: 0x02000E3F RID: 3647
	public class WorkingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040031E7 RID: 12775
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State idle;

		// Token: 0x040031E8 RID: 12776
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State complete;
	}

	// Token: 0x02000E40 RID: 3648
	public class MeltingStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040031E9 RID: 12777
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State entering;

		// Token: 0x040031EA RID: 12778
		public IceKettle.WorkingStates working;

		// Token: 0x040031EB RID: 12779
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State exit;
	}

	// Token: 0x02000E41 RID: 3649
	public class IdleStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040031EC RID: 12780
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State notEnoughFuel;

		// Token: 0x040031ED RID: 12781
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSolids;

		// Token: 0x040031EE RID: 12782
		public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSpaceInLiquidTank;
	}

	// Token: 0x02000E42 RID: 3650
	public class OperationalStates : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
	{
		// Token: 0x040031EF RID: 12783
		public IceKettle.MeltingStates melting;

		// Token: 0x040031F0 RID: 12784
		public IceKettle.IdleStates idle;
	}

	// Token: 0x02000E43 RID: 3651
	public new class Instance : GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.GameInstance
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06004753 RID: 18259 RVA: 0x000D2A1B File Offset: 0x000D0C1B
		public float CurrentTemperatureOfSolidsStored
		{
			get
			{
				if (this.kettleStorage.MassStored() <= 0f)
				{
					return 0f;
				}
				return this.kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06004754 RID: 18260 RVA: 0x000D2A50 File Offset: 0x000D0C50
		public float MeltDurationPerBatch
		{
			get
			{
				return base.def.KGToMeltPerBatch / base.def.KGMeltedPerSecond;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06004755 RID: 18261 RVA: 0x000D2A69 File Offset: 0x000D0C69
		public float FuelUnitsAvailable
		{
			get
			{
				return this.fuelStorage.MassStored();
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x000D2A76 File Offset: 0x000D0C76
		public bool HasAtLeastOneBatchOfSolidsWaitingToMelt
		{
			get
			{
				return this.kettleStorage.MassStored() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06004757 RID: 18263 RVA: 0x000D2A93 File Offset: 0x000D0C93
		public bool HasEnoughFuelUnitsToMeltNextBatch
		{
			get
			{
				return this.kettleStorage.MassStored() <= 0f || this.FuelUnitsAvailable >= this.FuelRequiredForNextBratch;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x000D2ABA File Offset: 0x000D0CBA
		public bool LiquidTankHasCapacityForNextBatch
		{
			get
			{
				return this.outputStorage.RemainingCapacity() >= base.def.KGToMeltPerBatch;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06004759 RID: 18265 RVA: 0x000D2AD7 File Offset: 0x000D0CD7
		public float LiquidTankCapacity
		{
			get
			{
				return this.outputStorage.capacityKg;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x000D2AE4 File Offset: 0x000D0CE4
		public float LiquidStored
		{
			get
			{
				return this.outputStorage.MassStored();
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600475B RID: 18267 RVA: 0x000D2AF1 File Offset: 0x000D0CF1
		public float FuelRequiredForNextBratch
		{
			get
			{
				return this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, this.CurrentTemperatureOfSolidsStored);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600475C RID: 18268 RVA: 0x000D2B10 File Offset: 0x000D0D10
		public float InUseWorkableDuration
		{
			get
			{
				return this.dupeWorkable.workTime;
			}
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00260264 File Offset: 0x0025E464
		public Instance(IStateMachineTarget master, IceKettle.Def def) : base(master, def)
		{
			this.elementToMelt = ElementLoader.GetElement(def.targetElementTag);
			this.LiquidMeter = new MeterController(this.animController, IceKettle.LIQUID_METER_TARGET_NAME, IceKettle.LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.fuelStorage = components[0];
			this.kettleStorage = components[1];
			this.outputStorage = components[2];
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x000D2B1D File Offset: 0x000D0D1D
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateMeter();
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x000D2B2B File Offset: 0x000D0D2B
		public void UpdateMeter()
		{
			this.LiquidMeter.SetPositionPercent(this.outputStorage.MassStored() / this.outputStorage.capacityKg);
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x002602D4 File Offset: 0x0025E4D4
		public void MeltNextBatch()
		{
			if (!this.HasAtLeastOneBatchOfSolidsWaitingToMelt)
			{
				return;
			}
			PrimaryElement component = this.kettleStorage.FindFirst(base.def.targetElementTag).GetComponent<PrimaryElement>();
			float num = Mathf.Min(this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, base.def.KGToMeltPerBatch, component.Temperature), this.FuelUnitsAvailable);
			float mass = 0f;
			float num2 = 0f;
			SimUtil.DiseaseInfo diseaseInfo;
			this.kettleStorage.ConsumeAndGetDisease(this.elementToMelt.id.CreateTag(), base.def.KGToMeltPerBatch, out mass, out diseaseInfo, out num2);
			this.outputStorage.AddElement(this.elementToMelt.highTempTransitionTarget, mass, base.def.TargetTemperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			float temperature = this.fuelStorage.FindFirst(base.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
			this.fuelStorage.ConsumeIgnoringDisease(base.def.fuelElementTag, num);
			float mass2 = num * base.def.ExhaustMassPerUnitOfLumber;
			Element element = ElementLoader.FindElementByHash(base.def.exhaust_tag);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.gameObject), element.id, null, mass2, temperature, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x00260418 File Offset: 0x0025E618
		public float GetUnitsOfFuelRequiredToMelt(Element elementToMelt, float massToMelt_KG, float elementToMelt_initialTemperature)
		{
			if (!elementToMelt.IsSolid)
			{
				return -1f;
			}
			float num = massToMelt_KG * elementToMelt.specificHeatCapacity * elementToMelt_initialTemperature;
			float targetTemperature = base.def.TargetTemperature;
			return (massToMelt_KG * elementToMelt.specificHeatCapacity * targetTemperature - num) / base.def.EnergyPerUnitOfLumber;
		}

		// Token: 0x040031F1 RID: 12785
		private Storage fuelStorage;

		// Token: 0x040031F2 RID: 12786
		private Storage kettleStorage;

		// Token: 0x040031F3 RID: 12787
		private Storage outputStorage;

		// Token: 0x040031F4 RID: 12788
		private Element elementToMelt;

		// Token: 0x040031F5 RID: 12789
		private MeterController LiquidMeter;

		// Token: 0x040031F6 RID: 12790
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040031F7 RID: 12791
		[MyCmpGet]
		private IceKettleWorkable dupeWorkable;

		// Token: 0x040031F8 RID: 12792
		[MyCmpGet]
		private KBatchedAnimController animController;
	}
}
