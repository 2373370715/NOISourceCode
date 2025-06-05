using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class FullereneCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060012EC RID: 4844 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x001971AC File Offset: 0x001953AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(FullereneCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.FULLERENECOMET.NAME, "meteor_fullerene_kanim", SimHashes.Fullerene, new Vector2(3f, 20f), new Vector2(323.15f, 423.15f), "Meteor_Medium_Impact", 1, SimHashes.CarbonDioxide, SpawnFXHashes.MeteorImpactMetal, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.entityDamage = 15;
		component.totalTileDamage = 0.5f;
		component.affectedByDifficulty = false;
		return gameObject;
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D3B RID: 3387
	public static readonly string ID = "FullereneComet";
}
