using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000CD0 RID: 3280
public class ArtifactAnalysisStation : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>
{
	// Token: 0x06003E99 RID: 16025 RVA: 0x00243264 File Offset: 0x00241464
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Not(new StateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.Transition.ConditionCallback(this.HasArtifactToStudy))).ToggleChore(new Func<ArtifactAnalysisStation.StatesInstance, Chore>(this.CreateChore), new Action<ArtifactAnalysisStation.StatesInstance, Chore>(ArtifactAnalysisStation.SetRemoteChore), this.operational);
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x000CD1C9 File Offset: 0x000CB3C9
	private static void SetRemoteChore(ArtifactAnalysisStation.StatesInstance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x000CD1D7 File Offset: 0x000CB3D7
	private bool HasArtifactToStudy(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.CharmedArtifact) >= 1f;
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x000AA9B7 File Offset: 0x000A8BB7
	private bool IsOperational(ArtifactAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0024334C File Offset: 0x0024154C
	private Chore CreateChore(ArtifactAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<ArtifactAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeArtifact, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04002B4C RID: 11084
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State inoperational;

	// Token: 0x04002B4D RID: 11085
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State operational;

	// Token: 0x04002B4E RID: 11086
	public GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.State ready;

	// Token: 0x02000CD1 RID: 3281
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000CD2 RID: 3282
	public class StatesInstance : GameStateMachine<ArtifactAnalysisStation, ArtifactAnalysisStation.StatesInstance, IStateMachineTarget, ArtifactAnalysisStation.Def>.GameInstance
	{
		// Token: 0x06003EA0 RID: 16032 RVA: 0x000CD1FB File Offset: 0x000CB3FB
		public StatesInstance(IStateMachineTarget master, ArtifactAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x000CD211 File Offset: 0x000CB411
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x04002B4F RID: 11087
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04002B50 RID: 11088
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04002B51 RID: 11089
		[MyCmpReq]
		public ArtifactAnalysisStationWorkable workable;

		// Token: 0x04002B52 RID: 11090
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04002B53 RID: 11091
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
