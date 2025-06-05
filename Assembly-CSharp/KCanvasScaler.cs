using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D7A RID: 7546
[AddComponentMenu("KMonoBehaviour/scripts/KCanvasScaler")]
public class KCanvasScaler : KMonoBehaviour
{
	// Token: 0x06009D87 RID: 40327 RVA: 0x003D7DE8 File Offset: 0x003D5FE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (KPlayerPrefs.HasKey(KCanvasScaler.UIScalePrefKey))
		{
			this.SetUserScale(KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey) / 100f);
		}
		else
		{
			this.SetUserScale(1f);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06009D88 RID: 40328 RVA: 0x0010AF67 File Offset: 0x00109167
	private void OnResize()
	{
		this.SetUserScale(this.userScale);
	}

	// Token: 0x06009D89 RID: 40329 RVA: 0x0010AF75 File Offset: 0x00109175
	public void SetUserScale(float scale)
	{
		if (this.canvasScaler == null)
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
		}
		this.userScale = scale;
		this.canvasScaler.scaleFactor = this.GetCanvasScale();
	}

	// Token: 0x06009D8A RID: 40330 RVA: 0x0010AFA9 File Offset: 0x001091A9
	public float GetUserScale()
	{
		return this.userScale;
	}

	// Token: 0x06009D8B RID: 40331 RVA: 0x0010AFB1 File Offset: 0x001091B1
	public float GetCanvasScale()
	{
		return this.userScale * this.ScreenRelativeScale();
	}

	// Token: 0x06009D8C RID: 40332 RVA: 0x003D7E50 File Offset: 0x003D6050
	private float ScreenRelativeScale()
	{
		float dpi = Screen.dpi;
		Camera x = Camera.main;
		if (x == null)
		{
			x = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		x != null;
		float num = (float)Screen.width / (float)Screen.height;
		if ((float)Screen.height <= this.scaleSteps[0].maxRes_y || num < 1.6f)
		{
			return this.scaleSteps[0].scale;
		}
		if ((float)Screen.height > this.scaleSteps[this.scaleSteps.Length - 1].maxRes_y)
		{
			return this.scaleSteps[this.scaleSteps.Length - 1].scale;
		}
		for (int i = 0; i < this.scaleSteps.Length; i++)
		{
			if ((float)Screen.height > this.scaleSteps[i].maxRes_y && (float)Screen.height <= this.scaleSteps[i + 1].maxRes_y)
			{
				float t = ((float)Screen.height - this.scaleSteps[i].maxRes_y) / (this.scaleSteps[i + 1].maxRes_y - this.scaleSteps[i].maxRes_y);
				return Mathf.Lerp(this.scaleSteps[i].scale, this.scaleSteps[i + 1].scale, t);
			}
		}
		return 1f;
	}

	// Token: 0x04007BAF RID: 31663
	[MyCmpReq]
	private CanvasScaler canvasScaler;

	// Token: 0x04007BB0 RID: 31664
	public static string UIScalePrefKey = "UIScalePref";

	// Token: 0x04007BB1 RID: 31665
	private float userScale = 1f;

	// Token: 0x04007BB2 RID: 31666
	[Range(0.75f, 2f)]
	private KCanvasScaler.ScaleStep[] scaleSteps = new KCanvasScaler.ScaleStep[]
	{
		new KCanvasScaler.ScaleStep(720f, 0.86f),
		new KCanvasScaler.ScaleStep(1080f, 1f),
		new KCanvasScaler.ScaleStep(2160f, 1.33f)
	};

	// Token: 0x02001D7B RID: 7547
	[Serializable]
	public struct ScaleStep
	{
		// Token: 0x06009D8F RID: 40335 RVA: 0x0010AFCC File Offset: 0x001091CC
		public ScaleStep(float maxRes_y, float scale)
		{
			this.maxRes_y = maxRes_y;
			this.scale = scale;
		}

		// Token: 0x04007BB3 RID: 31667
		public float scale;

		// Token: 0x04007BB4 RID: 31668
		public float maxRes_y;
	}
}
