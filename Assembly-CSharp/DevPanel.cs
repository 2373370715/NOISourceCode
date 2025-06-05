using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000BC9 RID: 3017
public class DevPanel
{
	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x000C9705 File Offset: 0x000C7905
	// (set) Token: 0x06003921 RID: 14625 RVA: 0x000C970D File Offset: 0x000C790D
	public bool isRequestingToClose { get; private set; }

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06003922 RID: 14626 RVA: 0x000C9716 File Offset: 0x000C7916
	// (set) Token: 0x06003923 RID: 14627 RVA: 0x000C971E File Offset: 0x000C791E
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06003924 RID: 14628 RVA: 0x000C9727 File Offset: 0x000C7927
	// (set) Token: 0x06003925 RID: 14629 RVA: 0x000C972F File Offset: 0x000C792F
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	// Token: 0x06003926 RID: 14630 RVA: 0x0022A21C File Offset: 0x0022841C
	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x0022A278 File Offset: 0x00228478
	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x000C9738 File Offset: 0x000C7938
	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x000C9746 File Offset: 0x000C7946
	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x0022A2AC File Offset: 0x002284AC
	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.manager.AddPanelFor(devTool);
			return;
		}
		for (int i = this.devTools.Count - 1; i > this.currentDevToolIndex; i--)
		{
			this.devTools[i].Internal_Uninit();
			this.devTools.RemoveAt(i);
		}
		this.devTools.Add(devTool);
		this.currentDevToolIndex = this.devTools.Count - 1;
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x0022A32C File Offset: 0x0022852C
	public bool NavGoBack()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x0022A35C File Offset: 0x0022855C
	public bool NavGoForward()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x000C9758 File Offset: 0x000C7958
	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

	// Token: 0x0600392E RID: 14638 RVA: 0x0022A38C File Offset: 0x0022858C
	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = this.currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= this.devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	// Token: 0x0600392F RID: 14639 RVA: 0x0022A3D0 File Offset: 0x002285D0
	public void RenderPanel()
	{
		DevTool currentDevTool = this.GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
			return;
		}
		ImGuiWindowFlags flags;
		this.ConfigureImGuiWindowFor(currentDevTool, out flags);
		currentDevTool.Internal_Update();
		bool flag = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + this.uniquePanelId, ref flag, flags))
		{
			if (!flag)
			{
				this.isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				this.DrawNavigation();
				ImGui.SameLine(0f, 20f);
				this.DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (this.GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (this.GetCurrentDevTool().isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
		}
	}

	// Token: 0x06003930 RID: 14640 RVA: 0x0022A498 File Offset: 0x00228698
	private void DrawNavigation()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			this.currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + this.devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = this.TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			this.currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + this.devTools[option2.Unwrap()].Name);
			return;
		}
		ImGuiEx.TooltipForPrevious("Go forward");
	}

	// Token: 0x06003931 RID: 14641 RVA: 0x000AA038 File Offset: 0x000A8238
	private void DrawMenuBarContents()
	{
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x0022A57C File Offset: 0x0022877C
	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = (ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags);
		if (this.nextImGuiWindowPosition.HasValue)
		{
			ValueTuple<Vector2, ImGuiCond> value = this.nextImGuiWindowPosition.Value;
			Vector2 item = value.Item1;
			ImGuiCond item2 = value.Item2;
			ImGui.SetNextWindowPos(item, item2);
			this.nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
		if (this.nextImGuiWindowSize.HasValue)
		{
			Vector2 item3 = this.nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item3);
			this.nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x000C976B File Offset: 0x000C796B
	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x000C977F File Offset: 0x000C797F
	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x000C9793 File Offset: 0x000C7993
	public void Close()
	{
		this.isRequestingToClose = true;
	}

	// Token: 0x06003936 RID: 14646 RVA: 0x0022A614 File Offset: 0x00228814
	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	// Token: 0x0400278F RID: 10127
	public readonly string uniquePanelId;

	// Token: 0x04002790 RID: 10128
	public readonly DevPanelList manager;

	// Token: 0x04002791 RID: 10129
	public readonly Type initialDevToolType;

	// Token: 0x04002792 RID: 10130
	public readonly uint idPostfixNumber;

	// Token: 0x04002793 RID: 10131
	private List<DevTool> devTools;

	// Token: 0x04002794 RID: 10132
	private int currentDevToolIndex;
}
