using System;
using System.Collections.Generic;

// Token: 0x02001376 RID: 4982
public class TagNameComparer : IComparer<Tag>
{
	// Token: 0x06006606 RID: 26118 RVA: 0x000AA024 File Offset: 0x000A8224
	public TagNameComparer()
	{
	}

	// Token: 0x06006607 RID: 26119 RVA: 0x000E708B File Offset: 0x000E528B
	public TagNameComparer(Tag firstTag)
	{
		this.firstTag = firstTag;
	}

	// Token: 0x06006608 RID: 26120 RVA: 0x002D8E5C File Offset: 0x002D705C
	public int Compare(Tag x, Tag y)
	{
		if (x == y)
		{
			return 0;
		}
		if (this.firstTag.IsValid)
		{
			if (x == this.firstTag && y != this.firstTag)
			{
				return 1;
			}
			if (x != this.firstTag && y == this.firstTag)
			{
				return -1;
			}
		}
		return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
	}

	// Token: 0x04004B97 RID: 19351
	private Tag firstTag;
}
