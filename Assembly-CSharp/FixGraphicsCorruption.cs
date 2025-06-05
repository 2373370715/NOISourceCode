using System;
using UnityEngine;

// Token: 0x02001ADE RID: 6878
public class FixGraphicsCorruption : MonoBehaviour
{
	// Token: 0x06008FC6 RID: 36806 RVA: 0x00102553 File Offset: 0x00100753
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		component.transparencySortMode = TransparencySortMode.Orthographic;
		component.tag = "Untagged";
	}

	// Token: 0x06008FC7 RID: 36807 RVA: 0x0010256C File Offset: 0x0010076C
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}
}
