using System;

// Token: 0x020007B6 RID: 1974
public abstract class GameplayEvent<StateMachineInstanceType> : GameplayEvent where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x06002305 RID: 8965 RVA: 0x000BB373 File Offset: 0x000B9573
	public GameplayEvent(string id, int priority = 0, int importance = 0) : base(id, priority, importance)
	{
	}
}
