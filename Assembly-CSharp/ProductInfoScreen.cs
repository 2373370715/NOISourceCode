using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F0F RID: 7951
public class ProductInfoScreen : KScreen
{
	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x0600A720 RID: 42784 RVA: 0x00110E13 File Offset: 0x0010F013
	public FacadeSelectionPanel FacadeSelectionPanel
	{
		get
		{
			return this.facadeSelectionPanel;
		}
	}

	// Token: 0x0600A721 RID: 42785 RVA: 0x00110E1B File Offset: 0x0010F01B
	private void RefreshScreen()
	{
		if (this.currentDef != null)
		{
			this.SetTitle(this.currentDef);
			return;
		}
		this.ClearProduct(true);
	}

	// Token: 0x0600A722 RID: 42786 RVA: 0x00403360 File Offset: 0x00401560
	public void ClearProduct(bool deactivateTool = true)
	{
		if (this.materialSelectionPanel == null)
		{
			return;
		}
		this.currentDef = null;
		this.materialSelectionPanel.ClearMaterialToggles();
		if (PlayerController.Instance.ActiveTool == BuildTool.Instance && deactivateTool)
		{
			BuildTool.Instance.Deactivate();
		}
		if (PlayerController.Instance.ActiveTool == UtilityBuildTool.Instance || PlayerController.Instance.ActiveTool == WireBuildTool.Instance)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.ClearLabels();
		this.Show(false);
	}

