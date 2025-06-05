using System;
using UnityEngine;

// Token: 0x02000B00 RID: 2816
[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06003431 RID: 13361 RVA: 0x000C67FA File Offset: 0x000C49FA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x000C682B File Offset: 0x000C4A2B
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x000C6853 File Offset: 0x000C4A53
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x00216A9C File Offset: 0x00214C9C
	private void RefreshDepthAvailable()
	{
		int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
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
				PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x000C686B File Offset: 0x000C4A6B
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x00216B20 File Offset: 0x00214D20
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			int key2 = Grid.OffsetCell(cell, 1, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
				Grid.ObjectLayers[1][key2] = go;
			}
			else
			{
				if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
				{
					Grid.ObjectLayers[1][key] = null;
				}
				if (Grid.ObjectLayers[1].ContainsKey(key2) && Grid.ObjectLayers[1][key2] == go)
				{
					Grid.ObjectLayers[1][key2] = null;
				}
			}
		}
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x00216BF0 File Offset: 0x00214DF0
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			int num3 = Grid.OffsetCell(root_cell, 1, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(num3) || Grid.Solid[num3] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)) || (Grid.ObjectLayers[1].ContainsKey(num3) && !(Grid.ObjectLayers[1][num3] == null) && !(Grid.ObjectLayers[1][num3] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x040023BC RID: 9148
	private int previousDepthAvailable = -1;

	// Token: 0x040023BD RID: 9149
	public GameObject parent;

	// Token: 0x040023BE RID: 9150
	public bool occupyTiles;

	// Token: 0x040023BF RID: 9151
	private KBatchedAnimController parentController;

	// Token: 0x040023C0 RID: 9152
	private KBatchedAnimController guideController;
}
