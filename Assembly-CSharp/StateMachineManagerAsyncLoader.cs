using System;
using System.Collections.Generic;

// Token: 0x02000927 RID: 2343
internal class StateMachineManagerAsyncLoader : GlobalAsyncLoader<StateMachineManagerAsyncLoader>
{
	// Token: 0x0600291A RID: 10522 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void Run()
	{
	}

	// Token: 0x04001BF4 RID: 7156
	public List<StateMachine> stateMachines = new List<StateMachine>();
}
