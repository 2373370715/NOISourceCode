using System;
using System.IO;
using ImGuiNET;

// Token: 0x02000BEF RID: 3055
public class DevToolMenuNodeList
{
	// Token: 0x060039F7 RID: 14839 RVA: 0x00230688 File Offset: 0x0022E888
	public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
	{
		string[] array = Path.GetDirectoryName(childPath).Split('/', StringSplitOptions.None);
		string text = "";
		DevToolMenuNodeParent devToolMenuNodeParent = this.root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string split = array2[i];
			text += devToolMenuNodeParent.GetName();
			IMenuNode menuNode = devToolMenuNodeParent.children.Find((IMenuNode x) => x.GetName() == split);
			DevToolMenuNodeParent devToolMenuNodeParent3;
			if (menuNode != null)
			{
				DevToolMenuNodeParent devToolMenuNodeParent2 = menuNode as DevToolMenuNodeParent;
				if (devToolMenuNodeParent2 == null)
				{
					throw new Exception("Conflict! Both a leaf and parent node exist at path: " + text);
				}
				devToolMenuNodeParent3 = devToolMenuNodeParent2;
			}
			else
			{
				devToolMenuNodeParent3 = new DevToolMenuNodeParent(split);
				devToolMenuNodeParent.AddChild(devToolMenuNodeParent3);
			}
			devToolMenuNodeParent = devToolMenuNodeParent3;
		}
		return devToolMenuNodeParent;
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x00230734 File Offset: 0x0022E934
	public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
	{
		DevToolMenuNodeAction devToolMenuNodeAction = new DevToolMenuNodeAction(Path.GetFileName(path), onClickFn);
		this.AddOrGetParentFor(path).AddChild(devToolMenuNodeAction);
		return devToolMenuNodeAction;
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0023075C File Offset: 0x0022E95C
	public void Draw()
	{
		foreach (IMenuNode menuNode in this.root.children)
		{
			menuNode.Draw();
		}
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x000C9F76 File Offset: 0x000C8176
	public void DrawFull()
	{
		if (ImGui.BeginMainMenuBar())
		{
			this.Draw();
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x04002815 RID: 10261
	private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");
}
