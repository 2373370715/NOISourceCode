using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016D1 RID: 5841
public class OnDemandUpdater : MonoBehaviour
{
	// Token: 0x06007885 RID: 30853 RVA: 0x000F3BC5 File Offset: 0x000F1DC5
	public static void DestroyInstance()
	{
		OnDemandUpdater.Instance = null;
	}

	// Token: 0x06007886 RID: 30854 RVA: 0x000F3BCD File Offset: 0x000F1DCD
	private void Awake()
	{
		OnDemandUpdater.Instance = this;
	}

	// Token: 0x06007887 RID: 30855 RVA: 0x000F3BD5 File Offset: 0x000F1DD5
	public void Register(IUpdateOnDemand updater)
	{
		if (!this.Updaters.Contains(updater))
		{
			this.Updaters.Add(updater);
		}
	}

	// Token: 0x06007888 RID: 30856 RVA: 0x000F3BF1 File Offset: 0x000F1DF1
	public void Unregister(IUpdateOnDemand updater)
	{
		if (this.Updaters.Contains(updater))
		{
			this.Updaters.Remove(updater);
		}
	}

	// Token: 0x06007889 RID: 30857 RVA: 0x0031FD1C File Offset: 0x0031DF1C
	private void Update()
	{
		for (int i = 0; i < this.Updaters.Count; i++)
		{
			if (this.Updaters[i] != null)
			{
				this.Updaters[i].UpdateOnDemand();
			}
		}
	}

	// Token: 0x04005A8C RID: 23180
	private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();

	// Token: 0x04005A8D RID: 23181
	public static OnDemandUpdater Instance;
}
