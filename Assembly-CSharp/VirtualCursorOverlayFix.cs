using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020BA RID: 8378
public class VirtualCursorOverlayFix : MonoBehaviour
{
	// Token: 0x0600B2B0 RID: 45744 RVA: 0x0043E388 File Offset: 0x0043C588
	private void Awake()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		this.cursorRendTex = new RenderTexture(width, height, 0);
		this.screenSpaceCamera.enabled = true;
		this.screenSpaceCamera.targetTexture = this.cursorRendTex;
		this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
		base.StartCoroutine(this.RenderVirtualCursor());
	}

	// Token: 0x0600B2B1 RID: 45745 RVA: 0x00118AEF File Offset: 0x00116CEF
	private IEnumerator RenderVirtualCursor()
	{
		bool ShowCursor = KInputManager.currentControllerIsGamepad;
		while (Application.isPlaying)
		{
			ShowCursor = KInputManager.currentControllerIsGamepad;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
			{
				ShowCursor = true;
			}
			this.screenSpaceCamera.enabled = true;
			if (!this.screenSpaceOverlayImage.enabled && ShowCursor)
			{
				yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
			}
			this.actualCursor.enabled = ShowCursor;
			this.screenSpaceOverlayImage.enabled = ShowCursor;
			this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04008D14 RID: 36116
	private RenderTexture cursorRendTex;

	// Token: 0x04008D15 RID: 36117
	public Camera screenSpaceCamera;

	// Token: 0x04008D16 RID: 36118
	public Image screenSpaceOverlayImage;

	// Token: 0x04008D17 RID: 36119
	public RawImage actualCursor;
}
