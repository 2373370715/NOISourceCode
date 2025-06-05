using System;

// Token: 0x020014A9 RID: 5289
public class UtilityBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06006D81 RID: 28033 RVA: 0x000EC5B3 File Offset: 0x000EA7B3
	public static void DestroyInstance()
	{
		UtilityBuildTool.Instance = null;
	}

	// Token: 0x06006D82 RID: 28034 RVA: 0x000EC5BB File Offset: 0x000EA7BB
	protected override void OnPrefabInit()
	{
		UtilityBuildTool.Instance = this;
		base.OnPrefabInit();
		this.populateHitsList = true;
		this.canChangeDragAxis = false;
	}

	// Token: 0x06006D83 RID: 28035 RVA: 0x002FA18C File Offset: 0x002F838C
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, cell2);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					string text;
					if (this.conduitMgr.CanAddConnection(utilityConnections, cell, false, out text) && this.conduitMgr.CanAddConnection(new_connection, cell2, false, out text))
					{
						this.conduitMgr.AddConnection(utilityConnections, cell, false);
						this.conduitMgr.AddConnection(new_connection, cell2, false);
					}
					else if (i == this.path.Count - 1 && this.lastPathHead != i)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, null, Grid.CellToPosCCC(cell2, (Grid.SceneLayer)0), 1.5f, false, false);
					}
				}
			}
		}
		this.lastPathHead = this.path.Count - 1;
	}

	// Token: 0x0400528A RID: 21130
	public static UtilityBuildTool Instance;

	// Token: 0x0400528B RID: 21131
	private int lastPathHead = -1;
}
