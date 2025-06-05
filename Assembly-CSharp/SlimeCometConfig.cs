using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200046A RID: 1130
public class SlimeCometConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001317 RID: 4887 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00197B78 File Offset: 0x00195D78
	public GameObject CreatePrefab()
	{
		float mass = ElementLoader.FindElementByHash(SimHashes.SlimeMold).defaultValues.mass;
		GameObject gameObject = BaseCometConfig.BaseComet(SlimeCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.SLIMECOMET.NAME, "meteor_slime_kanim", SimHashes.SlimeMold, new Vector2(mass * 0.8f * 2f, mass * 1.2f * 2f), new Vector2(310.15f, 323.15f), "Meteor_slimeball_Impact", 7, SimHashes.ContaminatedOxygen, SpawnFXHashes.MeteorImpactSlime, 0.6f);
		Comet component = gameObject.GetComponent<Comet>();
		component.entityDamage = 0;
		component.totalTileDamage = 0f;
		component.explosionOreCount = new Vector2I(1, 2);
		component.explosionSpeedRange = new Vector2(4f, 7f);
		component.addTiles = 2;
		component.addTilesMinHeight = 1;
		component.addTilesMaxHeight = 2;
		component.diseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		component.addDiseaseCount = (int)(component.EXHAUST_RATE * 100000f);
		return gameObject;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00197C7C File Offset: 0x00195E7C
	public void OnSpawn(GameObject go)
	{
		go.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex("SlimeLung"), (int)(UnityEngine.Random.Range(0.9f, 1.2f) * 50f * 100000f), "Meteor");
	}

	// Token: 0x04000D46 RID: 3398
	public static string ID = "SlimeComet";

	// Token: 0x04000D47 RID: 3399
	public const int ADDED_CELLS = 2;

	// Token: 0x04000D48 RID: 3400
	private const SimHashes element = SimHashes.SlimeMold;
}
