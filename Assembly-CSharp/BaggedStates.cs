using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class BaggedStates : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>
{
	// Token: 0x06000424 RID: 1060 RVA: 0x0015E1B4 File Offset: 0x0015C3B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.bagged;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.BAGGED.NAME;
		string tooltip = CREATURES.STATUSITEMS.BAGGED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.bagged.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagStart)).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(new Func<BaggedStates.Instance, string>(BaggedStates.GetBaggedAnimName), KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Bagged, null, true).Transition(this.escape, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.Transition.ConditionCallback(BaggedStates.ShouldEscape), UpdateRate.SIM_4000ms).EventHandler(GameHashes.OnStore, new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.OnStore)).Exit(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.BagEnd));
		this.escape.Enter(new StateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State.Callback(BaggedStates.Unbag)).PlayAnim("escape").OnAnimQueueComplete(null);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000AB6FB File Offset: 0x000A98FB
	public static string GetBaggedAnimName(BaggedStates.Instance smi)
	{
		return Baggable.GetBaggedAnimName(smi.gameObject);
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000AB708 File Offset: 0x000A9908
	private static void BagStart(BaggedStates.Instance smi)
	{
		if (smi.baggedTime == 0f)
		{
			smi.baggedTime = GameClock.Instance.GetTime();
		}
		smi.UpdateFaller(true);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x000AB72E File Offset: 0x000A992E
	private static void BagEnd(BaggedStates.Instance smi)
	{
		smi.baggedTime = 0f;
		smi.UpdateFaller(false);
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0015E2C0 File Offset: 0x0015C4C0
	private static void Unbag(BaggedStates.Instance smi)
	{
		Baggable component = smi.gameObject.GetComponent<Baggable>();
		if (component)
		{
			component.Free();
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000AB742 File Offset: 0x000A9942
	private static void OnStore(BaggedStates.Instance smi)
	{
		smi.UpdateFaller(true);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000AB74B File Offset: 0x000A994B
	private static bool ShouldEscape(BaggedStates.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Stored) && GameClock.Instance.GetTime() - smi.baggedTime >= smi.def.escapeTime;
	}

	// Token: 0x040002F9 RID: 761
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State bagged;

	// Token: 0x040002FA RID: 762
	public GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.State escape;

	// Token: 0x02000110 RID: 272
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040002FB RID: 763
		public float escapeTime = 300f;
	}

	// Token: 0x02000111 RID: 273
	public new class Instance : GameStateMachine<BaggedStates, BaggedStates.Instance, IStateMachineTarget, BaggedStates.Def>.GameInstance
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x000AB79D File Offset: 0x000A999D
		public Instance(Chore<BaggedStates.Instance> chore, BaggedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(BaggedStates.Instance.IsBagged, null);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0015E2E8 File Offset: 0x0015C4E8
		public void UpdateFaller(bool bagged)
		{
			bool flag = bagged && !base.gameObject.HasTag(GameTags.Stored);
			bool flag2 = GameComps.Fallers.Has(base.gameObject);
			if (flag != flag2)
			{
				if (flag)
				{
					GameComps.Fallers.Add(base.gameObject, Vector2.zero);
					return;
				}
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x040002FC RID: 764
		[Serialize]
		public float baggedTime;

		// Token: 0x040002FD RID: 765
		public static readonly Chore.Precondition IsBagged = new Chore.Precondition
		{
			id = "IsBagged",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Bagged);
			}
		};
	}
}
