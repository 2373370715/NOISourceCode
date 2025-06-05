using System;

// Token: 0x02001FA1 RID: 8097
public interface ICheckboxListGroupControl
{
	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x0600AB27 RID: 43815
	string Title { get; }

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x0600AB28 RID: 43816
	string Description { get; }

	// Token: 0x0600AB29 RID: 43817
	ICheckboxListGroupControl.ListGroup[] GetData();

	// Token: 0x0600AB2A RID: 43818
	bool SidescreenEnabled();

	// Token: 0x0600AB2B RID: 43819
	int CheckboxSideScreenSortOrder();

	// Token: 0x02001FA2 RID: 8098
	public struct ListGroup
	{
		// Token: 0x0600AB2C RID: 43820 RVA: 0x00113A95 File Offset: 0x00111C95
		public ListGroup(string title, ICheckboxListGroupControl.CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}

		// Token: 0x040086B6 RID: 34486
		public Func<string, string> resolveTitleCallback;

		// Token: 0x040086B7 RID: 34487
		public System.Action onItemClicked;

		// Token: 0x040086B8 RID: 34488
		public string title;

		// Token: 0x040086B9 RID: 34489
		public ICheckboxListGroupControl.CheckboxItem[] checkboxItems;
	}

	// Token: 0x02001FA3 RID: 8099
	public struct CheckboxItem
	{
		// Token: 0x040086BA RID: 34490
		public string text;

		// Token: 0x040086BB RID: 34491
		public string tooltip;

		// Token: 0x040086BC RID: 34492
		public bool isOn;

		// Token: 0x040086BD RID: 34493
		public Func<string, bool> overrideLinkActions;

		// Token: 0x040086BE RID: 34494
		public Func<string, object, string> resolveTooltipCallback;
	}
}
