using System;

// Token: 0x02000BF3 RID: 3059
public class DevToolMenuNodeAction : IMenuNode
{
	// Token: 0x06003A04 RID: 14852 RVA: 0x000C9FE5 File Offset: 0x000C81E5
	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x000C9FFB File Offset: 0x000C81FB
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x000CA003 File Offset: 0x000C8203
	public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

	// Token: 0x04002819 RID: 10265
	public string name;

	// Token: 0x0400281A RID: 10266
	public System.Action onClickFn;

	// Token: 0x0400281B RID: 10267
	public Func<bool> isEnabledFn;
}
