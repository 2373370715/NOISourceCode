﻿using System;
using UnityEngine;

public class MinionAssignablesProxyConfig : IEntityConfig
{
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<Ownables>();
		gameObject.AddOrGet<Equipment>();
		gameObject.AddOrGet<MinionAssignablesProxy>();
		return gameObject;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public static string ID = "MinionAssignablesProxy";
}
