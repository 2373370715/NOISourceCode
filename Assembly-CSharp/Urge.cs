using System;

// Token: 0x020007AC RID: 1964
public class Urge : Resource
{
	// Token: 0x060022D1 RID: 8913 RVA: 0x000BB146 File Offset: 0x000B9346
	public Urge(string id) : base(id, null, null)
	{
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000BB151 File Offset: 0x000B9351
	public override string ToString()
	{
		return this.Id;
	}
}
