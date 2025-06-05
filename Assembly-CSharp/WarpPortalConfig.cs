using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
public class WarpPortalConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001AEE RID: 6894 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x001B5898 File Offset: 0x001B3A98
	public GameObject CreatePrefab()
	{
		string id = "WarpPortal";
		string name = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.WARPPORTAL.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("warp_portal_sender_kanim"), "idle", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddTag(GameTags.NotRoomAssignable);
		gameObject.AddTag(GameTags.WarpTech);
		gameObject.AddTag(GameTags.Gravitas);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<WarpPortal>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Ownable>().tintWhenUnassigned = false;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("notes_teleportation", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_SENDER));
		gameObject.AddOrGet<Prioritizable>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
		return gameObject;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x001B5994 File Offset: 0x001B3B94
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<WarpPortal>().workLayer = Grid.SceneLayer.Building;
		inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		inst.GetComponent<Deconstructable>();
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04001151 RID: 4433
	public const string ID = "WarpPortal";
}
