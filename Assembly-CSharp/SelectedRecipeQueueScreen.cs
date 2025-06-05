using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200202C RID: 8236
public class SelectedRecipeQueueScreen : KScreen
{
	// Token: 0x0600AE7F RID: 44671 RVA: 0x00427370 File Offset: 0x00425570
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DecrementButton.onClick = delegate()
		{
			this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.IncrementButton.onClick = delegate()
		{
			this.target.IncrementRecipeQueueCount(this.selectedRecipe);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.InfiniteButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
		this.InfiniteButton.onClick += delegate()
		{
			if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
			}
			else
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
			}
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onEndEdit += delegate()
		{
			base.isEditing = false;
			this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onStartEdit += delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
		};
		MultiToggle multiToggle = this.previousRecipeButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.CyclePreviousRecipe));
		MultiToggle multiToggle2 = this.nextRecipeButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.CycleNextRecipe));
	}

	// Token: 0x0600AE80 RID: 44672 RVA: 0x00427460 File Offset: 0x00425660
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedRecipe != null)
		{
			GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
			Equippable equippable = (prefab != null) ? prefab.GetComponent<Equippable>() : null;
			if (equippable != null && equippable.GetBuildOverride() != null)
			{
				this.minionWidget.RemoveEquipment(equippable);
			}
		}
	}

	// Token: 0x0600AE81 RID: 44673 RVA: 0x004274CC File Offset: 0x004256CC
	public void SetRecipe(ComplexFabricatorSideScreen owner, ComplexFabricator target, ComplexRecipe recipe)
	{
		this.ownerScreen = owner;
		this.target = target;
		this.selectedRecipe = recipe;
		this.recipeName.text = recipe.GetUIName(false);
		global::Tuple<Sprite, Color> uisprite;
		if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
		{
			uisprite = Def.GetUISprite(recipe.ingredients[0].material, "ui", false);
		}
		else if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom && !string.IsNullOrEmpty(recipe.customSpritePrefabID))
		{
			uisprite = Def.GetUISprite(recipe.customSpritePrefabID, "ui", false);
		}
		else
		{
			uisprite = Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
		}
		if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
		{
			this.recipeIcon.sprite = owner.radboltSprite;
			this.recipeIcon.sprite = owner.radboltSprite;
		}
		else
		{
			this.recipeIcon.sprite = uisprite.first;
			this.recipeIcon.color = uisprite.second;
		}
		string text = (recipe.time.ToString() + " " + UI.UNITSUFFIXES.SECONDS).ToLower();
		this.recipeMainDescription.SetText(recipe.description);
		this.recipeDuration.SetText(text);
		string simpleTooltip = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, text);
		this.recipeDurationTooltip.SetSimpleTooltip(simpleTooltip);
		this.RefreshIngredientDescriptors();
		this.RefreshResultDescriptors();
		this.RefreshQueueCountDisplay();
		this.ToggleAndRefreshMinionDisplay();
	}

	// Token: 0x0600AE82 RID: 44674 RVA: 0x00115E34 File Offset: 0x00114034
	private void CyclePreviousRecipe()
	{
		this.ownerScreen.CycleRecipe(-1);
	}

	// Token: 0x0600AE83 RID: 44675 RVA: 0x00115E42 File Offset: 0x00114042
	private void CycleNextRecipe()
	{
		this.ownerScreen.CycleRecipe(1);
	}

	// Token: 0x0600AE84 RID: 44676 RVA: 0x00115E50 File Offset: 0x00114050
	private void ToggleAndRefreshMinionDisplay()
	{
		this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
	}

	// Token: 0x0600AE85 RID: 44677 RVA: 0x00427638 File Offset: 0x00425838
	private bool RefreshMinionDisplayAnim()
	{
		GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
		if (prefab == null)
		{
			return false;
		}
		Equippable component = prefab.GetComponent<Equippable>();
		if (component == null)
		{
			return false;
		}
		KAnimFile buildOverride = component.GetBuildOverride();
		if (buildOverride == null)
		{
			return false;
		}
		this.minionWidget.SetDefaultPortraitAnimator();
		KAnimFile animFile = buildOverride;
		if (!this.selectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
		{
			EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.selectedRecipe.results[0].facadeID);
			if (equippableFacadeResource != null)
			{
				animFile = Assets.GetAnim(equippableFacadeResource.BuildOverride);
			}
		}
		this.minionWidget.UpdateEquipment(component, animFile);
		return true;
	}

	// Token: 0x0600AE86 RID: 44678 RVA: 0x004276F4 File Offset: 0x004258F4
	private void RefreshQueueCountDisplay()
	{
		this.ResearchRequiredContainer.SetActive(!this.selectedRecipe.IsRequiredTechUnlocked());
		bool flag = this.target.GetRecipeQueueCount(this.selectedRecipe) == ComplexFabricator.QUEUE_INFINITE;
		if (!flag)
		{
			this.QueueCount.SetAmount((float)this.target.GetRecipeQueueCount(this.selectedRecipe));
		}
		else
		{
			this.QueueCount.SetDisplayValue("");
		}
		this.InfiniteIcon.gameObject.SetActive(flag);
	}

	// Token: 0x0600AE87 RID: 44679 RVA: 0x00427778 File Offset: 0x00425978
	private void RefreshResultDescriptors()
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		list.AddRange(this.GetResultDescriptions(this.selectedRecipe));
		foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipe))
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
		}
		if (list.Count > 0)
		{
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeEffectsDescriptorRows)
			{
				Util.KDestroyGameObject(keyValuePair.Value);
			}
			this.recipeEffectsDescriptorRows.Clear();
			foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list)
			{
				GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				Image reference = component.GetReference<Image>("Icon");
				bool flag = descriptorWithSprite.tintedSprite != null && descriptorWithSprite.tintedSprite.first != null;
				reference.sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
				reference.gameObject.SetActive(flag);
				if (!flag)
				{
					reference.gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>().padding.left = 30;
				}
				reference.color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
				component.GetReference<LocText>("Label").SetText(flag ? descriptorWithSprite.descriptor.IndentedText() : descriptorWithSprite.descriptor.text);
				component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
				component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
				this.recipeEffectsDescriptorRows.Add(descriptorWithSprite, gameObject);
			}
		}
	}

	// Token: 0x0600AE88 RID: 44680 RVA: 0x004279FC File Offset: 0x00425BFC
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		if (recipe.producedHEP > 0)
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.producedHEP), string.Format("<b>{0}</b>: {1}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.producedHEP), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.results)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, recipeElement.facadeID), false));
			Element element = ElementLoader.GetElement(recipeElement.material);
			if (element != null)
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list2 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc in GameUtil.GetMaterialDescriptors(element))
				{
					list2.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list2)
				{
					descriptorWithSprite.descriptor.IncreaseIndent();
				}
				list.AddRange(list2);
			}
			else
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list3 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc2 in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false)))
				{
					list3.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc2, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite2 in list3)
				{
					descriptorWithSprite2.descriptor.IncreaseIndent();
				}
				list.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x0600AE89 RID: 44681 RVA: 0x00427CCC File Offset: 0x00425ECC
	private void RefreshIngredientDescriptors()
	{
		new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> ingredientDescriptions = this.GetIngredientDescriptions(this.selectedRecipe);
		this.IngredientsDescriptorPanel.gameObject.SetActive(true);
		foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeIngredientDescriptorRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.recipeIngredientDescriptorRows.Clear();
		foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in ingredientDescriptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.IngredientsDescriptorPanel.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
			component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
			component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
			component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
			component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
			this.recipeIngredientDescriptorRows.Add(descriptorWithSprite, gameObject);
		}
	}

	// Token: 0x0600AE8A RID: 44682 RVA: 0x00427E64 File Offset: 0x00426064
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetIngredientDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			float amount = this.target.GetMyWorld().worldInventory.GetAmount(recipeElement.material, true);
			string formattedByTag2 = GameUtil.GetFormattedByTag(recipeElement.material, amount, GameUtil.TimeSlice.None);
			string text = (amount >= recipeElement.amount) ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) : ("<color=#F44A47>" + string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) + "</color>");
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(text, text, Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, "ui", false), Assets.GetPrefab(recipeElement.material).GetComponent<MutantPlant>() != null));
		}
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = this.target.GetComponent<HighEnergyParticleStorage>();
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1} / {2}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.consumedHEP, component.Particles), string.Format("<b>{0}</b>: {1} / {2}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.consumedHEP, component.Particles), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		return list;
	}

	// Token: 0x0400894F RID: 35151
	public Image recipeIcon;

	// Token: 0x04008950 RID: 35152
	public LocText recipeName;

	// Token: 0x04008951 RID: 35153
	public LocText recipeMainDescription;

	// Token: 0x04008952 RID: 35154
	public LocText recipeDuration;

	// Token: 0x04008953 RID: 35155
	public ToolTip recipeDurationTooltip;

	// Token: 0x04008954 RID: 35156
	public GameObject IngredientsDescriptorPanel;

	// Token: 0x04008955 RID: 35157
	public GameObject EffectsDescriptorPanel;

	// Token: 0x04008956 RID: 35158
	public KNumberInputField QueueCount;

	// Token: 0x04008957 RID: 35159
	public MultiToggle DecrementButton;

	// Token: 0x04008958 RID: 35160
	public MultiToggle IncrementButton;

	// Token: 0x04008959 RID: 35161
	public KButton InfiniteButton;

	// Token: 0x0400895A RID: 35162
	public GameObject InfiniteIcon;

	// Token: 0x0400895B RID: 35163
	public GameObject ResearchRequiredContainer;

	// Token: 0x0400895C RID: 35164
	private ComplexFabricator target;

	// Token: 0x0400895D RID: 35165
	private ComplexFabricatorSideScreen ownerScreen;

	// Token: 0x0400895E RID: 35166
	private ComplexRecipe selectedRecipe;

	// Token: 0x0400895F RID: 35167
	[SerializeField]
	private GameObject recipeElementDescriptorPrefab;

	// Token: 0x04008960 RID: 35168
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04008961 RID: 35169
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04008962 RID: 35170
	[SerializeField]
	private FullBodyUIMinionWidget minionWidget;

	// Token: 0x04008963 RID: 35171
	[SerializeField]
	private MultiToggle previousRecipeButton;

	// Token: 0x04008964 RID: 35172
	[SerializeField]
	private MultiToggle nextRecipeButton;

	// Token: 0x0200202D RID: 8237
	private class DescriptorWithSprite
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600AE91 RID: 44689 RVA: 0x00115EE7 File Offset: 0x001140E7
		public Descriptor descriptor { get; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600AE92 RID: 44690 RVA: 0x00115EEF File Offset: 0x001140EF
		public global::Tuple<Sprite, Color> tintedSprite { get; }

		// Token: 0x0600AE93 RID: 44691 RVA: 0x00115EF7 File Offset: 0x001140F7
		public DescriptorWithSprite(Descriptor desc, global::Tuple<Sprite, Color> sprite, bool filterRowVisible = false)
		{
			this.descriptor = desc;
			this.tintedSprite = sprite;
			this.showFilterRow = filterRowVisible;
		}

		// Token: 0x04008967 RID: 35175
		public bool showFilterRow;
	}
}
