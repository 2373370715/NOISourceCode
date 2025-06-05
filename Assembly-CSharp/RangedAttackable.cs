using System;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class RangedAttackable : AttackableBase
{
	// Token: 0x0600343A RID: 13370 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x000C68AF File Offset: 0x000C4AAF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.preferUnreservedCell = true;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x000C1501 File Offset: 0x000BF701
	public new int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x00216CD8 File Offset: 0x00214ED8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 0.5f, 0.5f, 0.15f);
		foreach (CellOffset offset in base.GetOffsets())
		{
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset)), Vector3.one);
		}
	}
}
