using System;

// Token: 0x0200179C RID: 6044
public abstract class RemoteDockWorkTargetComponent : KMonoBehaviour, IRemoteDockWorkTarget
{
	// Token: 0x06007C4E RID: 31822 RVA: 0x000F635A File Offset: 0x000F455A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06007C4F RID: 31823 RVA: 0x000F6378 File Offset: 0x000F4578
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06007C50 RID: 31824
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06007C51 RID: 31825 RVA: 0x000F6396 File Offset: 0x000F4596
	public virtual IApproachable Approachable
	{
		get
		{
			return base.gameObject.GetComponent<IApproachable>();
		}
	}
}
