using System;
using UnityEngine;

// Token: 0x02001ADD RID: 6877
public class FillRenderTargetEffect : MonoBehaviour
{
	// Token: 0x06008FC3 RID: 36803 RVA: 0x0010253C File Offset: 0x0010073C
	public void SetFillTexture(Texture tex)
	{
		this.fillTexture = tex;
	}

	// Token: 0x06008FC4 RID: 36804 RVA: 0x00102545 File Offset: 0x00100745
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(this.fillTexture, null);
	}

	// Token: 0x04006C5C RID: 27740
	private Texture fillTexture;
}
