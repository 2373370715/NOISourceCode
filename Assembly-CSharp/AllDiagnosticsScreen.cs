using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C20 RID: 7200
public class AllDiagnosticsScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x060095BD RID: 38333 RVA: 0x00106015 File Offset: 0x00104215
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AllDiagnosticsScreen.Instance = this;
		this.ConfigureDebugToggle();
	}

	// Token: 0x060095BE RID: 38334 RVA: 0x00106029 File Offset: 0x00104229
	protected override void OnForcedCleanUp()
	{
		AllDiagnosticsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060095BF RID: 38335 RVA: 0x003A8968 File Offset: 0x003A6B68
	private void ConfigureDebugToggle()
	{
		Game.Instance.Subscribe(1557339983, new Action<object>(this.DebugToggleRefresh));
		MultiToggle toggle = this.debugNotificationToggleCotainer.GetComponentInChildren<MultiToggle>();
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			DebugHandler.ToggleDisableNotifications();
			toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
		}));
		this.DebugToggleRefresh(null);
		toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
	}

	// Token: 0x060095C0 RID: 38336 RVA: 0x00106037 File Offset: 0x00104237
	private void DebugToggleRefresh(object data = null)
	{
		this.debugNotificationToggleCotainer.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x060095C1 RID: 38337 RVA: 0x003A89EC File Offset: 0x003A6BEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		Game.Instance.Subscribe(-1280433810, new Action<object>(this.Populate));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		this.searchInputField.onValueChanged.AddListener(delegate(string value)
		{
			this.SearchFilter(value);
		});
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.Show(false);
	}

	// Token: 0x060095C2 RID: 38338 RVA: 0x0010604E File Offset: 0x0010424E
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllResourcesScreen.Instance.Show(false);
			this.RefreshSubrows();
		}
	}

	// Token: 0x060095C3 RID: 38339 RVA: 0x003A8ADC File Offset: 0x003A6CDC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060095C4 RID: 38340 RVA: 0x00106075 File Offset: 0x00104275
	public int GetRowCount()
	{
		return this.diagnosticRows.Count;
	}

	// Token: 0x060095C5 RID: 38341 RVA: 0x003A8B30 File Offset: 0x003A6D30
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060095C6 RID: 38342 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060095C7 RID: 38343 RVA: 0x003A8B84 File Offset: 0x003A6D84
	public void Populate(object data = null)
	{
		this.SpawnRows();
		foreach (string s in this.diagnosticRows.Keys)
		{
			Tag key = s;
			this.currentlyDisplayedRows[key] = true;
		}
		this.SearchFilter(this.searchInputField.text);
		this.RefreshRows();
	}

	// Token: 0x060095C8 RID: 38344 RVA: 0x003A8C04 File Offset: 0x003A6E04
	private void SpawnRows()
	{
		foreach (KeyValuePair<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> keyValuePair in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings)
		{
			foreach (KeyValuePair<string, ColonyDiagnosticUtility.DisplaySetting> keyValuePair2 in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[keyValuePair.Key])
			{
				if (!this.diagnosticRows.ContainsKey(keyValuePair2.Key))
				{
					ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair2.Key, keyValuePair.Key);
					if (!(diagnostic is WorkTimeDiagnostic) && !(diagnostic is ChoreGroupDiagnostic))
					{
						this.SpawnRow(diagnostic, this.rootListContainer);
					}
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.diagnosticRows)
		{
			list.Add(keyValuePair3.Key);
		}
		list.Sort((string a, string b) => UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(a)).CompareTo(UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(b))));
		foreach (string key in list)
		{
			this.diagnosticRows[key].transform.SetAsLastSibling();
		}
	}

	// Token: 0x060095C9 RID: 38345 RVA: 0x003A8DB8 File Offset: 0x003A6FB8
	private void SpawnRow(ColonyDiagnostic diagnostic, GameObject container)
	{
		if (diagnostic == null)
		{
			return;
		}
		if (!this.diagnosticRows.ContainsKey(diagnostic.id))
		{
			GameObject gameObject = Util.KInstantiateUI(this.diagnosticLinePrefab, container, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("NameLabel").SetText(diagnostic.name);
			string id2 = diagnostic.id;
			MultiToggle reference = component.GetReference<MultiToggle>("PinToggle");
			string id = diagnostic.id;
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnostic.id))
				{
					ColonyDiagnosticUtility.Instance.ClearDiagnosticTutorialSetting(diagnostic.id);
				}
				else
				{
					int activeWorldId = ClusterManager.Instance.activeWorldId;
					int num = ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] - ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
					if (num < 0)
					{
						num = 2;
					}
					ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] = (ColonyDiagnosticUtility.DisplaySetting)num;
				}
				this.RefreshRows();
				ColonyDiagnosticScreen.Instance.RefreshAll();
			}));
			GraphBase component2 = component.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>();
			component2.axis_x.min_value = 0f;
			component2.axis_x.max_value = 600f;
			component2.axis_x.guide_frequency = 120f;
			component2.RefreshGuides();
			this.diagnosticRows.Add(id2, gameObject);
			this.criteriaRows.Add(id2, new Dictionary<string, GameObject>());
			this.currentlyDisplayedRows.Add(id2, true);
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(diagnostic.icon);
			this.RefreshPinnedState(id2);
			RectTransform reference2 = component.GetReference<RectTransform>("SubRows");
			DiagnosticCriterion[] criteria = diagnostic.GetCriteria();
			for (int i = 0; i < criteria.Length; i++)
			{
				DiagnosticCriterion sub = criteria[i];
				GameObject gameObject2 = Util.KInstantiateUI(this.subDiagnosticLinePrefab, reference2.gameObject, true);
				gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.DIAGNOSTICS_SCREEN.CRITERIA_TOOLTIP, diagnostic.name, sub.name));
				HierarchyReferences component3 = gameObject2.GetComponent<HierarchyReferences>();
				component3.GetReference<LocText>("Label").SetText(sub.name);
				this.criteriaRows[diagnostic.id].Add(sub.id, gameObject2);
				MultiToggle reference3 = component3.GetReference<MultiToggle>("PinToggle");
				reference3.onClick = (System.Action)Delegate.Combine(reference3.onClick, new System.Action(delegate()
				{
					int activeWorldId = ClusterManager.Instance.activeWorldId;
					bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, diagnostic.id, sub.id);
					ColonyDiagnosticUtility.Instance.SetCriteriaEnabled(activeWorldId, diagnostic.id, sub.id, !flag);
					this.RefreshSubrows();
				}));
			}
			this.subrowContainerOpen.Add(diagnostic.id, false);
			MultiToggle reference4 = component.GetReference<MultiToggle>("SubrowToggle");
			MultiToggle multiToggle = reference4;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				this.subrowContainerOpen[diagnostic.id] = !this.subrowContainerOpen[diagnostic.id];
				this.RefreshSubrows();
			}));
			component.GetReference<MultiToggle>("MainToggle").onClick = reference4.onClick;
		}
	}

	// Token: 0x060095CA RID: 38346 RVA: 0x00106082 File Offset: 0x00104282
	private void FilterRowBySearch(Tag tag, string filter)
	{
		this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
	}

	// Token: 0x060095CB RID: 38347 RVA: 0x003A90D4 File Offset: 0x003A72D4
	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			this.FilterRowBySearch(keyValuePair.Key, search);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.diagnosticRows)
		{
			this.currentlyDisplayedRows[keyValuePair2.Key] = this.PassesSearchFilter(keyValuePair2.Key, search);
		}
		this.SetRowsActive();
	}

	// Token: 0x060095CC RID: 38348 RVA: 0x003A91A0 File Offset: 0x003A73A0
	private bool PassesSearchFilter(Tag tag, string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			return true;
		}
		filter = filter.ToUpper();
		string id = tag.ToString();
		if (ColonyDiagnosticUtility.Instance.GetDiagnosticName(id).ToUpper().Contains(filter) || tag.Name.ToUpper().Contains(filter))
		{
			return true;
		}
		ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(id, ClusterManager.Instance.activeWorldId);
		if (diagnostic == null)
		{
			return false;
		}
		DiagnosticCriterion[] criteria = diagnostic.GetCriteria();
		if (criteria == null)
		{
			return false;
		}
		DiagnosticCriterion[] array = criteria;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.ToUpper().Contains(filter))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060095CD RID: 38349 RVA: 0x003A9250 File Offset: 0x003A7450
	private void RefreshPinnedState(string diagnosticID)
	{
		if (!ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId].ContainsKey(diagnosticID))
		{
			return;
		}
		MultiToggle reference = this.diagnosticRows[diagnosticID].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
		{
			reference.ChangeState(3);
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				reference.ChangeState(2);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				reference.ChangeState(1);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				reference.ChangeState(0);
				break;
			}
		}
		string simpleTooltip = "";
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
		{
			simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.TUTORIAL_DISABLED;
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.NEVER;
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALWAYS;
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				simpleTooltip = UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALERT_ONLY;
				break;
			}
		}
		reference.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x060095CE RID: 38350 RVA: 0x003A9384 File Offset: 0x003A7584
	public void RefreshRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		if (this.allowRefresh)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
			{
				HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("AvailableLabel").SetText(keyValuePair.Key);
				component.GetReference<RectTransform>("SubRows").gameObject.SetActive(false);
				ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId);
				if (diagnostic != null)
				{
					component.GetReference<LocText>("AvailableLabel").SetText(diagnostic.GetAverageValueString());
					component.GetReference<Image>("Indicator").color = diagnostic.colors[diagnostic.LatestResult.opinion];
					ToolTip reference = component.GetReference<ToolTip>("Tooltip");
					reference.refreshWhileHovering = true;
					reference.SetSimpleTooltip(Strings.Get(new StringKey("STRINGS.UI.COLONY_DIAGNOSTICS." + diagnostic.id.ToUpper() + ".TOOLTIP_NAME")) + "\n" + diagnostic.LatestResult.GetFormattedMessage());
				}
				this.RefreshPinnedState(keyValuePair.Key);
			}
		}
		this.SetRowsActive();
		this.RefreshSubrows();
	}

	// Token: 0x060095CF RID: 38351 RVA: 0x003A9514 File Offset: 0x003A7714
	private void RefreshSubrows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
			component.GetReference<MultiToggle>("SubrowToggle").ChangeState((!this.subrowContainerOpen[keyValuePair.Key]) ? 0 : 1);
			component.GetReference<RectTransform>("SubRows").gameObject.SetActive(this.subrowContainerOpen[keyValuePair.Key]);
			int num = 0;
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.criteriaRows[keyValuePair.Key])
			{
				MultiToggle reference = keyValuePair2.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
				int activeWorldId = ClusterManager.Instance.activeWorldId;
				string key = keyValuePair.Key;
				string key2 = keyValuePair2.Key;
				bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, key, key2);
				reference.ChangeState(flag ? 1 : 0);
				if (flag)
				{
					num++;
				}
			}
			component.GetReference<LocText>("SubrowHeaderLabel").SetText(string.Format(UI.DIAGNOSTICS_SCREEN.CRITERIA_ENABLED_COUNT, num, this.criteriaRows[keyValuePair.Key].Count));
		}
	}

	// Token: 0x060095D0 RID: 38352 RVA: 0x003A96CC File Offset: 0x003A78CC
	private void RefreshCharts()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
			ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId);
			if (diagnostic != null)
			{
				SparkLayer reference = component.GetReference<SparkLayer>("Chart");
				Tracker tracker = diagnostic.tracker;
				if (tracker != null)
				{
					float num = 3000f;
					global::Tuple<float, float>[] array = tracker.ChartableData(num);
					reference.graph.axis_x.max_value = array[array.Length - 1].first;
					reference.graph.axis_x.min_value = reference.graph.axis_x.max_value - num;
					reference.RefreshLine(array, "resourceAmount");
				}
			}
		}
	}

	// Token: 0x060095D1 RID: 38353 RVA: 0x003A97C8 File Offset: 0x003A79C8
	private void SetRowsActive()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.diagnosticRows)
		{
			if (ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, ClusterManager.Instance.activeWorldId) == null)
			{
				this.currentlyDisplayedRows[keyValuePair.Key] = false;
			}
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.diagnosticRows)
		{
			if (keyValuePair2.Value.activeSelf != this.currentlyDisplayedRows[keyValuePair2.Key])
			{
				keyValuePair2.Value.SetActive(this.currentlyDisplayedRows[keyValuePair2.Key]);
			}
		}
	}

	// Token: 0x060095D2 RID: 38354 RVA: 0x00106098 File Offset: 0x00104298
	public void Sim4000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshCharts();
	}

	// Token: 0x060095D3 RID: 38355 RVA: 0x001060A9 File Offset: 0x001042A9
	public void Sim1000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshRows();
	}

	// Token: 0x04007485 RID: 29829
	private Dictionary<string, GameObject> diagnosticRows = new Dictionary<string, GameObject>();

	// Token: 0x04007486 RID: 29830
	private Dictionary<string, Dictionary<string, GameObject>> criteriaRows = new Dictionary<string, Dictionary<string, GameObject>>();

	// Token: 0x04007487 RID: 29831
	public GameObject rootListContainer;

	// Token: 0x04007488 RID: 29832
	public GameObject diagnosticLinePrefab;

	// Token: 0x04007489 RID: 29833
	public GameObject subDiagnosticLinePrefab;

	// Token: 0x0400748A RID: 29834
	public KButton closeButton;

	// Token: 0x0400748B RID: 29835
	public bool allowRefresh = true;

	// Token: 0x0400748C RID: 29836
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x0400748D RID: 29837
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x0400748E RID: 29838
	public static AllDiagnosticsScreen Instance;

	// Token: 0x0400748F RID: 29839
	public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();

	// Token: 0x04007490 RID: 29840
	public Dictionary<Tag, bool> subrowContainerOpen = new Dictionary<Tag, bool>();

	// Token: 0x04007491 RID: 29841
	[SerializeField]
	private RectTransform debugNotificationToggleCotainer;
}
