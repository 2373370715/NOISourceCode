using System;
using STRINGS;

// Token: 0x0200017D RID: 381
public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
	// Token: 0x06000573 RID: 1395 RVA: 0x00161FAC File Offset: 0x001601AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DROWNING.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Drowning, null, true);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
		this.move_to_safe.MoveTo((DrowningStates.Instance smi) => smi.safeCell, null, null, false);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00162098 File Offset: 0x00160298
	public bool UpdateSafeCell(DrowningStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		DrowningStates.EscapeCellQuery escapeCellQuery = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
		component.RunQuery(escapeCellQuery);
		smi.safeCell = escapeCellQuery.GetResultCell();
		return smi.safeCell != Grid.InvalidCell;
	}

	// Token: 0x040003F9 RID: 1017
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;

	// Token: 0x040003FA RID: 1018
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;

	// Token: 0x040003FB RID: 1019
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

	// Token: 0x0200017E RID: 382
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200017F RID: 383
	public new class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x000AC58A File Offset: 0x000AA78A
		public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}

		// Token: 0x040003FC RID: 1020
		public int safeCell = Grid.InvalidCell;
	}

	// Token: 0x02000180 RID: 384
	public class EscapeCellQuery : PathFinderQuery
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x000AC5B9 File Offset: 0x000AA7B9
		public EscapeCellQuery(DrowningMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000AC5C8 File Offset: 0x000AA7C8
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			return this.monitor.IsCellSafe(cell);
		}

		// Token: 0x040003FD RID: 1021
		private DrowningMonitor monitor;
	}
}
