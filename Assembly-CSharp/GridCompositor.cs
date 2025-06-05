using System;
using UnityEngine;

// Token: 0x020017CA RID: 6090
public class GridCompositor : MonoBehaviour
{
	// Token: 0x06007D1F RID: 32031 RVA: 0x000F6CE7 File Offset: 0x000F4EE7
	public static void DestroyInstance()
	{
		GridCompositor.Instance = null;
	}

	// Token: 0x06007D20 RID: 32032 RVA: 0x000F6CEF File Offset: 0x000F4EEF
	private void Awake()
	{
		GridCompositor.Instance = this;
		base.enabled = false;
	}

	// Token: 0x06007D21 RID: 32033 RVA: 0x000F6CFE File Offset: 0x000F4EFE
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));
	}

	// Token: 0x06007D22 RID: 32034 RVA: 0x000F6D15 File Offset: 0x000F4F15
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x06007D23 RID: 32035 RVA: 0x000F6D24 File Offset: 0x000F4F24
	public void ToggleMajor(bool on)
	{
		this.onMajor = on;
		this.Refresh();
	}

	// Token: 0x06007D24 RID: 32036 RVA: 0x000F6D33 File Offset: 0x000F4F33
	public void ToggleMinor(bool on)
	{
		this.onMinor = on;
		this.Refresh();
	}

	// Token: 0x06007D25 RID: 32037 RVA: 0x000F6D42 File Offset: 0x000F4F42
	private void Refresh()
	{
		base.enabled = (this.onMinor || this.onMajor);
	}

	// Token: 0x04005E40 RID: 24128
	public Material material;

	// Token: 0x04005E41 RID: 24129
	public static GridCompositor Instance;

	// Token: 0x04005E42 RID: 24130
	private bool onMajor;

	// Token: 0x04005E43 RID: 24131
	private bool onMinor;
}
