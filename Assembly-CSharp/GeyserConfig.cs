using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class GeyserConfig : IEntityConfig
{
	// Token: 0x060009DA RID: 2522 RVA: 0x00171A70 File Offset: 0x0016FC70
	public GameObject CreatePrefab()
	{
		string id = "Geyser";
		string name = STRINGS.CREATURES.SPECIES.GEYSER.NAME;
		string desc = STRINGS.CREATURES.SPECIES.GEYSER.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("geyser_side_steam_kanim"), "inactive", Grid.SceneLayer.BuildingBack, 4, 2, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IgneousRock, true);
		component.Temperature = 372.15f;
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<UserNameable>();
		GeyserConfigurator geyserConfigurator = gameObject.AddOrGet<GeyserConfigurator>();
		geyserConfigurator.presetType = "steam";
		geyserConfigurator.presetMin = 0.5f;
		geyserConfigurator.presetMax = 0.75f;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400078B RID: 1931
	public const int GEOTUNERS_REQUIRED_FOR_MAJOR_TRACKER_ANIMATION = 5;

	// Token: 0x0200029F RID: 671
	public enum TrackerMeterAnimNames
	{
		// Token: 0x0400078D RID: 1933
		tracker,
		// Token: 0x0400078E RID: 1934
		geotracker,
		// Token: 0x0400078F RID: 1935
		geotracker_minor,
		// Token: 0x04000790 RID: 1936
		geotracker_major
	}
}
