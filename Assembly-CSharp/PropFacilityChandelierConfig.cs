using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class PropFacilityChandelierConfig : IEntityConfig
{
	// Token: 0x06001679 RID: 5753 RVA: 0x001A30EC File Offset: 0x001A12EC
	public GameObject CreatePrefab()
	{
		string id = "PropFacilityChandelier";
		string name = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPFACILITYCHANDELIER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_chandelier_kanim"), "off", Grid.SceneLayer.Building, 5, 7, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x001A2F50 File Offset: 0x001A1150
	public void OnSpawn(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(inst);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
		}
	}
}
