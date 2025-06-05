using System;
using UnityEngine;

// Token: 0x020017C1 RID: 6081
public class CameraRenderTexture : MonoBehaviour
{
	// Token: 0x06007CFF RID: 31999 RVA: 0x000F6B56 File Offset: 0x000F4D56
	private void Awake()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
	}

	// Token: 0x06007D00 RID: 32000 RVA: 0x000F6B6D File Offset: 0x000F4D6D
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.OnResize();
	}

	// Token: 0x06007D01 RID: 32001 RVA: 0x0032F4B0 File Offset: 0x0032D6B0
	private void OnResize()
	{
		if (this.resultTexture != null)
		{
			this.resultTexture.DestroyRenderTexture();
		}
		this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		this.resultTexture.name = base.name;
		this.resultTexture.filterMode = FilterMode.Point;
		this.resultTexture.autoGenerateMips = false;
		if (this.TextureName != "")
		{
			Shader.SetGlobalTexture(this.TextureName, this.resultTexture);
		}
	}

	// Token: 0x06007D02 RID: 32002 RVA: 0x000F6BA8 File Offset: 0x000F4DA8
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.resultTexture, this.material);
	}

	// Token: 0x06007D03 RID: 32003 RVA: 0x000F6BBC File Offset: 0x000F4DBC
	public RenderTexture GetTexture()
	{
		return this.resultTexture;
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool ShouldFlip()
	{
		return false;
	}

	// Token: 0x04005E24 RID: 24100
	public string TextureName;

	// Token: 0x04005E25 RID: 24101
	private RenderTexture resultTexture;

	// Token: 0x04005E26 RID: 24102
	private Material material;
}
