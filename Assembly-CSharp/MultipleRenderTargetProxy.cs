using System;
using UnityEngine;

// Token: 0x020017DD RID: 6109
public class MultipleRenderTargetProxy : MonoBehaviour
{
	// Token: 0x06007D8D RID: 32141 RVA: 0x0033292C File Offset: 0x00330B2C
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.CreateRenderTarget();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06007D8E RID: 32142 RVA: 0x000F7309 File Offset: 0x000F5509
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.colouredOverlayBufferEnabled = enabled;
		this.CreateRenderTarget();
	}

	// Token: 0x06007D8F RID: 32143 RVA: 0x00332984 File Offset: 0x00330B84
	private void CreateRenderTarget()
	{
		RenderBuffer[] array = new RenderBuffer[this.colouredOverlayBufferEnabled ? 3 : 2];
		this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
		this.Textures[0].filterMode = FilterMode.Point;
		this.Textures[0].name = "MRT0";
		this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.R8);
		this.Textures[1].filterMode = FilterMode.Point;
		this.Textures[1].name = "MRT1";
		array[0] = this.Textures[0].colorBuffer;
		array[1] = this.Textures[1].colorBuffer;
		if (this.colouredOverlayBufferEnabled)
		{
			this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
			this.Textures[2].filterMode = FilterMode.Bilinear;
			this.Textures[2].name = "MRT2";
			array[2] = this.Textures[2].colorBuffer;
		}
		base.GetComponent<Camera>().SetTargetBuffers(array, this.Textures[0].depthBuffer);
		this.OnShadersReloaded();
	}

	// Token: 0x06007D90 RID: 32144 RVA: 0x00332AB0 File Offset: 0x00330CB0
	private RenderTexture RecreateRT(RenderTexture rt, int depth, RenderTextureFormat format)
	{
		RenderTexture result = rt;
		if (rt == null || rt.width != Screen.width || rt.height != Screen.height || rt.format != format)
		{
			if (rt != null)
			{
				rt.DestroyRenderTexture();
			}
			result = new RenderTexture(Screen.width, Screen.height, depth, format);
		}
		return result;
	}

	// Token: 0x06007D91 RID: 32145 RVA: 0x000F7318 File Offset: 0x000F5518
	private void OnResize()
	{
		this.CreateRenderTarget();
	}

	// Token: 0x06007D92 RID: 32146 RVA: 0x000F7320 File Offset: 0x000F5520
	private void Update()
	{
		if (!this.Textures[0].IsCreated())
		{
			this.CreateRenderTarget();
		}
	}

	// Token: 0x06007D93 RID: 32147 RVA: 0x000F7337 File Offset: 0x000F5537
	private void OnShadersReloaded()
	{
		Shader.SetGlobalTexture("_MRT0", this.Textures[0]);
		Shader.SetGlobalTexture("_MRT1", this.Textures[1]);
		if (this.colouredOverlayBufferEnabled)
		{
			Shader.SetGlobalTexture("_MRT2", this.Textures[2]);
		}
	}

	// Token: 0x04005F4F RID: 24399
	public RenderTexture[] Textures = new RenderTexture[3];

	// Token: 0x04005F50 RID: 24400
	private bool colouredOverlayBufferEnabled;
}
