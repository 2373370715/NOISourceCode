using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017DE RID: 6110
public static class RadiationGridManager
{
	// Token: 0x06007D95 RID: 32149 RVA: 0x000F7163 File Offset: 0x000F5363
	public static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06007D96 RID: 32150 RVA: 0x000F738B File Offset: 0x000F558B
	public static void Initialise()
	{
		RadiationGridManager.emitters = new List<RadiationGridEmitter>();
	}

	// Token: 0x06007D97 RID: 32151 RVA: 0x000F7397 File Offset: 0x000F5597
	public static void Shutdown()
	{
		RadiationGridManager.emitters.Clear();
	}

	// Token: 0x06007D98 RID: 32152 RVA: 0x00332B10 File Offset: 0x00330D10
	public static void Refresh()
	{
		for (int i = 0; i < RadiationGridManager.emitters.Count; i++)
		{
			if (RadiationGridManager.emitters[i].enabled)
			{
				RadiationGridManager.emitters[i].Emit();
			}
		}
	}

	// Token: 0x04005F51 RID: 24401
	public const float STANDARD_MASS_FALLOFF = 1000000f;

	// Token: 0x04005F52 RID: 24402
	public const int RADIATION_LINGER_RATE = 4;

	// Token: 0x04005F53 RID: 24403
	public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();

	// Token: 0x04005F54 RID: 24404
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04005F55 RID: 24405
	public static int[] previewLux;
}
