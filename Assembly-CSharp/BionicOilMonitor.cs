using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x0200154B RID: 5451
public class BionicOilMonitor : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>
{
	// Token: 0x0600716C RID: 29036 RVA: 0x0030A174 File Offset: 0x00308374
	private static Effect CreateFreshOilEffectVariation(string id, float stressBonus, float moralBonus)
	{
		Effect effect = new Effect("FreshOil_" + id, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, DUPLICANTS.MODIFIERS.FRESHOIL.TOOLTIP, 4800f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, moralBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, stressBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		return effect;
	}

	// Token: 0x0600716D RID: 29037 RVA: 0x0030A220 File Offset: 0x00308420
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.offline;
		this.root.Update(new Action<BionicOilMonitor.Instance, float>(BionicOilMonitor.OilAmountInstanceWatcherUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.offline.EventTransition(GameHashes.BionicOnline, this.online, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline)).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.online.EventTransition(GameHashes.BionicOffline, this.offline, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline))).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.AddBaseOilDeltaModifier)).DefaultState(this.online.idle).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.EnableSolidLubricationSensor)).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.DisableSolidLubricationSensor));
		this.online.idle.EnterTransition(this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.WantsOilChange)).OnSignal(this.OilValueChanged, this.online.seeking, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.WantsOilChange));
		this.online.seeking.OnSignal(this.OilFilledSignal, this.online.idle).OnSignal(this.OilValueChanged, this.online.idle, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.HasDecentAmountOfOil)).DefaultState(this.online.seeking.hasOil).ToggleThought(Db.Get().Thoughts.RefillOilDesire, null).ToggleUrge(Db.Get().Urges.OilRefill).ToggleChore((BionicOilMonitor.Instance smi) => new UseSolidLubricantChore(smi.master), this.online.idle);
		this.online.seeking.hasOil.EnterTransition(this.online.seeking.noOil, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.HasAnyAmountOfOil))).OnSignal(this.OilRanOutSignal, this.online.seeking.noOil).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWantsOilChange, null);
		this.online.seeking.noOil.Enter(delegate(BionicOilMonitor.Instance smi)
		{
			smi.currentNoLubricationEffectApplied = smi.effects.Add(smi.GetEffect(), false).effect.IdHash;
		}).Exit(delegate(BionicOilMonitor.Instance smi)
		{
			smi.effects.Remove(smi.currentNoLubricationEffectApplied);
		}).ToggleReactable(new Func<BionicOilMonitor.Instance, Reactable>(BionicOilMonitor.GrindingGearsReactable)).EventTransition(GameHashes.AssignedRoleChanged, this.online.seeking.hasOil, null);
	}

	// Token: 0x0600716E RID: 29038 RVA: 0x000EEAAC File Offset: 0x000ECCAC
	public static bool IsBionicOnline(BionicOilMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x0600716F RID: 29039 RVA: 0x000EEAB4 File Offset: 0x000ECCB4
	public static bool HasAnyAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilMass > 0f;
	}

	// Token: 0x06007170 RID: 29040 RVA: 0x000EEAC3 File Offset: 0x000ECCC3
	public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage > 0.2f;
	}

	// Token: 0x06007171 RID: 29041 RVA: 0x000EEAD2 File Offset: 0x000ECCD2
	public static bool WantsOilChange(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage <= 0.2f;
	}

	// Token: 0x06007172 RID: 29042 RVA: 0x000EEAE4 File Offset: 0x000ECCE4
	public static void AddBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(true);
	}

	// Token: 0x06007173 RID: 29043 RVA: 0x000EEAED File Offset: 0x000ECCED
	public static void RemoveBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(false);
	}

	// Token: 0x06007174 RID: 29044 RVA: 0x0030A4F0 File Offset: 0x003086F0
	public static void OilAmountInstanceWatcherUpdate(BionicOilMonitor.Instance smi, float dt)
	{
		float lastOilAmountMassRecorded = smi.LastOilAmountMassRecorded;
		float num = smi.CurrentOilMass - lastOilAmountMassRecorded;
		if (num != 0f)
		{
			smi.LastOilAmountMassRecorded = smi.CurrentOilMass;
			if (!smi.HasOil)
			{
				smi.ReportOilRanOut();
			}
			smi.ReportOilValueChanged(num);
		}
	}

	// Token: 0x06007175 RID: 29045 RVA: 0x000EEAF6 File Offset: 0x000ECCF6
	public static void EnableSolidLubricationSensor(BionicOilMonitor.Instance smi)
	{
		smi.SetSolidLubricationSensorActiveState(true);
	}

	// Token: 0x06007176 RID: 29046 RVA: 0x000EEAFF File Offset: 0x000ECCFF
	public static void DisableSolidLubricationSensor(BionicOilMonitor.Instance smi)
	{
		smi.SetSolidLubricationSensorActiveState(false);
	}

	// Token: 0x06007177 RID: 29047 RVA: 0x000EEB08 File Offset: 0x000ECD08
	private static Reactable GrindingGearsReactable(BionicOilMonitor.Instance smi)
	{
		return smi.GetGrindingGearReactable();
	}

	// Token: 0x06007179 RID: 29049 RVA: 0x0030A538 File Offset: 0x00308738
	// Note: this type is marked as 'beforefieldinit'.
	static BionicOilMonitor()
	{
		Dictionary<SimHashes, Effect> dictionary = new Dictionary<SimHashes, Effect>();
		dictionary[SimHashes.CrudeOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.CrudeOil.ToString(), -0.016666668f, 3f);
		dictionary[SimHashes.PhytoOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.PhytoOil.ToString(), -0.008333334f, 2f);
		BionicOilMonitor.LUBRICANT_TYPE_EFFECT = dictionary;
	}

	// Token: 0x0400552C RID: 21804
	public static Dictionary<SimHashes, Effect> LUBRICANT_TYPE_EFFECT;

	// Token: 0x0400552D RID: 21805
	public const float OIL_CAPACITY = 200f;

	// Token: 0x0400552E RID: 21806
	public const float OIL_TANK_DURATION = 6000f;

	// Token: 0x0400552F RID: 21807
	public const float OIL_REFILL_TRESHOLD = 0.2f;

	// Token: 0x04005530 RID: 21808
	public const string NO_OIL_EFFECT_NAME_MINOR = "NoLubricationMinor";

	// Token: 0x04005531 RID: 21809
	public const string NO_OIL_EFFECT_NAME_MAJOR = "NoLubricationMajor";

	// Token: 0x04005532 RID: 21810
	public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State offline;

	// Token: 0x04005533 RID: 21811
	public BionicOilMonitor.OnlineStates online;

	// Token: 0x04005534 RID: 21812
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilFilledSignal;

	// Token: 0x04005535 RID: 21813
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilRanOutSignal;

	// Token: 0x04005536 RID: 21814
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilValueChanged;

	// Token: 0x04005537 RID: 21815
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OnClosestSolidLubricantChangedSignal;

	// Token: 0x0200154C RID: 5452
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200154D RID: 5453
	public class WantsOilChangeState : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x04005538 RID: 21816
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State hasOil;

		// Token: 0x04005539 RID: 21817
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State noOil;
	}

	// Token: 0x0200154E RID: 5454
	public class OnlineStates : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x0400553A RID: 21818
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State idle;

		// Token: 0x0400553B RID: 21819
		public BionicOilMonitor.WantsOilChangeState seeking;
	}

	// Token: 0x0200154F RID: 5455
	public new class Instance : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.GameInstance
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600717D RID: 29053 RVA: 0x000EEB20 File Offset: 0x000ECD20
		public bool IsOnline
		{
			get
			{
				return this.batterySMI != null && this.batterySMI.IsOnline;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600717E RID: 29054 RVA: 0x000EEB37 File Offset: 0x000ECD37
		public bool HasOil
		{
			get
			{
				return this.CurrentOilMass > 0f;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600717F RID: 29055 RVA: 0x000EEB46 File Offset: 0x000ECD46
		public float CurrentOilPercentage
		{
			get
			{
				return this.CurrentOilMass / this.oilAmount.GetMax();
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06007180 RID: 29056 RVA: 0x000EEB5A File Offset: 0x000ECD5A
		public float CurrentOilMass
		{
			get
			{
				if (this.oilAmount != null)
				{
					return this.oilAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06007182 RID: 29058 RVA: 0x000EEB7E File Offset: 0x000ECD7E
		// (set) Token: 0x06007181 RID: 29057 RVA: 0x000EEB75 File Offset: 0x000ECD75
		public AmountInstance oilAmount { get; private set; }

		// Token: 0x06007183 RID: 29059 RVA: 0x0030A5AC File Offset: 0x003087AC
		public Instance(IStateMachineTarget master, BionicOilMonitor.Def def) : base(master, def)
		{
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			this.batterySMI = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x0030A630 File Offset: 0x00308830
		public override void StartSM()
		{
			this.closestSolidLubricantSensor = base.GetComponent<Sensors>().GetSensor<ClosestLubricantSensor>();
			ClosestLubricantSensor closestLubricantSensor = this.closestSolidLubricantSensor;
			closestLubricantSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestLubricantSensor.OnItemChanged, new Action<Pickupable>(this.OnClosestSolidLubricantChanged));
			this.LastOilAmountMassRecorded = this.CurrentOilMass;
			base.StartSM();
		}

		// Token: 0x06007185 RID: 29061 RVA: 0x000EEB86 File Offset: 0x000ECD86
		public string GetEffect()
		{
			if (!this.resume.HasPerk(Db.Get().SkillPerks.EfficientBionicGears))
			{
				return "NoLubricationMajor";
			}
			return "NoLubricationMinor";
		}

		// Token: 0x06007186 RID: 29062 RVA: 0x000EEBAF File Offset: 0x000ECDAF
		private void ReportOilTankFilled()
		{
			base.sm.OilFilledSignal.Trigger(this);
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x000EEBC2 File Offset: 0x000ECDC2
		public void ReportOilRanOut()
		{
			base.sm.OilRanOutSignal.Trigger(this);
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x000EEBD5 File Offset: 0x000ECDD5
		public void ReportOilValueChanged(float delta)
		{
			base.sm.OilValueChanged.Trigger(this);
			Action<float> onOilValueChanged = this.OnOilValueChanged;
			if (onOilValueChanged == null)
			{
				return;
			}
			onOilValueChanged(delta);
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x000EEBF9 File Offset: 0x000ECDF9
		public void SetOilMassValue(float value)
		{
			this.oilAmount.SetValue(value);
		}

		// Token: 0x0600718A RID: 29066 RVA: 0x0030A688 File Offset: 0x00308888
		public void SetBaseDeltaModifierActiveState(bool isActive)
		{
			MinionModifiers component = base.GetComponent<MinionModifiers>();
			if (isActive)
			{
				bool flag = false;
				int count = component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers.Count;
				for (int i = 0; i < count; i++)
				{
					if (component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers[i] == this.BaseOilDeltaModifier)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					component.attributes.Add(this.BaseOilDeltaModifier);
					return;
				}
			}
			else
			{
				component.attributes.Remove(this.BaseOilDeltaModifier);
			}
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x000EEC08 File Offset: 0x000ECE08
		public void RefillOil(float amount)
		{
			this.oilAmount.SetValue(this.CurrentOilMass + amount);
			this.ReportOilTankFilled();
		}

		// Token: 0x0600718C RID: 29068 RVA: 0x000EEC24 File Offset: 0x000ECE24
		private void OnClosestSolidLubricantChanged(Pickupable newItem)
		{
			base.sm.OnClosestSolidLubricantChangedSignal.Trigger(this);
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x000EEC37 File Offset: 0x000ECE37
		public Pickupable GetClosestSolidLubricant()
		{
			return this.closestSolidLubricantSensor.GetItem();
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x000EEC44 File Offset: 0x000ECE44
		public void SetSolidLubricationSensorActiveState(bool shouldItBeActive)
		{
			this.closestSolidLubricantSensor.SetActive(shouldItBeActive);
			if (shouldItBeActive)
			{
				this.closestSolidLubricantSensor.Update();
			}
		}

		// Token: 0x0600718F RID: 29071 RVA: 0x0030A724 File Offset: 0x00308924
		public Reactable GetGrindingGearReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.GrindingGears.Id, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 10f, float.PositiveInfinity, 0f);
			Emote grindingGears = Db.Get().Emotes.Minion.GrindingGears;
			selfEmoteReactable.SetEmote(grindingGears);
			selfEmoteReactable.SetThought(Db.Get().Thoughts.RefillOilDesire);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x0400553C RID: 21820
		public float LastOilAmountMassRecorded = -1f;

		// Token: 0x0400553D RID: 21821
		public Action<float> OnOilValueChanged;

		// Token: 0x0400553E RID: 21822
		private BionicBatteryMonitor.Instance batterySMI;

		// Token: 0x0400553F RID: 21823
		[MyCmpGet]
		private MinionResume resume;

		// Token: 0x04005540 RID: 21824
		[MyCmpGet]
		public Effects effects;

		// Token: 0x04005541 RID: 21825
		public HashedString currentNoLubricationEffectApplied;

		// Token: 0x04005542 RID: 21826
		private AttributeModifier BaseOilDeltaModifier = new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, -0.033333335f, BionicMinionConfig.NAME, false, false, true);

		// Token: 0x04005544 RID: 21828
		private ClosestLubricantSensor closestSolidLubricantSensor;
	}
}
