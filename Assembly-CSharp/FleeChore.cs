using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006D9 RID: 1753
public class FleeChore : Chore<FleeChore.StatesInstance>
{
	// Token: 0x06001F32 RID: 7986 RVA: 0x001C3B88 File Offset: 0x001C1D88
	public FleeChore(IStateMachineTarget target, GameObject enemy) : base(Db.Get().ChoreTypes.Flee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FleeChore.StatesInstance(this);
		base.smi.sm.self.Set(this.gameObject, base.smi, false);
		this.nav = this.gameObject.GetComponent<Navigator>();
		base.smi.sm.fleeFromTarget.Set(enemy, base.smi, false);
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x001C3C1C File Offset: 0x001C1E1C
	private bool isInFavoredDirection(int cell, int fleeFromCell)
	{
		bool flag = Grid.CellToPos(fleeFromCell).x < this.gameObject.transform.GetPosition().x;
		bool flag2 = Grid.CellToPos(fleeFromCell).x < Grid.CellToPos(cell).x;
		return flag == flag2;
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x001C3C70 File Offset: 0x001C1E70
	private bool CanFleeTo(int cell)
	{
		return this.nav.CanReach(cell) || this.nav.CanReach(Grid.OffsetCell(cell, -1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, -1, 1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, 1));
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x000B905C File Offset: 0x000B725C
	public GameObject CreateLocator(Vector3 pos)
	{
		return ChoreHelpers.CreateLocator("GoToLocator", pos);
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x001C3CE0 File Offset: 0x001C1EE0
	protected override void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (base.smi.sm.fleeToTarget.Get(base.smi) != null)
		{
			ChoreHelpers.DestroyLocator(base.smi.sm.fleeToTarget.Get(base.smi));
		}
		base.OnStateMachineStop(reason, status);
	}

	// Token: 0x0400147F RID: 5247
	private Navigator nav;

	// Token: 0x020006DA RID: 1754
	public class StatesInstance : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.GameInstance
	{
		// Token: 0x06001F37 RID: 7991 RVA: 0x000B9069 File Offset: 0x000B7269
		public StatesInstance(FleeChore master) : base(master)
		{
		}
	}

	// Token: 0x020006DB RID: 1755
	public class States : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore>
	{
		// Token: 0x06001F38 RID: 7992 RVA: 0x001C3D38 File Offset: 0x001C1F38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.planFleeRoute;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fleeing, null);
			this.planFleeRoute.Enter(delegate(FleeChore.StatesInstance smi)
			{
				int num = Grid.PosToCell(this.fleeFromTarget.Get(smi));
				HashSet<int> hashSet = GameUtil.FloodCollectCells(Grid.PosToCell(smi.master.gameObject), new Func<int, bool>(smi.master.CanFleeTo), 300, null, true);
				int num2 = -1;
				int num3 = -1;
				foreach (int num4 in hashSet)
				{
					if (smi.master.nav.CanReach(num4))
					{
						int num5 = -1;
						num5 += Grid.GetCellDistance(num4, num);
						if (smi.master.isInFavoredDirection(num4, num))
						{
							num5 += 8;
						}
						if (num5 > num3)
						{
							num3 = num5;
							num2 = num4;
						}
					}
				}
				int num6 = num2;
				if (num6 == -1)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.sm.fleeToTarget.Set(smi.master.CreateLocator(Grid.CellToPos(num6)), smi, false);
				smi.sm.fleeToTarget.Get(smi).name = "FleeLocator";
				if (num6 == num)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.GoTo(this.flee);
			});
			this.flee.InitializeStates(this.self, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).ToggleAnims("anim_loco_run_insane_kanim", 2f);
			this.cower.ToggleAnims("anim_cringe_kanim", 4f).PlayAnim("cringe_pre").QueueAnim("cringe_loop", false, null).QueueAnim("cringe_pst", false, null).OnAnimQueueComplete(this.end);
			this.end.Enter(delegate(FleeChore.StatesInstance smi)
			{
				smi.StopSM("stopped");
			});
		}

		// Token: 0x04001480 RID: 5248
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeFromTarget;

		// Token: 0x04001481 RID: 5249
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeToTarget;

		// Token: 0x04001482 RID: 5250
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter self;

		// Token: 0x04001483 RID: 5251
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State planFleeRoute;

		// Token: 0x04001484 RID: 5252
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.ApproachSubState<IApproachable> flee;

		// Token: 0x04001485 RID: 5253
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State cower;

		// Token: 0x04001486 RID: 5254
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State end;
	}
}
