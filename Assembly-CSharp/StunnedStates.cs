using System;
using STRINGS;

{
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

	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;
	public class Def : StateMachine.BaseDef
	{
	}

	{
		{
			chore.AddPrecondition(StunnedStates.Instance.IsStunned, null);

		{
			id = "IsStunned",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
			}
		};
}
