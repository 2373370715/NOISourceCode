using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class MissileBasicConfig : IEntityConfig
{
	// Token: 0x06001412 RID: 5138 RVA: 0x0019AD98 File Offset: 0x00198F98
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DCA RID: 3530
	public const string ID = "MissileBasic";

	// Token: 0x04000DCB RID: 3531
	public static ComplexRecipe recipe;

	// Token: 0x04000DCC RID: 3532
	public const float MASS_PER_MISSILE = 10f;
}
