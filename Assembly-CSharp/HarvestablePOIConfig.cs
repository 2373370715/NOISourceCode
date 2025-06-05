using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class HarvestablePOIConfig : IMultiEntityConfig
{
	// Token: 0x060013A0 RID: 5024 RVA: 0x00198E0C File Offset: 0x0019700C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (HarvestablePOIConfig.HarvestablePOIParams harvestablePOIParams in this.GenerateConfigs())
		{
			list.Add(HarvestablePOIConfig.CreateHarvestablePOI(harvestablePOIParams.id, harvestablePOIParams.anim, Strings.Get(harvestablePOIParams.nameStringKey), harvestablePOIParams.descStringKey, harvestablePOIParams.poiType.idHash, harvestablePOIParams.poiType.canProvideArtifacts, harvestablePOIParams.poiType.GetRequiredDlcIds(), harvestablePOIParams.poiType.GetForbiddenDlcIds()));
		}
		return list;
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000B2FA5 File Offset: 0x000B11A5
	public static GameObject CreateHarvestablePOI(string id, string anim, string name, StringKey descStringKey, HashedString poiType, bool canProvideArtifacts = false)
	{
		return HarvestablePOIConfig.CreateHarvestablePOI(id, anim, name, descStringKey, poiType, canProvideArtifacts, DlcManager.EXPANSION1, null);
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x00198EB8 File Offset: 0x001970B8
	public static GameObject CreateHarvestablePOI(string id, string anim, string name, StringKey descStringKey, HashedString poiType, bool canProvideArtifacts = false, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<HarvestablePOIConfigurator>().presetType = poiType;
		HarvestablePOIClusterGridEntity harvestablePOIClusterGridEntity = gameObject.AddOrGet<HarvestablePOIClusterGridEntity>();
		harvestablePOIClusterGridEntity.m_name = name;
		harvestablePOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<HarvestablePOIStates.Def>();
		if (canProvideArtifacts)
		{
			gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
			gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = ArtifactPOIConfigurator.defaultArtifactPoiType.idHash;
		}
		gameObject.AddOrGet<InfoDescription>().description = Strings.Get(descStringKey);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIds;
		component.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x00198F48 File Offset: 0x00197148
	private List<HarvestablePOIConfig.HarvestablePOIParams> GenerateConfigs()
	{
		List<HarvestablePOIConfig.HarvestablePOIParams> list = new List<HarvestablePOIConfig.HarvestablePOIParams>();
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("cloud", new HarvestablePOIConfigurator.HarvestablePOIType("CarbonAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.RefinedCarbon,
				1.5f
			},
			{
				SimHashes.Carbon,
				5.5f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("metallic_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("MetallicAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.MoltenIron,
				1.25f
			},
			{
				SimHashes.Cuprite,
				1.75f
			},
			{
				SimHashes.Obsidian,
				7f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("satellite_field", new HarvestablePOIConfigurator.HarvestablePOIType("SatelliteField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Sand,
				3f
			},
			{
				SimHashes.IronOre,
				3f
			},
			{
				SimHashes.MoltenCopper,
				2.67f
			},
			{
				SimHashes.Glass,
				1.33f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("rocky_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("RockyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cuprite,
				2f
			},
			{
				SimHashes.SedimentaryRock,
				4f
			},
			{
				SimHashes.IgneousRock,
				4f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("interstellar_ice_field", new HarvestablePOIConfigurator.HarvestablePOIType("InterstellarIceField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				2.5f
			},
			{
				SimHashes.SolidCarbonDioxide,
				7f
			},
			{
				SimHashes.SolidOxygen,
				0.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, new List<string>
		{
			Db.Get().OrbitalTypeCategories.iceCloud.Id,
			Db.Get().OrbitalTypeCategories.iceRock.Id
		}, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("organic_mass_field", new HarvestablePOIConfigurator.HarvestablePOIType("OrganicMassField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SlimeMold,
				3f
			},
			{
				SimHashes.Algae,
				3f
			},
			{
				SimHashes.ContaminatedOxygen,
				1f
			},
			{
				SimHashes.Dirt,
				3f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ice_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("IceAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				6f
			},
			{
				SimHashes.SolidCarbonDioxide,
				2f
			},
			{
				SimHashes.Oxygen,
				1.5f
			},
			{
				SimHashes.SolidMethane,
				0.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, new List<string>
		{
			Db.Get().OrbitalTypeCategories.iceCloud.Id,
			Db.Get().OrbitalTypeCategories.iceRock.Id
		}, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("gas_giant_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("GasGiantCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Methane,
				1f
			},
			{
				SimHashes.LiquidMethane,
				1f
			},
			{
				SimHashes.SolidMethane,
				1f
			},
			{
				SimHashes.Hydrogen,
				7f
			}
		}, 15000f, 20000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("chlorine_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("ChlorineCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Chlorine,
				2.5f
			},
			{
				SimHashes.BleachStone,
				7.5f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("gilded_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("GildedAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Gold,
				2.5f
			},
			{
				SimHashes.Fullerene,
				1f
			},
			{
				SimHashes.RefinedCarbon,
				1f
			},
			{
				SimHashes.SedimentaryRock,
				4.5f
			},
			{
				SimHashes.Regolith,
				1f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("glimmering_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("GlimmeringAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.MoltenTungsten,
				2f
			},
			{
				SimHashes.Wolframite,
				6f
			},
			{
				SimHashes.Carbon,
				1f
			},
			{
				SimHashes.CarbonDioxide,
				1f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("helium_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("HeliumCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Hydrogen,
				2f
			},
			{
				SimHashes.Water,
				8f
			}
		}, 30000f, 45000f, 30000f, 60000f, true, HarvestablePOIConfig.GasFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oily_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OilyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SolidCarbonDioxide,
				7.75f
			},
			{
				SimHashes.SolidMethane,
				1.125f
			},
			{
				SimHashes.CrudeOil,
				1.125f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oxidized_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OxidizedAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Rust,
				8f
			},
			{
				SimHashes.SolidCarbonDioxide,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("salty_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("SaltyAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SaltWater,
				5f
			},
			{
				SimHashes.Brine,
				4f
			},
			{
				SimHashes.SolidCarbonDioxide,
				1f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("frozen_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("FrozenOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Ice,
				2.33f
			},
			{
				SimHashes.DirtyIce,
				2.33f
			},
			{
				SimHashes.Snow,
				1.83f
			},
			{
				SimHashes.AluminumOre,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("foresty_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("ForestyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.IgneousRock,
				7f
			},
			{
				SimHashes.AluminumOre,
				1f
			},
			{
				SimHashes.CarbonDioxide,
				2f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("swampy_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("SwampyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Mud,
				2f
			},
			{
				SimHashes.ToxicSand,
				7f
			},
			{
				SimHashes.Cobaltite,
				1f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("sandy_ore_field", new HarvestablePOIConfigurator.HarvestablePOIType("SandyOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SandStone,
				4f
			},
			{
				SimHashes.Algae,
				2f
			},
			{
				SimHashes.Cuprite,
				1f
			},
			{
				SimHashes.Sand,
				3f
			}
		}, 54000f, 81000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("radioactive_gas_cloud", new HarvestablePOIConfigurator.HarvestablePOIType("RadioactiveGasCloud", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.UraniumOre,
				2f
			},
			{
				SimHashes.Chlorine,
				2f
			},
			{
				SimHashes.CarbonDioxide,
				7f
			}
		}, 5000f, 10000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("radioactive_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("RadioactiveAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.UraniumOre,
				2f
			},
			{
				SimHashes.Sulfur,
				3f
			},
			{
				SimHashes.BleachStone,
				2f
			},
			{
				SimHashes.Rust,
				4f
			}
		}, 5000f, 10000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("oxygen_rich_asteroid_field", new HarvestablePOIConfigurator.HarvestablePOIType("OxygenRichAsteroidField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Water,
				4f
			},
			{
				SimHashes.ContaminatedOxygen,
				2f
			},
			{
				SimHashes.Ice,
				4f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("interstellar_ocean", new HarvestablePOIConfigurator.HarvestablePOIType("InterstellarOcean", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.SaltWater,
				2.5f
			},
			{
				SimHashes.Brine,
				2.5f
			},
			{
				SimHashes.Salt,
				2.5f
			},
			{
				SimHashes.Ice,
				2.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1, null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ceres_debris_field", new HarvestablePOIConfigurator.HarvestablePOIType("DLC2CeresField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cinnabar,
				4.5f
			},
			{
				SimHashes.Mercury,
				2.5f
			},
			{
				SimHashes.Ice,
				2.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1.Append(DlcManager.DLC2), null)));
		list.Add(new HarvestablePOIConfig.HarvestablePOIParams("ceres_starting_field", new HarvestablePOIConfigurator.HarvestablePOIType("DLC2CeresOreField", new Dictionary<SimHashes, float>
		{
			{
				SimHashes.Cinnabar,
				2.5f
			},
			{
				SimHashes.Mercury,
				2.5f
			},
			{
				SimHashes.Ice,
				3.5f
			}
		}, 15000f, 25000f, 30000f, 60000f, true, HarvestablePOIConfig.AsteroidFieldOrbit, 20, DlcManager.EXPANSION1.Append(DlcManager.DLC2), null)));
		list.RemoveAll((HarvestablePOIConfig.HarvestablePOIParams poi) => !DlcManager.IsCorrectDlcSubscribed(poi.poiType));
		return list;
	}

	// Token: 0x04000D6E RID: 3438
	public const string CarbonAsteroidField = "CarbonAsteroidField";

	// Token: 0x04000D6F RID: 3439
	public const string MetallicAsteroidField = "MetallicAsteroidField";

	// Token: 0x04000D70 RID: 3440
	public const string SatelliteField = "SatelliteField";

	// Token: 0x04000D71 RID: 3441
	public const string RockyAsteroidField = "RockyAsteroidField";

	// Token: 0x04000D72 RID: 3442
	public const string InterstellarIceField = "InterstellarIceField";

	// Token: 0x04000D73 RID: 3443
	public const string OrganicMassField = "OrganicMassField";

	// Token: 0x04000D74 RID: 3444
	public const string IceAsteroidField = "IceAsteroidField";

	// Token: 0x04000D75 RID: 3445
	public const string GasGiantCloud = "GasGiantCloud";

	// Token: 0x04000D76 RID: 3446
	public const string ChlorineCloud = "ChlorineCloud";

	// Token: 0x04000D77 RID: 3447
	public const string GildedAsteroidField = "GildedAsteroidField";

	// Token: 0x04000D78 RID: 3448
	public const string GlimmeringAsteroidField = "GlimmeringAsteroidField";

	// Token: 0x04000D79 RID: 3449
	public const string HeliumCloud = "HeliumCloud";

	// Token: 0x04000D7A RID: 3450
	public const string OilyAsteroidField = "OilyAsteroidField";

	// Token: 0x04000D7B RID: 3451
	public const string OxidizedAsteroidField = "OxidizedAsteroidField";

	// Token: 0x04000D7C RID: 3452
	public const string SaltyAsteroidField = "SaltyAsteroidField";

	// Token: 0x04000D7D RID: 3453
	public const string FrozenOreField = "FrozenOreField";

	// Token: 0x04000D7E RID: 3454
	public const string ForestyOreField = "ForestyOreField";

	// Token: 0x04000D7F RID: 3455
	public const string SwampyOreField = "SwampyOreField";

	// Token: 0x04000D80 RID: 3456
	public const string SandyOreField = "SandyOreField";

	// Token: 0x04000D81 RID: 3457
	public const string RadioactiveGasCloud = "RadioactiveGasCloud";

	// Token: 0x04000D82 RID: 3458
	public const string RadioactiveAsteroidField = "RadioactiveAsteroidField";

	// Token: 0x04000D83 RID: 3459
	public const string OxygenRichAsteroidField = "OxygenRichAsteroidField";

	// Token: 0x04000D84 RID: 3460
	public const string InterstellarOcean = "InterstellarOcean";

	// Token: 0x04000D85 RID: 3461
	public const string DLC2CeresField = "DLC2CeresField";

	// Token: 0x04000D86 RID: 3462
	public const string DLC2CeresOreField = "DLC2CeresOreField";

	// Token: 0x04000D87 RID: 3463
	private static readonly List<string> GasFieldOrbit = new List<string>
	{
		Db.Get().OrbitalTypeCategories.iceCloud.Id,
		Db.Get().OrbitalTypeCategories.heliumCloud.Id,
		Db.Get().OrbitalTypeCategories.purpleGas.Id,
		Db.Get().OrbitalTypeCategories.radioactiveGas.Id
	};

	// Token: 0x04000D88 RID: 3464
	private static readonly List<string> AsteroidFieldOrbit = new List<string>
	{
		Db.Get().OrbitalTypeCategories.iceRock.Id,
		Db.Get().OrbitalTypeCategories.frozenOre.Id,
		Db.Get().OrbitalTypeCategories.rocky.Id
	};

	// Token: 0x02000482 RID: 1154
	public struct HarvestablePOIParams
	{
		// Token: 0x060013A8 RID: 5032 RVA: 0x00199C50 File Offset: 0x00197E50
		public HarvestablePOIParams(string anim, HarvestablePOIConfigurator.HarvestablePOIType poiType)
		{
			this.id = "HarvestableSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x04000D89 RID: 3465
		public string id;

		// Token: 0x04000D8A RID: 3466
		public string anim;

		// Token: 0x04000D8B RID: 3467
		public StringKey nameStringKey;

		// Token: 0x04000D8C RID: 3468
		public StringKey descStringKey;

		// Token: 0x04000D8D RID: 3469
		public HarvestablePOIConfigurator.HarvestablePOIType poiType;
	}
}
