using System;
using UnityEngine;

// Token: 0x020017C0 RID: 6080
public class CameraReferenceTexture : MonoBehaviour
{
	// Token: 0x06007CFD RID: 31997 RVA: 0x0032F44C File Offset: 0x0032D64C
	private void OnPreCull()
	{
		if (this.quad == null)
		{
			this.quad = new FullScreenQuad("CameraReferenceTexture", base.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
		}
		if (this.referenceCamera != null)
		{
			this.quad.Draw(this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
		}
	}

	// Token: 0x04005E22 RID: 24098
	public Camera referenceCamera;

	// Token: 0x04005E23 RID: 24099
	private FullScreenQuad quad;
}
