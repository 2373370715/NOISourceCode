using System;
using KSerialization;

// Token: 0x02001308 RID: 4872
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
	// Token: 0x060063DC RID: 25564 RVA: 0x000E5A60 File Offset: 0x000E3C60
	public CellEventInstance(int cell, int data, int data2, CellEvent ev) : base(ev)
	{
		this.cell = cell;
		this.data = data;
		this.data2 = data2;
	}

	// Token: 0x04004789 RID: 18313
	[Serialize]
	public int cell;

	// Token: 0x0400478A RID: 18314
	[Serialize]
	public int data;

	// Token: 0x0400478B RID: 18315
	[Serialize]
	public int data2;
}
