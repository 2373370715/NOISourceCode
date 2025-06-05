using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class IncubatingStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>
{
	// Token: 0x06000657 RID: 1623 RVA: 0x00164338 File Offset: 0x00162538
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubator;
		GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
		string tooltip = CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.incubator.DefaultState(this.incubator.idle).ToggleTag(GameTags.Creatures.Deliverable).TagTransition(GameTags.Creatures.InIncubator, null, true);
		this.incubator.idle.Enter("VariantUpdate", new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State.Callback(IncubatingStates.VariantUpdate)).PlayAnim("incubator_idle_loop").OnAnimQueueComplete(this.incubator.choose);
		this.incubator.choose.Transition(this.incubator.variant, new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant), UpdateRate.SIM_200ms).Transition(this.incubator.idle, GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Not(new StateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.Transition.ConditionCallback(IncubatingStates.DoVariant)), UpdateRate.SIM_200ms);
		this.incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(this.incubator.idle);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x000ACFE9 File Offset: 0x000AB1E9
	public static bool DoVariant(IncubatingStates.Instance smi)
	{
		return smi.variant_time == 0;
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x000ACFF4 File Offset: 0x000AB1F4
	public static void VariantUpdate(IncubatingStates.Instance smi)
	{
		if (smi.variant_time <= 0)
		{
			smi.variant_time = UnityEngine.Random.Range(3, 7);
			return;
		}
		smi.variant_time--;
	}

	// Token: 0x04000497 RID: 1175
	public IncubatingStates.IncubatorStates incubator;

	// Token: 0x020001D2 RID: 466
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001D3 RID: 467
	public new class Instance : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.GameInstance
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x000AD023 File Offset: 0x000AB223
		public Instance(Chore<IncubatingStates.Instance> chore, IncubatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(IncubatingStates.Instance.IsInIncubator, null);
		}

		// Token: 0x04000498 RID: 1176
		public int variant_time = 3;

		// Token: 0x04000499 RID: 1177
		public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition
		{
			id = "IsInIncubator",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator);
			}
		};
	}

	// Token: 0x020001D5 RID: 469
	public class IncubatorStates : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State
	{
		// Token: 0x0400049B RID: 1179
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State idle;

		// Token: 0x0400049C RID: 1180
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State choose;

		// Token: 0x0400049D RID: 1181
		public GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def>.State variant;
	}
}
