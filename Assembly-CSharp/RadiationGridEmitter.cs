using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017DF RID: 6111
[Serializable]
public class RadiationGridEmitter
{
	// Token: 0x06007D9A RID: 32154 RVA: 0x00332B54 File Offset: 0x00330D54
	public RadiationGridEmitter(int originCell, int intensity)
	{
		this.originCell = originCell;
		this.intensity = intensity;
	}

	// Token: 0x06007D9B RID: 32155 RVA: 0x00332BA4 File Offset: 0x00330DA4
	public void Emit()
	{
		this.scanCells.Clear();
		Vector2 a = Grid.CellToPosCCC(this.originCell, Grid.SceneLayer.Building);
		for (float num = (float)this.direction - (float)this.angle / 2f; num < (float)this.direction + (float)this.angle / 2f; num += (float)(this.angle / this.projectionCount))
		{
			float num2 = UnityEngine.Random.Range((float)(-(float)this.angle / this.projectionCount) / 2f, (float)(this.angle / this.projectionCount) / 2f);
			Vector2 vector = new Vector2(Mathf.Cos((num + num2) * 3.1415927f / 180f), Mathf.Sin((num + num2) * 3.1415927f / 180f));
			int num3 = 3;
			float num4 = (float)(this.intensity / 4);
			Vector2 a2 = vector;
			float num5 = 0f;
			while ((double)num4 > 0.01 && num5 < (float)RadiationGridEmitter.MAX_EMIT_DISTANCE)
			{
				num5 += 1f / (float)num3;
				int num6 = Grid.PosToCell(a + a2 * num5);
				if (!Grid.IsValidCell(num6))
				{
					break;
				}
				if (!this.scanCells.Contains(num6))
				{
					SimMessages.ModifyRadiationOnCell(num6, (float)Mathf.RoundToInt(num4), -1);
					this.scanCells.Add(num6);
				}
				num4 *= Mathf.Max(0f, 1f - Mathf.Pow(Grid.Mass[num6], 1.25f) * Grid.Element[num6].molarMass / 1000000f);
				num4 *= UnityEngine.Random.Range(0.96f, 0.98f);
			}
		}
	}

	// Token: 0x06007D9C RID: 32156 RVA: 0x000F73B9 File Offset: 0x000F55B9
	private int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x04005F56 RID: 24406
	private static int MAX_EMIT_DISTANCE = 128;

	// Token: 0x04005F57 RID: 24407
	public int originCell = -1;

	// Token: 0x04005F58 RID: 24408
	public int intensity = 1;

	// Token: 0x04005F59 RID: 24409
	public int projectionCount = 20;

	// Token: 0x04005F5A RID: 24410
	public int direction;

	// Token: 0x04005F5B RID: 24411
	public int angle = 360;

	// Token: 0x04005F5C RID: 24412
	public bool enabled;

	// Token: 0x04005F5D RID: 24413
	private HashSet<int> scanCells = new HashSet<int>();
}
