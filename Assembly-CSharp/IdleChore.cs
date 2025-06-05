using System;
using UnityEngine;

// Token: 0x020006E4 RID: 1764
public class IdleChore : Chore<IdleChore.StatesInstance>
{
	// Token: 0x06001F51 RID: 8017 RVA: 0x001C47B8 File Offset: 0x001C29B8
	public IdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.IdleTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new IdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020006E5 RID: 1765
	public class StatesInstance : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
	{
		// Token: 0x06001F52 RID: 8018 RVA: 0x000B9103 File Offset: 0x000B7303
		public StatesInstance(IdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
			this.idleCellSensor = base.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x001C4808 File Offset: 0x001C2A08
		public void UpdateNavType()
		{
			NavType currentNavType = base.GetComponent<Navigator>().CurrentNavType;
			base.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, this, false);
			base.sm.isOnTube.Set(currentNavType == NavType.Tube, this, false);
			int num = Grid.PosToCell(base.smi);
			base.sm.isOnSuitMarkerCell.Set(Grid.IsValidCell(num) && Grid.HasSuitMarker[num], this, false);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000B9136 File Offset: 0x000B7336
		public int GetIdleCell()
		{
			return this.idleCellSensor.GetCell();
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000B9143 File Offset: 0x000B7343
		public bool HasIdleCell()
		{
			return this.idleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x040014A0 RID: 5280
		private IdleCellSensor idleCellSensor;
	}

	// Token: 0x020006E6 RID: 1766
	public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
	{
		// Token: 0x06001F56 RID: 8022 RVA: 0x001C488C File Offset: 0x001C2A8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Update("UpdateNavType", delegate(IdleChore.StatesInstance smi, float dt)
			{
				smi.UpdateNavType();
			}, UpdateRate.SIM_200ms, false).ToggleStateMachine((IdleChore.StatesInstance smi) => new TaskAvailabilityMonitor.Instance(smi.master)).ToggleTag(GameTags.Idle);
			this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnSuitMarkerCell, this.idle.onsuitmarker, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", delegate(IdleChore.StatesInstance smi, float dt)
			{
				if (smi.HasIdleCell())
				{
					smi.GoTo(this.idle.move);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.idle.onsuitmarker.PlayAnim("idle_default", KAnim.PlayMode.Loop).Enter(delegate(IdleChore.StatesInstance smi)
			{
				Navigator component = smi.GetComponent<Navigator>();
				int cell = Grid.PosToCell(component);
				Grid.SuitMarker.Flags flags;
				PathFinder.PotentialPath.Flags flags2;
				Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2);
				IdleSuitMarkerCellQuery idleSuitMarkerCellQuery = new IdleSuitMarkerCellQuery((flags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0, Grid.CellToXY(cell).X);
				component.RunQuery(idleSuitMarkerCellQuery);
				component.GoTo(idleSuitMarkerCellQuery.GetResultCell(), null);
			}).EventTransition(GameHashes.DestinationReached, this.idle, null).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.move.Transition(this.idle, (IdleChore.StatesInstance smi) => !smi.HasIdleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((IdleChore.StatesInstance smi) => smi.GetIdleCell(), this.idle, this.idle, false).Exit("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Exit("ClearWalk", delegate(IdleChore.StatesInstance smi)
			{
				smi.GetComponent<KBatchedAnimController>().Play("idle_default", KAnim.PlayMode.Once, 1f, 0f);
			});
		}

		// Token: 0x040014A1 RID: 5281
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;

		// Token: 0x040014A2 RID: 5282
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;

		// Token: 0x040014A3 RID: 5283
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnSuitMarkerCell;

		// Token: 0x040014A4 RID: 5284
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;

		// Token: 0x040014A5 RID: 5285
		public IdleChore.States.IdleState idle;

		// Token: 0x020006E7 RID: 1767
		public class IdleState : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
		{
			// Token: 0x040014A6 RID: 5286
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;

			// Token: 0x040014A7 RID: 5287
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;

			// Token: 0x040014A8 RID: 5288
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;

			// Token: 0x040014A9 RID: 5289
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onsuitmarker;

			// Token: 0x040014AA RID: 5290
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
		}
	}
}
