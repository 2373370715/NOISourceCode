using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	// Token: 0x06001C9D RID: 7325 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x000B74EE File Offset: 0x000B56EE
	protected override void OnSpawn()
	{
		this.prefabId = base.GetComponent<KPrefabID>();
		this.choreConsumer = base.GetComponent<ChoreConsumer>();
		this.running = true;
		Components.Brains.Add(this);
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06001C9F RID: 7327 RVA: 0x001B8C58 File Offset: 0x001B6E58
	// (remove) Token: 0x06001CA0 RID: 7328 RVA: 0x001B8C90 File Offset: 0x001B6E90
	public event System.Action onPreUpdate;

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000B751A File Offset: 0x000B571A
	public virtual void UpdateBrain()
	{
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (this.IsRunning())
		{
			this.UpdateChores();
		}
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x000B753D File Offset: 0x000B573D
	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return this.choreConsumer.FindNextChore(ref context);
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x001B8CC8 File Offset: 0x001B6EC8
	private void UpdateChores()
	{
		if (this.prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context chore = default(Chore.Precondition.Context);
		if (this.FindBetterChore(ref chore))
		{
			if (this.prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				base.Trigger(1485595942, null);
				return;
			}
			this.choreConsumer.choreDriver.SetChore(chore);
		}
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x000B754B File Offset: 0x000B574B
	public bool IsRunning()
	{
		return this.running && !this.suspend;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x000B7560 File Offset: 0x000B5760
	public void Reset(string reason)
	{
		this.Stop("Reset");
		this.running = true;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x000B7574 File Offset: 0x000B5774
	public void Stop(string reason)
	{
		base.GetComponent<ChoreDriver>().StopChore();
		this.running = false;
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x000B7588 File Offset: 0x000B5788
	public void Resume(string caller)
	{
		this.suspend = false;
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000B7591 File Offset: 0x000B5791
	public void Suspend(string caller)
	{
		this.suspend = true;
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x000B759A File Offset: 0x000B579A
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.Stop("OnCmpDisable");
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x000B75AD File Offset: 0x000B57AD
	protected override void OnCleanUp()
	{
		this.Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}

	// Token: 0x04001227 RID: 4647
	private bool running;

	// Token: 0x04001228 RID: 4648
	private bool suspend;

	// Token: 0x04001229 RID: 4649
	protected KPrefabID prefabId;

	// Token: 0x0400122A RID: 4650
	protected ChoreConsumer choreConsumer;
}
