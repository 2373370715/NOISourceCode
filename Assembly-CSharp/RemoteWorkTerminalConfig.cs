using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class RemoteWorkTerminalConfig : IBuildingConfig
{
	// Token: 0x06001775 RID: 6005 RVA: 0x001A60AC File Offset: 0x001A42AC
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkTerminalConfig.ID;
		int width = 3;
		int height = 3;
		string anim = "remote_work_terminal_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x001A612C File Offset: 0x001A432C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
		go.AddComponent<RemoteWorkTerminalSM>();
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = RemoteWorkTerminalConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(RemoteWorkTerminalConfig.INPUT_MATERIAL, 0.013333334f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGet<ElementConverterOperationalRequirement>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x000B4498 File Offset: 0x000B2698
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC3_ID"
		};
	}

	// Token: 0x04000F77 RID: 3959
	public static string ID = "RemoteWorkTerminal";

	// Token: 0x04000F78 RID: 3960
	public static readonly Tag INPUT_MATERIAL = DatabankHelper.TAG;

	// Token: 0x04000F79 RID: 3961
	public const float INPUT_CAPACITY = 10f;

	// Token: 0x04000F7A RID: 3962
	public const float INPUT_CONSUMPTION_RATE_PER_S = 0.013333334f;

	// Token: 0x04000F7B RID: 3963
	public const float INPUT_REFILL_RATIO = 0.5f;
}
