using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E83 RID: 3715
[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
	// Token: 0x06004937 RID: 18743 RVA: 0x000D3F05 File Offset: 0x000D2105
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06004938 RID: 18744 RVA: 0x000D3F13 File Offset: 0x000D2113
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Unregister();
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x0026772C File Offset: 0x0026592C
	private void Register()
	{
		this.Unregister();
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellTwo, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellThree, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellFour, false));
		}
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellThree, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellOne, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellTwo, true));
		}
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.AddVisElem(elem);
		}
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x002678B0 File Offset: 0x00265AB0
	private void Unregister()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.RemoveVisElem(elem);
		}
		this.visChildren.Clear();
	}

	// Token: 0x040033BF RID: 13247
	private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

	// Token: 0x02000E84 RID: 3716
	private class IOVisualizer : ILogicUIElement, IUniformGridObject
	{
		// Token: 0x0600493C RID: 18748 RVA: 0x000D3F34 File Offset: 0x000D2134
		public IOVisualizer(int cell, bool input)
		{
			this.cell = cell;
			this.input = input;
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x000D3F4A File Offset: 0x000D214A
		public int GetLogicUICell()
		{
			return this.cell;
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x000D3F52 File Offset: 0x000D2152
		public LogicPortSpriteType GetLogicPortSpriteType()
		{
			if (!this.input)
			{
				return LogicPortSpriteType.Output;
			}
			return LogicPortSpriteType.Input;
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x000D3F5F File Offset: 0x000D215F
		public Vector2 PosMin()
		{
			return Grid.CellToPos2D(this.cell);
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x000D3F71 File Offset: 0x000D2171
		public Vector2 PosMax()
		{
			return this.PosMin();
		}

		// Token: 0x040033C0 RID: 13248
		private int cell;

		// Token: 0x040033C1 RID: 13249
		private bool input;
	}
}
