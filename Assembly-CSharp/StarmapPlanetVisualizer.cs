using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200207F RID: 8319
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
	// Token: 0x04008B78 RID: 35704
	public Image image;

	// Token: 0x04008B79 RID: 35705
	public LocText label;

	// Token: 0x04008B7A RID: 35706
	public MultiToggle button;

	// Token: 0x04008B7B RID: 35707
	public RectTransform selection;

	// Token: 0x04008B7C RID: 35708
	public GameObject analysisSelection;

	// Token: 0x04008B7D RID: 35709
	public Image unknownBG;

	// Token: 0x04008B7E RID: 35710
	public GameObject rocketIconContainer;
}
