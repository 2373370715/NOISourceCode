using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class MiniCometConfig : IEntityConfig
{
	// Token: 0x060013EB RID: 5099 RVA: 0x0019A564 File Offset: 0x00198764
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MiniCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.MINICOMET.NAME, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<LoopingSounds>();
		MiniComet miniComet = gameObject.AddOrGet<MiniComet>();
		Sim.PhysicsData defaultValues = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
		miniComet.impactSound = "MeteorDamage_Rock";
		miniComet.flyingSoundID = 2;
		miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		gameObject.AddOrGet<PrimaryElement>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("meteor_sand_kanim")
		};
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = "fall_loop";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		gameObject.AddOrGet<KCircleCollider2D>().radius = 0.5f;
		gameObject.AddTag(GameTags.Comet);
		gameObject.AddTag(GameTags.HideFromSpawnTool);
		gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
		return gameObject;
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DB6 RID: 3510
	public static readonly string ID = "MiniComet";

	// Token: 0x04000DB7 RID: 3511
	private const SimHashes element = SimHashes.Regolith;

	// Token: 0x04000DB8 RID: 3512
	private const int ADDED_CELLS = 6;
}
