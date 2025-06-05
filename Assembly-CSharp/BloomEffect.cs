using System;
using UnityEngine;

// Token: 0x02001ADC RID: 6876
public class BloomEffect : MonoBehaviour
{
	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06008FBC RID: 36796 RVA: 0x001024D4 File Offset: 0x001006D4
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.blurShader);
				this.m_Material.hideFlags = HideFlags.DontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06008FBD RID: 36797 RVA: 0x00102508 File Offset: 0x00100708
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06008FBE RID: 36798 RVA: 0x00385214 File Offset: 0x00383414
	protected void Start()
	{
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
		this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
	}

	// Token: 0x06008FBF RID: 36799 RVA: 0x00385274 File Offset: 0x00383474
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06008FC0 RID: 36800 RVA: 0x003852E0 File Offset: 0x003834E0
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06008FC1 RID: 36801 RVA: 0x00385344 File Offset: 0x00383544
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		temporary.name = "bloom_source";
		Graphics.Blit(source, temporary, this.BloomMaskMaterial);
		int width = Math.Max(source.width / 4, 4);
		int height = Math.Max(source.height / 4, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		renderTexture.name = "bloom_downsampled";
		this.DownSample4x(temporary, renderTexture);
		RenderTexture.ReleaseTemporary(temporary);
		for (int i = 0; i < this.iterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
			temporary2.name = "bloom_blurred";
			this.FourTapCone(renderTexture, temporary2, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary2;
		}
		this.BloomCompositeMaterial.SetTexture("_BloomTex", renderTexture);
		Graphics.Blit(source, destination, this.BloomCompositeMaterial);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04006C56 RID: 27734
	private Material BloomMaskMaterial;

	// Token: 0x04006C57 RID: 27735
	private Material BloomCompositeMaterial;

	// Token: 0x04006C58 RID: 27736
	public int iterations = 3;

	// Token: 0x04006C59 RID: 27737
	public float blurSpread = 0.6f;

	// Token: 0x04006C5A RID: 27738
	public Shader blurShader;

	// Token: 0x04006C5B RID: 27739
	private Material m_Material;
}
