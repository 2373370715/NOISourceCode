using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class LonelyMinionHouseConfig : IBuildingConfig
{
	// Token: 0x060010C5 RID: 4293 RVA: 0x0018BBF8 File Offset: 0x00189DF8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LonelyMinionHouse";
		int width = 4;
		int height = 6;
		string anim = "lonely_dupe_home_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, LonelyMinionHouseConfig.HOUSE_DECOR, none, 0.2f);
		buildingDef.DefaultAnimState = "on";
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.AddLogicPowerPort = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(2, 1);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0018BCB0 File Offset: 0x00189EB0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<NonEssentialEnergyConsumer>();
		go.GetComponent<Deconstructable>().allowDeconstruction = false;
		Prioritizable.AddRef(go);
		go.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.high, 5));
		Storage storage = go.AddOrGet<Storage>();
		KnockKnock knockKnock = go.AddOrGet<KnockKnock>();
		LonelyMinionHouse.Def def = go.AddOrGetDef<LonelyMinionHouse.Def>();
		storage.allowItemRemoval = false;
		storage.capacityKg = 250000f;
		storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
		storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		knockKnock.triggerWorkReactions = false;
		knockKnock.synchronizeAnims = false;
		knockKnock.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_doorknock_kanim")
		};
		knockKnock.workAnims = new HashedString[]
		{
			"knocking_pre",
			"knocking_loop"
		};
		knockKnock.workingPstComplete = new HashedString[]
		{
			"knocking_pst"
		};
		knockKnock.workingPstFailed = null;
		knockKnock.SetButtonTextOverride(new ButtonMenuTextOverride
		{
			Text = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT,
			CancelText = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.CANCELTEXT,
			ToolTip = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TOOLTIP,
			CancelToolTip = CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.CANCEL_TOOLTIP
		});
		def.Story = Db.Get().Stories.LonelyMinion;
		def.CompletionData = new StoryCompleteData
		{
			KeepSakeSpawnOffset = default(CellOffset),
			CameraTargetOffset = new CellOffset(0, 3)
		};
		def.InitalLoreId = "story_trait_lonelyminion_initial";
		def.EventIntroInfo = new StoryManager.PopupInfo
		{
			Title = CODEX.STORY_TRAITS.LONELYMINION.BEGIN_POPUP.NAME,
			Description = CODEX.STORY_TRAITS.LONELYMINION.BEGIN_POPUP.DESCRIPTION,
			CloseButtonText = CODEX.STORY_TRAITS.CLOSE_BUTTON,
			TextureName = "minionhouseactivate_kanim",
			DisplayImmediate = true,
			PopupType = EventInfoDataHelper.PopupType.BEGIN
		};
		def.CompleteLoreId = "story_trait_lonelyminion_complete";
		def.EventCompleteInfo = new StoryManager.PopupInfo
		{
			Title = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.NAME,
			Description = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.DESCRIPTION,
			CloseButtonText = CODEX.STORY_TRAITS.LONELYMINION.END_POPUP.BUTTON,
			TextureName = "minionhousecomplete_kanim",
			PopupType = EventInfoDataHelper.PopupType.COMPLETE
		};
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x000B1D50 File Offset: 0x000AFF50
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
		this.ConfigureLights(go);
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0018BF00 File Offset: 0x0018A100
	private void ConfigureLights(GameObject go)
	{
		GameObject gameObject = new GameObject("FestiveLights");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(go.transform);
		gameObject.AddOrGet<Light2D>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = component.AnimFiles;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialAnim = "meter_lights_off";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.SetAnimControllers(kbatchedAnimController, component);
		kbatchedAnimTracker.symbol = "lights_target";
		kbatchedAnimTracker.offset = Vector3.zero;
		for (int i = 0; i < LonelyMinionHouseConfig.LIGHTS_SYMBOLS.Length; i++)
		{
			component.SetSymbolVisiblity(LonelyMinionHouseConfig.LIGHTS_SYMBOLS[i], false);
		}
	}

	// Token: 0x04000BA9 RID: 2985
	public const string ID = "LonelyMinionHouse";

	// Token: 0x04000BAA RID: 2986
	public const string LORE_UNLOCK_PREFIX = "story_trait_lonelyminion_";

	// Token: 0x04000BAB RID: 2987
	public const int FriendshipQuestCount = 3;

	// Token: 0x04000BAC RID: 2988
	public const string METER_TARGET = "meter_storage_target";

	// Token: 0x04000BAD RID: 2989
	public const string METER_ANIM = "meter";

	// Token: 0x04000BAE RID: 2990
	public static readonly string[] METER_SYMBOLS = new string[]
	{
		"meter_storage",
		"meter_level"
	};

	// Token: 0x04000BAF RID: 2991
	public const string BLINDS_TARGET = "blinds_target";

	// Token: 0x04000BB0 RID: 2992
	public const string BLINDS_PREFIX = "meter_blinds";

	// Token: 0x04000BB1 RID: 2993
	public static readonly string[] BLINDS_SYMBOLS = new string[]
	{
		"blinds_target",
		"blind",
		"blind_string",
		"blinds"
	};

	// Token: 0x04000BB2 RID: 2994
	private const string LIGHTS_TARGET = "lights_target";

	// Token: 0x04000BB3 RID: 2995
	private static readonly string[] LIGHTS_SYMBOLS = new string[]
	{
		"lights_target",
		"festive_lights",
		"lights_wire",
		"light_bulb",
		"snapTo_light_locator"
	};

	// Token: 0x04000BB4 RID: 2996
	public static readonly HashedString ANSWER = "answer";

	// Token: 0x04000BB5 RID: 2997
	public static readonly HashedString LIGHTS_OFF = "meter_lights_off";

	// Token: 0x04000BB6 RID: 2998
	public static readonly HashedString LIGHTS_ON = "meter_lights_on_loop";

	// Token: 0x04000BB7 RID: 2999
	public static readonly HashedString STORAGE = "storage_off";

	// Token: 0x04000BB8 RID: 3000
	public static readonly HashedString STORAGE_WORK_PST = "working_pst";

	// Token: 0x04000BB9 RID: 3001
	public static readonly HashedString[] STORAGE_WORKING = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04000BBA RID: 3002
	public static readonly EffectorValues HOUSE_DECOR = new EffectorValues
	{
		amount = -25,
		radius = 6
	};

	// Token: 0x04000BBB RID: 3003
	public static readonly EffectorValues STORAGE_DECOR = DECOR.PENALTY.TIER1;
}
