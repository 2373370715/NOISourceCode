using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001605 RID: 5637
public class RadiationMonitor : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance>
{
	// Token: 0x060074CB RID: 29899 RVA: 0x00313158 File Offset: 0x00311358
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.init;
		this.init.Transition(null, (RadiationMonitor.Instance smi) => !Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms).Transition(this.active, (RadiationMonitor.Instance smi) => Sim.IsRadiationEnabled(), UpdateRate.SIM_200ms);
		this.active.Update(new Action<RadiationMonitor.Instance, float>(RadiationMonitor.CheckRadiationLevel), UpdateRate.SIM_1000ms, false).DefaultState(this.active.idle);
		this.active.idle.DoNothing().ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ParamTransition<float>(this.radiationExposure, this.active.sick.minor, RadiationMonitor.COMPARE_GTE_MINOR);
		this.active.sick.ParamTransition<float>(this.radiationExposure, this.active.idle, RadiationMonitor.COMPARE_LT_MINOR).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(true, smi, false);
		}).Exit(delegate(RadiationMonitor.Instance smi)
		{
			smi.sm.isSick.Set(false, smi, false);
		});
		this.active.sick.minor.ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.minorSicknessEffect;
			}
			return RadiationMonitor.bionic_minorSicknessEffect;
		}).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ParamTransition<float>(this.radiationExposure, this.active.sick.major, RadiationMonitor.COMPARE_GTE_MAJOR).ToggleAnims("anim_loco_radiation1_kanim", 4f).ToggleAnims("anim_idle_radiation1_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation1, null).DefaultState(this.active.sick.minor.waiting);
		this.active.sick.minor.reacting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.minor.waiting);
		this.active.sick.major.ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.majorSicknessEffect;
			}
			return RadiationMonitor.bionic_majorSicknessEffect;
		}).ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_EXTREME).ToggleAnims("anim_loco_radiation2_kanim", 4f).ToggleAnims("anim_idle_radiation2_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation2, null).DefaultState(this.active.sick.major.waiting);
		this.active.sick.major.waiting.ScheduleGoTo(120f, this.active.sick.major.vomiting);
		this.active.sick.major.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.major.waiting);
		this.active.sick.extreme.ParamTransition<float>(this.radiationExposure, this.active.sick.deadly, RadiationMonitor.COMPARE_GTE_DEADLY).ToggleEffect(delegate(RadiationMonitor.Instance smi)
		{
			if (!smi.master.gameObject.HasTag(GameTags.Minions.Models.Bionic))
			{
				return RadiationMonitor.extremeSicknessEffect;
			}
			return RadiationMonitor.bionic_extremeSicknessEffect;
		}).ToggleAnims("anim_loco_radiation3_kanim", 4f).ToggleAnims("anim_idle_radiation3_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation3, null).DefaultState(this.active.sick.extreme.waiting);
		this.active.sick.extreme.waiting.ScheduleGoTo(60f, this.active.sick.extreme.vomiting);
		this.active.sick.extreme.vomiting.ToggleChore(new Func<RadiationMonitor.Instance, Chore>(this.CreateVomitChore), this.active.sick.extreme.waiting);
		this.active.sick.deadly.ToggleAnims("anim_loco_radiation4_kanim", 4f).ToggleAnims("anim_idle_radiation4_kanim", 4f).ToggleExpression(Db.Get().Expressions.Radiation4, null).ParamTransition<float>(this.radiationExposure, this.active.sick.extreme, RadiationMonitor.COMPARE_GTE_NO_LONGER_DEADLY).Enter(delegate(RadiationMonitor.Instance smi)
		{
			smi.GetComponent<Health>().Incapacitate(GameTags.RadiationSicknessIncapacitation);
		});
	}

	// Token: 0x060074CC RID: 29900 RVA: 0x003136E4 File Offset: 0x003118E4
	private Chore CreateVomitChore(RadiationMonitor.Instance smi)
	{
		Notification notification = new Notification(DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.RADIATIONVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		return new VomitChore(Db.Get().ChoreTypes.Vomit, smi.master, Db.Get().DuplicantStatusItems.Vomiting, notification, null);
	}

	// Token: 0x060074CD RID: 29901 RVA: 0x0031375C File Offset: 0x0031195C
	private static void RadiationRecovery(RadiationMonitor.Instance smi, float dt)
	{
		float num = Db.Get().Attributes.RadiationRecovery.Lookup(smi.gameObject).GetTotalValue() * dt;
		smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num);
		smi.master.Trigger(1556680150, num);
	}

	// Token: 0x060074CE RID: 29902 RVA: 0x003137CC File Offset: 0x003119CC
	private static void CheckRadiationLevel(RadiationMonitor.Instance smi, float dt)
	{
		RadiationMonitor.RadiationRecovery(smi, dt);
		smi.sm.timeUntilNextExposureReact.Delta(-dt, smi);
		smi.sm.timeUntilNextSickReact.Delta(-dt, smi);
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			float num2 = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(smi.gameObject).GetTotalValue());
			float num3 = Grid.Radiation[num] * 1f * num2 / 600f * dt;
			smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).ApplyDelta(num3);
			float num4 = num3 / dt * 600f;
			smi.sm.currentExposurePerCycle.Set(num4, smi, false);
			if (smi.sm.timeUntilNextExposureReact.Get(smi) <= 0f && !smi.HasTag(GameTags.InTransitTube) && RadiationMonitor.COMPARE_REACT(smi, num4))
			{
				smi.sm.timeUntilNextExposureReact.Set(120f, smi, false);
				Emote radiation_Glare = Db.Get().Emotes.Minion.Radiation_Glare;
				smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Glare, true, Db.Get().ChoreTypes.EmoteHighPriority, 0f, 20f, float.NegativeInfinity, 0f, null);
			}
		}
		if (smi.sm.timeUntilNextSickReact.Get(smi) <= 0f && smi.sm.isSick.Get(smi) && !smi.HasTag(GameTags.InTransitTube))
		{
			smi.sm.timeUntilNextSickReact.Set(60f, smi, false);
			Emote radiation_Itch = Db.Get().Emotes.Minion.Radiation_Itch;
			smi.master.gameObject.GetSMI<ReactionMonitor.Instance>().AddSelfEmoteReactable(smi.master.gameObject, "RadiationReact", radiation_Itch, true, Db.Get().ChoreTypes.RadiationPain, 0f, 20f, float.NegativeInfinity, 0f, null);
		}
		smi.sm.radiationExposure.Set(smi.master.gameObject.GetComponent<KSelectable>().GetAmounts().GetValue("RadiationBalance"), smi, false);
	}

	// Token: 0x040057A3 RID: 22435
	public const float BASE_ABSORBTION_RATE = 1f;

	// Token: 0x040057A4 RID: 22436
	public const float MIN_TIME_BETWEEN_EXPOSURE_REACTS = 120f;

	// Token: 0x040057A5 RID: 22437
	public const float MIN_TIME_BETWEEN_SICK_REACTS = 60f;

	// Token: 0x040057A6 RID: 22438
	public const int VOMITS_PER_CYCLE_MAJOR = 5;

	// Token: 0x040057A7 RID: 22439
	public const int VOMITS_PER_CYCLE_EXTREME = 10;

	// Token: 0x040057A8 RID: 22440
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter radiationExposure;

	// Token: 0x040057A9 RID: 22441
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter currentExposurePerCycle;

	// Token: 0x040057AA RID: 22442
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSick;

	// Token: 0x040057AB RID: 22443
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextExposureReact;

	// Token: 0x040057AC RID: 22444
	public StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeUntilNextSickReact;

	// Token: 0x040057AD RID: 22445
	public static string minorSicknessEffect = "RadiationExposureMinor";

	// Token: 0x040057AE RID: 22446
	public static string majorSicknessEffect = "RadiationExposureMajor";

	// Token: 0x040057AF RID: 22447
	public static string extremeSicknessEffect = "RadiationExposureExtreme";

	// Token: 0x040057B0 RID: 22448
	public static string bionic_minorSicknessEffect = "BionicRadiationExposureMinor";

	// Token: 0x040057B1 RID: 22449
	public static string bionic_majorSicknessEffect = "BionicRadiationExposureMajor";

	// Token: 0x040057B2 RID: 22450
	public static string bionic_extremeSicknessEffect = "BionicRadiationExposureExtreme";

	// Token: 0x040057B3 RID: 22451
	public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State init;

	// Token: 0x040057B4 RID: 22452
	public RadiationMonitor.ActiveStates active;

	// Token: 0x040057B5 RID: 22453
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_RECOVERY_IMMEDIATE = (RadiationMonitor.Instance smi, float p) => p > 100f * smi.difficultySettingMod / 2f;

	// Token: 0x040057B6 RID: 22454
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_REACT = (RadiationMonitor.Instance smi, float p) => p >= 133f * smi.difficultySettingMod;

	// Token: 0x040057B7 RID: 22455
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_LT_MINOR = (RadiationMonitor.Instance smi, float p) => p < 100f * smi.difficultySettingMod;

	// Token: 0x040057B8 RID: 22456
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MINOR = (RadiationMonitor.Instance smi, float p) => p >= 100f * smi.difficultySettingMod;

	// Token: 0x040057B9 RID: 22457
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_MAJOR = (RadiationMonitor.Instance smi, float p) => p >= 300f * smi.difficultySettingMod;

	// Token: 0x040057BA RID: 22458
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_EXTREME = (RadiationMonitor.Instance smi, float p) => p >= 600f * smi.difficultySettingMod;

	// Token: 0x040057BB RID: 22459
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_DEADLY = (RadiationMonitor.Instance smi, float p) => p >= 900f * smi.difficultySettingMod;

	// Token: 0x040057BC RID: 22460
	public static readonly StateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>.Callback COMPARE_GTE_NO_LONGER_DEADLY = (RadiationMonitor.Instance smi, float p) => p < 900f * smi.difficultySettingMod;

	// Token: 0x02001606 RID: 5638
	public class ActiveStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040057BD RID: 22461
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040057BE RID: 22462
		public RadiationMonitor.SickStates sick;
	}

	// Token: 0x02001607 RID: 5639
	public class SickStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040057BF RID: 22463
		public RadiationMonitor.SickStates.MinorStates minor;

		// Token: 0x040057C0 RID: 22464
		public RadiationMonitor.SickStates.MajorStates major;

		// Token: 0x040057C1 RID: 22465
		public RadiationMonitor.SickStates.ExtremeStates extreme;

		// Token: 0x040057C2 RID: 22466
		public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State deadly;

		// Token: 0x02001608 RID: 5640
		public class MinorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040057C3 RID: 22467
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040057C4 RID: 22468
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State reacting;
		}

		// Token: 0x02001609 RID: 5641
		public class MajorStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040057C5 RID: 22469
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040057C6 RID: 22470
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}

		// Token: 0x0200160A RID: 5642
		public class ExtremeStates : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040057C7 RID: 22471
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State waiting;

			// Token: 0x040057C8 RID: 22472
			public GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.State vomiting;
		}
	}

	// Token: 0x0200160B RID: 5643
	public new class Instance : GameStateMachine<RadiationMonitor, RadiationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074D6 RID: 29910 RVA: 0x00313B54 File Offset: 0x00311D54
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			if (Sim.IsRadiationEnabled())
			{
				SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Radiation);
				if (currentQualitySetting != null)
				{
					string id = currentQualitySetting.id;
					if (id == "Easiest")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIEST;
						return;
					}
					if (id == "Easier")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.EASIER;
						return;
					}
					if (id == "Harder")
					{
						this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDER;
						return;
					}
					if (!(id == "Hardest"))
					{
						return;
					}
					this.difficultySettingMod = DUPLICANTSTATS.RADIATION_DIFFICULTY_MODIFIERS.HARDEST;
				}
			}
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x000F1320 File Offset: 0x000EF520
		public float SicknessSecondsRemaining()
		{
			return 600f * (Mathf.Max(0f, base.sm.radiationExposure.Get(base.smi) - 100f * this.difficultySettingMod) / 100f);
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x00313C04 File Offset: 0x00311E04
		public string GetEffectStatusTooltip()
		{
			if (this.effects.HasEffect(RadiationMonitor.minorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.minorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.minorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.majorSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.majorSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.majorSicknessEffect));
			}
			if (this.effects.HasEffect(RadiationMonitor.extremeSicknessEffect))
			{
				return base.smi.master.gameObject.GetComponent<Effects>().Get(RadiationMonitor.extremeSicknessEffect).statusItem.GetTooltip(this.effects.Get(RadiationMonitor.extremeSicknessEffect));
			}
			return DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.TOOLTIP;
		}

		// Token: 0x040057C9 RID: 22473
		public Effects effects;

		// Token: 0x040057CA RID: 22474
		public float difficultySettingMod = 1f;
	}
}
