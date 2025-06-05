using System;

// Token: 0x0200179B RID: 6043
public abstract class RemoteWorkable : Workable, IRemoteDockWorkTarget
{
	// Token: 0x06007C49 RID: 31817 RVA: 0x000F631E File Offset: 0x000F451E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06007C4A RID: 31818 RVA: 0x000F633C File Offset: 0x000F453C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06007C4B RID: 31819
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06007C4C RID: 31820 RVA: 0x000BC493 File Offset: 0x000BA693
	public virtual IApproachable Approachable
	{
		get
		{
			return this;
		}
	}
}
