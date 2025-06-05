using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000592 RID: 1426
public class PropHumanMurphyBedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060018A1 RID: 6305 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x001ABC54 File Offset: 0x001A9E54
	public GameObject CreatePrefab()
	{
		string id = "PropHumanMurphyBed";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_murphybed_kanim"), "on", Grid.SceneLayer.Building, 5, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
