using System;
using UnityEngine;

// Token: 0x02000945 RID: 2373
public class KAnimLayering
{
	// Token: 0x06002A06 RID: 10758 RVA: 0x000BFC90 File Offset: 0x000BDE90
	public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
	{
		this.controller = controller;
		this.layer = layer;
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x001E4874 File Offset: 0x001E2A74
	public void SetLayer(Grid.SceneLayer layer)
	{
		this.layer = layer;
		if (this.foregroundController != null)
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.transform.SetLocalPosition(position);
		}
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000BFCAE File Offset: 0x000BDEAE
	public void SetIsForeground(bool is_foreground)
	{
		this.isForeground = is_foreground;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000BFCB7 File Offset: 0x000BDEB7
	public bool GetIsForeground()
	{
		return this.isForeground;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x000BFCBF File Offset: 0x000BDEBF
	public KAnimLink GetLink()
	{
		return this.link;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x001E48E0 File Offset: 0x001E2AE0
	private static bool IsAnimLayered(KAnimFile[] anims)
	{
		foreach (KAnimFile kanimFile in anims)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x001E4948 File Offset: 0x001E2B48
	private void HideSymbolsInternal()
	{
		foreach (KAnimFile kanimFile in this.controller.AnimFiles)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0 != this.isForeground && !(symbols[j].hash == KAnimLayering.UI))
						{
							this.controller.SetSymbolVisiblity(symbols[j].hash, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x001E49F4 File Offset: 0x001E2BF4
	public void HideSymbols()
	{
		if (EntityPrefabs.Instance == null)
		{
			return;
		}
		if (this.isForeground)
		{
			return;
		}
		KAnimFile[] animFiles = this.controller.AnimFiles;
		bool flag = KAnimLayering.IsAnimLayered(animFiles);
		if (flag && this.layer != Grid.SceneLayer.NoLayer)
		{
			bool flag2 = this.foregroundController == null;
			if (flag2)
			{
				GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject, null);
				gameObject.name = this.controller.name + "_fg";
				this.foregroundController = gameObject.GetComponent<KAnimControllerBase>();
				this.link = new KAnimLink(this.controller, this.foregroundController);
			}
			this.foregroundController.AnimFiles = animFiles;
			this.foregroundController.GetLayering().SetIsForeground(true);
			this.foregroundController.initialAnim = this.controller.initialAnim;
			this.Dirty();
			KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
			if (flag2)
			{
				synchronizer.Add(this.foregroundController);
			}
			else
			{
				this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
			}
			synchronizer.Sync(this.foregroundController);
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(this.layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.gameObject.transform.SetLocalPosition(position);
			this.foregroundController.gameObject.SetActive(true);
		}
		else if (!flag && this.foregroundController != null)
		{
			this.controller.GetSynchronizer().Remove(this.foregroundController);
			this.foregroundController.gameObject.DeleteObject();
			this.link = null;
		}
		if (this.foregroundController != null)
		{
			this.HideSymbolsInternal();
			KAnimLayering layering = this.foregroundController.GetLayering();
			if (layering != null)
			{
				layering.HideSymbolsInternal();
			}
		}
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000BFCC7 File Offset: 0x000BDEC7
	public void RefreshForegroundBatchGroup()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x001E4BF8 File Offset: 0x001E2DF8
	public void Dirty()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.Offset = this.controller.Offset;
		this.foregroundController.Pivot = this.controller.Pivot;
		this.foregroundController.Rotation = this.controller.Rotation;
		this.foregroundController.FlipX = this.controller.FlipX;
		this.foregroundController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04001C89 RID: 7305
	private bool isForeground;

	// Token: 0x04001C8A RID: 7306
	private KAnimControllerBase controller;

	// Token: 0x04001C8B RID: 7307
	private KAnimControllerBase foregroundController;

	// Token: 0x04001C8C RID: 7308
	private KAnimLink link;

	// Token: 0x04001C8D RID: 7309
	private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04001C8E RID: 7310
	public static readonly KAnimHashedString UI = new KAnimHashedString("ui");
}
