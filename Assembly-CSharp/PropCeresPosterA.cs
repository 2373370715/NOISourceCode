using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class PropCeresPosterA : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001641 RID: 5697 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x001A28D8 File Offset: 0x001A0AD8
	public GameObject CreatePrefab()
	{
		string id = "PropCeresPosterA";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCERESPOSTERA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poster_ceres_a_kanim"), "art_a", Grid.SceneLayer.Building, 2, 3, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
