using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class StaterpillarGasConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000915 RID: 2325 RVA: 0x0016DDEC File Offset: 0x0016BFEC
	public static GameObject CreateStaterpillarGas(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "gas_consume_pre",
			inhaleAnimLoop = "gas_consume_loop",
			inhaleAnimPst = "gas_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarGasConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarGasBaseTrait", is_baby, ObjectLayer.GasConduit, StaterpillarGasConnectorConfig.ID, GameTags.Unbreathable, "gas_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Unbreathable;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarGasConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarGasConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarGasConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarGasBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarGasConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0016E044 File Offset: 0x0016C244
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGas", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "caterpillar_kanim", false), this, "StaterpillarGasEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarGasBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_GAS, 1, true, false, 1f, false);
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x000AE94B File Offset: 0x000ACB4B
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006FB RID: 1787
	public const string ID = "StaterpillarGas";

	// Token: 0x040006FC RID: 1788
	public const string BASE_TRAIT_ID = "StaterpillarGasBaseTrait";

	// Token: 0x040006FD RID: 1789
	public const string EGG_ID = "StaterpillarGasEgg";

	// Token: 0x040006FE RID: 1790
	public const int EGG_SORT_ORDER = 1;

	// Token: 0x040006FF RID: 1791
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000700 RID: 1792
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarGasConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000701 RID: 1793
	private static float STORAGE_CAPACITY = 100f;

	// Token: 0x04000702 RID: 1794
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x04000703 RID: 1795
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x04000704 RID: 1796
	private static float CONSUMPTION_RATE = 0.5f;

	// Token: 0x04000705 RID: 1797
	private static float INHALE_TIME = 6f;
}
