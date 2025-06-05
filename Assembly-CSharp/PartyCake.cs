using System;
using System.Collections.Generic;

// Token: 0x02000F47 RID: 3911
public class PartyCake : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>
{
	// Token: 0x06004E6E RID: 20078 RVA: 0x002764DC File Offset: 0x002746DC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.creating.ready;
		this.creating.ready.PlayAnim("base").GoTo(this.creating.tier1);
		this.creating.tier1.InitializeStates(this.masterTarget, "tier_1", this.creating.tier2);
		this.creating.tier2.InitializeStates(this.masterTarget, "tier_2", this.creating.tier3);
		this.creating.tier3.InitializeStates(this.masterTarget, "tier_3", this.ready_to_party);
		this.ready_to_party.PlayAnim("unlit");
		this.party.PlayAnim("lit");
		this.complete.PlayAnim("finished");
	}

	// Token: 0x06004E6F RID: 20079 RVA: 0x002765C0 File Offset: 0x002747C0
	private static Chore CreateFetchChore(PartyCake.StatesInstance smi)
	{
		return new FetchChore(Db.Get().ChoreTypes.FarmFetch, smi.GetComponent<Storage>(), 10f, new HashSet<Tag>
		{
			"MushBar".ToTag()
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.Functional, 0);
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x00276610 File Offset: 0x00274810
	private static Chore CreateWorkChore(PartyCake.StatesInstance smi)
	{
		Workable component = smi.master.GetComponent<PartyCakeWorkable>();
		return new WorkChore<PartyCakeWorkable>(Db.Get().ChoreTypes.Cook, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Work, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x04003707 RID: 14087
	public PartyCake.CreatingStates creating;

	// Token: 0x04003708 RID: 14088
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready_to_party;

	// Token: 0x04003709 RID: 14089
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State party;

	// Token: 0x0400370A RID: 14090
	public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State complete;

	// Token: 0x02000F48 RID: 3912
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F49 RID: 3913
	public class CreatingStates : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
	{
		// Token: 0x0400370B RID: 14091
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State ready;

		// Token: 0x0400370C RID: 14092
		public PartyCake.CreatingStates.Tier tier1;

		// Token: 0x0400370D RID: 14093
		public PartyCake.CreatingStates.Tier tier2;

		// Token: 0x0400370E RID: 14094
		public PartyCake.CreatingStates.Tier tier3;

		// Token: 0x0400370F RID: 14095
		public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State finish;

		// Token: 0x02000F4A RID: 3914
		public class Tier : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State
		{
			// Token: 0x06004E74 RID: 20084 RVA: 0x00276660 File Offset: 0x00274860
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State InitializeStates(StateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.TargetParameter targ, string animPrefix, GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State success)
			{
				base.root.Target(targ).DefaultState(this.fetch);
				this.fetch.PlayAnim(animPrefix + "_ready").ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateFetchChore), this.work);
				this.work.Enter(delegate(PartyCake.StatesInstance smi)
				{
					KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
					component.Play(animPrefix + "_working", KAnim.PlayMode.Once, 1f, 0f);
					component.SetPositionPercent(0f);
				}).ToggleChore(new Func<PartyCake.StatesInstance, Chore>(PartyCake.CreateWorkChore), success, this.work);
				return this;
			}

			// Token: 0x04003710 RID: 14096
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State fetch;

			// Token: 0x04003711 RID: 14097
			public GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.State work;
		}
	}

	// Token: 0x02000F4C RID: 3916
	public class StatesInstance : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def>.GameInstance
	{
		// Token: 0x06004E78 RID: 20088 RVA: 0x000D758A File Offset: 0x000D578A
		public StatesInstance(IStateMachineTarget smi, PartyCake.Def def) : base(smi, def)
		{
		}
	}
}
