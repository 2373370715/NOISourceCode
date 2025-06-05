using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010B0 RID: 4272
[AddComponentMenu("KMonoBehaviour/Workable/Clinic")]
public class Clinic : Workable, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x060056D5 RID: 22229 RVA: 0x002921D0 File Offset: 0x002903D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.assignable.subSlots = new AssignableSlot[]
		{
			Db.Get().AssignableSlots.MedicalBed
		};
		this.assignable.AddAutoassignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAutoAssignTo));
		this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanManuallyAssignTo));
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x000DD0E8 File Offset: 0x000DB2E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		Components.Clinics.Add(this);
		base.SetWorkTime(float.PositiveInfinity);
		this.clinicSMI = new Clinic.ClinicSM.Instance(this);
		this.clinicSMI.StartSM();
	}

	// Token: 0x060056D7 RID: 22231 RVA: 0x000DD128 File Offset: 0x000DB328
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		Components.Clinics.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060056D8 RID: 22232 RVA: 0x0029223C File Offset: 0x0029043C
	private KAnimFile[] GetAppropriateOverrideAnims(WorkerBase worker)
	{
		KAnimFile[] result = null;
		if (!worker.GetSMI<WoundMonitor.Instance>().ShouldExitInfirmary())
		{
			result = this.workerInjuredAnims;
		}
		else if (this.workerDiseasedAnims != null && this.IsValidEffect(this.diseaseEffect) && worker.GetSMI<SicknessMonitor.Instance>().IsSick())
		{
			result = this.workerDiseasedAnims;
		}
		return result;
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x000DD146 File Offset: 0x000DB346
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		this.overrideAnims = this.GetAppropriateOverrideAnims(worker);
		return base.GetAnim(worker);
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x000DD15C File Offset: 0x000DB35C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("Sleep", false);
		this.InstantExtremeRadiationRecovery();
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x0029228C File Offset: 0x0029048C
	private void InstantExtremeRadiationRecovery()
	{
		if (Game.IsDlcActiveForCurrentSave("EXPANSION1_ID"))
		{
			RadiationMonitor.Instance smi = base.worker.GetSMI<RadiationMonitor.Instance>();
			if (smi.sm.radiationExposure.Get(smi) >= 900f * smi.difficultySettingMod)
			{
				smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).SetValue(600f * smi.difficultySettingMod);
			}
		}
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x00292308 File Offset: 0x00290508
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		KAnimFile[] appropriateOverrideAnims = this.GetAppropriateOverrideAnims(worker);
		if (appropriateOverrideAnims == null || appropriateOverrideAnims != this.overrideAnims)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x000DD17D File Offset: 0x000DB37D
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Sleep");
		base.OnStopWork(worker);
	}

	// Token: 0x060056DE RID: 22238 RVA: 0x00292338 File Offset: 0x00290538
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.assignable.Unassign();
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Clinic.EffectsRemoved.Length; i++)
		{
			string effect_id = Clinic.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x060056DF RID: 22239 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x060056E0 RID: 22240 RVA: 0x00292380 File Offset: 0x00290580
	private Chore CreateWorkChore(ChoreType chore_type, bool allow_prioritization, bool allow_in_red_alert, PriorityScreen.PriorityClass priority_class, bool ignore_schedule_block = false)
	{
		return new WorkChore<Clinic>(chore_type, this, null, true, null, null, null, allow_in_red_alert, null, ignore_schedule_block, true, null, false, true, allow_prioritization, priority_class, 5, false, false);
	}

	// Token: 0x060056E1 RID: 22241 RVA: 0x002923AC File Offset: 0x002905AC
	private Chore CreateBionicWorkChore(ChoreType chore_type, bool allow_prioritization, bool allow_in_red_alert, PriorityScreen.PriorityClass priority_class, bool ignore_schedule_block = false)
	{
		WorkChore<Clinic> workChore = new WorkChore<Clinic>(chore_type, this, null, true, null, null, null, allow_in_red_alert, null, ignore_schedule_block, true, null, false, true, allow_prioritization, priority_class, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsBionic, null);
		return workChore;
	}

	// Token: 0x060056E2 RID: 22242 RVA: 0x002923E8 File Offset: 0x002905E8
	private bool CanAutoAssignTo(MinionAssignablesProxy worker)
	{
		bool flag = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			if (this.IsValidEffect(this.healthEffect))
			{
				Health component = minionIdentity.GetComponent<Health>();
				if (component != null && component.hitPoints < component.maxHitPoints)
				{
					flag = true;
				}
			}
			if (!flag && this.IsValidEffect(this.diseaseEffect))
			{
				flag = (minionIdentity.GetComponent<MinionModifiers>().sicknesses.Count > 0);
			}
		}
		return flag;
	}

	// Token: 0x060056E3 RID: 22243 RVA: 0x00292460 File Offset: 0x00290660
	private bool CanManuallyAssignTo(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = this.IsHealthBelowThreshold(minionIdentity.gameObject);
		}
		return result;
	}

	// Token: 0x060056E4 RID: 22244 RVA: 0x00292494 File Offset: 0x00290694
	private bool IsHealthBelowThreshold(GameObject minion)
	{
		Health health = (minion != null) ? minion.GetComponent<Health>() : null;
		if (health != null)
		{
			float num = health.hitPoints / health.maxHitPoints;
			if (health != null)
			{
				return num < this.MedicalAttentionMinimum;
			}
		}
		return false;
	}

	// Token: 0x060056E5 RID: 22245 RVA: 0x000DD196 File Offset: 0x000DB396
	private bool IsValidEffect(string effect)
	{
		return effect != null && effect != "";
	}

	// Token: 0x060056E6 RID: 22246 RVA: 0x000DD1A8 File Offset: 0x000DB3A8
	private bool AllowDoctoring()
	{
		return this.IsValidEffect(this.doctoredDiseaseEffect) || this.IsValidEffect(this.doctoredHealthEffect);
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x002924E0 File Offset: 0x002906E0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.IsValidEffect(this.healthEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.healthEffect, false);
		}
		if (this.diseaseEffect != this.healthEffect && this.IsValidEffect(this.diseaseEffect))
		{
			Effect.AddModifierDescriptions(base.gameObject, descriptors, this.diseaseEffect, false);
		}
		if (this.AllowDoctoring())
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.DOCTORING, UI.BUILDINGEFFECTS.TOOLTIPS.DOCTORING, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			if (this.IsValidEffect(this.doctoredHealthEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredHealthEffect, true);
			}
			if (this.doctoredDiseaseEffect != this.doctoredHealthEffect && this.IsValidEffect(this.doctoredDiseaseEffect))
			{
				Effect.AddModifierDescriptions(base.gameObject, descriptors, this.doctoredDiseaseEffect, true);
			}
		}
		return descriptors;
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x060056E8 RID: 22248 RVA: 0x000DD1C6 File Offset: 0x000DB3C6
	public float MedicalAttentionMinimum
	{
		get
		{
			return this.sicknessSliderValue / 100f;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x060056E9 RID: 22249 RVA: 0x000DD1D4 File Offset: 0x000DB3D4
	string ISliderControl.SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x060056EA RID: 22250 RVA: 0x000CF907 File Offset: 0x000CDB07
	string ISliderControl.SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x060056EB RID: 22251 RVA: 0x000B1628 File Offset: 0x000AF828
	int ISliderControl.SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060056EC RID: 22252 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	float ISliderControl.GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060056ED RID: 22253 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	float ISliderControl.GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x060056EE RID: 22254 RVA: 0x000DD1DB File Offset: 0x000DB3DB
	float ISliderControl.GetSliderValue(int index)
	{
		return this.sicknessSliderValue;
	}

	// Token: 0x060056EF RID: 22255 RVA: 0x000DD1E3 File Offset: 0x000DB3E3
	void ISliderControl.SetSliderValue(float percent, int index)
	{
		if (percent != this.sicknessSliderValue)
		{
			this.sicknessSliderValue = (float)Mathf.RoundToInt(percent);
			Game.Instance.Trigger(875045922, null);
		}
	}

	// Token: 0x060056F0 RID: 22256 RVA: 0x000DD20B File Offset: 0x000DB40B
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP"), this.sicknessSliderValue);
	}

	// Token: 0x060056F1 RID: 22257 RVA: 0x000DD22C File Offset: 0x000DB42C
	string ISliderControl.GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MEDICALCOTSIDESCREEN.TOOLTIP";
	}

	// Token: 0x04003D7E RID: 15742
	[MyCmpReq]
	private Assignable assignable;

	// Token: 0x04003D7F RID: 15743
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04003D80 RID: 15744
	private const int MAX_RANGE = 10;

	// Token: 0x04003D81 RID: 15745
	private const float CHECK_RANGE_INTERVAL = 10f;

	// Token: 0x04003D82 RID: 15746
	public float doctorVisitInterval = 300f;

	// Token: 0x04003D83 RID: 15747
	public KAnimFile[] workerInjuredAnims;

	// Token: 0x04003D84 RID: 15748
	public KAnimFile[] workerDiseasedAnims;

	// Token: 0x04003D85 RID: 15749
	public string diseaseEffect;

	// Token: 0x04003D86 RID: 15750
	public string healthEffect;

	// Token: 0x04003D87 RID: 15751
	public string doctoredDiseaseEffect;

	// Token: 0x04003D88 RID: 15752
	public string doctoredHealthEffect;

	// Token: 0x04003D89 RID: 15753
	public string doctoredPlaceholderEffect;

	// Token: 0x04003D8A RID: 15754
	private Clinic.ClinicSM.Instance clinicSMI;

	// Token: 0x04003D8B RID: 15755
	public static readonly Chore.Precondition IsOverSicknessThreshold = new Chore.Precondition
	{
		id = "IsOverSicknessThreshold",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Clinic)data).IsHealthBelowThreshold(context.consumerState.gameObject);
		}
	};

	// Token: 0x04003D8C RID: 15756
	[Serialize]
	private float sicknessSliderValue = 70f;

	// Token: 0x020010B1 RID: 4273
	public class ClinicSM : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic>
	{
		// Token: 0x060056F4 RID: 22260 RVA: 0x0029263C File Offset: 0x0029083C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Clinic.ClinicSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Assignable>().Unassign();
			});
			this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Clinic.ClinicSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.AssigneeChanged, this.unoperational, null).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.Heal, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.HealCritical, false, true, PriorityScreen.PriorityClass.personalNeeds, false), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.healthEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.RestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateWorkChore(Db.Get().ChoreTypes.SleepDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect)).ToggleRecurringChore((Clinic.ClinicSM.Instance smi) => smi.master.CreateBionicWorkChore(Db.Get().ChoreTypes.BionicRestDueToDisease, false, true, PriorityScreen.PriorityClass.personalNeeds, true), (Clinic.ClinicSM.Instance smi) => !string.IsNullOrEmpty(smi.master.diseaseEffect));
			this.operational.idle.WorkableStartTransition((Clinic.ClinicSM.Instance smi) => smi.master, this.operational.healing);
			this.operational.healing.DefaultState(this.operational.healing.undoctored).WorkableStopTransition((Clinic.ClinicSM.Instance smi) => smi.GetComponent<Clinic>(), this.operational.idle).Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.master.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.healing.undoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.healthEffect, false);
				smi.StartEffect(smi.master.diseaseEffect, false);
				bool flag = false;
				if (smi.master.worker != null)
				{
					flag = (smi.HasEffect(smi.master.doctoredHealthEffect) || smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredPlaceholderEffect));
				}
				if (smi.master.AllowDoctoring())
				{
					if (flag)
					{
						smi.GoTo(this.operational.healing.doctored);
						return;
					}
					smi.StartDoctorChore();
				}
			}).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StopEffect(smi.master.healthEffect);
				smi.StopEffect(smi.master.diseaseEffect);
				smi.StopDoctorChore();
			});
			this.operational.healing.newlyDoctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
				smi.StartEffect(smi.master.doctoredHealthEffect, true);
				smi.GoTo(this.operational.healing.doctored);
			});
			this.operational.healing.doctored.Enter(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredPlaceholderEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredPlaceholderEffect);
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredDiseaseEffect, true);
					if (effectInstance2 != null)
					{
						float num = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance2.timeRemaining = effectInstance2.effect.duration - num;
					}
					EffectInstance effectInstance3 = smi.StartEffect(smi.master.doctoredHealthEffect, true);
					if (effectInstance3 != null)
					{
						float num2 = effectInstance.effect.duration - effectInstance.timeRemaining;
						effectInstance3.timeRemaining = effectInstance3.effect.duration - num2;
					}
					component.Remove(smi.master.doctoredPlaceholderEffect);
				}
			}).ScheduleGoTo(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				float num = smi.master.doctorVisitInterval;
				if (smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredHealthEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					num = Mathf.Min(num, effectInstance.GetTimeRemaining());
				}
				return num;
			}, this.operational.healing.undoctored).Exit(delegate(Clinic.ClinicSM.Instance smi)
			{
				Effects component = smi.master.worker.GetComponent<Effects>();
				if (smi.HasEffect(smi.master.doctoredDiseaseEffect) || smi.HasEffect(smi.master.doctoredHealthEffect))
				{
					EffectInstance effectInstance = component.Get(smi.master.doctoredDiseaseEffect);
					if (effectInstance == null)
					{
						effectInstance = component.Get(smi.master.doctoredHealthEffect);
					}
					EffectInstance effectInstance2 = smi.StartEffect(smi.master.doctoredPlaceholderEffect, true);
					effectInstance2.timeRemaining = effectInstance2.effect.duration - (effectInstance.effect.duration - effectInstance.timeRemaining);
					component.Remove(smi.master.doctoredDiseaseEffect);
					component.Remove(smi.master.doctoredHealthEffect);
				}
			});
		}

		// Token: 0x04003D8D RID: 15757
		public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State unoperational;

		// Token: 0x04003D8E RID: 15758
		public Clinic.ClinicSM.OperationalStates operational;

		// Token: 0x020010B2 RID: 4274
		public class OperationalStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04003D8F RID: 15759
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State idle;

			// Token: 0x04003D90 RID: 15760
			public Clinic.ClinicSM.HealingStates healing;
		}

		// Token: 0x020010B3 RID: 4275
		public class HealingStates : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State
		{
			// Token: 0x04003D91 RID: 15761
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State undoctored;

			// Token: 0x04003D92 RID: 15762
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State doctored;

			// Token: 0x04003D93 RID: 15763
			public GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.State newlyDoctored;
		}

		// Token: 0x020010B4 RID: 4276
		public new class Instance : GameStateMachine<Clinic.ClinicSM, Clinic.ClinicSM.Instance, Clinic, object>.GameInstance
		{
			// Token: 0x060056FA RID: 22266 RVA: 0x000DD29F File Offset: 0x000DB49F
			public Instance(Clinic master) : base(master)
			{
			}

			// Token: 0x060056FB RID: 22267 RVA: 0x00292AD0 File Offset: 0x00290CD0
			public void StartDoctorChore()
			{
				if (base.master.IsValidEffect(base.master.doctoredHealthEffect) || base.master.IsValidEffect(base.master.doctoredDiseaseEffect))
				{
					this.doctorChore = new WorkChore<DoctorChoreWorkable>(Db.Get().ChoreTypes.Doctor, base.smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
					WorkChore<DoctorChoreWorkable> workChore = this.doctorChore;
					workChore.onComplete = (Action<Chore>)Delegate.Combine(workChore.onComplete, new Action<Chore>(delegate(Chore chore)
					{
						base.smi.GoTo(base.smi.sm.operational.healing.newlyDoctored);
					}));
				}
			}

			// Token: 0x060056FC RID: 22268 RVA: 0x000DD2A8 File Offset: 0x000DB4A8
			public void StopDoctorChore()
			{
				if (this.doctorChore != null)
				{
					this.doctorChore.Cancel("StopDoctorChore");
					this.doctorChore = null;
				}
			}

			// Token: 0x060056FD RID: 22269 RVA: 0x00292B6C File Offset: 0x00290D6C
			public bool HasEffect(string effect)
			{
				bool result = false;
				if (base.master.IsValidEffect(effect))
				{
					result = base.smi.master.worker.GetComponent<Effects>().HasEffect(effect);
				}
				return result;
			}

			// Token: 0x060056FE RID: 22270 RVA: 0x00292BA8 File Offset: 0x00290DA8
			public EffectInstance StartEffect(string effect, bool should_save)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (!component.HasEffect(effect))
						{
							return component.Add(effect, should_save);
						}
					}
				}
				return null;
			}

			// Token: 0x060056FF RID: 22271 RVA: 0x00292BF8 File Offset: 0x00290DF8
			public void StopEffect(string effect)
			{
				if (base.master.IsValidEffect(effect))
				{
					WorkerBase worker = base.smi.master.worker;
					if (worker != null)
					{
						Effects component = worker.GetComponent<Effects>();
						if (component.HasEffect(effect))
						{
							component.Remove(effect);
						}
					}
				}
			}

			// Token: 0x04003D94 RID: 15764
			private WorkChore<DoctorChoreWorkable> doctorChore;
		}
	}
}
