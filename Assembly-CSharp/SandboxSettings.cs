using System;
using System.Collections.Generic;

// Token: 0x02001F46 RID: 8006
public class SandboxSettings
{
	// Token: 0x0600A8CA RID: 43210 RVA: 0x00111E40 File Offset: 0x00110040
	public void AddIntSetting(string prefsKey, Action<int> setAction, int defaultValue)
	{
		this.intSettings.Add(new SandboxSettings.Setting<int>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A8CB RID: 43211 RVA: 0x00111E55 File Offset: 0x00110055
	public int GetIntSetting(string prefsKey)
	{
		return KPlayerPrefs.GetInt(prefsKey);
	}

	// Token: 0x0600A8CC RID: 43212 RVA: 0x0040E0C4 File Offset: 0x0040C2C4
	public void SetIntSetting(string prefsKey, int value)
	{
		SandboxSettings.Setting<int> setting = this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No intSetting named: ",
				prefsKey,
				" could be found amongst ",
				this.intSettings.Count.ToString(),
				" int settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A8CD RID: 43213 RVA: 0x00111E5D File Offset: 0x0011005D
	public void RestoreIntSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetIntSetting(prefsKey, this.GetIntSetting(prefsKey));
			return;
		}
		this.ForceDefaultIntSetting(prefsKey);
	}

	// Token: 0x0600A8CE RID: 43214 RVA: 0x0040E148 File Offset: 0x0040C348
	public void ForceDefaultIntSetting(string prefsKey)
	{
		this.SetIntSetting(prefsKey, this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A8CF RID: 43215 RVA: 0x00111E7D File Offset: 0x0011007D
	public void AddFloatSetting(string prefsKey, Action<float> setAction, float defaultValue)
	{
		this.floatSettings.Add(new SandboxSettings.Setting<float>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A8D0 RID: 43216 RVA: 0x00111E92 File Offset: 0x00110092
	public float GetFloatSetting(string prefsKey)
	{
		return KPlayerPrefs.GetFloat(prefsKey);
	}

	// Token: 0x0600A8D1 RID: 43217 RVA: 0x0040E18C File Offset: 0x0040C38C
	public void SetFloatSetting(string prefsKey, float value)
	{
		SandboxSettings.Setting<float> setting = this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs float setting named: ",
				prefsKey,
				" could be found amongst ",
				this.floatSettings.Count.ToString(),
				" float settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A8D2 RID: 43218 RVA: 0x00111E9A File Offset: 0x0011009A
	public void RestoreFloatSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetFloatSetting(prefsKey, this.GetFloatSetting(prefsKey));
			return;
		}
		this.ForceDefaultFloatSetting(prefsKey);
	}

	// Token: 0x0600A8D3 RID: 43219 RVA: 0x0040E210 File Offset: 0x0040C410
	public void ForceDefaultFloatSetting(string prefsKey)
	{
		this.SetFloatSetting(prefsKey, this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A8D4 RID: 43220 RVA: 0x00111EBA File Offset: 0x001100BA
	public void AddStringSetting(string prefsKey, Action<string> setAction, string defaultValue)
	{
		this.stringSettings.Add(new SandboxSettings.Setting<string>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A8D5 RID: 43221 RVA: 0x00111ECF File Offset: 0x001100CF
	public string GetStringSetting(string prefsKey)
	{
		return KPlayerPrefs.GetString(prefsKey);
	}

	// Token: 0x0600A8D6 RID: 43222 RVA: 0x0040E254 File Offset: 0x0040C454
	public void SetStringSetting(string prefsKey, string value)
	{
		SandboxSettings.Setting<string> setting = this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs string setting named: ",
				prefsKey,
				" could be found amongst ",
				this.stringSettings.Count.ToString(),
				" settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A8D7 RID: 43223 RVA: 0x00111ED7 File Offset: 0x001100D7
	public void RestoreStringSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetStringSetting(prefsKey, this.GetStringSetting(prefsKey));
			return;
		}
		this.ForceDefaultStringSetting(prefsKey);
	}

	// Token: 0x0600A8D8 RID: 43224 RVA: 0x0040E2D8 File Offset: 0x0040C4D8
	public void ForceDefaultStringSetting(string prefsKey)
	{
		this.SetStringSetting(prefsKey, this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A8D9 RID: 43225 RVA: 0x0040E31C File Offset: 0x0040C51C
	public SandboxSettings()
	{
		this.AddStringSetting("SandboxTools.SelectedEntity", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedEntity", data);
			this.OnChangeEntity();
		}, "MushBar");
		this.AddIntSetting("SandboxTools.SelectedElement", delegate(int data)
		{
			KPlayerPrefs.SetInt("SandboxTools.SelectedElement", data);
			this.OnChangeElement(this.hasRestoredElement);
			this.hasRestoredElement = true;
		}, (int)ElementLoader.GetElementIndex(SimHashes.Oxygen));
		this.AddStringSetting("SandboxTools.SelectedDisease", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedDisease", data);
			this.OnChangeDisease();
		}, Db.Get().Diseases.FoodGerms.Id);
		this.AddIntSetting("SandboxTools.DiseaseCount", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.DiseaseCount", val);
			this.OnChangeDiseaseCount();
		}, 0);
		this.AddStringSetting("SandboxTools.SelectedStory", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedStory", data);
			this.OnChangeStory();
		}, Db.Get().Stories.resources[Db.Get().Stories.resources.Count - 1].Id);
		this.AddIntSetting("SandboxTools.BrushSize", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.BrushSize", val);
			this.OnChangeBrushSize();
		}, 1);
		this.AddFloatSetting("SandboxTools.NoiseScale", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseScale", val);
			this.OnChangeNoiseScale();
		}, 1f);
		this.AddFloatSetting("SandboxTools.NoiseDensity", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseDensity", val);
			this.OnChangeNoiseDensity();
		}, 1f);
		this.AddFloatSetting("SandboxTools.Mass", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.Mass", val);
			this.OnChangeMass();
		}, 1f);
		this.AddFloatSetting("SandbosTools.Temperature", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.Temperature", val);
			this.OnChangeTemperature();
		}, 300f);
		this.AddFloatSetting("SandbosTools.TemperatureAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.TemperatureAdditive", val);
			this.OnChangeAdditiveTemperature();
		}, 5f);
		this.AddFloatSetting("SandbosTools.StressAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.StressAdditive", val);
			this.OnChangeAdditiveStress();
		}, 50f);
		this.AddIntSetting("SandbosTools.MoraleAdjustment", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandbosTools.MoraleAdjustment", val);
			this.OnChangeMoraleAdjustment();
		}, 50);
	}

	// Token: 0x0600A8DA RID: 43226 RVA: 0x0040E4F8 File Offset: 0x0040C6F8
	public void RestorePrefs()
	{
		foreach (SandboxSettings.Setting<int> setting in this.intSettings)
		{
			this.RestoreIntSetting(setting.PrefsKey);
		}
		foreach (SandboxSettings.Setting<float> setting2 in this.floatSettings)
		{
			this.RestoreFloatSetting(setting2.PrefsKey);
		}
		foreach (SandboxSettings.Setting<string> setting3 in this.stringSettings)
		{
			this.RestoreStringSetting(setting3.PrefsKey);
		}
	}

	// Token: 0x040084DE RID: 34014
	private List<SandboxSettings.Setting<int>> intSettings = new List<SandboxSettings.Setting<int>>();

	// Token: 0x040084DF RID: 34015
	private List<SandboxSettings.Setting<float>> floatSettings = new List<SandboxSettings.Setting<float>>();

	// Token: 0x040084E0 RID: 34016
	private List<SandboxSettings.Setting<string>> stringSettings = new List<SandboxSettings.Setting<string>>();

	// Token: 0x040084E1 RID: 34017
	public bool InstantBuild = true;

	// Token: 0x040084E2 RID: 34018
	private bool hasRestoredElement;

	// Token: 0x040084E3 RID: 34019
	public Action<bool> OnChangeElement;

	// Token: 0x040084E4 RID: 34020
	public System.Action OnChangeMass;

	// Token: 0x040084E5 RID: 34021
	public System.Action OnChangeDisease;

	// Token: 0x040084E6 RID: 34022
	public System.Action OnChangeDiseaseCount;

	// Token: 0x040084E7 RID: 34023
	public System.Action OnChangeStory;

	// Token: 0x040084E8 RID: 34024
	public System.Action OnChangeEntity;

	// Token: 0x040084E9 RID: 34025
	public System.Action OnChangeBrushSize;

	// Token: 0x040084EA RID: 34026
	public System.Action OnChangeNoiseScale;

	// Token: 0x040084EB RID: 34027
	public System.Action OnChangeNoiseDensity;

	// Token: 0x040084EC RID: 34028
	public System.Action OnChangeTemperature;

	// Token: 0x040084ED RID: 34029
	public System.Action OnChangeAdditiveTemperature;

	// Token: 0x040084EE RID: 34030
	public System.Action OnChangeAdditiveStress;

	// Token: 0x040084EF RID: 34031
	public System.Action OnChangeMoraleAdjustment;

	// Token: 0x040084F0 RID: 34032
	public const string KEY_SELECTED_ENTITY = "SandboxTools.SelectedEntity";

	// Token: 0x040084F1 RID: 34033
	public const string KEY_SELECTED_ELEMENT = "SandboxTools.SelectedElement";

	// Token: 0x040084F2 RID: 34034
	public const string KEY_SELECTED_DISEASE = "SandboxTools.SelectedDisease";

	// Token: 0x040084F3 RID: 34035
	public const string KEY_DISEASE_COUNT = "SandboxTools.DiseaseCount";

	// Token: 0x040084F4 RID: 34036
	public const string KEY_SELECTED_STORY = "SandboxTools.SelectedStory";

	// Token: 0x040084F5 RID: 34037
	public const string KEY_BRUSH_SIZE = "SandboxTools.BrushSize";

	// Token: 0x040084F6 RID: 34038
	public const string KEY_NOISE_SCALE = "SandboxTools.NoiseScale";

	// Token: 0x040084F7 RID: 34039
	public const string KEY_NOISE_DENSITY = "SandboxTools.NoiseDensity";

	// Token: 0x040084F8 RID: 34040
	public const string KEY_MASS = "SandboxTools.Mass";

	// Token: 0x040084F9 RID: 34041
	public const string KEY_TEMPERATURE = "SandbosTools.Temperature";

	// Token: 0x040084FA RID: 34042
	public const string KEY_TEMPERATURE_ADDITIVE = "SandbosTools.TemperatureAdditive";

	// Token: 0x040084FB RID: 34043
	public const string KEY_STRESS_ADDITIVE = "SandbosTools.StressAdditive";

	// Token: 0x040084FC RID: 34044
	public const string KEY_MORALE_ADJUSTMENT = "SandbosTools.MoraleAdjustment";

	// Token: 0x02001F47 RID: 8007
	public class Setting<T>
	{
		// Token: 0x0600A8E8 RID: 43240 RVA: 0x0011203C File Offset: 0x0011023C
		public Setting(string prefsKey, Action<T> setAction, T defaultValue)
		{
			this.prefsKey = prefsKey;
			this.SetAction = setAction;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x0600A8E9 RID: 43241 RVA: 0x00112059 File Offset: 0x00110259
		public string PrefsKey
		{
			get
			{
				return this.prefsKey;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (set) Token: 0x0600A8EA RID: 43242 RVA: 0x00112061 File Offset: 0x00110261
		public T Value
		{
			set
			{
				this.SetAction(value);
			}
		}

		// Token: 0x040084FD RID: 34045
		private string prefsKey;

		// Token: 0x040084FE RID: 34046
		private Action<T> SetAction;

		// Token: 0x040084FF RID: 34047
		public T defaultValue;
	}
}
