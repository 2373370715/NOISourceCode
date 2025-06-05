using System;

// Token: 0x0200111A RID: 4378
public interface IConduitFlow
{
	// Token: 0x06005985 RID: 22917
	void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default);

	// Token: 0x06005986 RID: 22918
	void RemoveConduitUpdater(Action<float> callback);

	// Token: 0x06005987 RID: 22919
	bool IsConduitEmpty(int cell);
}
