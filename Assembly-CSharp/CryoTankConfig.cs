using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class CryoTankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001875 RID: 6261 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x001AB49C File Offset: 0x001A969C
	public GameObject CreatePrefab()
	{
		string id = "CryoTank";
		string name = STRINGS.BUILDINGS.PREFABS.CRYOTANK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.CRYOTANK.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("cryo_chamber_kanim"), "off", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KAnimControllerBase>().SetFGLayer(Grid.SceneLayer.BuildingFront);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		CryoTank cryoTank = gameObject.AddOrGet<CryoTank>();
		cryoTank.overrideAnim = "anim_interacts_cryo_activation_kanim";
		cryoTank.dropOffset = new CellOffset(1, 0);
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntry("cryotank_warning", UI.USERMENUACTIONS.READLORE.SEARCH_CRYO_TANK));
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04001024 RID: 4132
	public const string ID = "CryoTank";
}
