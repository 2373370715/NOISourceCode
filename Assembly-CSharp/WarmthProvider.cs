using System;
using System.Collections.Generic;

// Token: 0x02000BA3 RID: 2979
public class WarmthProvider : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
	// Token: 0x060037FC RID: 14332 RVA: 0x000C8C7A File Offset: 0x000C6E7A
	public static bool IsWarmCell(int cell)
	{
		return WarmthProvider.WarmCells.ContainsKey(cell) && WarmthProvider.WarmCells[cell] > 0;
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x000C8C99 File Offset: 0x000C6E99
	public static int GetWarmthValue(int cell)
	{
		if (!WarmthProvider.WarmCells.ContainsKey(cell))
		{
			return -1;
		}
		return (int)WarmthProvider.WarmCells[cell];
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x00226EC4 File Offset: 0x002250C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.EventTransition(GameHashes.ActiveChanged, this.on, (WarmthProvider.Instance smi) => smi.GetComponent<Operational>().IsActive).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.RemoveWarmCells));
		this.on.EventTransition(GameHashes.ActiveChanged, this.off, (WarmthProvider.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Operational, this.off, true).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.AddWarmCells));
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x000C8CB5 File Offset: 0x000C6EB5
	private static void AddWarmCells(WarmthProvider.Instance smi)
	{
		smi.AddWarmCells();
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x000C8CBD File Offset: 0x000C6EBD
	private static void RemoveWarmCells(WarmthProvider.Instance smi)
	{
		smi.RemoveWarmCells();
	}

	// Token: 0x040026A3 RID: 9891
	public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();

	// Token: 0x040026A4 RID: 9892
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State off;

	// Token: 0x040026A5 RID: 9893
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State on;

	// Token: 0x02000BA4 RID: 2980
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040026A6 RID: 9894
		public Vector2I OriginOffset;

		// Token: 0x040026A7 RID: 9895
		public Vector2I RangeMin;

		// Token: 0x040026A8 RID: 9896
		public Vector2I RangeMax;

		// Token: 0x040026A9 RID: 9897
		public Func<int, bool> blockingCellCallback = new Func<int, bool>(Grid.IsSolidCell);
	}

	// Token: 0x02000BA5 RID: 2981
	public new class Instance : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.GameInstance
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x000C8CF3 File Offset: 0x000C6EF3
		public bool IsWarming
		{
			get
			{
				return base.IsInsideState(base.sm.on);
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000C8D06 File Offset: 0x000C6F06
		public Instance(IStateMachineTarget master, WarmthProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x00226F80 File Offset: 0x00225180
		public override void StartSM()
		{
			EntityCellVisualizer component = base.GetComponent<EntityCellVisualizer>();
			if (component != null)
			{
				component.AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
			}
			this.WorldID = base.gameObject.GetMyWorldId();
			this.SetupRange();
			this.CreateCellListeners();
			base.StartSM();
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x00226FD4 File Offset: 0x002251D4
		private void SetupRange()
		{
			Vector2I u = Grid.PosToXY(base.transform.GetPosition());
			Vector2I vector2I = base.def.OriginOffset;
			this.range_min = base.def.RangeMin;
			this.range_max = base.def.RangeMax;
			Rotatable rotatable;
			if (base.gameObject.TryGetComponent<Rotatable>(out rotatable))
			{
				vector2I = rotatable.GetRotatedOffset(vector2I);
				Vector2I rotatedOffset = rotatable.GetRotatedOffset(this.range_min);
				Vector2I rotatedOffset2 = rotatable.GetRotatedOffset(this.range_max);
				this.range_min.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_min.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				this.range_max.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_max.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			this.origin = u + vector2I;
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x00227108 File Offset: 0x00225308
		public bool ContainsCell(int cell)
		{
			if (this.cellsInRange == null)
			{
				return false;
			}
			for (int i = 0; i < this.cellsInRange.Length; i++)
			{
				if (this.cellsInRange[i] == cell)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x00227140 File Offset: 0x00225340
		private void UnmarkAllCellsInRange()
		{
			if (this.cellsInRange != null)
			{
				for (int i = 0; i < this.cellsInRange.Length; i++)
				{
					int num = this.cellsInRange[i];
					if (WarmthProvider.WarmCells.ContainsKey(num))
					{
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b - 1;
					}
				}
			}
			this.cellsInRange = null;
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x002271A0 File Offset: 0x002253A0
		private void UpdateCellsInRange()
		{
			this.UnmarkAllCellsInRange();
			Grid.PosToCell(this);
			List<int> list = new List<int>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int num = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(num, this.WorldID) && this.IsCellVisible(num))
					{
						list.Add(num);
						if (!WarmthProvider.WarmCells.ContainsKey(num))
						{
							WarmthProvider.WarmCells.Add(num, 0);
						}
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b + 1;
					}
				}
			}
			this.cellsInRange = list.ToArray();
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000C8D10 File Offset: 0x000C6F10
		public void AddWarmCells()
		{
			this.UpdateCellsInRange();
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000C8D18 File Offset: 0x000C6F18
		public void RemoveWarmCells()
		{
			this.UnmarkAllCellsInRange();
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000C8D20 File Offset: 0x000C6F20
		protected override void OnCleanUp()
		{
			this.RemoveWarmCells();
			this.ClearCellListeners();
			base.OnCleanUp();
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x002272B4 File Offset: 0x002254B4
		public bool IsCellVisible(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
			Vector2I vector2I2 = Grid.CellToXY(cell);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, base.def.blockingCellCallback, false, false);
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000C8D34 File Offset: 0x000C6F34
		public void OnSolidCellChanged(object obj)
		{
			if (this.IsWarming)
			{
				this.UpdateCellsInRange();
			}
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x00227300 File Offset: 0x00225500
		private void CreateCellListeners()
		{
			Grid.PosToCell(this);
			List<HandleVector<int>.Handle> list = new List<HandleVector<int>.Handle>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int cell = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(cell, this.WorldID))
					{
						list.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidCellChanged)));
					}
				}
			}
			this.partitionEntries = list.ToArray();
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x002273F0 File Offset: 0x002255F0
		private void ClearCellListeners()
		{
			if (this.partitionEntries != null)
			{
				for (int i = 0; i < this.partitionEntries.Length; i++)
				{
					HandleVector<int>.Handle handle = this.partitionEntries[i];
					GameScenePartitioner.Instance.Free(ref handle);
				}
			}
		}

		// Token: 0x040026AA RID: 9898
		public int WorldID;

		// Token: 0x040026AB RID: 9899
		private int[] cellsInRange;

		// Token: 0x040026AC RID: 9900
		private HandleVector<int>.Handle[] partitionEntries;

		// Token: 0x040026AD RID: 9901
		public Vector2I range_min;

		// Token: 0x040026AE RID: 9902
		public Vector2I range_max;

		// Token: 0x040026AF RID: 9903
		public Vector2I origin;
	}
}
