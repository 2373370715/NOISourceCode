using System;

// Token: 0x020014AA RID: 5290
public class WireBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06006D85 RID: 28037 RVA: 0x000EC5E6 File Offset: 0x000EA7E6
	public static void DestroyInstance()
	{
		WireBuildTool.Instance = null;
	}

	// Token: 0x06006D86 RID: 28038 RVA: 0x000EC5EE File Offset: 0x000EA7EE
	protected override void OnPrefabInit()
	{
		WireBuildTool.Instance = this;
		base.OnPrefabInit();
		this.viewMode = OverlayModes.Power.ID;
	}

	// Token: 0x06006D87 RID: 28039 RVA: 0x002FA2C8 File Offset: 0x002F84C8
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
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, this.path[i].cell);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					this.conduitMgr.AddConnection(utilityConnections, cell, false);
					this.conduitMgr.AddConnection(new_connection, cell2, false);
				}
			}
		}
	}

	// Token: 0x0400528C RID: 21132
	public static WireBuildTool Instance;
}
