﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class StaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
	public static GameObject CreateStaterpillar(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarBaseTrait", is_baby, ObjectLayer.Wire, StaterpillarGeneratorConfig.ID, Tag.Invalid, null, 283.15f, 313.15f, 173.15f, 373.15f, null), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		Trait trait = Db.Get().CreateTrait("StaterpillarBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		GameObject gameObject = BaseStaterpillarConfig.SetupDiet(prefab, list);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarConfig.CreateStaterpillar("Staterpillar", STRINGS.CREATURES.SPECIES.STATERPILLAR.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "caterpillar_kanim", false), this, "StaterpillarEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_BASE, 0, true, false, 1f, false);
	}

	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gulp", false);
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public const string ID = "Staterpillar";

	public const string BASE_TRAIT_ID = "StaterpillarBaseTrait";

	public const string EGG_ID = "StaterpillarEgg";

	public const int EGG_SORT_ORDER = 0;

	private static float KG_ORE_EATEN_PER_CYCLE = 60f;

	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarConfig.KG_ORE_EATEN_PER_CYCLE;
}
