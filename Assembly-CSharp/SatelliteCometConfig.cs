using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class SatelliteCometConfig : IEntityConfig
{
	// Token: 0x060012FF RID: 4863 RVA: 0x00197534 File Offset: 0x00195734
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SatelliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SATELLITE.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		Comet comet = gameObject.AddOrGet<Comet>();
		comet.massRange = new Vector2(100f, 200f);
		comet.EXHAUST_ELEMENT = SimHashes.AluminumGas;
		comet.temperatureRange = new Vector2(473.15f, 573.15f);
		comet.entityDamage = 2;
		comet.explosionOreCount = new Vector2I(8, 8);
		comet.totalTileDamage = 2f;
		comet.splashRadius = 1;
		comet.impactSound = "Meteor_Large_Impact";
		comet.flyingSoundID = 1;
		comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		comet.addTiles = 0;
		comet.craterPrefabs = new string[]
		{
			"PropSurfaceSatellite1",
			PropSurfaceSatellite2Config.ID,
			PropSurfaceSatellite3Config.ID
		};
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(SimHashes.Aluminum, true);
		primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_rock_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.DeprecatedContent);
		return gameObject;
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D3F RID: 3391
	public static string ID = "SatelliteCometComet";
}
