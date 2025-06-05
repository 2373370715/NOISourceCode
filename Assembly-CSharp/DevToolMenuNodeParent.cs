using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000BF2 RID: 3058
public class DevToolMenuNodeParent : IMenuNode
{
	// Token: 0x06003A00 RID: 14848 RVA: 0x000C9FB5 File Offset: 0x000C81B5
	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		this.children = new List<IMenuNode>();
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x000C9FCF File Offset: 0x000C81CF
	public void AddChild(IMenuNode menuNode)
	{
		this.children.Add(menuNode);
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x000C9FDD File Offset: 0x000C81DD
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x002307B4 File Offset: 0x0022E9B4
	public void Draw()
	{
		if (ImGui.BeginMenu(this.name))
		{
			foreach (IMenuNode menuNode in this.children)
			{
				menuNode.Draw();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x04002817 RID: 10263
	public string name;

	// Token: 0x04002818 RID: 10264
	public List<IMenuNode> children;
}
