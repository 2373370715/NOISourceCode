using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F16 RID: 7958
public class ReportErrorDialog : MonoBehaviour
{
	// Token: 0x0600A75E RID: 42846 RVA: 0x00404968 File Offset: 0x00402B68
	private void Start()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(true);
		}
		this.StackTrace.SetActive(false);
		this.CrashLabel.text = ((this.mode == ReportErrorDialog.Mode.SubmitError) ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
		this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
		this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
		if (this.mode == ReportErrorDialog.Mode.DisableMods)
		{
			this.BuildModsList();
		}
		this.submitButton.gameObject.SetActive(this.submitAction != null);
		this.submitButton.onClick += this.OnSelect_SUBMIT;
		this.moreInfoButton.onClick += this.OnSelect_MOREINFO;
		this.continueGameButton.gameObject.SetActive(this.continueAction != null);
		this.continueGameButton.onClick += this.OnSelect_CONTINUE;
		this.quitButton.onClick += this.OnSelect_QUIT;
		this.messageInputField.text = UI.CRASHSCREEN.BODY;
		KCrashReporter.onCrashReported += this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress += this.UpdateProgressBar;
	}

	// Token: 0x0600A75F RID: 42847 RVA: 0x00404AC4 File Offset: 0x00402CC4
	private void BuildModsList()
	{
		DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
		Manager mod_mgr = Global.Instance.modManager;
		List<Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
		allCrashableMods.Sort((Mod x, Mod y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
		foreach (Mod mod in allCrashableMods)
		{
			if (mod.foundInStackTrace && mod.label.distribution_platform != Label.DistributionPlatform.Dev)
			{
				mod_mgr.EnableMod(mod.label, false, this);
			}
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject, false);
			LocText reference = hierarchyReferences.GetReference<LocText>("Title");
			reference.text = mod.title;
			reference.color = (mod.foundInStackTrace ? Color.red : Color.white);
			MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
			toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
			Label mod_label = mod.label;
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				bool flag = !mod_mgr.IsModEnabled(mod_label);
				toggle.ChangeState(flag ? 1 : 0);
				mod_mgr.EnableMod(mod_label, flag, this);
			}));
			toggle.GetComponent<ToolTip>().OnToolTip = (() => mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
			hierarchyReferences.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600A760 RID: 42848 RVA: 0x0010A248 File Offset: 0x00108448
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x0600A761 RID: 42849 RVA: 0x00404C98 File Offset: 0x00402E98
	private void OnDestroy()
	{
		if (KCrashReporter.terminateOnError)
		{
			App.Quit();
		}
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(false);
		}
		KCrashReporter.onCrashReported -= this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress -= this.UpdateProgressBar;
	}

	// Token: 0x0600A762 RID: 42850 RVA: 0x00111099 File Offset: 0x0010F299
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

	// Token: 0x0600A763 RID: 42851 RVA: 0x001110AA File Offset: 0x0010F2AA
	public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x0600A764 RID: 42852 RVA: 0x001110D0 File Offset: 0x0010F2D0
	public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x0600A765 RID: 42853 RVA: 0x00404CEC File Offset: 0x00402EEC
	public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

	// Token: 0x0600A766 RID: 42854 RVA: 0x001110EE File Offset: 0x0010F2EE
	public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x0600A767 RID: 42855 RVA: 0x0011111B File Offset: 0x0010F31B
	public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

	// Token: 0x0600A768 RID: 42856 RVA: 0x0011114E File Offset: 0x0010F34E
	public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

	// Token: 0x0600A769 RID: 42857 RVA: 0x00111163 File Offset: 0x0010F363
	public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

	// Token: 0x0600A76A RID: 42858 RVA: 0x00404D58 File Offset: 0x00402F58
	public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

	// Token: 0x0600A76B RID: 42859 RVA: 0x00404DB4 File Offset: 0x00402FB4
	public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600A76C RID: 42860 RVA: 0x00111178 File Offset: 0x0010F378
	public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

	// Token: 0x0600A76D RID: 42861 RVA: 0x0011118C File Offset: 0x0010F38C
	public string UserMessage()
	{
		return this.messageInputField.text;
	}

	// Token: 0x0600A76E RID: 42862 RVA: 0x00111199 File Offset: 0x0010F399
	private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

	// Token: 0x0600A76F RID: 42863 RVA: 0x001111AC File Offset: 0x0010F3AC
	public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x0400834E RID: 33614
	private System.Action submitAction;

	// Token: 0x0400834F RID: 33615
	private System.Action quitAction;

	// Token: 0x04008350 RID: 33616
	private System.Action continueAction;

	// Token: 0x04008351 RID: 33617
	public KInputTextField messageInputField;

	// Token: 0x04008352 RID: 33618
	[Header("Message")]
	public GameObject referenceMessage;

	// Token: 0x04008353 RID: 33619
	public LocText messageText;

	// Token: 0x04008354 RID: 33620
	[Header("Upload Progress")]
	public GameObject uploadInProgress;

	// Token: 0x04008355 RID: 33621
	public Image progressBar;

	// Token: 0x04008356 RID: 33622
	public LocText progressText;

	// Token: 0x04008357 RID: 33623
	private string m_stackTrace;

	// Token: 0x04008358 RID: 33624
	private bool m_crashSubmitted;

	// Token: 0x04008359 RID: 33625
	[SerializeField]
	private KButton submitButton;

	// Token: 0x0400835A RID: 33626
	[SerializeField]
	private KButton moreInfoButton;

	// Token: 0x0400835B RID: 33627
	[SerializeField]
	private KButton quitButton;

	// Token: 0x0400835C RID: 33628
	[SerializeField]
	private KButton continueGameButton;

	// Token: 0x0400835D RID: 33629
	[SerializeField]
	private LocText CrashLabel;

	// Token: 0x0400835E RID: 33630
	[SerializeField]
	private GameObject CrashDescription;

	// Token: 0x0400835F RID: 33631
	[SerializeField]
	private GameObject ModsInfo;

	// Token: 0x04008360 RID: 33632
	[SerializeField]
	private GameObject StackTrace;

	// Token: 0x04008361 RID: 33633
	[SerializeField]
	private GameObject modEntryPrefab;

	// Token: 0x04008362 RID: 33634
	[SerializeField]
	private Transform modEntryParent;

	// Token: 0x04008363 RID: 33635
	private ReportErrorDialog.Mode mode;

	// Token: 0x02001F17 RID: 7959
	private enum Mode
	{
		// Token: 0x04008365 RID: 33637
		SubmitError,
		// Token: 0x04008366 RID: 33638
		DisableMods
	}
}
