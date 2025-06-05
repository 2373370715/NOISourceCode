using System;
using UnityEngine;

// Token: 0x02001540 RID: 5440
public class BionicBedTimeMonitor : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>
{
	// Token: 0x06007148 RID: 29000 RVA: 0x00309C68 File Offset: 0x00307E68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notAllowed;
		this.notAllowed.ScheduleChange(this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime)).EventTransition(GameHashes.BionicOnline, this.bedTime, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime));
		this.bedTime.DefaultState(this.bedTime.runChore);
		this.bedTime.runChore.ToggleChore((BionicBedTimeMonitor.Instance smi) => new BionicBedTimeModeChore(smi.master), this.bedTime.choreEnded, this.bedTime.choreEnded).DefaultState(this.bedTime.runChore.notStarted);
		this.bedTime.runChore.notStarted.EventTransition(GameHashes.BeginChore, this.bedTime.runChore.running, new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning)).ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed, null);
		this.bedTime.runChore.running.EventTransition(GameHashes.EndChore, this.bedTime.runChore.notStarted, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.ChoreIsRunning))).DefaultState(this.bedTime.runChore.running.traveling);
		this.bedTime.runChore.running.traveling.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.defragmenting, false);
		this.bedTime.runChore.running.defragmenting.TagTransition(GameTags.BionicBedTime, this.bedTime.runChore.running.traveling, true).Enter(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.EnableLight)).Exit(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State.Callback(BionicBedTimeMonitor.DisableLight));
		this.bedTime.choreEnded.ScheduleChange(this.notAllowed, GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Not(new StateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.Transition.ConditionCallback(BionicBedTimeMonitor.CanGoToBedTime))).EventTransition(GameHashes.BionicOffline, this.notAllowed, null).GoTo(this.bedTime.runChore);
	}

	// Token: 0x06007149 RID: 29001 RVA: 0x000EE8E1 File Offset: 0x000ECAE1
	public static bool CanGoToBedTime(BionicBedTimeMonitor.Instance smi)
	{
		return BionicBedTimeMonitor.IsOnline(smi) && BionicBedTimeMonitor.ScheduleIsInBedTime(smi);
	}

	// Token: 0x0600714A RID: 29002 RVA: 0x000EE8F3 File Offset: 0x000ECAF3
	private static void EnableLight(BionicBedTimeMonitor.Instance smi)
	{
		smi.EnableLight();
	}

	// Token: 0x0600714B RID: 29003 RVA: 0x000EE8FB File Offset: 0x000ECAFB
	private static void DisableLight(BionicBedTimeMonitor.Instance smi)
	{
		smi.DisableLight();
	}

	// Token: 0x0600714C RID: 29004 RVA: 0x000EE903 File Offset: 0x000ECB03
	private static bool IsOnline(BionicBedTimeMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x0600714D RID: 29005 RVA: 0x000EE90B File Offset: 0x000ECB0B
	private static bool ScheduleIsInBedTime(BionicBedTimeMonitor.Instance smi)
	{
		return smi.IsScheduleInBedTime;
	}

	// Token: 0x0600714E RID: 29006 RVA: 0x00309EC4 File Offset: 0x003080C4
	public static bool ChoreIsRunning(BionicBedTimeMonitor.Instance smi)
	{
		ChoreDriver component = smi.GetComponent<ChoreDriver>();
		Chore chore = (component == null) ? null : component.GetCurrentChore();
		return chore != null && chore.choreType == Db.Get().ChoreTypes.BionicBedtimeMode;
	}

	// Token: 0x04005514 RID: 21780
	private const float LIGHT_RADIUS = 3f;

	// Token: 0x04005515 RID: 21781
	private const int LIGHT_LUX = 1800;

	// Token: 0x04005516 RID: 21782
	public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notAllowed;

	// Token: 0x04005517 RID: 21783
	public BionicBedTimeMonitor.BedTimeStates bedTime;

	// Token: 0x02001541 RID: 5441
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001542 RID: 5442
	public class DefragmentingStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x04005518 RID: 21784
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State traveling;

		// Token: 0x04005519 RID: 21785
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State defragmenting;
	}

	// Token: 0x02001543 RID: 5443
	public class ChoreStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x0400551A RID: 21786
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State notStarted;

		// Token: 0x0400551B RID: 21787
		public BionicBedTimeMonitor.DefragmentingStates running;
	}

	// Token: 0x02001544 RID: 5444
	public class BedTimeStates : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State
	{
		// Token: 0x0400551C RID: 21788
		public BionicBedTimeMonitor.ChoreStates runChore;

		// Token: 0x0400551D RID: 21789
		public GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.State choreEnded;
	}

	// Token: 0x02001545 RID: 5445
	public new class Instance : GameStateMachine<BionicBedTimeMonitor, BionicBedTimeMonitor.Instance, IStateMachineTarget, BionicBedTimeMonitor.Def>.GameInstance
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06007154 RID: 29012 RVA: 0x000EE923 File Offset: 0x000ECB23
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06007155 RID: 29013 RVA: 0x000EE93A File Offset: 0x000ECB3A
		public bool IsBedTimeChoreRunning
		{
			get
			{
				return this.prefabID.HasTag(GameTags.BionicBedTime);
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06007156 RID: 29014 RVA: 0x000EE94C File Offset: 0x000ECB4C
		public bool IsScheduleInBedTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep);
			}
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x000EE968 File Offset: 0x000ECB68
		public Instance(IStateMachineTarget master, BionicBedTimeMonitor.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x00309F08 File Offset: 0x00308108
		public void EnableLight()
		{
			this.lightSymbolTracker = base.gameObject.AddOrGet<LightSymbolTracker>();
			this.lightSymbolTracker.targetSymbol = "snapTo_mouth";
			this.lightSymbolTracker.enabled = true;
			this.light = base.gameObject.AddOrGet<Light2D>();
			this.light.Lux = 1800;
			this.light.Range = 3f;
			this.light.enabled = true;
			this.light.drawOverlay = true;
			this.light.Color = new Color(0f, 0.3137255f, 1f, 1f);
			this.light.overlayColour = new Color(1f, 1f, 1f, 1f);
			this.light.FullRefresh();
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x000EE99B File Offset: 0x000ECB9B
		public void DisableLight()
		{
			if (this.light != null)
			{
				this.light.enabled = false;
			}
			if (this.lightSymbolTracker != null)
			{
				this.lightSymbolTracker.enabled = false;
			}
		}

		// Token: 0x0400551E RID: 21790
		private Light2D light;

		// Token: 0x0400551F RID: 21791
		private LightSymbolTracker lightSymbolTracker;

		// Token: 0x04005520 RID: 21792
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04005521 RID: 21793
		private Schedulable schedulable;

		// Token: 0x04005522 RID: 21794
		private KPrefabID prefabID;
	}
}
