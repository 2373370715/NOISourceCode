using System;
using ImGuiNET;

public abstract class DevTool
{
add) Token: 0x06003942 RID: 14658 RVA: 0x0022A8B0 File Offset: 0x00228AB0
remove) Token: 0x06003943 RID: 14659 RVA: 0x0022A8E8 File Offset: 0x00228AE8
	public event System.Action OnInit;

add) Token: 0x06003944 RID: 14660 RVA: 0x0022A920 File Offset: 0x00228B20
remove) Token: 0x06003945 RID: 14661 RVA: 0x0022A958 File Offset: 0x00228B58
	public event System.Action OnUpdate;

add) Token: 0x06003946 RID: 14662 RVA: 0x0022A990 File Offset: 0x00228B90
remove) Token: 0x06003947 RID: 14663 RVA: 0x0022A9C8 File Offset: 0x00228BC8
	public event System.Action OnUninit;

	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	protected abstract void RenderTo(DevPanel panel);

	public void Internal_TryInit()
	{
		if (this.didInit)
		{
			return;
		}
		this.didInit = true;
		if (this.OnInit != null)
		{
			this.OnInit();
		}
	}

	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	public string Name;

	public bool RequiresGameRunning;

	public bool isRequestingToClosePanel;

	public ImGuiWindowFlags drawFlags;

	private bool didInit;
}
