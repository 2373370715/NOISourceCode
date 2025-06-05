using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001F1B RID: 7963
public class ReportScreen : KScreen
{
	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x0600A778 RID: 42872 RVA: 0x0011122B File Offset: 0x0010F42B
	// (set) Token: 0x0600A779 RID: 42873 RVA: 0x00111232 File Offset: 0x0010F432
	public static ReportScreen Instance { get; private set; }

	// Token: 0x0600A77A RID: 42874 RVA: 0x0011123A File Offset: 0x0010F43A
	public static void DestroyInstance()
	{
		ReportScreen.Instance = null;
	}

	// Token: 0x0600A77B RID: 42875 RVA: 0x00404E7C File Offset: 0x0040307C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ReportScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.prevButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day - 1);
		};
		this.nextButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day + 1);
		};
		this.summaryButton.onClick += delegate()
		{
			RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
		};
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A77C RID: 42876 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600A77D RID: 42877 RVA: 0x00111242 File Offset: 0x0010F442
	protected override void OnShow(bool bShow)
	{
		base.OnShow(bShow);
		if (ReportManager.Instance != null)
		{
			this.currentReport = ReportManager.Instance.TodaysReport;
		}
	}

	// Token: 0x0600A77E RID: 42878 RVA: 0x00111268 File Offset: 0x0010F468
	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	// Token: 0x0600A77F RID: 42879 RVA: 0x00111276 File Offset: 0x0010F476
	public override void ScreenUpdate(bool b)
	{
		base.ScreenUpdate(b);
		this.Refresh();
	}

	// Token: 0x0600A780 RID: 42880 RVA: 0x00404F20 File Offset: 0x00403120
	private void Refresh()
	{
		global::Debug.Assert(this.currentReport != null);
		if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, this.currentReport.day));
		}
		else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, this.currentReport.day));
		}
		else
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day));
		}
		bool flag = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
		this.nextButton.isInteractable = flag;
		if (flag)
		{
			this.nextButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day + 1);
			this.nextButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.nextButton.GetComponent<ToolTip>().enabled = false;
		}
		flag = (this.currentReport.day > 1);
		this.prevButton.isInteractable = flag;
		if (flag)
		{
			this.prevButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day - 1);
			this.prevButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.prevButton.GetComponent<ToolTip>().enabled = false;
		}
		this.AddSpacer(0);
		int num = 1;
		foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in ReportManager.Instance.ReportGroups)
		{
			ReportManager.ReportEntry entry = this.currentReport.GetEntry(keyValuePair.Key);
			if (num != keyValuePair.Value.group)
			{
				num = keyValuePair.Value.group;
				this.AddSpacer(num);
			}
			bool flag2 = entry.accumulate != 0f || keyValuePair.Value.reportIfZero;
			if (keyValuePair.Value.isHeader)
			{
				this.CreateHeader(keyValuePair.Value);
			}
			else if (flag2)
			{
				this.CreateOrUpdateLine(entry, keyValuePair.Value, flag2);
			}
		}
	}

	// Token: 0x0600A781 RID: 42881 RVA: 0x00111285 File Offset: 0x0010F485
	public void ShowReport(int day)
	{
		this.currentReport = ReportManager.Instance.FindReport(day);
		global::Debug.Assert(this.currentReport != null, "Can't find report for day: " + day.ToString());
		this.Refresh();
	}

	// Token: 0x0600A782 RID: 42882 RVA: 0x004051BC File Offset: 0x004033BC
	private GameObject AddSpacer(int group)
	{
		GameObject gameObject;
		if (this.lineItems.ContainsKey(group.ToString()))
		{
			gameObject = this.lineItems[group.ToString()];
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
			gameObject.name = "Spacer" + group.ToString();
			this.lineItems[group.ToString()] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600A783 RID: 42883 RVA: 0x0040523C File Offset: 0x0040343C
	private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (gameObject == null)
		{
			gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
			gameObject.name = "LineItemHeader" + this.lineItems.Count.ToString();
			this.lineItems[reportGroup.stringKey] = gameObject;
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
		return gameObject;
	}

	// Token: 0x0600A784 RID: 42884 RVA: 0x004052C4 File Offset: 0x004034C4
	private GameObject CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (!is_line_active)
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if (gameObject == null)
			{
				gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
				gameObject.name = "LineItem" + this.lineItems.Count.ToString();
				this.lineItems[reportGroup.stringKey] = gameObject;
			}
			gameObject.SetActive(true);
			gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
		}
		return gameObject;
	}

	// Token: 0x0600A785 RID: 42885 RVA: 0x001112BD File Offset: 0x0010F4BD
	private void OnClickClose()
	{
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
		this.Show(false);
	}

	// Token: 0x0400836E RID: 33646
	[SerializeField]
	private LocText title;

	// Token: 0x0400836F RID: 33647
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008370 RID: 33648
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04008371 RID: 33649
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04008372 RID: 33650
	[SerializeField]
	private KButton summaryButton;

	// Token: 0x04008373 RID: 33651
	[SerializeField]
	private GameObject lineItem;

	// Token: 0x04008374 RID: 33652
	[SerializeField]
	private GameObject lineItemSpacer;

	// Token: 0x04008375 RID: 33653
	[SerializeField]
	private GameObject lineItemHeader;

	// Token: 0x04008376 RID: 33654
	[SerializeField]
	private GameObject contentFolder;

	// Token: 0x04008377 RID: 33655
	private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();

	// Token: 0x04008378 RID: 33656
	private ReportManager.DailyReport currentReport;
}
