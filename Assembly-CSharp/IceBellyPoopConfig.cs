using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class IceBellyPoopConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013B0 RID: 5040 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x00199D3C File Offset: 0x00197F3C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IceBellyPoop", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.DESC, 100f, false, Assets.GetAnim("bammoth_poop_kanim"), "idle3", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>().offset = new Vector2(0f, 0.05f);
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.PENALTY.TIER3);
		decorProvider.overrideName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME;
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D91 RID: 3473
	public const string ID = "IceBellyPoop";
}
