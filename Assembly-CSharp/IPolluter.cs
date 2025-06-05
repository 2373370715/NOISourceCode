using System;
using UnityEngine;

// Token: 0x020016B4 RID: 5812
public interface IPolluter
{
	// Token: 0x060077C7 RID: 30663
	int GetRadius();

	// Token: 0x060077C8 RID: 30664
	int GetNoise();

	// Token: 0x060077C9 RID: 30665
	GameObject GetGameObject();

	// Token: 0x060077CA RID: 30666
	void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

	// Token: 0x060077CB RID: 30667
	string GetName();

	// Token: 0x060077CC RID: 30668
	Vector2 GetPosition();

	// Token: 0x060077CD RID: 30669
	void Clear();

	// Token: 0x060077CE RID: 30670
	void SetSplat(NoiseSplat splat);
}
