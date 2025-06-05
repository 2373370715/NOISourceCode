using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001AF9 RID: 6905
public class BuildMenuCategoriesScreen : KIconToggleMenu
{
	// Token: 0x06009068 RID: 36968 RVA: 0x00102DA9 File Offset: 0x00100FA9
	public override float GetSortKey()
	{
		return 7f;
	}

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x06009069 RID: 36969 RVA: 0x00102DB0 File Offset: 0x00100FB0
	public HashedString Category
	{
		get
		{
			return this.category;
		}
	}

	// Token: 0x0600906A RID: 36970 RVA: 0x00102DB8 File Offset: 0x00100FB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.onSelect += this.OnClickCategory;
	}

	// Token: 0x0600906B RID: 36971 RVA: 0x003871FC File Offset: 0x003853FC
	public void Configure(HashedString category, int depth, object data, Dictionary<HashedString, List<BuildingDef>> categorized_building_map, Dictionary<HashedString, List<HashedString>> categorized_category_map, BuildMenuBuildingsScreen buildings_screen)
	{
		this.category = category;
		this.categorizedBuildingMap = categorized_building_map;
		this.categorizedCategoryMap = categorized_category_map;
		this.buildingsScreen = buildings_screen;
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		if (typeof(IList<BuildMenu.BuildingInfo>).IsAssignableFrom(data.GetType()))
		{
			this.buildingInfos = (IList<BuildMenu.BuildingInfo>)data;
		}
		else if (typeof(IList<BuildMenu.DisplayInfo>).IsAssignableFrom(data.GetType()))
		{
			this.subcategories = new List<HashedString>();
			foreach (BuildMenu.DisplayInfo displayInfo in ((IList<BuildMenu.DisplayInfo>)data))
			{
				string iconName = displayInfo.iconName;
				string text = HashCache.Get().Get(displayInfo.category).ToUpper();
				text = text.Replace(" ", "");
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".NAME"), iconName, new BuildMenuCategoriesScreen.UserData
				{
					category = displayInfo.category,
					depth = depth,
					requirementsState = PlanScreen.RequirementsState.Tech
				}, displayInfo.hotkey, Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".TOOLTIP"), "");
				list.Add(item);
				this.subcategories.Add(displayInfo.category);
			}
			base.Setup(list);
			this.toggles.ForEach(delegate(KToggle to)
			{
				foreach (ImageToggleState imageToggleState in to.GetComponents<ImageToggleState>())
				{
					if (imageToggleState.TargetImage.sprite != null && imageToggleState.TargetImage.name == "FG" && !imageToggleState.useSprites)
					{
						imageToggleState.SetSprites(Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"), imageToggleState.TargetImage.sprite, imageToggleState.TargetImage.sprite, Assets.GetSprite(imageToggleState.TargetImage.sprite.name + "_disabled"));
					}
				}
				to.GetComponent<KToggle>().soundPlayer.Enabled = false;
			});
		}
		this.UpdateBuildableStates(true);
	}

	// Token: 0x0600906C RID: 36972 RVA: 0x003873A4 File Offset: 0x003855A4
	private void OnClickCategory(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggle_info.userData;
		PlanScreen.RequirementsState requirementsState = userData.requirementsState;
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (this.selectedCategory != userData.category)
			{
				this.selectedCategory = userData.category;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedCategory = HashedString.Invalid;
				this.ClearSelection();
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedCategory = HashedString.Invalid;
			this.ClearSelection();
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		toggle_info.toggle.GetComponent<PlanCategoryNotifications>().ToggleAttention(false);
		if (this.onCategoryClicked != null)
		{
			this.onCategoryClicked(this.selectedCategory, userData.depth);
		}
	}

	// Token: 0x0600906D RID: 36973 RVA: 0x00387470 File Offset: 0x00385670
	private void UpdateButtonStates()
	{
		if (this.toggleInfo != null && this.toggleInfo.Count > 0)
		{
			foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
			{
				BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
				HashedString x = userData.category;
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(x);
				bool flag = categoryRequirements == PlanScreen.RequirementsState.Tech;
				toggleInfo.toggle.gameObject.SetActive(!flag);
				if (categoryRequirements != PlanScreen.RequirementsState.Materials)
				{
					if (categoryRequirements == PlanScreen.RequirementsState.Complete)
					{
						ImageToggleState.State state = (!this.selectedCategory.IsValid || x != this.selectedCategory) ? ImageToggleState.State.Inactive : ImageToggleState.State.Active;
						if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state)
						{
							userData.currentToggleState = new ImageToggleState.State?(state);
							this.SetImageToggleState(toggleInfo.toggle.gameObject, state);
						}
					}
				}
				else
				{
					toggleInfo.toggle.fgImage.SetAlpha(flag ? 0.2509804f : 1f);
					ImageToggleState.State state2 = (this.selectedCategory.IsValid && x == this.selectedCategory) ? ImageToggleState.State.DisabledActive : ImageToggleState.State.Disabled;
					if (userData.currentToggleState == null || userData.currentToggleState.GetValueOrDefault() != state2)
					{
						userData.currentToggleState = new ImageToggleState.State?(state2);
						this.SetImageToggleState(toggleInfo.toggle.gameObject, state2);
					}
				}
				toggleInfo.toggle.fgImage.transform.Find("ResearchIcon").gameObject.gameObject.SetActive(flag);
			}
		}
	}

	// Token: 0x0600906E RID: 36974 RVA: 0x00387634 File Offset: 0x00385834
	private void SetImageToggleState(GameObject target, ImageToggleState.State state)
	{
		ImageToggleState[] components = target.GetComponents<ImageToggleState>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState(state);
		}
	}

	// Token: 0x0600906F RID: 36975 RVA: 0x00387660 File Offset: 0x00385860
	private PlanScreen.RequirementsState GetCategoryRequirements(HashedString category)
	{
		bool flag = true;
		bool flag2 = true;
		List<BuildingDef> list;
		if (this.categorizedBuildingMap.TryGetValue(category, out list))
		{
			if (list.Count <= 0)
			{
				goto IL_F3;
			}
			using (List<BuildingDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BuildingDef buildingDef = enumerator.Current;
					if (buildingDef.ShowInBuildMenu && buildingDef.IsAvailable())
					{
						PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(buildingDef);
						flag = (flag && requirementsState == PlanScreen.RequirementsState.Tech);
						flag2 = (flag2 && (requirementsState == PlanScreen.RequirementsState.Materials || requirementsState == PlanScreen.RequirementsState.Tech));
					}
				}
				goto IL_F3;
			}
		}
		List<HashedString> list2;
		if (this.categorizedCategoryMap.TryGetValue(category, out list2))
		{
			foreach (HashedString hashedString in list2)
			{
				PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
				flag = (flag && categoryRequirements == PlanScreen.RequirementsState.Tech);
				flag2 = (flag2 && (categoryRequirements == PlanScreen.RequirementsState.Materials || categoryRequirements == PlanScreen.RequirementsState.Tech));
			}
		}
		IL_F3:
		PlanScreen.RequirementsState result;
		if (flag)
		{
			result = PlanScreen.RequirementsState.Tech;
		}
		else if (flag2)
		{
			result = PlanScreen.RequirementsState.Materials;
		}
		else
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		if (DebugHandler.InstantBuildMode)
		{
			result = PlanScreen.RequirementsState.Complete;
		}
		return result;
	}

	// Token: 0x06009070 RID: 36976 RVA: 0x00387798 File Offset: 0x00385998
	public void UpdateNotifications(ICollection<HashedString> updated_categories)
	{
		if (this.toggleInfo == null)
		{
			return;
		}
		this.UpdateBuildableStates(false);
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			HashedString item = ((BuildMenuCategoriesScreen.UserData)toggleInfo.userData).category;
			if (updated_categories.Contains(item))
			{
				toggleInfo.toggle.gameObject.GetComponent<PlanCategoryNotifications>().ToggleAttention(true);
			}
		}
	}

	// Token: 0x06009071 RID: 36977 RVA: 0x00102DD2 File Offset: 0x00100FD2
	public override void Close()
	{
		base.Close();
		this.selectedCategory = HashedString.Invalid;
		this.SetHasFocus(false);
		if (this.buildingInfos != null)
		{
			this.buildingsScreen.Close();
		}
	}

	// Token: 0x06009072 RID: 36978 RVA: 0x00102DFF File Offset: 0x00100FFF
	[ContextMenu("ForceUpdateBuildableStates")]
	private void ForceUpdateBuildableStates()
	{
		this.UpdateBuildableStates(true);
	}

	// Token: 0x06009073 RID: 36979 RVA: 0x00387820 File Offset: 0x00385A20
	public void UpdateBuildableStates(bool skip_flourish)
	{
		if (this.subcategories != null && this.subcategories.Count > 0)
		{
			this.UpdateButtonStates();
			using (IEnumerator<KIconToggleMenu.ToggleInfo> enumerator = this.toggleInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KIconToggleMenu.ToggleInfo toggleInfo = enumerator.Current;
					BuildMenuCategoriesScreen.UserData userData = (BuildMenuCategoriesScreen.UserData)toggleInfo.userData;
					HashedString hashedString = userData.category;
					PlanScreen.RequirementsState categoryRequirements = this.GetCategoryRequirements(hashedString);
					if (userData.requirementsState != categoryRequirements)
					{
						userData.requirementsState = categoryRequirements;
						toggleInfo.userData = userData;
						if (!skip_flourish)
						{
							toggleInfo.toggle.ActivateFlourish(false);
							string text = "NotificationPing";
							if (!toggleInfo.toggle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag(text))
							{
								toggleInfo.toggle.gameObject.GetComponent<Animator>().Play(text);
								BuildMenu.Instance.PlayNewBuildingSounds();
							}
						}
					}
				}
				return;
			}
		}
		this.buildingsScreen.UpdateBuildableStates();
	}

	// Token: 0x06009074 RID: 36980 RVA: 0x00387924 File Offset: 0x00385B24
	protected override void OnShow(bool show)
	{
		if (this.buildingInfos != null)
		{
			if (show)
			{
				this.buildingsScreen.Configure(this.category, this.buildingInfos);
				this.buildingsScreen.Show(true);
			}
			else
			{
				this.buildingsScreen.Close();
			}
		}
		base.OnShow(show);
	}

	// Token: 0x06009075 RID: 36981 RVA: 0x00387974 File Offset: 0x00385B74
	public override void ClearSelection()
	{
		this.selectedCategory = HashedString.Invalid;
		base.ClearSelection();
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.isOn = false;
		}
	}

	// Token: 0x06009076 RID: 36982 RVA: 0x003879D8 File Offset: 0x00385BD8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					Game.Instance.Trigger(288942073, null);
					return;
				}
				base.OnKeyDown(e);
				if (!e.Consumed)
				{
					global::Action action = e.GetAction();
					if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
					{
						e.TryConsume(action);
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyDown(e);
			if (e.Consumed)
			{
				this.UpdateButtonStates();
			}
		}
	}

	// Token: 0x06009077 RID: 36983 RVA: 0x00387A48 File Offset: 0x00385C48
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.modalKeyInputBehaviour)
		{
			if (this.HasFocus)
			{
				if (e.TryConsume(global::Action.Escape))
				{
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
						return;
					}
				}
			}
		}
		else
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06009078 RID: 36984 RVA: 0x00102E08 File Offset: 0x00101008
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x04006D27 RID: 27943
	public Action<HashedString, int> onCategoryClicked;

	// Token: 0x04006D28 RID: 27944
	[SerializeField]
	public bool modalKeyInputBehaviour;

	// Token: 0x04006D29 RID: 27945
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04006D2A RID: 27946
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04006D2B RID: 27947
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04006D2C RID: 27948
	private IList<HashedString> subcategories;

	// Token: 0x04006D2D RID: 27949
	private Dictionary<HashedString, List<BuildingDef>> categorizedBuildingMap;

	// Token: 0x04006D2E RID: 27950
	private Dictionary<HashedString, List<HashedString>> categorizedCategoryMap;

	// Token: 0x04006D2F RID: 27951
	private BuildMenuBuildingsScreen buildingsScreen;

	// Token: 0x04006D30 RID: 27952
	private HashedString category;

	// Token: 0x04006D31 RID: 27953
	private IList<BuildMenu.BuildingInfo> buildingInfos;

	// Token: 0x04006D32 RID: 27954
	private HashedString selectedCategory = HashedString.Invalid;

	// Token: 0x02001AFA RID: 6906
	private class UserData
	{
		// Token: 0x04006D33 RID: 27955
		public HashedString category;

		// Token: 0x04006D34 RID: 27956
		public int depth;

		// Token: 0x04006D35 RID: 27957
		public PlanScreen.RequirementsState requirementsState;

		// Token: 0x04006D36 RID: 27958
		public ImageToggleState.State? currentToggleState;
	}
}
