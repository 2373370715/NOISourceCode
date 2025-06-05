using System;

// Token: 0x02001619 RID: 5657
public class RocketPassengerMonitor : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance>
{
	// Token: 0x06007522 RID: 29986 RVA: 0x00314784 File Offset: 0x00312984
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.satisfied.ParamTransition<int>(this.targetCell, this.moving, (RocketPassengerMonitor.Instance smi, int p) => p != Grid.InvalidCell);
		this.moving.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.satisfied).Exit(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(Grid.InvalidCell, smi, false);
		});
		this.movingToModuleDeployPre.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(smi.moduleDeployTaskTargetMoveCell, smi, false);
			smi.GoTo(this.movingToModuleDeploy);
		});
		this.movingToModuleDeploy.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.moduleDeploy);
		this.moduleDeploy.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			smi.moduleDeployCompleteCallback(null);
			this.targetCell.Set(Grid.InvalidCell, smi, false);
			smi.moduleDeployCompleteCallback = null;
			smi.GoTo(smi.sm.satisfied);
		});
	}

	// Token: 0x06007523 RID: 29987 RVA: 0x003148B4 File Offset: 0x00312AB4
	public Chore CreateChore(RocketPassengerMonitor.Instance smi)
	{
		MoveChore moveChore = new MoveChore(smi.master, Db.Get().ChoreTypes.RocketEnterExit, (MoveChore.StatesInstance mover_smi) => this.targetCell.Get(smi), false);
		moveChore.AddPrecondition(ChorePreconditions.instance.CanMoveToCell, this.targetCell.Get(smi));
		return moveChore;
	}

	// Token: 0x04005800 RID: 22528
	public StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter targetCell = new StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter(Grid.InvalidCell);

	// Token: 0x04005801 RID: 22529
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005802 RID: 22530
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moving;

	// Token: 0x04005803 RID: 22531
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeployPre;

	// Token: 0x04005804 RID: 22532
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeploy;

	// Token: 0x04005805 RID: 22533
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moduleDeploy;

	// Token: 0x0200161A RID: 5658
	public new class Instance : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600752A RID: 29994 RVA: 0x000F1721 File Offset: 0x000EF921
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x00314928 File Offset: 0x00312B28
		public bool ShouldMoveThroughRocketDoor()
		{
			int num = base.sm.targetCell.Get(this);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if ((int)Grid.WorldIdx[num] == this.GetMyWorldId())
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x000F172A File Offset: 0x000EF92A
		public void SetMoveTarget(int cell)
		{
			if ((int)Grid.WorldIdx[cell] == this.GetMyWorldId())
			{
				return;
			}
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x000F1750 File Offset: 0x000EF950
		public void SetModuleDeployChore(int cell, Action<Chore> OnChoreCompleteCallback)
		{
			this.moduleDeployCompleteCallback = OnChoreCompleteCallback;
			this.moduleDeployTaskTargetMoveCell = cell;
			this.GoTo(base.sm.movingToModuleDeployPre);
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x0600752E RID: 29998 RVA: 0x000F1785 File Offset: 0x000EF985
		public void CancelModuleDeployChore()
		{
			this.moduleDeployCompleteCallback = null;
			this.moduleDeployTaskTargetMoveCell = Grid.InvalidCell;
			base.sm.targetCell.Set(Grid.InvalidCell, base.smi, false);
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x0031497C File Offset: 0x00312B7C
		public void ClearMoveTarget(int testCell)
		{
			int num = base.sm.targetCell.Get(this);
			if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[testCell])
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				if (base.IsInsideState(base.sm.moving))
				{
					this.GoTo(base.sm.satisfied);
				}
			}
		}

		// Token: 0x04005806 RID: 22534
		public int lastWorldID;

		// Token: 0x04005807 RID: 22535
		public Action<Chore> moduleDeployCompleteCallback;

		// Token: 0x04005808 RID: 22536
		public int moduleDeployTaskTargetMoveCell;
	}
}
