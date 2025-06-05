using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001AF4 RID: 6900
public class BuildMenuBuildingsScreen : KIconToggleMenu
{
	// Token: 0x06009052 RID: 36946 RVA: 0x000F0401 File Offset: 0x000EE601
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06009053 RID: 36947 RVA: 0x0038668C File Offset: 0x0038488C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateBuildableStates();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.onSelect += this.OnClickBuilding;
		Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
	}

	// Token: 0x06009054 RID: 36948 RVA: 0x003866F0 File Offset: 0x003848F0
	public void Configure(HashedString category, IList<BuildMenu.BuildingInfo> building_infos)
	{
		this.ClearButtons();
		this.SetHasFocus(true);
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		string text = HashCache.Get().Get(category).ToUpper();
		text = text.Replace(" ", "");
		this.titleLabel.text = Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".BUILDMENUTITLE");
		foreach (BuildMenu.BuildingInfo buildingInfo in building_infos)
		{
			BuildingDef def = Assets.GetBuildingDef(buildingInfo.id);
			if (def.ShouldShowInBuildMenu() && def.IsAvailable())
			{
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(def.Name, new BuildMenuBuildingsScreen.UserData(def, PlanScreen.RequirementsState.Tech), def.HotKey, () => def.GetUISprite("ui", false));
				list.Add(item);
			}
		}
		base.Setup(list);
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			this.RefreshToggle(this.toggleInfo[i]);
		}
		int num = 0;
		using (IEnumerator enumerator2 = this.gridSizer.transform.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((Transform)enumerator2.Current).gameObject.activeSelf)
				{
					num++;
				}
			}
		}
		this.gridSizer.constraintCount = Mathf.Min(num, 3);
		int num2 = Mathf.Min(num, this.gridSizer.constraintCount);
		int num3 = (num + this.gridSizer.constraintCount - 1) / this.gridSizer.constraintCount;
		int num4 = num2 - 1;
		int num5 = num3 - 1;
		Vector2 vector = new Vector2((float)num2 * this.gridSizer.cellSize.x + (float)num4 * this.gridSizer.spacing.x + (float)this.gridSizer.padding.left + (float)this.gridSizer.padding.right, (float)num3 * this.gridSizer.cellSize.y + (float)num5 * this.gridSizer.spacing.y + (float)this.gridSizer.padding.top + (float)this.gridSizer.padding.bottom);
		this.contentSizeLayout.minWidth = vector.x;
		this.contentSizeLayout.minHeight = vector.y;
	}

	// Token: 0x06009055 RID: 36949 RVA: 0x00102CEA File Offset: 0x00100EEA
	private void ConfigureToolTip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(def.Name, this.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(def.Effect, this.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x06009056 RID: 36950 RVA: 0x003869B0 File Offset: 0x00384BB0
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06009057 RID: 36951 RVA: 0x00386A18 File Offset: 0x00384C18
	private void RefreshToggle(KIconToggleMenu.ToggleInfo info)
	{
		if (info == null || info.toggle == null)
		{
			return;
		}
		BuildingDef def = (info.userData as BuildMenuBuildingsScreen.UserData).def;
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		bool flag = DebugHandler.InstantBuildMode || techItem == null || techItem.IsComplete();
		bool flag2 = flag || techItem == null || techItem.ParentTech.ArePrerequisitesComplete();
		KToggle toggle = info.toggle;
		if (toggle.gameObject.activeSelf != flag2)
		{
			toggle.gameObject.SetActive(flag2);
		}
		if (toggle.bgImage == null)
		{
			return;
		}
		Image image = toggle.bgImage.GetComponentsInChildren<Image>()[1];
		Sprite uisprite = def.GetUISprite("ui", false);
		image.sprite = uisprite;
		image.SetNativeSize();
		image.rectTransform().sizeDelta /= 4f;
		ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		string text = def.Name;
		string effect = def.Effect;
		if (def.HotKey != global::Action.NumActions)
		{
			text += GameUtil.GetHotkeyString(def.HotKey);
		}
		component.AddMultiStringTooltip(text, this.buildingToolTipSettings.BuildButtonName);
		component.AddMultiStringTooltip(effect, this.buildingToolTipSettings.BuildButtonDescription);
		LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = def.Name;
		}
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		int num = (requirementsState == PlanScreen.RequirementsState.Complete) ? 1 : 0;
		ImageToggleState.State state;
		if (def == this.selectedBuilding && (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode))
		{
			state = ImageToggleState.State.Active;
		}
		else
		{
			state = ((requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode) ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		if (def == this.selectedBuilding && state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.DisabledActive;
		}
		else if (state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.Disabled;
		}
		toggle.GetComponent<ImageToggleState>().SetState(state);
		Material material;
		Color color;
		if (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode)
		{
			material = this.defaultUIMaterial;
			color = Color.white;
		}
		else
		{
			material = this.desaturatedUIMaterial;
			Color color3;
			if (!flag)
			{
				Graphic graphic = image;
				Color color2 = new Color(1f, 1f, 1f, 0.15f);
				graphic.color = color2;
				color3 = color2;
			}
			else
			{
				color3 = new Color(1f, 1f, 1f, 0.6f);
			}
			color = color3;
		}
		if (image.material != material)
		{
			image.material = material;
			image.color = color;
		}
		Image fgImage = toggle.gameObject.GetComponent<KToggle>().fgImage;
		fgImage.gameObject.SetActive(false);
		if (!flag)
		{
			fgImage.sprite = this.Overlay_NeedTech;
			fgImage.gameObject.SetActive(true);
			string newString = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			component.AddMultiStringTooltip(newString, this.buildingToolTipSettings.ResearchRequirement);
			return;
		}
		if (requirementsState != PlanScreen.RequirementsState.Complete)
		{
			fgImage.gameObject.SetActive(false);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			string newString2 = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
			component.AddMultiStringTooltip(newString2, this.buildingToolTipSettings.ResearchRequirement);
			foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
			{
				string newString3 = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				component.AddMultiStringTooltip(newString3, this.buildingToolTipSettings.ResearchRequirement);
			}
			component.AddMultiStringTooltip("", this.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x06009058 RID: 36952 RVA: 0x00102D20 File Offset: 0x00100F20
	public void ClearUI()
	{
		this.Show(false);
		this.ClearButtons();
	}

	// Token: 0x06009059 RID: 36953 RVA: 0x00386E08 File Offset: 0x00385008
	private void ClearButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.gameObject.SetActive(false);
			ktoggle.gameObject.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(ktoggle.gameObject);
		}
		if (this.toggles != null)
		{
			this.toggles.Clear();
		}
		if (this.toggleInfo != null)
		{
			this.toggleInfo.Clear();
		}
	}

	// Token: 0x0600905A RID: 36954 RVA: 0x00386EA0 File Offset: 0x003850A0
	private void OnClickBuilding(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuBuildingsScreen.UserData userData = toggle_info.userData as BuildMenuBuildingsScreen.UserData;
		this.OnSelectBuilding(userData.def);
	}

	// Token: 0x0600905B RID: 36955 RVA: 0x00386EC8 File Offset: 0x003850C8
	private void OnSelectBuilding(BuildingDef def)
	{
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (def != this.selectedBuilding)
			{
				this.selectedBuilding = def;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedBuilding = null;
				this.ClearSelection();
				this.CloseRecipe(true);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedBuilding = null;
			this.ClearSelection();
			this.CloseRecipe(true);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x0600905C RID: 36956 RVA: 0x00386F6C File Offset: 0x0038516C
	public void UpdateBuildableStates()
	{
		if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
		{
			return;
		}
		BuildingDef buildingDef = null;
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			this.RefreshToggle(toggleInfo);
			BuildMenuBuildingsScreen.UserData userData = toggleInfo.userData as BuildMenuBuildingsScreen.UserData;
			BuildingDef def = userData.def;
			if (def.IsAvailable())
			{
				PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
				if (requirementsState != userData.requirementsState)
				{
					if (def == BuildMenu.Instance.SelectedBuildingDef)
					{
						buildingDef = def;
					}
					this.RefreshToggle(toggleInfo);
					userData.requirementsState = requirementsState;
				}
			}
		}
		if (buildingDef != null)
		{
			BuildMenu.Instance.RefreshProductInfoScreen(buildingDef);
		}
	}

	// Token: 0x0600905D RID: 36957 RVA: 0x00102D2F File Offset: 0x00100F2F
	private void OnResearchComplete(object data)
	{
		this.UpdateBuildableStates();
	}

	// Token: 0x0600905E RID: 36958 RVA: 0x00387040 File Offset: 0x00385240
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || typeof(PrebuildTool).IsAssignableFrom(type))
			{
				activeTool.DeactivateTool(null);
			}
		}
	}

	// Token: 0x0600905F RID: 36959 RVA: 0x003870A8 File Offset: 0x003852A8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!this.HasFocus)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			Game.Instance.Trigger(288942073, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			return;
		}
		base.OnKeyDown(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06009060 RID: 36960 RVA: 0x0038712C File Offset: 0x0038532C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!this.HasFocus)
		{
			return;
		}
		if (this.selectedBuilding != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			Game.Instance.Trigger(288942073, null);
			return;
		}
		base.OnKeyUp(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06009061 RID: 36961 RVA: 0x003871A4 File Offset: 0x003853A4
	public override void Close()
	{
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.ClearButtons();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009062 RID: 36962 RVA: 0x00102D37 File Offset: 0x00100F37
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x06009063 RID: 36963 RVA: 0x00102D6F File Offset: 0x00100F6F
	private void OnBuildToolDeactivated(object data)
	{
		this.CloseRecipe(false);
	}

	// Token: 0x04006D10 RID: 27920
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04006D11 RID: 27921
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04006D12 RID: 27922
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04006D13 RID: 27923
	public Action<BuildingDef> onBuildingSelected;

	// Token: 0x04006D14 RID: 27924
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04006D15 RID: 27925
	[SerializeField]
	private BuildMenuBuildingsScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04006D16 RID: 27926
	[SerializeField]
	private LayoutElement contentSizeLayout;

	// Token: 0x04006D17 RID: 27927
	[SerializeField]
	private GridLayoutGroup gridSizer;

	// Token: 0x04006D18 RID: 27928
	[SerializeField]
	private Sprite Overlay_NeedTech;

	// Token: 0x04006D19 RID: 27929
	[SerializeField]
	private Material defaultUIMaterial;

	// Token: 0x04006D1A RID: 27930
	[SerializeField]
	private Material desaturatedUIMaterial;

	// Token: 0x04006D1B RID: 27931
	private BuildingDef selectedBuilding;

	// Token: 0x02001AF5 RID: 6901
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04006D1C RID: 27932
		public TextStyleSetting BuildButtonName;

		// Token: 0x04006D1D RID: 27933
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x04006D1E RID: 27934
		public TextStyleSetting MaterialRequirement;

		// Token: 0x04006D1F RID: 27935
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001AF6 RID: 6902
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x04006D20 RID: 27936
		public TextStyleSetting ActiveSelected;

		// Token: 0x04006D21 RID: 27937
		public TextStyleSetting ActiveDeselected;

		// Token: 0x04006D22 RID: 27938
		public TextStyleSetting InactiveSelected;

		// Token: 0x04006D23 RID: 27939
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001AF7 RID: 6903
	private class UserData
	{
		// Token: 0x06009065 RID: 36965 RVA: 0x00102D80 File Offset: 0x00100F80
		public UserData(BuildingDef def, PlanScreen.RequirementsState state)
		{
			this.def = def;
			this.requirementsState = state;
		}

		// Token: 0x04006D24 RID: 27940
		public BuildingDef def;

		// Token: 0x04006D25 RID: 27941
		public PlanScreen.RequirementsState requirementsState;
	}
}
