using System;
using UnityEngine;

// Token: 0x02000A92 RID: 2706
[AddComponentMenu("KMonoBehaviour/scripts/GasSourceManager")]
public class GasSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x06003149 RID: 12617 RVA: 0x000C47A5 File Offset: 0x000C29A5
	protected override void OnPrefabInit()
	{
		GasSourceManager.Instance = this;
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x000C47AD File Offset: 0x000C29AD
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x000C47C3 File Offset: 0x000C29C3
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x040021E6 RID: 8678
	public static GasSourceManager Instance;
}
