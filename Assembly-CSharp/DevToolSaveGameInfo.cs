using System;
using ImGuiNET;

// Token: 0x02000BFB RID: 3067
public class DevToolSaveGameInfo : DevTool
{
	// Token: 0x06003A20 RID: 14880 RVA: 0x00231888 File Offset: 0x0022FA88
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
		ImGui.Text("Content Enabled: ");
		foreach (string text in SaveLoader.Instance.GameInfo.dlcIds)
		{
			string str = (text == "") ? "VANILLA_ID" : text;
			ImGui.Text(" - " + str);
		}
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref this.clSearch, 10U);
		ImGui.PopItemWidth();
		foreach (uint num in Game.Instance.changelistsPlayedOn)
		{
			if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
			{
				ImGui.Text(num.ToString());
			}
		}
		ImGui.NewLine();
	}

	// Token: 0x0400282B RID: 10283
	private string clSearch = "";
}
