using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000C04 RID: 3076
public class DevToolSpaceScannerNetwork : DevTool
{
	// Token: 0x06003A40 RID: 14912 RVA: 0x00234318 File Offset: 0x00232518
	public DevToolSpaceScannerNetwork()
	{
		this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (DevToolSpaceScannerNetwork.Entry e) => e.worldId).Column("Network Quality (0->1)", (DevToolSpaceScannerNetwork.Entry e) => e.networkQuality).Column("Targets Detected", (DevToolSpaceScannerNetwork.Entry e) => e.targetsString).FixedHeight(300f).Build();
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x002343C0 File Offset: 0x002325C0
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("Game instance is null");
			return;
		}
		if (Game.Instance.spaceScannerNetworkManager == null)
		{
			ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
			return;
		}
		if (ClusterManager.Instance == null)
		{
			ImGui.Text("ClusterManager instance is null");
			return;
		}
		if (ImGui.CollapsingHeader("Worlds Data"))
		{
			this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
		}
		if (ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
		{
			ImGuiEx.DrawObject(Game.Instance.spaceScannerNetworkManager, null);
		}
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x000CA19D File Offset: 0x000C839D
	public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
		}
		List<WorldContainer>.Enumerator enumerator = default(List<WorldContainer>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06003A43 RID: 14915 RVA: 0x00234454 File Offset: 0x00232654
	public static string GetTargetsString(WorldContainer world)
	{
		SpaceScannerWorldData spaceScannerWorldData;
		if (!Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out spaceScannerWorldData))
		{
			return "<none>";
		}
		if (spaceScannerWorldData.targetIdsDetected.Count == 0)
		{
			return "<none>";
		}
		return string.Join(",", spaceScannerWorldData.targetIdsDetected);
	}

	// Token: 0x0400285E RID: 10334
	private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

	// Token: 0x02000C05 RID: 3077
	public readonly struct Entry
	{
		// Token: 0x06003A44 RID: 14916 RVA: 0x000CA1A6 File Offset: 0x000C83A6
		public Entry(int worldId, float networkQuality, string targetsString)
		{
			this.worldId = worldId;
			this.networkQuality = networkQuality;
			this.targetsString = targetsString;
		}

		// Token: 0x0400285F RID: 10335
		public readonly int worldId;

		// Token: 0x04002860 RID: 10336
		public readonly float networkQuality;

		// Token: 0x04002861 RID: 10337
		public readonly string targetsString;
	}
}
