using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F4E RID: 8014
public class SandboxToolParameterMenu : KScreen
{
	// Token: 0x0600A8F7 RID: 43255 RVA: 0x001120E1 File Offset: 0x001102E1
	public static void DestroyInstance()
	{
		SandboxToolParameterMenu.instance = null;
	}

	// Token: 0x0600A8F8 RID: 43256 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x0600A8F9 RID: 43257 RVA: 0x001120E9 File Offset: 0x001102E9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ConfigureSettings();
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A8FA RID: 43258 RVA: 0x0040E5E4 File Offset: 0x0040C7E4
	private void ConfigureSettings()
	{
		this.massSlider.clampValueLow = 0.001f;
		this.massSlider.clampValueHigh = 10000f;
		this.temperatureAdditiveSlider.clampValueLow = -9999f;
		this.temperatureAdditiveSlider.clampValueHigh = 9999f;
		this.temperatureSlider.clampValueLow = -458f;
		this.temperatureSlider.clampValueHigh = 9999f;
		this.brushRadiusSlider.clampValueLow = 1f;
		this.brushRadiusSlider.clampValueHigh = 50f;
		this.diseaseCountSlider.clampValueHigh = 1000000f;
		this.diseaseCountSlider.slideMaxValue = 1000000f;
		this.settings = new SandboxSettings();
		SandboxSettings sandboxSettings = this.settings;
		sandboxSettings.OnChangeElement = (Action<bool>)Delegate.Combine(sandboxSettings.OnChangeElement, new Action<bool>(delegate(bool forceElementDefaults)
		{
			int num = this.settings.GetIntSetting("SandboxTools.SelectedElement");
			if (num >= ElementLoader.elements.Count)
			{
				num = 0;
			}
			Element element = ElementLoader.elements[num];
			this.elementSelector.button.GetComponentInChildren<LocText>().text = element.name + " (" + element.GetStateString() + ")";
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
			this.elementSelector.button.GetComponentsInChildren<Image>()[1].sprite = uisprite.first;
			this.elementSelector.button.GetComponentsInChildren<Image>()[1].color = uisprite.second;
			this.SetAbsoluteTemperatureSliderRange(element);
			this.massSlider.SetRange(0.1f, Mathf.Min(element.maxMass * 2f, this.massSlider.clampValueHigh), false);
			if (forceElementDefaults)
			{
				this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(element.defaultValues.temperature, true), true);
				this.massSlider.SetValue(element.defaultValues.mass, true);
			}
		}));
		SandboxSettings sandboxSettings2 = this.settings;
		sandboxSettings2.OnChangeMass = (System.Action)Delegate.Combine(sandboxSettings2.OnChangeMass, new System.Action(delegate()
		{
			this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"), false);
		}));
		SandboxSettings sandboxSettings3 = this.settings;
		sandboxSettings3.OnChangeDisease = (System.Action)Delegate.Combine(sandboxSettings3.OnChangeDisease, new System.Action(delegate()
		{
			Disease disease = Db.Get().Diseases.TryGet(SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedDisease"));
			if (disease == null)
			{
				disease = Db.Get().Diseases.Get("FoodPoisoning");
			}
			this.diseaseSelector.button.GetComponentInChildren<LocText>().text = disease.Name;
			this.diseaseSelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite("germ");
			this.diseaseCountSlider.SetRange(0f, 1000000f, false);
		}));
		SandboxSettings sandboxSettings4 = this.settings;
		sandboxSettings4.OnChangeDiseaseCount = (System.Action)Delegate.Combine(sandboxSettings4.OnChangeDiseaseCount, new System.Action(delegate()
		{
			this.diseaseCountSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.DiseaseCount"), false);
		}));
		SandboxSettings sandboxSettings5 = this.settings;
		sandboxSettings5.OnChangeStory = (System.Action)Delegate.Combine(sandboxSettings5.OnChangeStory, new System.Action(delegate()
		{
			string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
			Story story = Db.Get().Stories.Get(stringSetting);
			if (story == null)
			{
				this.settings.ForceDefaultStringSetting("SandboxTools.SelectedStory");
				return;
			}
			this.storySelector.button.GetComponentInChildren<LocText>().text = Strings.Get(story.StoryTrait.name);
			this.storySelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite(story.StoryTrait.icon);
		}));
		SandboxSettings sandboxSettings6 = this.settings;
		sandboxSettings6.OnChangeEntity = (System.Action)Delegate.Combine(sandboxSettings6.OnChangeEntity, new System.Action(delegate()
		{
			string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
			GameObject gameObject = Assets.TryGetPrefab(stringSetting);
			if (gameObject == null || !Game.IsCorrectDlcActiveForCurrentSave(gameObject.GetComponent<KPrefabID>()))
			{
				this.settings.ForceDefaultStringSetting("SandboxTools.SelectedEntity");
				return;
			}
			this.entitySelector.button.GetComponentInChildren<LocText>().text = gameObject.GetProperName();
			global::Tuple<Sprite, Color> tuple;
			if (gameObject.HasTag(GameTags.BaseMinion))
			{
				tuple = new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(gameObject.PrefabID()), Color.white);
			}
			else
			{
				tuple = Def.GetUISprite(stringSetting, "ui", false);
			}
			if (tuple != null)
			{
				this.entitySelector.button.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
				this.entitySelector.button.GetComponentsInChildren<Image>()[1].color = tuple.second;
			}
		}));
		SandboxSettings sandboxSettings7 = this.settings;
		sandboxSettings7.OnChangeBrushSize = (System.Action)Delegate.Combine(sandboxSettings7.OnChangeBrushSize, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is BrushTool)
			{
				(PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings8 = this.settings;
		sandboxSettings8.OnChangeNoiseScale = (System.Action)Delegate.Combine(sandboxSettings8.OnChangeNoiseScale, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is SandboxSprinkleTool)
			{
				(PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings9 = this.settings;
		sandboxSettings9.OnChangeNoiseDensity = (System.Action)Delegate.Combine(sandboxSettings9.OnChangeNoiseDensity, new System.Action(delegate()
		{
			if (PlayerController.Instance.ActiveTool is SandboxSprinkleTool)
			{
				(PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
			}
		}));
		SandboxSettings sandboxSettings10 = this.settings;
		sandboxSettings10.OnChangeTemperature = (System.Action)Delegate.Combine(sandboxSettings10.OnChangeTemperature, new System.Action(delegate()
		{
			this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature"), false), false);
		}));
		SandboxSettings sandboxSettings11 = this.settings;
		sandboxSettings11.OnChangeAdditiveTemperature = (System.Action)Delegate.Combine(sandboxSettings11.OnChangeAdditiveTemperature, new System.Action(delegate()
		{
			this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true), false);
		}));
		Game.Instance.Subscribe(999382396, new Action<object>(this.OnTemperatureUnitChanged));
		SandboxSettings sandboxSettings12 = this.settings;
		sandboxSettings12.OnChangeAdditiveStress = (System.Action)Delegate.Combine(sandboxSettings12.OnChangeAdditiveStress, new System.Action(delegate()
		{
			this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"), false);
		}));
		SandboxSettings sandboxSettings13 = this.settings;
		sandboxSettings13.OnChangeMoraleAdjustment = (System.Action)Delegate.Combine(sandboxSettings13.OnChangeMoraleAdjustment, new System.Action(delegate()
		{
			this.moraleSlider.SetValue((float)this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"), false);
		}));
	}

	// Token: 0x0600A8FB RID: 43259 RVA: 0x0040E8B4 File Offset: 0x0040CAB4
	public void DisableParameters()
	{
		this.elementSelector.row.SetActive(false);
		this.entitySelector.row.SetActive(false);
		this.brushRadiusSlider.row.SetActive(false);
		this.noiseScaleSlider.row.SetActive(false);
		this.noiseDensitySlider.row.SetActive(false);
		this.massSlider.row.SetActive(false);
		this.temperatureAdditiveSlider.row.SetActive(false);
		this.temperatureSlider.row.SetActive(false);
		this.diseaseCountSlider.row.SetActive(false);
		this.diseaseSelector.row.SetActive(false);
		this.stressAdditiveSlider.row.SetActive(false);
		this.moraleSlider.row.SetActive(false);
		this.storySelector.row.SetActive(false);
	}

	// Token: 0x0600A8FC RID: 43260 RVA: 0x0040E9A0 File Offset: 0x0040CBA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureElementSelector();
		this.ConfigureDiseaseSelector();
		this.ConfigureEntitySelector();
		this.ConfigureStoryTraitSelector();
		this.SpawnSelector(this.entitySelector);
		this.SpawnSelector(this.elementSelector);
		this.SpawnSelector(this.storySelector);
		this.SpawnSlider(this.brushRadiusSlider);
		this.SpawnSlider(this.noiseScaleSlider);
		this.SpawnSlider(this.noiseDensitySlider);
		this.SpawnSlider(this.massSlider);
		this.SpawnSlider(this.temperatureSlider);
		this.SpawnSlider(this.temperatureAdditiveSlider);
		this.SpawnSlider(this.stressAdditiveSlider);
		this.SpawnSelector(this.diseaseSelector);
		this.SpawnSlider(this.diseaseCountSlider);
		this.SpawnSlider(this.moraleSlider);
		if (SandboxToolParameterMenu.instance == null)
		{
			SandboxToolParameterMenu.instance = this;
			base.gameObject.SetActive(false);
			this.settings.RestorePrefs();
		}
	}

	// Token: 0x0600A8FD RID: 43261 RVA: 0x0040EAA0 File Offset: 0x0040CCA0
	private void ConfigureElementSelector()
	{
		Func<object, bool> condition = (object element) => (element as Element).IsSolid;
		Func<object, bool> condition2 = (object element) => (element as Element).IsLiquid;
		Func<object, bool> condition3 = (object element) => (element as Element).IsGas;
		List<Element> commonElements = new List<Element>();
		Func<object, bool> condition4 = (object element) => commonElements.Contains(element as Element);
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Oxygen));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Water));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Vacuum));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Dirt));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SandStone));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Cuprite));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Steel));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Algae));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CrudeOil));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CarbonDioxide));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Sand));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SlimeMold));
		commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Granite));
		List<Element> list = new List<Element>();
		foreach (Element element2 in ElementLoader.elements)
		{
			if (!element2.disabled)
			{
				bool flag = false;
				Tag[] oreTags = element2.oreTags;
				for (int i = 0; i < oreTags.Length; i++)
				{
					if (oreTags[i] == GameTags.HideFromSpawnTool)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(element2);
				}
			}
		}
		list.Sort((Element a, Element b) => a.name.CompareTo(b.name));
		object[] options = list.ToArray();
		this.elementSelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object element)
		{
			this.settings.SetIntSetting("SandboxTools.SelectedElement", (int)((Element)element).idx);
		}, (object element) => (element as Element).name + " (" + (element as Element).GetStateString() + ")", (string filterString, object option) => ((option as Element).name.ToUpper() + (option as Element).GetStateString().ToUpper()).Contains(filterString.ToUpper()), (object element) => Def.GetUISprite(element as Element, "ui", false), UI.SANDBOXTOOLS.SETTINGS.ELEMENT.NAME, new SandboxToolParameterMenu.SelectorValue.SearchFilter[]
		{
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.COMMON, condition4, null, null),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.SOLID, condition, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.SandStone), "ui", false)),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.LIQUID, condition2, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.Water), "ui", false)),
			new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.GAS, condition3, null, Def.GetUISprite(ElementLoader.FindElementByHash(SimHashes.Oxygen), "ui", false))
		});
	}

	// Token: 0x0600A8FE RID: 43262 RVA: 0x0040EE30 File Offset: 0x0040D030
	private void ConfigureEntitySelector()
	{
		List<SandboxToolParameterMenu.SelectorValue.SearchFilter> list = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
		SandboxToolParameterMenu.SelectorValue.SearchFilter item = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.FOOD, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			string idString = kprefabID2.PrefabID().ToString();
			return (!kprefabID2.HasTag(GameTags.Egg) && EdiblesManager.GetAllFoodTypes().Find((EdiblesManager.FoodInfo match) => match.Id == idString) != null) || kprefabID2.HasTag(GameTags.Dehydrated);
		}, null, Def.GetUISprite(Assets.GetPrefab("MushBar"), "ui", false));
		list.Add(item);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.COMETS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return kprefabID2.HasTag(GameTags.Comet) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2);
		}, null, Def.GetUISprite(Assets.GetPrefab(CopperCometConfig.ID), "ui", false));
		list.Add(item2);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item3 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL, delegate(object entity)
		{
			KPrefabID restrictions = entity as KPrefabID;
			return (entity as KPrefabID).HasTag(GameTags.BaseMinion) && Game.IsCorrectDlcActiveForCurrentSave(restrictions);
		}, null, new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(GameTags.Minions.Models.Standard), Color.white));
		list.Add(item3);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			SandboxToolParameterMenu.SelectorValue.SearchFilter item4 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.BIONICUPGRADES, delegate(object entity)
			{
				KPrefabID kprefabID2 = entity as KPrefabID;
				return kprefabID2.HasTag(GameTags.BionicUpgrade) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2);
			}, null, new global::Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("upgrade_disc_kanim"), "ui", false, ""), Color.white));
			list.Add(item4);
		}
		SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter = null;
		searchFilter = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE, (object entity) => false, null, Def.GetUISprite(Assets.GetPrefab("Hatch"), "ui", false));
		list.Add(searchFilter);
		List<Tag> list2 = new List<Tag>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.CreatureBrain))
		{
			CreatureBrain brain = gameObject.GetComponent<CreatureBrain>();
			if (!list2.Contains(brain.species) && Game.IsCorrectDlcActiveForCurrentSave(brain.GetComponent<KPrefabID>()))
			{
				global::Tuple<Sprite, Color> icon = new global::Tuple<Sprite, Color>(CodexCache.entries[brain.species.ToString().ToUpper()].icon, CodexCache.entries[brain.species.ToString().ToUpper()].iconColor);
				list2.Add(brain.species);
				SandboxToolParameterMenu.SelectorValue.SearchFilter item5 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + brain.species.ToString().ToUpper()), delegate(object entity)
				{
					CreatureBrain component = Assets.GetPrefab((entity as KPrefabID).PrefabID()).GetComponent<CreatureBrain>();
					return (entity as KPrefabID).HasTag(GameTags.CreatureBrain) && component.species == brain.species;
				}, searchFilter, icon);
				list.Add(item5);
			}
		}
		SandboxToolParameterMenu.SelectorValue.SearchFilter item6 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE_EGG, (object entity) => (entity as KPrefabID).HasTag(GameTags.Egg), searchFilter, Def.GetUISprite(Assets.GetPrefab("HatchEgg"), "ui", false));
		list.Add(item6);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item7 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.EQUIPMENT, delegate(object entity)
		{
			if ((entity as KPrefabID).gameObject == null)
			{
				return false;
			}
			GameObject gameObject2 = (entity as KPrefabID).gameObject;
			return gameObject2 != null && gameObject2.GetComponent<Equippable>() != null;
		}, null, Def.GetUISprite(Assets.GetPrefab("Funky_Vest"), "ui", false));
		list.Add(item7);
		SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.PLANTS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return !(kprefabID2 == null) && !(kprefabID2.gameObject == null) && (kprefabID2 != null && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2)) && (kprefabID2.GetComponent<Harvestable>() != null || kprefabID2.GetComponent<WiltCondition>() != null);
		}, null, Def.GetUISprite(Assets.GetPrefab("PrickleFlower"), "ui", false));
		list.Add(searchFilter2);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item8 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.SEEDS, delegate(object entity)
		{
			if ((entity as KPrefabID).gameObject == null)
			{
				return false;
			}
			GameObject gameObject2 = (entity as KPrefabID).gameObject;
			return gameObject2 != null && gameObject2.GetComponent<PlantableSeed>() != null;
		}, searchFilter2, Def.GetUISprite(Assets.GetPrefab("PrickleFlowerSeed"), "ui", false));
		list.Add(item8);
		SandboxToolParameterMenu.SelectorValue.SearchFilter item9 = new SandboxToolParameterMenu.SelectorValue.SearchFilter(UI.SANDBOXTOOLS.FILTERS.ENTITIES.INDUSTRIAL_PRODUCTS, delegate(object entity)
		{
			KPrefabID kprefabID2 = entity as KPrefabID;
			return !(kprefabID2 == null) && !(kprefabID2.gameObject == null) && !kprefabID2.HasTag(GameTags.DeprecatedContent) && Game.IsCorrectDlcActiveForCurrentSave(kprefabID2) && (kprefabID2.HasTag(GameTags.IndustrialIngredient) || kprefabID2.HasTag(GameTags.IndustrialProduct) || kprefabID2.HasTag(GameTags.Medicine) || kprefabID2.HasTag(GameTags.MedicalSupplies) || kprefabID2.HasTag(GameTags.ChargedPortableBattery));
		}, null, Def.GetUISprite(Assets.GetPrefab("BasicCure"), "ui", false));
		list.Add(item9);
		List<KPrefabID> list3 = new List<KPrefabID>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(kprefabID) && !kprefabID.HasTag(GameTags.HideFromSpawnTool))
			{
				using (List<SandboxToolParameterMenu.SelectorValue.SearchFilter>.Enumerator enumerator3 = list.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.condition(kprefabID))
						{
							list3.Add(kprefabID);
							break;
						}
					}
				}
			}
		}
		object[] options = list3.ToArray();
		this.entitySelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object entity)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedEntity", (entity as KPrefabID).PrefabID().Name);
		}, (object entity) => (entity as KPrefabID).GetProperName(), null, delegate(object entity)
		{
			GameObject prefab = Assets.GetPrefab((entity as KPrefabID).PrefabTag);
			KPrefabID component = prefab.GetComponent<KPrefabID>();
			if (prefab != null)
			{
				if (component.HasTag(GameTags.BaseMinion))
				{
					return new global::Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel((entity as KPrefabID).PrefabID()), Color.white);
				}
				KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
				if (component2 != null && component2.AnimFiles.Length != 0 && component2.AnimFiles[0] != null)
				{
					return Def.GetUISprite(prefab, "ui", false);
				}
			}
			return null;
		}, UI.SANDBOXTOOLS.SETTINGS.SPAWN_ENTITY.NAME, list.ToArray());
	}

	// Token: 0x0600A8FF RID: 43263 RVA: 0x0040F42C File Offset: 0x0040D62C
	private void ConfigureStoryTraitSelector()
	{
		object[] options = Db.Get().Stories.resources.ToArray();
		this.storySelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object story)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedStory", ((Story)story).Id);
		}, (object story) => Strings.Get((story as Story).StoryTrait.name), null, (object story) => new global::Tuple<Sprite, Color>(Assets.GetSprite(((Story)story).StoryTrait.icon), Color.white), UI.SANDBOXTOOLS.SETTINGS.SPAWN_STORY_TRAIT.NAME, null);
	}

	// Token: 0x0600A900 RID: 43264 RVA: 0x0040F4B0 File Offset: 0x0040D6B0
	private void ConfigureDiseaseSelector()
	{
		object[] options = Db.Get().Diseases.resources.ToArray();
		this.diseaseSelector = new SandboxToolParameterMenu.SelectorValue(options, delegate(object disease)
		{
			this.settings.SetStringSetting("SandboxTools.SelectedDisease", ((Disease)disease).Id);
		}, (object disease) => (disease as Disease).Name, null, (object disease) => new global::Tuple<Sprite, Color>(Assets.GetSprite("germ"), GlobalAssets.Instance.colorSet.GetColorByName((disease as Disease).overlayColourName)), UI.SANDBOXTOOLS.SETTINGS.DISEASE.NAME, null);
	}

	// Token: 0x0600A901 RID: 43265 RVA: 0x00112105 File Offset: 0x00110305
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (PlayerController.Instance.ActiveTool != null && SandboxToolParameterMenu.instance != null)
		{
			this.RefreshDisplay();
		}
	}

	// Token: 0x0600A902 RID: 43266 RVA: 0x0040F534 File Offset: 0x0040D734
	public void RefreshDisplay()
	{
		this.brushRadiusSlider.row.SetActive(PlayerController.Instance.ActiveTool is BrushTool);
		if (PlayerController.Instance.ActiveTool is BrushTool)
		{
			this.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
		}
		this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"), true);
		this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"), true);
		this.RefreshTemperatureUnitDisplays();
		this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature"), true), true);
		this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true), true);
		this.diseaseCountSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.DiseaseCount"), true);
		this.moraleSlider.SetValue((float)this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"), true);
	}

	// Token: 0x0600A903 RID: 43267 RVA: 0x0040F650 File Offset: 0x0040D850
	private void OnTemperatureUnitChanged(object unit)
	{
		int num = this.settings.GetIntSetting("SandboxTools.SelectedElement");
		if (num >= ElementLoader.elements.Count)
		{
			num = 0;
		}
		Element absoluteTemperatureSliderRange = ElementLoader.elements[num];
		this.SetAbsoluteTemperatureSliderRange(absoluteTemperatureSliderRange);
		this.temperatureAdditiveSlider.SetValue(5f, true);
	}

	// Token: 0x0600A904 RID: 43268 RVA: 0x0040F6A4 File Offset: 0x0040D8A4
	private void SetAbsoluteTemperatureSliderRange(Element element)
	{
		float num = Mathf.Max(element.lowTemp - 10f, 1f);
		float num2;
		if (element.IsGas)
		{
			num2 = Mathf.Min(new float[]
			{
				9999f,
				element.highTemp + 10f,
				element.defaultValues.temperature + 100f
			});
		}
		else
		{
			num2 = Mathf.Min(9999f, element.highTemp + 10f);
		}
		num = GameUtil.GetConvertedTemperature(num, true);
		num2 = GameUtil.GetConvertedTemperature(num2, true);
		this.temperatureSlider.SetRange(num, num2, false);
	}

	// Token: 0x0600A905 RID: 43269 RVA: 0x0040F740 File Offset: 0x0040D940
	private void RefreshTemperatureUnitDisplays()
	{
		this.temperatureSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
		this.temperatureSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
		this.temperatureAdditiveSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
		this.temperatureAdditiveSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
	}

	// Token: 0x0600A906 RID: 43270 RVA: 0x0040F7C4 File Offset: 0x0040D9C4
	private GameObject SpawnSelector(SandboxToolParameterMenu.SelectorValue selector)
	{
		GameObject gameObject = Util.KInstantiateUI(this.selectorPropertyPrefab, base.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		GameObject panel = component.GetReference("ScrollPanel").gameObject;
		GameObject gameObject2 = component.GetReference("Content").gameObject;
		InputField filterInputField = component.GetReference<InputField>("Filter");
		component.GetReference<LocText>("Label").SetText(selector.labelText);
		Game.Instance.Subscribe(1174281782, delegate(object data)
		{
			if (panel.activeSelf)
			{
				panel.SetActive(false);
			}
		});
		KButton reference = component.GetReference<KButton>("Button");
		reference.onClick += delegate()
		{
			panel.SetActive(!panel.activeSelf);
			if (panel.activeSelf)
			{
				panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
				filterInputField.ActivateInputField();
				filterInputField.onValueChanged.Invoke(filterInputField.text);
			}
		};
		GameObject gameObject3 = component.GetReference("optionPrefab").gameObject;
		selector.row = gameObject;
		selector.optionButtons = new List<KeyValuePair<object, GameObject>>();
		GameObject clearFilterButton = Util.KInstantiateUI(gameObject3, gameObject2, false);
		clearFilterButton.GetComponentInChildren<LocText>().text = UI.SANDBOXTOOLS.FILTERS.BACK;
		clearFilterButton.GetComponentsInChildren<Image>()[1].enabled = false;
		clearFilterButton.GetComponent<KButton>().onClick += delegate()
		{
			selector.currentFilter = null;
			selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
			{
				if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
				{
					test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
					return;
				}
				test.Value.SetActive(false);
			});
			clearFilterButton.SetActive(false);
			panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
			filterInputField.text = "";
			filterInputField.onValueChanged.Invoke(filterInputField.text);
		};
		if (selector.filters != null)
		{
			SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = selector.filters;
			for (int i = 0; i < filters.Length; i++)
			{
				SandboxToolParameterMenu.SelectorValue.SearchFilter filter = filters[i];
				GameObject gameObject4 = Util.KInstantiateUI(gameObject3, gameObject2, false);
				gameObject4.SetActive(filter.parentFilter == null);
				gameObject4.GetComponentInChildren<LocText>().text = filter.Name;
				if (filter.icon != null)
				{
					gameObject4.GetComponentsInChildren<Image>()[1].sprite = filter.icon.first;
					gameObject4.GetComponentsInChildren<Image>()[1].color = filter.icon.second;
				}
				Action<KeyValuePair<object, GameObject>> <>9__6;
				gameObject4.GetComponent<KButton>().onClick += delegate()
				{
					selector.currentFilter = filter;
					clearFilterButton.SetActive(true);
					List<KeyValuePair<object, GameObject>> optionButtons = selector.optionButtons;
					Action<KeyValuePair<object, GameObject>> action;
					if ((action = <>9__6) == null)
					{
						action = (<>9__6 = delegate(KeyValuePair<object, GameObject> test)
						{
							if (!(test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter))
							{
								test.Value.SetActive(selector.runCurrentFilter(test.Key));
								return;
							}
							if ((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null)
							{
								test.Value.SetActive(false);
								return;
							}
							test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == filter);
						});
					}
					optionButtons.ForEach(action);
					panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
				};
				selector.optionButtons.Add(new KeyValuePair<object, GameObject>(filter, gameObject4));
			}
		}
		object[] options = selector.options;
		for (int i = 0; i < options.Length; i++)
		{
			object option = options[i];
			GameObject gameObject5 = Util.KInstantiateUI(gameObject3, gameObject2, true);
			gameObject5.GetComponentInChildren<LocText>().text = selector.getOptionName(option);
			gameObject5.GetComponent<KButton>().onClick += delegate()
			{
				selector.onValueChanged(option);
				panel.SetActive(false);
			};
			global::Tuple<Sprite, Color> tuple = selector.getOptionSprite(option);
			gameObject5.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
			gameObject5.GetComponentsInChildren<Image>()[1].color = tuple.second;
			selector.optionButtons.Add(new KeyValuePair<object, GameObject>(option, gameObject5));
			if (option is SandboxToolParameterMenu.SelectorValue.SearchFilter)
			{
				gameObject5.SetActive((option as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
			}
			else
			{
				gameObject5.SetActive(false);
			}
		}
		selector.button = reference;
		filterInputField.onValueChanged.AddListener(delegate(string filterString)
		{
			if (!clearFilterButton.activeSelf && !string.IsNullOrEmpty(filterString))
			{
				clearFilterButton.SetActive(true);
			}
			new List<KeyValuePair<object, GameObject>>();
			bool flag = selector.optionButtons.Find((KeyValuePair<object, GameObject> match) => match.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter).Key != null;
			if (string.IsNullOrEmpty(filterString))
			{
				if (!flag)
				{
					selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
					{
						test.Value.SetActive(true);
					});
				}
				else
				{
					selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
					{
						if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter && ((SandboxToolParameterMenu.SelectorValue.SearchFilter)test.Key).parentFilter == null)
						{
							test.Value.SetActive(true);
							return;
						}
						test.Value.SetActive(false);
					});
				}
			}
			else
			{
				selector.optionButtons.ForEach(delegate(KeyValuePair<object, GameObject> test)
				{
					if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
					{
						test.Value.SetActive(((SandboxToolParameterMenu.SelectorValue.SearchFilter)test.Key).Name.ToUpper().Contains(filterString.ToUpper()));
						return;
					}
					test.Value.SetActive(selector.getOptionName(test.Key).ToUpper().Contains(filterString.ToUpper()));
				});
			}
			if (selector.filterOptionFunction != null)
			{
				object[] options2 = selector.options;
				for (int j = 0; j < options2.Length; j++)
				{
					object option = options2[j];
					foreach (KeyValuePair<object, GameObject> keyValuePair in selector.optionButtons.FindAll((KeyValuePair<object, GameObject> match) => match.Key == option))
					{
						if (string.IsNullOrEmpty(filterString))
						{
							keyValuePair.Value.SetActive(false);
						}
						else
						{
							keyValuePair.Value.SetActive(selector.filterOptionFunction(filterString, option));
						}
					}
				}
			}
			panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
		});
		this.inputFields.Add(filterInputField.gameObject);
		panel.SetActive(false);
		return gameObject;
	}

	// Token: 0x0600A907 RID: 43271 RVA: 0x0040FB9C File Offset: 0x0040DD9C
	private GameObject SpawnSlider(SandboxToolParameterMenu.SliderValue value)
	{
		GameObject gameObject = Util.KInstantiateUI(this.sliderPropertyPrefab, base.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("BottomIcon").sprite = Assets.GetSprite(value.bottomSprite);
		component.GetReference<Image>("TopIcon").sprite = Assets.GetSprite(value.topSprite);
		component.GetReference<LocText>("Label").SetText(value.labelText);
		KSlider slider = component.GetReference<KSlider>("Slider");
		KNumberInputField inputField = component.GetReference<KNumberInputField>("InputField");
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(value.tooltip);
		slider.minValue = value.slideMinValue;
		slider.maxValue = value.slideMaxValue;
		inputField.minValue = value.clampValueLow;
		inputField.maxValue = value.clampValueHigh;
		this.inputFields.Add(inputField.gameObject);
		value.slider = slider;
		inputField.decimalPlaces = value.roundToDecimalPlaces;
		value.inputField = inputField;
		value.row = gameObject;
		slider.onReleaseHandle += delegate()
		{
			float value2 = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = value2;
			inputField.currentValue = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		slider.onDrag += delegate()
		{
			float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = num;
			inputField.currentValue = num;
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		slider.onMove += delegate()
		{
			float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			slider.value = num;
			inputField.currentValue = num;
			inputField.SetDisplayValue(inputField.currentValue.ToString());
			if (value.onValueChanged != null)
			{
				value.onValueChanged(slider.value);
			}
		};
		inputField.onEndEdit += delegate()
		{
			float num = inputField.currentValue;
			num = Mathf.Round(num * Mathf.Pow(10f, (float)value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float)value.roundToDecimalPlaces);
			inputField.SetDisplayValue(num.ToString());
			slider.value = num;
			if (value.onValueChanged != null)
			{
				value.onValueChanged(num);
			}
		};
		component.GetReference<LocText>("UnitLabel").text = value.unitString;
		return gameObject;
	}

	// Token: 0x0600A908 RID: 43272 RVA: 0x00112132 File Offset: 0x00110332
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600A909 RID: 43273 RVA: 0x0040FD98 File Offset: 0x0040DF98
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (GameObject gameObject in this.inputFields)
				{
					if (currentSelectedGameObject == gameObject.gameObject)
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04008506 RID: 34054
	public static SandboxToolParameterMenu instance;

	// Token: 0x04008507 RID: 34055
	public SandboxSettings settings;

	// Token: 0x04008508 RID: 34056
	[SerializeField]
	private GameObject sliderPropertyPrefab;

	// Token: 0x04008509 RID: 34057
	[SerializeField]
	private GameObject selectorPropertyPrefab;

	// Token: 0x0400850A RID: 34058
	private List<GameObject> inputFields = new List<GameObject>();

	// Token: 0x0400850B RID: 34059
	public SandboxToolParameterMenu.SelectorValue elementSelector;

	// Token: 0x0400850C RID: 34060
	public SandboxToolParameterMenu.SliderValue brushRadiusSlider = new SandboxToolParameterMenu.SliderValue(1f, 10f, "dash", "circle_hard", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.BrushSize", Mathf.Clamp(Mathf.RoundToInt(value), 1, 50));
	}, 0);

	// Token: 0x0400850D RID: 34061
	public SandboxToolParameterMenu.SliderValue noiseScaleSlider = new SandboxToolParameterMenu.SliderValue(0f, 1f, "little", "lots", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseScale", value);
	}, 2);

	// Token: 0x0400850E RID: 34062
	public SandboxToolParameterMenu.SliderValue noiseDensitySlider = new SandboxToolParameterMenu.SliderValue(1f, 20f, "little", "lots", "", UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_DENSITY.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseDensity", value);
	}, 2);

	// Token: 0x0400850F RID: 34063
	public SandboxToolParameterMenu.SliderValue massSlider = new SandboxToolParameterMenu.SliderValue(0.1f, 1000f, "action_pacify", "status_item_plant_solid", UI.UNITSUFFIXES.MASS.KILOGRAM, UI.SANDBOXTOOLS.SETTINGS.MASS.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.MASS.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Clamp(value, 0.001f, 9999f));
	}, 2);

	// Token: 0x04008510 RID: 34064
	public SandboxToolParameterMenu.SliderValue temperatureSlider = new SandboxToolParameterMenu.SliderValue(150f, 500f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Clamp(GameUtil.GetTemperatureConvertedToKelvin(value), 1f, 9999f));
	}, 0);

	// Token: 0x04008511 RID: 34065
	public SandboxToolParameterMenu.SliderValue temperatureAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-15f, 15f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.TemperatureAdditive", GameUtil.GetTemperatureConvertedToKelvin(value));
	}, 0);

	// Token: 0x04008512 RID: 34066
	public SandboxToolParameterMenu.SliderValue stressAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-10f, 10f, "little", "lots", UI.UNITSUFFIXES.PERCENT, UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.StressAdditive", value);
	}, 0);

	// Token: 0x04008513 RID: 34067
	public SandboxToolParameterMenu.SliderValue moraleSlider = new SandboxToolParameterMenu.SliderValue(-25f, 25f, "little", "lots", UI.UNITSUFFIXES.UNITS, UI.SANDBOXTOOLS.SETTINGS.MORALE.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.MORALE.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandbosTools.MoraleAdjustment", Mathf.RoundToInt(value));
	}, 0);

	// Token: 0x04008514 RID: 34068
	public SandboxToolParameterMenu.SelectorValue diseaseSelector;

	// Token: 0x04008515 RID: 34069
	public SandboxToolParameterMenu.SliderValue diseaseCountSlider = new SandboxToolParameterMenu.SliderValue(0f, 10000f, "status_item_barren", "germ", UI.UNITSUFFIXES.DISEASE.UNITS, UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.TOOLTIP, UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.NAME, delegate(float value)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Mathf.RoundToInt(value));
	}, 0);

	// Token: 0x04008516 RID: 34070
	public SandboxToolParameterMenu.SelectorValue entitySelector;

	// Token: 0x04008517 RID: 34071
	public SandboxToolParameterMenu.SelectorValue storySelector;

	// Token: 0x02001F4F RID: 8015
	public class SelectorValue
	{
		// Token: 0x0600A91B RID: 43291 RVA: 0x004104D4 File Offset: 0x0040E6D4
		public SelectorValue(object[] options, Action<object> onValueChanged, Func<object, string> getOptionName, Func<string, object, bool> filterOptionFunction, Func<object, global::Tuple<Sprite, Color>> getOptionSprite, string labelText, SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = null)
		{
			this.options = options;
			this.onValueChanged = onValueChanged;
			this.getOptionName = getOptionName;
			this.filterOptionFunction = filterOptionFunction;
			this.getOptionSprite = getOptionSprite;
			this.filters = filters;
			this.labelText = labelText;
		}

		// Token: 0x0600A91C RID: 43292 RVA: 0x001122BD File Offset: 0x001104BD
		public bool runCurrentFilter(object obj)
		{
			return this.currentFilter == null || this.currentFilter.condition(obj);
		}

		// Token: 0x04008518 RID: 34072
		public GameObject row;

		// Token: 0x04008519 RID: 34073
		public List<KeyValuePair<object, GameObject>> optionButtons;

		// Token: 0x0400851A RID: 34074
		public KButton button;

		// Token: 0x0400851B RID: 34075
		public object[] options;

		// Token: 0x0400851C RID: 34076
		public Action<object> onValueChanged;

		// Token: 0x0400851D RID: 34077
		public Func<object, string> getOptionName;

		// Token: 0x0400851E RID: 34078
		public Func<string, object, bool> filterOptionFunction;

		// Token: 0x0400851F RID: 34079
		public Func<object, global::Tuple<Sprite, Color>> getOptionSprite;

		// Token: 0x04008520 RID: 34080
		public SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters;

		// Token: 0x04008521 RID: 34081
		public List<SandboxToolParameterMenu.SelectorValue.SearchFilter> activeFilters = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();

		// Token: 0x04008522 RID: 34082
		public SandboxToolParameterMenu.SelectorValue.SearchFilter currentFilter;

		// Token: 0x04008523 RID: 34083
		public string labelText;

		// Token: 0x02001F50 RID: 8016
		public class SearchFilter
		{
			// Token: 0x0600A91D RID: 43293 RVA: 0x001122DF File Offset: 0x001104DF
			public SearchFilter(string Name, Func<object, bool> condition, SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter = null, global::Tuple<Sprite, Color> icon = null)
			{
				this.Name = Name;
				this.condition = condition;
				this.parentFilter = parentFilter;
				this.icon = icon;
			}

			// Token: 0x04008524 RID: 34084
			public string Name;

			// Token: 0x04008525 RID: 34085
			public Func<object, bool> condition;

			// Token: 0x04008526 RID: 34086
			public SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter;

			// Token: 0x04008527 RID: 34087
			public global::Tuple<Sprite, Color> icon;
		}
	}

	// Token: 0x02001F51 RID: 8017
	public class SliderValue
	{
		// Token: 0x0600A91E RID: 43294 RVA: 0x00410528 File Offset: 0x0040E728
		public SliderValue(float slideMinValue, float slideMaxValue, string bottomSprite, string topSprite, string unitString, string tooltip, string labelText, Action<float> onValueChanged, int decimalPlaces = 0)
		{
			this.slideMinValue = slideMinValue;
			this.slideMaxValue = slideMaxValue;
			this.bottomSprite = bottomSprite;
			this.topSprite = topSprite;
			this.unitString = unitString;
			this.onValueChanged = onValueChanged;
			this.tooltip = tooltip;
			this.roundToDecimalPlaces = decimalPlaces;
			this.labelText = labelText;
			this.clampValueLow = slideMinValue;
			this.clampValueHigh = slideMaxValue;
		}

		// Token: 0x0600A91F RID: 43295 RVA: 0x00410590 File Offset: 0x0040E790
		public void SetRange(float min, float max, bool resetCurrentValue = true)
		{
			this.slideMinValue = min;
			this.slideMaxValue = max;
			this.slider.minValue = this.slideMinValue;
			this.slider.maxValue = this.slideMaxValue;
			this.inputField.currentValue = this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f;
			this.inputField.SetDisplayValue(this.inputField.currentValue.ToString());
			if (resetCurrentValue)
			{
				this.slider.value = this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f;
				this.onValueChanged(this.slideMinValue + (this.slideMaxValue - this.slideMinValue) / 2f);
			}
		}

		// Token: 0x0600A920 RID: 43296 RVA: 0x0041065C File Offset: 0x0040E85C
		public void SetValue(float value, bool runOnValueChanged = true)
		{
			value = Mathf.Clamp(value, this.clampValueLow, this.clampValueHigh);
			this.slider.value = value;
			this.inputField.currentValue = value;
			if (runOnValueChanged)
			{
				this.onValueChanged(value);
			}
			this.RefreshDisplay();
		}

		// Token: 0x0600A921 RID: 43297 RVA: 0x004106AC File Offset: 0x0040E8AC
		public void RefreshDisplay()
		{
			this.inputField.SetDisplayValue(((this.roundToDecimalPlaces == 0) ? ((float)Mathf.RoundToInt(this.inputField.currentValue)) : this.inputField.currentValue).ToString());
		}

		// Token: 0x04008528 RID: 34088
		public GameObject row;

		// Token: 0x04008529 RID: 34089
		public string bottomSprite;

		// Token: 0x0400852A RID: 34090
		public string topSprite;

		// Token: 0x0400852B RID: 34091
		public float slideMinValue;

		// Token: 0x0400852C RID: 34092
		public float slideMaxValue;

		// Token: 0x0400852D RID: 34093
		public float clampValueLow;

		// Token: 0x0400852E RID: 34094
		public float clampValueHigh;

		// Token: 0x0400852F RID: 34095
		public string unitString;

		// Token: 0x04008530 RID: 34096
		public Action<float> onValueChanged;

		// Token: 0x04008531 RID: 34097
		public string tooltip;

		// Token: 0x04008532 RID: 34098
		public int roundToDecimalPlaces;

		// Token: 0x04008533 RID: 34099
		public string labelText;

		// Token: 0x04008534 RID: 34100
		public KSlider slider;

		// Token: 0x04008535 RID: 34101
		public KNumberInputField inputField;
	}
}
