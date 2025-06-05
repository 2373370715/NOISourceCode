using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002027 RID: 8231
public class SelectModuleSideScreen : KScreen
{
	// Token: 0x0600AE56 RID: 44630 RVA: 0x00114927 File Offset: 0x00112B27
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x0600AE57 RID: 44631 RVA: 0x00115D16 File Offset: 0x00113F16
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SelectModuleSideScreen.Instance = this;
		this.SpawnButtons(null);
		this.buildSelectedModuleButton.onClick += this.OnClickBuildSelectedModule;
	}

	// Token: 0x0600AE58 RID: 44632 RVA: 0x00115D42 File Offset: 0x00113F42
	protected override void OnForcedCleanUp()
	{
		SelectModuleSideScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600AE59 RID: 44633 RVA: 0x00115D50 File Offset: 0x00113F50
	protected override void OnCmpDisable()
	{
		this.ClearSubscriptionHandles();
		this.module = null;
		base.OnCmpDisable();
	}

	// Token: 0x0600AE5A RID: 44634 RVA: 0x00425FA4 File Offset: 0x004241A4
	private void ClearSubscriptionHandles()
	{
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x0600AE5B RID: 44635 RVA: 0x00426008 File Offset: 0x00424208
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.ClearSubscriptionHandles();
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateBuildableStates)));
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.UpdateBuildableStates)));
	}

	// Token: 0x0600AE5C RID: 44636 RVA: 0x00426070 File Offset: 0x00424270
	protected override void OnCleanUp()
	{
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
		base.OnCleanUp();
	}

	// Token: 0x0600AE5D RID: 44637 RVA: 0x004260D8 File Offset: 0x004242D8
	public void SetLaunchPad(LaunchPad pad)
	{
		this.launchPad = pad;
		this.module = null;
		this.UpdateBuildableStates(null);
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			this.SetupBuildingTooltip(keyValuePair.Value.GetComponent<ToolTip>(), keyValuePair.Key);
		}
	}

	// Token: 0x0600AE5E RID: 44638 RVA: 0x00426154 File Offset: 0x00424354
	public void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.module = new_target.GetComponent<RocketModuleCluster>();
		if (this.module == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RocketModuleCluster component");
			return;
		}
		this.launchPad = null;
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			this.SetupBuildingTooltip(keyValuePair.Value.GetComponent<ToolTip>(), keyValuePair.Key);
		}
		this.UpdateBuildableStates(null);
		this.buildSelectedModuleButton.isInteractable = false;
		if (this.selectedModuleDef != null)
		{
			this.SelectModule(this.selectedModuleDef);
		}
	}

	// Token: 0x0600AE5F RID: 44639 RVA: 0x00426228 File Offset: 0x00424428
	private void UpdateBuildableStates(object data = null)
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			if (!this.moduleBuildableState.ContainsKey(keyValuePair.Key))
			{
				this.moduleBuildableState.Add(keyValuePair.Key, false);
			}
			TechItem techItem = Db.Get().TechItems.TryGet(keyValuePair.Key.PrefabID);
			if (techItem != null)
			{
				bool active = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem.IsComplete();
				keyValuePair.Value.SetActive(active);
			}
			else
			{
				keyValuePair.Value.SetActive(true);
			}
			this.moduleBuildableState[keyValuePair.Key] = this.TestBuildable(keyValuePair.Key);
		}
		if (this.selectedModuleDef != null)
		{
			this.ConfigureMaterialSelector();
		}
		this.SetButtonColors();
	}

	// Token: 0x0600AE60 RID: 44640 RVA: 0x00115D65 File Offset: 0x00113F65
	private void OnClickBuildSelectedModule()
	{
		if (this.selectedModuleDef != null)
		{
			this.OrderBuildSelectedModule();
		}
	}

	// Token: 0x0600AE61 RID: 44641 RVA: 0x00426334 File Offset: 0x00424534
	private void ConfigureMaterialSelector()
	{
		this.buildSelectedModuleButton.isInteractable = false;
		if (this.materialSelectionPanel == null)
		{
			this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, true);
			this.materialSelectionPanel.transform.SetSiblingIndex(this.buildSelectedModuleButton.transform.GetSiblingIndex());
		}
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(this.selectedModuleDef.CraftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(this.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(this.GetErrorTooltips));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.UpdateBuildButton));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
	}

	// Token: 0x0600AE62 RID: 44642 RVA: 0x00426400 File Offset: 0x00424600
	private void ConfigureFacadeSelector()
	{
		if (this.facadeSelectionPanel == null)
		{
			this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab, base.gameObject, true);
			this.facadeSelectionPanel.transform.SetSiblingIndex(this.materialSelectionPanel.transform.GetSiblingIndex());
		}
		this.facadeSelectionPanel.SetBuildingDef(this.selectedModuleDef.PrefabID, null);
	}

	// Token: 0x0600AE63 RID: 44643 RVA: 0x00115D7B File Offset: 0x00113F7B
	private bool IsDefBuildable(BuildingDef def)
	{
		return this.moduleBuildableState.ContainsKey(def) && this.moduleBuildableState[def];
	}

	// Token: 0x0600AE64 RID: 44644 RVA: 0x0042646C File Offset: 0x0042466C
	private void UpdateBuildButton()
	{
		this.buildSelectedModuleButton.isInteractable = (this.materialSelectionPanel != null && this.materialSelectionPanel.AllSelectorsSelected() && this.selectedModuleDef != null && this.moduleBuildableState[this.selectedModuleDef]);
	}

	// Token: 0x0600AE65 RID: 44645 RVA: 0x004264C4 File Offset: 0x004246C4
	public void SetButtonColors()
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			MultiToggle component = keyValuePair.Value.GetComponent<MultiToggle>();
			HierarchyReferences component2 = keyValuePair.Value.GetComponent<HierarchyReferences>();
			if (!this.moduleBuildableState[keyValuePair.Key])
			{
				component2.GetReference<Image>("FG").material = PlanScreen.Instance.desaturatedUIMaterial;
				if (keyValuePair.Key == this.selectedModuleDef)
				{
					component.ChangeState(1);
				}
				else
				{
					component.ChangeState(0);
				}
			}
			else
			{
				component2.GetReference<Image>("FG").material = PlanScreen.Instance.defaultUIMaterial;
				if (keyValuePair.Key == this.selectedModuleDef)
				{
					component.ChangeState(3);
				}
				else
				{
					component.ChangeState(2);
				}
			}
		}
		this.UpdateBuildButton();
	}

	// Token: 0x0600AE66 RID: 44646 RVA: 0x004265C8 File Offset: 0x004247C8
	private bool TestBuildable(BuildingDef def)
	{
		GameObject buildingComplete = def.BuildingComplete;
		SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
		if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleAbove && this.module != null)
		{
			BuildingAttachPoint component = this.module.GetComponent<BuildingAttachPoint>();
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells, null))
			{
				return false;
			}
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && !this.module.GetComponent<ReorderableBuilding>().CanMoveVertically(def.HeightInCells, null))
		{
			return false;
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && this.module != null && def != null && this.module.GetComponent<Building>().Def == def)
		{
			return false;
		}
		foreach (SelectModuleCondition selectModuleCondition in buildingComplete.GetComponent<ReorderableBuilding>().buildConditions)
		{
			if ((!selectModuleCondition.IgnoreInSanboxMode() || (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)) && !selectModuleCondition.EvaluateCondition((this.module == null) ? this.launchPad.gameObject : this.module.gameObject, def, selectionContext))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600AE67 RID: 44647 RVA: 0x00426758 File Offset: 0x00424958
	private void ClearButtons()
	{
		foreach (KeyValuePair<BuildingDef, GameObject> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		for (int i = this.categories.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.categories[i]);
		}
		this.categories.Clear();
		this.buttons.Clear();
	}

	// Token: 0x0600AE68 RID: 44648 RVA: 0x004267F0 File Offset: 0x004249F0
	public void SpawnButtons(object data = null)
	{
		this.ClearButtons();
		GameObject gameObject = Util.KInstantiateUI(this.categoryPrefab, this.categoryContent, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		this.categories.Add(gameObject);
		component.GetReference<LocText>("label");
		Transform reference = component.GetReference<Transform>("content");
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<RocketModuleCluster>();
		using (List<string>.Enumerator enumerator = SelectModuleSideScreen.moduleButtonSortOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelectModuleSideScreen.<>c__DisplayClass42_0 CS$<>8__locals1 = new SelectModuleSideScreen.<>c__DisplayClass42_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.id = enumerator.Current;
				GameObject part = prefabsWithComponent.Find((GameObject p) => p.PrefabID().Name == CS$<>8__locals1.id);
				if (part == null)
				{
					global::Debug.LogWarning("Found an id [" + CS$<>8__locals1.id + "] in moduleButtonSortOrder in SelectModuleSideScreen.cs that doesn't have a corresponding rocket part!");
				}
				else
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.moduleButtonPrefab, reference.gameObject, true);
					gameObject2.GetComponentsInChildren<Image>()[1].sprite = Def.GetUISprite(part, "ui", false).first;
					LocText componentInChildren = gameObject2.GetComponentInChildren<LocText>();
					componentInChildren.text = part.GetProperName();
					componentInChildren.alignment = TextAlignmentOptions.Bottom;
					componentInChildren.enableWordWrapping = true;
					MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
					component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.SelectModule(part.GetComponent<Building>().Def);
					}));
					this.buttons.Add(part.GetComponent<Building>().Def, gameObject2);
					if (this.selectedModuleDef != null)
					{
						this.SelectModule(this.selectedModuleDef);
					}
				}
			}
		}
		this.UpdateBuildableStates(null);
	}

	// Token: 0x0600AE69 RID: 44649 RVA: 0x004269D8 File Offset: 0x00424BD8
	private void SetupBuildingTooltip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		string name = def.Name;
		string text = def.Effect;
		RocketModuleCluster component = def.BuildingComplete.GetComponent<RocketModuleCluster>();
		BuildingDef buildingDef = (this.GetSelectionContext(def) == SelectModuleCondition.SelectionContext.ReplaceModule) ? this.module.GetComponent<Building>().Def : null;
		if (component != null)
		{
			text = text + "\n\n" + UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.TITLE;
			float burden = component.performanceStats.burden;
			float fuelKilogramPerDistance = component.performanceStats.FuelKilogramPerDistance;
			float enginePower = component.performanceStats.enginePower;
			int heightInCells = component.GetComponent<Building>().Def.HeightInCells;
			CraftModuleInterface craftModuleInterface = null;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			int num4 = 0;
			if (base.GetComponentInParent<DetailsScreen>() != null && base.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>() != null)
			{
				craftModuleInterface = base.GetComponentInParent<DetailsScreen>().target.GetComponent<RocketModuleCluster>().CraftInterface;
			}
			int num5 = -1;
			if (craftModuleInterface != null)
			{
				num5 = craftModuleInterface.MaxHeight;
			}
			RocketEngineCluster component2 = component.GetComponent<RocketEngineCluster>();
			if (component2 != null)
			{
				num5 = component2.maxHeight;
			}
			float num6;
			float num7;
			if (craftModuleInterface == null)
			{
				num = burden;
				num2 = fuelKilogramPerDistance;
				num3 = enginePower;
				num6 = num3 / num;
				num7 = num6;
				num4 = heightInCells;
			}
			else
			{
				if (buildingDef != null)
				{
					RocketModulePerformance performanceStats = this.module.GetComponent<RocketModuleCluster>().performanceStats;
					num -= performanceStats.burden;
					num2 -= performanceStats.fuelKilogramPerDistance;
					num3 -= performanceStats.enginePower;
					num4 -= buildingDef.HeightInCells;
				}
				num = burden + craftModuleInterface.TotalBurden;
				num2 = fuelKilogramPerDistance + craftModuleInterface.Range;
				num3 = component.performanceStats.enginePower + craftModuleInterface.EnginePower;
				num6 = (component.performanceStats.enginePower + craftModuleInterface.EnginePower) / num;
				num7 = num6 - craftModuleInterface.EnginePower / craftModuleInterface.TotalBurden;
				num4 = craftModuleInterface.RocketHeight + heightInCells;
			}
			string arg = (burden >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, burden), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, burden);
			string arg2 = (fuelKilogramPerDistance >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, Math.Round((double)fuelKilogramPerDistance, 2)), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, Math.Round((double)fuelKilogramPerDistance, 2));
			string arg3 = (enginePower >= 0f) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, enginePower), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, enginePower);
			string arg4 = (num7 >= num6) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, Math.Round((double)num7, 3)), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, Math.Round((double)num7, 2));
			string arg5 = (heightInCells >= 0) ? GameUtil.AddPositiveSign(string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.POSITIVEDELTA, heightInCells), true) : string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.NEGATIVEDELTA, heightInCells);
			if (num5 != -1)
			{
				text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT, num4, arg5, num5);
			}
			else
			{
				text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.HEIGHT_NOMAX, num4, arg5);
			}
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.BURDEN, num, arg);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.RANGE, Math.Round((double)num2, 2), arg2);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.ENGINEPOWER, num3, arg3);
			text = text + "\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.MODULESTATCHANGE.SPEED, Math.Round((double)num6, 3), arg4);
			if (component.GetComponent<RocketEngineCluster>() != null)
			{
				text = text + "\n\n" + string.Format(UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ENGINE_MAX_HEIGHT, num5);
			}
		}
		tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(text, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
		this.AddErrorTooltips(tooltip, def, false);
	}

	// Token: 0x0600AE6A RID: 44650 RVA: 0x00426E90 File Offset: 0x00425090
	private SelectModuleCondition.SelectionContext GetSelectionContext(BuildingDef def)
	{
		SelectModuleCondition.SelectionContext result = SelectModuleCondition.SelectionContext.AddModuleAbove;
		if (this.launchPad == null)
		{
			if (!this.addingNewModule)
			{
				result = SelectModuleCondition.SelectionContext.ReplaceModule;
			}
			else
			{
				List<SelectModuleCondition> buildConditions = Assets.GetPrefab(this.module.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions;
				ReorderableBuilding component = def.BuildingComplete.GetComponent<ReorderableBuilding>();
				if (buildConditions.Find((SelectModuleCondition match) => match is TopOnly) == null)
				{
					if (component.buildConditions.Find((SelectModuleCondition match) => match is EngineOnBottom) == null)
					{
						return result;
					}
				}
				result = SelectModuleCondition.SelectionContext.AddModuleBelow;
			}
		}
		return result;
	}

	// Token: 0x0600AE6B RID: 44651 RVA: 0x00426F3C File Offset: 0x0042513C
	private string GetErrorTooltips(BuildingDef def)
	{
		List<SelectModuleCondition> buildConditions = def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions;
		SelectModuleCondition.SelectionContext selectionContext = this.GetSelectionContext(def);
		string text = "";
		for (int i = 0; i < buildConditions.Count; i++)
		{
			if (!buildConditions[i].IgnoreInSanboxMode() || (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive))
			{
				GameObject gameObject = (this.module == null) ? this.launchPad.gameObject : this.module.gameObject;
				if (!buildConditions[i].EvaluateCondition(gameObject, def, selectionContext))
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "\n";
					}
					text += buildConditions[i].GetStatusTooltip(false, gameObject, def);
				}
			}
		}
		return text;
	}

	// Token: 0x0600AE6C RID: 44652 RVA: 0x00427008 File Offset: 0x00425208
	private void AddErrorTooltips(ToolTip tooltip, BuildingDef def, bool clearFirst = false)
	{
		if (clearFirst)
		{
			tooltip.ClearMultiStringTooltip();
		}
		if (!clearFirst)
		{
			tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
		}
		tooltip.AddMultiStringTooltip(this.GetErrorTooltips(def), PlanScreen.Instance.buildingToolTipSettings.MaterialRequirement);
	}

	// Token: 0x0600AE6D RID: 44653 RVA: 0x00115D99 File Offset: 0x00113F99
	public void SelectModule(BuildingDef def)
	{
		this.selectedModuleDef = def;
		this.ConfigureMaterialSelector();
		this.ConfigureFacadeSelector();
		this.SetButtonColors();
		this.UpdateBuildButton();
		this.AddErrorTooltips(this.buildSelectedModuleButton.GetComponent<ToolTip>(), this.selectedModuleDef, true);
	}

	// Token: 0x0600AE6E RID: 44654 RVA: 0x0042705C File Offset: 0x0042525C
	private void OrderBuildSelectedModule()
	{
		BuildingDef previousSelectedDef = this.selectedModuleDef;
		GameObject gameObject2;
		if (this.module != null)
		{
			GameObject gameObject = this.module.gameObject;
			if (this.addingNewModule)
			{
				gameObject2 = this.module.GetComponent<ReorderableBuilding>().AddModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
			}
			else
			{
				gameObject2 = this.module.GetComponent<ReorderableBuilding>().ConvertModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
			}
		}
		else
		{
			gameObject2 = this.launchPad.AddBaseModule(this.selectedModuleDef, this.materialSelectionPanel.GetSelectedElementAsList);
		}
		if (gameObject2 != null)
		{
			Vector2 anchoredPosition = this.mainContents.GetComponent<KScrollRect>().content.anchoredPosition;
			if (this.facadeSelectionPanel.SelectedFacade != null && this.facadeSelectionPanel.SelectedFacade != "DEFAULT_FACADE")
			{
				gameObject2.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeSelectionPanel.SelectedFacade), false);
			}
			SelectTool.Instance.StartCoroutine(this.SelectNextFrame(gameObject2.GetComponent<KSelectable>(), previousSelectedDef, anchoredPosition.y));
		}
	}

	// Token: 0x0600AE6F RID: 44655 RVA: 0x00115DD2 File Offset: 0x00113FD2
	private IEnumerator SelectNextFrame(KSelectable selectable, BuildingDef previousSelectedDef, float scrollPosition)
	{
		yield return 0;
		SelectTool.Instance.Select(selectable, false);
		RocketModuleSideScreen.instance.ClickAddNew(scrollPosition, previousSelectedDef);
		yield break;
	}

	// Token: 0x0400892C RID: 35116
	public RocketModule module;

	// Token: 0x0400892D RID: 35117
	private LaunchPad launchPad;

	// Token: 0x0400892E RID: 35118
	public GameObject mainContents;

	// Token: 0x0400892F RID: 35119
	[Header("Category")]
	public GameObject categoryPrefab;

	// Token: 0x04008930 RID: 35120
	public GameObject moduleButtonPrefab;

	// Token: 0x04008931 RID: 35121
	public GameObject categoryContent;

	// Token: 0x04008932 RID: 35122
	private BuildingDef selectedModuleDef;

	// Token: 0x04008933 RID: 35123
	public List<GameObject> categories = new List<GameObject>();

	// Token: 0x04008934 RID: 35124
	public Dictionary<BuildingDef, GameObject> buttons = new Dictionary<BuildingDef, GameObject>();

	// Token: 0x04008935 RID: 35125
	private Dictionary<BuildingDef, bool> moduleBuildableState = new Dictionary<BuildingDef, bool>();

	// Token: 0x04008936 RID: 35126
	public static SelectModuleSideScreen Instance;

	// Token: 0x04008937 RID: 35127
	public bool addingNewModule;

	// Token: 0x04008938 RID: 35128
	public GameObject materialSelectionPanelPrefab;

	// Token: 0x04008939 RID: 35129
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x0400893A RID: 35130
	public GameObject facadeSelectionPanelPrefab;

	// Token: 0x0400893B RID: 35131
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x0400893C RID: 35132
	public KButton buildSelectedModuleButton;

	// Token: 0x0400893D RID: 35133
	public ColorStyleSetting colorStyleButton;

	// Token: 0x0400893E RID: 35134
	public ColorStyleSetting colorStyleButtonSelected;

	// Token: 0x0400893F RID: 35135
	public ColorStyleSetting colorStyleButtonInactive;

	// Token: 0x04008940 RID: 35136
	public ColorStyleSetting colorStyleButtonInactiveSelected;

	// Token: 0x04008941 RID: 35137
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x04008942 RID: 35138
	public static List<string> moduleButtonSortOrder = new List<string>
	{
		"CO2Engine",
		"SugarEngine",
		"SteamEngineCluster",
		"KeroseneEngineClusterSmall",
		"KeroseneEngineCluster",
		"HEPEngine",
		"HydrogenEngineCluster",
		"HabitatModuleSmall",
		"HabitatModuleMedium",
		"RoboPilotModule",
		"NoseconeBasic",
		"NoseconeHarvest",
		"OrbitalCargoModule",
		"ScoutModule",
		"PioneerModule",
		"LiquidFuelTankCluster",
		"SmallOxidizerTank",
		"OxidizerTankCluster",
		"OxidizerTankLiquidCluster",
		"SolidCargoBaySmall",
		"LiquidCargoBaySmall",
		"GasCargoBaySmall",
		"CargoBayCluster",
		"LiquidCargoBayCluster",
		"GasCargoBayCluster",
		"SpecialCargoBayCluster",
		"BatteryModule",
		"SolarPanelModule",
		"ArtifactCargoBay",
		"ScannerModule"
	};
}
