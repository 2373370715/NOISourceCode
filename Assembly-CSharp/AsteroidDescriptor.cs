using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C54 RID: 7252
public struct AsteroidDescriptor
{
	// Token: 0x060096AB RID: 38571 RVA: 0x00106901 File Offset: 0x00104B01
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	// Token: 0x04007510 RID: 29968
	public string text;

	// Token: 0x04007511 RID: 29969
	public string tooltip;

	// Token: 0x04007512 RID: 29970
	public List<global::Tuple<string, Color, float>> bands;

	// Token: 0x04007513 RID: 29971
	public Color associatedColor;

	// Token: 0x04007514 RID: 29972
	public string associatedIcon;
}
