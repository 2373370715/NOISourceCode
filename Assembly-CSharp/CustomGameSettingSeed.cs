using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001CDF RID: 7391
public class CustomGameSettingSeed : CustomGameSettingWidget
{
	// Token: 0x06009A1D RID: 39453 RVA: 0x003C6348 File Offset: 0x003C4548
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.RandomizeButton.onClick += this.GetNewRandomSeed;
	}

	// Token: 0x06009A1E RID: 39454 RVA: 0x00108B28 File Offset: 0x00106D28
	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.GetNewRandomSeed();
	}

	// Token: 0x06009A1F RID: 39455 RVA: 0x003C63AC File Offset: 0x003C45AC
	public override void Refresh()
	{
		base.Refresh();
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(this.config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		this.allowChange = (currentClusterLayout.fixedCoordinate == -1);
		this.Input.interactable = this.allowChange;
		this.RandomizeButton.isInteractable = this.allowChange;
		if (this.allowChange)
		{
			this.InputToolTip.enabled = false;
			this.RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			this.InputToolTip.enabled = true;
			this.RandomizeButtonToolTip.enabled = true;
			this.InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			this.RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		this.Input.text = currentQualitySettingLevelId;
	}

	// Token: 0x06009A20 RID: 39456 RVA: 0x00108B59 File Offset: 0x00106D59
	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	// Token: 0x06009A21 RID: 39457 RVA: 0x003C647C File Offset: 0x003C467C
	private void OnEndEdit(string text)
	{
		int seed;
		try
		{
			seed = Convert.ToInt32(text);
		}
		catch
		{
			seed = 0;
		}
		this.SetSeed(seed);
	}

	// Token: 0x06009A22 RID: 39458 RVA: 0x00108B68 File Offset: 0x00106D68
	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(this.config, seed.ToString());
		this.Refresh();
	}

	// Token: 0x06009A23 RID: 39459 RVA: 0x003C64B0 File Offset: 0x003C46B0
	private void OnValueChanged(string text)
	{
		int num = 0;
		try
		{
			num = Convert.ToInt32(text);
		}
		catch
		{
			if (text.Length > 0)
			{
				this.Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				this.Input.text = "";
			}
		}
		if (num > 2147483647)
		{
			this.Input.text = text.Substring(0, text.Length - 1);
		}
	}

	// Token: 0x06009A24 RID: 39460 RVA: 0x003C6534 File Offset: 0x003C4734
	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		this.SetSeed(seed);
	}

	// Token: 0x04007844 RID: 30788
	[SerializeField]
	private LocText Label;

	// Token: 0x04007845 RID: 30789
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04007846 RID: 30790
	[SerializeField]
	private KInputTextField Input;

	// Token: 0x04007847 RID: 30791
	[SerializeField]
	private KButton RandomizeButton;

	// Token: 0x04007848 RID: 30792
	[SerializeField]
	private ToolTip InputToolTip;

	// Token: 0x04007849 RID: 30793
	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	// Token: 0x0400784A RID: 30794
	private const int MAX_VALID_SEED = 2147483647;

	// Token: 0x0400784B RID: 30795
	private SeedSettingConfig config;

	// Token: 0x0400784C RID: 30796
	private bool allowChange = true;
}
