using System;

// Token: 0x02001E1F RID: 7711
public class SymbolicConsumableItem : IConsumableUIItem
{
	// Token: 0x0600A10E RID: 41230 RVA: 0x0010D59B File Offset: 0x0010B79B
	public SymbolicConsumableItem(string id, string name, int majorOrder, int minorOrder, bool display, string overrideSpriteName, Func<bool> revealTest)
	{
		this.id = id;
		this.name = name;
		this.majorOrder = majorOrder;
		this.minorOrder = minorOrder;
		this.display = display;
		this.overrideSpriteName = overrideSpriteName;
		this.revealTest = revealTest;
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x0600A10F RID: 41231 RVA: 0x0010D5D8 File Offset: 0x0010B7D8
	string IConsumableUIItem.ConsumableId
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x0600A110 RID: 41232 RVA: 0x0010D5E0 File Offset: 0x0010B7E0
	string IConsumableUIItem.ConsumableName
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x0600A111 RID: 41233 RVA: 0x0010D5E8 File Offset: 0x0010B7E8
	int IConsumableUIItem.MajorOrder
	{
		get
		{
			return this.majorOrder;
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x0600A112 RID: 41234 RVA: 0x0010D5F0 File Offset: 0x0010B7F0
	int IConsumableUIItem.MinorOrder
	{
		get
		{
			return this.minorOrder;
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x0600A113 RID: 41235 RVA: 0x0010D5F8 File Offset: 0x0010B7F8
	bool IConsumableUIItem.Display
	{
		get
		{
			return this.display;
		}
	}

	// Token: 0x0600A114 RID: 41236 RVA: 0x0010D600 File Offset: 0x0010B800
	string IConsumableUIItem.OverrideSpriteName()
	{
		return this.overrideSpriteName;
	}

	// Token: 0x0600A115 RID: 41237 RVA: 0x0010D608 File Offset: 0x0010B808
	bool IConsumableUIItem.RevealTest()
	{
		return this.revealTest();
	}

	// Token: 0x04007E73 RID: 32371
	private string id;

	// Token: 0x04007E74 RID: 32372
	private string name;

	// Token: 0x04007E75 RID: 32373
	private int majorOrder;

	// Token: 0x04007E76 RID: 32374
	private int minorOrder;

	// Token: 0x04007E77 RID: 32375
	private bool display;

	// Token: 0x04007E78 RID: 32376
	private string overrideSpriteName;

	// Token: 0x04007E79 RID: 32377
	private Func<bool> revealTest;
}
