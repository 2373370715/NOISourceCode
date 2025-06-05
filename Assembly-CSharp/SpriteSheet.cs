using System;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
[Serializable]
public struct SpriteSheet
{
	// Token: 0x040024EE RID: 9454
	public string name;

	// Token: 0x040024EF RID: 9455
	public int numFrames;

	// Token: 0x040024F0 RID: 9456
	public int numXFrames;

	// Token: 0x040024F1 RID: 9457
	public Vector2 uvFrameSize;

	// Token: 0x040024F2 RID: 9458
	public int renderLayer;

	// Token: 0x040024F3 RID: 9459
	public Material material;

	// Token: 0x040024F4 RID: 9460
	public Texture2D texture;
}
