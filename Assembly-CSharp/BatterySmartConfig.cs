using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class BatterySmartConfig : BaseBatteryConfig
{
	// Token: 0x060000CA RID: 202 RVA: 0x00149BC4 File Offset: 0x00147DC4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatterySmart";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "smartbattery_kanim";
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0f;
		float self_heat_kilowatts_active = 0.5f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, refined_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(BatterySmart.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE, true, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000AA3B9 File Offset: 0x000A85B9
	public override void DoPostConfigureComplete(GameObject go)
	{
		BatterySmart batterySmart = go.AddOrGet<BatterySmart>();
		batterySmart.capacity = 20000f;
		batterySmart.joulesLostPerSecond = 0.6666667f;
		batterySmart.powerSortOrder = 1000;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000AA364 File Offset: 0x000A8564
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000087 RID: 135
	public const string ID = "BatterySmart";
}
