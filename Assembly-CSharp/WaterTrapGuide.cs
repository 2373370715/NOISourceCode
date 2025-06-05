using System;
using UnityEngine;

// Token: 0x02001A95 RID: 6805
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06008DEB RID: 36331 RVA: 0x001012E2 File Offset: 0x000FF4E2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06008DEC RID: 36332 RVA: 0x00101313 File Offset: 0x000FF513
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06008DED RID: 36333 RVA: 0x0010132B File Offset: 0x000FF52B
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06008DEE RID: 36334 RVA: 0x00377DFC File Offset: 0x00375FFC
	private void RefreshDepthAvailable()
	{
		int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
		if (depthAvailable != this.previousDepthAvailable)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0f);
			}
			if (this.occupyTiles)
			{
				WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06008DEF RID: 36335 RVA: 0x00101353 File Offset: 0x000FF553
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06008DF0 RID: 36336 RVA: 0x00377E80 File Offset: 0x00376080
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
			}
			else if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
			{
				Grid.ObjectLayers[1][key] = null;
			}
		}
	}

	// Token: 0x06008DF1 RID: 36337 RVA: 0x00377F00 File Offset: 0x00376100
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x04006B28 RID: 27432
	private int previousDepthAvailable = -1;

	// Token: 0x04006B29 RID: 27433
	public GameObject parent;

	// Token: 0x04006B2A RID: 27434
	public bool occupyTiles;

	// Token: 0x04006B2B RID: 27435
	private KBatchedAnimController parentController;

	// Token: 0x04006B2C RID: 27436
	private KBatchedAnimController guideController;
}
