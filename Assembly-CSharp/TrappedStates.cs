using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class TrappedStates : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
	// Token: 0x06000740 RID: 1856 RVA: 0x00167A40 File Offset: 0x00165C40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.trapped;
		GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.TRAPPED.NAME;
		string tooltip = CREATURES.STATUSITEMS.TRAPPED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.trapped.Enter(delegate(TrappedStates.Instance smi)
		{
			Navigator component = smi.GetComponent<Navigator>();
			if (component.IsValidNavType(NavType.Floor))
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<TrappedStates.Instance, string>(TrappedStates.GetTrappedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Trapped, null, true);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00167AF4 File Offset: 0x00165CF4
	public static string GetTrappedAnimName(TrappedStates.Instance smi)
	{
		string result = "trapped";
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Pickupable component = smi.gameObject.GetComponent<Pickupable>();
		GameObject gameObject = (component != null) ? component.storage.gameObject : Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			if (gameObject.GetComponent<TrappedStates.ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName = gameObject.GetComponent<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName != null)
				{
					return trappedAnimationName;
				}
			}
			if (gameObject.GetSMI<TrappedStates.ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName2 = gameObject.GetSMI<TrappedStates.ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName2 != null)
				{
					return trappedAnimationName2;
				}
			}
		}
		Trappable component2 = smi.gameObject.GetComponent<Trappable>();
		if (component2 != null && component2.HasTag(GameTags.Creatures.Swimmer) && Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
		{
			result = "trapped_onLand";
		}
		return result;
	}

	// Token: 0x04000566 RID: 1382
	public const string DEFAULT_TRAPPED_ANIM_NAME = "trapped";

	// Token: 0x04000567 RID: 1383
	private GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.State trapped;

	// Token: 0x02000216 RID: 534
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000217 RID: 535
	public interface ITrapStateAnimationInstructions
	{
		// Token: 0x06000744 RID: 1860
		string GetTrappedAnimationName();
	}

	// Token: 0x02000218 RID: 536
	public new class Instance : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>.GameInstance
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x000AD9DD File Offset: 0x000ABBDD
		public Instance(Chore<TrappedStates.Instance> chore, TrappedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(TrappedStates.Instance.IsTrapped, null);
		}

		// Token: 0x04000568 RID: 1384
		public static readonly Chore.Precondition IsTrapped = new Chore.Precondition
		{
			id = "IsTrapped",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Trapped);
			}
		};
	}
}
