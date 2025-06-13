using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class RanchStationConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RanchStation";
		int width = 2;
		int height = 3;
		string anim = "rancherstation_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRanchStation.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
		def.IsCritterEligibleToBeRanchedCb = ((GameObject creature_go, RanchStation.Instance ranch_station_smi) => !creature_go.GetComponent<Effects>().HasEffect("Ranched"));
		def.OnRanchCompleteCb = delegate(GameObject creature_go, WorkerBase rancher_wb)
		{
			creature_go.GetSMI<RanchableMonitor.Instance>().TargetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>();
			Attributes attributes = rancher_wb.GetAttributes();
			float num = (attributes != null) ? attributes.Get(Db.Get().Attributes.Ranching.Id).GetTotalValue() : 0f;
			float num2 = 1f + num * 0.1f;
			creature_go.GetComponent<Effects>().Add("Ranched", true).timeRemaining *= num2;
			AmountInstance amountInstance = Db.Get().Amounts.HitPoints.Lookup(creature_go);
			if (amountInstance != null)
			{
				amountInstance.ApplyDelta(amountInstance.GetMax() - amountInstance.value + 1f);
			}
		};
		def.RanchedPreAnim = "grooming_pre";
		def.RanchedLoopAnim = "grooming_loop";
		def.RanchedPstAnim = "grooming_pst";
		def.WorkTime = 12f;
		def.GetTargetRanchCell = delegate(RanchStation.Instance smi)
		{
			int result = Grid.InvalidCell;
			if (!smi.IsNullOrStopped())
			{
				result = Grid.CellRight(Grid.PosToCell(smi.transform.GetPosition()));
			}
			return result;
		};
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
		Prioritizable.AddRef(go);
	}

	public const string ID = "RanchStation";
}
