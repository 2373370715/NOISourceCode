using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class WaterCoolerConfig : IBuildingConfig
{
	// Token: 0x06001B07 RID: 6919 RVA: 0x001B5F10 File Offset: 0x001B4110
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WaterCooler";
		int width = 2;
		int height = 2;
		string anim = "watercooler_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WATER);
		return buildingDef;
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x001B5F90 File Offset: 0x001B4190
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 9f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		HeatImmunityProvider.Def def = go.AddOrGetDef<HeatImmunityProvider.Def>();
		def.range = new CellOffset[][]
		{
			new CellOffset[]
			{
				new CellOffset(1, 0)
			},
			new CellOffset[]
			{
				new CellOffset(0, 0)
			}
		};
		def.overrideFileName = new Func<GameObject, string>(this.GetImmunityProviderAnimFileName);
		def.overrideAnims = new string[]
		{
			"working_pre",
			"working_loop",
			"working_pst"
		};
		def.specialRequirements = new Func<GameObject, bool>(this.RefreshFromHeatCondition);
		HeatImmunityProvider.Def def2 = def;
		def2.onEffectApplied = (Action<GameObject, HeatImmunityProvider.Instance>)Delegate.Combine(def2.onEffectApplied, new Action<GameObject, HeatImmunityProvider.Instance>(this.OnHeatImmunityEffectApplied));
		go.AddOrGet<WaterCooler>();
		WaterCooler.OnDuplicantDrank = (Action<GameObject, GameObject>)Delegate.Combine(WaterCooler.OnDuplicantDrank, new Action<GameObject, GameObject>(this.ApplyImmunityEffectWhenDrankRecreationally));
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x001B6128 File Offset: 0x001B4328
	private string GetImmunityProviderAnimFileName(GameObject theEntitySeekingImmunity)
	{
		if (theEntitySeekingImmunity == null)
		{
			return "anim_interacts_watercooler_kanim";
		}
		MinionIdentity component = theEntitySeekingImmunity.GetComponent<MinionIdentity>();
		if (component != null && component.model == BionicMinionConfig.MODEL)
		{
			return "anim_bionic_interacts_watercooler_kanim";
		}
		return "anim_interacts_watercooler_kanim";
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x001B6174 File Offset: 0x001B4374
	private void ApplyImmunityEffectWhenDrankRecreationally(GameObject duplicant, GameObject waterCoolerInstance)
	{
		HeatImmunityProvider.Instance smi = waterCoolerInstance.GetSMI<HeatImmunityProvider.Instance>();
		if (smi != null)
		{
			smi.ApplyImmunityEffect(duplicant, false);
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000B6110 File Offset: 0x000B4310
	private void OnHeatImmunityEffectApplied(GameObject duplicant, HeatImmunityProvider.Instance smi)
	{
		smi.GetSMI<WaterCooler.StatesInstance>().Drink(duplicant, false);
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x001B6194 File Offset: 0x001B4394
	private bool RefreshFromHeatCondition(GameObject go_instance)
	{
		WaterCooler.StatesInstance smi = go_instance.GetSMI<WaterCooler.StatesInstance>();
		return smi != null && smi.IsInsideState(smi.sm.dispensing);
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400115D RID: 4445
	public const string ID = "WaterCooler";

	// Token: 0x0400115E RID: 4446
	public static global::Tuple<Tag, string>[] BEVERAGE_CHOICE_OPTIONS = new global::Tuple<Tag, string>[]
	{
		new global::Tuple<Tag, string>(SimHashes.Water.CreateTag(), ""),
		new global::Tuple<Tag, string>(SimHashes.Milk.CreateTag(), "DuplicantGotMilk")
	};

	// Token: 0x0400115F RID: 4447
	public const string MilkEffectID = "DuplicantGotMilk";
}
