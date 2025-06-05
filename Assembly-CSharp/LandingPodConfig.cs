using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B4 RID: 948
public class LandingPodConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000F5A RID: 3930 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x001869B4 File Offset: 0x00184BB4
	public GameObject CreatePrefab()
	{
		string id = "LandingPod";
		string name = STRINGS.BUILDINGS.PREFABS.LANDING_POD.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.LANDING_POD.DESC;
		float mass = 2000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("rocket_puft_pod_kanim"), "grounded", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<PodLander>();
		gameObject.AddOrGet<MinionStorage>();
		return gameObject;
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x000AA768 File Offset: 0x000A8968
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B35 RID: 2869
	public const string ID = "LandingPod";
}
