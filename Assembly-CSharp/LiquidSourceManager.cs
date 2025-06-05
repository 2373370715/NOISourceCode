using System;
using UnityEngine;

// Token: 0x02000AAD RID: 2733
[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x060031E0 RID: 12768 RVA: 0x000C4E08 File Offset: 0x000C3008
	protected override void OnPrefabInit()
	{
		LiquidSourceManager.Instance = this;
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x000C4E10 File Offset: 0x000C3010
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x000C47C3 File Offset: 0x000C29C3
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04002222 RID: 8738
	public static LiquidSourceManager Instance;
}
