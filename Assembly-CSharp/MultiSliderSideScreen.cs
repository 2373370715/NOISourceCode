using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FF5 RID: 8181
public class MultiSliderSideScreen : SideScreenContent
{
	// Token: 0x0600ACE9 RID: 44265 RVA: 0x0042020C File Offset: 0x0041E40C
	public override bool IsValidForTarget(GameObject target)
	{
		IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
		return component != null && component.SidescreenEnabled();
	}

	// Token: 0x0600ACEA RID: 44266 RVA: 0x00114CD7 File Offset: 0x00112ED7
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IMultiSliderControl>();
		this.titleKey = this.target.SidescreenTitleKey;
		this.Refresh();
	}

	// Token: 0x0600ACEB RID: 44267 RVA: 0x0042022C File Offset: 0x0041E42C
	private void Refresh()
	{
		while (this.liveSliders.Count < this.target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			SliderSet sliderSet = new SliderSet();
			sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
			sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
			if (sliderSet.numberInput != null)
			{
				sliderSet.numberInput.Activate();
			}
			sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
			sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
			sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
			sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
			sliderSet.SetupSlider(this.liveSliders.Count);
			this.liveSliders.Add(gameObject);
			this.sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < this.liveSliders.Count; i++)
		{
			if (i >= this.target.sliderControls.Length)
			{
				this.liveSliders[i].SetActive(false);
			}
			else
			{
				if (!this.liveSliders[i].activeSelf)
				{
					this.liveSliders[i].SetActive(true);
					this.liveSliders[i].gameObject.SetActive(true);
				}
				this.sliderSets[i].SetTarget(this.target.sliderControls[i], i);
			}
		}
	}

	// Token: 0x0400881E RID: 34846
	public LayoutElement sliderPrefab;

	// Token: 0x0400881F RID: 34847
	public RectTransform sliderContainer;

	// Token: 0x04008820 RID: 34848
	private IMultiSliderControl target;

	// Token: 0x04008821 RID: 34849
	private List<GameObject> liveSliders = new List<GameObject>();

	// Token: 0x04008822 RID: 34850
	private List<SliderSet> sliderSets = new List<SliderSet>();
}
