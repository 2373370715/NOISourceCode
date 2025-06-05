using System;
using TUNING;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
public class WarpConduitSenderConfig : IBuildingConfig
{
	// Token: 0x06001AE6 RID: 6886 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x001B5654 File Offset: 0x001B3854
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitSender";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_sender_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.CanMove = true;
		buildingDef.Invincible = true;
		return buildingDef;
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000B603D File Offset: 0x000B423D
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x001B56CC File Offset: 0x001B38CC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitSender warpConduitSender = go.AddOrGet<WarpConduitSender>();
		warpConduitSender.liquidPortInfo = this.liquidInputPort;
		warpConduitSender.gasPortInfo = this.gasInputPort;
		warpConduitSender.solidPortInfo = this.solidInputPort;
		warpConduitSender.gasStorage = go.AddComponent<Storage>();
		warpConduitSender.gasStorage.showInUI = false;
		warpConduitSender.gasStorage.capacityKg = 1f;
		warpConduitSender.liquidStorage = go.AddComponent<Storage>();
		warpConduitSender.liquidStorage.showInUI = false;
		warpConduitSender.liquidStorage.capacityKg = 10f;
		warpConduitSender.solidStorage = go.AddComponent<Storage>();
		warpConduitSender.solidStorage.showInUI = false;
		warpConduitSender.solidStorage.capacityKg = 100f;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_sender_kanim")
		};
		activatable.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"sending_pre"
		};
		activatable.SetWorkTime(30f);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000B5FDA File Offset: 0x000B41DA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x000B6072 File Offset: 0x000B4272
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x000B608A File Offset: 0x000B428A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x0400114D RID: 4429
	public const string ID = "WarpConduitSender";

	// Token: 0x0400114E RID: 4430
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 1));

	// Token: 0x0400114F RID: 4431
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));

	// Token: 0x04001150 RID: 4432
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(2, 1));
}
