using System;
using TUNING;

// Token: 0x020007D8 RID: 2008
public class DataRainer : GameStateMachine<DataRainer, DataRainer.Instance>
{
	// Token: 0x06002381 RID: 9089 RVA: 0x001D24F0 File Offset: 0x001D06F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.databanksCreated, this.overjoyed.exitEarly, (DataRainer.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.DATA_RAINER.NUM_MICROCHIPS).Exit(delegate(DataRainer.Instance smi)
		{
			this.databanksCreated.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(DataRainer.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.raining);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerPlanning, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.raining, (DataRainer.Instance smi) => smi.IsRecTime());
		this.overjoyed.raining.ToggleStatusItem(Db.Get().DuplicantStatusItems.DataRainerRaining, null).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (DataRainer.Instance smi) => !smi.IsRecTime()).ToggleChore((DataRainer.Instance smi) => new DataRainerChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(DataRainer.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x040017D5 RID: 6101
	public StateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.IntParameter databanksCreated;

	// Token: 0x040017D6 RID: 6102
	public static float databankSpawnInterval = 1.8f;

	// Token: 0x040017D7 RID: 6103
	public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017D8 RID: 6104
	public DataRainer.OverjoyedStates overjoyed;

	// Token: 0x020007D9 RID: 2009
	public class OverjoyedStates : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040017D9 RID: 6105
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040017DA RID: 6106
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State raining;

		// Token: 0x040017DB RID: 6107
		public GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x020007DA RID: 2010
	public new class Instance : GameStateMachine<DataRainer, DataRainer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06002387 RID: 9095 RVA: 0x000BB8A7 File Offset: 0x000B9AA7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000BB8B0 File Offset: 0x000B9AB0
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x001D26B8 File Offset: 0x001D08B8
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}
	}
}
