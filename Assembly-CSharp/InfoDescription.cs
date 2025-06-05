using System;
using UnityEngine;

// Token: 0x02001D57 RID: 7511
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x06009CC9 RID: 40137 RVA: 0x0010A747 File Offset: 0x00108947
	// (set) Token: 0x06009CC8 RID: 40136 RVA: 0x0010A720 File Offset: 0x00108920
	public string DescriptionLocString
	{
		get
		{
			return this.descriptionLocString;
		}
		set
		{
			this.descriptionLocString = value;
			if (this.descriptionLocString != null)
			{
				this.description = Strings.Get(this.descriptionLocString);
			}
		}
	}

	// Token: 0x06009CCA RID: 40138 RVA: 0x003D3608 File Offset: 0x003D1808
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!string.IsNullOrEmpty(this.nameLocString))
		{
			this.displayName = Strings.Get(this.nameLocString);
		}
		if (!string.IsNullOrEmpty(this.descriptionLocString))
		{
			this.description = Strings.Get(this.descriptionLocString);
		}
	}

	// Token: 0x04007ACF RID: 31439
	public string nameLocString = "";

	// Token: 0x04007AD0 RID: 31440
	private string descriptionLocString = "";

	// Token: 0x04007AD1 RID: 31441
	public string description;

	// Token: 0x04007AD2 RID: 31442
	public string effect = "";

	// Token: 0x04007AD3 RID: 31443
	public string displayName;
}
