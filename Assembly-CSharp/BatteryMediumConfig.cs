using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class BatteryMediumConfig : BaseBatteryConfig
{
	// Token: 0x060000C0 RID: 192 RVA: 0x001499C8 File Offset: 0x00147BC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatteryMedium";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterymed_kanim";
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x000AA387 File Offset: 0x000A8587
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 40000f;
		battery.joulesLostPerSecond = 3.3333333f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000AA364 File Offset: 0x000A8564
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x04000083 RID: 131
	public const string ID = "BatteryMedium";
}
