using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200025B RID: 603
[EntityConfigOrder(1)]
public class PacuCleanerConfig : IEntityConfig
{
	// Token: 0x0600088A RID: 2186 RVA: 0x0016C2F0 File Offset: 0x0016A4F0
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePacuConfig.CreatePrefab(id, "PacuCleanerBaseTrait", name, desc, anim_file, is_baby, "glp_", 243.15f, 278.15f, 223.15f, 298.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PacuTuning.PEN_SIZE_PER_CREATURE, false);
		if (!is_baby)
		{
			Storage storage = gameObject.AddComponent<Storage>();
			storage.capacityKg = 10f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			PassiveElementConsumer passiveElementConsumer = gameObject.AddOrGet<PassiveElementConsumer>();
			passiveElementConsumer.elementToConsume = SimHashes.DirtyWater;
			passiveElementConsumer.consumptionRate = 0.2f;
			passiveElementConsumer.capacityKG = 10f;
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.showInStatusPanel = true;
			passiveElementConsumer.sampleCellOffset = new Vector3(0f, 0f, 0f);
			passiveElementConsumer.isRequired = false;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.showDescriptor = false;
			gameObject.AddOrGet<UpdateElementConsumerPosition>();
			BubbleSpawner bubbleSpawner = gameObject.AddComponent<BubbleSpawner>();
			bubbleSpawner.element = SimHashes.Water;
			bubbleSpawner.emitMass = 2f;
			bubbleSpawner.emitVariance = 0.5f;
			bubbleSpawner.initialVelocity = new Vector2f(0, 1);
			ElementConverter elementConverter = gameObject.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.2f, true)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.2f, SimHashes.Water, 0f, true, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
			};
		}
		return gameObject;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0016C468 File Offset: 0x0016A668
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuCleanerConfig.CreatePacu("PacuCleaner", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), this as IHasDlcRestrictions, "PacuCleanerEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_CLEANER.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuCleanerBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_CLEANER, 501, false, true, 0.75f, false);
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0016C4F4 File Offset: 0x0016A6F4
	public void OnSpawn(GameObject inst)
	{
		ElementConsumer component = inst.GetComponent<ElementConsumer>();
		if (component != null)
		{
			component.EnableConsumption(true);
		}
	}

	// Token: 0x04000684 RID: 1668
	public const string ID = "PacuCleaner";

	// Token: 0x04000685 RID: 1669
	public const string BASE_TRAIT_ID = "PacuCleanerBaseTrait";

	// Token: 0x04000686 RID: 1670
	public const string EGG_ID = "PacuCleanerEgg";

	// Token: 0x04000687 RID: 1671
	public const float POLLUTED_WATER_CONVERTED_PER_CYCLE = 120f;

	// Token: 0x04000688 RID: 1672
	public const SimHashes INPUT_ELEMENT = SimHashes.DirtyWater;

	// Token: 0x04000689 RID: 1673
	public const SimHashes OUTPUT_ELEMENT = SimHashes.Water;

	// Token: 0x0400068A RID: 1674
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x0400068B RID: 1675
	public const int EGG_SORT_ORDER = 501;
}
