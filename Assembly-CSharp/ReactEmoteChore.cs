using System;
using UnityEngine;

// Token: 0x02000717 RID: 1815
public class ReactEmoteChore : Chore<ReactEmoteChore.StatesInstance>
{
	// Token: 0x06001FFE RID: 8190 RVA: 0x001C6DD0 File Offset: 0x001C4FD0
	public ReactEmoteChore(IStateMachineTarget target, ChoreType chore_type, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new ReactEmoteChore.StatesInstance(this, target.gameObject, reactable, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000B9720 File Offset: 0x000B7920
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x001C6E5C File Offset: 0x001C505C
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "ReactEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "ReactEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04001535 RID: 5429
	private Func<StatusItem> getStatusItem;

	// Token: 0x02000718 RID: 1816
	public class StatesInstance : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.GameInstance
	{
		// Token: 0x06002001 RID: 8193 RVA: 0x001C6ED4 File Offset: 0x001C50D4
		public StatesInstance(ReactEmoteChore master, GameObject emoter, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04001536 RID: 5430
		public HashedString[] emoteAnims;

		// Token: 0x04001537 RID: 5431
		public HashedString emoteKAnim;

		// Token: 0x04001538 RID: 5432
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02000719 RID: 1817
	public class States : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore>
	{
		// Token: 0x06002002 RID: 8194 RVA: 0x001C6F3C File Offset: 0x001C513C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleThought((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).thought).ToggleExpression((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).expression).ToggleAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteAnims, (ReactEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null).Enter(delegate(ReactEmoteChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.reactable.Get(smi).sourceCell));
			});
		}

		// Token: 0x04001539 RID: 5433
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.TargetParameter emoter;

		// Token: 0x0400153A RID: 5434
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.ObjectParameter<EmoteReactable> reactable;
	}
}
