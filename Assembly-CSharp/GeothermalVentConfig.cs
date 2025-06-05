using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class GeothermalVentConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000DDE RID: 3550 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00180328 File Offset: 0x0017E528
	public virtual GameObject CreatePrefab()
	{
		string id = "GeothermalVentEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geospout_kanim"), "off", Grid.SceneLayer.BuildingBack, 3, 4, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<GeothermalVent>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<UserNameable>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showCapacityAsMainStatus = false;
		storage.showCapacityStatusItem = false;
		storage.showDescriptor = false;
		return gameObject;
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x001803FC File Offset: 0x0017E5FC
	public void OnPrefabInit(GameObject inst)
	{
		LogicPorts logicPorts = inst.AddOrGet<LogicPorts>();
		logicPorts.inputPortInfo = new LogicPorts.Port[0];
		logicPorts.outputPortInfo = new LogicPorts.Port[]
		{
			LogicPorts.Port.OutputPort("GEOTHERMAL_VENT_STATUS_PORT", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_INACTIVE, false, false)
		};
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A3C RID: 2620
	public const string ID = "GeothermalVentEntity";

	// Token: 0x04000A3D RID: 2621
	public const string OUTPUT_LOGIC_PORT_ID = "GEOTHERMAL_VENT_STATUS_PORT";

	// Token: 0x04000A3E RID: 2622
	private const string ANIM_FILE = "gravitas_geospout_kanim";

	// Token: 0x04000A3F RID: 2623
	public const string OFFLINE_ANIM = "off";

	// Token: 0x04000A40 RID: 2624
	public const string QUEST_ENTOMBED_ANIM = "pooped";

	// Token: 0x04000A41 RID: 2625
	public const string IDLE_ANIM = "on";

	// Token: 0x04000A42 RID: 2626
	public const string OBSTRUCTED_ANIM = "over_pressure";

	// Token: 0x04000A43 RID: 2627
	public const int EMISSION_RANGE = 1;

	// Token: 0x04000A44 RID: 2628
	public const float EMISSION_INTERVAL_SEC = 0.2f;

	// Token: 0x04000A45 RID: 2629
	public const float EMISSION_MAX_PRESSURE_KG = 120f;

	// Token: 0x04000A46 RID: 2630
	public const float EMISSION_MAX_RATE_PER_TICK = 3f;

	// Token: 0x04000A47 RID: 2631
	public static string TOGGLE_ANIMATION = "working_loop";

	// Token: 0x04000A48 RID: 2632
	public static HashedString TOGGLE_ANIM_OVERRIDE = "anim_interacts_geospout_kanim";

	// Token: 0x04000A49 RID: 2633
	public const float TOGGLE_CHORE_DURATION_SECONDS = 10f;

	// Token: 0x04000A4A RID: 2634
	public static MathUtil.MinMax INITIAL_DEBRIS_VELOCIOTY = new MathUtil.MinMax(1f, 5f);

	// Token: 0x04000A4B RID: 2635
	public static MathUtil.MinMax INITIAL_DEBRIS_ANGLE = new MathUtil.MinMax(200f, 340f);

	// Token: 0x04000A4C RID: 2636
	public static MathUtil.MinMax DEBRIS_MASS_KG = new MathUtil.MinMax(30f, 34f);

	// Token: 0x04000A4D RID: 2637
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x04000A4E RID: 2638
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x04000A4F RID: 2639
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000A50 RID: 2640
	public const string CONNECTED_ANIM = "meter_connected";

	// Token: 0x04000A51 RID: 2641
	public const string CONNECTED_TARGET = "meter_connected_target";

	// Token: 0x04000A52 RID: 2642
	public static string[] CONNECTED_SYMBOLS = new string[]
	{
		"meter_connected_target"
	};

	// Token: 0x04000A53 RID: 2643
	public const float CONNECTED_PROGRESS = 1f;

	// Token: 0x04000A54 RID: 2644
	public const float DISCONNECTED_PROGRESS = 0f;
}
