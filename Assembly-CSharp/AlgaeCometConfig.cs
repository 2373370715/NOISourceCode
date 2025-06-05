using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class AlgaeCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600133A RID: 4922 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x001981E8 File Offset: 0x001963E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseCometConfig.BaseComet(AlgaeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ALGAECOMET.NAME, "meteor_algae_kanim", SimHashes.Algae, new Vector2(3f, 20f), new Vector2(310.15f, 323.15f), "Meteor_algae_Impact", 7, SimHashes.Void, SpawnFXHashes.MeteorImpactAlgae, 0.3f);
		Comet component = gameObject.GetComponent<Comet>();
		component.explosionOreCount = new Vector2I(2, 4);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		return gameObject;
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D4F RID: 3407
	public static string ID = "AlgaeComet";
}
