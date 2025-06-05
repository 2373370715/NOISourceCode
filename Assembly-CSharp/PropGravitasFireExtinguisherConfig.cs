using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class PropGravitasFireExtinguisherConfig : IEntityConfig
{
	// Token: 0x060016C9 RID: 5833 RVA: 0x001A3D60 File Offset: 0x001A1F60
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFireExtinguisher";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIREEXTINGUISHER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_fireextinguisher_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
