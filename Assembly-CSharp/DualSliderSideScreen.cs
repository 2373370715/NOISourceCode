using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001FC8 RID: 8136
public class DualSliderSideScreen : SideScreenContent
{
	// Token: 0x0600ABF5 RID: 44021 RVA: 0x0041BAF0 File Offset: 0x00419CF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x0600ABF6 RID: 44022 RVA: 0x00114360 File Offset: 0x00112560
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDualSliderControl>() != null;
	}

	// Token: 0x0600ABF7 RID: 44023 RVA: 0x0041BB2C File Offset: 0x00419D2C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IDualSliderControl>();
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

	// Token: 0x04008768 RID: 34664
	private IDualSliderControl target;

	// Token: 0x04008769 RID: 34665
	public List<SliderSet> sliderSets;
}
