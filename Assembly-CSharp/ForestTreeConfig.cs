using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class ForestTreeConfig : IEntityConfig
{
	// Token: 0x060009CC RID: 2508 RVA: 0x001714AC File Offset: 0x0016F6AC
	public GameObject CreatePrefab()
	{
		string id = "ForestTree";
		string name = STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
		float mass = 2f;
		EffectorValues tier = DECOR.BONUS.TIER1;
		KAnimFile anim = Assets.GetAnim("tree_kanim");
		string initialAnim = "idle_empty";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.Building;
		int width = 1;
		int height = 2;
		EffectorValues decor = tier;
		List<Tag> additionalTags = new List<Tag>();
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 298.15f);
		EntityTemplates.ExtendEntityToBasicPlant(gameObject, 258.15f, 288.15f, 313.15f, 448.15f, null, true, 0f, 0.15f, "WoodLog", true, true, true, false, 2400f, 0f, 9800f, "ForestTreeOriginal", STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
		PlantBranchGrower.Def def = gameObject.AddOrGetDef<PlantBranchGrower.Def>();
		def.preventStartSMIOnSpawn = true;
		def.onBranchSpawned = new Action<PlantBranch.Instance, PlantBranchGrower.Instance>(this.RollChancesForSeed);
		def.onBranchHarvested = new Action<PlantBranch.Instance, PlantBranchGrower.Instance>(this.RollChancesForSeed);
		def.onEarlySpawn = new Action<PlantBranchGrower.Instance>(this.TranslateOldBranchesToNewSystem);
		def.BRANCH_PREFAB_NAME = "ForestTreeBranch";
		def.harvestOnDrown = true;
		def.MAX_BRANCH_COUNT = 5;
		def.BRANCH_OFFSETS = new CellOffset[]
		{
			new CellOffset(-1, 0),
			new CellOffset(-1, 1),
			new CellOffset(-1, 2),
			new CellOffset(0, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		};
		gameObject.AddOrGet<BuddingTrunk>();
		gameObject.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
		gameObject.UpdateComponentRequirement(false);
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		EntityTemplates.ExtendPlantToIrrigated(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = tag,
				massConsumptionRate = 0.11666667f
			}
		});
		EntityTemplates.ExtendPlantToFertilizable(gameObject, new PlantElementAbsorber.ConsumeInfo[]
		{
			new PlantElementAbsorber.ConsumeInfo
			{
				tag = GameTags.Dirt,
				massConsumptionRate = 0.016666668f
			}
		});
		gameObject.AddComponent<StandardCropPlant>().wiltsOnReadyToHarvest = true;
		gameObject.AddComponent<ForestTreeSeedMonitor>();
		GameObject plant = gameObject;
		IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
		SeedProducer.ProductionType productionType = SeedProducer.ProductionType.Hidden;
		string id2 = "ForestTreeSeed";
		string name2 = STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME;
		string desc2 = STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC;
		KAnimFile anim2 = Assets.GetAnim("seed_tree_kanim");
		string initialAnim2 = "object";
		int numberOfSeeds = 1;
		List<Tag> list = new List<Tag>();
		list.Add(GameTags.CropSeed);
		SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top;
		string domesticatedDescription = STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC;
		EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, productionType, id2, name2, desc2, anim2, initialAnim2, numberOfSeeds, list, planterDirection, default(Tag), 4, domesticatedDescription, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, null, "", false), "ForestTree_preview", Assets.GetAnim("tree_kanim"), "place", 3, 3);
		return gameObject;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000AEC23 File Offset: 0x000ACE23
	public void RollChancesForSeed(PlantBranch.Instance branch_smi, PlantBranchGrower.Instance trunk_smi)
	{
		trunk_smi.GetComponent<ForestTreeSeedMonitor>().TryRollNewSeed();
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00171768 File Offset: 0x0016F968
	public void TranslateOldBranchesToNewSystem(PlantBranchGrower.Instance smi)
	{
		KPrefabID[] andForgetOldSerializedBranches = smi.GetComponent<BuddingTrunk>().GetAndForgetOldSerializedBranches();
		if (andForgetOldSerializedBranches != null)
		{
			smi.ManuallyDefineBranchArray(andForgetOldSerializedBranches);
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000780 RID: 1920
	public const string ID = "ForestTree";

	// Token: 0x04000781 RID: 1921
	public const string SEED_ID = "ForestTreeSeed";

	// Token: 0x04000782 RID: 1922
	public const float FERTILIZATION_RATE = 0.016666668f;

	// Token: 0x04000783 RID: 1923
	public const float WATER_RATE = 0.11666667f;

	// Token: 0x04000784 RID: 1924
	public const float BRANCH_GROWTH_TIME = 2100f;

	// Token: 0x04000785 RID: 1925
	public const int NUM_BRANCHES = 7;
}
