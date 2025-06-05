using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FA6 RID: 4006
public class RocketControlStation : StateMachineComponent<RocketControlStation.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06005095 RID: 20629 RVA: 0x000D8F3A File Offset: 0x000D713A
	// (set) Token: 0x06005096 RID: 20630 RVA: 0x000D8F42 File Offset: 0x000D7142
	public bool RestrictWhenGrounded
	{
		get
		{
			return this.m_restrictWhenGrounded;
		}
		set
		{
			this.m_restrictWhenGrounded = value;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x06005097 RID: 20631 RVA: 0x0027DCD4 File Offset: 0x0027BED4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.RocketControlStations.Add(this);
		base.Subscribe<RocketControlStation>(-801688580, RocketControlStation.OnLogicValueChangedDelegate);
		base.Subscribe<RocketControlStation>(1861523068, RocketControlStation.OnRocketRestrictionChanged);
		this.UpdateRestrictionAnimSymbol(null);
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x000D8F57 File Offset: 0x000D7157
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RocketControlStations.Remove(this);
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06005099 RID: 20633 RVA: 0x0027DD28 File Offset: 0x0027BF28
	public bool BuildingRestrictionsActive
	{
		get
		{
			if (this.IsLogicInputConnected())
			{
				return this.m_logicUsageRestrictionState;
			}
			base.smi.sm.AquireClustercraft(base.smi, false);
			GameObject gameObject = base.smi.sm.clusterCraft.Get(base.smi);
			return this.RestrictWhenGrounded && gameObject != null && gameObject.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x0600509A RID: 20634 RVA: 0x000D8F6A File Offset: 0x000D716A
	public bool IsLogicInputConnected()
	{
		return this.GetNetwork() != null;
	}

	// Token: 0x0600509B RID: 20635 RVA: 0x0027DD9C File Offset: 0x0027BF9C
	public void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RocketControlStation.PORT_ID)
		{
			LogicCircuitNetwork network = this.GetNetwork();
			int value = (network != null) ? network.OutputValue : 1;
			bool logicUsageRestrictionState = LogicCircuitNetwork.IsBitActive(0, value);
			this.m_logicUsageRestrictionState = logicUsageRestrictionState;
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x0600509C RID: 20636 RVA: 0x000D8F75 File Offset: 0x000D7175
	public void OnTagsChanged(object obj)
	{
		if (((TagChangedEventData)obj).tag == GameTags.RocketOnGround)
		{
			base.Trigger(1861523068, null);
		}
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x0027DDF0 File Offset: 0x0027BFF0
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RocketControlStation.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x0600509E RID: 20638 RVA: 0x000D8F9A File Offset: 0x000D719A
	private void UpdateRestrictionAnimSymbol(object o = null)
	{
		base.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("restriction_sign", this.BuildingRestrictionsActive);
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x0027DE20 File Offset: 0x0027C020
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_HEADER, Descriptor.DescriptorType.Effect, false));
		string newValue = string.Join(", ", (from t in RocketControlStation.CONTROLLED_BUILDINGS
		select Strings.Get("STRINGS.BUILDINGS.PREFABS." + t.Name.ToUpper() + ".NAME").String).ToArray<string>());
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.ROCKETRESTRICTION_BUILDINGS.text.Replace("{buildinglist}", newValue), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x040038C3 RID: 14531
	public static List<Tag> CONTROLLED_BUILDINGS = new List<Tag>();

	// Token: 0x040038C4 RID: 14532
	private const int UNNETWORKED_VALUE = 1;

	// Token: 0x040038C5 RID: 14533
	[Serialize]
	public float TimeRemaining;

	// Token: 0x040038C6 RID: 14534
	private bool m_logicUsageRestrictionState;

	// Token: 0x040038C7 RID: 14535
	[Serialize]
	private bool m_restrictWhenGrounded;

	// Token: 0x040038C8 RID: 14536
	public static readonly HashedString PORT_ID = "LogicUsageRestriction";

	// Token: 0x040038C9 RID: 14537
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040038CA RID: 14538
	private static readonly EventSystem.IntraObjectHandler<RocketControlStation> OnRocketRestrictionChanged = new EventSystem.IntraObjectHandler<RocketControlStation>(delegate(RocketControlStation component, object data)
	{
		component.UpdateRestrictionAnimSymbol(data);
	});

	// Token: 0x02000FA7 RID: 4007
	public class States : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation>
	{
		// Token: 0x060050A2 RID: 20642 RVA: 0x0027DF20 File Offset: 0x0027C120
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.unoperational;
			this.root.Enter("SetTarget", delegate(RocketControlStation.StatesInstance smi)
			{
				this.AquireClustercraft(smi, true);
			}).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, 1f);
			});
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_4000ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).EventTransition(GameHashes.LaunchConditionChanged, this.launch, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch)).Target(this.masterTarget).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
			this.launch.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateLaunchChore), this.operational).Transition(this.launch.fadein, new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight), UpdateRate.SIM_200ms).Target(this.clusterCraft).EventTransition(GameHashes.RocketRequestLaunch, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).EventTransition(GameHashes.LaunchConditionChanged, this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.RocketReadyForLaunch))).Target(this.masterTarget);
			this.launch.fadein.Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				if (CameraController.Instance.cameraActiveCluster == this.clusterCraft.Get(smi).GetComponent<WorldContainer>().id)
				{
					CameraController.Instance.FadeIn(0f, 1f, null);
				}
			});
			this.running.PlayAnim("on").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).ParamTransition<float>(this.timeRemaining, this.ready, (RocketControlStation.StatesInstance smi, float p) => p <= 0f).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(30f, smi, false);
			});
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<RocketControlStation.StatesInstance, Chore>(this.CreateChore), this.ready.post, this.ready).Transition(this.operational, GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Not(new StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Transition.ConditionCallback(this.IsInFlight)), UpdateRate.SIM_200ms).OnSignal(this.pilotSuccessful, this.ready.post).Update("Decrement time", new Action<RocketControlStation.StatesInstance, float>(this.DecrementTime), UpdateRate.SIM_200ms, false);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ParamTransition<float>(this.timeRemaining, this.ready.warning, (RocketControlStation.StatesInstance smi, float p) => p <= 15f);
			this.ready.warning.PlayAnim("on_alert", KAnim.PlayMode.Loop).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).ToggleMainStatusItem(Db.Get().BuildingStatusItems.PilotNeeded, null).ParamTransition<float>(this.timeRemaining, this.ready.autopilot, (RocketControlStation.StatesInstance smi, float p) => p <= 0f);
			this.ready.autopilot.PlayAnim("on_failed", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().BuildingStatusItems.AutoPilotActive, null).WorkableStartTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.working).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 0.5f, smi.pilotSpeedMult);
			});
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(RocketControlStation.StatesInstance smi)
			{
				this.SetRocketSpeedModifiers(smi, 1f, smi.pilotSpeedMult);
			}).WorkableStopTransition((RocketControlStation.StatesInstance smi) => smi.master.GetComponent<RocketControlStationIdleWorkable>(), this.ready.idle);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.running).Exit(delegate(RocketControlStation.StatesInstance smi)
			{
				this.timeRemaining.Set(120f, smi, false);
			});
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x0027E44C File Offset: 0x0027C64C
		public void AquireClustercraft(RocketControlStation.StatesInstance smi, bool force = false)
		{
			if (force || this.clusterCraft.IsNull(smi))
			{
				GameObject rocket = this.GetRocket(smi);
				this.clusterCraft.Set(rocket, smi, false);
				if (rocket != null)
				{
					rocket.Subscribe(-1582839653, new Action<object>(smi.master.OnTagsChanged));
				}
			}
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x000D8FBF File Offset: 0x000D71BF
		private void DecrementTime(RocketControlStation.StatesInstance smi, float dt)
		{
			this.timeRemaining.Delta(-dt, smi);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0027E4A8 File Offset: 0x0027C6A8
		private bool RocketReadyForLaunch(RocketControlStation.StatesInstance smi)
		{
			Clustercraft component = this.clusterCraft.Get(smi).GetComponent<Clustercraft>();
			return component.LaunchRequested && component.CheckReadyToLaunch();
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0027E4D8 File Offset: 0x0027C6D8
		private GameObject GetRocket(RocketControlStation.StatesInstance smi)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(smi.GetMyWorldId());
			if (world == null)
			{
				return null;
			}
			return world.gameObject.GetComponent<Clustercraft>().gameObject;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x000D8FD0 File Offset: 0x000D71D0
		private void SetRocketSpeedModifiers(RocketControlStation.StatesInstance smi, float autoPilotSpeedMultiplier, float pilotSkillMultiplier = 1f)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().AutoPilotMultiplier = autoPilotSpeedMultiplier;
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().PilotSkillMultiplier = pilotSkillMultiplier;
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0027E514 File Offset: 0x0027C714
		private Chore CreateChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationIdleWorkable>();
			WorkChore<RocketControlStationIdleWorkable> workChore = new WorkChore<RocketControlStationIdleWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			workChore.AddPrecondition(ChorePreconditions.instance.IsRocketTravelling, null);
			return workChore;
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x0027E594 File Offset: 0x0027C794
		private Chore CreateLaunchChore(RocketControlStation.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<RocketControlStationLaunchWorkable>();
			WorkChore<RocketControlStationLaunchWorkable> workChore = new WorkChore<RocketControlStationLaunchWorkable>(Db.Get().ChoreTypes.RocketControl, component, null, true, null, null, null, true, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.topPriority, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRocketControlStation);
			return workChore;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x000D9000 File Offset: 0x000D7200
		public void LaunchRocket(RocketControlStation.StatesInstance smi)
		{
			this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Launch(false);
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x000D9019 File Offset: 0x000D7219
		public bool IsInFlight(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x000D9034 File Offset: 0x000D7234
		public bool IsLaunching(RocketControlStation.StatesInstance smi)
		{
			return this.clusterCraft.Get(smi).GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Launching;
		}

		// Token: 0x040038CB RID: 14539
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.TargetParameter clusterCraft;

		// Token: 0x040038CC RID: 14540
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State unoperational;

		// Token: 0x040038CD RID: 14541
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State operational;

		// Token: 0x040038CE RID: 14542
		private GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State running;

		// Token: 0x040038CF RID: 14543
		private RocketControlStation.States.ReadyStates ready;

		// Token: 0x040038D0 RID: 14544
		private RocketControlStation.States.LaunchStates launch;

		// Token: 0x040038D1 RID: 14545
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.Signal pilotSuccessful;

		// Token: 0x040038D2 RID: 14546
		public StateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.FloatParameter timeRemaining;

		// Token: 0x02000FA8 RID: 4008
		public class ReadyStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x040038D3 RID: 14547
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State idle;

			// Token: 0x040038D4 RID: 14548
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State working;

			// Token: 0x040038D5 RID: 14549
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State post;

			// Token: 0x040038D6 RID: 14550
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State warning;

			// Token: 0x040038D7 RID: 14551
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State autopilot;
		}

		// Token: 0x02000FA9 RID: 4009
		public class LaunchStates : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State
		{
			// Token: 0x040038D8 RID: 14552
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State launch;

			// Token: 0x040038D9 RID: 14553
			public GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.State fadein;
		}
	}

	// Token: 0x02000FAB RID: 4011
	public class StatesInstance : GameStateMachine<RocketControlStation.States, RocketControlStation.StatesInstance, RocketControlStation, object>.GameInstance
	{
		// Token: 0x060050C4 RID: 20676 RVA: 0x000D912D File Offset: 0x000D732D
		public StatesInstance(RocketControlStation smi) : base(smi)
		{
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x000D9141 File Offset: 0x000D7341
		public void LaunchRocket()
		{
			base.sm.LaunchRocket(this);
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x0027E5F4 File Offset: 0x0027C7F4
		public void SetPilotSpeedMult(WorkerBase pilot)
		{
			AttributeConverter pilotingSpeed = Db.Get().AttributeConverters.PilotingSpeed;
			AttributeConverterInstance converter = pilot.GetComponent<AttributeConverters>().GetConverter(pilotingSpeed.Id);
			float a = 1f + converter.Evaluate();
			this.pilotSpeedMult = Mathf.Max(a, 0.1f);
		}

		// Token: 0x040038E2 RID: 14562
		public float pilotSpeedMult = 1f;
	}
}
