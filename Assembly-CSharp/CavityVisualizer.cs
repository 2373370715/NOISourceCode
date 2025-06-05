using System;
using System.Collections.Generic;
using ProcGenGame;
using UnityEngine;

// Token: 0x020020E0 RID: 8416
[AddComponentMenu("KMonoBehaviour/scripts/CavityVisualizer")]
public class CavityVisualizer : KMonoBehaviour
{
	// Token: 0x0600B363 RID: 45923 RVA: 0x00441D30 File Offset: 0x0043FF30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
		{
			foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
			{
				foreach (int item in hashSet)
				{
					this.cavityCells.Add(item);
				}
			}
		}
	}

	// Token: 0x0600B364 RID: 45924 RVA: 0x00441E08 File Offset: 0x00440008
	private void OnDrawGizmosSelected()
	{
		if (this.drawCavity)
		{
			Color[] array = new Color[]
			{
				Color.blue,
				Color.yellow
			};
			int num = 0;
			foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
			{
				Gizmos.color = array[num % array.Length];
				Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
				num++;
				foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
				{
					foreach (int cell in hashSet)
					{
						Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
					}
				}
			}
		}
		if (this.spawnCells != null && this.drawSpawnCells)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
			foreach (int cell2 in this.spawnCells)
			{
				Gizmos.DrawCube(Grid.CellToPos(cell2) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
			}
		}
	}

	// Token: 0x04008DF1 RID: 36337
	public List<int> cavityCells = new List<int>();

	// Token: 0x04008DF2 RID: 36338
	public List<int> spawnCells = new List<int>();

	// Token: 0x04008DF3 RID: 36339
	public bool drawCavity = true;

	// Token: 0x04008DF4 RID: 36340
	public bool drawSpawnCells = true;
}
