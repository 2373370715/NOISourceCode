using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02001239 RID: 4665
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x000E24DC File Offset: 0x000E06DC
	public static CustomGameSettings Instance
	{
		get
		{
			return CustomGameSettings.instance;
		}
	}

	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x06005EBA RID: 24250 RVA: 0x000E24E3 File Offset: 0x000E06E3
	public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
	{
		get
		{
			return this.currentStoryLevelsBySetting;
		}
	}

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06005EBB RID: 24251 RVA: 0x002B0AC8 File Offset: 0x002AECC8
	// (remove) Token: 0x06005EBC RID: 24252 RVA: 0x002B0B00 File Offset: 0x002AED00
	public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06005EBD RID: 24253 RVA: 0x002B0B38 File Offset: 0x002AED38
	// (remove) Token: 0x06005EBE RID: 24254 RVA: 0x002B0B70 File Offset: 0x002AED70
	public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06005EBF RID: 24255 RVA: 0x002B0BA8 File Offset: 0x002AEDA8
	// (remove) Token: 0x06005EC0 RID: 24256 RVA: 0x002B0BE0 File Offset: 0x002AEDE0
	public event Action<SettingConfig, SettingLevel> OnMixingSettingChanged;

	// Token: 0x06005EC1 RID: 24257 RVA: 0x002B0C18 File Offset: 0x002AEE18
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
		{
			this.customGameMode = (this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival);
		}
		if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
		{
			if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
			{
				this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
			}
			this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
		}
		this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
		string clusterDefaultName;
		this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
		if (clusterDefaultName.IsNullOrWhiteSpace())
		{
			if (!DlcManager.IsExpansion1Active())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead"
				});
			}
			clusterDefaultName = WorldGenSettings.ClusterDefaultName;
			this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
		}
		if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
		{
			global::Debug.Log("Deserializing CustomGameSettings.ClusterLayout: '" + clusterDefaultName + "' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
			string text = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '",
					clusterDefaultName,
					"' to '",
					text,
					"'"
				}));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, text);
			}
			else
			{
				global::Debug.LogWarning("Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '" + clusterDefaultName + "' including the scoped path, setting to default cluster name.");
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
			}
		}
		this.CheckCustomGameMode();
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x002B0DEC File Offset: 0x002AEFEC
	private void AddMissingQualitySettings()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			SettingConfig value = keyValuePair.Value;
			if (Game.IsCorrectDlcActiveForCurrentSave(value) && !this.CurrentQualityLevelsBySetting.ContainsKey(value.id))
			{
				if (value.missing_content_default != "")
				{
					DebugUtil.LogArgs(new object[]
					{
						string.Concat(new string[]
						{
							"QualitySetting '",
							value.id,
							"' is missing, setting it to missing_content_default '",
							value.missing_content_default,
							"'."
						})
					});
					this.SetQualitySetting(value, value.missing_content_default);
				}
				else
				{
					DebugUtil.DevLogError("QualitySetting '" + value.id + "' is missing in this save. Either provide a missing_content_default or handle it in OnDeserialized.");
				}
			}
		}
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x002B0EE4 File Offset: 0x002AF0E4
	protected override void OnPrefabInit()
	{
		DlcManager.IsExpansion1Active();
		Action<SettingConfig> action = delegate(SettingConfig setting)
		{
			this.AddQualitySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedQualitySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action2 = delegate(SettingConfig setting)
		{
			this.AddStorySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedStorySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action3 = delegate(SettingConfig setting)
		{
			this.AddMixingSettingsConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedMixingSettings.Add(setting.id);
			}
		};
		CustomGameSettings.instance = this;
		action(CustomGameSettingConfigs.ClusterLayout);
		action(CustomGameSettingConfigs.WorldgenSeed);
		action(CustomGameSettingConfigs.ImmuneSystem);
		action(CustomGameSettingConfigs.CalorieBurn);
		action(CustomGameSettingConfigs.Morale);
		action(CustomGameSettingConfigs.Durability);
		action(CustomGameSettingConfigs.MeteorShowers);
		action(CustomGameSettingConfigs.Radiation);
		action(CustomGameSettingConfigs.Stress);
		action(CustomGameSettingConfigs.StressBreaks);
		action(CustomGameSettingConfigs.CarePackages);
		action(CustomGameSettingConfigs.SandboxMode);
		action(CustomGameSettingConfigs.FastWorkersMode);
		action(CustomGameSettingConfigs.SaveToCloud);
		action(CustomGameSettingConfigs.Teleporters);
		action(CustomGameSettingConfigs.BionicWattage);
		action3(CustomMixingSettingsConfigs.DLC2Mixing);
		action3(CustomMixingSettingsConfigs.IceCavesMixing);
		action3(CustomMixingSettingsConfigs.CarrotQuarryMixing);
		action3(CustomMixingSettingsConfigs.SugarWoodsMixing);
		action3(CustomMixingSettingsConfigs.CeresAsteroidMixing);
		action3(CustomMixingSettingsConfigs.DLC3Mixing);
		foreach (Story story in Db.Get().Stories.GetStoriesSortedByCoordinateOrder())
		{
			int num = (story.kleiUseOnlyCoordinateOrder == -1) ? -1 : 3;
			SettingConfig obj = new ListSettingConfig(story.Id, "", "", new List<SettingLevel>
			{
				new SettingLevel("Disabled", "", "", 0L, null),
				new SettingLevel("Guaranteed", "", "", 1L, null)
			}, "Disabled", "Disabled", (long)num, false, false, null, "", false);
			action2(obj);
		}
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && DlcManager.IsContentSubscribed(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(dlcMixingSettingConfig, "Enabled");
			}
		}
		this.VerifySettingCoordinates();
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x002B1154 File Offset: 0x002AF354
	public void DisableAllStories()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.StorySettings)
		{
			this.SetStorySetting(keyValuePair.Value, false);
		}
	}

	// Token: 0x06005EC5 RID: 24261 RVA: 0x002B11B0 File Offset: 0x002AF3B0
	public void SetSurvivalDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetDefaultLevelId());
		}
	}

	// Token: 0x06005EC6 RID: 24262 RVA: 0x002B121C File Offset: 0x002AF41C
	public void SetNosweatDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetNoSweatDefaultLevelId());
		}
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x000E24EB File Offset: 0x000E06EB
	public SettingLevel CycleQualitySettingLevel(ListSettingConfig config, int direction)
	{
		this.SetQualitySetting(config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06005EC8 RID: 24264 RVA: 0x000E2523 File Offset: 0x000E0723
	public SettingLevel ToggleQualitySettingLevel(ToggleSettingConfig config)
	{
		this.SetQualitySetting(config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06005EC9 RID: 24265 RVA: 0x002B1288 File Offset: 0x002AF488
	private void CheckCustomGameMode()
	{
		bool flag = true;
		bool flag2 = true;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Quality settings missing " + keyValuePair.Key
				});
			}
			else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
			{
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
				{
					flag = false;
				}
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
				{
					flag2 = false;
				}
				if (!flag && !flag2)
				{
					break;
				}
			}
		}
		CustomGameSettings.CustomGameMode customGameMode;
		if (flag)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Survival;
		}
		else if (flag2)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		}
		else
		{
			customGameMode = CustomGameSettings.CustomGameMode.Custom;
		}
		if (customGameMode != this.customGameMode)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Game mode changed from",
				this.customGameMode,
				"to",
				customGameMode
			});
			this.customGameMode = customGameMode;
		}
	}

	// Token: 0x06005ECA RID: 24266 RVA: 0x000E255A File Offset: 0x000E075A
	public void SetQualitySetting(SettingConfig config, string value)
	{
		this.SetQualitySetting(config, value, true);
	}

	// Token: 0x06005ECB RID: 24267 RVA: 0x000E2565 File Offset: 0x000E0765
	public void SetQualitySetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentQualityLevelsBySetting[config.id] = value;
		this.CheckCustomGameMode();
		if (notify && this.OnQualitySettingChanged != null)
		{
			this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
		}
	}

	// Token: 0x06005ECC RID: 24268 RVA: 0x000E259D File Offset: 0x000E079D
	public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
	{
		return this.GetCurrentQualitySetting(setting.id);
	}

	// Token: 0x06005ECD RID: 24269 RVA: 0x002B13DC File Offset: 0x002AF5DC
	public SettingLevel GetCurrentQualitySetting(string setting_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
		{
			this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : settingConfig.GetDefaultLevelId();
		return this.QualitySettings[setting_id].GetLevel(level_id);
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x000E25AB File Offset: 0x000E07AB
	public string GetCurrentQualitySettingLevelId(SettingConfig config)
	{
		return this.CurrentQualityLevelsBySetting[config.id];
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x002B1490 File Offset: 0x002AF690
	public string GetSettingLevelLabel(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.label;
			}
		}
		global::Debug.LogWarning("No label string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x002B14DC File Offset: 0x002AF6DC
	public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.tooltip;
			}
		}
		global::Debug.LogWarning("No tooltip string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x002B1528 File Offset: 0x002AF728
	public void AddQualitySettingConfig(SettingConfig config)
	{
		this.QualitySettings.Add(config.id, config);
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
		{
			this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x002B158C File Offset: 0x002AF78C
	public void AddStorySettingConfig(SettingConfig config)
	{
		this.StorySettings.Add(config.id, config);
		if (!this.currentStoryLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
		{
			this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x000E25BE File Offset: 0x000E07BE
	public void SetStorySetting(SettingConfig config, string value)
	{
		this.SetStorySetting(config, value == "Guaranteed");
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x000E25D2 File Offset: 0x000E07D2
	public void SetStorySetting(SettingConfig config, bool value)
	{
		this.currentStoryLevelsBySetting[config.id] = (value ? "Guaranteed" : "Disabled");
		if (this.OnStorySettingChanged != null)
		{
			this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
		}
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x002B15F0 File Offset: 0x002AF7F0
	public void ParseAndApplyStoryTraitSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedStorySettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.StorySettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetStorySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x002B174C File Offset: 0x002AF94C
	private string GetStoryTraitSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedStorySettings)
		{
			SettingConfig settingConfig = this.StorySettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.currentStoryLevelsBySetting[key]).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x000E260F File Offset: 0x000E080F
	public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
	{
		return this.GetCurrentStoryTraitSetting(setting.id);
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x002B17E8 File Offset: 0x002AF9E8
	public SettingLevel GetCurrentStoryTraitSetting(string settingId)
	{
		SettingConfig settingConfig = this.StorySettings[settingId];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
		{
			this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.currentStoryLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.StorySettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x002B189C File Offset: 0x002AFA9C
	public List<string> GetCurrentStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x002B1914 File Offset: 0x002AFB14
	public bool IsStoryActive(string id, string level)
	{
		SettingConfig settingConfig;
		return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
	}

	// Token: 0x06005EDB RID: 24283 RVA: 0x000E261D File Offset: 0x000E081D
	public void SetMixingSetting(SettingConfig config, string value)
	{
		this.SetMixingSetting(config, value, true);
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x000E2628 File Offset: 0x000E0828
	public void SetMixingSetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentMixingLevelsBySetting[config.id] = value;
		if (notify && this.OnMixingSettingChanged != null)
		{
			this.OnMixingSettingChanged(config, this.GetCurrentMixingSettingLevel(config));
		}
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x002B1944 File Offset: 0x002AFB44
	public void AddMixingSettingsConfig(SettingConfig config)
	{
		this.MixingSettings.Add(config.id, config);
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentMixingLevelsBySetting[config.id]))
		{
			this.CurrentMixingLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x000E265A File Offset: 0x000E085A
	public SettingLevel GetCurrentMixingSettingLevel(SettingConfig setting)
	{
		return this.GetCurrentMixingSettingLevel(setting.id);
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x002B19A8 File Offset: 0x002AFBA8
	public SettingConfig GetWorldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && worldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x002B1A20 File Offset: 0x002AFC20
	public SettingConfig GetSubworldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && subworldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x002B1A98 File Offset: 0x002AFC98
	public void DisableAllMixing()
	{
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			this.SetMixingSetting(settingConfig, settingConfig.GetDefaultLevelId());
		}
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x002B1AF8 File Offset: 0x002AFCF8
	public List<SubworldMixingSettingConfig> GetActiveSubworldMixingSettings()
	{
		List<SubworldMixingSettingConfig> list = new List<SubworldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = settingConfig as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(subworldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x002B1B7C File Offset: 0x002AFD7C
	public List<WorldMixingSettingConfig> GetActiveWorldMixingSettings()
	{
		List<WorldMixingSettingConfig> list = new List<WorldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = settingConfig as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(worldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x000E2668 File Offset: 0x000E0868
	public SettingLevel CycleMixingSettingLevel(ListSettingConfig config, int direction)
	{
		this.SetMixingSetting(config, config.CycleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06005EE5 RID: 24293 RVA: 0x000E26A0 File Offset: 0x000E08A0
	public SettingLevel ToggleMixingSettingLevel(ToggleSettingConfig config)
	{
		this.SetMixingSetting(config, config.ToggleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x002B1C00 File Offset: 0x002AFE00
	public SettingLevel GetCurrentMixingSettingLevel(string settingId)
	{
		SettingConfig settingConfig = this.MixingSettings[settingId];
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(settingId))
		{
			this.CurrentMixingLevelsBySetting[settingId] = this.MixingSettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentMixingLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.MixingSettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x002B1C7C File Offset: 0x002AFE7C
	public List<string> GetCurrentDlcMixingIds()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && dlcMixingSettingConfig.IsOnLevel(this.GetCurrentMixingSettingLevel(dlcMixingSettingConfig.id).id))
			{
				list.Add(dlcMixingSettingConfig.id);
			}
		}
		return list;
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x002B1D04 File Offset: 0x002AFF04
	public void ParseAndApplyMixingSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedMixingSettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.MixingSettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetMixingSetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x002B1E60 File Offset: 0x002B0060
	private string GetMixingSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedMixingSettings)
		{
			SettingConfig settingConfig = this.MixingSettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentMixingSettingLevel(settingConfig).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x002B1EFC File Offset: 0x002B00FC
	public void RemoveInvalidMixingSettings()
	{
		ClusterLayout currentClusterLayout = this.GetCurrentClusterLayout();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && currentClusterLayout.requiredDlcIds.Contains(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(keyValuePair.Value, "Disabled");
			}
		}
		CustomGameSettings.<>c__DisplayClass71_0 CS$<>8__locals1;
		CS$<>8__locals1.availableDlcs = this.GetCurrentDlcMixingIds();
		CS$<>8__locals1.availableDlcs.AddRange(currentClusterLayout.requiredDlcIds);
		foreach (KeyValuePair<string, SettingConfig> keyValuePair2 in this.MixingSettings)
		{
			SettingConfig value = keyValuePair2.Value;
			WorldMixingSettingConfig worldMixingSettingConfig = value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig == null)
			{
				SubworldMixingSettingConfig subworldMixingSettingConfig = value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null)
				{
					if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(subworldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(subworldMixingSettingConfig.forbiddenClusterTags))
					{
						this.SetMixingSetting(keyValuePair2.Value, "Disabled");
					}
				}
			}
			else if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(worldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(worldMixingSettingConfig.forbiddenClusterTags))
			{
				this.SetMixingSetting(keyValuePair2.Value, "Disabled");
			}
		}
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x002B2070 File Offset: 0x002B0270
	public ClusterLayout GetCurrentClusterLayout()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			return null;
		}
		return SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x002B20A4 File Offset: 0x002B02A4
	public int GetCurrentWorldgenSeed()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		if (currentQualitySetting == null)
		{
			return 0;
		}
		return int.Parse(currentQualitySetting.id);
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x002B20D4 File Offset: 0x002B02D4
	public void LoadClusters()
	{
		Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
		List<SettingLevel> list = new List<SettingLevel>(clusterCache.Count);
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
		{
			StringEntry stringEntry;
			string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.name;
			string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.description;
			list.Add(new SettingLevel(keyValuePair.Key, label, tooltip, 0L, null));
		}
		CustomGameSettingConfigs.ClusterLayout.StompLevels(list, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
	}

	// Token: 0x06005EEE RID: 24302 RVA: 0x002B21C4 File Offset: 0x002B03C4
	public void Print()
	{
		string text = "Custom Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				"=",
				keyValuePair.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Story Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair2 in this.currentStoryLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair2.Key,
				"=",
				keyValuePair2.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Mixing Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair3 in this.CurrentMixingLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair3.Key,
				"=",
				keyValuePair3.Value,
				","
			});
		}
		global::Debug.Log(text);
	}

	// Token: 0x06005EEF RID: 24303 RVA: 0x002B2348 File Offset: 0x002B0548
	private bool AllValuesMatch(Dictionary<string, string> data, CustomGameSettings.CustomGameMode mode)
	{
		bool result = true;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			if (!(keyValuePair.Key == CustomGameSettingConfigs.WorldgenSeed.id))
			{
				string b = null;
				if (mode != CustomGameSettings.CustomGameMode.Survival)
				{
					if (mode == CustomGameSettings.CustomGameMode.Nosweat)
					{
						b = keyValuePair.Value.GetNoSweatDefaultLevelId();
					}
				}
				else
				{
					b = keyValuePair.Value.GetDefaultLevelId();
				}
				if (data.ContainsKey(keyValuePair.Key) && data[keyValuePair.Key] != b)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06005EF0 RID: 24304 RVA: 0x002B23FC File Offset: 0x002B05FC
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		list.Add(new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameMode",
			Value = this.customGameMode.ToString()
		});
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			list.Add(new CustomGameSettings.MetricSettingsData
			{
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
		}
		CustomGameSettings.MetricSettingsData item = new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameModeActual",
			Value = CustomGameSettings.CustomGameMode.Custom.ToString()
		};
		foreach (object obj in Enum.GetValues(typeof(CustomGameSettings.CustomGameMode)))
		{
			CustomGameSettings.CustomGameMode customGameMode = (CustomGameSettings.CustomGameMode)obj;
			if (customGameMode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, customGameMode))
			{
				item.Value = customGameMode.ToString();
				break;
			}
		}
		list.Add(item);
		return list;
	}

	// Token: 0x06005EF1 RID: 24305 RVA: 0x002B2568 File Offset: 0x002B0768
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMixingMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
		{
			if (DlcManager.IsAllContentSubscribed(this.MixingSettings[keyValuePair.Key].required_content))
			{
				list.Add(new CustomGameSettings.MetricSettingsData
				{
					Name = keyValuePair.Key,
					Value = keyValuePair.Value
				});
			}
		}
		return list;
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x002B2604 File Offset: 0x002B0804
	public bool VerifySettingCoordinates()
	{
		bool flag = this.VerifySettingsDictionary(this.QualitySettings);
		bool flag2 = this.VerifySettingsDictionary(this.StorySettings);
		return flag || flag2;
	}

	// Token: 0x06005EF3 RID: 24307 RVA: 0x002B262C File Offset: 0x002B082C
	private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
	{
		bool result = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in configs)
		{
			if (keyValuePair.Value.coordinate_range >= 0L)
			{
				List<SettingLevel> levels = keyValuePair.Value.GetLevels();
				if (keyValuePair.Value.coordinate_range < (long)levels.Count)
				{
					result = true;
					global::Debug.Assert(false, string.Concat(new string[]
					{
						keyValuePair.Value.id,
						": Range between coordinate min and max insufficient for all levels (",
						keyValuePair.Value.coordinate_range.ToString(),
						"<",
						levels.Count.ToString(),
						")"
					}));
				}
				foreach (SettingLevel settingLevel in levels)
				{
					Dictionary<long, string> dictionary = new Dictionary<long, string>();
					string text = keyValuePair.Value.id + " > " + settingLevel.id;
					if (keyValuePair.Value.coordinate_range <= settingLevel.coordinate_value)
					{
						result = true;
						global::Debug.Assert(false, string.Format("%s: Level coordinate value (%u) exceedes range (%u)", text, settingLevel.coordinate_value, keyValuePair.Value.coordinate_range));
					}
					if (settingLevel.coordinate_value < 0L)
					{
						result = true;
						global::Debug.Assert(false, text + ": Level coordinate value must be >= 0");
					}
					else if (settingLevel.coordinate_value == 0L)
					{
						if (settingLevel.id != keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Only the default level should have a coordinate value of 0");
						}
					}
					else
					{
						string str;
						bool flag = !dictionary.TryGetValue(settingLevel.coordinate_value, out str);
						dictionary[settingLevel.coordinate_value] = text;
						if (settingLevel.id == keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Default level must be a coordinate value of 0");
						}
						if (!flag)
						{
							result = true;
							global::Debug.Assert(false, text + ": Combined coordinate conflicts with another coordinate (" + str + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x002B28AC File Offset: 0x002B0AAC
	public static string[] ParseSettingCoordinate(string coord)
	{
		Match match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)").Match(coord);
		for (int i = 1; i <= 2; i++)
		{
			if (match.Groups.Count == 1)
			{
				match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)".Remove("(.*)-(\\d*)-(.*)-(.*)-(.*)".Length - i * 5)).Match(coord);
			}
		}
		string[] array = new string[match.Groups.Count];
		for (int j = 0; j < match.Groups.Count; j++)
		{
			array[j] = match.Groups[j].Value;
		}
		return array;
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x002B2944 File Offset: 0x002B0B44
	public string GetSettingsCoordinate()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			return "0-0-0-0-0";
		}
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		string otherSettingsCode = this.GetOtherSettingsCode();
		string storyTraitSettingsCode = this.GetStoryTraitSettingsCode();
		string mixingSettingsCode = this.GetMixingSettingsCode();
		return string.Format("{0}-{1}-{2}-{3}-{4}", new object[]
		{
			clusterData.GetCoordinatePrefix(),
			currentQualitySetting2.id,
			otherSettingsCode,
			storyTraitSettingsCode,
			mixingSettingsCode
		});
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x002B2A10 File Offset: 0x002B0C10
	public void ParseAndApplySettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedQualitySettings))
		{
			string key = (string)obj;
			if (this.QualitySettings.ContainsKey(key))
			{
				SettingConfig settingConfig = this.QualitySettings[key];
				if (settingConfig.coordinate_range != -1L)
				{
					long num = (long)(dividend % settingConfig.coordinate_range);
					dividend /= settingConfig.coordinate_range;
					foreach (SettingLevel settingLevel in settingConfig.GetLevels())
					{
						if (settingLevel.coordinate_value == num)
						{
							dictionary[settingConfig] = settingLevel.id;
							break;
						}
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x002B2B7C File Offset: 0x002B0D7C
	private string GetOtherSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string text in this.CoordinatedQualitySettings)
		{
			SettingConfig settingConfig = this.QualitySettings[text];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentQualitySetting(text).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x002B2C18 File Offset: 0x002B0E18
	private BigInteger Base36toBinary(string input)
	{
		if (input == "0")
		{
			return 0;
		}
		BigInteger bigInteger = 0;
		for (int i = input.Length - 1; i >= 0; i--)
		{
			bigInteger *= 36;
			long value = (long)this.hexChars.IndexOf(input[i]);
			bigInteger += value;
		}
		DebugUtil.LogArgs(new object[]
		{
			"tried converting",
			input,
			", got",
			bigInteger,
			"and returns to",
			this.BinarytoBase36(bigInteger)
		});
		return bigInteger;
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x002B2CC0 File Offset: 0x002B0EC0
	private string BinarytoBase36(BigInteger input)
	{
		if (input == 0L)
		{
			return "0";
		}
		BigInteger bigInteger = input;
		string text = "";
		while (bigInteger > 0L)
		{
			text += this.hexChars[(int)(bigInteger % 36)].ToString();
			bigInteger /= 36;
		}
		return text;
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x002B2DB0 File Offset: 0x002B0FB0
	[CompilerGenerated]
	internal static bool <RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(string[] requiredContent, ref CustomGameSettings.<>c__DisplayClass71_0 A_1)
	{
		foreach (string text in requiredContent)
		{
			if (!(text == "") && !A_1.availableDlcs.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040043B5 RID: 17333
	private static CustomGameSettings instance;

	// Token: 0x040043B6 RID: 17334
	public const long NO_COORDINATE_RANGE = -1L;

	// Token: 0x040043B7 RID: 17335
	private const int NUM_STORY_LEVELS = 3;

	// Token: 0x040043B8 RID: 17336
	public const string STORY_DISABLED_LEVEL = "Disabled";

	// Token: 0x040043B9 RID: 17337
	public const string STORY_GUARANTEED_LEVEL = "Guaranteed";

	// Token: 0x040043BA RID: 17338
	[Serialize]
	public bool is_custom_game;

	// Token: 0x040043BB RID: 17339
	[Serialize]
	public CustomGameSettings.CustomGameMode customGameMode;

	// Token: 0x040043BC RID: 17340
	[Serialize]
	private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040043BD RID: 17341
	[Serialize]
	private Dictionary<string, string> CurrentMixingLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040043BE RID: 17342
	private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x040043BF RID: 17343
	public List<string> CoordinatedQualitySettings = new List<string>();

	// Token: 0x040043C0 RID: 17344
	public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040043C1 RID: 17345
	public List<string> CoordinatedStorySettings = new List<string>();

	// Token: 0x040043C2 RID: 17346
	public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040043C3 RID: 17347
	public List<string> CoordinatedMixingSettings = new List<string>();

	// Token: 0x040043C4 RID: 17348
	public Dictionary<string, SettingConfig> MixingSettings = new Dictionary<string, SettingConfig>();

	// Token: 0x040043C8 RID: 17352
	private const string coordinatePatern = "(.*)-(\\d*)-(.*)-(.*)-(.*)";

	// Token: 0x040043C9 RID: 17353
	private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Token: 0x0200123A RID: 4666
	public enum CustomGameMode
	{
		// Token: 0x040043CB RID: 17355
		Survival,
		// Token: 0x040043CC RID: 17356
		Nosweat,
		// Token: 0x040043CD RID: 17357
		Custom = 255
	}

	// Token: 0x0200123B RID: 4667
	public struct MetricSettingsData
	{
		// Token: 0x040043CE RID: 17358
		public string Name;

		// Token: 0x040043CF RID: 17359
		public string Value;
	}
}
