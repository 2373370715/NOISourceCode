using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
public class StickerBombConfig : IEntityConfig
{
	// Token: 0x06001A0B RID: 6667 RVA: 0x001B1510 File Offset: 0x001AF710
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("StickerBomb", STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim("sticker_a_kanim"), "off", Grid.SceneLayer.Backwall, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
		gameObject.AddOrGet<StickerBomb>();
		return gameObject;
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000B5829 File Offset: 0x000B3A29
	public void OnPrefabInit(GameObject inst)
	{
		inst.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		inst.AddComponent<Modifiers>();
		inst.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER2);
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040010E2 RID: 4322
	public const string ID = "StickerBomb";
}
