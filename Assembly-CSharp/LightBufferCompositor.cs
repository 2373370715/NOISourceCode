using System;
using UnityEngine;

// Token: 0x020017CF RID: 6095
public class LightBufferCompositor : MonoBehaviour
{
	// Token: 0x06007D5A RID: 32090 RVA: 0x0033109C File Offset: 0x0032F29C
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/LightBufferCompositor"));
		this.material.SetTexture("_InvalidTex", Assets.instance.invalidAreaTex);
		this.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		this.OnShadersReloaded();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x06007D5B RID: 32091 RVA: 0x000F70FB File Offset: 0x000F52FB
	private void OnEnable()
	{
		this.OnShadersReloaded();
	}

	// Token: 0x06007D5C RID: 32092 RVA: 0x00331104 File Offset: 0x0032F304
	private void DownSample4x(Texture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06007D5D RID: 32093 RVA: 0x000F7103 File Offset: 0x000F5303
	[ContextMenu("ToggleParticles")]
	private void ToggleParticles()
	{
		this.particlesEnabled = !this.particlesEnabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06007D5E RID: 32094 RVA: 0x000F711A File Offset: 0x000F531A
	public void SetParticlesEnabled(bool enabled)
	{
		this.particlesEnabled = enabled;
		this.UpdateMaterialState();
	}

	// Token: 0x06007D5F RID: 32095 RVA: 0x000F7129 File Offset: 0x000F5329
	private void UpdateMaterialState()
	{
		if (this.particlesEnabled)
		{
			this.material.DisableKeyword("DISABLE_TEMPERATURE_PARTICLES");
			return;
		}
		this.material.EnableKeyword("DISABLE_TEMPERATURE_PARTICLES");
	}

	// Token: 0x06007D60 RID: 32096 RVA: 0x00331168 File Offset: 0x0032F368
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		RenderTexture renderTexture = null;
		if (PropertyTextures.instance != null)
		{
			Texture texture = PropertyTextures.instance.GetTexture(PropertyTextures.Property.Temperature);
			texture.name = "temperature_tex";
			renderTexture = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(texture, renderTexture, this.blurMaterial);
			Shader.SetGlobalTexture("_BlurredTemperature", renderTexture);
		}
		this.material.SetTexture("_LightBufferTex", LightBuffer.Instance.Texture);
		Graphics.Blit(src, dest, this.material);
		if (renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}

	// Token: 0x06007D61 RID: 32097 RVA: 0x00331204 File Offset: 0x0032F404
	private void OnShadersReloaded()
	{
		if (this.material != null && Lighting.Instance != null)
		{
			this.material.SetTexture("_EmberTex", Lighting.Instance.Settings.EmberTex);
			this.material.SetTexture("_FrostTex", Lighting.Instance.Settings.FrostTex);
			this.material.SetTexture("_Thermal1Tex", Lighting.Instance.Settings.Thermal1Tex);
			this.material.SetTexture("_Thermal2Tex", Lighting.Instance.Settings.Thermal2Tex);
			this.material.SetTexture("_RadHaze1Tex", Lighting.Instance.Settings.Radiation1Tex);
			this.material.SetTexture("_RadHaze2Tex", Lighting.Instance.Settings.Radiation2Tex);
			this.material.SetTexture("_RadHaze3Tex", Lighting.Instance.Settings.Radiation3Tex);
			this.material.SetTexture("_NoiseTex", Lighting.Instance.Settings.NoiseTex);
		}
	}

	// Token: 0x04005E68 RID: 24168
	[SerializeField]
	private Material material;

	// Token: 0x04005E69 RID: 24169
	[SerializeField]
	private Material blurMaterial;

	// Token: 0x04005E6A RID: 24170
	private bool particlesEnabled = true;
}
