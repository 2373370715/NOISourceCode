using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000543 RID: 1347
public class PropSurfaceSatellite1Config : IEntityConfig
{
	// Token: 0x06001725 RID: 5925 RVA: 0x001A4A80 File Offset: 0x001A2C80
	public GameObject CreatePrefab()
	{
		string id = "PropSurfaceSatellite1";
		string name = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPSURFACESATELLITE1.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("satellite1_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.numDataBanks = new int[]
		{
			4,
			9
		};
		gameObject.AddOrGet<Demolishable>();
		LoreBearerUtil.AddLoreTo(gameObject);
		return gameObject;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x001A4B60 File Offset: 0x001A2D60
	public static string[][] GetLockerBaseContents()
	{
		return new string[][]
		{
			new string[]
			{
				DatabankHelper.ID,
				DatabankHelper.ID,
				DatabankHelper.ID
			},
			new string[]
			{
				"ColdBreatherSeed",
				"ColdBreatherSeed",
				"ColdBreatherSeed"
			},
			new string[]
			{
				"Atmo_Suit",
				"Glom",
				"Glom",
				"Glom"
			}
		};
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x001A4BE0 File Offset: 0x001A2DE0
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
		component.ChooseContents();
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		RadiationEmitter radiationEmitter = inst.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x001A4C58 File Offset: 0x001A2E58
	public void OnSpawn(GameObject inst)
	{
		RadiationEmitter component = inst.GetComponent<RadiationEmitter>();
		if (component != null)
		{
			component.SetEmitting(true);
		}
	}

	// Token: 0x04000F48 RID: 3912
	public const string ID = "PropSurfaceSatellite1";
}
