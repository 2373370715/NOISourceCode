using System;
using UnityEngine;

// Token: 0x02001B83 RID: 7043
public class LogicModeUI : ScriptableObject
{
	// Token: 0x04006FF4 RID: 28660
	[Header("Base Assets")]
	public Sprite inputSprite;

	// Token: 0x04006FF5 RID: 28661
	public Sprite outputSprite;

	// Token: 0x04006FF6 RID: 28662
	public Sprite resetSprite;

	// Token: 0x04006FF7 RID: 28663
	public GameObject prefab;

	// Token: 0x04006FF8 RID: 28664
	public GameObject ribbonInputPrefab;

	// Token: 0x04006FF9 RID: 28665
	public GameObject ribbonOutputPrefab;

	// Token: 0x04006FFA RID: 28666
	public GameObject controlInputPrefab;

	// Token: 0x04006FFB RID: 28667
	[Header("Colouring")]
	public Color32 colourOn = new Color32(0, byte.MaxValue, 0, 0);

	// Token: 0x04006FFC RID: 28668
	public Color32 colourOff = new Color32(byte.MaxValue, 0, 0, 0);

	// Token: 0x04006FFD RID: 28669
	public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04006FFE RID: 28670
	public Color32 colourOnProtanopia = new Color32(179, 204, 0, 0);

	// Token: 0x04006FFF RID: 28671
	public Color32 colourOffProtanopia = new Color32(166, 51, 102, 0);

	// Token: 0x04007000 RID: 28672
	public Color32 colourOnDeuteranopia = new Color32(128, 0, 128, 0);

	// Token: 0x04007001 RID: 28673
	public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, 153, 0, 0);

	// Token: 0x04007002 RID: 28674
	public Color32 colourOnTritanopia = new Color32(51, 102, byte.MaxValue, 0);

	// Token: 0x04007003 RID: 28675
	public Color32 colourOffTritanopia = new Color32(byte.MaxValue, 153, 0, 0);
}
