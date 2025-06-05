using System;

// Token: 0x020007B9 RID: 1977
public class GameplayEventMinionFilter
{
	// Token: 0x04001794 RID: 6036
	public string id;

	// Token: 0x04001795 RID: 6037
	public GameplayEventMinionFilter.FilterFn filter;

	// Token: 0x020007BA RID: 1978
	// (Invoke) Token: 0x0600231A RID: 8986
	public delegate bool FilterFn(MinionIdentity minion);
}
