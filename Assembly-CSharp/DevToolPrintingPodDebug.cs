using System;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
public class DevToolPrintingPodDebug : DevTool
{
	// Token: 0x06003A1A RID: 14874 RVA: 0x000CA09D File Offset: 0x000C829D
	protected override void RenderTo(DevPanel panel)
	{
		if (Immigration.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x002315F4 File Offset: 0x0022F7F4
	private void ShowButtons()
	{
		if (Components.Telepads.Count == 0)
		{
			ImGui.Text("No printing pods available");
			return;
		}
		ImGui.Text("Time until next print available: " + Mathf.CeilToInt(Immigration.Instance.timeBeforeSpawn).ToString() + "s");
		if (ImGui.Button("Activate now"))
		{
			Immigration.Instance.timeBeforeSpawn = 0f;
		}
		if (ImGui.Button("Shuffle Options"))
		{
			if (ImmigrantScreen.instance.Telepad == null)
			{
				ImmigrantScreen.InitializeImmigrantScreen(Components.Telepads[0]);
				return;
			}
			ImmigrantScreen.instance.DebugShuffleOptions();
		}
	}
}
