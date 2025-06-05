using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class HeatCubeConfig : IEntityConfig
{
	// Token: 0x060013AC RID: 5036 RVA: 0x00199CCC File Offset: 0x00197ECC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("HeatCube", "Heat Cube", "A cube that holds heat.", 1000f, true, Assets.GetAnim("copper_kanim"), "idle_tallstone", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.BUILDINGELEMENTS, SimHashes.Diamond, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D90 RID: 3472
	public const string ID = "HeatCube";
}
