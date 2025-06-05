using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class UraniumCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001310 RID: 4880 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00197AB8 File Offset: 0x00195CB8
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.UraniumOre).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(UraniumCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.URANIUMORECOMET.NAME, "meteor_uranium_kanim", SimHashes.UraniumOre, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(323.15f, 403.15f), "Meteor_Nuclear_Impact", 3, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactUranium, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.entityDamage = 15;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D43 RID: 3395
	public static readonly string ID = "UraniumComet";

	// Token: 0x04000D44 RID: 3396
	private const SimHashes element = SimHashes.UraniumOre;

	// Token: 0x04000D45 RID: 3397
	private const int ADDED_CELLS = 6;
}
