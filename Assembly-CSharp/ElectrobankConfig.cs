using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class ElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600136F RID: 4975 RVA: 0x000AA12F File Offset: 0x000A832F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x001988A4 File Offset: 0x00196AA4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Electrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.DESC, 20f, true, Assets.GetAnim("electrobank_large_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Katairite, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddComponent<Electrobank>().rechargeable = true;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D5E RID: 3422
	public const string ID = "Electrobank";

	// Token: 0x04000D5F RID: 3423
	public const float MASS = 20f;

	// Token: 0x04000D60 RID: 3424
	public const float POWER_CAPACITY = 120000f;

	// Token: 0x04000D61 RID: 3425
	public static ComplexRecipe recipe;
}
