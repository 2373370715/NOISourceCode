using System;
using KSerialization;
using UnityEngine;

// Token: 0x020016ED RID: 5869
public class PedestalArtifactSpawner : KMonoBehaviour
{
	// Token: 0x0600790D RID: 30989 RVA: 0x00321E68 File Offset: 0x00320068
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject gameObject in this.storage.items)
		{
			if (ArtifactSelector.Instance.GetArtifactType(gameObject.name) == ArtifactType.Terrestrial)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
			}
		}
		if (this.artifactSpawned)
		{
			return;
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Terrestrial)), base.transform.position);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		this.storage.Store(gameObject2, false, false, true, false);
		this.receptacle.ForceDeposit(gameObject2);
		this.artifactSpawned = true;
	}

	// Token: 0x04005ADC RID: 23260
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005ADD RID: 23261
	[MyCmpReq]
	private SingleEntityReceptacle receptacle;

	// Token: 0x04005ADE RID: 23262
	[Serialize]
	private bool artifactSpawned;
}
