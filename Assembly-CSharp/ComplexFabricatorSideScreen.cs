using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FB0 RID: 8112
public class ComplexFabricatorSideScreen : SideScreenContent
{
	// Token: 0x0600AB81 RID: 43905 RVA: 0x00419860 File Offset: 0x00417A60
	public override string GetTitle()
	{
		if (this.targetFab == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetFab.GetProperName());
	}

	// Token: 0x0600AB82 RID: 43906 RVA: 0x004198BC File Offset: 0x00417ABC
	public override bool IsValidForTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		return component != null && component.enabled;
	}

	// Token: 0x0600AB83 RID: 43907 RVA: 0x004198E4 File Offset: 0x00417AE4
	public override void SetTarget(GameObject target)
	{
		ComplexFabricator component = target.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a ComplexFabricator!");
			return;
		}
		if (this.targetOrdersUpdatedSubHandle != -1)
		{
			base.Unsubscribe(this.targetOrdersUpdatedSubHandle);
		}
		this.Initialize(component);
		this.targetOrdersUpdatedSubHandle = this.targetFab.Subscribe(1721324763, new Action<object>(this.UpdateQueueCountLabels));
		this.UpdateQueueCountLabels(null);
	}

	// Token: 0x0600AB84 RID: 43908 RVA: 0x00419954 File Offset: 0x00417B54
	private void UpdateQueueCountLabels(object data = null)
	{
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe r = recipes[i];
			GameObject gameObject = this.recipeToggles.Find((GameObject match) => this.recipeMap[match] == r);
			if (gameObject != null)
			{
				this.RefreshQueueCountDisplay(gameObject, this.targetFab);
			}
		}
		if (this.targetFab.CurrentWorkingOrder != null)
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, this.targetFab.CurrentWorkingOrder.GetUIName(false));
		}
		else
		{
			this.currentOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
		}
		if (this.targetFab.NextOrder != null)
		{
			this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, this.targetFab.NextOrder.GetUIName(false));
			return;
		}
		this.nextOrderLabel.text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
	}

	// Token: 0x0600AB85 RID: 43909 RVA: 0x00419A70 File Offset: 0x00417C70
	protected override void OnShow(bool show)
	{
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.selectedRecipe = null;
			this.selectedToggle = null;
		}
		base.OnShow(show);
	}

	// Token: 0x0600AB86 RID: 43910 RVA: 0x00419ACC File Offset: 0x00417CCC
	public void Initialize(ComplexFabricator target)
	{
		if (target == null)
		{
			global::Debug.LogError("ComplexFabricator provided was null.");
			return;
		}
		this.targetFab = target;
		base.gameObject.SetActive(true);
		this.recipeMap = new Dictionary<GameObject, ComplexRecipe>();
		this.recipeToggles.ForEach(delegate(GameObject rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.recipeToggles.Clear();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.recipeCategories)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value.transform.parent.gameObject);
		}
		this.recipeCategories.Clear();
		int num = 0;
		ComplexRecipe[] recipes = this.targetFab.GetRecipes();
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			bool flag = false;
			if (DebugHandler.InstantBuildMode)
			{
				flag = true;
			}
			else if (recipe.RequiresTechUnlock())
			{
				if ((recipe.IsRequiredTechUnlocked() || Db.Get().Techs.Get(recipe.requiredTech).ArePrerequisitesComplete()) && (!recipe.RequiresAllIngredientsDiscovered || this.AllRecipeRequirementsDiscovered(recipe)))
				{
					flag = true;
				}
			}
			else if (target.GetRecipeQueueCount(recipe) != 0)
			{
				flag = true;
			}
			else if (recipe.RequiresAllIngredientsDiscovered)
			{
				if (this.AllRecipeRequirementsDiscovered(recipe))
				{
					flag = true;
				}
			}
			else if (this.AnyRecipeRequirementsDiscovered(recipe))
			{
				flag = true;
			}
			else if (this.HasAnyRecipeRequirements(recipe))
			{
				flag = true;
			}
			if (flag)
			{
				num++;
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(recipe.ingredients[0].material, "ui", false);
				global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
				KToggle newToggle = null;
				ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = target.sideScreenStyle;
				GameObject entryGO;
				if (sideScreenStyle - ComplexFabricatorSideScreen.StyleSetting.ListInputOutput > 1)
				{
					if (sideScreenStyle != ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
					{
						newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButton, this.recipeGrid, false);
						entryGO = newToggle.gameObject;
						Image componentInChildrenOnly = newToggle.gameObject.GetComponentInChildrenOnly<Image>();
						if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.GridInput || target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListInput)
						{
							componentInChildrenOnly.sprite = uisprite.first;
							componentInChildrenOnly.color = uisprite.second;
						}
						else
						{
							componentInChildrenOnly.sprite = uisprite2.first;
							componentInChildrenOnly.color = uisprite2.second;
						}
					}
					else
					{
						newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButtonQueueHybrid, this.recipeGrid, false);
						entryGO = newToggle.gameObject;
						this.recipeMap.Add(entryGO, recipe);
						if (recipe.recipeCategoryID != "")
						{
							if (!this.recipeCategories.ContainsKey(recipe.recipeCategoryID))
							{
								GameObject gameObject = global::Util.KInstantiateUI(this.recipeCategoryHeader, this.recipeGrid, true);
								gameObject.GetComponentInChildren<LocText>().SetText(Strings.Get("STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_CATEGORIES." + recipe.recipeCategoryID.ToUpper()).String);
								HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
								RectTransform categoryContent = component.GetReference<RectTransform>("content");
								component.GetReference<Image>("icon").sprite = recipe.GetUIIcon();
								categoryContent.gameObject.SetActive(false);
								MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
								MultiToggle toggle2 = toggle;
								toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
								{
									categoryContent.gameObject.SetActive(!categoryContent.gameObject.activeSelf);
									toggle.ChangeState(categoryContent.gameObject.activeSelf ? 1 : 0);
								}));
								this.recipeCategories.Add(recipe.recipeCategoryID, categoryContent.gameObject);
							}
							newToggle.transform.SetParent(this.recipeCategories[recipe.recipeCategoryID].rectTransform());
						}
						Image image = entryGO.GetComponentsInChildrenOnly<Image>()[2];
						if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
						{
							image.sprite = uisprite.first;
							image.color = uisprite.second;
						}
						else if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
						{
							image.sprite = this.radboltSprite;
						}
						else if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom)
						{
							image.sprite = recipe.GetUIIcon();
						}
						else
						{
							image.sprite = uisprite2.first;
							image.color = uisprite2.second;
						}
						entryGO.GetComponentInChildren<LocText>().text = recipe.GetUIName(false);
						bool flag2 = this.HasAllRecipeRequirements(recipe);
						image.material = (flag2 ? Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial);
						this.RefreshQueueCountDisplay(entryGO, this.targetFab);
						entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("DecrementButton").onClick = delegate()
						{
							target.DecrementRecipeQueueCount(recipe, false);
							this.RefreshQueueCountDisplay(entryGO, target);
						};
						entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("IncrementButton").onClick = delegate()
						{
							target.IncrementRecipeQueueCount(recipe);
							this.RefreshQueueCountDisplay(entryGO, target);
						};
						entryGO.gameObject.SetActive(true);
					}
				}
				else
				{
					newToggle = global::Util.KInstantiateUI<KToggle>(this.recipeButtonMultiple, this.recipeGrid, false);
					entryGO = newToggle.gameObject;
					HierarchyReferences component2 = newToggle.GetComponent<HierarchyReferences>();
					foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
					{
						GameObject gameObject2 = global::Util.KInstantiateUI(component2.GetReference("FromIconPrefab").gameObject, component2.GetReference("FromIcons").gameObject, true);
						gameObject2.GetComponent<Image>().sprite = Def.GetUISprite(recipeElement.material, "ui", false).first;
						gameObject2.GetComponent<Image>().color = Def.GetUISprite(recipeElement.material, "ui", false).second;
						gameObject2.gameObject.name = recipeElement.material.Name;
					}
					foreach (ComplexRecipe.RecipeElement recipeElement2 in recipe.results)
					{
						GameObject gameObject3 = global::Util.KInstantiateUI(component2.GetReference("ToIconPrefab").gameObject, component2.GetReference("ToIcons").gameObject, true);
						gameObject3.GetComponent<Image>().sprite = Def.GetUISprite(recipeElement2.material, "ui", false).first;
						gameObject3.GetComponent<Image>().color = Def.GetUISprite(recipeElement2.material, "ui", false).second;
						gameObject3.gameObject.name = recipeElement2.material.Name;
					}
				}
				if (this.targetFab.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ClassicFabricator)
				{
					newToggle.GetComponentInChildren<LocText>().text = recipe.results[0].material.ProperName();
				}
				else if (this.targetFab.sideScreenStyle != ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
				{
					newToggle.GetComponentInChildren<LocText>().text = string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_WITH_NEWLINES, recipe.ingredients[0].material.ProperName(), recipe.results[0].material.ProperName());
				}
				ToolTip component3 = entryGO.GetComponent<ToolTip>();
				component3.toolTipPosition = ToolTip.TooltipPosition.Custom;
				component3.parentPositionAnchor = new Vector2(0f, 0.5f);
				component3.tooltipPivot = new Vector2(1f, 1f);
				component3.tooltipPositionOffset = new Vector2(-24f, 20f);
				component3.ClearMultiStringTooltip();
				component3.AddMultiStringTooltip(recipe.GetUIName(false), this.styleTooltipHeader);
				component3.AddMultiStringTooltip(recipe.description, this.styleTooltipBody);
				if (recipe.runTimeDescription != null)
				{
					component3.AddMultiStringTooltip("\n" + recipe.runTimeDescription(), this.styleTooltipBody);
				}
				newToggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				entryGO.SetActive(true);
				this.recipeToggles.Add(entryGO);
			}
		}
		if (this.recipeToggles.Count > 0)
		{
			VerticalLayoutGroup component4 = this.buttonContentContainer.GetComponent<VerticalLayoutGroup>();
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = Mathf.Min(451f, (float)(component4.padding.top + component4.padding.bottom) + (float)num * this.recipeButtonQueueHybrid.GetComponent<LayoutElement>().minHeight + (float)(num - 1) * component4.spacing);
			this.subtitleLabel.SetText(this.targetFab.SideScreenSubtitleLabel);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(false);
		}
		else
		{
			this.subtitleLabel.SetText(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED);
			this.noRecipesDiscoveredLabel.SetText(UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED_BODY);
			this.noRecipesDiscoveredLabel.gameObject.SetActive(true);
			this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = this.noRecipesDiscoveredLabel.GetComponent<LayoutElement>().minHeight + 10f;
		}
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x0600AB87 RID: 43911 RVA: 0x0041A564 File Offset: 0x00418764
	public void RefreshQueueCountDisplayForRecipe(ComplexRecipe recipe, ComplexFabricator fabricator)
	{
		GameObject gameObject = this.recipeToggles.Find((GameObject match) => this.recipeMap[match] == recipe);
		if (gameObject != null)
		{
			this.RefreshQueueCountDisplay(gameObject, fabricator);
		}
	}

	// Token: 0x0600AB88 RID: 43912 RVA: 0x0041A5B0 File Offset: 0x004187B0
	private void RefreshQueueCountDisplay(GameObject entryGO, ComplexFabricator fabricator)
	{
		HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
		bool flag = fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]) == ComplexFabricator.QUEUE_INFINITE;
		component.GetReference<LocText>("CountLabel").text = (flag ? "" : fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]).ToString());
		component.GetReference<RectTransform>("InfiniteIcon").gameObject.SetActive(flag);
		bool flag2 = !this.recipeMap[entryGO].IsRequiredTechUnlocked();
		GameObject gameObject = component.GetReference<RectTransform>("TechRequired").gameObject;
		gameObject.SetActive(flag2);
		KButton component2 = gameObject.GetComponent<KButton>();
		component2.ClearOnClick();
		if (flag2)
		{
			component2.onClick += delegate()
			{
				ManagementMenu.Instance.OpenResearch(this.recipeMap[entryGO].requiredTech);
			};
		}
	}

	// Token: 0x0600AB89 RID: 43913 RVA: 0x0041A69C File Offset: 0x0041889C
	private void ToggleClicked(KToggle toggle)
	{
		if (!this.recipeMap.ContainsKey(toggle.gameObject))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedToggle == toggle)
		{
			this.selectedToggle.isOn = false;
			this.selectedToggle = null;
			this.selectedRecipe = null;
		}
		else
		{
			this.selectedToggle = toggle;
			this.selectedToggle.isOn = true;
			this.selectedRecipe = this.recipeMap[toggle.gameObject];
			this.selectedRecipeFabricatorMap[this.targetFab] = this.recipeToggles.IndexOf(toggle.gameObject);
		}
		this.RefreshIngredientAvailabilityVis();
		if (toggle.isOn)
		{
			this.recipeScreen = (SelectedRecipeQueueScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.recipeScreenPrefab, this.targetFab.SideScreenRecipeScreenTitle);
			this.recipeScreen.SetRecipe(this, this.targetFab, this.selectedRecipe);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x0600AB8A RID: 43914 RVA: 0x0041A794 File Offset: 0x00418994
	public void CycleRecipe(int increment)
	{
		int num = 0;
		if (this.selectedToggle != null)
		{
			num = this.recipeToggles.IndexOf(this.selectedToggle.gameObject);
		}
		int num2 = (num + increment) % this.recipeToggles.Count;
		if (num2 < 0)
		{
			num2 = this.recipeToggles.Count + num2;
		}
		this.ToggleClicked(this.recipeToggles[num2].GetComponent<KToggle>());
	}

	// Token: 0x0600AB8B RID: 43915 RVA: 0x0041A804 File Offset: 0x00418A04
	private bool HasAnyRecipeRequirements(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) >= recipeElement.amount)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600AB8C RID: 43916 RVA: 0x0041A8A4 File Offset: 0x00418AA4
	private bool HasAllRecipeRequirements(ComplexRecipe recipe)
	{
		bool result = true;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (this.targetFab.GetMyWorld().worldInventory.GetAmountWithoutTag(recipeElement.material, true, this.targetFab.ForbiddenTags) + this.targetFab.inStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) + this.targetFab.buildStorage.GetAmountAvailable(recipeElement.material, this.targetFab.ForbiddenTags) < recipeElement.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600AB8D RID: 43917 RVA: 0x0041A948 File Offset: 0x00418B48
	private bool AnyRecipeRequirementsDiscovered(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (DiscoveredResources.Instance.IsDiscovered(recipeElement.material))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600AB8E RID: 43918 RVA: 0x0041A984 File Offset: 0x00418B84
	private bool AllRecipeRequirementsDiscovered(ComplexRecipe recipe)
	{
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			if (!DiscoveredResources.Instance.IsDiscovered(recipeElement.material))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600AB8F RID: 43919 RVA: 0x00113EEB File Offset: 0x001120EB
	private void Update()
	{
		this.RefreshIngredientAvailabilityVis();
	}

	// Token: 0x0600AB90 RID: 43920 RVA: 0x0041A9C0 File Offset: 0x00418BC0
	private void RefreshIngredientAvailabilityVis()
	{
		foreach (KeyValuePair<GameObject, ComplexRecipe> keyValuePair in this.recipeMap)
		{
			HierarchyReferences component = keyValuePair.Key.GetComponent<HierarchyReferences>();
			bool flag = this.HasAllRecipeRequirements(keyValuePair.Value);
			KToggle component2 = keyValuePair.Key.GetComponent<KToggle>();
			if (flag)
			{
				if (this.selectedRecipe == keyValuePair.Value)
				{
					component2.ActivateFlourish(true, ImageToggleState.State.Active);
				}
				else
				{
					component2.ActivateFlourish(false, ImageToggleState.State.Inactive);
				}
			}
			else if (this.selectedRecipe == keyValuePair.Value)
			{
				component2.ActivateFlourish(true, ImageToggleState.State.DisabledActive);
			}
			else
			{
				component2.ActivateFlourish(false, ImageToggleState.State.Disabled);
			}
			component.GetReference<LocText>("Label").color = (flag ? Color.black : new Color(0.22f, 0.22f, 0.22f, 1f));
		}
	}

	// Token: 0x0600AB91 RID: 43921 RVA: 0x0041AAB4 File Offset: 0x00418CB4
	private Element[] GetRecipeElements(Recipe recipe)
	{
		Element[] array = new Element[recipe.Ingredients.Count];
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			Tag tag = recipe.Ingredients[i].tag;
			foreach (Element element in ElementLoader.elements)
			{
				if (GameTagExtensions.Create(element.id) == tag)
				{
					array[i] = element;
					break;
				}
			}
		}
		return array;
	}

	// Token: 0x040086FD RID: 34557
	[Header("Recipe List")]
	[SerializeField]
	private GameObject recipeGrid;

	// Token: 0x040086FE RID: 34558
	[Header("Recipe button variants")]
	[SerializeField]
	private GameObject recipeButton;

	// Token: 0x040086FF RID: 34559
	[SerializeField]
	private GameObject recipeButtonMultiple;

	// Token: 0x04008700 RID: 34560
	[SerializeField]
	private GameObject recipeButtonQueueHybrid;

	// Token: 0x04008701 RID: 34561
	[SerializeField]
	private GameObject recipeCategoryHeader;

	// Token: 0x04008702 RID: 34562
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x04008703 RID: 34563
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x04008704 RID: 34564
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x04008705 RID: 34565
	[SerializeField]
	public Sprite radboltSprite;

	// Token: 0x04008706 RID: 34566
	private KToggle selectedToggle;

	// Token: 0x04008707 RID: 34567
	public LayoutElement buttonScrollContainer;

	// Token: 0x04008708 RID: 34568
	public RectTransform buttonContentContainer;

	// Token: 0x04008709 RID: 34569
	[SerializeField]
	private GameObject elementContainer;

	// Token: 0x0400870A RID: 34570
	[SerializeField]
	private LocText currentOrderLabel;

	// Token: 0x0400870B RID: 34571
	[SerializeField]
	private LocText nextOrderLabel;

	// Token: 0x0400870C RID: 34572
	private Dictionary<ComplexFabricator, int> selectedRecipeFabricatorMap = new Dictionary<ComplexFabricator, int>();

	// Token: 0x0400870D RID: 34573
	public EventReference createOrderSound;

	// Token: 0x0400870E RID: 34574
	[SerializeField]
	private RectTransform content;

	// Token: 0x0400870F RID: 34575
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x04008710 RID: 34576
	[SerializeField]
	private LocText noRecipesDiscoveredLabel;

	// Token: 0x04008711 RID: 34577
	public TextStyleSetting styleTooltipHeader;

	// Token: 0x04008712 RID: 34578
	public TextStyleSetting styleTooltipBody;

	// Token: 0x04008713 RID: 34579
	private ComplexFabricator targetFab;

	// Token: 0x04008714 RID: 34580
	private ComplexRecipe selectedRecipe;

	// Token: 0x04008715 RID: 34581
	private Dictionary<GameObject, ComplexRecipe> recipeMap;

	// Token: 0x04008716 RID: 34582
	private Dictionary<string, GameObject> recipeCategories = new Dictionary<string, GameObject>();

	// Token: 0x04008717 RID: 34583
	private List<GameObject> recipeToggles = new List<GameObject>();

	// Token: 0x04008718 RID: 34584
	public SelectedRecipeQueueScreen recipeScreenPrefab;

	// Token: 0x04008719 RID: 34585
	private SelectedRecipeQueueScreen recipeScreen;

	// Token: 0x0400871A RID: 34586
	private int targetOrdersUpdatedSubHandle = -1;

	// Token: 0x02001FB1 RID: 8113
	public enum StyleSetting
	{
		// Token: 0x0400871C RID: 34588
		GridResult,
		// Token: 0x0400871D RID: 34589
		ListResult,
		// Token: 0x0400871E RID: 34590
		GridInput,
		// Token: 0x0400871F RID: 34591
		ListInput,
		// Token: 0x04008720 RID: 34592
		ListInputOutput,
		// Token: 0x04008721 RID: 34593
		GridInputOutput,
		// Token: 0x04008722 RID: 34594
		ClassicFabricator,
		// Token: 0x04008723 RID: 34595
		ListQueueHybrid
	}
}
