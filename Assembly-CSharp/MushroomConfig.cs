using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000310 RID: 784
public class MushroomConfig : IEntityConfig
{
	// Token: 0x06000C2B RID: 3115 RVA: 0x0017A1F8 File Offset: 0x001783F8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MushroomConfig.ID, STRINGS.ITEMS.FOOD.MUSHROOM.NAME, STRINGS.ITEMS.FOOD.MUSHROOM.DESC, 1f, false, Assets.GetAnim("funguscap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHROOM);
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000AFBC0 File Offset: 0x000ADDC0
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, MushroomConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0017A25C File Offset: 0x0017845C
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
				if (UnityEngine.Random.value < MushroomConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("MushroomSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000949 RID: 2377
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400094A RID: 2378
	public static string ID = "Mushroom";

	// Token: 0x0400094B RID: 2379
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		MushroomConfig.OnEatComplete(component);
	});
}
