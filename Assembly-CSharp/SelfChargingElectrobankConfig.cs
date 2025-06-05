using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class SelfChargingElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001442 RID: 5186 RVA: 0x000B33BA File Offset: 0x000B15BA
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1.Append(DlcManager.DLC3);
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0019B3B4 File Offset: 0x001995B4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("SelfChargingElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.DESC, 10f, true, Assets.GetAnim("electrobank_large_uranium_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.EnrichedUranium, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 5;
		radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
		radiationEmitter.emitRads = 120f;
		radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		SelfChargingElectrobank selfChargingElectrobank = gameObject.AddComponent<SelfChargingElectrobank>();
		selfChargingElectrobank.rechargeable = false;
		selfChargingElectrobank.keepEmpty = true;
		selfChargingElectrobank.radioactivityTuning = radiationEmitter.emitRads;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DD6 RID: 3542
	public const string ID = "SelfChargingElectrobank";

	// Token: 0x04000DD7 RID: 3543
	public const float MASS = 10f;

	// Token: 0x04000DD8 RID: 3544
	public const float POWER_DURATION = 90000f;

	// Token: 0x04000DD9 RID: 3545
	public const float SELF_CHARGE_WATTAGE = 60f;

	// Token: 0x04000DDA RID: 3546
	public static ComplexRecipe recipe;
}
