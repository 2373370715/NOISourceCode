using System;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public class Chore<StateMachineInstanceType> : StandardChoreBase, IStateMachineTarget where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06002219 RID: 8729 RVA: 0x000BAA0D File Offset: 0x000B8C0D
	// (set) Token: 0x0600221A RID: 8730 RVA: 0x000BAA15 File Offset: 0x000B8C15
	public StateMachineInstanceType smi { get; protected set; }

	// Token: 0x0600221B RID: 8731 RVA: 0x000BAA1E File Offset: 0x000B8C1E
	protected override StateMachine.Instance GetSMI()
	{
		return this.smi;
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000BAA2B File Offset: 0x000B8C2B
	public int Subscribe(int hash, Action<object> handler)
	{
		return this.GetComponent<KPrefabID>().Subscribe(hash, handler);
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000BAA3A File Offset: 0x000B8C3A
	public void Unsubscribe(int hash, Action<object> handler)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(hash, handler);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000BAA49 File Offset: 0x000B8C49
	public void Unsubscribe(int id)
	{
		this.GetComponent<KPrefabID>().Unsubscribe(id);
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000BAA57 File Offset: 0x000B8C57
	public void Trigger(int hash, object data = null)
	{
		this.GetComponent<KPrefabID>().Trigger(hash, data);
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000BAA66 File Offset: 0x000B8C66
	public ComponentType GetComponent<ComponentType>()
	{
		return this.target.GetComponent<ComponentType>();
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06002221 RID: 8737 RVA: 0x000BAA73 File Offset: 0x000B8C73
	public override GameObject gameObject
	{
		get
		{
			return this.target.gameObject;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06002222 RID: 8738 RVA: 0x000BAA80 File Offset: 0x000B8C80
	public Transform transform
	{
		get
		{
			return this.target.gameObject.transform;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06002223 RID: 8739 RVA: 0x000BAA92 File Offset: 0x000B8C92
	public string name
	{
		get
		{
			return this.gameObject.name;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06002224 RID: 8740 RVA: 0x000BAA9F File Offset: 0x000B8C9F
	public override bool isNull
	{
		get
		{
			return this.target.isNull;
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x001CE5F0 File Offset: 0x001CC7F0
	public Chore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, PriorityScreen.PriorityClass master_priority_class = PriorityScreen.PriorityClass.basic, int master_priority_value = 5, bool is_preemptable = false, bool allow_in_context_menu = true, int priority_mod = 0, bool add_to_daily_report = false, ReportManager.ReportType report_type = ReportManager.ReportType.WorkTime) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, master_priority_class, master_priority_value, is_preemptable, allow_in_context_menu, priority_mod, add_to_daily_report, report_type)
	{
		target.Subscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		this.reportType = report_type;
		this.addToDailyReport = add_to_daily_report;
		if (this.addToDailyReport)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, chore_type.Name, GameUtil.GetChoreName(this, null));
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000BAAAC File Offset: 0x000B8CAC
	public override string ResolveString(string str)
	{
		if (!this.target.isNull)
		{
			str = str.Replace("{Target}", this.target.gameObject.GetProperName());
		}
		return base.ResolveString(str);
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x000BAADF File Offset: 0x000B8CDF
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.target != null)
		{
			this.target.Unsubscribe(1969584890, new Action<object>(this.OnTargetDestroyed));
		}
		if (this.onCleanup != null)
		{
			this.onCleanup(this);
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000BAB1F File Offset: 0x000B8D1F
	private void OnTargetDestroyed(object data)
	{
		this.Cancel("Target Destroyed");
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000BAB2C File Offset: 0x000B8D2C
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		return base.CanPreempt(context);
	}
}
