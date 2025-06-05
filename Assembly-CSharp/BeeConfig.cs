using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class BeeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000406 RID: 1030 RVA: 0x0015DE14 File Offset: 0x0015C014
	public static GameObject CreateBee(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseBeeConfig.BaseBee(id, name, desc, anim_file, "BeeBaseTrait", DECOR.BONUS.TIER4, is_baby, null);
		Trait trait = Db.Get().CreateTrait("BeeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 5f, name, false, false, true));
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x000AB5B2 File Offset: 0x000A97B2
	public GameObject CreatePrefab()
	{
		return BeeConfig.CreateBee("Bee", STRINGS.CREATURES.SPECIES.BEE.NAME, STRINGS.CREATURES.SPECIES.BEE.DESC, "bee_kanim", false);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x000AB5D8 File Offset: 0x000A97D8
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040002E5 RID: 741
	public const string ID = "Bee";

	// Token: 0x040002E6 RID: 742
	public const string BASE_TRAIT_ID = "BeeBaseTrait";
}
