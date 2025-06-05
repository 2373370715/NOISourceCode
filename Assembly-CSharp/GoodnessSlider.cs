using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D38 RID: 7480
[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
	// Token: 0x06009C38 RID: 39992 RVA: 0x0010A179 File Offset: 0x00108379
	protected override void OnSpawn()
	{
		base.Spawn();
		this.UpdateValues();
	}

	// Token: 0x06009C39 RID: 39993 RVA: 0x003CFBD8 File Offset: 0x003CDDD8
	public void UpdateValues()
	{
		this.text.color = (this.fill.color = this.gradient.Evaluate(this.slider.value));
		for (int i = 0; i < this.gradient.colorKeys.Length; i++)
		{
			if (this.gradient.colorKeys[i].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
			if (i == this.gradient.colorKeys.Length - 1 && this.gradient.colorKeys[i - 1].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
		}
	}

	// Token: 0x04007A2E RID: 31278
	public Image icon;

	// Token: 0x04007A2F RID: 31279
	public Text text;

	// Token: 0x04007A30 RID: 31280
	public Slider slider;

	// Token: 0x04007A31 RID: 31281
	public Image fill;

	// Token: 0x04007A32 RID: 31282
	public Gradient gradient;

	// Token: 0x04007A33 RID: 31283
	public string[] names;
}