	// Token: 0x0600A723 RID: 42787 RVA: 0x004033F4 File Offset: 0x004015F4
	public new void Awake()
	{
		base.Awake();
		this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab.gameObject, base.gameObject, false);
		FacadeSelectionPanel facadeSelectionPanel = this.facadeSelectionPanel;
		facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(this.OnFacadeSelectionChanged));
		this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x0600A724 RID: 42788 RVA: 0x00403468 File Offset: 0x00401668
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen instance = BuildingGroupScreen.Instance;
			instance.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
			instance2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance2.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (PlanScreen.Instance != null)
		{
			PlanScreen instance3 = PlanScreen.Instance;
			instance3.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance3.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			PlanScreen instance4 = PlanScreen.Instance;
			instance4.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance4.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu instance5 = BuildMenu.Instance;
			instance5.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance5.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildMenu instance6 = BuildMenu.Instance;
			instance6.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance6.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		base.ConsumeMouseScroll = true;
		this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		MultiToggle multiToggle = this.sandboxInstantBuildToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
			this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		}));
		this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		});
	}

	// Token: 0x0600A725 RID: 42789 RVA: 0x00110E3F File Offset: 0x0010F03F
	public void ConfigureScreen(BuildingDef def)
	{
		this.ConfigureScreen(def, this.FacadeSelectionPanel.SelectedFacade);
	}

	// Token: 0x0600A726 RID: 42790 RVA: 0x00403650 File Offset: 0x00401850
	public void ConfigureScreen(BuildingDef def, string facadeID)
	{
		this.configuring = true;
		this.currentDef = def;
		this.SetTitle(def);
		this.SetDescription(def);
		this.SetEffects(def);
		this.facadeSelectionPanel.SetBuildingDef(def.PrefabID, null);
		BuildingFacadeResource buildingFacadeResource = null;
		if ("DEFAULT_FACADE" != facadeID)
		{
			buildingFacadeResource = Db.GetBuildingFacades().TryGet(facadeID);
		}
		if (buildingFacadeResource != null && buildingFacadeResource.PrefabID == def.PrefabID && buildingFacadeResource.IsUnlocked())
		{
			this.facadeSelectionPanel.SelectedFacade = facadeID;
		}
		else
		{
			this.facadeSelectionPanel.SelectedFacade = "DEFAULT_FACADE";
		}
		this.SetMaterials(def);
		this.configuring = false;
	}

	// Token: 0x0600A727 RID: 42791 RVA: 0x00110E53 File Offset: 0x0010F053
	private void ExpandInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(true);
	}

	// Token: 0x0600A728 RID: 42792 RVA: 0x00110E5C File Offset: 0x0010F05C
	private void CollapseInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(false);
	}

	// Token: 0x0600A729 RID: 42793 RVA: 0x004036F8 File Offset: 0x004018F8
	public void ToggleExpandedInfo(bool state)
	{
		this.expandedInfo = state;
		if (this.ProductDescriptionPane != null)
		{
			this.ProductDescriptionPane.SetActive(this.expandedInfo);
		}
		if (this.ProductRequirementsPane != null)
		{
			this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
		}
		if (this.RoomConstrainsPanel != null)
		{
			this.RoomConstrainsPanel.gameObject.SetActive(this.expandedInfo && this.RoomConstrainsPanel.HasDescriptors());
		}
		if (this.ProductEffectsPane != null)
		{
			this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
		}
		if (this.ProductFlavourPane != null)
		{
			this.ProductFlavourPane.SetActive(this.expandedInfo);
		}
		if (this.materialSelectionPanel != null && this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
		}
	}

	// Token: 0x0600A72A RID: 42794 RVA: 0x00403820 File Offset: 0x00401A20
	private void CheckMouseOver(PointerEventData data)
	{
		bool state = base.GetMouseOver || (PlanScreen.Instance != null && ((PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.GetMouseOver) || BuildingGroupScreen.Instance.GetMouseOver)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.GetMouseOver);
		this.ToggleExpandedInfo(state);
	}

	// Token: 0x0600A72B RID: 42795 RVA: 0x00403898 File Offset: 0x00401A98
	private void Update()
	{
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && this.currentDef != null && this.materialSelectionPanel.CurrentSelectedElement != null && !MaterialSelector.AllowInsufficientMaterialBuild() && this.currentDef.Mass[0] > ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.materialSelectionPanel.CurrentSelectedElement, true))
		{
			this.materialSelectionPanel.AutoSelectAvailableMaterial();
		}
	}

	// Token: 0x0600A72C RID: 42796 RVA: 0x00403920 File Offset: 0x00401B20
	private void SetTitle(BuildingDef def)
	{
		this.titleBar.SetTitle(def.Name);
		bool flag = (PlanScreen.Instance != null && PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.IsDefBuildable(def)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete);
		this.titleBar.GetComponentInChildren<KImage>().ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Disabled);
	}

	// Token: 0x0600A72D RID: 42797 RVA: 0x004039AC File Offset: 0x00401BAC
	private void SetDescription(BuildingDef def)
	{
		if (def == null)
		{
			return;
		}
		if (this.productFlavourText == null)
		{
			return;
		}
		string text = "";
		text += def.Desc;
		Dictionary<Klei.AI.Attribute, float> dictionary = new Dictionary<Klei.AI.Attribute, float>();
		Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
		foreach (Klei.AI.Attribute key in def.attributes)
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary[key] = 0f;
			}
		}
		foreach (AttributeModifier attributeModifier in def.attributeModifiers)
		{
			float num = 0f;
			Klei.AI.Attribute key2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			dictionary.TryGetValue(key2, out num);
			num += attributeModifier.Value;
			dictionary[key2] = num;
		}
		if (this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
			if (element != null)
			{
				using (List<AttributeModifier>.Enumerator enumerator2 = element.attributeModifiers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AttributeModifier attributeModifier2 = enumerator2.Current;
						float num2 = 0f;
						Klei.AI.Attribute key3 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId);
						dictionary2.TryGetValue(key3, out num2);
						num2 += attributeModifier2.Value;
						dictionary2[key3] = num2;
					}
					goto IL_229;
				}
			}
			PrefabAttributeModifiers component = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
			if (component != null)
			{
				foreach (AttributeModifier attributeModifier3 in component.descriptors)
				{
					float num3 = 0f;
					Klei.AI.Attribute key4 = Db.Get().BuildingAttributes.Get(attributeModifier3.AttributeId);
					dictionary2.TryGetValue(key4, out num3);
					num3 += attributeModifier3.Value;
					dictionary2[key4] = num3;
				}
			}
		}
		IL_229:
		if (dictionary.Count > 0)
		{
			text += "\n\n";
			foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary)
			{
				float num4 = 0f;
				dictionary.TryGetValue(keyValuePair.Key, out num4);
				float num5 = 0f;
				string text2 = "";
				if (dictionary2.TryGetValue(keyValuePair.Key, out num5))
				{
					num5 = Mathf.Abs(num4 * num5);
					text2 = "(+" + num5.ToString() + ")";
				}
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key.Name,
					": ",
					(num4 + num5).ToString(),
					text2
				});
			}
		}
		this.productFlavourText.text = text;
	}

	// Token: 0x0600A72E RID: 42798 RVA: 0x00403D18 File Offset: 0x00401F18
	private void SetEffects(BuildingDef def)
	{
		if (this.productDescriptionText.text != null)
		{
			this.productDescriptionText.text = string.Format("{0}", def.Effect);
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		List<Descriptor> list = new List<Descriptor>();
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS, Descriptor.DescriptorType.Effect);
			requirementDescriptors.Insert(0, item);
			this.ProductRequirementsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductRequirementsPane.gameObject.SetActive(false);
		}
		this.ProductRequirementsPane.SetDescriptors(requirementDescriptors);
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONEFFECTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS, Descriptor.DescriptorType.Effect);
			effectDescriptors.Insert(0, item2);
			this.ProductEffectsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductEffectsPane.gameObject.SetActive(false);
		}
		this.ProductEffectsPane.SetDescriptors(effectDescriptors);
		foreach (Tag tag in def.BuildingComplete.GetComponent<KPrefabID>().Tags)
		{
			if (RoomConstraints.ConstraintTags.AllTags.Contains(tag) && !this.HiddenRoomConstrainTags.Contains(tag))
			{
				Descriptor item3 = default(Descriptor);
				item3.SetupDescriptor(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag), null, Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		if (list.Count > 0)
		{
			list = GameUtil.GetEffectDescriptors(list);
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(CODEX.HEADERS.BUILDINGTYPE, UI.BUILDINGEFFECTS.TOOLTIPS.BUILDINGROOMREQUIREMENTCLASS, Descriptor.DescriptorType.Effect);
			list.Insert(0, item4);
			this.RoomConstrainsPanel.gameObject.SetActive(true);
		}
		else
		{
			this.RoomConstrainsPanel.gameObject.SetActive(false);
		}
		this.RoomConstrainsPanel.SetDescriptors(list);
	}

	// Token: 0x0600A72F RID: 42799 RVA: 0x00403F2C File Offset: 0x0040212C
	public void ClearLabels()
	{
		List<string> list = new List<string>(this.descLabels.Keys);
		if (list.Count > 0)
		{
			foreach (string key in list)
			{
				GameObject gameObject = this.descLabels[key];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				this.descLabels.Remove(key);
			}
		}
	}

	// Token: 0x0600A730 RID: 42800 RVA: 0x00403FB8 File Offset: 0x004021B8
	public void SetMaterials(BuildingDef def)
	{
		this.materialSelectionPanel.gameObject.SetActive(true);
		Recipe craftRecipe = def.CraftRecipe;
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(craftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(PlanScreen.Instance.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(PlanScreen.Instance.GetTooltipForBuildable));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
		this.ActivateAppropriateTool(def);
	}

	// Token: 0x0600A731 RID: 42801 RVA: 0x00110E65 File Offset: 0x0010F065
	private void OnFacadeSelectionChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
	}

	// Token: 0x0600A732 RID: 42802 RVA: 0x00110E82 File Offset: 0x0010F082
	private void onMenuMaterialChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
		this.SetDescription(this.currentDef);
	}

	// Token: 0x0600A733 RID: 42803 RVA: 0x00404064 File Offset: 0x00402264
	private void ActivateAppropriateTool(BuildingDef def)
	{
		global::Debug.Assert(def != null, "def was null");
		if (((PlanScreen.Instance != null) ? PlanScreen.Instance.IsDefBuildable(def) : (BuildMenu.Instance != null && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete)) && this.materialSelectionPanel.AllSelectorsSelected() && this.facadeSelectionPanel.SelectedFacade != null)
		{
			this.onElementsFullySelected.Signal();
			return;
		}
		if (!MaterialSelector.AllowInsufficientMaterialBuild() && !DebugHandler.InstantBuildMode)
		{
			if (PlayerController.Instance.ActiveTool == BuildTool.Instance)
			{
				BuildTool.Instance.Deactivate();
			}
			PrebuildTool.Instance.Activate(def, PlanScreen.Instance.GetTooltipForBuildable(def));
		}
	}

	// Token: 0x0600A734 RID: 42804 RVA: 0x00404128 File Offset: 0x00402328
	public static bool MaterialsMet(Recipe recipe)
	{
		if (recipe == null)
		{
			global::Debug.LogError("Trying to verify the materials on a null recipe!");
			return false;
		}
		if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
		{
			global::Debug.LogError("Trying to verify the materials on a recipe with no MaterialCategoryTags!");
			return false;
		}
		bool result = true;
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			if (MaterialSelectionPanel.Filter(recipe.Ingredients[i].tag).kgAvailable < recipe.Ingredients[i].amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600A735 RID: 42805 RVA: 0x00110EAB File Offset: 0x0010F0AB
	public void Close()
	{
		if (this.configuring)
		{
			return;
		}
		this.ClearProduct(true);
		this.Show(false);
	}

	// Token: 0x0400831B RID: 33563
	public TitleBar titleBar;

	// Token: 0x0400831C RID: 33564
	public GameObject ProductDescriptionPane;

	// Token: 0x0400831D RID: 33565
	public LocText productDescriptionText;

	// Token: 0x0400831E RID: 33566
	public DescriptorPanel ProductRequirementsPane;

	// Token: 0x0400831F RID: 33567
	public DescriptorPanel ProductEffectsPane;

	// Token: 0x04008320 RID: 33568
	public DescriptorPanel RoomConstrainsPanel;

	// Token: 0x04008321 RID: 33569
	public GameObject ProductFlavourPane;

	// Token: 0x04008322 RID: 33570
	public LocText productFlavourText;

	// Token: 0x04008323 RID: 33571
	public RectTransform BGPanel;

	// Token: 0x04008324 RID: 33572
	public MaterialSelectionPanel materialSelectionPanelPrefab;

	// Token: 0x04008325 RID: 33573
	public FacadeSelectionPanel facadeSelectionPanelPrefab;

	// Token: 0x04008326 RID: 33574
	private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();

	// Token: 0x04008327 RID: 33575
	public MultiToggle sandboxInstantBuildToggle;

	// Token: 0x04008328 RID: 33576
	private List<Tag> HiddenRoomConstrainTags = new List<Tag>
	{
		RoomConstraints.ConstraintTags.Refrigerator,
		RoomConstraints.ConstraintTags.FarmStationType,
		RoomConstraints.ConstraintTags.LuxuryBedType,
		RoomConstraints.ConstraintTags.MassageTable,
		RoomConstraints.ConstraintTags.MessTable,
		RoomConstraints.ConstraintTags.NatureReserve,
		RoomConstraints.ConstraintTags.Park,
		RoomConstraints.ConstraintTags.SpiceStation,
		RoomConstraints.ConstraintTags.DeStressingBuilding,
		RoomConstraints.ConstraintTags.Decor20,
		RoomConstraints.ConstraintTags.MachineShopType
	};

	// Token: 0x04008329 RID: 33577
	[NonSerialized]
	public MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x0400832A RID: 33578
	[SerializeField]
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x0400832B RID: 33579
	[NonSerialized]
	public BuildingDef currentDef;

	// Token: 0x0400832C RID: 33580
	public System.Action onElementsFullySelected;

	// Token: 0x0400832D RID: 33581
	private bool expandedInfo = true;

	// Token: 0x0400832E RID: 33582
	private bool configuring;
}
