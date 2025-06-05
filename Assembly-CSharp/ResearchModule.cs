using System;
using TUNING;
using UnityEngine;

// Token: 0x02000F93 RID: 3987
[AddComponentMenu("KMonoBehaviour/scripts/ResearchModule")]
public class ResearchModule : KMonoBehaviour
{
	// Token: 0x0600504A RID: 20554 RVA: 0x0027D03C File Offset: 0x0027B23C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<ResearchModule>(-1277991738, ResearchModule.OnLaunchDelegate);
		base.Subscribe<ResearchModule>(-887025858, ResearchModule.OnLandDelegate);
	}

	// Token: 0x0600504B RID: 20555 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnLaunch(object data)
	{
	}

	// Token: 0x0600504C RID: 20556 RVA: 0x0027D094 File Offset: 0x0027B294
	public void OnLand(object data)
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			SpaceDestination.ResearchOpportunity researchOpportunity = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>())).TryCompleteResearchOpportunity();
			if (researchOpportunity != null)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.GetComponent<PrimaryElement>().Mass = (float)researchOpportunity.dataValue;
				if (!string.IsNullOrEmpty(researchOpportunity.discoveredRareItem))
				{
					GameObject prefab = Assets.GetPrefab(researchOpportunity.discoveredRareItem);
					if (prefab == null)
					{
						KCrashReporter.Assert(false, "Missing prefab: " + researchOpportunity.discoveredRareItem, null);
					}
					else
					{
						GameUtil.KInstantiate(prefab, base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
					}
				}
			}
		}
		GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<PrimaryElement>().Mass = (float)ROCKETRY.DESTINATION_RESEARCH.EVERGREEN;
	}

	// Token: 0x04003891 RID: 14481
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003892 RID: 14482
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLandDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLand(data);
	});
}
