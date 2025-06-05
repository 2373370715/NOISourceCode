using System;

// Token: 0x02000BF6 RID: 3062
public class DevToolObjectViewer<T> : DevTool
{
	// Token: 0x06003A14 RID: 14868 RVA: 0x000CA079 File Offset: 0x000C8279
	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		this.Name = typeof(T).Name;
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x002310B8 File Offset: 0x0022F2B8
	protected override void RenderTo(DevPanel panel)
	{
		T t = this.getValue();
		this.Name = t.GetType().Name;
		ImGuiEx.DrawObject(t, null);
	}

	// Token: 0x04002829 RID: 10281
	private Func<T> getValue;
}
