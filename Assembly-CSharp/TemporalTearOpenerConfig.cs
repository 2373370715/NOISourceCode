using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class TemporalTearOpenerConfig : IBuildingConfig
{
	// Token: 0x06001A67 RID: 6759 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x001B3B20 File Offset: 0x001B1D20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TemporalTearOpener";
		int width = 3;
		int height = 4;
		string anim = "temporal_tear_opener_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.DefaultAnimState = "off";
		buildingDef.Entombable = false;
		buildingDef.Invincible = true;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 2);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x001B3BE4 File Offset: 0x001B1DE4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.capacity = 1000f;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
		TemporalTearOpener.Def def = go.AddOrGetDef<TemporalTearOpener.Def>();
		def.numParticlesToOpen = 10000f;
		def.consumeRate = 5f;
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000B59B0 File Offset: 0x000B3BB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
	}

	// Token: 0x04001114 RID: 4372
	public const string ID = "TemporalTearOpener";

	// Token: 0x04001115 RID: 4373
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04001116 RID: 4374
	public const float PARTICLES_CAPACITY = 1000f;

	// Token: 0x04001117 RID: 4375
	public const float NUM_PARTICLES_TO_OPEN_TEAR = 10000f;

	// Token: 0x04001118 RID: 4376
	public const float PARTICLE_CONSUME_RATE = 5f;
}
