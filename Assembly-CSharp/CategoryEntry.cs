using System;
using System.Collections.Generic;

// Token: 0x02001C7D RID: 7293
public class CategoryEntry : CodexEntry
{
	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x060097DD RID: 38877 RVA: 0x001075EB File Offset: 0x001057EB
	// (set) Token: 0x060097DE RID: 38878 RVA: 0x001075F3 File Offset: 0x001057F3
	public bool largeFormat { get; set; }

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x060097DF RID: 38879 RVA: 0x001075FC File Offset: 0x001057FC
	// (set) Token: 0x060097E0 RID: 38880 RVA: 0x00107604 File Offset: 0x00105804
	public bool sort { get; set; }

	// Token: 0x060097E1 RID: 38881 RVA: 0x0010760D File Offset: 0x0010580D
	public CategoryEntry(string category, List<ContentContainer> contentContainers, string name, List<CodexEntry> entriesInCategory, bool largeFormat, bool sort) : base(category, contentContainers, name)
	{
		this.entriesInCategory = entriesInCategory;
		this.largeFormat = largeFormat;
		this.sort = sort;
	}

	// Token: 0x0400763E RID: 30270
	public List<CodexEntry> entriesInCategory = new List<CodexEntry>();
}
