using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class EmptyElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001375 RID: 4981 RVA: 0x000AA12F File Offset: 0x000A832F
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00198970 File Offset: 0x00196B70
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EmptyElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.DESC, 20f, true, Assets.GetAnim("electrobank_large_depleted_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Katairite, new List<Tag>
		{
			GameTags.EmptyPortableBattery,
			GameTags.PedestalDisplayable
		});
		if (!Assets.IsTagCountable(GameTags.EmptyPortableBattery))
		{
			Assets.AddCountableTag(GameTags.EmptyPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000D62 RID: 3426
	public const string ID = "EmptyElectrobank";

	// Token: 0x04000D63 RID: 3427
	public const float MASS = 20f;
}
