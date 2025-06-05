using System;
using System.Collections.Generic;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x02000C57 RID: 3159
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactFinder")]
public class ArtifactFinder : KMonoBehaviour
{
	// Token: 0x06003BA9 RID: 15273 RVA: 0x000CAFC6 File Offset: 0x000C91C6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ArtifactFinder>(-887025858, ArtifactFinder.OnLandDelegate);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00239590 File Offset: 0x00237790
	public ArtifactTier GetArtifactDropTier(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactDropRate artifactDropTable = destination.GetDestinationType().artifactDropTable;
		bool flag = minionID.traitIDs.Contains("Archaeologist");
		if (artifactDropTable != null)
		{
			float num = artifactDropTable.totalWeight;
			if (flag)
			{
				num -= artifactDropTable.GetTierWeight(DECOR.SPACEARTIFACT.TIER_NONE);
			}
			float num2 = UnityEngine.Random.value * num;
			foreach (global::Tuple<ArtifactTier, float> tuple in artifactDropTable.rates)
			{
				if (!flag || (flag && tuple.first != DECOR.SPACEARTIFACT.TIER_NONE))
				{
					num2 -= tuple.second;
				}
				if (num2 <= 0f)
				{
					return tuple.first;
				}
			}
		}
		return DECOR.SPACEARTIFACT.TIER0;
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x0023965C File Offset: 0x0023785C
	public List<string> GetArtifactsOfTier(ArtifactTier tier)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<ArtifactType, List<string>> keyValuePair in ArtifactConfig.artifactItems)
		{
			foreach (string text in keyValuePair.Value)
			{
				GameObject prefab = Assets.GetPrefab(text.ToTag());
				ArtifactTier artifactTier = prefab.GetComponent<SpaceArtifact>().GetArtifactTier();
				if (Game.IsCorrectDlcActiveForCurrentSave(prefab.GetComponent<KPrefabID>()) && artifactTier == tier)
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00239720 File Offset: 0x00237920
	public string SearchForArtifact(StoredMinionIdentity minionID, SpaceDestination destination)
	{
		ArtifactTier artifactDropTier = this.GetArtifactDropTier(minionID, destination);
		if (artifactDropTier == DECOR.SPACEARTIFACT.TIER_NONE)
		{
			return null;
		}
		List<string> artifactsOfTier = this.GetArtifactsOfTier(artifactDropTier);
		return artifactsOfTier[UnityEngine.Random.Range(0, artifactsOfTier.Count)];
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0023975C File Offset: 0x0023795C
	public void OnLand(object data)
	{
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>()));
		foreach (MinionStorage.Info info in this.minionStorage.GetStoredMinionInfo())
		{
			StoredMinionIdentity minionID = info.serializedMinion.Get<StoredMinionIdentity>();
			string text = this.SearchForArtifact(minionID, spacecraftDestination);
			if (text != null)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(text.ToTag()), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
			}
		}
	}

	// Token: 0x04002961 RID: 10593
	public const string ID = "ArtifactFinder";

	// Token: 0x04002962 RID: 10594
	[MyCmpReq]
	private MinionStorage minionStorage;

	// Token: 0x04002963 RID: 10595
	private static readonly EventSystem.IntraObjectHandler<ArtifactFinder> OnLandDelegate = new EventSystem.IntraObjectHandler<ArtifactFinder>(delegate(ArtifactFinder component, object data)
	{
		component.OnLand(data);
	});
}
