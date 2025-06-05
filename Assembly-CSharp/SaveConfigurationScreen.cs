using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F5D RID: 8029
[Serializable]
public class SaveConfigurationScreen
{
	// Token: 0x0600A969 RID: 43369 RVA: 0x004110FC File Offset: 0x0040F2FC
	public void ToggleDisabledContent(bool enable)
	{
		if (enable)
		{
			this.disabledContentPanel.SetActive(true);
			this.disabledContentWarning.SetActive(false);
			this.perSaveWarning.SetActive(true);
			return;
		}
		this.disabledContentPanel.SetActive(false);
		this.disabledContentWarning.SetActive(true);
		this.perSaveWarning.SetActive(false);
	}

	// Token: 0x0600A96A RID: 43370 RVA: 0x00411158 File Offset: 0x0040F358
	public void Init()
	{
		this.autosaveFrequencySlider.minValue = 0f;
		this.autosaveFrequencySlider.maxValue = (float)(this.sliderValueToCycleCount.Length - 1);
		this.autosaveFrequencySlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnAutosaveValueChanged(Mathf.FloorToInt(val));
		});
		this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
		this.timelapseResolutionSlider.minValue = 0f;
		this.timelapseResolutionSlider.maxValue = (float)(this.sliderValueToResolution.Length - 1);
		this.timelapseResolutionSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnTimelapseValueChanged(Mathf.FloorToInt(val));
		});
		this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
		this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
	}

	// Token: 0x0600A96B RID: 43371 RVA: 0x00411238 File Offset: 0x0040F438
	public void Show(bool show)
	{
		if (show)
		{
			this.autosaveFrequencySlider.value = (float)this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
			this.timelapseResolutionSlider.value = (float)this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
			this.OnAutosaveValueChanged(Mathf.FloorToInt(this.autosaveFrequencySlider.value));
			this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
		}
	}

	// Token: 0x0600A96C RID: 43372 RVA: 0x004112AC File Offset: 0x0040F4AC
	private void OnTimelapseValueChanged(int sliderValue)
	{
		Vector2I vector2I = this.SliderValueToResolution(sliderValue);
		if (vector2I.x <= 0)
		{
			this.timelapseDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
		}
		else
		{
			this.timelapseDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, vector2I.x, vector2I.y));
		}
		SaveGame.Instance.TimelapseResolution = vector2I;
		Game.Instance.Trigger(75424175, null);
	}

	// Token: 0x0600A96D RID: 43373 RVA: 0x0041132C File Offset: 0x0040F52C
	private void OnAutosaveValueChanged(int sliderValue)
	{
		int num = this.SliderValueToCycleCount(sliderValue);
		if (sliderValue == 0)
		{
			this.autosaveDescriptionLabel.SetText(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
		}
		else
		{
			this.autosaveDescriptionLabel.SetText(string.Format(UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, num));
		}
		SaveGame.Instance.AutoSaveCycleInterval = num;
	}

	// Token: 0x0600A96E RID: 43374 RVA: 0x0011278F File Offset: 0x0011098F
	private int SliderValueToCycleCount(int sliderValue)
	{
		return this.sliderValueToCycleCount[sliderValue];
	}

	// Token: 0x0600A96F RID: 43375 RVA: 0x00411388 File Offset: 0x0040F588
	private int CycleCountToSlider(int count)
	{
		for (int i = 0; i < this.sliderValueToCycleCount.Length; i++)
		{
			if (this.sliderValueToCycleCount[i] == count)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x0600A970 RID: 43376 RVA: 0x00112799 File Offset: 0x00110999
	private Vector2I SliderValueToResolution(int sliderValue)
	{
		return this.sliderValueToResolution[sliderValue];
	}

	// Token: 0x0600A971 RID: 43377 RVA: 0x004113B8 File Offset: 0x0040F5B8
	private int ResolutionToSliderValue(Vector2I resolution)
	{
		for (int i = 0; i < this.sliderValueToResolution.Length; i++)
		{
			if (this.sliderValueToResolution[i] == resolution)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04008570 RID: 34160
	[SerializeField]
	private KSlider autosaveFrequencySlider;

	// Token: 0x04008571 RID: 34161
	[SerializeField]
	private LocText timelapseDescriptionLabel;

	// Token: 0x04008572 RID: 34162
	[SerializeField]
	private KSlider timelapseResolutionSlider;

	// Token: 0x04008573 RID: 34163
	[SerializeField]
	private LocText autosaveDescriptionLabel;

	// Token: 0x04008574 RID: 34164
	private int[] sliderValueToCycleCount = new int[]
	{
		-1,
		50,
		20,
		10,
		5,
		2,
		1
	};

	// Token: 0x04008575 RID: 34165
	private Vector2I[] sliderValueToResolution = new Vector2I[]
	{
		new Vector2I(-1, -1),
		new Vector2I(256, 384),
		new Vector2I(512, 768),
		new Vector2I(1024, 1536),
		new Vector2I(2048, 3072),
		new Vector2I(4096, 6144),
		new Vector2I(8192, 12288)
	};

	// Token: 0x04008576 RID: 34166
	[SerializeField]
	private GameObject disabledContentPanel;

	// Token: 0x04008577 RID: 34167
	[SerializeField]
	private GameObject disabledContentWarning;

	// Token: 0x04008578 RID: 34168
	[SerializeField]
	private GameObject perSaveWarning;
}
