using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001FE0 RID: 8160
public class IntSliderSideScreen : SideScreenContent
{
	// Token: 0x0600AC61 RID: 44129 RVA: 0x0041D6E0 File Offset: 0x0041B8E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
			this.sliderSets[i].valueSlider.wholeNumbers = true;
		}
	}

	// Token: 0x0600AC62 RID: 44130 RVA: 0x0011482A File Offset: 0x00112A2A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;
	}

	// Token: 0x0600AC63 RID: 44131 RVA: 0x0041D734 File Offset: 0x0041B934
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IIntSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IIntSliderControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a Manual Generator component");
			return;
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040087B9 RID: 34745
	private IIntSliderControl target;

	// Token: 0x040087BA RID: 34746
	public List<SliderSet> sliderSets;
}
