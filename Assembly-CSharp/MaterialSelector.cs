using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E44 RID: 7748
public class MaterialSelector : KScreen
{
	// Token: 0x0600A213 RID: 41491 RVA: 0x0010DD0A File Offset: 0x0010BF0A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleGroup = base.GetComponent<ToggleGroup>();
	}

	// Token: 0x0600A214 RID: 41492 RVA: 0x0010DD1E File Offset: 0x0010BF1E
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A215 RID: 41493 RVA: 0x003EC8DC File Offset: 0x003EAADC
	public void ClearMaterialToggles()
	{
		this.CurrentSelectedElement = null;
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			keyValuePair.Value.gameObject.SetActive(false);
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.ElementToggles.Clear();
	}

	// Token: 0x0600A216 RID: 41494 RVA: 0x003EC974 File Offset: 0x003EAB74
	public static List<Tag> GetValidMaterials(Tag _materialTypeTag, bool omitDisabledElements = false)
	{
		string[] array = _materialTypeTag.ToString().Split('&', StringSplitOptions.None);
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			foreach (Element element in ElementLoader.elements)
			{
				if ((!element.disabled || !omitDisabledElements) && element.IsSolid && (element.tag == tag || element.HasTag(tag)))
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
		}
		return list;
	}

	// Token: 0x0600A217 RID: 41495 RVA: 0x003ECAE4 File Offset: 0x003EACE4
	public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
	{
		this.activeIngredient = ingredient;
		this.activeRecipe = recipe;
		this.activeMass = ingredient.amount;
		List<Tag> validMaterials = MaterialSelector.GetValidMaterials(ingredient.tag, false);
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (!validMaterials.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.ElementToggles[key].gameObject.SetActive(false);
			Util.KDestroyGameObject(this.ElementToggles[key].gameObject);
			this.ElementToggles.Remove(key);
		}
		foreach (Tag tag in validMaterials)
		{
			if (!this.ElementToggles.ContainsKey(tag))
			{
				GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				KToggle component = gameObject.GetComponent<KToggle>();
				this.ElementToggles.Add(tag, component);
				component.group = this.toggleGroup;
				gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
			}
		}
		this.ConfigureMaterialTooltips();
		this.RefreshToggleContents();
	}

	// Token: 0x0600A218 RID: 41496 RVA: 0x003ECCBC File Offset: 0x003EAEBC
	private void SetToggleBGImage(KToggle toggle, Tag elem)
	{
		if (toggle == this.selectedToggle)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponent<ImageToggleState>().SetActive();
			return;
		}
		if (ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
			toggle.GetComponent<ImageToggleState>().SetInactive();
			return;
		}
		toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
		toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
		if (!MaterialSelector.AllowInsufficientMaterialBuild())
		{
			toggle.GetComponent<ImageToggleState>().SetDisabled();
		}
	}

	// Token: 0x0600A219 RID: 41497 RVA: 0x003ECDB0 File Offset: 0x003EAFB0
	public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
	{
		KToggle x = this.ElementToggles[elem];
		if (x != this.selectedToggle)
		{
			this.selectedToggle = x;
			if (recipe != null)
			{
				SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, recipe.Result, elem);
			}
			this.CurrentSelectedElement = elem;
			if (this.selectMaterialActions != null)
			{
				this.selectMaterialActions();
			}
			this.UpdateHeader();
			this.SetDescription(elem);
			this.SetEffects(elem);
			if (this.MaterialDescriptionPane != null)
			{
				if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
				{
					this.DescriptorsPanel.SetActive(false);
				}
				else
				{
					this.DescriptorsPanel.SetActive(true);
				}
			}
		}
		if (focusScrollRect && this.ElementToggles.Count > 1)
		{
			List<Tag> list = new List<Tag>();
			foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
			{
				list.Add(keyValuePair.Key);
			}
			list.Sort(new Comparison<Tag>(this.ElementSorter));
			float num = (float)list.IndexOf(elem);
			int constraintCount = this.LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
			float num2 = num / (float)constraintCount / (float)Math.Max((list.Count - 1) / constraintCount, 1);
			this.ScrollRect.normalizedPosition = new Vector2(0f, 1f - num2);
		}
		this.RefreshToggleContents();
	}

	// Token: 0x0600A21A RID: 41498 RVA: 0x003ECF54 File Offset: 0x003EB154
	public void RefreshToggleContents()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			KToggle value = keyValuePair.Value;
			Tag elem = keyValuePair.Key;
			GameObject gameObject = value.gameObject;
			bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			if (gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (flag)
			{
				LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
				LocText locText = componentsInChildren[0];
				TMP_Text tmp_Text = componentsInChildren[1];
				Image image = gameObject.GetComponentsInChildren<Image>()[1];
				tmp_Text.text = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true));
				locText.text = Util.FormatWholeNumber(this.activeMass);
				GameObject gameObject2 = Assets.TryGetPrefab(keyValuePair.Key);
				if (gameObject2 != null)
				{
					KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
					image.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
				}
				this.SetToggleBGImage(keyValuePair.Value, keyValuePair.Key);
				value.soundPlayer.AcceptClickCondition = (() => this.IsEnoughMass(elem));
				value.ClearOnClick();
				if (this.IsEnoughMass(elem))
				{
					value.onClick += delegate()
					{
						this.OnSelectMaterial(elem, this.activeRecipe, false);
					};
				}
			}
		}
		this.SortElementToggles();
		this.UpdateHeader();
	}

	// Token: 0x0600A21B RID: 41499 RVA: 0x0010DD30 File Offset: 0x0010BF30
	private bool IsEnoughMass(Tag t)
	{
		return ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || MaterialSelector.AllowInsufficientMaterialBuild();
	}

	// Token: 0x0600A21C RID: 41500 RVA: 0x003ED110 File Offset: 0x003EB310
	public bool AutoSelectAvailableMaterial()
	{
		if (this.activeRecipe == null || this.ElementToggles.Count == 0)
		{
			return false;
		}
		Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, this.activeRecipe.Result);
		if (previousElement != null)
		{
			KToggle x;
			this.ElementToggles.TryGetValue(previousElement, out x);
			if (x != null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, true) >= this.activeMass))
			{
				this.OnSelectMaterial(previousElement, this.activeRecipe, true);
				return true;
			}
		}
		float num = -1f;
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort(new Comparison<Tag>(this.ElementSorter));
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			this.OnSelectMaterial(list[0], this.activeRecipe, true);
			return true;
		}
		Tag tag = null;
		foreach (Tag tag2 in list)
		{
			float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
			if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag2))
			{
				num2 = Mathf.Min(this.activeMass, num2);
			}
			if (num2 >= this.activeMass && num2 > num)
			{
				num = num2;
				tag = tag2;
			}
		}
		if (tag != null)
		{
			UISounds.PlaySound(UISounds.Sound.Object_AutoSelected);
			Element element = ElementLoader.GetElement(tag);
			string arg;
			if (element == null)
			{
				GameObject prefab = Assets.GetPrefab(tag);
				arg = ((prefab != null) ? prefab.GetProperName() : tag.Name);
			}
			else
			{
				arg = element.name;
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(MISC.POPFX.RESOURCE_SELECTION_CHANGED, arg), null, Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()), 1.5f, false, false);
			this.OnSelectMaterial(tag, this.activeRecipe, true);
			return true;
		}
		return false;
	}

	// Token: 0x0600A21D RID: 41501 RVA: 0x003ED390 File Offset: 0x003EB590
	private void SortElementToggles()
	{
		bool flag = false;
		int num = -1;
		this.elementsToSort.Clear();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.elementsToSort.Add(keyValuePair.Key);
			}
		}
		this.elementsToSort.Sort(new Comparison<Tag>(this.ElementSorter));
		for (int i = 0; i < this.elementsToSort.Count; i++)
		{
			int siblingIndex = this.ElementToggles[this.elementsToSort[i]].transform.GetSiblingIndex();
			if (siblingIndex <= num)
			{
				flag = true;
				break;
			}
			num = siblingIndex;
		}
		if (flag)
		{
			foreach (Tag key in this.elementsToSort)
			{
				this.ElementToggles[key].transform.SetAsLastSibling();
			}
		}
		this.UpdateScrollBar();
	}

	// Token: 0x0600A21E RID: 41502 RVA: 0x003ED4D0 File Offset: 0x003EB6D0
	private void ConfigureMaterialTooltips()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			ToolTip component = keyValuePair.Value.gameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = GameUtil.GetMaterialTooltips(keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600A21F RID: 41503 RVA: 0x003ED54C File Offset: 0x003EB74C
	private void UpdateScrollBar()
	{
		if (this.Scrollbar == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		if (this.Scrollbar.activeSelf != num > 5)
		{
			this.Scrollbar.SetActive(num > 5);
		}
		this.ScrollRect.GetComponent<LayoutElement>().minHeight = (float)(74 * ((num <= 5) ? 1 : 2));
	}

	// Token: 0x0600A220 RID: 41504 RVA: 0x003ED5FC File Offset: 0x003EB7FC
	private void UpdateHeader()
	{
		if (this.activeIngredient == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
		string[] array = this.activeIngredient.tag.ToString().Split('&', StringSplitOptions.None);
		string text = array[0].ToTag().ProperName();
		for (int i = 1; i < array.Length; i++)
		{
			text = text + " or " + array[i].ToTag().ProperName();
		}
		if (num == 0)
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, text, GameUtil.GetFormattedMass(this.activeIngredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string text2 = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_DESC, text);
			this.NoMaterialDiscovered.text = text2;
			this.NoMaterialDiscovered.gameObject.SetActive(true);
			this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
			this.BadBG.SetActive(true);
			if (this.Scrollbar != null)
			{
				this.Scrollbar.SetActive(false);
			}
			this.LayoutContainer.SetActive(false);
			return;
		}
		componentInChildren.text = string.Format(UI.PRODUCTINFO_SELECTMATERIAL, text);
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		this.BadBG.SetActive(false);
		this.LayoutContainer.SetActive(true);
		this.UpdateScrollBar();
	}

	// Token: 0x0600A221 RID: 41505 RVA: 0x0010DD6A File Offset: 0x0010BF6A
	public void ToggleShowDescriptorsPanel(bool show)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		this.DescriptorsPanel.gameObject.SetActive(show);
	}

	// Token: 0x0600A222 RID: 41506 RVA: 0x003ED7C0 File Offset: 0x003EB9C0
	private void SetDescription(Tag element)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		StringEntry stringEntry = null;
		if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out stringEntry))
		{
			this.MaterialDescriptionText.text = stringEntry.ToString();
			this.MaterialDescriptionPane.SetActive(true);
			return;
		}
		this.MaterialDescriptionPane.SetActive(false);
	}

	// Token: 0x0600A223 RID: 41507 RVA: 0x003ED838 File Offset: 0x003EBA38
	private void SetEffects(Tag element)
	{
		if (this.MaterialDescriptionPane == null)
		{
			return;
		}
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.MaterialEffectsPane.gameObject.SetActive(true);
			this.MaterialEffectsPane.SetDescriptors(materialDescriptors);
			return;
		}
		this.MaterialEffectsPane.gameObject.SetActive(false);
	}

	// Token: 0x0600A224 RID: 41508 RVA: 0x0010DD8C File Offset: 0x0010BF8C
	public static bool AllowInsufficientMaterialBuild()
	{
		return GenericGameSettings.instance.allowInsufficientMaterialBuild;
	}

	// Token: 0x0600A225 RID: 41509 RVA: 0x003ED8C0 File Offset: 0x003EBAC0
	private int ElementSorter(Tag at, Tag bt)
	{
		GameObject gameObject = Assets.TryGetPrefab(at);
		IHasSortOrder hasSortOrder = (gameObject != null) ? gameObject.GetComponent<IHasSortOrder>() : null;
		GameObject gameObject2 = Assets.TryGetPrefab(bt);
		IHasSortOrder hasSortOrder2 = (gameObject2 != null) ? gameObject2.GetComponent<IHasSortOrder>() : null;
		if (hasSortOrder == null || hasSortOrder2 == null)
		{
			return 0;
		}
		Element element = ElementLoader.GetElement(at);
		Element element2 = ElementLoader.GetElement(bt);
		if (element != null && element2 != null && element.buildMenuSort == element2.buildMenuSort)
		{
			return element.idx.CompareTo(element2.idx);
		}
		return hasSortOrder.sortOrder.CompareTo(hasSortOrder2.sortOrder);
	}

	// Token: 0x04007F18 RID: 32536
	public static List<Tag> DeprioritizeAutoSelectElementList = new List<Tag>
	{
		SimHashes.WoodLog.ToString().ToTag(),
		SimHashes.SolidMercury.ToString().ToTag(),
		SimHashes.Lead.ToString().ToTag()
	};

	// Token: 0x04007F19 RID: 32537
	public Tag CurrentSelectedElement;

	// Token: 0x04007F1A RID: 32538
	public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();

	// Token: 0x04007F1B RID: 32539
	public int selectorIndex;

	// Token: 0x04007F1C RID: 32540
	public MaterialSelector.SelectMaterialActions selectMaterialActions;

	// Token: 0x04007F1D RID: 32541
	public MaterialSelector.SelectMaterialActions deselectMaterialActions;

	// Token: 0x04007F1E RID: 32542
	private ToggleGroup toggleGroup;

	// Token: 0x04007F1F RID: 32543
	public GameObject TogglePrefab;

	// Token: 0x04007F20 RID: 32544
	public GameObject LayoutContainer;

	// Token: 0x04007F21 RID: 32545
	public KScrollRect ScrollRect;

	// Token: 0x04007F22 RID: 32546
	public GameObject Scrollbar;

	// Token: 0x04007F23 RID: 32547
	public GameObject Headerbar;

	// Token: 0x04007F24 RID: 32548
	public GameObject BadBG;

	// Token: 0x04007F25 RID: 32549
	public LocText NoMaterialDiscovered;

	// Token: 0x04007F26 RID: 32550
	public GameObject MaterialDescriptionPane;

	// Token: 0x04007F27 RID: 32551
	public LocText MaterialDescriptionText;

	// Token: 0x04007F28 RID: 32552
	public DescriptorPanel MaterialEffectsPane;

	// Token: 0x04007F29 RID: 32553
	public GameObject DescriptorsPanel;

	// Token: 0x04007F2A RID: 32554
	private KToggle selectedToggle;

	// Token: 0x04007F2B RID: 32555
	private Recipe.Ingredient activeIngredient;

	// Token: 0x04007F2C RID: 32556
	private Recipe activeRecipe;

	// Token: 0x04007F2D RID: 32557
	private float activeMass;

	// Token: 0x04007F2E RID: 32558
	private List<Tag> elementsToSort = new List<Tag>();

	// Token: 0x02001E45 RID: 7749
	// (Invoke) Token: 0x0600A229 RID: 41513
	public delegate void SelectMaterialActions();
}
