using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class PinkRockCarvedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600142B RID: 5163 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x0019B17C File Offset: 0x0019937C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PinkRockCarved", STRINGS.CREATURES.SPECIES.PINKROCKCARVED.NAME, STRINGS.CREATURES.SPECIES.PINKROCKCARVED.DESC, 1f, true, Assets.GetAnim("pinkrock_decor_kanim"), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.RareMaterials,
			GameTags.MiscPickupable,
			GameTags.PedestalDisplayable,
			GameTags.Experimental
		});
		gameObject.AddOrGet<OccupyArea>();
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(TUNING.BUILDINGS.DECOR.BONUS.TIER1);
		decorProvider.overrideName = gameObject.GetProperName();
		Light2D light2D = gameObject.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.PINKROCK_COLOR;
		light2D.Color = LIGHT2D.PINKROCK_COLOR;
		light2D.Range = 3f;
		light2D.Angle = 0f;
		light2D.Direction = LIGHT2D.PINKROCK_DIRECTION;
		light2D.Offset = LIGHT2D.PINKROCK_OFFSET;
		light2D.shape = global::LightShape.Circle;
		light2D.drawOverlay = true;
		light2D.disableOnStore = true;
		gameObject.GetComponent<KCircleCollider2D>().offset = new Vector2(0f, 0.25f);
		return gameObject;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DD2 RID: 3538
	public const string ID = "PinkRockCarved";
}
