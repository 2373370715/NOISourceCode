using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class GeoTunerConfig : IBuildingConfig
{
	// Token: 0x06000DCE RID: 3534 RVA: 0x0017F1F8 File Offset: 0x0017D3F8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GeoTuner";
		int width = 4;
		int height = 3;
		string anim = "geoTuner_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.Overheatable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.UseStructureTemperature = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("GEYSER_ERUPTION_STATUS_PORT", new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTUNER.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.RequiresPowerInput = true;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.AllowGeyserTuning.Id;
		return buildingDef;
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0017F31C File Offset: 0x0017D51C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 0f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		manualDeliveryKG.capacity = 0f;
		manualDeliveryKG.refillMass = 0f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<GeoTunerWorkable>();
		go.AddOrGet<GeoTunerSwitchGeyserWorkable>();
		go.AddOrGet<CopyBuildingSettings>();
		GeoTuner.Def def = go.AddOrGetDef<GeoTuner.Def>();
		def.OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";
		def.geotunedGeyserSettings = GeoTunerConfig.geotunerGeyserSettings;
		def.defaultSetting = GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.DEFAULT_CATEGORY];
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0017F418 File Offset: 0x0017D618
	// Note: this type is marked as 'beforefieldinit'.
	static GeoTunerConfig()
	{
		Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings> dictionary = new Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings>();
		dictionary[GeoTunerConfig.Category.DEFAULT_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Dirt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.1f,
				temperatureModifier = 10f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.WATER_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.BleachStone.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 20f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.ORGANIC_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Salt.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.HYDROCARBON_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 15f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.VOLCANO_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Katairite.CreateTag(),
			quantity = 100f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 150f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.METALS_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.Phosphorus.CreateTag(),
			quantity = 80f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 50f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		dictionary[GeoTunerConfig.Category.CO2_CATEGORY] = new GeoTunerConfig.GeotunedGeyserSettings
		{
			material = SimHashes.ToxicSand.CreateTag(),
			quantity = 50f,
			duration = 600f,
			template = new Geyser.GeyserModification
			{
				massPerCycleModifier = 0.2f,
				temperatureModifier = 5f,
				iterationDurationModifier = 0f,
				iterationPercentageModifier = 0f,
				yearDurationModifier = 0f,
				yearPercentageModifier = 0f,
				maxPressureModifier = 0f
			}
		};
		GeoTunerConfig.CategorySettings = dictionary;
		GeoTunerConfig.geotunerGeyserSettings = new Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings>
		{
			{
				"steam",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"hot_steam",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slimy_po2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"hot_po2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"methane",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"chlorine_gas",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.ORGANIC_CATEGORY]
			},
			{
				"hot_co2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
			},
			{
				"hot_hydrogen",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"hot_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"salt_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slush_salt_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"filthy_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"slush_water",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.WATER_CATEGORY]
			},
			{
				"liquid_sulfur",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"liquid_co2",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.CO2_CATEGORY]
			},
			{
				"oil_drip",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.HYDROCARBON_CATEGORY]
			},
			{
				"small_volcano",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
			},
			{
				"big_volcano",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.VOLCANO_CATEGORY]
			},
			{
				"molten_copper",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_gold",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_iron",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_aluminum",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_cobalt",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_niobium",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			},
			{
				"molten_tungsten",
				GeoTunerConfig.CategorySettings[GeoTunerConfig.Category.METALS_CATEGORY]
			}
		};
	}

	// Token: 0x04000A0A RID: 2570
	public const int MAX_GEOTUNED = 5;

	// Token: 0x04000A0B RID: 2571
	public static Dictionary<GeoTunerConfig.Category, GeoTunerConfig.GeotunedGeyserSettings> CategorySettings;

	// Token: 0x04000A0C RID: 2572
	public static Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunerGeyserSettings;

	// Token: 0x04000A0D RID: 2573
	public const string ID = "GeoTuner";

	// Token: 0x04000A0E RID: 2574
	public const string OUTPUT_LOGIC_PORT_ID = "GEYSER_ERUPTION_STATUS_PORT";

	// Token: 0x04000A0F RID: 2575
	public const string GeyserAnimationModelTarget = "geyser_target";

	// Token: 0x04000A10 RID: 2576
	public const string GeyserAnimation_GeyserSymbols_LogicLightSymbol = "light_bloom";

	// Token: 0x02000367 RID: 871
	public struct GeotunedGeyserSettings
	{
		// Token: 0x06000DD3 RID: 3539 RVA: 0x000B06E7 File Offset: 0x000AE8E7
		public GeotunedGeyserSettings(Tag material, float quantity, float duration, Geyser.GeyserModification template)
		{
			this.quantity = quantity;
			this.material = material;
			this.template = template;
			this.duration = duration;
		}

		// Token: 0x04000A11 RID: 2577
		public Tag material;

		// Token: 0x04000A12 RID: 2578
		public float quantity;

		// Token: 0x04000A13 RID: 2579
		public Geyser.GeyserModification template;

		// Token: 0x04000A14 RID: 2580
		public float duration;
	}

	// Token: 0x02000368 RID: 872
	public enum Category
	{
		// Token: 0x04000A16 RID: 2582
		DEFAULT_CATEGORY,
		// Token: 0x04000A17 RID: 2583
		WATER_CATEGORY,
		// Token: 0x04000A18 RID: 2584
		ORGANIC_CATEGORY,
		// Token: 0x04000A19 RID: 2585
		HYDROCARBON_CATEGORY,
		// Token: 0x04000A1A RID: 2586
		VOLCANO_CATEGORY,
		// Token: 0x04000A1B RID: 2587
		METALS_CATEGORY,
		// Token: 0x04000A1C RID: 2588
		CO2_CATEGORY
	}
}
