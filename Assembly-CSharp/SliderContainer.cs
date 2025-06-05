using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200206E RID: 8302
[AddComponentMenu("KMonoBehaviour/scripts/SliderContainer")]
public class SliderContainer : KMonoBehaviour
{
	// Token: 0x0600B0B4 RID: 45236 RVA: 0x0011775C File Offset: 0x0011595C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.UpdateSliderLabel));
	}

	// Token: 0x0600B0B5 RID: 45237 RVA: 0x004334F0 File Offset: 0x004316F0
	public void UpdateSliderLabel(float newValue)
	{
		if (this.isPercentValue)
		{
			this.valueLabel.text = (newValue * 100f).ToString("F0") + "%";
			return;
		}
		this.valueLabel.text = newValue.ToString();
	}

	// Token: 0x04008B25 RID: 35621
	public bool isPercentValue = true;

	// Token: 0x04008B26 RID: 35622
	public KSlider slider;

	// Token: 0x04008B27 RID: 35623
	public LocText nameLabel;

	// Token: 0x04008B28 RID: 35624
	public LocText valueLabel;
}
