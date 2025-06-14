﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class SupermaterialRefineryConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SupermaterialRefinery";
		int width = 4;
		int height = 5;
		string anim = "supermaterial_refinery_kanim";
		int hitpoints = 30;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1600f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.heatedTemperature = SupermaterialRefineryConfig.OUTPUT_TEMPERATURE;
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		Prioritizable.AddRef(go);
		if (DlcManager.IsExpansion1Active())
		{
			float num = 0.9f;
			float num2 = 1f - num;
			ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Graphite.CreateTag(), 100f * num),
				new ComplexRecipe.RecipeElement(SimHashes.Sulfur.CreateTag(), 100f * num2 / 2f),
				new ComplexRecipe.RecipeElement(SimHashes.Aluminum.CreateTag(), 100f * num2 / 2f)
			};
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array, array2), array, array2);
			complexRecipe.time = 80f;
			complexRecipe.description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.FULLERENE_RECIPE_DESCRIPTION;
			complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			complexRecipe.fabricators = new List<Tag>
			{
				TagManager.Create("SupermaterialRefinery")
			};
		}
		float num3 = 0.15f;
		float num4 = 0.7f;
		float num5 = 0.15f;
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f * num3),
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 100f * num4),
			new ComplexRecipe.RecipeElement(SimHashes.MilkFat.CreateTag(), 100f * num5)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.HardPolypropylene.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array3, array4), array3, array4);
		complexRecipe2.time = 80f;
		complexRecipe2.description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.HARDPLASTIC_RECIPE_DESCRIPTION;
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe2.fabricators = new List<Tag>
		{
			TagManager.Create("SupermaterialRefinery")
		};
		float num6 = 0.15f;
		float num7 = 0.05f;
		float num8 = 1f - num7 - num6;
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num6),
			new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num8),
			new ComplexRecipe.RecipeElement(new Tag[]
			{
				BasicFabricConfig.ID.ToTag(),
				FeatherFabricConfig.ID.ToTag()
			}, 100f * num7)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array5, array6), array5, array6);
		complexRecipe3.time = 80f;
		complexRecipe3.description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION;
		complexRecipe3.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe3.fabricators = new List<Tag>
		{
			TagManager.Create("SupermaterialRefinery")
		};
		float num9 = 0.05f;
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 100f * num9),
			new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), 100f * (1f - num9))
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated, false)
		};
		ComplexRecipe complexRecipe4 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array7, array8), array7, array8);
		complexRecipe4.time = 80f;
		complexRecipe4.description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION;
		complexRecipe4.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe4.fabricators = new List<Tag>
		{
			TagManager.Create("SupermaterialRefinery")
		};
		if (DlcManager.IsAllContentSubscribed(new string[]
		{
			"DLC3_ID",
			"EXPANSION1_ID"
		}))
		{
			ComplexRecipe.RecipeElement[] array9 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SimHashes.EnrichedUranium.CreateTag(), 10f)
			};
			ComplexRecipe.RecipeElement[] array10 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("SelfChargingElectrobank", 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			ComplexRecipe complexRecipe5 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", array9, array10), array9, array10, DlcManager.EXPANSION1.Append(DlcManager.DLC3));
			complexRecipe5.time = 80f;
			complexRecipe5.description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SELF_CHARGING_POWERBANK_RECIPE_DESCRIPTION;
			complexRecipe5.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			complexRecipe5.fabricators = new List<Tag>
			{
				TagManager.Create("SupermaterialRefinery")
			};
			complexRecipe5.requiredTech = Db.Get().TechItems.selfChargingElectrobank.parentTechId;
		}
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			KAnimFile anim = Assets.GetAnim("anim_interacts_supermaterial_refinery_kanim");
			KAnimFile[] overrideAnims = new KAnimFile[]
			{
				anim
			};
			component.overrideAnims = overrideAnims;
			component.workAnims = new HashedString[]
			{
				"working_pre",
				"working_loop"
			};
			component.synchronizeAnims = false;
			KAnimFileData data = anim.GetData();
			int animCount = data.animCount;
			this.dupeInteractAnims = new HashedString[animCount - 2];
			int i = 0;
			int num = 0;
			while (i < animCount)
			{
				HashedString hashedString = data.GetAnim(i).name;
				if (hashedString != "working_pre" && hashedString != "working_pst")
				{
					this.dupeInteractAnims[num] = hashedString;
					num++;
				}
				i++;
			}
			component.GetDupeInteract = (() => new HashedString[]
			{
				"working_loop",
				this.dupeInteractAnims.GetRandom<HashedString>()
			});
		};
	}

	public const string ID = "SupermaterialRefinery";

	private const float INPUT_KG = 100f;

	private const float OUTPUT_KG = 100f;

	public static float OUTPUT_TEMPERATURE = 313.15f;

	private HashedString[] dupeInteractAnims;
}
