using System;
using ImGuiNET;

// Token: 0x02000BE8 RID: 3048
public class DevToolEntity_SearchGameObjects : DevTool
{
	// Token: 0x060039C7 RID: 14791 RVA: 0x000C9DC5 File Offset: 0x000C7FC5
	public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x000C9DD4 File Offset: 0x000C7FD4
	protected override void RenderTo(DevPanel panel)
	{
		ImGui.Text("Not implemented yet");
	}

	// Token: 0x040027D6 RID: 10198
	private Action<DevToolEntityTarget> onSelectionMadeFn;
}
