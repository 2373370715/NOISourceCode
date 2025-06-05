using System;
using TUNING;
using UnityEngine;

// Token: 0x0200069E RID: 1694
public class DataRainerChore : Chore<DataRainerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001E3E RID: 7742 RVA: 0x001BF430 File Offset: 0x001BD630
	public DataRainerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DataRainerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000B8774 File Offset: 0x000B6974
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x040013A2 RID: 5026
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x0200069F RID: 1695
	public class States : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore>
	{
		// Token: 0x06001E40 RID: 7744 RVA: 0x001BF4CC File Offset: 0x001BD6CC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.dataRainer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (DataRainerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((DataRainerChore.StatesInstance smi) => smi.GetTargetCell(), this.raining, this.idle, false);
			this.raining.ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.raining.loop).Update(delegate(DataRainerChore.StatesInstance smi, float dt)
			{
				this.nextBankTimer.Delta(dt, smi);
				if (this.nextBankTimer.Get(smi) >= DataRainer.databankSpawnInterval)
				{
					this.nextBankTimer.Delta(-DataRainer.databankSpawnInterval, smi);
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("PowerStationTools"), smi.master.transform.position + Vector3.up);
					gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Iron, true);
					gameObject.SetActive(true);
					KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
					float num = (float)component.currentFrame / (float)component.GetCurrentNumFrames();
					Vector2 initial_velocity = new Vector2((num < 0.5f) ? -2.5f : 2.5f, 4f);
					if (GameComps.Fallers.Has(gameObject))
					{
						GameComps.Fallers.Remove(gameObject);
					}
					GameComps.Fallers.Add(gameObject, initial_velocity);
					DataRainer.Instance smi2 = this.dataRainer.Get(smi).GetSMI<DataRainer.Instance>();
					DataRainer sm = smi2.sm;
					sm.databanksCreated.Set(sm.databanksCreated.Get(smi2) + 1, smi2, false);
				}
			}, UpdateRate.SIM_33ms, false);
			this.raining.loop.PlayAnim("makeitrain2", KAnim.PlayMode.Loop);
		}

		// Token: 0x040013A3 RID: 5027
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.TargetParameter dataRainer;

		// Token: 0x040013A4 RID: 5028
		public StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter nextBankTimer = new StateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.FloatParameter(DataRainer.databankSpawnInterval / 2f);

		// Token: 0x040013A5 RID: 5029
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State idle;

		// Token: 0x040013A6 RID: 5030
		public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State goToStand;

		// Token: 0x040013A7 RID: 5031
		public DataRainerChore.States.RainingStates raining;

		// Token: 0x020006A0 RID: 1696
		public class RainingStates : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State
		{
			// Token: 0x040013A8 RID: 5032
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pre;

			// Token: 0x040013A9 RID: 5033
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State loop;

			// Token: 0x040013AA RID: 5034
			public GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.State pst;
		}
	}

	// Token: 0x020006A2 RID: 1698
	public class StatesInstance : GameStateMachine<DataRainerChore.States, DataRainerChore.StatesInstance, DataRainerChore, object>.GameInstance
	{
		// Token: 0x06001E48 RID: 7752 RVA: 0x000B87C4 File Offset: 0x000B69C4
		public StatesInstance(DataRainerChore master, GameObject dataRainer) : base(master)
		{
			this.dataRainer = dataRainer;
			base.sm.dataRainer.Set(dataRainer, base.smi, false);
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000B87ED File Offset: 0x000B69ED
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x001BF6D4 File Offset: 0x001BD8D4
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

		// Token: 0x040013AE RID: 5038
		private GameObject dataRainer;
	}
}
