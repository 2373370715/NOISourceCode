using System;

// Token: 0x02001F9D RID: 8093
[Serializable]
public struct ButtonMenuTextOverride
{
	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x0600AB0B RID: 43787 RVA: 0x00113926 File Offset: 0x00111B26
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x0600AB0C RID: 43788 RVA: 0x0011394F File Offset: 0x00111B4F
	public bool HasCancelText
	{
		get
		{
			return !string.IsNullOrEmpty(this.CancelText) && !string.IsNullOrEmpty(this.CancelToolTip);
		}
	}

	// Token: 0x040086A6 RID: 34470
	public LocString Text;

	// Token: 0x040086A7 RID: 34471
	public LocString CancelText;

	// Token: 0x040086A8 RID: 34472
	public LocString ToolTip;

	// Token: 0x040086A9 RID: 34473
	public LocString CancelToolTip;
}
