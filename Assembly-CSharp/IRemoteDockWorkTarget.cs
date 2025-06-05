using System;

// Token: 0x0200179A RID: 6042
public interface IRemoteDockWorkTarget
{
	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06007C47 RID: 31815
	Chore RemoteDockChore { get; }

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06007C48 RID: 31816
	IApproachable Approachable { get; }
}
