using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006E9 RID: 1769
public class MingleChore : Chore<MingleChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001F6A RID: 8042 RVA: 0x001C4C14 File Offset: 0x001C2E14
	public MingleChore(IStateMachineTarget target)
	{
		Chore.Precondition hasMingleCell = default(Chore.Precondition);
		hasMingleCell.id = "HasMingleCell";
		hasMingleCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_MINGLE_CELL;
		hasMingleCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((MingleChore)data).smi.HasMingleCell();
		};
		this.HasMingleCell = hasMingleCell;
		base..ctor(Db.Get().ChoreTypes.Relax, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new MingleChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(this.HasMingleCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000B91F9 File Offset: 0x000B73F9
	protected override StatusItem GetStatusItem()
	{
		return Db.Get().DuplicantStatusItems.Mingling;
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x000B920A File Offset: 0x000B740A
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x040014B7 RID: 5303
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x040014B8 RID: 5304
	private Chore.Precondition HasMingleCell;

	// Token: 0x020006EA RID: 1770
	public class States : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore>
	{
		// Token: 0x06001F6D RID: 8045 RVA: 0x001C4D10 File Offset: 0x001C2F10
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.mingle;
			base.Target(this.mingler);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, null, (MingleChore.StatesInstance smi) => !smi.IsRecTime());
			this.mingle.Transition(this.walk, (MingleChore.StatesInstance smi) => smi.IsSameRoom(), UpdateRate.SIM_200ms).Transition(this.move, (MingleChore.StatesInstance smi) => !smi.IsSameRoom(), UpdateRate.SIM_200ms);
			this.move.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.walk.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.onfloor.ToggleAnims("anim_generic_convo_kanim", 0f).PlayAnim("idle", KAnim.PlayMode.Loop).ScheduleGoTo((MingleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 10), this.success).ToggleTag(GameTags.AlwaysConverse);
			this.success.ReturnSuccess();
		}

		// Token: 0x040014B9 RID: 5305
		public StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.TargetParameter mingler;

		// Token: 0x040014BA RID: 5306
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State mingle;

		// Token: 0x040014BB RID: 5307
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State move;

		// Token: 0x040014BC RID: 5308
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State walk;

		// Token: 0x040014BD RID: 5309
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State onfloor;

		// Token: 0x040014BE RID: 5310
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State success;
	}

	// Token: 0x020006EC RID: 1772
	public class StatesInstance : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.GameInstance
	{
		// Token: 0x06001F79 RID: 8057 RVA: 0x000B9265 File Offset: 0x000B7465
		public StatesInstance(MingleChore master, GameObject mingler) : base(master)
		{
			this.mingler = mingler;
			base.sm.mingler.Set(mingler, base.smi, false);
			this.mingleCellSensor = base.GetComponent<Sensors>().GetSensor<MingleCellSensor>();
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000B929F File Offset: 0x000B749F
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000B92C0 File Offset: 0x000B74C0
		public int GetMingleCell()
		{
			return this.mingleCellSensor.GetCell();
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000B92CD File Offset: 0x000B74CD
		public bool HasMingleCell()
		{
			return this.mingleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x001C4EF8 File Offset: 0x001C30F8
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.mingler);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetMingleCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x040014C8 RID: 5320
		private MingleCellSensor mingleCellSensor;

		// Token: 0x040014C9 RID: 5321
		private GameObject mingler;
	}
}
