using System;
using UnityEngine;

// Token: 0x020017E1 RID: 6113
public class ScreenResize : MonoBehaviour
{
	// Token: 0x06007DA4 RID: 32164 RVA: 0x000F745E File Offset: 0x000F565E
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

	// Token: 0x06007DA5 RID: 32165 RVA: 0x00332E28 File Offset: 0x00331028
	private void LateUpdate()
	{
		if (Screen.width != this.Width || Screen.height != this.Height || this.isFullscreen != Screen.fullScreen)
		{
			this.Width = Screen.width;
			this.Height = Screen.height;
			this.isFullscreen = Screen.fullScreen;
			this.TriggerResize();
		}
	}

	// Token: 0x06007DA6 RID: 32166 RVA: 0x000F7493 File Offset: 0x000F5693
	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	// Token: 0x06007DA7 RID: 32167 RVA: 0x000F74A8 File Offset: 0x000F56A8
	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	// Token: 0x04005F63 RID: 24419
	public System.Action OnResize;

	// Token: 0x04005F64 RID: 24420
	public static ScreenResize Instance;

	// Token: 0x04005F65 RID: 24421
	private int Width;

	// Token: 0x04005F66 RID: 24422
	private int Height;

	// Token: 0x04005F67 RID: 24423
	private bool isFullscreen;
}
