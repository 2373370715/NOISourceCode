using System;
using STRINGS;

// Token: 0x02000794 RID: 1940
public class ChoreDriver : StateMachineComponent<ChoreDriver.StatesInstance>
{
	// Token: 0x06002264 RID: 8804 RVA: 0x000BADBF File Offset: 0x000B8FBF
	public Chore GetCurrentChore()
	{
		return base.smi.GetCurrentChore();
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000BADCC File Offset: 0x000B8FCC
	public bool HasChore()
	{
		return base.smi.GetCurrentChore() != null;
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000BADDC File Offset: 0x000B8FDC
	public void StopChore()
	{
		base.smi.sm.stop.Trigger(base.smi);
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x001CF5D8 File Offset: 0x001CD7D8
	public void SetChore(Chore.Precondition.Context context)
	{
		Chore currentChore = base.smi.GetCurrentChore();
		if (currentChore != context.chore)
		{
			this.StopChore();
			if (context.chore.IsValid())
			{
				context.chore.PrepareChore(ref context);
				this.context = context;
				base.smi.sm.nextChore.Set(context.chore, base.smi, false);
				return;
			}
			string text = "Null";
			string text2 = "Null";
			if (currentChore != null)
			{
				text = currentChore.GetType().Name;
			}
			if (context.chore != null)
			{
				text2 = context.chore.GetType().Name;
			}
			Debug.LogWarning(string.Concat(new string[]
			{
				"Stopping chore ",
				text,
				" to start ",
				text2,
				" but stopping the first chore cancelled the second one."
			}));
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000BADF9 File Offset: 0x000B8FF9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04001718 RID: 5912
	[MyCmpAdd]
	private User user;

	// Token: 0x04001719 RID: 5913
	private Chore.Precondition.Context context;

	// Token: 0x02000795 RID: 1941
	public class StatesInstance : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.GameInstance
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x000BAE14 File Offset: 0x000B9014
		// (set) Token: 0x0600226B RID: 8811 RVA: 0x000BAE1C File Offset: 0x000B901C
		public string masterProperName { get; private set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x000BAE25 File Offset: 0x000B9025
		// (set) Token: 0x0600226D RID: 8813 RVA: 0x000BAE2D File Offset: 0x000B902D
		public KPrefabID masterPrefabId { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600226E RID: 8814 RVA: 0x000BAE36 File Offset: 0x000B9036
		// (set) Token: 0x0600226F RID: 8815 RVA: 0x000BAE3E File Offset: 0x000B903E
		public Navigator navigator { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x000BAE47 File Offset: 0x000B9047
		// (set) Token: 0x06002271 RID: 8817 RVA: 0x000BAE4F File Offset: 0x000B904F
		public WorkerBase worker { get; private set; }

		// Token: 0x06002272 RID: 8818 RVA: 0x001CF6AC File Offset: 0x001CD8AC
		public StatesInstance(ChoreDriver master) : base(master)
		{
			this.masterProperName = base.master.GetProperName();
			this.masterPrefabId = base.master.GetComponent<KPrefabID>();
			this.navigator = base.master.GetComponent<Navigator>();
			this.worker = base.master.GetComponent<WorkerBase>();
			this.choreConsumer = base.GetComponent<ChoreConsumer>();
			ChoreConsumer choreConsumer = this.choreConsumer;
			choreConsumer.choreRulesChanged = (System.Action)Delegate.Combine(choreConsumer.choreRulesChanged, new System.Action(this.OnChoreRulesChanged));
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x001CF738 File Offset: 0x001CD938
		public void BeginChore()
		{
			Chore nextChore = this.GetNextChore();
			Chore chore = base.smi.sm.currentChore.Set(nextChore, base.smi, false);
			if (chore != null && chore.IsPreemptable && chore.driver != null)
			{
				chore.Fail("Preemption!");
			}
			base.smi.sm.nextChore.Set(null, base.smi, false);
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, new Action<Chore>(this.OnChoreExit));
			chore.Begin(base.master.context);
			base.Trigger(-1988963660, chore);
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x001CF7EC File Offset: 0x001CD9EC
		public void EndChore(string reason)
		{
			if (this.GetCurrentChore() != null)
			{
				Chore currentChore = this.GetCurrentChore();
				base.smi.sm.currentChore.Set(null, base.smi, false);
				Chore chore = currentChore;
				chore.onExit = (Action<Chore>)Delegate.Remove(chore.onExit, new Action<Chore>(this.OnChoreExit));
				currentChore.Fail(reason);
				base.Trigger(1745615042, currentChore);
			}
			if (base.smi.choreConsumer.prioritizeBrainIfNoChore)
			{
				Game.BrainScheduler.PrioritizeBrain(this.brain);
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000BAE58 File Offset: 0x000B9058
		private void OnChoreExit(Chore chore)
		{
			base.smi.sm.stop.Trigger(base.smi);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000BAE75 File Offset: 0x000B9075
		public Chore GetNextChore()
		{
			return base.smi.sm.nextChore.Get(base.smi);
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000BAE92 File Offset: 0x000B9092
		public Chore GetCurrentChore()
		{
			return base.smi.sm.currentChore.Get(base.smi);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x001CF880 File Offset: 0x001CDA80
		private void OnChoreRulesChanged()
		{
			Chore currentChore = this.GetCurrentChore();
			if (currentChore != null && !this.choreConsumer.IsPermittedOrEnabled(currentChore.choreType, currentChore))
			{
				this.EndChore("Permissions changed");
			}
		}

		// Token: 0x0400171E RID: 5918
		private ChoreConsumer choreConsumer;

		// Token: 0x0400171F RID: 5919
		[MyCmpGet]
		private Brain brain;
	}

	// Token: 0x02000796 RID: 1942
	public class States : GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver>
	{
		// Token: 0x06002279 RID: 8825 RVA: 0x001CF8B8 File Offset: 0x001CDAB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.nochore;
			this.saveHistory = true;
			this.nochore.Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.HasTag(GameTags.BaseMinion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WorkTime, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.TIME_SPENT, DUPLICANTS.CHORES.THINKING.NAME), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).ParamTransition<Chore>(this.nextChore, this.haschore, (ChoreDriver.StatesInstance smi, Chore next_chore) => next_chore != null);
			this.haschore.Enter("BeginChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.BeginChore();
			}).Update(delegate(ChoreDriver.StatesInstance smi, float dt)
			{
				if (smi.masterPrefabId.HasTag(GameTags.BaseMinion) && !smi.masterPrefabId.HasTag(GameTags.Dead))
				{
					Chore chore = this.currentChore.Get(smi);
					if (chore == null)
					{
						return;
					}
					if (smi.navigator.IsMoving())
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.TravelTime, dt, GameUtil.GetChoreName(chore, null), smi.master.GetProperName());
						return;
					}
					ReportManager.ReportType reportType = chore.GetReportType();
					Workable workable = smi.worker.GetWorkable();
					if (workable != null)
					{
						ReportManager.ReportType reportType2 = workable.GetReportType();
						if (reportType != reportType2)
						{
							reportType = reportType2;
						}
					}
					ReportManager.Instance.ReportValue(reportType, dt, string.Format(UI.ENDOFDAYREPORT.NOTES.WORK_TIME, GameUtil.GetChoreName(chore, null)), smi.master.GetProperName());
				}
			}, UpdateRate.SIM_200ms, false).Exit("EndChore", delegate(ChoreDriver.StatesInstance smi)
			{
				smi.EndChore("ChoreDriver.SignalStop");
			}).OnSignal(this.stop, this.nochore);
		}

		// Token: 0x04001720 RID: 5920
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> currentChore;

		// Token: 0x04001721 RID: 5921
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.ObjectParameter<Chore> nextChore;

		// Token: 0x04001722 RID: 5922
		public StateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.Signal stop;

		// Token: 0x04001723 RID: 5923
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State nochore;

		// Token: 0x04001724 RID: 5924
		public GameStateMachine<ChoreDriver.States, ChoreDriver.StatesInstance, ChoreDriver, object>.State haschore;
	}
}
