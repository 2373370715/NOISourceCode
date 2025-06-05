using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class PioneerLanderConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060015F1 RID: 5617 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x001A147C File Offset: 0x0019F67C
	public GameObject CreatePrefab()
	{
		string id = "PioneerLander";
		string name = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PIONEERLANDER.DESC;
		float mass = 400f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.RoomProberBuilding
		}, 293f);
		gameObject.AddOrGetDef<CargoLander.Def>().previewTag = "PioneerLander_Preview".ToTag();
		CargoDropperMinion.Def def = gameObject.AddOrGetDef<CargoDropperMinion.Def>();
		def.kAnimName = "anim_interacts_pioneer_cargo_lander_kanim";
		def.animName = "enter";
		gameObject.AddOrGet<MinionStorage>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<Deconstructable>().audioSize = "large";
		gameObject.AddOrGet<Storable>();
		Placeable placeable = gameObject.AddOrGet<Placeable>();
		placeable.kAnimName = "rocket_pioneer_cargo_lander_kanim";
		placeable.animName = "place";
		placeable.placementRules = new List<Placeable.PlacementRules>
		{
			Placeable.PlacementRules.OnFoundation,
			Placeable.PlacementRules.VisibleToSpace,
			Placeable.PlacementRules.RestrictToWorld
		};
		placeable.checkRootCellOnly = true;
		EntityTemplates.CreateAndRegisterPreview("PioneerLander_Preview", Assets.GetAnim("rocket_pioneer_cargo_lander_kanim"), "place", ObjectLayer.Building, 3, 3);
		return gameObject;
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x000AB23D File Offset: 0x000A943D
	public void OnPrefabInit(GameObject inst)
	{
		OccupyArea component = inst.GetComponent<OccupyArea>();
		component.ApplyToCells = false;
		component.objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000F1B RID: 3867
	public const string ID = "PioneerLander";

	// Token: 0x04000F1C RID: 3868
	public const string PREVIEW_ID = "PioneerLander_Preview";

	// Token: 0x04000F1D RID: 3869
	public const float MASS = 400f;
}
