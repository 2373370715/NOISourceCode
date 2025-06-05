using System;
using System.Diagnostics;

// Token: 0x02001317 RID: 4887
[DebuggerDisplay("{face.hash} {priority}")]
public class Expression : Resource
{
	// Token: 0x0600641C RID: 25628 RVA: 0x000E5D34 File Offset: 0x000E3F34
	public Expression(string id, ResourceSet parent, Face face) : base(id, parent, null)
	{
		this.face = face;
	}

	// Token: 0x040047EB RID: 18411
	public Face face;

	// Token: 0x040047EC RID: 18412
	public int priority;
}
