using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02001AE6 RID: 6886
public class GlobalResources : ScriptableObject
{
	// Token: 0x06008FFB RID: 36859 RVA: 0x0010279C File Offset: 0x0010099C
	public static GlobalResources Instance()
	{
		if (GlobalResources._Instance == null)
		{
			GlobalResources._Instance = Resources.Load<GlobalResources>("GlobalResources");
		}
		return GlobalResources._Instance;
	}

	// Token: 0x04006CBF RID: 27839
	public Material AnimMaterial;

	// Token: 0x04006CC0 RID: 27840
	public Material AnimUIMaterial;

	// Token: 0x04006CC1 RID: 27841
	public Material AnimPlaceMaterial;

	// Token: 0x04006CC2 RID: 27842
	public Material AnimMaterialUIDesaturated;

	// Token: 0x04006CC3 RID: 27843
	public Material AnimSimpleMaterial;

	// Token: 0x04006CC4 RID: 27844
	public Material AnimOverlayMaterial;

	// Token: 0x04006CC5 RID: 27845
	public Texture2D WhiteTexture;

	// Token: 0x04006CC6 RID: 27846
	public EventReference ConduitOverlaySoundLiquid;

	// Token: 0x04006CC7 RID: 27847
	public EventReference ConduitOverlaySoundGas;

	// Token: 0x04006CC8 RID: 27848
	public EventReference ConduitOverlaySoundSolid;

	// Token: 0x04006CC9 RID: 27849
	public EventReference AcousticDisturbanceSound;

	// Token: 0x04006CCA RID: 27850
	public EventReference AcousticDisturbanceBubbleSound;

	// Token: 0x04006CCB RID: 27851
	public EventReference WallDamageLayerSound;

	// Token: 0x04006CCC RID: 27852
	public Sprite sadDupeAudio;

	// Token: 0x04006CCD RID: 27853
	public Sprite sadDupe;

	// Token: 0x04006CCE RID: 27854
	public Sprite baseGameLogoSmall;

	// Token: 0x04006CCF RID: 27855
	public Sprite expansion1LogoSmall;

	// Token: 0x04006CD0 RID: 27856
	private static GlobalResources _Instance;
}
