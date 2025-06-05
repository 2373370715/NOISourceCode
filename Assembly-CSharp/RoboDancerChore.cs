using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000741 RID: 1857
public class RoboDancerChore : Chore<RoboDancerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060020A5 RID: 8357 RVA: 0x001C8F98 File Offset: 0x001C7198
	public RoboDancerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new RoboDancerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x000B9DB5 File Offset: 0x000B7FB5
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x040015B7 RID: 5559
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x02000742 RID: 1858
	public class States : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore>
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x001C9034 File Offset: 0x001C7234
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.roboDancer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (RoboDancerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((RoboDancerChore.StatesInstance smi) => smi.GetTargetCell(), this.dancing, this.idle, false);
			this.dancing.ToggleEffect("Dancing").ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.dancing.pre).Update(delegate(RoboDancerChore.StatesInstance smi, float dt)
			{
				RoboDancer.Instance smi2 = this.roboDancer.Get(smi).GetSMI<RoboDancer.Instance>();
				RoboDancer sm = smi2.sm;
				sm.hasAudience.Set(smi.HasAudience(), smi2, false);
				sm.timeSpentDancing.Set(sm.timeSpentDancing.Get(smi2) + dt, smi2, false);
			}, UpdateRate.SIM_33ms, false).Exit(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
			});
			this.dancing.pre.QueueAnim("robotdance_pre", false, null).OnAnimQueueComplete(this.dancing.variation_1).Enter(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
				smi.CreateAudienceWorkables();
			});
			this.dancing.variation_1.QueueAnim("robotdance_loop", false, null).OnAnimQueueComplete(this.dancing.variation_2);
			this.dancing.variation_2.QueueAnim("robotdance_2_loop", false, null).OnAnimQueueComplete(this.dancing.pst);
			this.dancing.pst.QueueAnim("robotdance_pst", false, null).OnAnimQueueComplete(this.dancing.pre);
		}

		// Token: 0x040015B8 RID: 5560
		public StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.TargetParameter roboDancer;

		// Token: 0x040015B9 RID: 5561
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State idle;

		// Token: 0x040015BA RID: 5562
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State goToStand;

		// Token: 0x040015BB RID: 5563
		public RoboDancerChore.States.DancingStates dancing;

		// Token: 0x02000743 RID: 1859
		public class DancingStates : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State
		{
			// Token: 0x040015BC RID: 5564
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pre;

			// Token: 0x040015BD RID: 5565
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_1;

			// Token: 0x040015BE RID: 5566
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_2;

			// Token: 0x040015BF RID: 5567
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pst;
		}
	}

	// Token: 0x02000745 RID: 1861
	public class StatesInstance : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.GameInstance
	{
		// Token: 0x060020B1 RID: 8369 RVA: 0x001C9248 File Offset: 0x001C7448
		public StatesInstance(RoboDancerChore master, GameObject roboDancer)
		{
			Chore.Precondition isNotRoboHyped = default(Chore.Precondition);
			isNotRoboHyped.id = "IsNotRoboHyped";
			isNotRoboHyped.description = "__ Duplicant hasn't watched the dance yet";
			isNotRoboHyped.fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT);
			};
			this.IsNotRoboHyped = isNotRoboHyped;
			base..ctor(master);
			this.roboDancer = roboDancer;
			base.sm.roboDancer.Set(roboDancer, base.smi, false);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000B9E05 File Offset: 0x000B8005
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x001C92D8 File Offset: 0x001C74D8
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x001C9388 File Offset: 0x001C7588
		public bool HasAudience()
		{
			if (base.smi.watchWorkables == null)
			{
				return false;
			}
			WatchRoboDancerWorkable[] array = base.smi.watchWorkables;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].worker)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x001C93D0 File Offset: 0x001C75D0
		public void CreateAudienceWorkables()
		{
			int num = Grid.PosToCell(base.gameObject);
			Vector3Int[] array = new Vector3Int[]
			{
				Vector3Int.left * 3,
				Vector3Int.left * 2,
				Vector3Int.left,
				Vector3Int.right,
				Vector3Int.right * 2,
				Vector3Int.right * 3
			};
			int num2 = 0;
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				int cell = Grid.OffsetCell(num, array[i].x, array[i].y);
				if (Grid.IsValidCellInWorld(cell, (int)Grid.WorldIdx[num]))
				{
					GameObject gameObject = ChoreHelpers.CreateLocator("WatchRoboDancerWorkable", Grid.CellToPos(cell));
					this.audienceWorkables[i] = gameObject;
					KSelectable kselectable = gameObject.AddOrGet<KSelectable>();
					kselectable.SetName("WatchRoboDancerWorkable");
					kselectable.IsSelectable = false;
					WatchRoboDancerWorkable watchRoboDancerWorkable = gameObject.AddOrGet<WatchRoboDancerWorkable>();
					watchRoboDancerWorkable.owner = this.roboDancer;
					WorkChore<WatchRoboDancerWorkable> workChore = new WorkChore<WatchRoboDancerWorkable>(Db.Get().ChoreTypes.JoyReaction, watchRoboDancerWorkable, null, true, null, null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
					workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
					workChore.AddPrecondition(this.IsNotRoboHyped, workChore);
					num2++;
				}
			}
			this.watchWorkables = new WatchRoboDancerWorkable[num2];
			for (int j = 0; j < num2; j++)
			{
				this.watchWorkables[j] = this.audienceWorkables[j].GetComponent<WatchRoboDancerWorkable>();
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x001C9578 File Offset: 0x001C7778
		public void ClearAudienceWorkables()
		{
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				if (!(this.audienceWorkables[i] == null))
				{
					WorkerBase worker = this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().worker;
					if (worker != null)
					{
						this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().CompleteWork(worker);
					}
					ChoreHelpers.DestroyLocator(this.audienceWorkables[i]);
				}
			}
			this.watchWorkables = null;
		}

		// Token: 0x040015C5 RID: 5573
		private GameObject roboDancer;

		// Token: 0x040015C6 RID: 5574
		private GameObject[] audienceWorkables = new GameObject[4];

		// Token: 0x040015C7 RID: 5575
		private WatchRoboDancerWorkable[] watchWorkables;

		// Token: 0x040015C8 RID: 5576
		private Chore.Precondition IsNotRoboHyped;
	}
}
