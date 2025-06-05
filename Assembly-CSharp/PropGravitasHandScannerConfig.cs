using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public class PropGravitasHandScannerConfig : IEntityConfig
{
	// Token: 0x060016D6 RID: 5846 RVA: 0x001A3FB0 File Offset: 0x001A21B0
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasHandScanner";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASHANDSCANNER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_hand_scanner_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
