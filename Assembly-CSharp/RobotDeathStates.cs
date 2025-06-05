using System;
using STRINGS;

// Token: 0x020001F9 RID: 505
public class RobotDeathStates : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
	// Token: 0x060006D8 RID: 1752 RVA: 0x00165CE0 File Offset: 0x00163EE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).PlayAnim((RobotDeathStates.Instance smi) => smi.def.deathAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, (RobotDeathStates.Instance smi) => smi.gameObject).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x0400050A RID: 1290
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop;

	// Token: 0x0400050B RID: 1291
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State pst;

	// Token: 0x020001FA RID: 506
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400050C RID: 1292
		public string deathAnim = "death";
	}

	// Token: 0x020001FB RID: 507
	public new class Instance : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.GameInstance
	{
		// Token: 0x060006DB RID: 1755 RVA: 0x00165DB4 File Offset: 0x00163FB4
		public Instance(Chore<RobotDeathStates.Instance> chore, RobotDeathStates.Def def) : base(chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}
	}
}
