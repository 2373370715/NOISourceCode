using System;
using UnityEngine;

// Token: 0x02000949 RID: 2377
public class KAnimSynchronizedController
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000BFDF7 File Offset: 0x000BDFF7
	// (set) Token: 0x06002A1E RID: 10782 RVA: 0x000BFDFF File Offset: 0x000BDFFF
	public string Postfix
	{
		get
		{
			return this.postfix;
		}
		set
		{
			this.postfix = value;
		}
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x001E4ED0 File Offset: 0x001E30D0
	public KAnimSynchronizedController(KAnimControllerBase controller, Grid.SceneLayer layer, string postfix)
	{
		this.controller = controller;
		this.Postfix = postfix;
		GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject, null);
		gameObject.name = controller.name + postfix;
		this.synchronizedController = gameObject.GetComponent<KAnimControllerBase>();
		this.synchronizedController.AnimFiles = controller.AnimFiles;
		gameObject.SetActive(true);
		this.synchronizedController.initialAnim = controller.initialAnim + postfix;
		this.synchronizedController.defaultAnim = this.synchronizedController.initialAnim;
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - 0.1f);
		gameObject.transform.SetLocalPosition(position);
		this.link = new KAnimLink(controller, this.synchronizedController);
		this.Dirty();
		KAnimSynchronizer synchronizer = controller.GetSynchronizer();
		synchronizer.Add(this);
		synchronizer.SyncController(this);
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000BFE08 File Offset: 0x000BE008
	public void Enable(bool enable)
	{
		this.synchronizedController.enabled = enable;
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000BFE16 File Offset: 0x000BE016
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.synchronizedController.enabled && this.synchronizedController.HasAnimation(anim_name))
		{
			this.synchronizedController.Play(anim_name, mode, speed, time_offset);
		}
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x001E4FC0 File Offset: 0x001E31C0
	public void Dirty()
	{
		if (this.synchronizedController == null)
		{
			return;
		}
		this.synchronizedController.Offset = this.controller.Offset;
		this.synchronizedController.Pivot = this.controller.Pivot;
		this.synchronizedController.Rotation = this.controller.Rotation;
		this.synchronizedController.FlipX = this.controller.FlipX;
		this.synchronizedController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04001C9A RID: 7322
	private KAnimControllerBase controller;

	// Token: 0x04001C9B RID: 7323
	public KAnimControllerBase synchronizedController;

	// Token: 0x04001C9C RID: 7324
	private KAnimLink link;

	// Token: 0x04001C9D RID: 7325
	private string postfix;
}
