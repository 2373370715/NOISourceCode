using System;

// Token: 0x020008E8 RID: 2280
public abstract class StateEvent
{
	// Token: 0x060027E8 RID: 10216 RVA: 0x000BE670 File Offset: 0x000BC870
	public StateEvent(string name)
	{
		this.name = name;
		this.debugName = "(Event)" + name;
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000BE690 File Offset: 0x000BC890
	public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
	{
		return new StateEvent.Context(this);
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
	{
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000BE698 File Offset: 0x000BC898
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x000BE6A0 File Offset: 0x000BC8A0
	public string GetDebugName()
	{
		return this.debugName;
	}

	// Token: 0x04001B80 RID: 7040
	protected string name;

	// Token: 0x04001B81 RID: 7041
	private string debugName;

	// Token: 0x020008E9 RID: 2281
	public struct Context
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x000BE6A8 File Offset: 0x000BC8A8
		public Context(StateEvent state_event)
		{
			this.stateEvent = state_event;
			this.data = 0;
		}

		// Token: 0x04001B82 RID: 7042
		public StateEvent stateEvent;

		// Token: 0x04001B83 RID: 7043
		public int data;
	}
}
