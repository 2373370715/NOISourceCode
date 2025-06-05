using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class PhosphoricCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001341 RID: 4929 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x00198280 File Offset: 0x00196480
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(PhosphoricCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.PHOSPHORICCOMET.NAME, "meteor_phosphoric_kanim", SimHashes.Phosphorite, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_dust_heavy_Impact", 0, SimHashes.Void, SpawnFXHashes.MeteorImpactPhosphoric, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D50 RID: 3408
	public static string ID = "PhosphoricComet";
}
