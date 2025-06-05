using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class PrickleFruitConfig : IEntityConfig
{
	// Token: 0x06000C55 RID: 3157 RVA: 0x0017A638 File Offset: 0x00178838
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(PrickleFruitConfig.ID, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.NAME, STRINGS.ITEMS.FOOD.PRICKLEFRUIT.DESC, 1f, false, Assets.GetAnim("bristleberry_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.PRICKLEFRUIT);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x000AFC28 File Offset: 0x000ADE28
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, PrickleFruitConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0017A69C File Offset: 0x0017889C
	private static void OnEatComplete(Edible edible)
	{
		if (edible != null)
		{
			int num = 0;
			float unitsConsumed = edible.unitsConsumed;
			int num2 = Mathf.FloorToInt(unitsConsumed);
			float num3 = unitsConsumed % 1f;
			if (UnityEngine.Random.value < num3)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if (UnityEngine.Random.value < PrickleFruitConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("PrickleFlowerSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400095B RID: 2395
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400095C RID: 2396
	public static string ID = "PrickleFruit";

	// Token: 0x0400095D RID: 2397
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		PrickleFruitConfig.OnEatComplete(component);
	});
}
