using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200022B RID: 555
[EntityConfigOrder(1)]
public class DivergentWormConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000791 RID: 1937 RVA: 0x00168B54 File Offset: 0x00166D54
	public static GameObject CreateWorm(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDivergentConfig.BaseDivergent(id, name, desc, 200f, anim_file, "DivergentWormBaseTrait", is_baby, 8f, null, "DivergentCropTendedWorm", 3, false), DivergentTuning.PEN_SIZE_PER_CREATURE_WORM);
		Trait trait = Db.Get().CreateTrait("DivergentWormBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DivergentTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DivergentTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		prefab.AddWeapon(2f, 3f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		List<Diet.Info> list = BaseDivergentConfig.BasicSulfurDiet(SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, null, 0f);
		list.Add(new Diet.Info(new HashSet<Tag>
		{
			SimHashes.Sucrose.CreateTag()
		}, SimHashes.Mud.CreateTag(), DivergentWormConfig.CALORIES_PER_KG_OF_SUCROSE, 1f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null));
		GameObject gameObject = BaseDivergentConfig.SetupDiet(prefab, list, DivergentWormConfig.CALORIES_PER_KG_OF_ORE, DivergentWormConfig.MINI_POOP_SIZE_IN_KG);
		SegmentedCreature.Def def = gameObject.AddOrGetDef<SegmentedCreature.Def>();
		def.segmentTrackerSymbol = new HashedString("segmenttracker");
		def.numBodySegments = 5;
		def.midAnim = Assets.GetAnim("worm_torso_kanim");
		def.tailAnim = Assets.GetAnim("worm_tail_kanim");
		def.animFrameOffset = 2;
		def.pathSpacing = 0.2f;
		def.numPathNodes = 15;
		def.minSegmentSpacing = 0.1f;
		def.maxSegmentSpacing = 0.4f;
		def.retractionSegmentSpeed = 1f;
		def.retractionPathSpeed = 2f;
		def.compressedMaxScale = 0.25f;
		def.headOffset = new Vector3(0.12f, 0.4f, 0f);
		return gameObject;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00168DA0 File Offset: 0x00166FA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DivergentWormConfig.CreateWorm("DivergentWorm", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "worm_head_kanim", false), this, "DivergentWormEgg", STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.EGG_NAME, STRINGS.CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.DESC, "egg_worm_kanim", DivergentTuning.EGG_MASS, "DivergentWormBaby", 90f, 30f, DivergentTuning.EGG_CHANCES_WORM, DivergentWormConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005A3 RID: 1443
	public const string ID = "DivergentWorm";

	// Token: 0x040005A4 RID: 1444
	public const string BASE_TRAIT_ID = "DivergentWormBaseTrait";

	// Token: 0x040005A5 RID: 1445
	public const string EGG_ID = "DivergentWormEgg";

	// Token: 0x040005A6 RID: 1446
	private const float LIFESPAN = 150f;

	// Token: 0x040005A7 RID: 1447
	public const float CROP_TENDED_MULTIPLIER_EFFECT = 0.5f;

	// Token: 0x040005A8 RID: 1448
	public const float CROP_TENDED_MULTIPLIER_DURATION = 600f;

	// Token: 0x040005A9 RID: 1449
	private const int NUM_SEGMENTS = 5;

	// Token: 0x040005AA RID: 1450
	private const SimHashes EMIT_ELEMENT = SimHashes.Mud;

	// Token: 0x040005AB RID: 1451
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040005AC RID: 1452
	private static float KG_SUCROSE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040005AD RID: 1453
	private static float CALORIES_PER_KG_OF_ORE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040005AE RID: 1454
	private static float CALORIES_PER_KG_OF_SUCROSE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE / DivergentWormConfig.KG_SUCROSE_EATEN_PER_CYCLE;

	// Token: 0x040005AF RID: 1455
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040005B0 RID: 1456
	private static float MINI_POOP_SIZE_IN_KG = 4f;
}
