using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D1F RID: 7455
public class FacadeSelectionPanel : KMonoBehaviour
{
	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x06009BB6 RID: 39862 RVA: 0x00109C9A File Offset: 0x00107E9A
	private int GridLayoutConstraintCount
	{
		get
		{
			if (this.gridLayout != null)
			{
				return this.gridLayout.constraintCount;
			}
			return 3;
		}
	}

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x06009BB8 RID: 39864 RVA: 0x00109CC6 File Offset: 0x00107EC6
	// (set) Token: 0x06009BB7 RID: 39863 RVA: 0x00109CB7 File Offset: 0x00107EB7
	public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
	{
		get
		{
			return this.selectedOutfitCategory;
		}
		set
		{
			this.selectedOutfitCategory = value;
			this.Refresh();
		}
	}

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x06009BB9 RID: 39865 RVA: 0x00109CCE File Offset: 0x00107ECE
	public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x06009BBA RID: 39866 RVA: 0x00109CD6 File Offset: 0x00107ED6
	// (set) Token: 0x06009BBB RID: 39867 RVA: 0x003CDDE4 File Offset: 0x003CBFE4
	public string SelectedFacade
	{
		get
		{
			return this._selectedFacade;
		}
		set
		{
			if (this._selectedFacade != value)
			{
				this._selectedFacade = value;
				FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
				if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
				{
					if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
					{
						this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
					}
				}
				else
				{
					this.RefreshTogglesForBuilding();
				}
				if (this.OnFacadeSelectionChanged != null)
				{
					this.OnFacadeSelectionChanged();
				}
			}
		}
	}

