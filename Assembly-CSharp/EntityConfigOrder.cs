using System;

// Token: 0x020012D9 RID: 4825
public class EntityConfigOrder : Attribute
{
	// Token: 0x0600630D RID: 25357 RVA: 0x000E514C File Offset: 0x000E334C
	public EntityConfigOrder(int sort_order)
	{
		this.sortOrder = sort_order;
	}

	// Token: 0x04004707 RID: 18183
	public int sortOrder;
}
