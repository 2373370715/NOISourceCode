using System;

// Token: 0x020007AD RID: 1965
public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
	// Token: 0x060022D3 RID: 8915
	public abstract void StartUsing(User user);

	// Token: 0x060022D4 RID: 8916 RVA: 0x001D1108 File Offset: 0x001CF308
	protected void StartUsing(StateMachine.Instance smi, User user)
	{
		DebugUtil.Assert(this.smi == null);
		DebugUtil.Assert(smi != null);
		this.smi = smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
		smi.StartSM();
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x001D115C File Offset: 0x001CF35C
	public void StopUsing(User user)
	{
		if (this.smi != null)
		{
			StateMachine.Instance instance = this.smi;
			instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(instance.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
			this.smi.StopSM("Usable.StopUsing");
			this.smi = null;
		}
	}

	// Token: 0x0400175D RID: 5981
	private StateMachine.Instance smi;
}
