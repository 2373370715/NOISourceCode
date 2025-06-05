using System;
using UnityEngine;

// Token: 0x020017BF RID: 6079
public static class Blur
{
	// Token: 0x06007CFC RID: 31996 RVA: 0x000F6B32 File Offset: 0x000F4D32
	public static RenderTexture Run(Texture2D image)
	{
		if (Blur.blurMaterial == null)
		{
			Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		}
		return null;
	}

	// Token: 0x04005E21 RID: 24097
	private static Material blurMaterial;
}
