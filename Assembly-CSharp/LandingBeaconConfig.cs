using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B2 RID: 946
public class LandingBeaconConfig : IBuildingConfig
{
	// Token: 0x06000F4E RID: 3918 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00186890 File Offset: 0x00184A90
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LandingBeacon";
		int width = 1;
		int height = 3;
		string anim = "landing_beacon_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		return buildingDef;
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x000B0F9A File Offset: 0x000AF19A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x000B0FBB File Offset: 0x000AF1BB
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x000B0FC3 File Offset: 0x000AF1C3
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x000B0FC3 File Offset: 0x000AF1C3
	public override void DoPostConfigureComplete(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000B0FCB File Offset: 0x000AF1CB
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility);
		};
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0018692C File Offset: 0x00184B2C
	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000B31 RID: 2865
	public const string ID = "LandingBeacon";

	// Token: 0x04000B32 RID: 2866
	public const int LANDING_ACCURACY = 3;
}
