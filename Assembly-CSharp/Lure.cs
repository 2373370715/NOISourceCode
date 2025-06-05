using System;

// Token: 0x02001DDA RID: 7642
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	// Token: 0x06009FB6 RID: 40886 RVA: 0x003E0528 File Offset: 0x003DE728
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.DoNothing();
		this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
	}

	// Token: 0x06009FB7 RID: 40887 RVA: 0x003E057C File Offset: 0x003DE77C
	private void AddToScenePartitioner(Lure.Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	// Token: 0x06009FB8 RID: 40888 RVA: 0x0010C5DD File Offset: 0x0010A7DD
	private void RemoveFromScenePartitioner(Lure.Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}

	// Token: 0x04007D6D RID: 32109
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;

	// Token: 0x04007D6E RID: 32110
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

	// Token: 0x02001DDB RID: 7643
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007D6F RID: 32111
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		// Token: 0x04007D70 RID: 32112
		public int radius = 50;

		// Token: 0x04007D71 RID: 32113
		public Tag[] initialLures;
	}

	// Token: 0x02001DDC RID: 7644
	public new class Instance : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06009FBB RID: 40891 RVA: 0x0010C613 File Offset: 0x0010A813
		public int cell
		{
			get
			{
				if (this._cell == -1)
				{
					this._cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return this._cell;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06009FBC RID: 40892 RVA: 0x0010C63A File Offset: 0x0010A83A
		// (set) Token: 0x06009FBD RID: 40893 RVA: 0x0010C656 File Offset: 0x0010A856
		public CellOffset[] LurePoints
		{
			get
			{
				if (this._lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return this._lurePoints;
			}
			set
			{
				this._lurePoints = value;
			}
		}

		// Token: 0x06009FBE RID: 40894 RVA: 0x0010C65F File Offset: 0x0010A85F
		public Instance(IStateMachineTarget master, Lure.Def def) : base(master, def)
		{
		}

		// Token: 0x06009FBF RID: 40895 RVA: 0x0010C670 File Offset: 0x0010A870
		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				this.SetActiveLures(base.def.initialLures);
			}
		}

		// Token: 0x06009FC0 RID: 40896 RVA: 0x003E05C4 File Offset: 0x003DE7C4
		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = base.IsInsideState(base.sm.on);
			if (flag)
			{
				this.GoTo(base.sm.off);
			}
			this.LurePoints = new CellOffset[]
			{
				Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell)
			};
			this._cell = newCell;
			if (flag)
			{
				this.GoTo(base.sm.on);
			}
		}

		// Token: 0x06009FC1 RID: 40897 RVA: 0x0010C696 File Offset: 0x0010A896
		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				this.GoTo(base.sm.off);
				return;
			}
			this.GoTo(base.sm.on);
		}

		// Token: 0x06009FC2 RID: 40898 RVA: 0x0010C6C9 File Offset: 0x0010A8C9
		public bool IsActive()
		{
			return this.GetCurrentState() == base.sm.on;
		}

		// Token: 0x06009FC3 RID: 40899 RVA: 0x003E0640 File Offset: 0x003DE840
		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (this.lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag a in creature_lures)
			{
				foreach (Tag b in this.lures)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04007D72 RID: 32114
		private int _cell = -1;

		// Token: 0x04007D73 RID: 32115
		private Tag[] lures;

		// Token: 0x04007D74 RID: 32116
		public HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04007D75 RID: 32117
		private CellOffset[] _lurePoints;
	}
}
