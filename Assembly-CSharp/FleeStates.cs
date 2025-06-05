using System;
using STRINGS;

// Token: 0x02000196 RID: 406
public class FleeStates : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x00162968 File Offset: 0x00160B68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.plan;
		GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State state = this.root.Enter("SetFleeTarget", delegate(FleeStates.Instance smi)
		{
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi.GetSMI<ThreatMonitor.Instance>().MainThreat), smi, false);
		});
		string name = CREATURES.STATUSITEMS.FLEEING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FLEEING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.plan.Enter(delegate(FleeStates.Instance smi)
		{
			ThreatMonitor.Instance smi2 = smi.master.gameObject.GetSMI<ThreatMonitor.Instance>();
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi2.MainThreat), smi, false);
			if (this.fleeToTarget.Get(smi) != null)
			{
				smi.GoTo(this.approach);
				return;
			}
			smi.GoTo(this.cower);
		});
		this.approach.InitializeStates(this.mover, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).Enter(delegate(FleeStates.Instance smi)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, CREATURES.STATUSITEMS.FLEEING.NAME.text, smi.master.transform, 1.5f, false);
		});
		this.cower.Enter(delegate(FleeStates.Instance smi)
		{
			string s = "DEFAULT COWER ANIMATION";
			if (smi.Get<KBatchedAnimController>().HasAnimation("cower"))
			{
				s = "cower";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle"))
			{
				s = "idle";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle_loop"))
			{
				s = "idle_loop";
			}
			smi.Get<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}).ScheduleGoTo(2f, this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Flee, false);
	}

	// Token: 0x0400042B RID: 1067
	private StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter mover;

	// Token: 0x0400042C RID: 1068
	public StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter fleeToTarget;

	// Token: 0x0400042D RID: 1069
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State plan;

	// Token: 0x0400042E RID: 1070
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x0400042F RID: 1071
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State cower;

	// Token: 0x04000430 RID: 1072
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State behaviourcomplete;

	// Token: 0x02000197 RID: 407
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000198 RID: 408
	public new class Instance : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.GameInstance
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x000AC903 File Offset: 0x000AAB03
		public Instance(Chore<FleeStates.Instance> chore, FleeStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Flee);
			base.sm.mover.Set(base.GetComponent<Navigator>(), base.smi);
		}
	}
}
