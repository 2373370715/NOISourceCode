using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000529 RID: 1321
public class PropGravitasCeilingRobotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060016A9 RID: 5801 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x001A398C File Offset: 0x001A1B8C
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasCeilingRobot";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGROBOT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_ceiling_robot_kanim"), "off", Grid.SceneLayer.Building, 2, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
