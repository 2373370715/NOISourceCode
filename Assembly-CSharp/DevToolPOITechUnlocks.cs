using System;
using ImGuiNET;

// Token: 0x02000BF7 RID: 3063
public class DevToolPOITechUnlocks : DevTool
{
	// Token: 0x06003A17 RID: 14871 RVA: 0x00231100 File Offset: 0x0022F300
	protected override void RenderTo(DevPanel panel)
	{
		if (Research.Instance == null)
		{
			return;
		}
		foreach (TechItem techItem in Db.Get().TechItems.resources)
		{
			if (techItem.isPOIUnlock)
			{
				ImGui.Text(techItem.Id);
				ImGui.SameLine();
				bool flag = techItem.IsComplete();
				if (ImGui.Checkbox("Unlocked ", ref flag))
				{
					techItem.POIUnlocked();
				}
			}
		}
	}
}
