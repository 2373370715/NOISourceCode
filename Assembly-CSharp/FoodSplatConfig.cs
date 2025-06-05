using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class FoodSplatConfig : IEntityConfig
{
	// Token: 0x06000CE0 RID: 3296 RVA: 0x0017B8A0 File Offset: 0x00179AA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateBasicEntity("FoodSplat", STRINGS.ITEMS.FOOD.FOODSPLAT.NAME, STRINGS.ITEMS.FOOD.FOODSPLAT.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "idle_sticker_a", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x000AFDB2 File Offset: 0x000ADFB2
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<KSelectable>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		inst.AddOrGetDef<Splat.Def>();
		inst.AddOrGet<SplatWorkable>();
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040009AD RID: 2477
	public const string ID = "FoodSplat";
}
