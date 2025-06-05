using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B9F RID: 2975
public class UserMenu
{
	// Token: 0x060037F1 RID: 14321 RVA: 0x000C8BA5 File Offset: 0x000C6DA5
	public void Refresh(GameObject go)
	{
		Game.Instance.Trigger(1980521255, go);
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x00226DA0 File Offset: 0x00224FA0
	public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
	{
		if (button.onClick != null)
		{
			System.Action callback = button.onClick;
			button.onClick = delegate()
			{
				callback();
				Game.Instance.Trigger(1980521255, go);
			};
		}
		this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x000C8BB7 File Offset: 0x000C6DB7
	public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
	{
		this.sliders.Add(slider);
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x00226DF4 File Offset: 0x00224FF4
	public void AppendToScreen(GameObject go, UserMenuScreen screen)
	{
		this.buttons.Clear();
		this.sliders.Clear();
		go.Trigger(493375141, null);
		if (this.buttons.Count > 0)
		{
			this.buttons.Sort(delegate(KeyValuePair<KIconButtonMenu.ButtonInfo, float> x, KeyValuePair<KIconButtonMenu.ButtonInfo, float> y)
			{
				if (x.Value == y.Value)
				{
					return 0;
				}
				if (x.Value > y.Value)
				{
					return 1;
				}
				return -1;
			});
			for (int i = 0; i < this.buttons.Count; i++)
			{
				this.sortedButtons.Add(this.buttons[i].Key);
			}
			screen.AddButtons(this.sortedButtons);
			this.sortedButtons.Clear();
		}
		if (this.sliders.Count > 0)
		{
			screen.AddSliders(this.sliders);
		}
	}

	// Token: 0x04002683 RID: 9859
	public const float DECONSTRUCT_PRIORITY = 0f;

	// Token: 0x04002684 RID: 9860
	public const float DRAWPATHS_PRIORITY = 0.1f;

	// Token: 0x04002685 RID: 9861
	public const float FOLLOWCAM_PRIORITY = 0.3f;

	// Token: 0x04002686 RID: 9862
	public const float SETDIRECTION_PRIORITY = 0.4f;

	// Token: 0x04002687 RID: 9863
	public const float AUTOBOTTLE_PRIORITY = 0.4f;

	// Token: 0x04002688 RID: 9864
	public const float AUTOREPAIR_PRIORITY = 0.5f;

	// Token: 0x04002689 RID: 9865
	public const float DEFAULT_PRIORITY = 1f;

	// Token: 0x0400268A RID: 9866
	public const float SUITEQUIP_PRIORITY = 2f;

	// Token: 0x0400268B RID: 9867
	public const float AUTODISINFECT_PRIORITY = 10f;

	// Token: 0x0400268C RID: 9868
	public const float ROCKETUSAGERESTRICTION_PRIORITY = 11f;

	// Token: 0x0400268D RID: 9869
	private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();

	// Token: 0x0400268E RID: 9870
	private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();

	// Token: 0x0400268F RID: 9871
	private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x02000BA0 RID: 2976
	public class SliderInfo
	{
		// Token: 0x04002690 RID: 9872
		public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

		// Token: 0x04002691 RID: 9873
		public MinMaxSlider.Mode mode;

		// Token: 0x04002692 RID: 9874
		public Slider.Direction direction;

		// Token: 0x04002693 RID: 9875
		public bool interactable = true;

		// Token: 0x04002694 RID: 9876
		public bool lockRange;

		// Token: 0x04002695 RID: 9877
		public string toolTip;

		// Token: 0x04002696 RID: 9878
		public string toolTipMin;

		// Token: 0x04002697 RID: 9879
		public string toolTipMax;

		// Token: 0x04002698 RID: 9880
		public float minLimit;

		// Token: 0x04002699 RID: 9881
		public float maxLimit = 100f;

		// Token: 0x0400269A RID: 9882
		public float currentMinValue = 10f;

		// Token: 0x0400269B RID: 9883
		public float currentMaxValue = 90f;

		// Token: 0x0400269C RID: 9884
		public GameObject sliderGO;

		// Token: 0x0400269D RID: 9885
		public Action<MinMaxSlider> onMinChange;

		// Token: 0x0400269E RID: 9886
		public Action<MinMaxSlider> onMaxChange;
	}
}
