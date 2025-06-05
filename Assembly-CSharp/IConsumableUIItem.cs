using System;

// Token: 0x02001E19 RID: 7705
public interface IConsumableUIItem
{
	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x0600A0E5 RID: 41189
	string ConsumableId { get; }

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x0600A0E6 RID: 41190
	string ConsumableName { get; }

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x0600A0E7 RID: 41191
	int MajorOrder { get; }

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x0600A0E8 RID: 41192
	int MinorOrder { get; }

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x0600A0E9 RID: 41193
	bool Display { get; }

	// Token: 0x0600A0EA RID: 41194 RVA: 0x000AA765 File Offset: 0x000A8965
	string OverrideSpriteName()
	{
		return null;
	}

	// Token: 0x0600A0EB RID: 41195 RVA: 0x0010D536 File Offset: 0x0010B736
	bool RevealTest()
	{
		return ConsumerManager.instance.isDiscovered(this.ConsumableId.ToTag());
	}
}
