using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class DisposableElectrobankConfig : IMultiEntityConfig
{
	// Token: 0x06001362 RID: 4962 RVA: 0x0019849C File Offset: 0x0019669C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		if (!DlcManager.IsContentSubscribed("DLC3_ID"))
		{
			return list;
		}
		list.Add(this.CreateDisposableElectrobank("DisposableElectrobank_RawMetal", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_METAL_ORE.DESC, 20f, SimHashes.Cuprite, "electrobank_popcan_kanim", DlcManager.DLC3, null, "object"));
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = this.CreateDisposableElectrobank("DisposableElectrobank_UraniumOre", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_URANIUM_ORE.DESC, 10f, SimHashes.UraniumOre, "electrobank_uranium_kanim", DlcManager.EXPANSION1.Append(DlcManager.DLC3), null, "object");
			RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
			radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			radiationEmitter.radiusProportionalToRads = false;
			radiationEmitter.emitRadiusX = 5;
			radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
			radiationEmitter.emitRads = 60f;
			radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
			list.Add(gameObject);
			gameObject.GetComponent<Electrobank>().radioactivityTuning = radiationEmitter.emitRads;
		}
		list.RemoveAll((GameObject t) => t == null);
		return list;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x001985C4 File Offset: 0x001967C4
	private GameObject CreateDisposableElectrobank(string id, LocString name, LocString description, float mass, SimHashes element, string animName, string[] requiredDlcIDs = null, string[] forbiddenDlcIds = null, string initialAnim = "object")
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, description, mass, true, Assets.GetAnim(animName), initialAnim, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable,
			GameTags.DisposablePortableBattery
		});
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddComponent<Electrobank>();
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.requiredDlcIds = requiredDlcIDs;
		component.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D57 RID: 3415
	public const string ID = "DisposableElectrobank_";

	// Token: 0x04000D58 RID: 3416
	public const float MASS = 20f;

	// Token: 0x04000D59 RID: 3417
	public static Dictionary<Tag, ComplexRecipe> recipes = new Dictionary<Tag, ComplexRecipe>();

	// Token: 0x04000D5A RID: 3418
	public const string ID_METAL_ORE = "DisposableElectrobank_RawMetal";

	// Token: 0x04000D5B RID: 3419
	public const string ID_URANIUM_ORE = "DisposableElectrobank_UraniumOre";
}
