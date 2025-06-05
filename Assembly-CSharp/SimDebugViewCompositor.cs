using System;
using UnityEngine;

// Token: 0x020017E2 RID: 6114
public class SimDebugViewCompositor : MonoBehaviour
{
	// Token: 0x06007DA9 RID: 32169 RVA: 0x000F74AF File Offset: 0x000F56AF
	private void Awake()
	{
		SimDebugViewCompositor.Instance = this;
	}

	// Token: 0x06007DAA RID: 32170 RVA: 0x000F74B7 File Offset: 0x000F56B7
	private void OnDestroy()
	{
		SimDebugViewCompositor.Instance = null;
	}

	// Token: 0x06007DAB RID: 32171 RVA: 0x000F74BF File Offset: 0x000F56BF
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
		this.Toggle(false);
	}

	// Token: 0x06007DAC RID: 32172 RVA: 0x000F74DD File Offset: 0x000F56DD
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen.Instance.RunPostProcessEffects(src, dest);
		}
	}

	// Token: 0x06007DAD RID: 32173 RVA: 0x000F7505 File Offset: 0x000F5705
	public void Toggle(bool is_on)
	{
		base.enabled = is_on;
	}

	// Token: 0x04005F68 RID: 24424
	public Material material;

	// Token: 0x04005F69 RID: 24425
	public static SimDebugViewCompositor Instance;
}
