using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E6F RID: 7791
public class MetricsOptionsScreen : KModalScreen
{
	// Token: 0x0600A335 RID: 41781 RVA: 0x0010E812 File Offset: 0x0010CA12
	private bool IsSettingsDirty()
	{
		return this.disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	// Token: 0x0600A336 RID: 41782 RVA: 0x0010E829 File Offset: 0x0010CA29
	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)) && !this.IsSettingsDirty())
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A337 RID: 41783 RVA: 0x003EF444 File Offset: 0x003ED644
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		this.title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.OnClickToggle();
		};
		this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		this.dismissButton.onClick += delegate()
		{
			if (this.IsSettingsDirty())
			{
				this.ApplySettingsAndDoRestart();
				return;
			}
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.descriptionButton.onClick.AddListener(delegate()
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		this.Refresh();
	}

	// Token: 0x0600A338 RID: 41784 RVA: 0x0010E853 File Offset: 0x0010CA53
	private void OnClickToggle()
	{
		this.disableDataCollection = !this.disableDataCollection;
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(this.disableDataCollection);
		this.Refresh();
	}

	// Token: 0x0600A339 RID: 41785 RVA: 0x003EF548 File Offset: 0x003ED748
	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = this.disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	// Token: 0x0600A33A RID: 41786 RVA: 0x003EF5DC File Offset: 0x003ED7DC
	private void Refresh()
	{
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!this.disableDataCollection);
		this.closeButton.isInteractable = !this.IsSettingsDirty();
		this.restartWarningText.gameObject.SetActive(this.IsSettingsDirty());
		if (this.IsSettingsDirty())
		{
			this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
			return;
		}
		this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
	}

	// Token: 0x04007F9A RID: 32666
	public LocText title;

	// Token: 0x04007F9B RID: 32667
	public KButton dismissButton;

	// Token: 0x04007F9C RID: 32668
	public KButton closeButton;

	// Token: 0x04007F9D RID: 32669
	public GameObject enableButton;

	// Token: 0x04007F9E RID: 32670
	public Button descriptionButton;

	// Token: 0x04007F9F RID: 32671
	public LocText restartWarningText;

	// Token: 0x04007FA0 RID: 32672
	private bool disableDataCollection;
}
