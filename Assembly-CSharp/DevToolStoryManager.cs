using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000C0F RID: 3087
public class DevToolStoryManager : DevTool
{
	// Token: 0x06003A8B RID: 14987 RVA: 0x000CA44C File Offset: 0x000C864C
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Story Instance Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawStoryInstanceData();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Story Telemetry Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawTelemetryData();
		}
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x00235588 File Offset: 0x00233788
	private void DrawStoryInstanceData()
	{
		if (StoryManager.Instance == null)
		{
			ImGui.Text("Couldn't find StoryManager instance");
			return;
		}
		ImGui.Text(string.Format("Stories (count: {0})", StoryManager.Instance.GetStoryInstances().Count));
		string str = (StoryManager.Instance.GetHighestCoordinate() == -2) ? "Before stories" : StoryManager.Instance.GetHighestCoordinate().ToString();
		ImGui.Text("Highest generated: " + str);
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in StoryManager.Instance.GetStoryInstances())
		{
			ImGui.Text(" - " + keyValuePair.Value.storyId + ": " + keyValuePair.Value.CurrentState.ToString());
		}
		if (StoryManager.Instance.GetStoryInstances().Count == 0)
		{
			ImGui.Text(" - No stories");
		}
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x002356A4 File Offset: 0x002338A4
	private void DrawTelemetryData()
	{
		ImGuiEx.DrawObjectTable<StoryManager.StoryTelemetry>("ID_telemetry", StoryManager.GetTelemetry(), null);
	}
}
