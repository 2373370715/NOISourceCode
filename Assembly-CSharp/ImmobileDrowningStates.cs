using System;
using STRINGS;

// Token: 0x020001CE RID: 462
public class ImmobileDrowningStates : GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>
{
	// Token: 0x06000653 RID: 1619 RVA: 0x0016427C File Offset: 0x0016247C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.DROWNING.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWNING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TagTransition(GameTags.Creatures.Drowning, this.drown_pst, true);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Drowning, false);
	}

	// Token: 0x04000494 RID: 1172
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown;

	// Token: 0x04000495 RID: 1173
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown_pst;

	// Token: 0x04000496 RID: 1174
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State behaviourcomplete;

	// Token: 0x020001CF RID: 463
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001D0 RID: 464
	public new class Instance : GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.GameInstance
	{
		// Token: 0x06000656 RID: 1622 RVA: 0x000ACFC5 File Offset: 0x000AB1C5
		public Instance(Chore<ImmobileDrowningStates.Instance> chore, ImmobileDrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}
	}
}
