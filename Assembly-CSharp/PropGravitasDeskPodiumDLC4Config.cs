﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class PropGravitasDeskPodiumDLC4Config : IEntityConfig, IHasDlcRestrictions
{
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodiumDLC4";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_podium_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new string[]
		{
			"dlc4surfacepoi"
		});
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public static string ID = "PropGravitasDeskPodiumDLC4";
}
