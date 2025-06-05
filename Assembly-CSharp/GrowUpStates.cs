using System;
using STRINGS;

// Token: 0x0200019D RID: 413
public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
	// Token: 0x060005C6 RID: 1478 RVA: 0x00162E50 File Offset: 0x00161050
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grow_up_pre;
		GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.grow_up_pre.Enter(delegate(GrowUpStates.Instance smi)
		{
			smi.PlayPreGrowAnimation();
		}).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, this.spawn_adult);
		this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x000AC9D4 File Offset: 0x000AABD4
	private static void SpawnAdult(GrowUpStates.Instance smi)
	{
		smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
	}

	// Token: 0x04000438 RID: 1080
	public const float GROW_PRE_TIMEOUT = 4f;

	// Token: 0x04000439 RID: 1081
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;

	// Token: 0x0400043A RID: 1082
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

	// Token: 0x0200019E RID: 414
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200019F RID: 415
	public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
	{
		// Token: 0x060005CA RID: 1482 RVA: 0x000AC9E9 File Offset: 0x000AABE9
		public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00162F0C File Offset: 0x0016110C
		public void PlayPreGrowAnimation()
		{
			if (base.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("growup_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
