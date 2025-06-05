using System;

// Token: 0x0200091D RID: 2333
public static class StateMachineExtensions
{
	// Token: 0x060028E4 RID: 10468 RVA: 0x000BEFF7 File Offset: 0x000BD1F7
	public static bool IsNullOrStopped(this StateMachine.Instance smi)
	{
		return smi == null || !smi.IsRunning();
	}
}
