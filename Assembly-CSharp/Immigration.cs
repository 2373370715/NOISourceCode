using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200144A RID: 5194
[AddComponentMenu("KMonoBehaviour/scripts/Immigration")]
public class Immigration : KMonoBehaviour, ISaveLoadable, ISim200ms, IPersonalPriorityManager
{
	// Token: 0x06006A7B RID: 27259 RVA: 0x000EA3B4 File Offset: 0x000E85B4
	public static void DestroyInstance()
	{
		Immigration.Instance = null;
	}

	// Token: 0x06006A7C RID: 27260 RVA: 0x002EC31C File Offset: 0x002EA51C
	protected override void OnPrefabInit()
	{
		this.bImmigrantAvailable = false;
		Immigration.Instance = this;
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		this.timeBeforeSpawn = this.spawnInterval[num];
		this.SetupDLCCarePackages();
		this.ResetPersonalPriorities();
		this.ConfigureCarePackages();
	}

	// Token: 0x06006A7D RID: 27261 RVA: 0x002EC36C File Offset: 0x002EA56C
	private void SetupDLCCarePackages()
	{
		Dictionary<string, List<CarePackageInfo>> dictionary = new Dictionary<string, List<CarePackageInfo>>();
		string key = "DLC2_ID";
		List<CarePackageInfo> list = new List<CarePackageInfo>();
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cinnabar).tag.ToString(), 2000f, () => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cinnabar).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.WoodLog).tag.ToString(), 200f, () => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.WoodLog).tag)));
		list.Add(new CarePackageInfo("WoodDeerBaby", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("SealBaby", 1f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("IceBellyEgg", 1f, () => Immigration.CycleCondition(100)));
		list.Add(new CarePackageInfo("Pemmican", 3f, null));
		list.Add(new CarePackageInfo("FriesCarrot", 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("IceFlowerSeed", 3f, null));
		list.Add(new CarePackageInfo("BlueGrassSeed", 1f, null));
		list.Add(new CarePackageInfo("CarrotPlantSeed", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("SpaceTreeSeed", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("HardSkinBerryPlantSeed", 3f, null));
		dictionary.Add(key, list);
		string key2 = "DLC3_ID";
		List<CarePackageInfo> list2 = new List<CarePackageInfo>();
		list2.Add(new CarePackageInfo("DisposableElectrobank_RawMetal", 3f, () => Immigration.CycleCondition(12)));
		dictionary.Add(key2, list2);
		this.carePackagesByDlc = dictionary;
		foreach (KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> keyValuePair in BionicUpgradeComponentConfig.UpgradesData)
		{
			if (keyValuePair.Value.isCarePackage)
			{
				this.carePackagesByDlc["DLC3_ID"].Add(new CarePackageInfo(keyValuePair.Key.Name, 1f, () => Immigration.HasMinionModelCondition(BionicMinionConfig.MODEL)));
			}
		}
	}

	// Token: 0x06006A7E RID: 27262 RVA: 0x002EC694 File Offset: 0x002EA894
	private void ConfigureCarePackages()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.ConfigureMultiWorldCarePackages();
		}
		else
		{
			this.ConfigureBaseGameCarePackages();
		}
		foreach (string key in SaveLoader.Instance.GameInfo.dlcIds)
		{
			if (this.carePackagesByDlc.ContainsKey(key))
			{
				this.carePackages.AddRange(this.carePackagesByDlc[key]);
			}
		}
	}

	// Token: 0x06006A7F RID: 27263 RVA: 0x002EC724 File Offset: 0x002EA924
	private void ConfigureBaseGameCarePackages()
	{
		List<CarePackageInfo> list = new List<CarePackageInfo>();
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, () => Immigration.CycleCondition(12)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, () => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, () => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, () => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, () => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag)));
		list.Add(new CarePackageInfo("PrickleGrassSeed", 3f, null));
		list.Add(new CarePackageInfo("LeafyPlantSeed", 3f, null));
		list.Add(new CarePackageInfo("CactusPlantSeed", 3f, null));
		list.Add(new CarePackageInfo("MushroomSeed", 1f, null));
		list.Add(new CarePackageInfo("PrickleFlowerSeed", 2f, null));
		list.Add(new CarePackageInfo("OxyfernSeed", 1f, null));
		list.Add(new CarePackageInfo("ForestTreeSeed", 1f, null));
		list.Add(new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("SwampLilySeed", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("ColdBreatherSeed", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("SpiceVineSeed", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("FieldRation", 5f, null));
		list.Add(new CarePackageInfo("BasicForagePlant", 6f, null));
		list.Add(new CarePackageInfo("CookedEgg", 3f, () => Immigration.CycleCondition(6)));
		list.Add(new CarePackageInfo(PrickleFruitConfig.ID, 3f, () => Immigration.CycleCondition(12)));
		list.Add(new CarePackageInfo("FriedMushroom", 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("CookedMeat", 3f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("SpicyTofu", 3f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("LightBugBaby", 1f, null));
		list.Add(new CarePackageInfo("HatchBaby", 1f, null));
		list.Add(new CarePackageInfo("PuftBaby", 1f, null));
		list.Add(new CarePackageInfo("SquirrelBaby", 1f, null));
		list.Add(new CarePackageInfo("CrabBaby", 1f, null));
		list.Add(new CarePackageInfo("DreckoBaby", 1f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("Pacu", 8f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("MoleBaby", 1f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("OilfloaterBaby", 1f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("LightBugEgg", 3f, null));
		list.Add(new CarePackageInfo("HatchEgg", 3f, null));
		list.Add(new CarePackageInfo("PuftEgg", 3f, null));
		list.Add(new CarePackageInfo("OilfloaterEgg", 3f, () => Immigration.CycleCondition(12)));
		list.Add(new CarePackageInfo("MoleEgg", 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("DreckoEgg", 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("SquirrelEgg", 2f, null));
		list.Add(new CarePackageInfo("BasicCure", 3f, null));
		list.Add(new CarePackageInfo("CustomClothing", 1f, null, "SELECTRANDOM"));
		list.Add(new CarePackageInfo("Funky_Vest", 1f, null));
		this.carePackages = list;
	}

	// Token: 0x06006A80 RID: 27264 RVA: 0x002ED14C File Offset: 0x002EB34C
	private void ConfigureMultiWorldCarePackages()
	{
		List<CarePackageInfo> list = new List<CarePackageInfo>();
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, () => Immigration.CycleCondition(12)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, null));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, () => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, () => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, () => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, () => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag)));
		list.Add(new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, () => Immigration.CycleCondition(48) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag)));
		list.Add(new CarePackageInfo("PrickleGrassSeed", 3f, null));
		list.Add(new CarePackageInfo("LeafyPlantSeed", 3f, null));
		list.Add(new CarePackageInfo("CactusPlantSeed", 3f, null));
		list.Add(new CarePackageInfo("WineCupsSeed", 3f, null));
		list.Add(new CarePackageInfo("CylindricaSeed", 3f, null));
		list.Add(new CarePackageInfo("MushroomSeed", 1f, null));
		list.Add(new CarePackageInfo("PrickleFlowerSeed", 2f, () => Immigration.DiscoveredCondition("PrickleFlowerSeed") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("OxyfernSeed", 1f, null));
		list.Add(new CarePackageInfo("ForestTreeSeed", 1f, () => Immigration.DiscoveredCondition("ForestTreeSeed") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition(BasicFabricMaterialPlantConfig.SEED_ID) || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("SwampLilySeed", 1f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("SwampLilySeed") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("ColdBreatherSeed", 1f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("ColdBreatherSeed") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("SpiceVineSeed", 1f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("SpiceVineSeed") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("WormPlantSeed", 1f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("WormPlantSeed") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("FieldRation", 5f, null));
		list.Add(new CarePackageInfo("BasicForagePlant", 6f, () => Immigration.DiscoveredCondition("BasicForagePlant")));
		list.Add(new CarePackageInfo("ForestForagePlant", 2f, () => Immigration.DiscoveredCondition("ForestForagePlant")));
		list.Add(new CarePackageInfo("SwampForagePlant", 2f, () => Immigration.DiscoveredCondition("SwampForagePlant")));
		list.Add(new CarePackageInfo("CookedEgg", 3f, () => Immigration.CycleCondition(6)));
		list.Add(new CarePackageInfo(PrickleFruitConfig.ID, 3f, () => Immigration.CycleCondition(12) && (Immigration.DiscoveredCondition(PrickleFruitConfig.ID) || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("FriedMushroom", 3f, () => Immigration.CycleCondition(24)));
		list.Add(new CarePackageInfo("CookedMeat", 3f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("SpicyTofu", 3f, () => Immigration.CycleCondition(48)));
		list.Add(new CarePackageInfo("WormSuperFood", 2f, () => Immigration.DiscoveredCondition("WormPlantSeed") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("LightBugBaby", 1f, () => Immigration.DiscoveredCondition("LightBugEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("HatchBaby", 1f, () => Immigration.DiscoveredCondition("HatchEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("PuftBaby", 1f, () => Immigration.DiscoveredCondition("PuftEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("SquirrelBaby", 1f, () => Immigration.DiscoveredCondition("SquirrelEgg") || Immigration.CycleCondition(24) || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("CrabBaby", 1f, () => Immigration.DiscoveredCondition("CrabEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("DreckoBaby", 1f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("DreckoEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("Pacu", 8f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("PacuEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("MoleBaby", 1f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("MoleEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("OilfloaterBaby", 1f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("OilfloaterEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("DivergentBeetleBaby", 1f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("DivergentBeetleEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("StaterpillarBaby", 1f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("StaterpillarEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("LightBugEgg", 3f, () => Immigration.DiscoveredCondition("LightBugEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("HatchEgg", 3f, () => Immigration.DiscoveredCondition("HatchEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("PuftEgg", 3f, () => Immigration.DiscoveredCondition("PuftEgg") || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("OilfloaterEgg", 3f, () => Immigration.CycleCondition(12) && (Immigration.DiscoveredCondition("OilfloaterEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("MoleEgg", 3f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("MoleEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("DreckoEgg", 3f, () => Immigration.CycleCondition(24) && (Immigration.DiscoveredCondition("DreckoEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("SquirrelEgg", 2f, () => Immigration.DiscoveredCondition("SquirrelEgg") || Immigration.CycleCondition(24) || Immigration.CycleCondition(500)));
		list.Add(new CarePackageInfo("DivergentBeetleEgg", 2f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("DivergentBeetleEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("StaterpillarEgg", 2f, () => Immigration.CycleCondition(48) && (Immigration.DiscoveredCondition("StaterpillarEgg") || Immigration.CycleCondition(500))));
		list.Add(new CarePackageInfo("BasicCure", 3f, null));
		list.Add(new CarePackageInfo("CustomClothing", 1f, null, "SELECTRANDOM"));
		list.Add(new CarePackageInfo("Funky_Vest", 1f, null));
		this.carePackages = list;
	}

	// Token: 0x06006A81 RID: 27265 RVA: 0x000EA3BC File Offset: 0x000E85BC
	private static bool CycleCondition(int cycle)
	{
		return GameClock.Instance.GetCycle() >= cycle;
	}

	// Token: 0x06006A82 RID: 27266 RVA: 0x000EA3CE File Offset: 0x000E85CE
	private static bool DiscoveredCondition(Tag tag)
	{
		return DiscoveredResources.Instance.IsDiscovered(tag);
	}

	// Token: 0x06006A83 RID: 27267 RVA: 0x002EDEA8 File Offset: 0x002EC0A8
	private static bool HasMinionModelCondition(Tag model)
	{
		Components.Cmps<MinionIdentity> cmps;
		return Components.LiveMinionIdentitiesByModel.TryGetValue(model, out cmps) && cmps.Count > 0;
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06006A84 RID: 27268 RVA: 0x000EA3DB File Offset: 0x000E85DB
	public bool ImmigrantsAvailable
	{
		get
		{
			return this.bImmigrantAvailable;
		}
	}

	// Token: 0x06006A85 RID: 27269 RVA: 0x002EDED0 File Offset: 0x002EC0D0
	public int EndImmigration()
	{
		this.bImmigrantAvailable = false;
		this.spawnIdx++;
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		this.timeBeforeSpawn = this.spawnInterval[num];
		return this.spawnTable[num];
	}

	// Token: 0x06006A86 RID: 27270 RVA: 0x000EA3E3 File Offset: 0x000E85E3
	public float GetTimeRemaining()
	{
		return this.timeBeforeSpawn;
	}

	// Token: 0x06006A87 RID: 27271 RVA: 0x002EDF20 File Offset: 0x002EC120
	public float GetTotalWaitTime()
	{
		int num = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
		return this.spawnInterval[num];
	}

	// Token: 0x06006A88 RID: 27272 RVA: 0x002EDF4C File Offset: 0x002EC14C
	public void Sim200ms(float dt)
	{
		if (this.IsHalted() || this.bImmigrantAvailable)
		{
			return;
		}
		this.timeBeforeSpawn -= dt;
		this.timeBeforeSpawn = Math.Max(this.timeBeforeSpawn, 0f);
		if (this.timeBeforeSpawn <= 0f)
		{
			this.bImmigrantAvailable = true;
		}
	}

	// Token: 0x06006A89 RID: 27273 RVA: 0x002EDFA4 File Offset: 0x002EC1A4
	private bool IsHalted()
	{
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			Operational component = telepad.GetComponent<Operational>();
			if (component != null && component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006A8A RID: 27274 RVA: 0x002EE014 File Offset: 0x002EC214
	public int GetPersonalPriority(ChoreGroup group)
	{
		int result;
		if (!this.defaultPersonalPriorities.TryGetValue(group.IdHash, out result))
		{
			result = 3;
		}
		return result;
	}

	// Token: 0x06006A8B RID: 27275 RVA: 0x002EE03C File Offset: 0x002EC23C
	public CarePackageInfo RandomCarePackage()
	{
		List<CarePackageInfo> list = new List<CarePackageInfo>();
		foreach (CarePackageInfo carePackageInfo in this.carePackages)
		{
			if (carePackageInfo.requirement == null || carePackageInfo.requirement())
			{
				list.Add(carePackageInfo);
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06006A8C RID: 27276 RVA: 0x000EA3EB File Offset: 0x000E85EB
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		this.defaultPersonalPriorities[group.IdHash] = value;
	}

	// Token: 0x06006A8D RID: 27277 RVA: 0x000B1628 File Offset: 0x000AF828
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x06006A8E RID: 27278 RVA: 0x002EE0BC File Offset: 0x002EC2BC
	public void ApplyDefaultPersonalPriorities(GameObject minion)
	{
		IPersonalPriorityManager instance = Immigration.Instance;
		IPersonalPriorityManager component = minion.GetComponent<ChoreConsumer>();
		foreach (ChoreGroup group in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = instance.GetPersonalPriority(group);
			component.SetPersonalPriority(group, personalPriority);
		}
	}

	// Token: 0x06006A8F RID: 27279 RVA: 0x002EE130 File Offset: 0x002EC330
	public void ResetPersonalPriorities()
	{
		bool advancedPersonalPriorities = Game.Instance.advancedPersonalPriorities;
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			this.defaultPersonalPriorities[choreGroup.IdHash] = (advancedPersonalPriorities ? choreGroup.DefaultPersonalPriority : 3);
		}
	}

	// Token: 0x06006A90 RID: 27280 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsChoreGroupDisabled(ChoreGroup g)
	{
		return false;
	}

	// Token: 0x040050C6 RID: 20678
	public float[] spawnInterval;

	// Token: 0x040050C7 RID: 20679
	public int[] spawnTable;

	// Token: 0x040050C8 RID: 20680
	[Serialize]
	private Dictionary<HashedString, int> defaultPersonalPriorities = new Dictionary<HashedString, int>();

	// Token: 0x040050C9 RID: 20681
	[Serialize]
	public float timeBeforeSpawn = float.PositiveInfinity;

	// Token: 0x040050CA RID: 20682
	[Serialize]
	private bool bImmigrantAvailable;

	// Token: 0x040050CB RID: 20683
	[Serialize]
	private int spawnIdx;

	// Token: 0x040050CC RID: 20684
	private List<CarePackageInfo> carePackages;

	// Token: 0x040050CD RID: 20685
	private Dictionary<string, List<CarePackageInfo>> carePackagesByDlc;

	// Token: 0x040050CE RID: 20686
	public static Immigration Instance;

	// Token: 0x040050CF RID: 20687
	private const int CYCLE_THRESHOLD_A = 6;

	// Token: 0x040050D0 RID: 20688
	private const int CYCLE_THRESHOLD_B = 12;

	// Token: 0x040050D1 RID: 20689
	private const int CYCLE_THRESHOLD_C = 24;

	// Token: 0x040050D2 RID: 20690
	private const int CYCLE_THRESHOLD_D = 48;

	// Token: 0x040050D3 RID: 20691
	private const int CYCLE_THRESHOLD_E = 100;

	// Token: 0x040050D4 RID: 20692
	private const int CYCLE_THRESHOLD_UNLOCK_EVERYTHING = 500;

	// Token: 0x040050D5 RID: 20693
	public const string FACADE_SELECT_RANDOM = "SELECTRANDOM";
}