	// Token: 0x06009BBC RID: 39868 RVA: 0x00109CDE File Offset: 0x00107EDE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
	}

	// Token: 0x06009BBD RID: 39869 RVA: 0x00109CF7 File Offset: 0x00107EF7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.getMoreButton.ClearOnClick();
		this.getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

	// Token: 0x06009BBE RID: 39870 RVA: 0x003CDE40 File Offset: 0x003CC040
	public void SetBuildingDef(string defID, string currentFacadeID = null)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.BuildingFacade;
		this.ClearToggles();
		this.selectedBuildingDefID = defID;
		this.SelectedFacade = ((currentFacadeID == null) ? "DEFAULT_FACADE" : currentFacadeID);
		this.RefreshTogglesForBuilding();
		if (this.hideWhenEmpty)
		{
			base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
		}
	}

	// Token: 0x06009BBF RID: 39871 RVA: 0x00109D25 File Offset: 0x00107F25
	public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
		this.ClearToggles();
		this.SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009BC0 RID: 39872 RVA: 0x003CDEA0 File Offset: 0x003CC0A0
	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

	// Token: 0x06009BC1 RID: 39873 RVA: 0x003CDF28 File Offset: 0x003CC128
	public void Refresh()
	{
		FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
		if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
		{
			if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
			{
				this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
			}
		}
		else
		{
			this.RefreshTogglesForBuilding();
		}
		this.getMoreButton.gameObject.SetActive(this.showGetMoreButton);
		if (this.useDummyPlaceholder)
		{
			for (int i = 0; i < this.dummyGridPlaceholders.Count; i++)
			{
				this.dummyGridPlaceholders[i].SetActive(false);
			}
			int num = 0;
			for (int j = 0; j < this.toggleContainer.transform.childCount; j++)
			{
				if (this.toggleContainer.GetChild(j).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			this.getMoreButton.transform.SetAsLastSibling();
			if (num % this.GridLayoutConstraintCount != 0)
			{
				for (int k = 0; k < this.GridLayoutConstraintCount - 1; k++)
				{
					this.dummyGridPlaceholders[k].SetActive(k < this.GridLayoutConstraintCount - num % this.GridLayoutConstraintCount);
					this.dummyGridPlaceholders[k].transform.SetAsLastSibling();
				}
				return;
			}
		}
		else
		{
			this.getMoreButton.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06009BC2 RID: 39874 RVA: 0x003CE058 File Offset: 0x003CC258
	private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == outfitType
		select outfit;
		List<string> list = new List<string>();
		using (Dictionary<string, FacadeSelectionPanel.FacadeToggle>.Enumerator enumerator = this.activeFacadeToggles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> toggle = enumerator.Current;
				if (!enumerable.Any((ClothingOutfitTarget match) => match.OutfitId == toggle.Key))
				{
					list.Add(toggle.Key);
				}
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultOutfitToggle();
		enumerable = enumerable.StableSort((ClothingOutfitTarget a, ClothingOutfitTarget b) => a.OutfitId.CompareTo(b.OutfitId));
		foreach (ClothingOutfitTarget clothingOutfitTarget in enumerable)
		{
			if (!clothingOutfitTarget.DoesContainLockedItems())
			{
				this.AddNewOutfitToggle(clothingOutfitTarget.OutfitId, false);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			keyValuePair.Value.multiToggle.ChangeState((this.SelectedFacade != null && this.SelectedFacade == keyValuePair.Key) ? 1 : 0);
		}
		this.RefreshHeight();
	}

	// Token: 0x06009BC3 RID: 39875 RVA: 0x003CE27C File Offset: 0x003CC47C
	private void RefreshTogglesForBuilding()
	{
		BuildingDef buildingDef = Assets.GetBuildingDef(this.selectedBuildingDefID);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			if (!buildingDef.AvailableFacades.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultBuildingFacadeToggle();
		foreach (string text in buildingDef.AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(text);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				this.AddNewBuildingToggle(text);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair2 in this.activeFacadeToggles)
		{
			keyValuePair2.Value.multiToggle.ChangeState((this.SelectedFacade == keyValuePair2.Key) ? 1 : 0);
		}
		this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		this.RefreshHeight();
	}

	// Token: 0x06009BC4 RID: 39876 RVA: 0x00109D4D File Offset: 0x00107F4D
	private void RefreshHeight()
	{
		if (this.usesScrollRect)
		{
			LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
			component.minHeight = (float)(58 * ((this.activeFacadeToggles.Count <= 5) ? 1 : 2));
			component.preferredHeight = component.minHeight;
		}
	}

	// Token: 0x06009BC5 RID: 39877 RVA: 0x00109D89 File Offset: 0x00107F89
	private void AddDefaultBuildingFacadeToggle()
	{
		this.AddNewBuildingToggle("DEFAULT_FACADE");
	}

	// Token: 0x06009BC6 RID: 39878 RVA: 0x00109D96 File Offset: 0x00107F96
	private void AddDefaultOutfitToggle()
	{
		this.AddNewOutfitToggle("DEFAULT_FACADE", true);
	}

	// Token: 0x06009BC7 RID: 39879 RVA: 0x003CE484 File Offset: 0x003CC684
	private void AddNewBuildingToggle(string facadeID)
	{
		if (this.activeFacadeToggles.ContainsKey(facadeID))
		{
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
	}

	// Token: 0x06009BC8 RID: 39880 RVA: 0x003CE54C File Offset: 0x003CC74C
	private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
	{
		if (this.activeFacadeToggles.ContainsKey(outfitID))
		{
			if (setAsFirstSibling)
			{
				this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
			}
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(outfitID, gameObject, this.selectedOutfitCategory);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
		if (setAsFirstSibling)
		{
			this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
		}
	}

	// Token: 0x06009BC9 RID: 39881 RVA: 0x00109DA4 File Offset: 0x00107FA4
	private void SelectFacade(string id)
	{
		this.SelectedFacade = id;
	}

	// Token: 0x040079CB RID: 31179
	[SerializeField]
	private GameObject togglePrefab;

	// Token: 0x040079CC RID: 31180
	[SerializeField]
	private RectTransform toggleContainer;

	// Token: 0x040079CD RID: 31181
	[SerializeField]
	private bool usesScrollRect;

	// Token: 0x040079CE RID: 31182
	[SerializeField]
	private LayoutElement scrollRect;

	// Token: 0x040079CF RID: 31183
	private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

	// Token: 0x040079D0 RID: 31184
	private List<GameObject> pooledFacadeToggles = new List<GameObject>();

	// Token: 0x040079D1 RID: 31185
	[SerializeField]
	private KButton getMoreButton;

	// Token: 0x040079D2 RID: 31186
	[SerializeField]
	private bool showGetMoreButton;

	// Token: 0x040079D3 RID: 31187
	[SerializeField]
	private bool hideWhenEmpty = true;

	// Token: 0x040079D4 RID: 31188
	[SerializeField]
	private bool useDummyPlaceholder;

	// Token: 0x040079D5 RID: 31189
	private GridLayoutGroup gridLayout;

	// Token: 0x040079D6 RID: 31190
	[SerializeField]
	private List<GameObject> dummyGridPlaceholders;

	// Token: 0x040079D7 RID: 31191
	public System.Action OnFacadeSelectionChanged;

	// Token: 0x040079D8 RID: 31192
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	// Token: 0x040079D9 RID: 31193
	private string selectedBuildingDefID;

	// Token: 0x040079DA RID: 31194
	private FacadeSelectionPanel.ConfigType currentConfigType;

	// Token: 0x040079DB RID: 31195
	private string _selectedFacade;

	// Token: 0x040079DC RID: 31196
	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	// Token: 0x02001D20 RID: 7456
	private struct FacadeToggle
	{
		// Token: 0x06009BCB RID: 39883 RVA: 0x003CE658 File Offset: 0x003CC858
		public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
		{
			this.id = buildingFacadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(false);
			component.GetReference<Image>("FGImage").SetAlpha(1f);
			Sprite sprite;
			string simpleTooltip;
			string dlcId;
			if (buildingFacadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(buildingFacadeID);
				sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
				simpleTooltip = KleiItemsUI.GetTooltipStringFor(buildingFacadeResource);
				dlcId = buildingFacadeResource.GetDlcIdFrom();
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(buildingPrefabID);
				Building component2 = prefab.GetComponent<Building>();
				StringEntry entry;
				string text;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".NAME"
				}), out entry))
				{
					text = entry;
				}
				else if (component2 != null)
				{
					text = component2.Def.Name;
				}
				else
				{
					text = prefab.GetProperName();
				}
				StringEntry entry2;
				string str;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".DESC"
				}), out entry2))
				{
					str = entry2;
				}
				else if (component2 != null)
				{
					str = component2.Def.Desc;
				}
				else
				{
					str = "";
				}
				sprite = Def.GetUISprite(buildingPrefabID, "ui", false).first;
				simpleTooltip = KleiItemsUI.WrapAsToolTipTitle(text) + "\n" + str;
				dlcId = null;
			}
			component.GetReference<Image>("FGImage").sprite = sprite;
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
			Image reference = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference.gameObject.SetActive(true);
				reference.color = DlcManager.GetDlcBannerColor(dlcId);
				return;
			}
			reference.gameObject.SetActive(false);
		}

		// Token: 0x06009BCC RID: 39884 RVA: 0x003CE884 File Offset: 0x003CCA84
		public FacadeToggle(string outfitID, GameObject gameObject, ClothingOutfitUtility.OutfitType outfitType)
		{
			this.id = outfitID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			UIMannequin reference = component.GetReference<UIMannequin>("Mannequin");
			reference.gameObject.SetActive(true);
			component.GetReference<Image>("FGImage").SetAlpha(0f);
			ToolTip component2 = this.gameObject.GetComponent<ToolTip>();
			component2.SetSimpleTooltip("");
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget outfit = ClothingOutfitTarget.FromTemplateId(outfitID);
				component.GetReference<UIMannequin>("Mannequin").SetOutfit(outfit);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(outfit.ReadName()));
			}
			else
			{
				component.GetReference<UIMannequin>("Mannequin").ClearOutfit(outfitType);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(UI.OUTFIT_NAME.NONE));
			}
			string dlcId = null;
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget.Implementation impl = ClothingOutfitTarget.FromTemplateId(outfitID).impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					dlcId = databaseAuthoredTemplate.resource.GetDlcIdFrom();
				}
			}
			Image reference2 = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(false);
			}
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-16f, -16f);
			}
			reference.rectTransform().sizeDelta = sizeDelta;
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06009BCD RID: 39885 RVA: 0x00109DD2 File Offset: 0x00107FD2
		// (set) Token: 0x06009BCE RID: 39886 RVA: 0x00109DDA File Offset: 0x00107FDA
		public string id { readonly get; set; }

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06009BCF RID: 39887 RVA: 0x00109DE3 File Offset: 0x00107FE3
		// (set) Token: 0x06009BD0 RID: 39888 RVA: 0x00109DEB File Offset: 0x00107FEB
		public GameObject gameObject { readonly get; set; }

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06009BD1 RID: 39889 RVA: 0x00109DF4 File Offset: 0x00107FF4
		// (set) Token: 0x06009BD2 RID: 39890 RVA: 0x00109DFC File Offset: 0x00107FFC
		public MultiToggle multiToggle { readonly get; set; }
	}

	// Token: 0x02001D21 RID: 7457
	private enum ConfigType
	{
		// Token: 0x040079E1 RID: 31201
		BuildingFacade,
		// Token: 0x040079E2 RID: 31202
		MinionOutfit
	}
}
