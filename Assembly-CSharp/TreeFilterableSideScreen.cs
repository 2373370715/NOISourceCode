using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204E RID: 8270
public class TreeFilterableSideScreen : SideScreenContent
{
	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x0600AF9B RID: 44955 RVA: 0x00116C40 File Offset: 0x00114E40
	private bool InputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x0600AF9C RID: 44956 RVA: 0x00116C57 File Offset: 0x00114E57
	public bool IsStorage
	{
		get
		{
			return this.storage != null;
		}
	}

	// Token: 0x0600AF9D RID: 44957 RVA: 0x00116C65 File Offset: 0x00114E65
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Initialize();
	}

	// Token: 0x0600AF9E RID: 44958 RVA: 0x0042B288 File Offset: 0x00429488
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
		this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
		MultiToggle multiToggle = this.allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			TreeFilterableSideScreenRow.State allCheckboxState = this.GetAllCheckboxState();
			if (allCheckboxState > TreeFilterableSideScreenRow.State.Mixed)
			{
				if (allCheckboxState == TreeFilterableSideScreenRow.State.On)
				{
					this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
					return;
				}
			}
			else
			{
				this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
			}
		}));
		this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
		this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
		this.initialized = true;
	}

	// Token: 0x0600AF9F RID: 44959 RVA: 0x0042B31C File Offset: 0x0042951C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		this.inputField.ActivateInputField();
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.InitSearch();
	}

	// Token: 0x0600AFA0 RID: 44960 RVA: 0x001162D0 File Offset: 0x001144D0
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x0600AFA1 RID: 44961 RVA: 0x001162E6 File Offset: 0x001144E6
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AFA2 RID: 44962 RVA: 0x001162E6 File Offset: 0x001144E6
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AFA3 RID: 44963 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x0600AFA4 RID: 44964 RVA: 0x0042B3D0 File Offset: 0x004295D0
	private void UpdateAllCheckBoxVisualState()
	{
		switch (this.GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			this.allCheckBox.ChangeState(0);
			return;
		case TreeFilterableSideScreenRow.State.Mixed:
			this.allCheckBox.ChangeState(1);
			return;
		case TreeFilterableSideScreenRow.State.On:
			this.allCheckBox.ChangeState(2);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600AFA5 RID: 44965 RVA: 0x0042B420 File Offset: 0x00429620
	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.visualDirty)
			{
				this.visualDirty = true;
				break;
			}
		}
		if (this.visualDirty)
		{
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
			{
				keyValuePair2.Value.RefreshRowElements();
				keyValuePair2.Value.UpdateCheckBoxVisualState();
			}
			this.UpdateAllCheckBoxVisualState();
			this.visualDirty = false;
		}
	}

	// Token: 0x0600AFA6 RID: 44966 RVA: 0x00116C73 File Offset: 0x00114E73
	private void OnlyAllowTransportItemsClicked()
	{
		this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
	}

	// Token: 0x0600AFA7 RID: 44967 RVA: 0x00116C8E File Offset: 0x00114E8E
	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	// Token: 0x0600AFA8 RID: 44968 RVA: 0x0042B4EC File Offset: 0x004296EC
	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.standardCommodity)
			{
				switch (keyValuePair.Value.GetState())
				{
				case TreeFilterableSideScreenRow.State.Off:
					flag2 = true;
					break;
				case TreeFilterableSideScreenRow.State.Mixed:
					flag3 = true;
					break;
				case TreeFilterableSideScreenRow.State.On:
					flag = true;
					break;
				}
			}
		}
		if (flag3)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		return TreeFilterableSideScreenRow.State.Off;
	}

	// Token: 0x0600AFA9 RID: 44969 RVA: 0x0042B59C File Offset: 0x0042979C
	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.standardCommodity)
					{
						keyValuePair.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
					}
				}
				goto IL_AB;
			}
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			goto IL_AB;
		case TreeFilterableSideScreenRow.State.On:
			break;
		default:
			goto IL_AB;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
		{
			if (keyValuePair2.Value.standardCommodity)
			{
				keyValuePair2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
			}
		}
		IL_AB:
		this.visualDirty = true;
	}

	// Token: 0x0600AFAA RID: 44970 RVA: 0x00116CA9 File Offset: 0x00114EA9
	public bool GetElementTagAcceptedState(Tag t)
	{
		return this.targetFilterable.ContainsTag(t);
	}

	// Token: 0x0600AFAB RID: 44971 RVA: 0x0042B678 File Offset: 0x00429878
	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		return component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI) && target.GetSMI<StorageTile.Instance>() == null;
	}

	// Token: 0x0600AFAC RID: 44972 RVA: 0x00116CB7 File Offset: 0x00114EB7
	private void ReconfigureForPreviousTarget()
	{
		global::Debug.Assert(this.target != null, "TreeFilterableSideScreen trying to restore null target.");
		this.SetTarget(this.target);
	}

	// Token: 0x0600AFAD RID: 44973 RVA: 0x0042B6D0 File Offset: 0x004298D0
	public override void SetTarget(GameObject target)
	{
		this.Initialize();
		this.target = target;
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		this.contentMask.GetComponent<LayoutElement>().minHeight = (float)((this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		this.storage = this.targetFilterable.GetFilterStorage();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		this.OnOnlyFetchMarkedItemsSettingChanged(null);
		this.OnOnlySpicedItemsSettingChanged(null);
		this.allCheckBoxLabel.SetText(this.targetFilterable.allResourceFilterLabelString);
		this.CreateCategories();
		this.CreateSpecialItemRows();
		this.titlebar.SetActive(false);
		if (this.storage.showSideScreenTitleBar)
		{
			this.titlebar.SetActive(true);
			this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
		}
		if (!this.InputFieldEmpty)
		{
			this.ClearSearch();
		}
		this.ToggleSearchConfiguration(!this.InputFieldEmpty);
	}

	// Token: 0x0600AFAE RID: 44974 RVA: 0x0042B828 File Offset: 0x00429A28
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (this.storage.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyallowTransportItemsRow.SetActive(true);
			return;
		}
		this.onlyallowTransportItemsRow.SetActive(false);
	}

	// Token: 0x0600AFAF RID: 44975 RVA: 0x0042B878 File Offset: 0x00429A78
	private void OnOnlySpicedItemsSettingChanged(object data)
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			this.onlyallowSpicedItemsRow.SetActive(true);
			this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
			return;
		}
		this.onlyallowSpicedItemsRow.SetActive(false);
	}

	// Token: 0x0600AFB0 RID: 44976 RVA: 0x00116CDB File Offset: 0x00114EDB
	public bool IsTagAllowed(Tag tag)
	{
		return this.targetFilterable.AcceptedTags.Contains(tag);
	}

	// Token: 0x0600AFB1 RID: 44977 RVA: 0x00116CEE File Offset: 0x00114EEE
	public void AddTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.AddTagToFilter(tag);
	}

	// Token: 0x0600AFB2 RID: 44978 RVA: 0x00116D0B File Offset: 0x00114F0B
	public void RemoveTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.RemoveTagFromFilter(tag);
	}

	// Token: 0x0600AFB3 RID: 44979 RVA: 0x0042B8CC File Offset: 0x00429ACC
	private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TreeFilterableSideScreen.TagOrderInfo> list = new List<TreeFilterableSideScreen.TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TreeFilterableSideScreen.TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TreeFilterableSideScreen.TagOrderInfo a, TreeFilterableSideScreen.TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	// Token: 0x0600AFB4 RID: 44980 RVA: 0x0042B960 File Offset: 0x00429B60
	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (this.tagRowMap.ContainsKey(rowTag))
		{
			return this.tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
		freeElement.Parent = this;
		freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
		this.tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)))
		{
			dictionary.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	// Token: 0x0600AFB5 RID: 44981 RVA: 0x00116D28 File Offset: 0x00114F28
	public float GetAmountInStorage(Tag tag)
	{
		if (!this.IsStorage)
		{
			return 0f;
		}
		return this.storage.GetMassAvailable(tag);
	}

	// Token: 0x0600AFB6 RID: 44982 RVA: 0x0042BA64 File Offset: 0x00429C64
	private void CreateCategories()
	{
		if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
		{
			bool flag = this.target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(this.storage.storageFilters))
			{
				Tag tag = tagOrderInfo.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(tag))
				{
					this.AddRow(tag);
				}
			}
			this.visualDirty = true;
			return;
		}
		global::Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
	}

	// Token: 0x0600AFB7 RID: 44983 RVA: 0x0042BB24 File Offset: 0x00429D24
	private void CreateSpecialItemRows()
	{
		this.specialItemsHeader.transform.SetAsLastSibling();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				keyValuePair.Value.transform.transform.SetAsLastSibling();
			}
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x0600AFB8 RID: 44984 RVA: 0x0042BBAC File Offset: 0x00429DAC
	private void RefreshSpecialItemsHeader()
	{
		bool active = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				active = true;
				break;
			}
		}
		this.specialItemsHeader.gameObject.SetActive(active);
	}

	// Token: 0x0600AFB9 RID: 44985 RVA: 0x00116D44 File Offset: 0x00114F44
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.target != null && (this.tagRowMap == null || this.tagRowMap.Count == 0))
		{
			this.ReconfigureForPreviousTarget();
		}
	}

	// Token: 0x0600AFBA RID: 44986 RVA: 0x0042BC20 File Offset: 0x00429E20
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.storage != null)
		{
			this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
			this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		}
		this.rowPool.ClearAll();
		this.elementPool.ClearAll();
		this.tagRowMap.Clear();
	}

	// Token: 0x0600AFBB RID: 44987 RVA: 0x0042BC9C File Offset: 0x00429E9C
	private void RecordRowExpandedStatus()
	{
		this.rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			this.rowExpandedStatusMemory.Add(keyValuePair.Key, keyValuePair.Value.ArrowExpanded);
		}
	}

	// Token: 0x0600AFBC RID: 44988 RVA: 0x0042BD14 File Offset: 0x00429F14
	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (this.rowExpandedStatusMemory.ContainsKey(keyValuePair.Key))
			{
				keyValuePair.Value.SetArrowToggleState(this.rowExpandedStatusMemory[keyValuePair.Key]);
			}
		}
	}

	// Token: 0x0600AFBD RID: 44989 RVA: 0x0042BD94 File Offset: 0x00429F94
	private void InitSearch()
	{
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.RecordRowExpandedStatus();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			if (this.InputFieldEmpty)
			{
				this.RestoreRowExpandedStatus();
			}
			this.ToggleSearchConfiguration(!this.InputFieldEmpty);
			this.UpdateSearchFilter();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.clearButton.onClick += delegate()
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		};
	}

	// Token: 0x0600AFBE RID: 44990 RVA: 0x0042BE38 File Offset: 0x0042A038
	private void ToggleSearchConfiguration(bool searching)
	{
		this.configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.ShowToggleBox(!searching);
		}
		if (searching)
		{
			this.specialItemsHeader.gameObject.SetActive(false);
			return;
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x0600AFBF RID: 44991 RVA: 0x00116D75 File Offset: 0x00114F75
	private void ClearSearch()
	{
		this.inputField.text = "";
		this.RestoreRowExpandedStatus();
		this.ToggleSearchConfiguration(false);
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x0600AFC0 RID: 44992 RVA: 0x00116D94 File Offset: 0x00114F94
	public string CurrentSearchValue
	{
		get
		{
			if (this.inputField.text == null)
			{
				return "";
			}
			return this.inputField.text;
		}
	}

	// Token: 0x0600AFC1 RID: 44993 RVA: 0x0042BEC4 File Offset: 0x0042A0C4
	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.FilterAgainstSearch(keyValuePair.Key, this.CurrentSearchValue);
		}
	}

	// Token: 0x04008A07 RID: 35335
	[SerializeField]
	private MultiToggle allCheckBox;

	// Token: 0x04008A08 RID: 35336
	[SerializeField]
	private LocText allCheckBoxLabel;

	// Token: 0x04008A09 RID: 35337
	[SerializeField]
	private GameObject specialItemsHeader;

	// Token: 0x04008A0A RID: 35338
	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	// Token: 0x04008A0B RID: 35339
	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	// Token: 0x04008A0C RID: 35340
	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	// Token: 0x04008A0D RID: 35341
	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	// Token: 0x04008A0E RID: 35342
	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	// Token: 0x04008A0F RID: 35343
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x04008A10 RID: 35344
	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	// Token: 0x04008A11 RID: 35345
	[SerializeField]
	private GameObject titlebar;

	// Token: 0x04008A12 RID: 35346
	[SerializeField]
	private GameObject contentMask;

	// Token: 0x04008A13 RID: 35347
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04008A14 RID: 35348
	[SerializeField]
	private KButton clearButton;

	// Token: 0x04008A15 RID: 35349
	[SerializeField]
	private GameObject configurationRowsContainer;

	// Token: 0x04008A16 RID: 35350
	private GameObject target;

	// Token: 0x04008A17 RID: 35351
	private bool visualDirty;

	// Token: 0x04008A18 RID: 35352
	private bool initialized;

	// Token: 0x04008A19 RID: 35353
	private KImage onlyAllowTransportItemsImg;

	// Token: 0x04008A1A RID: 35354
	public UIPool<TreeFilterableSideScreenElement> elementPool;

	// Token: 0x04008A1B RID: 35355
	private UIPool<TreeFilterableSideScreenRow> rowPool;

	// Token: 0x04008A1C RID: 35356
	private TreeFilterable targetFilterable;

	// Token: 0x04008A1D RID: 35357
	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	// Token: 0x04008A1E RID: 35358
	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	// Token: 0x04008A1F RID: 35359
	private Storage storage;

	// Token: 0x0200204F RID: 8271
	private struct TagOrderInfo
	{
		// Token: 0x04008A20 RID: 35360
		public Tag tag;

		// Token: 0x04008A21 RID: 35361
		public string strippedName;
	}
}
