using System;
using TUNING;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class OxysconceConfig : IBuildingConfig
{
	// Token: 0x060015CD RID: 5581 RVA: 0x000AA536 File Offset: 0x000A8736
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x001A0A9C File Offset: 0x0019EC9C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Oxysconce";
		int width = 1;
		int height = 1;
		string anim = "oxy_sconce_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x001A0B1C File Offset: 0x0019ED1C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		new CellOffset(0, 0);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 240f;
		storage.showInUI = true;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.OxyRock.CreateTag();
		manualDeliveryKG.capacity = 240f;
		manualDeliveryKG.refillMass = 96f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<StorageMeter>();
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x000B40BF File Offset: 0x000B22BF
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.oxygenGenerators.Add(game_object);
		};
	}

	// Token: 0x04000F07 RID: 3847
	public const string ID = "Oxysconce";

	// Token: 0x04000F08 RID: 3848
	private const float OXYLITE_STORAGE = 240f;
}
