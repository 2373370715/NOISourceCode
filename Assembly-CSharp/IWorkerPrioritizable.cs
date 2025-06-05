using System;

// Token: 0x0200069A RID: 1690
public interface IWorkerPrioritizable
{
	// Token: 0x06001E06 RID: 7686
	bool GetWorkerPriority(WorkerBase worker, out int priority);
}
