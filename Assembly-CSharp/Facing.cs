using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A7F RID: 2687
[AddComponentMenu("KMonoBehaviour/scripts/Facing")]
public class Facing : KMonoBehaviour
{
	// Token: 0x060030D6 RID: 12502 RVA: 0x000C4331 File Offset: 0x000C2531
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("Facing", 35);
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x000C434B File Offset: 0x000C254B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateMirror();
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x0020B28C File Offset: 0x0020948C
	public void Face(float target_x)
	{
		float x = base.transform.GetLocalPosition().x;
		if (target_x < x)
		{
			this.SetFacing(true);
			return;
		}
		if (target_x > x)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x0020B2C4 File Offset: 0x002094C4
	public void Face(Vector3 target_pos)
	{
		int num = Grid.CellColumn(Grid.PosToCell(base.transform.GetLocalPosition()));
		int num2 = Grid.CellColumn(Grid.PosToCell(target_pos));
		if (num > num2)
		{
			this.SetFacing(true);
			return;
		}
		if (num2 > num)
		{
			this.SetFacing(false);
		}
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x000C4359 File Offset: 0x000C2559
	[ContextMenu("Flip")]
	public void SwapFacing()
	{
		this.SetFacing(!this.facingLeft);
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000C436A File Offset: 0x000C256A
	private void UpdateMirror()
	{
		if (this.kanimController != null && this.kanimController.FlipX != this.facingLeft)
		{
			this.kanimController.FlipX = this.facingLeft;
			bool flag = this.facingLeft;
		}
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000C43A5 File Offset: 0x000C25A5
	public bool GetFacing()
	{
		return this.facingLeft;
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x000C43AD File Offset: 0x000C25AD
	public void SetFacing(bool mirror_x)
	{
		this.facingLeft = mirror_x;
		this.UpdateMirror();
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x0020B30C File Offset: 0x0020950C
	public int GetFrontCell()
	{
		int cell = Grid.PosToCell(this);
		if (this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x0020B338 File Offset: 0x00209538
	public int GetBackCell()
	{
		int cell = Grid.PosToCell(this);
		if (!this.GetFacing())
		{
			return Grid.CellLeft(cell);
		}
		return Grid.CellRight(cell);
	}

	// Token: 0x04002199 RID: 8601
	[MyCmpGet]
	private KAnimControllerBase kanimController;

	// Token: 0x0400219A RID: 8602
	private LoggerFS log;

	// Token: 0x0400219B RID: 8603
	[Serialize]
	public bool facingLeft;
}
