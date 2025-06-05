using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001E3D RID: 7741
public class MaterialSelectionPanel : KScreen, IRender200ms
{
	// Token: 0x0600A1E6 RID: 41446 RVA: 0x0010DB1C File Offset: 0x0010BD1C
	public static void ClearStatics()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
	}

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x0600A1E7 RID: 41447 RVA: 0x0010DB28 File Offset: 0x0010BD28
	public Tag CurrentSelectedElement
	{
		get
		{
			if (this.materialSelectors.Count == 0)
			{
				return null;
			}
			return this.materialSelectors[0].CurrentSelectedElement;
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x0600A1E8 RID: 41448 RVA: 0x003EC118 File Offset: 0x003EA318
	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			this.currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in this.materialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					global::Debug.Assert(materialSelector.CurrentSelectedElement != null);
					this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return this.currentSelectedElements;
		}
	}

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x0600A1E9 RID: 41449 RVA: 0x0010DB4F File Offset: 0x0010BD4F
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x0600A1EA RID: 41450 RVA: 0x003EC1AC File Offset: 0x003EA3AC
	protected override void OnPrefabInit()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, base.gameObject, false);
			materialSelector.selectorIndex = i;
			this.materialSelectors.Add(materialSelector);
		}
		this.materialSelectors[0].gameObject.SetActive(true);
		this.MaterialSelectorTemplate.SetActive(false);
		this.ToggleResearchRequired(false);
		if (this.priorityScreenParent != null)
		{
			this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
			this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
			this.priorityScreenParent.transform.SetAsLastSibling();
		}
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate(object d)
		{
			this.RefreshSelectors();
		}));
	}

	// Token: 0x0600A1EB RID: 41451 RVA: 0x0010DB57 File Offset: 0x0010BD57
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateOnSpawn = true;
	}

	// Token: 0x0600A1EC RID: 41452 RVA: 0x003EC2A8 File Offset: 0x003EA4A8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x0600A1ED RID: 41453 RVA: 0x003EC310 File Offset: 0x003EA510
	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	// Token: 0x0600A1EE RID: 41454 RVA: 0x0010DB66 File Offset: 0x0010BD66
	public void ClearSelectActions()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	// Token: 0x0600A1EF RID: 41455 RVA: 0x0010DB92 File Offset: 0x0010BD92
	public void ClearMaterialToggles()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	// Token: 0x0600A1F0 RID: 41456 RVA: 0x0010DBBE File Offset: 0x0010BDBE
	public void ConfigureScreen(Recipe recipe, MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB, MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
	{
		this.activeRecipe = recipe;
		this.GetBuildableState = buildableStateCB;
		this.GetBuildableTooltip = buildableTooltipCB;
		this.RefreshSelectors();
	}

	// Token: 0x0600A1F1 RID: 41457 RVA: 0x003EC344 File Offset: 0x003EA544
	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in this.materialSelectors)
		{
			flag = (flag || materialSelector.gameObject.activeInHierarchy);
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x0600A1F2 RID: 41458 RVA: 0x003EC3CC File Offset: 0x003EA5CC
	public void RefreshSelectors()
	{
		if (this.activeRecipe == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(false);
		});
		BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
		bool flag = this.GetBuildableState(buildingDef);
		string text = this.GetBuildableTooltip(buildingDef);
		if (!flag)
		{
			this.ToggleResearchRequired(true);
			LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(false);
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.ToggleResearchRequired(false);
			for (int i = 0; i < this.activeRecipe.Ingredients.Count; i++)
			{
				this.materialSelectors[i].gameObject.SetActive(true);
				this.materialSelectors[i].ConfigureScreen(this.activeRecipe.Ingredients[i], this.activeRecipe);
			}
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.transform.SetAsLastSibling();
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(true);
				this.buildToolRotateButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x0600A1F3 RID: 41459 RVA: 0x0010DBDB File Offset: 0x0010BDDB
	private void UpdateResourceToggleValues()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	// Token: 0x0600A1F4 RID: 41460 RVA: 0x0010DC15 File Offset: 0x0010BE15
	private void ToggleResearchRequired(bool state)
	{
		if (this.ResearchRequired == null)
		{
			return;
		}
		this.ResearchRequired.SetActive(state);
	}

	// Token: 0x0600A1F5 RID: 41461 RVA: 0x003EC578 File Offset: 0x003EA778
	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (!this.materialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x0600A1F6 RID: 41462 RVA: 0x003EC5B4 File Offset: 0x003EA7B4
	public void SelectSourcesMaterials(Building building)
	{
		Tag[] array = null;
		Deconstructable component = building.gameObject.GetComponent<Deconstructable>();
		if (component != null)
		{
			array = component.constructionElements;
		}
		Constructable component2 = building.GetComponent<Constructable>();
		if (component2 != null)
		{
			array = component2.SelectedElementsTags.ToArray<Tag>();
		}
		if (array != null)
		{
			for (int i = 0; i < Mathf.Min(array.Length, this.materialSelectors.Count); i++)
			{
				if (this.materialSelectors[i].ElementToggles.ContainsKey(array[i]))
				{
					this.materialSelectors[i].OnSelectMaterial(array[i], this.activeRecipe, false);
				}
			}
		}
	}

	// Token: 0x0600A1F7 RID: 41463 RVA: 0x0010DC32 File Offset: 0x0010BE32
	public void ForceSelectPrimaryTag(Tag tag)
	{
		this.materialSelectors[0].OnSelectMaterial(tag, this.activeRecipe, false);
	}

	// Token: 0x0600A1F8 RID: 41464 RVA: 0x003EC65C File Offset: 0x003EA85C
	public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag _materialCategoryTag)
	{
		MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = default(MaterialSelectionPanel.SelectedElemInfo);
		selectedElemInfo.element = null;
		selectedElemInfo.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return selectedElemInfo;
		}
		string[] array = _materialCategoryTag.ToString().Split('&', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			List<Tag> list = null;
			if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(tag, out list))
			{
				list = new List<Tag>();
				foreach (Element element in ElementLoader.elements)
				{
					if (element.tag == tag || element.HasTag(tag))
					{
						list.Add(element.tag);
					}
				}
				foreach (Tag tag2 in GameTags.MaterialBuildingElements)
				{
					if (tag2 == tag)
					{
						foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag2))
						{
							KPrefabID component = gameObject.GetComponent<KPrefabID>();
							if (component != null && !list.Contains(component.PrefabTag))
							{
								list.Add(component.PrefabTag);
							}
						}
					}
				}
				MaterialSelectionPanel.elementsWithTag[tag] = list;
			}
			foreach (Tag tag3 in list)
			{
				float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag3, true);
				if (amount > selectedElemInfo.kgAvailable)
				{
					selectedElemInfo.kgAvailable = amount;
					selectedElemInfo.element = tag3;
				}
			}
		}
		return selectedElemInfo;
	}

	// Token: 0x0600A1F9 RID: 41465 RVA: 0x003EC890 File Offset: 0x003EAA90
	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (this.materialSelectors[i] != null)
			{
				this.materialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	// Token: 0x0600A1FA RID: 41466 RVA: 0x0010DC4D File Offset: 0x0010BE4D
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x0600A1FB RID: 41467 RVA: 0x0010DC5C File Offset: 0x0010BE5C
	public void Render200ms(float dt)
	{
		this.UpdateResourceToggleValues();
	}

	// Token: 0x04007F02 RID: 32514
	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	// Token: 0x04007F03 RID: 32515
	private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();

	// Token: 0x04007F04 RID: 32516
	private List<Tag> currentSelectedElements = new List<Tag>();

	// Token: 0x04007F05 RID: 32517
	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	// Token: 0x04007F06 RID: 32518
	[SerializeField]
	protected GameObject priorityScreenParent;

	// Token: 0x04007F07 RID: 32519
	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	// Token: 0x04007F08 RID: 32520
	private PriorityScreen priorityScreen;

	// Token: 0x04007F09 RID: 32521
	public GameObject MaterialSelectorTemplate;

	// Token: 0x04007F0A RID: 32522
	public GameObject ResearchRequired;

	// Token: 0x04007F0B RID: 32523
	private Recipe activeRecipe;

	// Token: 0x04007F0C RID: 32524
	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	// Token: 0x04007F0D RID: 32525
	private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;

	// Token: 0x04007F0E RID: 32526
	private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;

	// Token: 0x04007F0F RID: 32527
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x02001E3E RID: 7742
	// (Invoke) Token: 0x0600A200 RID: 41472
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	// Token: 0x02001E3F RID: 7743
	// (Invoke) Token: 0x0600A204 RID: 41476
	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	// Token: 0x02001E40 RID: 7744
	// (Invoke) Token: 0x0600A208 RID: 41480
	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	// Token: 0x02001E41 RID: 7745
	public struct SelectedElemInfo
	{
		// Token: 0x04007F10 RID: 32528
		public Tag element;

		// Token: 0x04007F11 RID: 32529
		public float kgAvailable;
	}
}
