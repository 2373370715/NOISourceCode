using System;

// Token: 0x02000BCE RID: 3022
public class DevToolAnimFile : DevTool
{
	// Token: 0x06003953 RID: 14675 RVA: 0x000C98D8 File Offset: 0x000C7AD8
	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		this.Name = "Anim File: \"" + animFile.name + "\"";
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x0022ACEC File Offset: 0x00228EEC
	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(this.animFile, null);
		ImGuiEx.DrawObject(this.animFile.GetData(), null);
	}

	// Token: 0x040027A0 RID: 10144
	private KAnimFile animFile;
}
