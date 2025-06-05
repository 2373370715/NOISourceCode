using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FCF RID: 8143
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x0600AC0E RID: 44046 RVA: 0x00114440 File Offset: 0x00112640
	public override string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x0600AC0F RID: 44047 RVA: 0x0011444C File Offset: 0x0011264C
	protected override void SetIcon(Sprite sprite, Color color)
	{
		if (this.icon != null)
		{
			this.icon.gameObject.SetActive(false);
		}
	}
}
