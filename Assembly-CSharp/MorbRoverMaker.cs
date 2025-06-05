using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class MorbRoverMaker : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x0019C678 File Offset: 0x0019A878
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.no_operational;
		this.root.Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.GermsRequiredFeedbackUpdate), UpdateRate.SIM_1000ms, false);
		this.no_operational.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.no_operational, true).DefaultState(this.operational.covered);
		this.operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty, null).ParamTransition<bool>(this.WasUncoverByDuplicant, this.operational.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue).Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe");
		}).DefaultState(this.operational.covered.idle);
		this.operational.covered.idle.PlayAnim("dusty").ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue);
		this.operational.covered.careOrderGiven.PlayAnim("dusty").Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_RevealMachine)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_RevealMachine)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_RevealMachine(), this.operational.covered.complete).ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsFalse);
		this.operational.covered.complete.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SetUncovered));
		this.operational.idle.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.EnableManualDelivery(smi, "Operational and discovered");
		}).EnterTransition(this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).EnterTransition(this.operational.waitingForMorb, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.IsCraftingCompleted)).EventTransition(GameHashes.OnStorageChange, this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.crafting.DefaultState(this.operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody, null);
		this.operational.crafting.conflict.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ResetRoverBodyCraftingProgress)).GoTo(this.operational.idle);
		this.operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).PlayAnim("crafting_pre").OnAnimQueueComplete(this.operational.crafting.loop);
		this.operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.CraftingUpdate), UpdateRate.SIM_200ms, false).PlayAnim("crafting_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.CraftProgress, this.operational.crafting.pst, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsOne);
		this.operational.crafting.pst.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ConsumeRoverBodyCraftingMaterials)).PlayAnim("crafting_pst").OnAnimQueueComplete(this.operational.waitingForMorb);
		this.operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition<long>(this.Germs, this.operational.doctor, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>.Callback(MorbRoverMaker.HasEnoughGerms)).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.doctor.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_ReleaseRover)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_ReleaseRover)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.finish).DefaultState(this.operational.doctor.needed);
		this.operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor, null);
		this.operational.doctor.working.WorkableStopTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.needed);
		this.operational.finish.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SpawnRover)).GoTo(this.operational.idle);
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x000B382D File Offset: 0x000B1A2D
	public static bool ShouldBeCrafting(MorbRoverMaker.Instance smi)
	{
		return smi.HasMaterialsForRover && smi.RoverDevelopment_Progress < 1f;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x000B3846 File Offset: 0x000B1A46
	public static bool IsCraftingCompleted(MorbRoverMaker.Instance smi)
	{
		return smi.RoverDevelopment_Progress == 1f;
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x000B3855 File Offset: 0x000B1A55
	public static bool HasEnoughGerms(MorbRoverMaker.Instance smi, long germCount)
	{
		return germCount >= smi.def.GERMS_PER_ROVER;
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x000B3868 File Offset: 0x000B1A68
	public static void StartWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_ReleaseRover();
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x000B3870 File Offset: 0x000B1A70
	public static void CancelWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_ReleaseRover();
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000B3878 File Offset: 0x000B1A78
	public static void StartWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_RevealMachine();
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x000B3880 File Offset: 0x000B1A80
	public static void CancelWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_RevealMachine();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000B3888 File Offset: 0x000B1A88
	public static void SetUncovered(MorbRoverMaker.Instance smi)
	{
		smi.Uncover();
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000B3890 File Offset: 0x000B1A90
	public static void SpawnRover(MorbRoverMaker.Instance smi)
	{
		smi.SpawnRover();
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x000B3898 File Offset: 0x000B1A98
	public static void EnableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.EnableManualDelivery(reason);
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x000B38A1 File Offset: 0x000B1AA1
	public static void DisableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.DisableManualDelivery(reason);
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x000B38AA File Offset: 0x000B1AAA
	public static void ConsumeRoverBodyCraftingMaterials(MorbRoverMaker.Instance smi)
	{
		smi.ConsumeRoverBodyCraftingMaterials();
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x000B38B2 File Offset: 0x000B1AB2
	public static void ResetRoverBodyCraftingProgress(MorbRoverMaker.Instance smi)
	{
		smi.SetRoverDevelopmentProgress(0f);
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x0019CC44 File Offset: 0x0019AE44
	public static void CraftingUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		float roverDevelopmentProgress = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0f, 1f);
		smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0019CC88 File Offset: 0x0019AE88
	public static void GermsRequiredFeedbackUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT & smi.MorbDevelopment_Progress < 1f & !smi.IsInsideState(smi.sm.operational.doctor) & smi.HasBeenRevealed)
		{
			smi.ShowGermRequiredStatusItemAlert();
			return;
		}
		smi.HideGermRequiredStatusItemAlert();
	}

	// Token: 0x04000E1A RID: 3610
	private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";

	// Token: 0x04000E1B RID: 3611
	private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";

	// Token: 0x04000E1C RID: 3612
	private const string COVERED_IDLE_ANIM_NAME = "dusty";

	// Token: 0x04000E1D RID: 3613
	private const string IDLE_ANIM_NAME = "idle";

	// Token: 0x04000E1E RID: 3614
	private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";

	// Token: 0x04000E1F RID: 3615
	private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";

	// Token: 0x04000E20 RID: 3616
	private const string CRAFT_PST_ANIM_NAME = "crafting_pst";

	// Token: 0x04000E21 RID: 3617
	private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";

	// Token: 0x04000E22 RID: 3618
	private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";

	// Token: 0x04000E23 RID: 3619
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter UncoverOrderRequested;

	// Token: 0x04000E24 RID: 3620
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter WasUncoverByDuplicant;

	// Token: 0x04000E25 RID: 3621
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.LongParameter Germs;

	// Token: 0x04000E26 RID: 3622
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.FloatParameter CraftProgress;

	// Token: 0x04000E27 RID: 3623
	public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State no_operational;

	// Token: 0x04000E28 RID: 3624
	public MorbRoverMaker.OperationalStates operational;

	// Token: 0x020004BE RID: 1214
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060014D7 RID: 5335 RVA: 0x0019CCF4 File Offset: 0x0019AEF4
		public float GetConduitMaxPackageMass()
		{
			ConduitType germ_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
			if (germ_INTAKE_CONDUIT_TYPE == ConduitType.Gas)
			{
				return 1f;
			}
			if (germ_INTAKE_CONDUIT_TYPE != ConduitType.Liquid)
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x04000E29 RID: 3625
		public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;

		// Token: 0x04000E2A RID: 3626
		public Tag ROVER_PREFAB_ID;

		// Token: 0x04000E2B RID: 3627
		public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;

		// Token: 0x04000E2C RID: 3628
		public float ROVER_CRAFTING_DURATION;

		// Token: 0x04000E2D RID: 3629
		public float METAL_PER_ROVER;

		// Token: 0x04000E2E RID: 3630
		public long GERMS_PER_ROVER;

		// Token: 0x04000E2F RID: 3631
		public int MAX_GERMS_TAKEN_PER_PACKAGE;

		// Token: 0x04000E30 RID: 3632
		public int GERM_TYPE;

		// Token: 0x04000E31 RID: 3633
		public SimHashes ROVER_MATERIAL;

		// Token: 0x04000E32 RID: 3634
		public ConduitType GERM_INTAKE_CONDUIT_TYPE;
	}

	// Token: 0x020004BF RID: 1215
	public class CoverStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E33 RID: 3635
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04000E34 RID: 3636
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State careOrderGiven;

		// Token: 0x04000E35 RID: 3637
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State complete;
	}

	// Token: 0x020004C0 RID: 1216
	public class OperationalStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E36 RID: 3638
		public MorbRoverMaker.CoverStates covered;

		// Token: 0x04000E37 RID: 3639
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04000E38 RID: 3640
		public MorbRoverMaker.CraftingStates crafting;

		// Token: 0x04000E39 RID: 3641
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State waitingForMorb;

		// Token: 0x04000E3A RID: 3642
		public MorbRoverMaker.DoctorStates doctor;

		// Token: 0x04000E3B RID: 3643
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State finish;
	}

	// Token: 0x020004C1 RID: 1217
	public class DoctorStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E3C RID: 3644
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State needed;

		// Token: 0x04000E3D RID: 3645
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State working;
	}

	// Token: 0x020004C2 RID: 1218
	public class CraftingStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E3E RID: 3646
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State conflict;

		// Token: 0x04000E3F RID: 3647
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pre;

		// Token: 0x04000E40 RID: 3648
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State loop;

		// Token: 0x04000E41 RID: 3649
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pst;
	}

	// Token: 0x020004C3 RID: 1219
	public new class Instance : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x000B38E2 File Offset: 0x000B1AE2
		public long MorbDevelopment_GermsCollected
		{
			get
			{
				return base.sm.Germs.Get(base.smi);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x000B38FA File Offset: 0x000B1AFA
		public long MorbDevelopment_RemainingGerms
		{
			get
			{
				return base.def.GERMS_PER_ROVER - this.MorbDevelopment_GermsCollected;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x000B390E File Offset: 0x000B1B0E
		public float MorbDevelopment_Progress
		{
			get
			{
				return Mathf.Clamp((float)this.MorbDevelopment_GermsCollected / (float)base.def.GERMS_PER_ROVER, 0f, 1f);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x000B3933 File Offset: 0x000B1B33
		public bool HasMaterialsForRover
		{
			get
			{
				return this.storage.GetMassAvailable(base.def.ROVER_MATERIAL) >= base.def.METAL_PER_ROVER;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x000B395B File Offset: 0x000B1B5B
		public float RoverDevelopment_Progress
		{
			get
			{
				return base.sm.CraftProgress.Get(base.smi);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x000B3973 File Offset: 0x000B1B73
		public bool HasBeenRevealed
		{
			get
			{
				return base.sm.WasUncoverByDuplicant.Get(base.smi);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x000B398B File Offset: 0x000B1B8B
		public bool CanPumpGerms
		{
			get
			{
				return this.operational && this.MorbDevelopment_Progress < 1f && this.HasBeenRevealed;
			}
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000B39AF File Offset: 0x000B1BAF
		public Workable GetWorkable_RevealMachine()
		{
			return this.workable_reveal;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x000B39B7 File Offset: 0x000B1BB7
		public Workable GetWorkable_ReleaseRover()
		{
			return this.workable_release;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0019CD24 File Offset: 0x0019AF24
		public void ShowGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle == default(Guid))
			{
				this.germsRequiredAlertStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, base.smi);
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0019CD70 File Offset: 0x0019AF70
		public void HideGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle != default(Guid))
			{
				this.selectable.RemoveStatusItem(this.germsRequiredAlertStatusItemHandle, false);
				this.germsRequiredAlertStatusItemHandle = default(Guid);
			}
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0019CDB4 File Offset: 0x0019AFB4
		public Instance(IStateMachineTarget master, MorbRoverMaker.Def def) : base(master, def)
		{
			this.RobotProgressMeter = new MeterController(this.buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0019CE1C File Offset: 0x0019B01C
		public override void StartSM()
		{
			Building component = base.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();
			base.StartSM();
			if (!this.HasBeenRevealed)
			{
				base.sm.Germs.Set(0L, base.smi, false);
				this.AddGerms((long)((float)base.def.GERMS_PER_ROVER * base.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
			}
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
			this.UpdateMeters();
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0019CEB8 File Offset: 0x0019B0B8
		public void AddGerms(long amount, bool playAnimations = true)
		{
			long value = this.MorbDevelopment_GermsCollected + amount;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			if (amount > 0L)
			{
				if (playAnimations)
				{
					this.capsule.PlayPumpGermsAnimation();
				}
				Action<long> germsAdded = this.GermsAdded;
				if (germsAdded != null)
				{
					germsAdded(amount);
				}
				this.lastTimeGermsAdded = GameClock.Instance.GetTime();
			}
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0019CF24 File Offset: 0x0019B124
		public long RemoveGerms(long amount)
		{
			long num = amount.Min(this.MorbDevelopment_GermsCollected);
			long value = this.MorbDevelopment_GermsCollected - num;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			return num;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x000B39BF File Offset: 0x000B1BBF
		public void EnableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(false, reason);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x000B39CE File Offset: 0x000B1BCE
		public void DisableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(true, reason);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x000B39DD File Offset: 0x000B1BDD
		public void SetRoverDevelopmentProgress(float value)
		{
			base.sm.CraftProgress.Set(value, base.smi, false);
			this.UpdateMeters();
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0019CF68 File Offset: 0x0019B168
		public void UpdateMeters()
		{
			this.RobotProgressMeter.SetPositionPercent(this.RoverDevelopment_Progress);
			this.capsule.SetMorbDevelopmentProgress(this.MorbDevelopment_Progress);
			this.capsule.SetGermMeterProgress(this.HasBeenRevealed ? this.MorbDevelopment_Progress : 0f);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000B39FE File Offset: 0x000B1BFE
		public void Uncover()
		{
			base.sm.WasUncoverByDuplicant.Set(true, base.smi, false);
			System.Action onUncovered = this.OnUncovered;
			if (onUncovered == null)
			{
				return;
			}
			onUncovered();
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0019CFB8 File Offset: 0x0019B1B8
		public void CreateWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover == null)
			{
				this.workChore_releaseRover = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, this.workable_release, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x000B3A29 File Offset: 0x000B1C29
		public void CancelWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover != null)
			{
				this.workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
				this.workChore_releaseRover = null;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0019D000 File Offset: 0x0019B200
		public void CreateWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine == null)
			{
				this.workChore_revealMachine = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, this.workable_reveal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000B3A4A File Offset: 0x000B1C4A
		public void CancelWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine != null)
			{
				this.workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
				this.workChore_revealMachine = null;
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0019D048 File Offset: 0x0019B248
		public void ConsumeRoverBodyCraftingMaterials()
		{
			float num = 0f;
			this.storage.ConsumeAndGetDisease(base.def.ROVER_MATERIAL.CreateTag(), base.def.METAL_PER_ROVER, out num, out this.lastastMaterialsConsumedDiseases, out this.lastastMaterialsConsumedTemp);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0019D090 File Offset: 0x0019B290
		public void SpawnRover()
		{
			if (this.RoverDevelopment_Progress == 1f)
			{
				this.RemoveGerms(base.def.GERMS_PER_ROVER);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(base.def.ROVER_PREFAB_ID), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.lastastMaterialsConsumedDiseases.idx != 255)
				{
					component.AddDisease(this.lastastMaterialsConsumedDiseases.idx, this.lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
				}
				if (this.lastastMaterialsConsumedTemp > 0f)
				{
					component.SetMassTemperature(component.Mass, this.lastastMaterialsConsumedTemp);
				}
				gameObject.SetActive(true);
				this.SetRoverDevelopmentProgress(0f);
				Action<GameObject> onRoverSpawned = this.OnRoverSpawned;
				if (onRoverSpawned == null)
				{
					return;
				}
				onRoverSpawned(gameObject);
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0019D168 File Offset: 0x0019B368
		private void Flow(float dt)
		{
			if (this.CanPumpGerms)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE);
				int num = 0;
				if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
				{
					ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
					ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
					float num2 = Mathf.Min(contents.mass, base.def.GetConduitMaxPackageMass() * dt);
					if (flowManager.CanMergeContents(contents, contents2, num2))
					{
						float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
						if (amountAllowedForMerging > 0f)
						{
							ConduitFlow conduitFlow = (base.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
							int num3 = contents.diseaseCount;
							if (contents.diseaseIdx != 255 && (int)contents.diseaseIdx == base.def.GERM_TYPE)
							{
								num = (int)this.MorbDevelopment_RemainingGerms.Min((long)base.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min((long)contents.diseaseCount);
								num3 -= num;
							}
							float num4 = conduitFlow.AddElement(this.outputCell, contents.element, amountAllowedForMerging, contents.temperature, contents.diseaseIdx, num3);
							if (amountAllowedForMerging != num4)
							{
								global::Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4).ToString());
							}
							flowManager.RemoveElement(this.inputCell, num4);
						}
					}
				}
				if (num > 0)
				{
					this.AddGerms((long)num, true);
				}
			}
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x000B3A6B File Offset: 0x000B1C6B
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060014F9 RID: 5369 RVA: 0x000B3A94 File Offset: 0x000B1C94
		public string SidescreenButtonText
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x000B3ACE File Offset: 0x000B1CCE
		public string SidescreenButtonTooltip
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP);
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x000B1628 File Offset: 0x000AF828
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0019D2EC File Offset: 0x0019B4EC
		public void OnSidescreenButtonPressed()
		{
			if (this.HasBeenRevealed)
			{
				this.storage.DropAll(false, false, default(Vector3), true, null);
				return;
			}
			bool flag = base.smi.sm.UncoverOrderRequested.Get(base.smi);
			base.smi.sm.UncoverOrderRequested.Set(!flag, base.smi, false);
		}

		// Token: 0x04000E42 RID: 3650
		public Action<long> GermsAdded;

		// Token: 0x04000E43 RID: 3651
		public System.Action OnUncovered;

		// Token: 0x04000E44 RID: 3652
		public Action<GameObject> OnRoverSpawned;

		// Token: 0x04000E45 RID: 3653
		[MyCmpGet]
		private MorbRoverMakerRevealWorkable workable_reveal;

		// Token: 0x04000E46 RID: 3654
		[MyCmpGet]
		private MorbRoverMakerWorkable workable_release;

		// Token: 0x04000E47 RID: 3655
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04000E48 RID: 3656
		[MyCmpGet]
		private KBatchedAnimController buildingAnimCtr;

		// Token: 0x04000E49 RID: 3657
		[MyCmpGet]
		private ManualDeliveryKG manualDelivery;

		// Token: 0x04000E4A RID: 3658
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04000E4B RID: 3659
		[MyCmpGet]
		private MorbRoverMaker_Capsule capsule;

		// Token: 0x04000E4C RID: 3660
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04000E4D RID: 3661
		private MeterController RobotProgressMeter;

		// Token: 0x04000E4E RID: 3662
		private int inputCell = -1;

		// Token: 0x04000E4F RID: 3663
		private int outputCell = -1;

		// Token: 0x04000E50 RID: 3664
		private Chore workChore_revealMachine;

		// Token: 0x04000E51 RID: 3665
		private Chore workChore_releaseRover;

		// Token: 0x04000E52 RID: 3666
		[Serialize]
		private float lastastMaterialsConsumedTemp = -1f;

		// Token: 0x04000E53 RID: 3667
		[Serialize]
		private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04000E54 RID: 3668
		public float lastTimeGermsAdded = -1f;

		// Token: 0x04000E55 RID: 3669
		private Guid germsRequiredAlertStatusItemHandle;
	}
}
