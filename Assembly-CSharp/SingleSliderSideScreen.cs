using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02002040 RID: 8256
public class SingleSliderSideScreen : SideScreenContent
{
	// Token: 0x0600AF22 RID: 44834 RVA: 0x00429260 File Offset: 0x00427460
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x0600AF23 RID: 44835 RVA: 0x0042929C File Offset: 0x0042749C
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		ISingleSliderControl singleSliderControl = target.GetComponent<ISingleSliderControl>();
		singleSliderControl = ((singleSliderControl != null) ? singleSliderControl : target.GetSMI<ISingleSliderControl>());
		return singleSliderControl != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor) && singleSliderControl.GetSliderMin(0) != singleSliderControl.GetSliderMax(0);
	}

	// Token: 0x0600AF24 RID: 44836 RVA: 0x00429334 File Offset: 0x00427534
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ISingleSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<ISingleSliderControl>();
			if (this.target == null)
			{
				global::Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040089A6 RID: 35238
	private ISingleSliderControl target;

	// Token: 0x040089A7 RID: 35239
	public List<SliderSet> sliderSets;
}
