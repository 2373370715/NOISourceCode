﻿using System;
using FMODUnity;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	public static Sounds Instance { get; private set; }

	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	public FMODAsset BlowUp_Generic;

	public FMODAsset Build_Generic;

	public FMODAsset InUse_Fabricator;

	public FMODAsset InUse_OxygenGenerator;

	public FMODAsset Place_OreOnSite;

	public FMODAsset Footstep_rock;

	public FMODAsset Ice_crack;

	public FMODAsset BuildingPowerOn;

	public FMODAsset ElectricGridOverload;

	public FMODAsset IngameMusic;

	public FMODAsset[] OreSplashSounds;

	public EventReference BlowUp_GenericMigrated;

	public EventReference Build_GenericMigrated;

	public EventReference InUse_FabricatorMigrated;

	public EventReference InUse_OxygenGeneratorMigrated;

	public EventReference Place_OreOnSiteMigrated;

	public EventReference Footstep_rockMigrated;

	public EventReference Ice_crackMigrated;

	public EventReference BuildingPowerOnMigrated;

	public EventReference ElectricGridOverloadMigrated;

	public EventReference IngameMusicMigrated;

	public EventReference[] OreSplashSoundsMigrated;
}
