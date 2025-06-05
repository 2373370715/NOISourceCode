using System;
using UnityEngine;

// Token: 0x020014EC RID: 5356
internal class LogicEventSender : ILogicEventSender, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006F48 RID: 28488 RVA: 0x000ED5FE File Offset: 0x000EB7FE
	public LogicEventSender(HashedString id, int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.id = id;
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06006F49 RID: 28489 RVA: 0x000ED633 File Offset: 0x000EB833
	public HashedString ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06006F4A RID: 28490 RVA: 0x000ED63B File Offset: 0x000EB83B
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06006F4B RID: 28491 RVA: 0x000ED643 File Offset: 0x000EB843
	public int GetLogicValue()
	{
		return this.logicValue;
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x000ED64B File Offset: 0x000EB84B
	public int GetLogicUICell()
	{
		return this.GetLogicCell();
	}

	// Token: 0x06006F4D RID: 28493 RVA: 0x000ED653 File Offset: 0x000EB853
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06006F4E RID: 28494 RVA: 0x000ED65B File Offset: 0x000EB85B
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F4F RID: 28495 RVA: 0x000ED65B File Offset: 0x000EB85B
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F50 RID: 28496 RVA: 0x00300774 File Offset: 0x002FE974
	public void SetValue(int value)
	{
		int arg = this.logicValue;
		this.logicValue = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x06006F51 RID: 28497 RVA: 0x000AA038 File Offset: 0x000A8238
	public void LogicTick()
	{
	}

	// Token: 0x06006F52 RID: 28498 RVA: 0x000ED66D File Offset: 0x000EB86D
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x040053B2 RID: 21426
	private HashedString id;

	// Token: 0x040053B3 RID: 21427
	private int cell;

	// Token: 0x040053B4 RID: 21428
	private int logicValue = -16;

	// Token: 0x040053B5 RID: 21429
	private Action<int, int> onValueChanged;

	// Token: 0x040053B6 RID: 21430
	private Action<int, bool> onConnectionChanged;

	// Token: 0x040053B7 RID: 21431
	private LogicPortSpriteType spriteType;
}
