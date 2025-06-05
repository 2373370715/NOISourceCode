using System;
using FMODUnity;
using UnityEngine;

// Token: 0x020018FE RID: 6398
[AddComponentMenu("KMonoBehaviour/scripts/Sounds")]
public class Sounds : KMonoBehaviour
{
	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06008478 RID: 33912 RVA: 0x000FB907 File Offset: 0x000F9B07
	// (set) Token: 0x06008479 RID: 33913 RVA: 0x000FB90E File Offset: 0x000F9B0E
	public static Sounds Instance { get; private set; }

	// Token: 0x0600847A RID: 33914 RVA: 0x000FB916 File Offset: 0x000F9B16
	public static void DestroyInstance()
	{
		Sounds.Instance = null;
	}

	// Token: 0x0600847B RID: 33915 RVA: 0x000FB91E File Offset: 0x000F9B1E
	protected override void OnPrefabInit()
	{
		Sounds.Instance = this;
	}

	// Token: 0x040064C9 RID: 25801
	public FMODAsset BlowUp_Generic;

	// Token: 0x040064CA RID: 25802
	public FMODAsset Build_Generic;

	// Token: 0x040064CB RID: 25803
	public FMODAsset InUse_Fabricator;

	// Token: 0x040064CC RID: 25804
	public FMODAsset InUse_OxygenGenerator;

	// Token: 0x040064CD RID: 25805
	public FMODAsset Place_OreOnSite;

	// Token: 0x040064CE RID: 25806
	public FMODAsset Footstep_rock;

	// Token: 0x040064CF RID: 25807
	public FMODAsset Ice_crack;

	// Token: 0x040064D0 RID: 25808
	public FMODAsset BuildingPowerOn;

	// Token: 0x040064D1 RID: 25809
	public FMODAsset ElectricGridOverload;

	// Token: 0x040064D2 RID: 25810
	public FMODAsset IngameMusic;

	// Token: 0x040064D3 RID: 25811
	public FMODAsset[] OreSplashSounds;

	// Token: 0x040064D5 RID: 25813
	public EventReference BlowUp_GenericMigrated;

	// Token: 0x040064D6 RID: 25814
	public EventReference Build_GenericMigrated;

	// Token: 0x040064D7 RID: 25815
	public EventReference InUse_FabricatorMigrated;

	// Token: 0x040064D8 RID: 25816
	public EventReference InUse_OxygenGeneratorMigrated;

	// Token: 0x040064D9 RID: 25817
	public EventReference Place_OreOnSiteMigrated;

	// Token: 0x040064DA RID: 25818
	public EventReference Footstep_rockMigrated;

	// Token: 0x040064DB RID: 25819
	public EventReference Ice_crackMigrated;

	// Token: 0x040064DC RID: 25820
	public EventReference BuildingPowerOnMigrated;

	// Token: 0x040064DD RID: 25821
	public EventReference ElectricGridOverloadMigrated;

	// Token: 0x040064DE RID: 25822
	public EventReference IngameMusicMigrated;

	// Token: 0x040064DF RID: 25823
	public EventReference[] OreSplashSoundsMigrated;
}
