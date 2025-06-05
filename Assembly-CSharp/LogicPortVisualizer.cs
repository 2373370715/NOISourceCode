using System;
using UnityEngine;

// Token: 0x020014F5 RID: 5365
public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006F9B RID: 28571 RVA: 0x000ED969 File Offset: 0x000EBB69
	public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.spriteType = sprite_type;
	}

	// Token: 0x06006F9C RID: 28572 RVA: 0x000ED97F File Offset: 0x000EBB7F
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06006F9D RID: 28573 RVA: 0x000ED987 File Offset: 0x000EBB87
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F9E RID: 28574 RVA: 0x000ED987 File Offset: 0x000EBB87
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F9F RID: 28575 RVA: 0x000ED999 File Offset: 0x000EBB99
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x040053E7 RID: 21479
	private int cell;

	// Token: 0x040053E8 RID: 21480
	private LogicPortSpriteType spriteType;
}
