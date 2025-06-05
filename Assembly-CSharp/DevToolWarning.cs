using System;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000C1B RID: 3099
public class DevToolWarning
{
	// Token: 0x06003ABE RID: 15038 RVA: 0x000CA66B File Offset: 0x000C886B
	public DevToolWarning()
	{
		this.Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x000CA683 File Offset: 0x000C8883
	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x00236148 File Offset: 0x00234348
	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			if (!isOpen)
			{
				ImGui.End();
				return;
			}
			ImGui.SetWindowSize(new Vector2(500f, 250f));
			ImGui.TextWrapped(UI.FRONTEND.DEVTOOLS.WARNING);
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
			if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
			{
				if (this.showAgain)
				{
					KPlayerPrefs.SetInt("ShowDevtools", 1);
				}
				DevToolManager.Instance.UserAcceptedWarning = true;
				isOpen = false;
			}
			ImGui.End();
		}
	}

	// Token: 0x0400289F RID: 10399
	private bool showAgain;

	// Token: 0x040028A0 RID: 10400
	public string Name;

	// Token: 0x040028A1 RID: 10401
	public bool ShouldDrawWindow;
}
