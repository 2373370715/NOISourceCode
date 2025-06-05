using System;
using UnityEngine;

// Token: 0x02001EE6 RID: 7910
public class LegendEntry
{
	// Token: 0x0600A600 RID: 42496 RVA: 0x003FC598 File Offset: 0x003FA798
	public LegendEntry(string name, string desc, Color colour, string desc_arg = null, Sprite sprite = null, bool displaySprite = true)
	{
		this.name = name;
		this.desc = desc;
		this.colour = colour;
		this.desc_arg = desc_arg;
		this.sprite = ((sprite == null) ? Assets.instance.LegendColourBox : sprite);
		this.displaySprite = displaySprite;
	}

	// Token: 0x040081F0 RID: 33264
	public string name;

	// Token: 0x040081F1 RID: 33265
	public string desc;

	// Token: 0x040081F2 RID: 33266
	public string desc_arg;

	// Token: 0x040081F3 RID: 33267
	public Color colour;

	// Token: 0x040081F4 RID: 33268
	public Sprite sprite;

	// Token: 0x040081F5 RID: 33269
	public bool displaySprite;
}
