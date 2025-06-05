using System;

// Token: 0x02000B87 RID: 2951
public abstract class MinionTracker : Tracker
{
	// Token: 0x06003759 RID: 14169 RVA: 0x000C8619 File Offset: 0x000C6819
	public MinionTracker(MinionIdentity identity)
	{
		this.identity = identity;
	}

	// Token: 0x04002617 RID: 9751
	public MinionIdentity identity;
}
