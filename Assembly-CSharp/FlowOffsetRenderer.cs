using System;
using UnityEngine;

// Token: 0x0200133E RID: 4926
[AddComponentMenu("KMonoBehaviour/scripts/FlowOffsetRenderer")]
public class FlowOffsetRenderer : KMonoBehaviour
{
	// Token: 0x060064E0 RID: 25824 RVA: 0x002CF044 File Offset: 0x002CD244
	protected override void OnSpawn()
	{
		this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.DoUpdate(0.1f);
	}

	// Token: 0x060064E1 RID: 25825 RVA: 0x002CF0A0 File Offset: 0x002CD2A0
	private void OnResize()
	{
		for (int i = 0; i < this.OffsetTextures.Length; i++)
		{
			if (this.OffsetTextures[i] != null)
			{
				this.OffsetTextures[i].DestroyRenderTexture();
			}
			this.OffsetTextures[i] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
			this.OffsetTextures[i].filterMode = FilterMode.Bilinear;
			this.OffsetTextures[i].name = "FlowOffsetTexture";
		}
	}

	// Token: 0x060064E2 RID: 25826 RVA: 0x002CF11C File Offset: 0x002CD31C
	private void LateUpdate()
	{
		if ((Time.deltaTime > 0f && Time.timeScale > 0f) || this.forceUpdate)
		{
			float num = Time.deltaTime / Time.timeScale;
			this.DoUpdate(num * Time.timeScale / 4f + num * 0.5f);
		}
	}

	// Token: 0x060064E3 RID: 25827 RVA: 0x002CF170 File Offset: 0x002CD370
	private void DoUpdate(float dt)
	{
		this.CurrentTime += dt;
		float num = this.CurrentTime * this.PhaseMultiplier;
		num -= (float)((int)num);
		float num2 = num - (float)((int)num);
		float y = 1f;
		if (num2 <= this.GasPhase0)
		{
			y = 0f;
		}
		this.GasPhase0 = num2;
		float z = 1f;
		float num3 = num + 0.5f - (float)((int)(num + 0.5f));
		if (num3 <= this.GasPhase1)
		{
			z = 0f;
		}
		this.GasPhase1 = num3;
		Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0f, 0f, 0f));
		Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0f, 0f));
		RenderTexture renderTexture = this.OffsetTextures[this.OffsetIdx];
		this.OffsetIdx = (this.OffsetIdx + 1) % 2;
		RenderTexture renderTexture2 = this.OffsetTextures[this.OffsetIdx];
		Material flowMaterial = this.FlowMaterial;
		flowMaterial.SetTexture("_PreviousOffsetTex", renderTexture);
		flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0f));
		flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
		flowMaterial.SetVector("_VisibleArea", new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells));
		flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0f, 0f));
		Graphics.Blit(renderTexture, renderTexture2, flowMaterial);
		Shader.SetGlobalTexture(this.OffsetTextureName, renderTexture2);
	}

	// Token: 0x04004896 RID: 18582
	private float GasPhase0;

	// Token: 0x04004897 RID: 18583
	private float GasPhase1;

	// Token: 0x04004898 RID: 18584
	public float PhaseMultiplier;

	// Token: 0x04004899 RID: 18585
	public float NoiseInfluence;

	// Token: 0x0400489A RID: 18586
	public float NoiseScale;

	// Token: 0x0400489B RID: 18587
	public float OffsetSpeed;

	// Token: 0x0400489C RID: 18588
	public string OffsetTextureName;

	// Token: 0x0400489D RID: 18589
	public string ParametersName;

	// Token: 0x0400489E RID: 18590
	public Vector2 MinFlow0;

	// Token: 0x0400489F RID: 18591
	public Vector2 MinFlow1;

	// Token: 0x040048A0 RID: 18592
	public Vector2 LiquidGasMask;

	// Token: 0x040048A1 RID: 18593
	[SerializeField]
	private Material FlowMaterial;

	// Token: 0x040048A2 RID: 18594
	[SerializeField]
	private bool forceUpdate;

	// Token: 0x040048A3 RID: 18595
	private TextureLerper FlowLerper;

	// Token: 0x040048A4 RID: 18596
	public RenderTexture[] OffsetTextures = new RenderTexture[2];

	// Token: 0x040048A5 RID: 18597
	private int OffsetIdx;

	// Token: 0x040048A6 RID: 18598
	private float CurrentTime;
}
