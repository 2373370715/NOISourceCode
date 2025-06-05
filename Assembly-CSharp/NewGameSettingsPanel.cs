using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001EA8 RID: 7848
[AddComponentMenu("KMonoBehaviour/scripts/NewGameSettingsPanel")]
public class NewGameSettingsPanel : CustomGameSettingsPanelBase
{
	// Token: 0x0600A48D RID: 42125 RVA: 0x0010F2DA File Offset: 0x0010D4DA
	public void SetCloseAction(System.Action onClose)
	{
		if (this.closeButton != null)
		{
			this.closeButton.onClick += onClose;
		}
		if (this.background != null)
		{
			this.background.onClick += onClose;
		}
	}

	// Token: 0x0600A48E RID: 42126 RVA: 0x003F64C4 File Offset: 0x003F46C4
	public override void Init()
	{
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		this.settings = CustomGameSettings.Instance;
		this.widgets = new List<CustomGameSettingWidget>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.settings.QualitySettings)
		{
			if (keyValuePair.Value.ShowInUI())
			{
				ListSettingConfig listSettingConfig = keyValuePair.Value as ListSettingConfig;
				if (listSettingConfig != null)
				{
					CustomGameSettingListWidget customGameSettingListWidget = Util.KInstantiateUI<CustomGameSettingListWidget>(this.prefab_cycle_setting, this.content.gameObject, false);
					customGameSettingListWidget.Initialize(listSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ListSettingConfig, int, SettingLevel>(CustomGameSettings.Instance.CycleQualitySettingLevel));
					customGameSettingListWidget.gameObject.SetActive(true);
					base.AddWidget(customGameSettingListWidget);
				}
				else
				{
					ToggleSettingConfig toggleSettingConfig = keyValuePair.Value as ToggleSettingConfig;
					if (toggleSettingConfig != null)
					{
						CustomGameSettingToggleWidget customGameSettingToggleWidget = Util.KInstantiateUI<CustomGameSettingToggleWidget>(this.prefab_checkbox_setting, this.content.gameObject, false);
						customGameSettingToggleWidget.Initialize(toggleSettingConfig, new Func<SettingConfig, SettingLevel>(CustomGameSettings.Instance.GetCurrentQualitySetting), new Func<ToggleSettingConfig, SettingLevel>(CustomGameSettings.Instance.ToggleQualitySettingLevel));
						customGameSettingToggleWidget.gameObject.SetActive(true);
						base.AddWidget(customGameSettingToggleWidget);
					}
					else
					{
						SeedSettingConfig seedSettingConfig = keyValuePair.Value as SeedSettingConfig;
						if (seedSettingConfig != null)
						{
							CustomGameSettingSeed customGameSettingSeed = Util.KInstantiateUI<CustomGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, false);
							customGameSettingSeed.Initialize(seedSettingConfig);
							customGameSettingSeed.gameObject.SetActive(true);
							base.AddWidget(customGameSettingSeed);
						}
					}
				}
			}
		}
		this.Refresh();
	}

	// Token: 0x0600A48F RID: 42127 RVA: 0x0010F310 File Offset: 0x0010D510
	public void ConsumeSettingsCode(string code)
	{
		this.settings.ParseAndApplySettingsCode(code);
	}

	// Token: 0x0600A490 RID: 42128 RVA: 0x0010F31E File Offset: 0x0010D51E
	public void ConsumeStoryTraitsCode(string code)
	{
		this.settings.ParseAndApplyStoryTraitSettingsCode(code);
	}

	// Token: 0x0600A491 RID: 42129 RVA: 0x0010F32C File Offset: 0x0010D52C
	public void ConsumeMixingSettingsCode(string code)
	{
		this.settings.ParseAndApplyMixingSettingsCode(code);
	}

	// Token: 0x0600A492 RID: 42130 RVA: 0x0010F33A File Offset: 0x0010D53A
	public void SetSetting(SettingConfig setting, string level, bool notify = true)
	{
		this.settings.SetQualitySetting(setting, level, notify);
	}

	// Token: 0x0600A493 RID: 42131 RVA: 0x0010F34A File Offset: 0x0010D54A
	public string GetSetting(SettingConfig setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x0600A494 RID: 42132 RVA: 0x0010F35D File Offset: 0x0010D55D
	public string GetSetting(string setting)
	{
		return this.settings.GetCurrentQualitySetting(setting).id;
	}

	// Token: 0x0600A495 RID: 42133 RVA: 0x000AA038 File Offset: 0x000A8238
	public void Cancel()
	{
	}

	// Token: 0x040080AB RID: 32939
	[SerializeField]
	private Transform content;

	// Token: 0x040080AC RID: 32940
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040080AD RID: 32941
	[SerializeField]
	private KButton background;

	// Token: 0x040080AE RID: 32942
	[Header("Prefab UI Refs")]
	[SerializeField]
	private GameObject prefab_cycle_setting;

	// Token: 0x040080AF RID: 32943
	[SerializeField]
	private GameObject prefab_slider_setting;

	// Token: 0x040080B0 RID: 32944
	[SerializeField]
	private GameObject prefab_checkbox_setting;

	// Token: 0x040080B1 RID: 32945
	[SerializeField]
	private GameObject prefab_seed_input_setting;

	// Token: 0x040080B2 RID: 32946
	private CustomGameSettings settings;
}
