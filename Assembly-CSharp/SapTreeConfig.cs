using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class SapTreeConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x00173F74 File Offset: 0x00172174
	public GameObject CreatePrefab()
	{
		string id = "SapTree";
		string name = STRINGS.CREATURES.SPECIES.SAPTREE.NAME;
		string desc = STRINGS.CREATURES.SPECIES.SAPTREE.DESC;
		float mass = 1f;
		EffectorValues positive_DECOR_EFFECT = SapTreeConfig.POSITIVE_DECOR_EFFECT;
		KAnimFile anim = Assets.GetAnim("gravitas_sap_tree_kanim");
		string initialAnim = "idle";
		Grid.SceneLayer sceneLayer = Grid.SceneLayer.BuildingFront;
		int width = 5;
		int height = 5;
		EffectorValues decor = positive_DECOR_EFFECT;
		List<Tag> additionalTags = new List<Tag>
		{
			GameTags.Decoration
		};
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, default(EffectorValues), SimHashes.Creature, additionalTags, 293f);
		SapTree.Def def = gameObject.AddOrGetDef<SapTree.Def>();
		def.foodSenseArea = new Vector2I(5, 1);
		def.massEatRate = 0.05f;
		def.kcalorieToKGConversionRatio = 0.005f;
		def.stomachSize = 5f;
		def.oozeRate = 2f;
		def.oozeOffsets = new List<Vector3>
		{
			new Vector3(-2f, 2f),
			new Vector3(2f, 1f)
		};
		def.attackSenseArea = new Vector2I(5, 5);
		def.attackCooldown = 5f;
		gameObject.AddOrGet<Storage>();
		FactionAlignment factionAlignment = gameObject.AddOrGet<FactionAlignment>();
		factionAlignment.Alignment = FactionManager.FactionID.Hostile;
		factionAlignment.canBePlayerTargeted = false;
		gameObject.AddOrGet<RangedAttackable>();
		gameObject.AddWeapon(5f, 5f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.AreaOfEffect, 1, 2f);
		gameObject.AddOrGet<WiltCondition>();
		gameObject.AddOrGet<TemperatureVulnerable>().Configure(173.15f, 0f, 373.15f, 1023.15f);
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		return gameObject;
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400080B RID: 2059
	public const string ID = "SapTree";

	// Token: 0x0400080C RID: 2060
	public static readonly EffectorValues POSITIVE_DECOR_EFFECT = DECOR.BONUS.TIER5;

	// Token: 0x0400080D RID: 2061
	private const int WIDTH = 5;

	// Token: 0x0400080E RID: 2062
	private const int HEIGHT = 5;

	// Token: 0x0400080F RID: 2063
	private const int ATTACK_RADIUS = 2;

	// Token: 0x04000810 RID: 2064
	public const float MASS_EAT_RATE = 0.05f;

	// Token: 0x04000811 RID: 2065
	public const float KCAL_TO_KG_RATIO = 0.005f;
}
