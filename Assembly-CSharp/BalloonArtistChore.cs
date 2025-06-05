using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class BalloonArtistChore : Chore<BalloonArtistChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001D59 RID: 7513 RVA: 0x001BAE24 File Offset: 0x001B9024
	public BalloonArtistChore(IStateMachineTarget target)
	{
		Chore.Precondition hasBalloonStallCell = default(Chore.Precondition);
		hasBalloonStallCell.id = "HasBalloonStallCell";
		hasBalloonStallCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_BALLOON_STALL_CELL;
		hasBalloonStallCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BalloonArtistChore)data).smi.HasBalloonStallCell();
		};
		this.HasBalloonStallCell = hasBalloonStallCell;
		base..ctor(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new BalloonArtistChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(this.HasBalloonStallCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000B7D06 File Offset: 0x000B5F06
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04001297 RID: 4759
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04001298 RID: 4760
	private Chore.Precondition HasBalloonStallCell;

	// Token: 0x0200066F RID: 1647
	public class States : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore>
	{
		// Token: 0x06001D5B RID: 7515 RVA: 0x001BAF20 File Offset: 0x001B9120
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.balloonArtist);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, this.idle, (BalloonArtistChore.StatesInstance smi) => !smi.IsRecTime());
			this.idle.DoNothing();
			this.goToStand.Transition(null, (BalloonArtistChore.StatesInstance smi) => !smi.HasBalloonStallCell(), UpdateRate.SIM_200ms).MoveTo((BalloonArtistChore.StatesInstance smi) => smi.GetBalloonStallCell(), this.balloonStand, null, false);
			this.balloonStand.ToggleAnims("anim_interacts_balloon_artist_kanim", 0f).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.SpawnBalloonStand();
			}).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				this.balloonArtist.GetSMI<BalloonArtist.Instance>(smi).Internal_InitBalloons();
			}).Exit(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.DestroyBalloonStand();
			}).DefaultState(this.balloonStand.idle);
			this.balloonStand.idle.PlayAnim("working_pre").QueueAnim("working_loop", true, null).OnSignal(this.giveBalloonOut, this.balloonStand.giveBalloon);
			this.balloonStand.giveBalloon.PlayAnim("working_pst").OnAnimQueueComplete(this.balloonStand.idle);
		}

		// Token: 0x04001299 RID: 4761
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.TargetParameter balloonArtist;

		// Token: 0x0400129A RID: 4762
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter balloonsGivenOut = new StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter(0);

		// Token: 0x0400129B RID: 4763
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.Signal giveBalloonOut;

		// Token: 0x0400129C RID: 4764
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

		// Token: 0x0400129D RID: 4765
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State goToStand;

		// Token: 0x0400129E RID: 4766
		public BalloonArtistChore.States.BalloonStandStates balloonStand;

		// Token: 0x02000670 RID: 1648
		public class BalloonStandStates : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State
		{
			// Token: 0x0400129F RID: 4767
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

			// Token: 0x040012A0 RID: 4768
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State giveBalloon;
		}
	}

	// Token: 0x02000672 RID: 1650
	public class StatesInstance : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.GameInstance
	{
		// Token: 0x06001D66 RID: 7526 RVA: 0x000B7D7A File Offset: 0x000B5F7A
		public StatesInstance(BalloonArtistChore master, GameObject balloonArtist) : base(master)
		{
			this.balloonArtist = balloonArtist;
			base.sm.balloonArtist.Set(balloonArtist, base.smi, false);
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000B7DA3 File Offset: 0x000B5FA3
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x000B7DC4 File Offset: 0x000B5FC4
		public int GetBalloonStallCell()
		{
			return this.balloonArtistCellSensor.GetCell();
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x000B7DD1 File Offset: 0x000B5FD1
		public int GetBalloonStallTargetCell()
		{
			return this.balloonArtistCellSensor.GetStandCell();
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000B7DDE File Offset: 0x000B5FDE
		public bool HasBalloonStallCell()
		{
			if (this.balloonArtistCellSensor == null)
			{
				this.balloonArtistCellSensor = base.GetComponent<Sensors>().GetSensor<BalloonStandCellSensor>();
			}
			return this.balloonArtistCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x001BB0BC File Offset: 0x001B92BC
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.balloonArtist);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetBalloonStallCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x001BB118 File Offset: 0x001B9318
		public void SpawnBalloonStand()
		{
			Vector3 vector = Grid.CellToPos(this.GetBalloonStallTargetCell());
			this.balloonArtist.GetComponent<Facing>().Face(vector);
			this.balloonStand = Util.KInstantiate(Assets.GetPrefab("BalloonStand"), vector, Quaternion.identity, null, null, true, 0);
			this.balloonStand.SetActive(true);
			this.balloonStand.GetComponent<GetBalloonWorkable>().SetBalloonArtist(base.smi);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000B7E0E File Offset: 0x000B600E
		public void DestroyBalloonStand()
		{
			this.balloonStand.DeleteObject();
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000B7E1B File Offset: 0x000B601B
		public BalloonOverrideSymbol GetBalloonOverride()
		{
			return this.balloonArtist.GetSMI<BalloonArtist.Instance>().GetCurrentBalloonSymbolOverride();
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B7E2D File Offset: 0x000B602D
		public void NextBalloonOverride()
		{
			this.balloonArtist.GetSMI<BalloonArtist.Instance>().ApplyNextBalloonSymbolOverride();
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x001BB188 File Offset: 0x001B9388
		public void GiveBalloon(BalloonOverrideSymbol balloonOverride)
		{
			BalloonArtist.Instance smi = this.balloonArtist.GetSMI<BalloonArtist.Instance>();
			smi.GiveBalloon();
			balloonOverride.ApplyTo(smi);
			base.smi.sm.giveBalloonOut.Trigger(base.smi);
		}

		// Token: 0x040012A7 RID: 4775
		private BalloonStandCellSensor balloonArtistCellSensor;

		// Token: 0x040012A8 RID: 4776
		private GameObject balloonArtist;

		// Token: 0x040012A9 RID: 4777
		private GameObject balloonStand;
	}
}
