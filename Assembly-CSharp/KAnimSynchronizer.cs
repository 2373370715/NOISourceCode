using System;
using System.Collections.Generic;

// Token: 0x0200094A RID: 2378
public class KAnimSynchronizer
{
	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06002A23 RID: 10787 RVA: 0x000BFE43 File Offset: 0x000BE043
	// (set) Token: 0x06002A24 RID: 10788 RVA: 0x000BFE4B File Offset: 0x000BE04B
	public string IdleAnim
	{
		get
		{
			return this.idle_anim;
		}
		set
		{
			this.idle_anim = value;
		}
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000BFE54 File Offset: 0x000BE054
	public KAnimSynchronizer(KAnimControllerBase master_controller)
	{
		this.masterController = master_controller;
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x000BFE84 File Offset: 0x000BE084
	private void Clear(KAnimControllerBase controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000BFEA2 File Offset: 0x000BE0A2
	public void Add(KAnimControllerBase controller)
	{
		this.Targets.Add(controller);
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000BFEB0 File Offset: 0x000BE0B0
	public void Remove(KAnimControllerBase controller)
	{
		this.Clear(controller);
		this.Targets.Remove(controller);
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000BFEC6 File Offset: 0x000BE0C6
	public void RemoveWithoutIdleAnim(KAnimControllerBase controller)
	{
		this.Targets.Remove(controller);
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x000BFED5 File Offset: 0x000BE0D5
	private void Clear(KAnimSynchronizedController controller)
	{
		controller.Play(this.IdleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x000BFEF3 File Offset: 0x000BE0F3
	public void Add(KAnimSynchronizedController controller)
	{
		this.SyncedControllers.Add(controller);
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000BFF01 File Offset: 0x000BE101
	public void Remove(KAnimSynchronizedController controller)
	{
		this.Clear(controller);
		this.SyncedControllers.Remove(controller);
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x001E504C File Offset: 0x001E324C
	public void Clear()
	{
		foreach (KAnimControllerBase kanimControllerBase in this.Targets)
		{
			if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
			{
				this.Clear(kanimControllerBase);
			}
		}
		this.Targets.Clear();
		foreach (KAnimSynchronizedController kanimSynchronizedController in this.SyncedControllers)
		{
			if (!(kanimSynchronizedController.synchronizedController == null) && kanimSynchronizedController.synchronizedController.AnimFiles != null)
			{
				this.Clear(kanimSynchronizedController);
			}
		}
		this.SyncedControllers.Clear();
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x001E5124 File Offset: 0x001E3324
	public void Sync(KAnimControllerBase controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		if (currentAnim != null && !string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation(currentAnim.name))
		{
			controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
		Facing component = controller.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.FlipX = this.masterController.FlipX;
		controller.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x001E5244 File Offset: 0x001E3444
	public void SyncController(KAnimSynchronizedController controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		string s = (currentAnim != null) ? (currentAnim.name + controller.Postfix) : string.Empty;
		if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation(s))
		{
			controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(s, mode, playSpeed, elapsedTime);
		Facing component = controller.synchronizedController.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.synchronizedController.FlipX = this.masterController.FlipX;
		controller.synchronizedController.FlipY = this.masterController.FlipY;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x001E538C File Offset: 0x001E358C
	public void Sync()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			KAnimControllerBase controller = this.Targets[i];
			this.Sync(controller);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			KAnimSynchronizedController controller2 = this.SyncedControllers[j];
			this.SyncController(controller2);
		}
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x001E53F0 File Offset: 0x001E35F0
	public void SyncTime()
	{
		float elapsedTime = this.masterController.GetElapsedTime();
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.Targets[i].SetElapsedTime(elapsedTime);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			this.SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
		}
	}

	// Token: 0x04001C9E RID: 7326
	private string idle_anim = "idle_default";

	// Token: 0x04001C9F RID: 7327
	private KAnimControllerBase masterController;

	// Token: 0x04001CA0 RID: 7328
	private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();

	// Token: 0x04001CA1 RID: 7329
	private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
}
