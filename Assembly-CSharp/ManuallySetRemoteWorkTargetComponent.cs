using System;

// Token: 0x0200179D RID: 6045
public class ManuallySetRemoteWorkTargetComponent : RemoteDockWorkTargetComponent
{
	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06007C53 RID: 31827 RVA: 0x000F63A3 File Offset: 0x000F45A3
	public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

	// Token: 0x06007C54 RID: 31828 RVA: 0x000F63AB File Offset: 0x000F45AB
	public void SetChore(Chore chore_)
	{
		this.chore = chore_;
	}

	// Token: 0x04005DA3 RID: 23971
	private Chore chore;
}
