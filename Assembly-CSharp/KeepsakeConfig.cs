using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class KeepsakeConfig : IMultiEntityConfig
{
	// Token: 0x060013B6 RID: 5046 RVA: 0x00199E04 File Offset: 0x00198004
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(KeepsakeConfig.CreateKeepsake("MegaBrain", UI.KEEPSAKES.MEGA_BRAIN.NAME, UI.KEEPSAKES.MEGA_BRAIN.DESCRIPTION, "keepsake_mega_brain_kanim", "idle", "ui", null, null, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("CritterManipulator", UI.KEEPSAKES.CRITTER_MANIPULATOR.NAME, UI.KEEPSAKES.CRITTER_MANIPULATOR.DESCRIPTION, "keepsake_critter_manipulator_kanim", "idle", "ui", null, null, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("LonelyMinion", UI.KEEPSAKES.LONELY_MINION.NAME, UI.KEEPSAKES.LONELY_MINION.DESCRIPTION, "keepsake_lonelyminion_kanim", "idle", "ui", null, null, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("FossilHunt", UI.KEEPSAKES.FOSSIL_HUNT.NAME, UI.KEEPSAKES.FOSSIL_HUNT.DESCRIPTION, "keepsake_fossil_dig_kanim", "idle", "ui", null, null, null, SimHashes.Creature));
		list.Add(KeepsakeConfig.CreateKeepsake("GeothermalPlant", UI.KEEPSAKES.GEOTHERMAL_PLANT.NAME, UI.KEEPSAKES.GEOTHERMAL_PLANT.DESCRIPTION, "keepsake_geothermal_vent_kanim", "idle", "ui", DlcManager.DLC2, null, null, SimHashes.Creature));
		GameObject gameObject = KeepsakeConfig.CreateKeepsake("MorbRoverMaker", UI.KEEPSAKES.MORB_ROVER_MAKER.NAME, UI.KEEPSAKES.MORB_ROVER_MAKER.DESCRIPTION, "keepsake_morb_tank_kanim", "idle", "ui", null, null, null, SimHashes.Creature);
		gameObject.AddOrGetDef<MorbRoverMakerKeepsake.Def>();
		list.Add(gameObject);
		list.RemoveAll((GameObject x) => x == null);
		return list;
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x00199FAC File Offset: 0x001981AC
	public static GameObject CreateKeepsake(string id, string name, string desc, string animFile, string initial_anim = "idle", string ui_anim = "ui", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, KeepsakeConfig.PostInitFn postInitFn = null, SimHashes element = SimHashes.Creature)
	{
		if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
		{
			return null;
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity("keepsake_" + id.ToLower(), name, desc, 25f, true, Assets.GetAnim(animFile), initial_anim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.KEEPSAKES, element, new List<Tag>
		{
			GameTags.MiscPickupable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<KSelectable>();
		gameObject.GetComponent<KBatchedAnimController>().initialMode = KAnim.PlayMode.Loop;
		if (postInitFn != null)
		{
			postInitFn(gameObject);
		}
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.AddTag(GameTags.Keepsake, false);
		KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
		component2.requiredDlcIds = requiredDlcIds;
		component2.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x0019A098 File Offset: 0x00198298
	[Obsolete]
	public static GameObject CreateKeepsake(string id, string name, string desc, string animFile, string initial_anim = "idle", string ui_anim = "ui", string[] dlcIDs = null, KeepsakeConfig.PostInitFn postInitFn = null, SimHashes element = SimHashes.Creature)
	{
		DlcRestrictionsUtil.TemporaryHelperObject transientHelperObjectFromAllowList = DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIDs);
		return KeepsakeConfig.CreateKeepsake(id, name, desc, animFile, initial_anim, ui_anim, transientHelperObjectFromAllowList.requiredDlcIds, transientHelperObjectFromAllowList.forbiddenDlcIds, postInitFn, element);
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x02000487 RID: 1159
	// (Invoke) Token: 0x060013BD RID: 5053
	public delegate void PostInitFn(GameObject gameObject);
}
