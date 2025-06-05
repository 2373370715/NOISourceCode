using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020015CB RID: 5579
public class GunkMonitor : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>
{
	// Token: 0x060073D5 RID: 29653 RVA: 0x00310F94 File Offset: 0x0030F194
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Update(new Action<GunkMonitor.Instance, float>(GunkMonitor.GunkAmountWatcherUpdate), UpdateRate.SIM_200ms, false);
		this.idle.OnSignal(this.gunkValueChangedSignal, this.mildUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold));
		this.mildUrge.OnSignal(this.gunkValueChangedSignal, this.criticalUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).OnSignal(this.gunkValueChangedSignal, this.idle, new Func<GunkMonitor.Instance, bool>(GunkMonitor.DoesNotWantToExpellGunk)).DefaultState(this.mildUrge.prevented);
		this.mildUrge.prevented.ScheduleChange(this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling));
		this.mildUrge.allowed.ScheduleChange(this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).ToggleUrge(Db.Get().Urges.Pee).ToggleUrge(Db.Get().Urges.GunkPee);
		this.criticalUrge.OnSignal(this.gunkValueChangedSignal, this.idle, new Func<GunkMonitor.Instance, bool>(GunkMonitor.DoesNotWantToExpellGunk)).OnSignal(this.gunkValueChangedSignal, this.mildUrge, (GunkMonitor.Instance smi) => !GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi)).OnSignal(this.gunkValueChangedSignal, this.cantHold, new Func<GunkMonitor.Instance, bool>(GunkMonitor.CanNotHoldGunkAnymore)).ToggleUrge(Db.Get().Urges.GunkPee).ToggleUrge(Db.Get().Urges.Pee).ToggleEffect("GunkSick").ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleThought(Db.Get().Thoughts.ExpellGunkDesire, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
		this.cantHold.ToggleUrge(Db.Get().Urges.GunkPee).ToggleThought(Db.Get().Thoughts.ExpellingGunk, null).ToggleChore((GunkMonitor.Instance smi) => new BionicGunkSpillChore(smi.master), this.emptyRemaining);
		this.emptyRemaining.Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ExpellAllGunk)).Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ApplyGunkHungoverEffect)).GoTo(this.idle);
	}

	// Token: 0x060073D6 RID: 29654 RVA: 0x000F05CF File Offset: 0x000EE7CF
	public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.DesperetlySeekForGunkToiletTreshold;
	}

	// Token: 0x060073D7 RID: 29655 RVA: 0x000F05E7 File Offset: 0x000EE7E7
	public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.SeekForGunkToiletTreshold_InSchedule;
	}

	// Token: 0x060073D8 RID: 29656 RVA: 0x000F05FF File Offset: 0x000EE7FF
	public static bool ScheduleAllowsExpelling(GunkMonitor.Instance smi)
	{
		return smi.DoesCurrentScheduleAllowsGunkToilet;
	}

	// Token: 0x060073D9 RID: 29657 RVA: 0x000F0607 File Offset: 0x000EE807
	public static bool DoesNotWantToExpellGunk(GunkMonitor.Instance smi)
	{
		return !GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
	}

	// Token: 0x060073DA RID: 29658 RVA: 0x000F0612 File Offset: 0x000EE812
	public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi)
	{
		return smi.IsGunkBuildupAtMax;
	}

	// Token: 0x060073DB RID: 29659 RVA: 0x000F061A File Offset: 0x000EE81A
	public static void ExpellAllGunk(GunkMonitor.Instance smi)
	{
		smi.ExpellAllGunk(null);
	}

	// Token: 0x060073DC RID: 29660 RVA: 0x000F0623 File Offset: 0x000EE823
	public static void ApplyGunkHungoverEffect(GunkMonitor.Instance smi)
	{
		smi.GetComponent<Effects>().Add("GunkHungover", true);
	}

	// Token: 0x060073DD RID: 29661 RVA: 0x000F0637 File Offset: 0x000EE837
	public static void GunkAmountWatcherUpdate(GunkMonitor.Instance smi, float dt)
	{
		smi.GunkAmountWatcherUpdate(dt);
	}

	// Token: 0x040056FA RID: 22266
	public const float BIONIC_RADS_REMOVED_WHEN_PEE = 300f;

	// Token: 0x040056FB RID: 22267
	public static readonly float GUNK_CAPACITY = 80f;

	// Token: 0x040056FC RID: 22268
	public const string GUNK_FULL_EFFECT_NAME = "GunkSick";

	// Token: 0x040056FD RID: 22269
	public const string GUNK_HUNGOVER_EFFECT_NAME = "GunkHungover";

	// Token: 0x040056FE RID: 22270
	public static SimHashes GunkElement = SimHashes.LiquidGunk;

	// Token: 0x040056FF RID: 22271
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State idle;

	// Token: 0x04005700 RID: 22272
	public GunkMonitor.MildUrgeStates mildUrge;

	// Token: 0x04005701 RID: 22273
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State criticalUrge;

	// Token: 0x04005702 RID: 22274
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State cantHold;

	// Token: 0x04005703 RID: 22275
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State emptyRemaining;

	// Token: 0x04005704 RID: 22276
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal gunkValueChangedSignal;

	// Token: 0x020015CC RID: 5580
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005705 RID: 22277
		public float SeekForGunkToiletTreshold_InSchedule = 0.6f;

		// Token: 0x04005706 RID: 22278
		public float DesperetlySeekForGunkToiletTreshold = 0.9f;
	}

	// Token: 0x020015CD RID: 5581
	public class MildUrgeStates : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State
	{
		// Token: 0x04005707 RID: 22279
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State allowed;

		// Token: 0x04005708 RID: 22280
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State prevented;
	}

	// Token: 0x020015CE RID: 5582
	public new class Instance : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.GameInstance
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x000F0684 File Offset: 0x000EE884
		public bool HasGunk
		{
			get
			{
				return this.CurrentGunkMass > 0f;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060073E3 RID: 29667 RVA: 0x000F0693 File Offset: 0x000EE893
		public bool IsGunkBuildupAtMax
		{
			get
			{
				return this.CurrentGunkPercentage >= 1f;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060073E4 RID: 29668 RVA: 0x000F06A5 File Offset: 0x000EE8A5
		public float CurrentGunkMass
		{
			get
			{
				if (this.gunkAmount != null)
				{
					return this.gunkAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060073E5 RID: 29669 RVA: 0x000F06C0 File Offset: 0x000EE8C0
		public float CurrentGunkPercentage
		{
			get
			{
				return this.CurrentGunkMass / this.gunkAmount.GetMax();
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060073E6 RID: 29670 RVA: 0x000F06D4 File Offset: 0x000EE8D4
		public bool DoesCurrentScheduleAllowsGunkToilet
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat) || this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
			}
		}

		// Token: 0x060073E7 RID: 29671 RVA: 0x00311238 File Offset: 0x0030F438
		public Instance(IStateMachineTarget master, GunkMonitor.Def def) : base(master, def)
		{
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.gunkAmount = Db.Get().Amounts.BionicGunk.Lookup(base.gameObject);
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x060073E8 RID: 29672 RVA: 0x0031129C File Offset: 0x0030F49C
		public override void StartSM()
		{
			this.oilMonitor = base.gameObject.GetSMI<BionicOilMonitor.Instance>();
			BionicOilMonitor.Instance instance = this.oilMonitor;
			instance.OnOilValueChanged = (Action<float>)Delegate.Combine(instance.OnOilValueChanged, new Action<float>(this.OnOilValueChanged));
			this.LastAmountOfGunkObserved = this.CurrentGunkMass;
			base.StartSM();
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x000F070E File Offset: 0x000EE90E
		public void GunkAmountWatcherUpdate(float dt)
		{
			if (this.LastAmountOfGunkObserved != this.CurrentGunkMass)
			{
				this.LastAmountOfGunkObserved = this.CurrentGunkMass;
				base.sm.gunkValueChangedSignal.Trigger(this);
			}
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x000F073B File Offset: 0x000EE93B
		protected override void OnCleanUp()
		{
			if (this.oilMonitor != null)
			{
				BionicOilMonitor.Instance instance = this.oilMonitor;
				instance.OnOilValueChanged = (Action<float>)Delegate.Remove(instance.OnOilValueChanged, new Action<float>(this.OnOilValueChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x060073EB RID: 29675 RVA: 0x003112F4 File Offset: 0x0030F4F4
		private void OnOilValueChanged(float delta)
		{
			float num = (delta < 0f) ? Mathf.Abs(delta) : 0f;
			float gunkMassValue = Mathf.Clamp(this.CurrentGunkMass + num, 0f, this.gunkAmount.GetMax());
			this.SetGunkMassValue(gunkMassValue);
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x000F0772 File Offset: 0x000EE972
		public void SetGunkMassValue(float value)
		{
			float currentGunkMass = this.CurrentGunkMass;
			this.gunkAmount.SetValue(value);
			this.LastAmountOfGunkObserved = this.CurrentGunkMass;
			base.sm.gunkValueChangedSignal.Trigger(this);
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x0031133C File Offset: 0x0030F53C
		public void ExpellGunk(float mass, Storage targetStorage = null)
		{
			if (this.HasGunk)
			{
				float currentGunkMass = this.CurrentGunkMass;
				float num = Mathf.Min(mass, this.CurrentGunkMass);
				num = Mathf.Max(num, Mathf.Epsilon);
				int gameCell = Grid.PosToCell(base.transform.position);
				byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE);
				float num2 = num / GunkMonitor.GUNK_CAPACITY;
				if (targetStorage != null)
				{
					targetStorage.AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else
				{
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						equippable.GetComponent<Storage>().AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, GunkMonitor.GunkElement, CellEventLogger.Instance.Vomit, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), true, -1);
					}
				}
				if (Sim.IsRadiationEnabled())
				{
					MinionIdentity component = base.transform.GetComponent<MinionIdentity>();
					AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup(component);
					RadiationMonitor.Instance smi = component.GetSMI<RadiationMonitor.Instance>();
					float num3 = DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / DUPLICANTSTATS.BIONICS.BaseStats.BLADDER_INCREASE_PER_SECOND;
					float num4 = Math.Min(amountInstance.value, 300f * num3 * smi.difficultySettingMod * num2);
					if (num4 >= 1f)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num4).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, component.transform, Vector3.up * 2f, 1.5f, false, false);
					}
					amountInstance.ApplyDelta(-num4);
				}
				this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass - num, 0f, this.gunkAmount.GetMax()));
			}
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x000F07A5 File Offset: 0x000EE9A5
		public void ExpellAllGunk(Storage targetStorage = null)
		{
			this.ExpellGunk(this.CurrentGunkMass, targetStorage);
		}

		// Token: 0x04005709 RID: 22281
		private float LastAmountOfGunkObserved;

		// Token: 0x0400570A RID: 22282
		private BionicOilMonitor.Instance oilMonitor;

		// Token: 0x0400570B RID: 22283
		private AmountInstance gunkAmount;

		// Token: 0x0400570C RID: 22284
		private AmountInstance bodyTemperature;

		// Token: 0x0400570D RID: 22285
		private Schedulable schedulable;
	}
}
