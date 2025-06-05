using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CFC RID: 7420
public class DetailTabHeader : KMonoBehaviour
{
	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06009ADE RID: 39646 RVA: 0x00109388 File Offset: 0x00107588
	public TargetPanel ActivePanel
	{
		get
		{
			if (this.tabPanels.ContainsKey(this.selectedTabID))
			{
				return this.tabPanels[this.selectedTabID];
			}
			return null;
		}
	}

	// Token: 0x06009ADF RID: 39647 RVA: 0x003C9EEC File Offset: 0x003C80EC
	public void Init()
	{
		this.detailsScreen = DetailsScreen.Instance;
		this.MakeTab("SIMPLEINFO", UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite("icon_display_screen_status"), UI.DETAILTABS.SIMPLEINFO.TOOLTIP, this.simpleInfoScreen);
		this.MakeTab("PERSONALITY", UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite("icon_display_screen_bio"), UI.DETAILTABS.PERSONALITY.TOOLTIP, this.minionPersonalityPanel);
		this.MakeTab("BUILDINGCHORES", UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite("icon_display_screen_errands"), UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, this.buildingInfoPanel);
		this.MakeTab("DETAILS", UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite("icon_display_screen_properties"), UI.DETAILTABS.DETAILS.TOOLTIP, this.additionalDetailsPanel);
		this.ChangeToDefaultTab();
	}

	// Token: 0x06009AE0 RID: 39648 RVA: 0x000AA038 File Offset: 0x000A8238
	private void MakeTabContents(GameObject panelToActivate)
	{
	}

	// Token: 0x06009AE1 RID: 39649 RVA: 0x003C9FDC File Offset: 0x003C81DC
	private void MakeTab(string id, string label, Sprite sprite, string tooltip, GameObject panelToActivate)
	{
		GameObject gameObject = Util.KInstantiateUI(this.tabPrefab, this.tabContainer, true);
		gameObject.name = "tab: " + id;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = sprite;
		component.GetReference<LocText>("label").text = label;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		GameObject gameObject2 = Util.KInstantiateUI(panelToActivate, this.panelContainer.gameObject, true);
		TargetPanel component3 = gameObject2.GetComponent<TargetPanel>();
		component3.SetTarget(this.detailsScreen.target);
		this.tabPanels.Add(id, component3);
		string targetTab = id;
		MultiToggle multiToggle = component2;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ChangeTab(targetTab);
		}));
		this.tabs.Add(id, component2);
		gameObject2.SetActive(false);
	}

	// Token: 0x06009AE2 RID: 39650 RVA: 0x003CA0C8 File Offset: 0x003C82C8
	private void ChangeTab(string id)
	{
		this.selectedTabID = id;
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.tabs)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.selectedTabID) ? 1 : 0);
		}
		foreach (KeyValuePair<string, TargetPanel> keyValuePair2 in this.tabPanels)
		{
			if (keyValuePair2.Key == id)
			{
				keyValuePair2.Value.gameObject.SetActive(true);
				keyValuePair2.Value.SetTarget(this.detailsScreen.target);
			}
			else
			{
				keyValuePair2.Value.SetTarget(null);
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06009AE3 RID: 39651 RVA: 0x001093B0 File Offset: 0x001075B0
	private void ChangeToDefaultTab()
	{
		this.ChangeTab("SIMPLEINFO");
	}

	// Token: 0x06009AE4 RID: 39652 RVA: 0x003CA1D4 File Offset: 0x003C83D4
	public void RefreshTabDisplayForTarget(GameObject target)
	{
		foreach (KeyValuePair<string, TargetPanel> keyValuePair in this.tabPanels)
		{
			this.tabs[keyValuePair.Key].gameObject.SetActive(keyValuePair.Value.IsValidForTarget(target));
		}
		if (this.tabPanels[this.selectedTabID].IsValidForTarget(target))
		{
			this.ChangeTab(this.selectedTabID);
			return;
		}
		this.ChangeToDefaultTab();
	}

	// Token: 0x040078F9 RID: 30969
	private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();

	// Token: 0x040078FA RID: 30970
	private string selectedTabID;

	// Token: 0x040078FB RID: 30971
	[SerializeField]
	private GameObject tabPrefab;

	// Token: 0x040078FC RID: 30972
	[SerializeField]
	private GameObject tabContainer;

	// Token: 0x040078FD RID: 30973
	[SerializeField]
	private GameObject panelContainer;

	// Token: 0x040078FE RID: 30974
	[Header("Screen Prefabs")]
	[SerializeField]
	private GameObject simpleInfoScreen;

	// Token: 0x040078FF RID: 30975
	[SerializeField]
	private GameObject minionPersonalityPanel;

	// Token: 0x04007900 RID: 30976
	[SerializeField]
	private GameObject buildingInfoPanel;

	// Token: 0x04007901 RID: 30977
	[SerializeField]
	private GameObject additionalDetailsPanel;

	// Token: 0x04007902 RID: 30978
	[SerializeField]
	private GameObject cosmeticsPanel;

	// Token: 0x04007903 RID: 30979
	[SerializeField]
	private GameObject materialPanel;

	// Token: 0x04007904 RID: 30980
	private DetailsScreen detailsScreen;

	// Token: 0x04007905 RID: 30981
	private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();
}
