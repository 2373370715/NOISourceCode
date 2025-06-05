using System;
using System.Collections.Generic;
using System.Text;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CFA RID: 15610
	public class PlantMutation : Modifier
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600EFB6 RID: 61366 RVA: 0x0014553B File Offset: 0x0014373B
		public List<string> AdditionalSoundEvents
		{
			get
			{
				return this.additionalSoundEvents;
			}
		}

		// Token: 0x0600EFB7 RID: 61367 RVA: 0x004EAAF4 File Offset: 0x004E8CF4
		public PlantMutation(string id, string name, string desc) : base(id, name, desc)
		{
		}

		// Token: 0x0600EFB8 RID: 61368 RVA: 0x00145543 File Offset: 0x00143743
		public void ApplyTo(MutantPlant target)
		{
			this.ApplyFunctionalTo(target);
			if (!target.HasTag(GameTags.Seed) && !target.HasTag(GameTags.CropSeed) && !target.HasTag(GameTags.Compostable))
			{
				this.ApplyVisualTo(target);
			}
		}

		// Token: 0x0600EFB9 RID: 61369 RVA: 0x004EAB78 File Offset: 0x004E8D78
		private void ApplyFunctionalTo(MutantPlant target)
		{
			SeedProducer component = target.GetComponent<SeedProducer>();
			if (component != null && component.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
			{
				component.Configure(component.seedInfo.seedId, SeedProducer.ProductionType.Sterile, 1);
			}
			if (this.bonusCropID.IsValid)
			{
				target.Subscribe(-1072826864, new Action<object>(this.OnHarvestBonusCrop));
			}
			if (!this.forcePrefersDarkness)
			{
				if (this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null)
				{
					goto IL_F0;
				}
			}
			IlluminationVulnerable illuminationVulnerable = target.GetComponent<IlluminationVulnerable>();
			if (illuminationVulnerable == null)
			{
				illuminationVulnerable = target.gameObject.AddComponent<IlluminationVulnerable>();
			}
			if (this.forcePrefersDarkness)
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(true);
				}
			}
			else
			{
				if (illuminationVulnerable != null)
				{
					illuminationVulnerable.SetPrefersDarkness(false);
				}
				target.GetComponent<Modifiers>().attributes.Add(Db.Get().PlantAttributes.MinLightLux);
			}
			IL_F0:
			byte b = this.droppedDiseaseID;
			if (this.harvestDiseaseID != 255)
			{
				target.Subscribe(35625290, new Action<object>(this.OnCropSpawnedAddDisease));
			}
			bool isValid = this.ensureIrrigationInfo.tag.IsValid;
			Attributes attributes = target.GetAttributes();
			this.AddTo(attributes);
		}

		// Token: 0x0600EFBA RID: 61370 RVA: 0x004EACC8 File Offset: 0x004E8EC8
		private void ApplyVisualTo(MutantPlant target)
		{
			KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
			if (this.symbolOverrideInfo != null && this.symbolOverrideInfo.Count > 0)
			{
				SymbolOverrideController component2 = target.GetComponent<SymbolOverrideController>();
				if (component2 != null)
				{
					foreach (PlantMutation.SymbolOverrideInfo symbolOverrideInfo in this.symbolOverrideInfo)
					{
						KAnim.Build.Symbol symbol = Assets.GetAnim(symbolOverrideInfo.sourceAnim).GetData().build.GetSymbol(symbolOverrideInfo.sourceSymbol);
						component2.AddSymbolOverride(symbolOverrideInfo.targetSymbolName, symbol, 0);
					}
				}
			}
			if (this.bGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.bGFXAnim, "_BGFX", 0.1f);
			}
			if (this.fGFXAnim != null)
			{
				PlantMutation.CreateFXObject(target, this.fGFXAnim, "_FGFX", -0.1f);
			}
			if (this.plantTint != Color.white)
			{
				component.TintColour = this.plantTint;
			}
			if (this.symbolTints.Count > 0)
			{
				for (int i = 0; i < this.symbolTints.Count; i++)
				{
					component.SetSymbolTint(this.symbolTintTargets[i], this.symbolTints[i]);
				}
			}
			if (this.symbolScales.Count > 0)
			{
				for (int j = 0; j < this.symbolScales.Count; j++)
				{
					component.SetSymbolScale(this.symbolScaleTargets[j], this.symbolScales[j]);
				}
			}
			if (this.additionalSoundEvents.Count > 0)
			{
				for (int k = 0; k < this.additionalSoundEvents.Count; k++)
				{
				}
			}
		}

		// Token: 0x0600EFBB RID: 61371 RVA: 0x004EAEAC File Offset: 0x004E90AC
		private static void CreateFXObject(MutantPlant target, string anim, string nameSuffix, float offset)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(SimpleFXConfig.ID));
			gameObject.name = target.name + nameSuffix;
			gameObject.transform.parent = target.transform;
			gameObject.AddComponent<LoopingSounds>();
			gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(gameObject.name);
			Extents extents = target.GetComponent<OccupyArea>().GetExtents();
			Vector3 position = target.transform.GetPosition();
			position.x = (float)extents.x + (float)extents.width / 2f;
			position.y = (float)extents.y + (float)extents.height / 2f;
			position.z += offset;
			gameObject.transform.SetPosition(position);
			KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
			component.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim(anim)
			};
			component.initialAnim = "idle";
			component.initialMode = KAnim.PlayMode.Loop;
			component.randomiseLoopedOffset = true;
			component.fgLayer = Grid.SceneLayer.NoLayer;
			if (target.HasTag(GameTags.Hanging))
			{
				component.Rotation = 180f;
			}
			gameObject.SetActive(true);
		}

		// Token: 0x0600EFBC RID: 61372 RVA: 0x0014557A File Offset: 0x0014377A
		private void OnHarvestBonusCrop(object data)
		{
			((Crop)data).SpawnSomeFruit(this.bonusCropID, this.bonusCropAmount);
		}

		// Token: 0x0600EFBD RID: 61373 RVA: 0x00145593 File Offset: 0x00143793
		private void OnCropSpawnedAddDisease(object data)
		{
			((GameObject)data).GetComponent<PrimaryElement>().AddDisease(this.harvestDiseaseID, this.harvestDiseaseAmount, this.Name);
		}

		// Token: 0x0600EFBE RID: 61374 RVA: 0x004EAFDC File Offset: 0x004E91DC
		public string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.desc);
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.Get(attributeModifier.AttributeId);
				}
				if (attribute.ShowInUI != Attribute.Display.Never)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString()));
				}
			}
			if (this.bonusCropID != null)
			{
				string newValue;
				if (GameTags.DisplayAsCalories.Contains(this.bonusCropID))
				{
					EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.bonusCropID.Name);
					DebugUtil.Assert(foodInfo != null, "Eeh? Trying to spawn a bonus crop that is caloric but isn't a food??", this.bonusCropID.ToString());
					newValue = GameUtil.GetFormattedCalories(this.bonusCropAmount * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true);
				}
				else if (GameTags.DisplayAsUnits.Contains(this.bonusCropID))
				{
					newValue = GameUtil.GetFormattedUnits(this.bonusCropAmount, GameUtil.TimeSlice.None, false, "");
				}
				else
				{
					newValue = GameUtil.GetFormattedMass(this.bonusCropAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
				}
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(CREATURES.PLANT_MUTATIONS.BONUS_CROP_FMT.Replace("{Crop}", this.bonusCropID.ProperName()).Replace("{Amount}", newValue));
			}
			if (this.droppedDiseaseID != 255)
			{
				if (this.droppedDiseaseOnGrowAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseOnGrowAmount, GameUtil.TimeSlice.None)));
				}
				if (this.droppedDiseaseContinuousAmount > 0)
				{
					stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
					stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.droppedDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.droppedDiseaseContinuousAmount, GameUtil.TimeSlice.PerSecond)));
				}
			}
			if (this.harvestDiseaseID != 255)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)));
			}
			if (this.forcePrefersDarkness)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS);
			}
			if (this.forceSelfHarvestOnGrown)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST);
			}
			if (this.ensureIrrigationInfo.tag.IsValid)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, this.ensureIrrigationInfo.tag.ProperName(), GameUtil.GetFormattedMass(-this.ensureIrrigationInfo.massConsumptionRate, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true));
			}
			if (!this.originalMutation)
			{
				stringBuilder.Append(DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY);
				stringBuilder.Append(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600EFBF RID: 61375 RVA: 0x004EB38C File Offset: 0x004E958C
		public void GetDescriptors(ref List<Descriptor> descriptors, GameObject go)
		{
			if (this.harvestDiseaseID != 255)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_ON_HARVEST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.harvestDiseaseID, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.harvestDiseaseAmount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.forceSelfHarvestOnGrown)
			{
				descriptors.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.AUTO_SELF_HARVEST, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.AUTO_SELF_HARVEST, Descriptor.DescriptorType.Effect, false));
			}
		}

		// Token: 0x0600EFC0 RID: 61376 RVA: 0x001455B7 File Offset: 0x001437B7
		public PlantMutation Original()
		{
			this.originalMutation = true;
			return this;
		}

		// Token: 0x0600EFC1 RID: 61377 RVA: 0x001455C1 File Offset: 0x001437C1
		public PlantMutation RequiredPrefabID(string requiredID)
		{
			this.requiredPrefabIDs.Add(requiredID);
			return this;
		}

		// Token: 0x0600EFC2 RID: 61378 RVA: 0x001455D0 File Offset: 0x001437D0
		public PlantMutation RestrictPrefabID(string restrictedID)
		{
			this.restrictedPrefabIDs.Add(restrictedID);
			return this;
		}

		// Token: 0x0600EFC3 RID: 61379 RVA: 0x004EB440 File Offset: 0x004E9640
		public PlantMutation AttributeModifier(Attribute attribute, float amount, bool multiplier = false)
		{
			DebugUtil.Assert(!this.forcePrefersDarkness || attribute != Db.Get().PlantAttributes.MinLightLux, "A plant mutation has both darkness and light defined!", this.Id);
			base.Add(new AttributeModifier(attribute.Id, amount, this.Name, multiplier, false, true));
			return this;
		}

		// Token: 0x0600EFC4 RID: 61380 RVA: 0x001455DF File Offset: 0x001437DF
		public PlantMutation BonusCrop(Tag cropPrefabID, float bonucCropAmount)
		{
			this.bonusCropID = cropPrefabID;
			this.bonusCropAmount = bonucCropAmount;
			return this;
		}

		// Token: 0x0600EFC5 RID: 61381 RVA: 0x001455F0 File Offset: 0x001437F0
		public PlantMutation DiseaseDropper(byte diseaseID, int onGrowAmount, int continuousAmount)
		{
			this.droppedDiseaseID = diseaseID;
			this.droppedDiseaseOnGrowAmount = onGrowAmount;
			this.droppedDiseaseContinuousAmount = continuousAmount;
			return this;
		}

		// Token: 0x0600EFC6 RID: 61382 RVA: 0x00145608 File Offset: 0x00143808
		public PlantMutation AddDiseaseToHarvest(byte diseaseID, int amount)
		{
			this.harvestDiseaseID = diseaseID;
			this.harvestDiseaseAmount = amount;
			return this;
		}

		// Token: 0x0600EFC7 RID: 61383 RVA: 0x004EB49C File Offset: 0x004E969C
		public PlantMutation ForcePrefersDarkness()
		{
			DebugUtil.Assert(this.SelfModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().PlantAttributes.MinLightLux.Id) == null, "A plant mutation has both darkness and light defined!", this.Id);
			this.forcePrefersDarkness = true;
			return this;
		}

		// Token: 0x0600EFC8 RID: 61384 RVA: 0x00145619 File Offset: 0x00143819
		public PlantMutation ForceSelfHarvestOnGrown()
		{
			this.forceSelfHarvestOnGrown = true;
			this.AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute, -0.999999f, true);
			return this;
		}

		// Token: 0x0600EFC9 RID: 61385 RVA: 0x00145644 File Offset: 0x00143844
		public PlantMutation EnsureIrrigated(PlantElementAbsorber.ConsumeInfo consumeInfo)
		{
			this.ensureIrrigationInfo = consumeInfo;
			return this;
		}

		// Token: 0x0600EFCA RID: 61386 RVA: 0x004EB4F0 File Offset: 0x004E96F0
		public PlantMutation VisualTint(float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			if (r < 0f)
			{
				this.plantTint = Color.white + new Color(r, g, b, 0f);
			}
			else
			{
				this.plantTint = new Color(r, g, b, 0f);
			}
			return this;
		}

		// Token: 0x0600EFCB RID: 61387 RVA: 0x004EB564 File Offset: 0x004E9764
		public PlantMutation VisualSymbolTint(string targetSymbolName, float r, float g, float b)
		{
			global::Debug.Assert(Mathf.Sign(r) == Mathf.Sign(g) && Mathf.Sign(r) == Mathf.Sign(b), "Vales for tints must be all positive or all negative for the shader to work correctly!");
			this.symbolTintTargets.Add(targetSymbolName);
			this.symbolTints.Add(Color.white + new Color(r, g, b, 0f));
			return this;
		}

		// Token: 0x0600EFCC RID: 61388 RVA: 0x0014564E File Offset: 0x0014384E
		public PlantMutation VisualSymbolOverride(string targetSymbolName, string sourceAnim, string sourceSymbol)
		{
			if (this.symbolOverrideInfo == null)
			{
				this.symbolOverrideInfo = new List<PlantMutation.SymbolOverrideInfo>();
			}
			this.symbolOverrideInfo.Add(new PlantMutation.SymbolOverrideInfo
			{
				targetSymbolName = targetSymbolName,
				sourceAnim = sourceAnim,
				sourceSymbol = sourceSymbol
			});
			return this;
		}

		// Token: 0x0600EFCD RID: 61389 RVA: 0x00145689 File Offset: 0x00143889
		public PlantMutation VisualSymbolScale(string targetSymbolName, float scale)
		{
			this.symbolScaleTargets.Add(targetSymbolName);
			this.symbolScales.Add(scale);
			return this;
		}

		// Token: 0x0600EFCE RID: 61390 RVA: 0x001456A4 File Offset: 0x001438A4
		public PlantMutation VisualBGFX(string animName)
		{
			this.bGFXAnim = animName;
			return this;
		}

		// Token: 0x0600EFCF RID: 61391 RVA: 0x001456AE File Offset: 0x001438AE
		public PlantMutation VisualFGFX(string animName)
		{
			this.fGFXAnim = animName;
			return this;
		}

		// Token: 0x0600EFD0 RID: 61392 RVA: 0x001456B8 File Offset: 0x001438B8
		public PlantMutation AddSoundEvent(string soundEventName)
		{
			this.additionalSoundEvents.Add(soundEventName);
			return this;
		}

		// Token: 0x0400EB43 RID: 60227
		public string desc;

		// Token: 0x0400EB44 RID: 60228
		public string animationSoundEvent;

		// Token: 0x0400EB45 RID: 60229
		public bool originalMutation;

		// Token: 0x0400EB46 RID: 60230
		public List<string> requiredPrefabIDs = new List<string>();

		// Token: 0x0400EB47 RID: 60231
		public List<string> restrictedPrefabIDs = new List<string>();

		// Token: 0x0400EB48 RID: 60232
		private Tag bonusCropID;

		// Token: 0x0400EB49 RID: 60233
		private float bonusCropAmount;

		// Token: 0x0400EB4A RID: 60234
		private byte droppedDiseaseID = byte.MaxValue;

		// Token: 0x0400EB4B RID: 60235
		private int droppedDiseaseOnGrowAmount;

		// Token: 0x0400EB4C RID: 60236
		private int droppedDiseaseContinuousAmount;

		// Token: 0x0400EB4D RID: 60237
		private byte harvestDiseaseID = byte.MaxValue;

		// Token: 0x0400EB4E RID: 60238
		private int harvestDiseaseAmount;

		// Token: 0x0400EB4F RID: 60239
		private bool forcePrefersDarkness;

		// Token: 0x0400EB50 RID: 60240
		private bool forceSelfHarvestOnGrown;

		// Token: 0x0400EB51 RID: 60241
		private PlantElementAbsorber.ConsumeInfo ensureIrrigationInfo;

		// Token: 0x0400EB52 RID: 60242
		private Color plantTint = Color.white;

		// Token: 0x0400EB53 RID: 60243
		private List<string> symbolTintTargets = new List<string>();

		// Token: 0x0400EB54 RID: 60244
		private List<Color> symbolTints = new List<Color>();

		// Token: 0x0400EB55 RID: 60245
		private List<PlantMutation.SymbolOverrideInfo> symbolOverrideInfo;

		// Token: 0x0400EB56 RID: 60246
		private List<string> symbolScaleTargets = new List<string>();

		// Token: 0x0400EB57 RID: 60247
		private List<float> symbolScales = new List<float>();

		// Token: 0x0400EB58 RID: 60248
		private string bGFXAnim;

		// Token: 0x0400EB59 RID: 60249
		private string fGFXAnim;

		// Token: 0x0400EB5A RID: 60250
		private List<string> additionalSoundEvents = new List<string>();

		// Token: 0x02003CFB RID: 15611
		private class SymbolOverrideInfo
		{
			// Token: 0x0400EB5B RID: 60251
			public string targetSymbolName;

			// Token: 0x0400EB5C RID: 60252
			public string sourceAnim;

			// Token: 0x0400EB5D RID: 60253
			public string sourceSymbol;
		}
	}
}
