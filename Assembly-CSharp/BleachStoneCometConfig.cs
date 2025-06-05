using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class BleachStoneCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600134F RID: 4943 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x001983C8 File Offset: 0x001965C8
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.OxyRock).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(BleachStoneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.BLEACHSTONECOMET.NAME, "meteor_bleachstone_kanim", SimHashes.BleachStone, new Vector2(mass * 0.8f * 1f, mass * 1.2f * 1f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 1, SimHashes.ChlorineGas, SpawnFXHashes.MeteorImpactIce, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.addTiles = 1;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 1;
		return gameObject;
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D53 RID: 3411
	public static string ID = "BleachStoneComet";

	// Token: 0x04000D54 RID: 3412
	private const int ADDED_CELLS = 1;
}
