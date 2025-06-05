using System;
using System.Collections.Generic;

// Token: 0x0200078B RID: 1931
public abstract class StandardChoreBase : Chore
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000BA799 File Offset: 0x000B8999
	// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000BA7A1 File Offset: 0x000B89A1
	public override int id { get; protected set; }

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060021E3 RID: 8675 RVA: 0x000BA7AA File Offset: 0x000B89AA
	// (set) Token: 0x060021E4 RID: 8676 RVA: 0x000BA7B2 File Offset: 0x000B89B2
	public override int priorityMod { get; protected set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060021E5 RID: 8677 RVA: 0x000BA7BB File Offset: 0x000B89BB
	// (set) Token: 0x060021E6 RID: 8678 RVA: 0x000BA7C3 File Offset: 0x000B89C3
	public override ChoreType choreType { get; protected set; }

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060021E7 RID: 8679 RVA: 0x000BA7CC File Offset: 0x000B89CC
	// (set) Token: 0x060021E8 RID: 8680 RVA: 0x000BA7D4 File Offset: 0x000B89D4
	public override ChoreDriver driver { get; protected set; }

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000BA7DD File Offset: 0x000B89DD
	// (set) Token: 0x060021EA RID: 8682 RVA: 0x000BA7E5 File Offset: 0x000B89E5
	public override ChoreDriver lastDriver { get; protected set; }

	// Token: 0x060021EB RID: 8683 RVA: 0x000BA7EE File Offset: 0x000B89EE
	public override bool SatisfiesUrge(Urge urge)
	{
		return urge == this.choreType.urge;
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000BA7FE File Offset: 0x000B89FE
	public override bool IsValid()
	{
		return this.provider != null && this.gameObject.GetMyWorldId() != -1;
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060021ED RID: 8685 RVA: 0x000BA821 File Offset: 0x000B8A21
	// (set) Token: 0x060021EE RID: 8686 RVA: 0x000BA829 File Offset: 0x000B8A29
	public override IStateMachineTarget target { get; protected set; }

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060021EF RID: 8687 RVA: 0x000BA832 File Offset: 0x000B8A32
	// (set) Token: 0x060021F0 RID: 8688 RVA: 0x000BA83A File Offset: 0x000B8A3A
	public override bool isComplete { get; protected set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060021F1 RID: 8689 RVA: 0x000BA843 File Offset: 0x000B8A43
	// (set) Token: 0x060021F2 RID: 8690 RVA: 0x000BA84B File Offset: 0x000B8A4B
	public override bool IsPreemptable { get; protected set; }

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060021F3 RID: 8691 RVA: 0x000BA854 File Offset: 0x000B8A54
	// (set) Token: 0x060021F4 RID: 8692 RVA: 0x000BA85C File Offset: 0x000B8A5C
	public override ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060021F5 RID: 8693 RVA: 0x000BA865 File Offset: 0x000B8A65
	// (set) Token: 0x060021F6 RID: 8694 RVA: 0x000BA86D File Offset: 0x000B8A6D
	public override Prioritizable prioritizable { get; protected set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000BA876 File Offset: 0x000B8A76
	// (set) Token: 0x060021F8 RID: 8696 RVA: 0x000BA87E File Offset: 0x000B8A7E
	public override ChoreProvider provider { get; set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000BA887 File Offset: 0x000B8A87
	// (set) Token: 0x060021FA RID: 8698 RVA: 0x000BA88F File Offset: 0x000B8A8F
	public override bool runUntilComplete { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060021FB RID: 8699 RVA: 0x000BA898 File Offset: 0x000B8A98
	// (set) Token: 0x060021FC RID: 8700 RVA: 0x000BA8A0 File Offset: 0x000B8AA0
	public override bool isExpanded { get; protected set; }

	// Token: 0x060021FD RID: 8701 RVA: 0x000BA8A9 File Offset: 0x000B8AA9
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return this.IsPreemptable;
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x000BA8B1 File Offset: 0x000B8AB1
	public override string GetReportName(string context = null)
	{
		if (context == null || this.choreType.reportName == null)
		{
			return this.choreType.Name;
		}
		return string.Format(this.choreType.reportName, context);
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x001CDF94 File Offset: 0x001CC194
	public override void Cancel(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000BA8E0 File Offset: 0x000B8AE0
	public override void Cleanup()
	{
		this.ClearPrioritizable();
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000BA8E8 File Offset: 0x000B8AE8
	public override ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x001CE00C File Offset: 0x001CC20C
	public override void AddPrecondition(Chore.Precondition precondition, object data = null)
	{
		this.arePreconditionsDirty = true;
		this.preconditions.Add(new Chore.PreconditionInstance
		{
			condition = precondition,
			data = data
		});
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x001CE044 File Offset: 0x001CC244
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context item = new Chore.Precondition.Context(this, consumer_state, is_attempting_override, null);
		item.RunPreconditions();
		if (!item.IsComplete())
		{
			incomplete_contexts.Add(item);
			return;
		}
		if (item.IsSuccess())
		{
			succeeded_contexts.Add(item);
			return;
		}
		failed_contexts.Add(item);
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000BA8F0 File Offset: 0x000B8AF0
	public override void Fail(string reason)
	{
		if (this.provider == null)
		{
			return;
		}
		if (this.driver == null)
		{
			return;
		}
		if (!this.runUntilComplete)
		{
			this.Cancel(reason);
			return;
		}
		this.End(reason);
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x001CE090 File Offset: 0x001CC290
	public override void Reserve(ChoreDriver reserver)
	{
		if (this.driver != null && this.driver != reserver && reserver != null)
		{
			Debug.LogErrorFormat("Chore.Reserve: driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				reserver
			});
		}
		this.driver = reserver;
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x001CE114 File Offset: 0x001CC314
	public override void Begin(Chore.Precondition.Context context)
	{
		if (this.driver != null && this.driver != context.consumerState.choreDriver)
		{
			Debug.LogErrorFormat("Chore.Begin driver already set {0} {1} {2}, provider {3}, driver {4} -> {5}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver,
				context.consumerState.choreDriver
			});
		}
		if (this.provider == null)
		{
			Debug.LogErrorFormat("Chore.Begin provider is null {0} {1} {2}, provider {3}, driver {4}", new object[]
			{
				this.id,
				base.GetType(),
				this.choreType.Id,
				this.provider,
				this.driver
			});
		}
		this.driver = context.consumerState.choreDriver;
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		KSelectable component = this.driver.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.GetStatusItem(), this);
		}
		smi.StartSM();
		if (this.onBegin != null)
		{
			this.onBegin(this);
		}
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000BA927 File Offset: 0x000B8B27
	public override bool InProgress()
	{
		return this.driver != null;
	}

	// Token: 0x06002209 RID: 8713
	protected abstract StateMachine.Instance GetSMI();

	// Token: 0x0600220A RID: 8714 RVA: 0x001CE278 File Offset: 0x001CC478
	public StandardChoreBase(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete, Action<Chore> on_complete, Action<Chore> on_begin, Action<Chore> on_end, PriorityScreen.PriorityClass priority_class, int priority_value, bool is_preemptable, bool allow_in_context_menu, int priority_mod, bool add_to_daily_report, ReportManager.ReportType report_type)
	{
		this.target = target;
		if (priority_value == 2147483647)
		{
			priority_class = PriorityScreen.PriorityClass.topPriority;
			priority_value = 2;
		}
		if (priority_value < 1 || priority_value > 9)
		{
			Debug.LogErrorFormat("Priority Value Out Of Range: {0}", new object[]
			{
				priority_value
			});
		}
		this.masterPriority = new PrioritySetting(priority_class, priority_value);
		this.priorityMod = priority_mod;
		this.id = Chore.GetNextChoreID();
		if (chore_provider == null)
		{
			chore_provider = GlobalChoreProvider.Instance;
			DebugUtil.Assert(chore_provider != null);
		}
		this.choreType = chore_type;
		this.runUntilComplete = run_until_complete;
		this.onComplete = on_complete;
		this.onEnd = on_end;
		this.onBegin = on_begin;
		this.IsPreemptable = is_preemptable;
		this.AddPrecondition(ChorePreconditions.instance.IsValid, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPermitted, null);
		this.AddPrecondition(ChorePreconditions.instance.IsPreemptable, null);
		this.AddPrecondition(ChorePreconditions.instance.HasUrge, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingEarly, null);
		this.AddPrecondition(ChorePreconditions.instance.IsMoreSatisfyingLate, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOverrideTargetNullOrMe, null);
		chore_provider.AddChore(this);
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x000BA935 File Offset: 0x000B8B35
	public virtual void SetPriorityMod(int priorityMod)
	{
		this.priorityMod = priorityMod;
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x001CE3BC File Offset: 0x001CC5BC
	public override List<Chore.PreconditionInstance> GetPreconditions()
	{
		if (this.arePreconditionsDirty)
		{
			List<Chore.PreconditionInstance> obj = this.preconditions;
			lock (obj)
			{
				if (this.arePreconditionsDirty)
				{
					this.preconditions.Sort((Chore.PreconditionInstance x, Chore.PreconditionInstance y) => x.condition.sortOrder.CompareTo(y.condition.sortOrder));
					this.arePreconditionsDirty = false;
				}
			}
		}
		return this.preconditions;
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x001CE440 File Offset: 0x001CC640
	protected void SetPrioritizable(Prioritizable prioritizable)
	{
		if (prioritizable != null && prioritizable.IsPrioritizable())
		{
			this.prioritizable = prioritizable;
			this.masterPriority = prioritizable.GetMasterPriority();
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000BA93E File Offset: 0x000B8B3E
	private void ClearPrioritizable()
	{
		if (this.prioritizable != null)
		{
			Prioritizable prioritizable = this.prioritizable;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnMasterPriorityChanged));
		}
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000BA975 File Offset: 0x000B8B75
	private void OnMasterPriorityChanged(PrioritySetting priority)
	{
		this.masterPriority = priority;
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000BA97E File Offset: 0x000B8B7E
	public void SetOverrideTarget(ChoreConsumer chore_consumer)
	{
		if (chore_consumer != null)
		{
			string name = chore_consumer.name;
		}
		this.overrideTarget = chore_consumer;
		this.Fail("New override target");
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x001CE494 File Offset: 0x001CC694
	protected virtual void End(string reason)
	{
		if (this.driver != null)
		{
			KSelectable component = this.driver.GetComponent<KSelectable>();
			if (component != null)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
			}
		}
		StateMachine.Instance smi = this.GetSMI();
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStateMachineStop));
		smi.StopSM(reason);
		if (this.driver == null)
		{
			return;
		}
		this.lastDriver = this.driver;
		this.driver = null;
		if (this.onEnd != null)
		{
			this.onEnd(this);
		}
		if (this.onExit != null)
		{
			this.onExit(this);
		}
		this.driver = null;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x001CE55C File Offset: 0x001CC75C
	protected void Succeed(string reason)
	{
		if (!this.RemoveFromProvider())
		{
			return;
		}
		this.isComplete = true;
		if (this.onComplete != null)
		{
			this.onComplete(this);
		}
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, null));
			SaveGame.Instance.ColonyAchievementTracker.LogSuitChore((this.driver != null) ? this.driver : this.lastDriver);
		}
		this.End(reason);
		this.Cleanup();
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000BA9A2 File Offset: 0x000B8BA2
	protected virtual StatusItem GetStatusItem()
	{
		return this.choreType.statusItem;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000BA9AF File Offset: 0x000B8BAF
	protected virtual void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (status == StateMachine.Status.Success)
		{
			this.Succeed(reason);
			return;
		}
		this.Fail(reason);
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000BA9C4 File Offset: 0x000B8BC4
	private bool RemoveFromProvider()
	{
		if (this.provider != null)
		{
			this.provider.RemoveChore(this);
			return true;
		}
		return false;
	}

	// Token: 0x040016D7 RID: 5847
	private Action<Chore> onBegin;

	// Token: 0x040016D8 RID: 5848
	private Action<Chore> onEnd;

	// Token: 0x040016D9 RID: 5849
	public Action<Chore> onCleanup;

	// Token: 0x040016DA RID: 5850
	private List<Chore.PreconditionInstance> preconditions = new List<Chore.PreconditionInstance>();

	// Token: 0x040016DB RID: 5851
	private bool arePreconditionsDirty;

	// Token: 0x040016DC RID: 5852
	public bool addToDailyReport;

	// Token: 0x040016DD RID: 5853
	public ReportManager.ReportType reportType;
}
