using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000BED RID: 3053
public class DevToolManager
{
	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000C9EDC File Offset: 0x000C80DC
	public bool Show
	{
		get
		{
			return this.showImGui;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000C9EE4 File Offset: 0x000C80E4
	private bool quickDevEnabled
	{
		get
		{
			return DebugHandler.enabled && GenericGameSettings.instance.quickDevTools;
		}
	}

	// Token: 0x060039E7 RID: 14823 RVA: 0x00230118 File Offset: 0x0022E318
	public DevToolManager()
	{
		DevToolManager.Instance = this;
		this.RegisterDevTool<DevToolSimDebug>("Debuggers/Sim Debug");
		this.RegisterDevTool<DevToolStateMachineDebug>("Debuggers/State Machine");
		this.RegisterDevTool<DevToolSaveGameInfo>("Debuggers/Save Game Info");
		this.RegisterDevTool<DevToolPerformanceInfo>("Debuggers/Performance Info");
		this.RegisterDevTool<DevToolPrintingPodDebug>("Debuggers/Printing Pod Debug");
		this.RegisterDevTool<DevToolBigBaseMutations>("Debuggers/Big Base Mutation Utilities");
		this.RegisterDevTool<DevToolNavGrid>("Debuggers/Nav Grid");
		this.RegisterDevTool<DevToolResearchDebugger>("Debuggers/Research");
		this.RegisterDevTool<DevToolStatusItems>("Debuggers/StatusItems");
		this.RegisterDevTool<DevToolUI>("Debuggers/UI");
		this.RegisterDevTool<DevToolUnlockedIds>("Debuggers/UnlockedIds List");
		this.RegisterDevTool<DevToolStringsTable>("Debuggers/StringsTable");
		this.RegisterDevTool<DevToolChoreDebugger>("Debuggers/Chore");
		this.RegisterDevTool<DevToolBatchedAnimDebug>("Debuggers/Batched Anim");
		this.RegisterDevTool<DevTool_StoryTraits_Reveal>("Debuggers/Story Traits Reveal");
		this.RegisterDevTool<DevTool_StoryTrait_CritterManipulator>("Debuggers/Story Trait - Critter Manipulator");
		this.RegisterDevTool<DevToolAnimEventManager>("Debuggers/Anim Event Manager");
		this.RegisterDevTool<DevToolSceneBrowser>("Scene/Browser");
		this.RegisterDevTool<DevToolSceneInspector>("Scene/Inspector");
		this.menuNodes.AddAction("Help/" + UI.FRONTEND.DEVTOOLS.TITLE.text, delegate
		{
			this.warning.ShouldDrawWindow = true;
		});
		this.RegisterDevTool<DevToolCommandPalette>("Help/Command Palette");
		this.RegisterAdditionalDevToolsByReflection();
	}

	// Token: 0x060039E8 RID: 14824 RVA: 0x000C9EF9 File Offset: 0x000C80F9
	public void Init()
	{
		this.UserAcceptedWarning = (KPlayerPrefs.GetInt("ShowDevtools", 0) == 1);
	}

	// Token: 0x060039E9 RID: 14825 RVA: 0x00230284 File Offset: 0x0022E484
	private void RegisterDevTool<T>(string location) where T : DevTool, new()
	{
		this.menuNodes.AddAction(location, delegate
		{
			this.panels.AddPanelFor<T>();
		});
		this.dontAutomaticallyRegisterTypes.Add(typeof(T));
		this.devToolNameDict[typeof(T)] = Path.GetFileName(location);
	}

	// Token: 0x060039EA RID: 14826 RVA: 0x002302DC File Offset: 0x0022E4DC
	private void RegisterAdditionalDevToolsByReflection()
	{
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				if (!type.IsAbstract && !this.dontAutomaticallyRegisterTypes.Contains(type) && ReflectionUtil.HasDefaultConstructor(type))
				{
					this.menuNodes.AddAction("Debuggers/" + DevToolUtil.GenerateDevToolName(type), delegate
					{
						this.panels.AddPanelFor((DevTool)Activator.CreateInstance(type));
					});
				}
			}
		}
	}

	// Token: 0x060039EB RID: 14827 RVA: 0x00230398 File Offset: 0x0022E598
	public void UpdateShouldShowTools()
	{
		if (!DebugHandler.enabled)
		{
			this.showImGui = false;
			return;
		}
		bool flag = Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
		if (!this.toggleKeyWasDown && flag)
		{
			this.showImGui = !this.showImGui;
		}
		this.toggleKeyWasDown = flag;
	}

