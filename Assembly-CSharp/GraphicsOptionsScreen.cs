using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001D39 RID: 7481
internal class GraphicsOptionsScreen : KModalScreen
{
	// Token: 0x06009C3B RID: 39995 RVA: 0x003CFCB4 File Offset: 0x003CDEB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.TITLE);
		this.originalSettings = this.CaptureSettings();
		this.applyButton.isInteractable = false;
		this.applyButton.onClick += this.OnApply;
		this.applyButton.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.APPLYBUTTON);
		this.doneButton.onClick += this.OnDone;
		this.closeButton.onClick += this.OnDone;
		this.doneButton.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.DONE_BUTTON);
		bool flag = QualitySettings.GetQualityLevel() == 1;
		this.lowResToggle.ChangeState(flag ? 1 : 0);
		MultiToggle multiToggle = this.lowResToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnLowResToggle));
		this.lowResToggle.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.LOWRES);
		this.resolutionDropdown.ClearOptions();
		this.BuildOptions();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(delegate()
		{
			this.BuildOptions();
			this.resolutionDropdown.options = this.options;
		}));
		this.resolutionDropdown.options = this.options;
		this.resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionChanged));
		this.fullscreenToggle.ChangeState(Screen.fullScreen ? 1 : 0);
		MultiToggle multiToggle2 = this.fullscreenToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnFullscreenToggle));
		this.fullscreenToggle.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.FULLSCREEN);
		this.resolutionDropdown.transform.parent.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.RESOLUTION);
		if (this.fullscreenToggle.CurrentState == 1)
		{
			int resolutionIndex = this.GetResolutionIndex(this.originalSettings.resolution);
			if (resolutionIndex != -1)
			{
				this.resolutionDropdown.value = resolutionIndex;
			}
		}
		this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>(true);
		this.UpdateSliderLabel();
		this.uiScaleSlider.onValueChanged.AddListener(delegate(float data)
		{
			this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%";
		});
		this.uiScaleSlider.onReleaseHandle += delegate()
		{
			this.UpdateUIScale(this.uiScaleSlider.value);
		};
		this.BuildColorModeOptions();
		this.colorModeDropdown.options = this.colorModeOptions;
		this.colorModeDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnColorModeChanged));
		int value = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
		{
			value = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
		}
		this.colorModeDropdown.value = value;
		this.RefreshColorExamples(this.originalSettings.colorSetId);
	}

	// Token: 0x06009C3C RID: 39996 RVA: 0x0010A187 File Offset: 0x00108387
	public static void SetSettingsFromPrefs()
	{
		GraphicsOptionsScreen.SetResolutionFromPrefs();
		GraphicsOptionsScreen.SetLowResFromPrefs();
	}

	// Token: 0x06009C3D RID: 39997 RVA: 0x003CFF88 File Offset: 0x003CE188
	public static void SetLowResFromPrefs()
	{
		int num = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.LowResKey))
		{
			num = KPlayerPrefs.GetInt(GraphicsOptionsScreen.LowResKey);
			QualitySettings.SetQualityLevel(num, true);
		}
		else
		{
			QualitySettings.SetQualityLevel(num, true);
		}
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Low Res Textures? {0}", (num == 1) ? "Yes" : "No")
		});
	}

	// Token: 0x06009C3E RID: 39998 RVA: 0x003CFFE8 File Offset: 0x003CE1E8
	public static void SetResolutionFromPrefs()
	{
		int num = Screen.currentResolution.width;
		int num2 = Screen.currentResolution.height;
		RefreshRate refreshRate = Screen.currentResolution.refreshRateRatio;
		FullScreenMode fullScreenMode = Screen.fullScreenMode;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionWidthKey) && KPlayerPrefs.HasKey(GraphicsOptionsScreen.ResolutionHeightKey))
		{
			int @int = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionWidthKey);
			int int2 = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ResolutionHeightKey);
			uint int3 = (uint)KPlayerPrefs.GetInt(GraphicsOptionsScreen.RefreshRateKeyNumerator, (int)Screen.currentResolution.refreshRateRatio.numerator);
			uint int4 = (uint)KPlayerPrefs.GetInt(GraphicsOptionsScreen.RefreshRateKeyDenominator, (int)Screen.currentResolution.refreshRateRatio.denominator);
			FullScreenMode fullScreenMode2 = (KPlayerPrefs.GetInt(GraphicsOptionsScreen.FullScreenKey, Screen.fullScreen ? 1 : 0) == 1) ? FullScreenMode.MaximizedWindow : FullScreenMode.Windowed;
			if (int2 <= 1 || @int <= 1)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Saved resolution was invalid, ignoring..."
				});
			}
			else
			{
				num = @int;
				num2 = int2;
				refreshRate.numerator = int3;
				refreshRate.denominator = int4;
				fullScreenMode = fullScreenMode2;
			}
		}
		if (num <= 1 || num2 <= 1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Detected a degenerate resolution, attempting to fix..."
			});
			foreach (Resolution resolution in Screen.resolutions)
			{
				if (resolution.width == 1920)
				{
					num = resolution.width;
					num2 = resolution.height;
					refreshRate = default(RefreshRate);
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				foreach (Resolution resolution2 in Screen.resolutions)
				{
					if (resolution2.width == 1280)
					{
						num = resolution2.width;
						num2 = resolution2.height;
						refreshRate = default(RefreshRate);
					}
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				foreach (Resolution resolution3 in Screen.resolutions)
				{
					if (resolution3.width > 1 && resolution3.height > 1 && resolution3.refreshRateRatio.value > 0.0)
					{
						num = resolution3.width;
						num2 = resolution3.height;
						refreshRate = default(RefreshRate);
					}
				}
			}
			if (num <= 1 || num2 <= 1)
			{
				string text = "Could not find a suitable resolution for this screen! Reported available resolutions are:";
				foreach (Resolution resolution4 in Screen.resolutions)
				{
					text += string.Format("\n{0}x{1} @ {2}hz", resolution4.width, resolution4.height, resolution4.refreshRateRatio.value);
				}
				global::Debug.LogError(text);
				num = 1280;
				num2 = 720;
				fullScreenMode = FullScreenMode.Windowed;
				refreshRate = default(RefreshRate);
			}
		}
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Applying resolution {0}x{1} @{2}hz (fullscreen: {3})", new object[]
			{
				num,
				num2,
				refreshRate,
				fullScreenMode
			})
		});
		Screen.SetResolution(num, num2, fullScreenMode, refreshRate);
	}

	// Token: 0x06009C3F RID: 39999 RVA: 0x003D0304 File Offset: 0x003CE504
	public static void SetColorModeFromPrefs()
	{
		int num = 0;
		if (KPlayerPrefs.HasKey(GraphicsOptionsScreen.ColorModeKey))
		{
			num = KPlayerPrefs.GetInt(GraphicsOptionsScreen.ColorModeKey);
		}
		GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[num];
	}

	// Token: 0x06009C40 RID: 40000 RVA: 0x003D0340 File Offset: 0x003CE540
	public static void OnResize()
	{
		GraphicsOptionsScreen.Settings settings = default(GraphicsOptionsScreen.Settings);
		settings.resolution = Screen.currentResolution;
		settings.resolution.width = Screen.width;
		settings.resolution.height = Screen.height;
		settings.fullscreen = Screen.fullScreenMode;
		settings.lowRes = QualitySettings.GetQualityLevel();
		settings.colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet);
		GraphicsOptionsScreen.SaveSettingsToPrefs(settings);
	}

	// Token: 0x06009C41 RID: 40001 RVA: 0x003D03C4 File Offset: 0x003CE5C4
	private static void SaveSettingsToPrefs(GraphicsOptionsScreen.Settings settings)
	{
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.LowResKey, settings.lowRes);
		global::Debug.LogFormat("Screen resolution updated, saving values to prefs: {0}x{1} @ {2}, fullscreen: {3}", new object[]
		{
			settings.resolution.width,
			settings.resolution.height,
			settings.resolution.refreshRateRatio,
			settings.fullscreen
		});
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionWidthKey, settings.resolution.width);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ResolutionHeightKey, settings.resolution.height);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.RefreshRateKeyNumerator, (int)settings.resolution.refreshRateRatio.numerator);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.RefreshRateKeyDenominator, (int)settings.resolution.refreshRateRatio.denominator);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.FullScreenKey, (settings.fullscreen == FullScreenMode.Windowed) ? 0 : 1);
		KPlayerPrefs.SetInt(GraphicsOptionsScreen.ColorModeKey, settings.colorSetId);
	}

	// Token: 0x06009C42 RID: 40002 RVA: 0x003D04C4 File Offset: 0x003CE6C4
	private void UpdateUIScale(float value)
	{
		this.CanvasScalers = UnityEngine.Object.FindObjectsOfType<KCanvasScaler>(true);
		foreach (KCanvasScaler kcanvasScaler in this.CanvasScalers)
		{
			float userScale = value / 100f;
			kcanvasScaler.SetUserScale(userScale);
			KPlayerPrefs.SetFloat(KCanvasScaler.UIScalePrefKey, value);
		}
		ScreenResize.Instance.TriggerResize();
		this.UpdateSliderLabel();
	}

	// Token: 0x06009C43 RID: 40003 RVA: 0x003D0520 File Offset: 0x003CE720
	private void UpdateSliderLabel()
	{
		if (this.CanvasScalers != null && this.CanvasScalers.Length != 0 && this.CanvasScalers[0] != null)
		{
			this.uiScaleSlider.value = this.CanvasScalers[0].GetUserScale() * 100f;
			this.sliderLabel.text = this.uiScaleSlider.value.ToString() + "%";
		}
	}

	// Token: 0x06009C44 RID: 40004 RVA: 0x0010A193 File Offset: 0x00108393
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.resolutionDropdown.Hide();
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009C45 RID: 40005 RVA: 0x003D0594 File Offset: 0x003CE794
	private void BuildOptions()
	{
		this.options.Clear();
		this.resolutions.Clear();
		Resolution resolution = default(Resolution);
		resolution.width = Screen.width;
		resolution.height = Screen.height;
		resolution.refreshRateRatio = Screen.currentResolution.refreshRateRatio;
		this.options.Add(new Dropdown.OptionData(this.ResolutionDisplayString(resolution)));
		this.resolutions.Add(resolution);
		foreach (Resolution resolution2 in Screen.resolutions)
		{
			if (resolution2.height >= 720)
			{
				this.options.Add(new Dropdown.OptionData(this.ResolutionDisplayString(resolution2)));
				this.resolutions.Add(resolution2);
			}
		}
	}

	// Token: 0x06009C46 RID: 40006 RVA: 0x003D0660 File Offset: 0x003CE860
	private string ResolutionDisplayString(Resolution resolution)
	{
		return string.Format("{0} x {1} @ {2}Hz", resolution.width, resolution.height, Mathf.Floor((float)resolution.refreshRateRatio.value));
	}

	// Token: 0x06009C47 RID: 40007 RVA: 0x003D06AC File Offset: 0x003CE8AC
	private void BuildColorModeOptions()
	{
		this.colorModeOptions.Clear();
		for (int i = 0; i < GlobalAssets.Instance.colorSetOptions.Length; i++)
		{
			this.colorModeOptions.Add(new Dropdown.OptionData(Strings.Get(GlobalAssets.Instance.colorSetOptions[i].settingName)));
		}
	}

	// Token: 0x06009C48 RID: 40008 RVA: 0x003D0708 File Offset: 0x003CE908
	private void RefreshColorExamples(int idx)
	{
		Color32 logicOn = GlobalAssets.Instance.colorSetOptions[idx].logicOn;
		Color32 logicOff = GlobalAssets.Instance.colorSetOptions[idx].logicOff;
		Color32 cropHalted = GlobalAssets.Instance.colorSetOptions[idx].cropHalted;
		Color32 cropGrowing = GlobalAssets.Instance.colorSetOptions[idx].cropGrowing;
		Color32 cropGrown = GlobalAssets.Instance.colorSetOptions[idx].cropGrown;
		logicOn.a = byte.MaxValue;
		logicOff.a = byte.MaxValue;
		cropHalted.a = byte.MaxValue;
		cropGrowing.a = byte.MaxValue;
		cropGrown.a = byte.MaxValue;
		this.colorExampleLogicOn.color = logicOn;
		this.colorExampleLogicOff.color = logicOff;
		this.colorExampleCropHalted.color = cropHalted;
		this.colorExampleCropGrowing.color = cropGrowing;
		this.colorExampleCropGrown.color = cropGrown;
	}

	// Token: 0x06009C49 RID: 40009 RVA: 0x003D0804 File Offset: 0x003CEA04
	private int GetResolutionIndex(Resolution resolution)
	{
		int num = -1;
		int result = -1;
		for (int i = 0; i < this.resolutions.Count; i++)
		{
			Resolution resolution2 = this.resolutions[i];
			if (resolution2.width == resolution.width && resolution2.height == resolution.height && resolution2.refreshRateRatio.value == 0.0)
			{
				result = i;
			}
			if (resolution2.width == resolution.width && resolution2.height == resolution.height && Math.Abs(resolution2.refreshRateRatio.value - resolution.refreshRateRatio.value) <= 1.0)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			return num;
		}
		return result;
	}

	// Token: 0x06009C4A RID: 40010 RVA: 0x003D08D8 File Offset: 0x003CEAD8
	private GraphicsOptionsScreen.Settings CaptureSettings()
	{
		return new GraphicsOptionsScreen.Settings
		{
			fullscreen = Screen.fullScreenMode,
			resolution = new Resolution
			{
				width = Screen.width,
				height = Screen.height,
				refreshRateRatio = Screen.currentResolution.refreshRateRatio
			},
			lowRes = QualitySettings.GetQualityLevel(),
			colorSetId = Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, GlobalAssets.Instance.colorSet)
		};
	}

	// Token: 0x06009C4B RID: 40011 RVA: 0x003D0964 File Offset: 0x003CEB64
	private void OnApply()
	{
		try
		{
			GraphicsOptionsScreen.Settings new_settings = default(GraphicsOptionsScreen.Settings);
			new_settings.resolution = this.resolutions[this.resolutionDropdown.value];
			new_settings.fullscreen = ((this.fullscreenToggle.CurrentState == 0) ? FullScreenMode.Windowed : FullScreenMode.MaximizedWindow);
			new_settings.lowRes = this.lowResToggle.CurrentState;
			new_settings.colorSetId = this.colorModeId;
			if (GlobalAssets.Instance.colorSetOptions[this.colorModeId] != GlobalAssets.Instance.colorSet)
			{
				this.colorModeChanged = true;
			}
			this.ApplyConfirmSettings(new_settings, delegate
			{
				this.applyButton.isInteractable = false;
				if (this.colorModeChanged)
				{
					this.feedbackDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, this.transform.parent.gameObject, false).GetComponent<ConfirmDialogScreen>();
					this.feedbackDialog.PopupConfirmDialog(UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK.text, null, null, UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.COLORBLIND_FEEDBACK_BUTTON.text, delegate
					{
						App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/117325-color-blindness-feedback/");
					}, null, null, null, null);
					this.feedbackDialog.gameObject.SetActive(true);
				}
				this.colorModeChanged = false;
				GraphicsOptionsScreen.SaveSettingsToPrefs(new_settings);
			});
		}
		catch (Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Failed to apply graphics options!\nResolutions:");
			foreach (Resolution resolution in this.resolutions)
			{
				stringBuilder.Append("\t" + resolution.ToString() + "\n");
			}
			stringBuilder.Append("Selected Resolution Idx: " + this.resolutionDropdown.value.ToString());
			stringBuilder.Append("FullScreen: " + this.fullscreenToggle.CurrentState.ToString());
			global::Debug.LogError(stringBuilder.ToString());
			throw ex;
		}
	}

	// Token: 0x06009C4C RID: 40012 RVA: 0x000CDCDF File Offset: 0x000CBEDF
	public void OnDone()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06009C4D RID: 40013 RVA: 0x003D0B10 File Offset: 0x003CED10
	private void RefreshApplyButton()
	{
		GraphicsOptionsScreen.Settings settings = this.CaptureSettings();
		if (settings.fullscreen != FullScreenMode.Windowed && this.fullscreenToggle.CurrentState == 0)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (settings.fullscreen == FullScreenMode.Windowed && this.fullscreenToggle.CurrentState == 1)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (settings.lowRes != this.lowResToggle.CurrentState)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		if (settings.colorSetId != this.colorModeId)
		{
			this.applyButton.isInteractable = true;
			return;
		}
		int resolutionIndex = this.GetResolutionIndex(settings.resolution);
		this.applyButton.isInteractable = (this.resolutionDropdown.value != resolutionIndex);
	}

	// Token: 0x06009C4E RID: 40014 RVA: 0x0010A1C0 File Offset: 0x001083C0
	private void OnFullscreenToggle()
	{
		this.fullscreenToggle.ChangeState((this.fullscreenToggle.CurrentState == 0) ? 1 : 0);
		this.RefreshApplyButton();
	}

	// Token: 0x06009C4F RID: 40015 RVA: 0x0010A1E4 File Offset: 0x001083E4
	private void OnResolutionChanged(int idx)
	{
		this.RefreshApplyButton();
	}

	// Token: 0x06009C50 RID: 40016 RVA: 0x0010A1EC File Offset: 0x001083EC
	private void OnColorModeChanged(int idx)
	{
		this.colorModeId = idx;
		this.RefreshApplyButton();
		this.RefreshColorExamples(this.colorModeId);
	}

	// Token: 0x06009C51 RID: 40017 RVA: 0x0010A207 File Offset: 0x00108407
	private void OnLowResToggle()
	{
		this.lowResToggle.ChangeState((this.lowResToggle.CurrentState == 0) ? 1 : 0);
		this.RefreshApplyButton();
	}

	// Token: 0x06009C52 RID: 40018 RVA: 0x003D0BD0 File Offset: 0x003CEDD0
	private void ApplyConfirmSettings(GraphicsOptionsScreen.Settings new_settings, System.Action on_confirm)
	{
		GraphicsOptionsScreen.Settings current_settings = this.CaptureSettings();
		this.ApplySettings(new_settings);
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.parent.gameObject, false).GetComponent<ConfirmDialogScreen>();
		System.Action action = delegate()
		{
			this.ApplySettings(current_settings);
		};
		Coroutine timer = base.StartCoroutine(this.Timer(15f, action));
		this.confirmDialog.onDeactivateCB = delegate()
		{
			if (timer != null)
			{
				this.StopCoroutine(timer);
			}
		};
		this.confirmDialog.PopupConfirmDialog(this.colorModeChanged ? UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES_STRING_COLOR.text : UI.FRONTEND.GRAPHICS_OPTIONS_SCREEN.ACCEPT_CHANGES.text, on_confirm, action, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x06009C53 RID: 40019 RVA: 0x003D0CA8 File Offset: 0x003CEEA8
	private void ApplySettings(GraphicsOptionsScreen.Settings new_settings)
	{
		Resolution resolution = new_settings.resolution;
		Screen.SetResolution(resolution.width, resolution.height, new_settings.fullscreen, resolution.refreshRateRatio);
		Screen.fullScreenMode = new_settings.fullscreen;
		int resolutionIndex = this.GetResolutionIndex(new_settings.resolution);
		if (resolutionIndex != -1)
		{
			this.resolutionDropdown.value = resolutionIndex;
		}
		GlobalAssets.Instance.colorSet = GlobalAssets.Instance.colorSetOptions[new_settings.colorSetId];
		global::Debug.Log("Applying low res settings " + new_settings.lowRes.ToString() + " / existing is " + QualitySettings.GetQualityLevel().ToString());
		if (QualitySettings.GetQualityLevel() != new_settings.lowRes)
		{
			QualitySettings.SetQualityLevel(new_settings.lowRes, true);
		}
	}

	// Token: 0x06009C54 RID: 40020 RVA: 0x0010A22B File Offset: 0x0010842B
	private IEnumerator Timer(float time, System.Action revert)
	{
		yield return new WaitForSecondsRealtime(time);
		if (this.confirmDialog != null)
		{
			this.confirmDialog.Deactivate();
			revert();
		}
		yield break;
	}

	// Token: 0x06009C55 RID: 40021 RVA: 0x0010A248 File Offset: 0x00108448
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04007A34 RID: 31284
	[SerializeField]
	private Dropdown resolutionDropdown;

	// Token: 0x04007A35 RID: 31285
	[SerializeField]
	private MultiToggle lowResToggle;

	// Token: 0x04007A36 RID: 31286
	[SerializeField]
	private MultiToggle fullscreenToggle;

	// Token: 0x04007A37 RID: 31287
	[SerializeField]
	private KButton applyButton;

	// Token: 0x04007A38 RID: 31288
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04007A39 RID: 31289
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007A3A RID: 31290
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04007A3B RID: 31291
	[SerializeField]
	private ConfirmDialogScreen feedbackPrefab;

	// Token: 0x04007A3C RID: 31292
	[SerializeField]
	private KSlider uiScaleSlider;

	// Token: 0x04007A3D RID: 31293
	[SerializeField]
	private LocText sliderLabel;

	// Token: 0x04007A3E RID: 31294
	[SerializeField]
	private LocText title;

	// Token: 0x04007A3F RID: 31295
	[SerializeField]
	private Dropdown colorModeDropdown;

	// Token: 0x04007A40 RID: 31296
	[SerializeField]
	private KImage colorExampleLogicOn;

	// Token: 0x04007A41 RID: 31297
	[SerializeField]
	private KImage colorExampleLogicOff;

	// Token: 0x04007A42 RID: 31298
	[SerializeField]
	private KImage colorExampleCropHalted;

	// Token: 0x04007A43 RID: 31299
	[SerializeField]
	private KImage colorExampleCropGrowing;

	// Token: 0x04007A44 RID: 31300
	[SerializeField]
	private KImage colorExampleCropGrown;

	// Token: 0x04007A45 RID: 31301
	public static readonly string ResolutionWidthKey = "ResolutionWidth";

	// Token: 0x04007A46 RID: 31302
	public static readonly string ResolutionHeightKey = "ResolutionHeight";

	// Token: 0x04007A47 RID: 31303
	public static readonly string RefreshRateKeyNumerator = "RefreshRateNumerator";

	// Token: 0x04007A48 RID: 31304
	public static readonly string RefreshRateKeyDenominator = "RefreshRateNumerator";

	// Token: 0x04007A49 RID: 31305
	public static readonly string FullScreenKey = "FullScreen";

	// Token: 0x04007A4A RID: 31306
	public static readonly string LowResKey = "LowResTextures";

	// Token: 0x04007A4B RID: 31307
	public static readonly string ColorModeKey = "ColorModeID";

	// Token: 0x04007A4C RID: 31308
	private const FullScreenMode FULLSCREEN = FullScreenMode.MaximizedWindow;

	// Token: 0x04007A4D RID: 31309
	private const FullScreenMode WINDOWED = FullScreenMode.Windowed;

	// Token: 0x04007A4E RID: 31310
	private KCanvasScaler[] CanvasScalers;

	// Token: 0x04007A4F RID: 31311
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x04007A50 RID: 31312
	private ConfirmDialogScreen feedbackDialog;

	// Token: 0x04007A51 RID: 31313
	private List<Resolution> resolutions = new List<Resolution>();

	// Token: 0x04007A52 RID: 31314
	private List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

	// Token: 0x04007A53 RID: 31315
	private List<Dropdown.OptionData> colorModeOptions = new List<Dropdown.OptionData>();

	// Token: 0x04007A54 RID: 31316
	private int colorModeId;

	// Token: 0x04007A55 RID: 31317
	private bool colorModeChanged;

	// Token: 0x04007A56 RID: 31318
	private GraphicsOptionsScreen.Settings originalSettings;

	// Token: 0x02001D3A RID: 7482
	private struct Settings
	{
		// Token: 0x04007A57 RID: 31319
		public FullScreenMode fullscreen;

		// Token: 0x04007A58 RID: 31320
		public Resolution resolution;

		// Token: 0x04007A59 RID: 31321
		public int lowRes;

		// Token: 0x04007A5A RID: 31322
		public int colorSetId;
	}
}
