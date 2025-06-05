using System;
using TUNING;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class WarpConduitReceiverConfig : IBuildingConfig
{
	// Token: 0x06001ADE RID: 6878 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x001B5478 File Offset: 0x001B3678
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitReceiver";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_receiver_kanim";
		int hitpoints = 250;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.DefaultAnimState = "off";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Disinfectable = false;
		buildingDef.Invincible = true;
		buildingDef.Repairable = false;
		return buildingDef;
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000B5FA5 File Offset: 0x000B41A5
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.solidOutputPort;
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x001B54F8 File Offset: 0x001B36F8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitReceiver warpConduitReceiver = go.AddOrGet<WarpConduitReceiver>();
		warpConduitReceiver.liquidPortInfo = this.liquidOutputPort;
		warpConduitReceiver.gasPortInfo = this.gasOutputPort;
		warpConduitReceiver.solidPortInfo = this.solidOutputPort;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.workAnims = new HashedString[]
		{
			"touchpanel_interact_pre",
			"touchpanel_interact_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_receiver_kanim")
		};
		activatable.SetWorkTime(30f);
		go.AddComponent<ConduitSecondaryOutput>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000B5FDA File Offset: 0x000B41DA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000B600E File Offset: 0x000B420E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000B6026 File Offset: 0x000B4226
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x04001149 RID: 4425
	public const string ID = "WarpConduitReceiver";

	// Token: 0x0400114A RID: 4426
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));

	// Token: 0x0400114B RID: 4427
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(-1, 1));

	// Token: 0x0400114C RID: 4428
	private ConduitPortInfo solidOutputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(1, 1));
}
