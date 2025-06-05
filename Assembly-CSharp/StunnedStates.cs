using System;
using STRINGS;

// Token: 0x02000211 RID: 529
public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
	// Token: 0x06000738 RID: 1848 RVA: 0x00167984 File Offset: 0x00165B84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stunned;
		GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.stunned.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Stunned, null, true);
	}

	// Token: 0x04000563 RID: 1379
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;

	// Token: 0x02000212 RID: 530
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000213 RID: 531
	public new class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x000AD99C File Offset: 0x000ABB9C
		public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(StunnedStates.Instance.IsStunned, null);
		}

		// Token: 0x04000564 RID: 1380
		public static readonly Chore.Precondition IsStunned = new Chore.Precondition
		{
			id = "IsStunned",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Stunned);
			}
		};
	}
}
