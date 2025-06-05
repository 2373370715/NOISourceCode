using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000DCF RID: 3535
public class GeneticAnalysisStation : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>
{
	// Token: 0x060044E5 RID: 17637 RVA: 0x00257964 File Offset: 0x00255B64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy))).ToggleChore(new Func<GeneticAnalysisStation.StatesInstance, Chore>(this.CreateChore), new Action<GeneticAnalysisStation.StatesInstance, Chore>(GeneticAnalysisStation.SetRemoteChore), this.operational);
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x000D1007 File Offset: 0x000CF207
	private static void SetRemoteChore(GeneticAnalysisStation.StatesInstance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x060044E7 RID: 17639 RVA: 0x000D1015 File Offset: 0x000CF215
	private bool HasSeedToStudy(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.UnidentifiedSeed) >= 1f;
	}

	// Token: 0x060044E8 RID: 17640 RVA: 0x000AA9B7 File Offset: 0x000A8BB7
	private bool IsOperational(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x060044E9 RID: 17641 RVA: 0x00257A4C File Offset: 0x00255C4C
	private Chore CreateChore(GeneticAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<GeneticAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeSeed, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04002FD1 RID: 12241
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State inoperational;

	// Token: 0x04002FD2 RID: 12242
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State operational;

	// Token: 0x04002FD3 RID: 12243
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State ready;

	// Token: 0x02000DD0 RID: 3536
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000DD1 RID: 3537
	public class StatesInstance : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.GameInstance
	{
		// Token: 0x060044EC RID: 17644 RVA: 0x000D1039 File Offset: 0x000CF239
		public StatesInstance(IStateMachineTarget master, GeneticAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x000D104F File Offset: 0x000CF24F
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshFetchTags();
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00257A84 File Offset: 0x00255C84
		public void SetSeedForbidden(Tag seedID, bool forbidden)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			bool flag;
			if (forbidden)
			{
				flag = this.forbiddenSeeds.Add(seedID);
			}
			else
			{
				flag = this.forbiddenSeeds.Remove(seedID);
			}
			if (flag)
			{
				this.RefreshFetchTags();
			}
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x000D105D File Offset: 0x000CF25D
		public bool GetSeedForbidden(Tag seedID)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			return this.forbiddenSeeds.Contains(seedID);
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x00257ACC File Offset: 0x00255CCC
		private void RefreshFetchTags()
		{
			if (this.forbiddenSeeds == null)
			{
				this.manualDelivery.ForbiddenTags = null;
				return;
			}
			Tag[] array = new Tag[this.forbiddenSeeds.Count];
			int num = 0;
			foreach (Tag tag in this.forbiddenSeeds)
			{
				array[num++] = tag;
				this.storage.Drop(tag);
			}
			this.manualDelivery.ForbiddenTags = array;
		}

		// Token: 0x04002FD4 RID: 12244
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04002FD5 RID: 12245
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04002FD6 RID: 12246
		[MyCmpReq]
		public GeneticAnalysisStationWorkable workable;

		// Token: 0x04002FD7 RID: 12247
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04002FD8 RID: 12248
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
