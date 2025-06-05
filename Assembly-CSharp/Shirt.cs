using System;

// Token: 0x020018B8 RID: 6328
public class Shirt : Resource
{
	// Token: 0x060082B8 RID: 33464 RVA: 0x000FA79D File Offset: 0x000F899D
	public Shirt(string id) : base(id, null, null)
	{
		this.hash = new HashedString(id);
	}

	// Token: 0x04006372 RID: 25458
	public HashedString hash;
}
