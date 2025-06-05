using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class CarrotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BBA RID: 3002 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0017973C File Offset: 0x0017793C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(CarrotConfig.ID, STRINGS.ITEMS.FOOD.CARROT.NAME, STRINGS.ITEMS.FOOD.CARROT.DESC, 1f, false, Assets.GetAnim("purplerootVegetable_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.CARROT);
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x000AFB58 File Offset: 0x000ADD58
	public void OnSpawn(GameObject inst)
	{
		inst.Subscribe(-10536414, CarrotConfig.OnEatCompleteDelegate);
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x001797A0 File Offset: 0x001779A0
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
				if (UnityEngine.Random.value < CarrotConfig.SEEDS_PER_FRUIT_CHANCE)
				{
					num++;
				}
			}
			if (num > 0)
			{
				Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
				vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("CarrotPlantSeed")), vector, Grid.SceneLayer.Ore, null, 0);
				PrimaryElement component = edible.GetComponent<PrimaryElement>();
				PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
				component2.Temperature = component.Temperature;
				component2.Units = (float)num;
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0400091C RID: 2332
	public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

	// Token: 0x0400091D RID: 2333
	public static string ID = "Carrot";

	// Token: 0x0400091E RID: 2334
	private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		CarrotConfig.OnEatComplete(component);
	});
}
