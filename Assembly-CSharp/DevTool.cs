using System;
using ImGuiNET;

// Token: 0x02000BCC RID: 3020
public abstract class DevTool
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06003942 RID: 14658 RVA: 0x0022A8B0 File Offset: 0x00228AB0
	// (remove) Token: 0x06003943 RID: 14659 RVA: 0x0022A8E8 File Offset: 0x00228AE8
	public event System.Action OnInit;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06003944 RID: 14660 RVA: 0x0022A920 File Offset: 0x00228B20
	// (remove) Token: 0x06003945 RID: 14661 RVA: 0x0022A958 File Offset: 0x00228B58
	public event System.Action OnUpdate;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06003946 RID: 14662 RVA: 0x0022A990 File Offset: 0x00228B90
	// (remove) Token: 0x06003947 RID: 14663 RVA: 0x0022A9C8 File Offset: 0x00228BC8
	public event System.Action OnUninit;

	// Token: 0x06003948 RID: 14664 RVA: 0x000C9808 File Offset: 0x000C7A08
	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x000C981C File Offset: 0x000C7A1C
	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x000C9845 File Offset: 0x000C7A45
	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	// Token: 0x0600394B RID: 14667
	protected abstract void RenderTo(DevPanel panel);

	// Token: 0x0600394C RID: 14668 RVA: 0x000C984E File Offset: 0x000C7A4E
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

	// Token: 0x0600394D RID: 14669 RVA: 0x000C9873 File Offset: 0x000C7A73
	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x0600394E RID: 14670 RVA: 0x000C9888 File Offset: 0x000C7A88
	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	// Token: 0x04002798 RID: 10136
	public string Name;

	// Token: 0x04002799 RID: 10137
	public bool RequiresGameRunning;

	// Token: 0x0400279A RID: 10138
	public bool isRequestingToClosePanel;

	// Token: 0x0400279B RID: 10139
	public ImGuiWindowFlags drawFlags;

	// Token: 0x0400279F RID: 10143
	private bool didInit;
}
