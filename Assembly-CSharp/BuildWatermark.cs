using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class BuildWatermark : KScreen
{
	// Token: 0x06000024 RID: 36 RVA: 0x000A9FED File Offset: 0x000A81ED
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildWatermark.Instance = this;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000A9FFB File Offset: 0x000A81FB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshText();
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00146070 File Offset: 0x00144270
	public static string GetBuildText()
	{
		string text = DistributionPlatform.Initialized ? (LaunchInitializer.BuildPrefix() + "-") : "??-";
		if (Application.isEditor)
		{
			text += "<EDITOR>";
		}
		else
		{
			text += 663500U.ToString();
		}
		if (DistributionPlatform.Initialized)
		{
			text = text + "-" + DlcManager.GetSubscribedContentLetters();
		}
		else
		{
			text += "-?";
		}
		if (DebugHandler.enabled)
		{
			text += "D";
		}
		return text;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00146100 File Offset: 0x00144300
	public void RefreshText()
	{
		bool flag = true;
		bool flag2 = DistributionPlatform.Initialized && DistributionPlatform.Inst.IsArchiveBranch;
		string buildText = BuildWatermark.GetBuildText();
		this.button.ClearOnClick();
		if (flag)
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.WATERMARK, buildText));
			this.toolTip.ClearMultiStringTooltip();
		}
		else
		{
			this.textDisplay.SetText(string.Format(UI.DEVELOPMENTBUILDS.TESTING_WATERMARK, buildText));
			this.toolTip.SetSimpleTooltip(UI.DEVELOPMENTBUILDS.TESTING_TOOLTIP);
			if (this.interactable)
			{
				this.button.onClick += this.ShowTestingMessage;
			}
		}
		foreach (GameObject gameObject in this.archiveIcons)
		{
			gameObject.SetActive(flag && flag2);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x001461F8 File Offset: 0x001443F8
	private void ShowTestingMessage()
	{
		Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, Global.Instance.globalCanvas, true).PopupConfirmDialog(UI.DEVELOPMENTBUILDS.TESTING_MESSAGE, delegate
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}, delegate
		{
		}, null, null, UI.DEVELOPMENTBUILDS.TESTING_MESSAGE_TITLE, UI.DEVELOPMENTBUILDS.TESTING_MORE_INFO, null, null);
	}

	// Token: 0x0400002B RID: 43
	public bool interactable = true;

	// Token: 0x0400002C RID: 44
	public LocText textDisplay;

	// Token: 0x0400002D RID: 45
	public ToolTip toolTip;

	// Token: 0x0400002E RID: 46
	public KButton button;

	// Token: 0x0400002F RID: 47
	public List<GameObject> archiveIcons;

	// Token: 0x04000030 RID: 48
	public static BuildWatermark Instance;
}
