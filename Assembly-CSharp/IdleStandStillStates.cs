using System;
using STRINGS;

// Token: 0x020001C4 RID: 452
public class IdleStandStillStates : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>
{
	// Token: 0x06000632 RID: 1586 RVA: 0x00163DAC File Offset: 0x00161FAC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State.Callback(this.PlayIdle));
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00163E2C File Offset: 0x0016202C
	public void PlayIdle(IdleStandStillStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		component.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04000485 RID: 1157
	private GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State loop;

	// Token: 0x020001C5 RID: 453
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000486 RID: 1158
		public IdleStandStillStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x020001C6 RID: 454
		// (Invoke) Token: 0x06000637 RID: 1591
		public delegate HashedString IdleAnimCallback(IdleStandStillStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x020001C7 RID: 455
	public new class Instance : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.GameInstance
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x000ACF13 File Offset: 0x000AB113
		public Instance(Chore<IdleStandStillStates.Instance> chore, IdleStandStillStates.Def def) : base(chore, def)
		{
		}
	}
}
