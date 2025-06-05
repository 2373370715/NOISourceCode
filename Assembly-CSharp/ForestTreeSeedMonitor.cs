using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200029C RID: 668
public class ForestTreeSeedMonitor : KMonoBehaviour
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x000AEC30 File Offset: 0x000ACE30
	public bool ExtraSeedAvailable
	{
		get
		{
			return this.hasExtraSeedAvailable;
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0017178C File Offset: 0x0016F98C
	public void ExtractExtraSeed()
	{
		if (!this.hasExtraSeedAvailable)
		{
			return;
		}
		this.hasExtraSeedAvailable = false;
		Vector3 position = base.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(true);
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000AEC38 File Offset: 0x000ACE38
	public void TryRollNewSeed()
	{
		if (!this.hasExtraSeedAvailable && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.hasExtraSeedAvailable = true;
		}
	}

	// Token: 0x04000786 RID: 1926
	[Serialize]
	private bool hasExtraSeedAvailable;
}
