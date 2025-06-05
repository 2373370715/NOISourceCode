using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class OxyliteCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001348 RID: 4936 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x00198318 File Offset: 0x00196518
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(OxyliteCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.OXYLITECOMET.NAME, "meteor_oxylite_kanim", SimHashes.OxyRock, new Vector2(mass * 0.8f * 6f, mass * 1.2f * 6f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Oxygen, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 6;
		component.addTilesMinHeight = 2;
		component.addTilesMaxHeight = 8;
		return gameObject;
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D51 RID: 3409
	public static string ID = "OxyliteComet";

	// Token: 0x04000D52 RID: 3410
	private const int ADDED_CELLS = 6;
}
