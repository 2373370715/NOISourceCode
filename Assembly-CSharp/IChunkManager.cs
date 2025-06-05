using System;
using UnityEngine;

// Token: 0x02000B60 RID: 2912
public interface IChunkManager
{
	// Token: 0x060036CB RID: 14027
	SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);

	// Token: 0x060036CC RID: 14028
	SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);
}