	// Token: 0x060039EC RID: 14828 RVA: 0x00230400 File Offset: 0x0022E600
	public void UpdateTools()
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (this.showImGui)
		{
			if (this.warning.ShouldDrawWindow)
			{
				this.warning.DrawWindow(out this.warning.ShouldDrawWindow);
			}
			if (!this.UserAcceptedWarning)
			{
				this.warning.DrawMenuBar();
			}
			else
			{
				this.DrawMenu();
				this.panels.Render();
				if (this.showImguiState)
				{
					if (ImGui.Begin("ImGui state", ref this.showImguiState))
					{
						ImGui.Checkbox("ImGui.GetIO().WantCaptureMouse", ImGui.GetIO().WantCaptureMouse);
						ImGui.Checkbox("ImGui.GetIO().WantCaptureKeyboard", ImGui.GetIO().WantCaptureKeyboard);
					}
					ImGui.End();
				}
				if (this.showImguiDemo)
				{
					ImGui.ShowDemoWindow(ref this.showImguiDemo);
				}
			}
		}
		this.UpdateConsumingGameInputs();
		this.UpdateShortcuts();
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x000C9F0F File Offset: 0x000C810F
	private void UpdateShortcuts()
	{
		if ((this.showImGui || this.quickDevEnabled) && this.UserAcceptedWarning)
		{
			this.<UpdateShortcuts>g__DoUpdate|26_0();
		}
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x002304D8 File Offset: 0x0022E6D8
	private void DrawMenu()
	{
		this.menuFontSize.InitializeIfNeeded();
		if (ImGui.BeginMainMenuBar())
		{
			this.menuNodes.Draw();
			this.menuFontSize.DrawMenu();
			if (ImGui.BeginMenu("IMGUI"))
			{
				ImGui.Checkbox("ImGui state", ref this.showImguiState);
				ImGui.Checkbox("ImGui Demo", ref this.showImguiDemo);
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x00230548 File Offset: 0x0022E748
	private unsafe void UpdateConsumingGameInputs()
	{
		this.doesImGuiWantInput = false;
		if (this.showImGui)
		{
			this.doesImGuiWantInput = (*ImGui.GetIO().WantCaptureMouse || *ImGui.GetIO().WantCaptureKeyboard);
			if (!this.prevDoesImGuiWantInput && this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0();
			}
			if (this.prevDoesImGuiWantInput && !this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
			}
		}
		if (this.prevShowImGui && this.prevDoesImGuiWantInput && !this.showImGui)
		{
			DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
		}
		this.prevShowImGui = this.showImGui;
		this.prevDoesImGuiWantInput = this.doesImGuiWantInput;
		KInputManager.devToolFocus = (this.showImGui && this.doesImGuiWantInput);
	}

	// Token: 0x060039F2 RID: 14834 RVA: 0x00230600 File Offset: 0x0022E800
	[CompilerGenerated]
	private void <UpdateShortcuts>g__DoUpdate|26_0()
	{
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space))
		{
			DevToolCommandPalette.Init();
			this.showImGui = true;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			DevToolUI.PingHoveredObject();
			this.showImGui = true;
		}
	}

	// Token: 0x060039F3 RID: 14835 RVA: 0x00230650 File Offset: 0x0022E850
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0()
	{
		UnityMouseCatcherUI.SetEnabled(true);
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			inputManager.GetController(i).HandleCancelInput();
		}
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x000C9F4B File Offset: 0x000C814B
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputExitImGui|28_1()
	{
		UnityMouseCatcherUI.SetEnabled(false);
	}

	// Token: 0x04002803 RID: 10243
	public const string SHOW_DEVTOOLS = "ShowDevtools";

	// Token: 0x04002804 RID: 10244
	public static DevToolManager Instance;

	// Token: 0x04002805 RID: 10245
	private bool toggleKeyWasDown;

	// Token: 0x04002806 RID: 10246
	private bool showImGui;

	// Token: 0x04002807 RID: 10247
	private bool prevShowImGui;

	// Token: 0x04002808 RID: 10248
	private bool doesImGuiWantInput;

	// Token: 0x04002809 RID: 10249
	private bool prevDoesImGuiWantInput;

	// Token: 0x0400280A RID: 10250
	private bool showImguiState;

	// Token: 0x0400280B RID: 10251
	private bool showImguiDemo;

	// Token: 0x0400280C RID: 10252
	public bool UserAcceptedWarning;

	// Token: 0x0400280D RID: 10253
	private DevToolWarning warning = new DevToolWarning();

	// Token: 0x0400280E RID: 10254
	private DevToolMenuFontSize menuFontSize = new DevToolMenuFontSize();

	// Token: 0x0400280F RID: 10255
	public DevPanelList panels = new DevPanelList();

	// Token: 0x04002810 RID: 10256
	public DevToolMenuNodeList menuNodes = new DevToolMenuNodeList();

	// Token: 0x04002811 RID: 10257
	public Dictionary<Type, string> devToolNameDict = new Dictionary<Type, string>();

	// Token: 0x04002812 RID: 10258
	private HashSet<Type> dontAutomaticallyRegisterTypes = new HashSet<Type>();
}
