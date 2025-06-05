using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class LonelyMinionConfig : IEntityConfig
{
	// Token: 0x060013DB RID: 5083 RVA: 0x0019A144 File Offset: 0x00198344
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODEL.STANDARD.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name, true);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "idle_default";
		kbatchedAnimController.initialAnim = "idle_default";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		this.ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(string.Format("cheek_00{0}", def.Personality.headShape)), 1);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x0019A288 File Offset: 0x00198488
	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		kbatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(LonelyMinionConfig.PARCEL_SNAPTO, false);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = component;
		kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
	}

	// Token: 0x04000D99 RID: 3481
	public static string ID = "LonelyMinion";

	// Token: 0x04000D9A RID: 3482
	public const int VOICE_IDX = -2;

	// Token: 0x04000D9B RID: 3483
	public const int STARTING_SKILL_POINTS = 3;

	// Token: 0x04000D9C RID: 3484
	public const int BASE_ATTRIBUTE_LEVEL = 7;

	// Token: 0x04000D9D RID: 3485
	public const int AGE_MIN = 2190;

	// Token: 0x04000D9E RID: 3486
	public const int AGE_MAX = 3102;

	// Token: 0x04000D9F RID: 3487
	public const float MIN_IDLE_DELAY = 20f;

	// Token: 0x04000DA0 RID: 3488
	public const float MAX_IDLE_DELAY = 40f;

	// Token: 0x04000DA1 RID: 3489
	public const string IDLE_PREFIX = "idle_blinds";

	// Token: 0x04000DA2 RID: 3490
	public static readonly HashedString GreetingCriteraId = "Neighbor";

	// Token: 0x04000DA3 RID: 3491
	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	// Token: 0x04000DA4 RID: 3492
	public static readonly HashedString DecorCriteriaId = "Decor";

	// Token: 0x04000DA5 RID: 3493
	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	// Token: 0x04000DA6 RID: 3494
	public static readonly HashedString CHECK_MAIL = "mail_pre";

	// Token: 0x04000DA7 RID: 3495
	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	// Token: 0x04000DA8 RID: 3496
	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	// Token: 0x04000DA9 RID: 3497
	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	// Token: 0x04000DAA RID: 3498
	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	// Token: 0x04000DAB RID: 3499
	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	// Token: 0x04000DAC RID: 3500
	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	// Token: 0x04000DAD RID: 3501
	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	// Token: 0x04000DAE RID: 3502
	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	// Token: 0x04000DAF RID: 3503
	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	// Token: 0x04000DB0 RID: 3504
	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	// Token: 0x04000DB1 RID: 3505
	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	// Token: 0x04000DB2 RID: 3506
	public const string PERSONALITY_ID = "JORGE";

	// Token: 0x04000DB3 RID: 3507
	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";
}
