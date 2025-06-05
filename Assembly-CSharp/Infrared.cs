using System;
using UnityEngine;

// Token: 0x02001ADF RID: 6879
public class Infrared : MonoBehaviour
{
	// Token: 0x06008FC9 RID: 36809 RVA: 0x00102575 File Offset: 0x00100775
	public static void DestroyInstance()
	{
		Infrared.Instance = null;
	}

	// Token: 0x06008FCA RID: 36810 RVA: 0x0010257D File Offset: 0x0010077D
	private void Awake()
	{
		Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
		Infrared.Instance = this;
		this.OnResize();
		this.UpdateState();
	}

	// Token: 0x06008FCB RID: 36811 RVA: 0x001025A0 File Offset: 0x001007A0
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.minionTexture);
		Graphics.Blit(source, dest);
	}

	// Token: 0x06008FCC RID: 36812 RVA: 0x00385420 File Offset: 0x00383620
	private void OnResize()
	{
		if (this.minionTexture != null)
		{
			this.minionTexture.DestroyRenderTexture();
		}
		if (this.cameraTexture != null)
		{
			this.cameraTexture.DestroyRenderTexture();
		}
		int num = 2;
		this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		base.GetComponent<Camera>().targetTexture = this.cameraTexture;
	}

	// Token: 0x06008FCD RID: 36813 RVA: 0x003854A8 File Offset: 0x003836A8
	public void SetMode(Infrared.Mode mode)
	{
		Vector4 zero;
		if (mode != Infrared.Mode.Disabled)
		{
			if (mode != Infrared.Mode.Disease)
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
			}
			else
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
				GameComps.InfraredVisualizers.ClearOverlayColour();
			}
		}
		else
		{
			zero = Vector4.zero;
		}
		Shader.SetGlobalVector("_ColouredOverlayParameters", zero);
		this.mode = mode;
		this.UpdateState();
	}

	// Token: 0x06008FCE RID: 36814 RVA: 0x001025B5 File Offset: 0x001007B5
	private void UpdateState()
	{
		base.gameObject.SetActive(this.mode > Infrared.Mode.Disabled);
		if (base.gameObject.activeSelf)
		{
			this.Update();
		}
	}

	// Token: 0x06008FCF RID: 36815 RVA: 0x00385520 File Offset: 0x00383720
	private void Update()
	{
		switch (this.mode)
		{
		case Infrared.Mode.Disabled:
			break;
		case Infrared.Mode.Infrared:
			GameComps.InfraredVisualizers.UpdateTemperature();
			return;
		case Infrared.Mode.Disease:
			GameComps.DiseaseContainers.UpdateOverlayColours();
			break;
		default:
			return;
		}
	}

	// Token: 0x04006C5D RID: 27741
	private RenderTexture minionTexture;

	// Token: 0x04006C5E RID: 27742
	private RenderTexture cameraTexture;

	// Token: 0x04006C5F RID: 27743
	private Infrared.Mode mode;

	// Token: 0x04006C60 RID: 27744
	public static int temperatureParametersId;

	// Token: 0x04006C61 RID: 27745
	public static Infrared Instance;

	// Token: 0x02001AE0 RID: 6880
	public enum Mode
	{
		// Token: 0x04006C63 RID: 27747
		Disabled,
		// Token: 0x04006C64 RID: 27748
		Infrared,
		// Token: 0x04006C65 RID: 27749
		Disease
	}
}
