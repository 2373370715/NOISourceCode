using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
public class TelephoneConfig : IBuildingConfig
{
	// Token: 0x06001A50 RID: 6736 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x001B3574 File Offset: 0x001B1774
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telephone";
		int width = 1;
		int height = 2;
		string anim = "telephone_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x001B3618 File Offset: 0x001B1818
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Telephone telephone = go.AddOrGet<Telephone>();
		telephone.babbleEffect = "TelephoneBabble";
		telephone.chatEffect = "TelephoneChat";
		telephone.longDistanceEffect = "TelephoneLongDistance";
		telephone.trackingEffect = "RecentlyTelephoned";
		go.AddOrGet<TelephoneCallerWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04001103 RID: 4355
	public const string ID = "Telephone";

	// Token: 0x04001104 RID: 4356
	public const float ringTime = 15f;

	// Token: 0x04001105 RID: 4357
	public const float callTime = 25f;
}
