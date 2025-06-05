using System;

// Token: 0x02001319 RID: 4889
public class Face : Resource
{
	// Token: 0x0600641E RID: 25630 RVA: 0x000E5D46 File Offset: 0x000E3F46
	public Face(string id, string headFXSymbol = null) : base(id, null, null)
	{
		this.hash = new HashedString(id);
		this.headFXHash = headFXSymbol;
	}

	// Token: 0x040047ED RID: 18413
	public HashedString hash;

	// Token: 0x040047EE RID: 18414
	public HashedString headFXHash;

	// Token: 0x040047EF RID: 18415
	private const string SYMBOL_PREFIX = "headfx_";
}
