using System;
using System.Collections.Generic;

// Token: 0x02000635 RID: 1589
public class StringSearchableList<T>
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06001C3F RID: 7231 RVA: 0x000B6FEF File Offset: 0x000B51EF
	// (set) Token: 0x06001C40 RID: 7232 RVA: 0x000B6FF7 File Offset: 0x000B51F7
	public bool didUseFilter { get; private set; }

	// Token: 0x06001C41 RID: 7233 RVA: 0x000B7000 File Offset: 0x000B5200
	public StringSearchableList(List<T> allValues, StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x000B702C File Offset: 0x000B522C
	public StringSearchableList(StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.allValues = new List<T>();
		this.filteredValues = new List<T>();
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x001B8648 File Offset: 0x001B6848
	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(this.filter))
		{
			this.filteredValues.Clear();
			foreach (T t in this.allValues)
			{
				if (!this.shouldFilterOutFn(t, this.filter))
				{
					this.filteredValues.Add(t);
				}
			}
			this.didUseFilter = true;
			return;
		}
		if (this.filteredValues.Count != this.allValues.Count)
		{
			this.filteredValues.Clear();
			this.filteredValues.AddRange(this.allValues);
		}
		this.didUseFilter = false;
	}

	// Token: 0x040011ED RID: 4589
	public string filter = "";

	// Token: 0x040011EE RID: 4590
	public List<T> allValues;

	// Token: 0x040011EF RID: 4591
	public List<T> filteredValues;

	// Token: 0x040011F1 RID: 4593
	public readonly StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn;

	// Token: 0x02000636 RID: 1590
	// (Invoke) Token: 0x06001C45 RID: 7237
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);
}
