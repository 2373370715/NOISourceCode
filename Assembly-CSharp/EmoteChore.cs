using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
	// Token: 0x06001E7E RID: 7806 RVA: 0x001C04C4 File Offset: 0x001BE6C4
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, int emoteIterations = 1, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, KAnim.PlayMode.Once, emoteIterations, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x001C050C File Offset: 0x001BE70C
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, KAnim.PlayMode play_mode, int emoteIterations = 1, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, play_mode, emoteIterations, flip_x);
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x001C054C File Offset: 0x001BE74C
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, KAnim.PlayMode.Once, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x001C0594 File Offset: 0x001BE794
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, KAnim.PlayMode play_mode, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, play_mode, flip_x);
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x000B89D2 File Offset: 0x000B6BD2
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x001C05D4 File Offset: 0x001BE7D4
	public override string ToString()
	{
		if (base.smi.animFile != null)
		{
			return "EmoteChore<" + base.smi.animFile.GetData().name + ">";
		}
		string str = "EmoteChore<";
		HashedString hashedString = base.smi.emoteAnims[0];
		return str + hashedString.ToString() + ">";
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x000B89EE File Offset: 0x000B6BEE
	public void PairReactable(SelfEmoteReactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x000B89F7 File Offset: 0x000B6BF7
	protected new virtual void End(string reason)
	{
		if (this.reactable != null)
		{
			this.reactable.PairEmote(null);
			this.reactable.Cleanup();
			this.reactable = null;
		}
		base.End(reason);
	}

	// Token: 0x040013E0 RID: 5088
	private Func<StatusItem> getStatusItem;

	// Token: 0x040013E1 RID: 5089
	private SelfEmoteReactable reactable;

	// Token: 0x020006B4 RID: 1716
	public class StatesInstance : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.GameInstance
	{
		// Token: 0x06001E86 RID: 7814 RVA: 0x001C0648 File Offset: 0x001BE848
		public StatesInstance(EmoteChore master, GameObject emoter, Emote emote, KAnim.PlayMode mode, int emoteIterations, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = emote.AnimSet;
			emote.CollectStepAnims(out this.emoteAnims, emoteIterations);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x001C06A0 File Offset: 0x001BE8A0
		public StatesInstance(EmoteChore master, GameObject emoter, HashedString animFile, HashedString[] anims, KAnim.PlayMode mode, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = Assets.GetAnim(animFile);
			this.emoteAnims = anims;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x040013E2 RID: 5090
		public KAnimFile animFile;

		// Token: 0x040013E3 RID: 5091
		public HashedString[] emoteAnims;

		// Token: 0x040013E4 RID: 5092
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x020006B5 RID: 1717
	public class States : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore>
	{
		// Token: 0x06001E88 RID: 7816 RVA: 0x001C06F0 File Offset: 0x001BE8F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((EmoteChore.StatesInstance smi) => smi.animFile).PlayAnims((EmoteChore.StatesInstance smi) => smi.emoteAnims, (EmoteChore.StatesInstance smi) => smi.mode).ScheduleGoTo(10f, this.finish).OnAnimQueueComplete(this.finish);
			this.finish.ReturnSuccess();
		}

		// Token: 0x040013E5 RID: 5093
		public StateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.TargetParameter emoter;

		// Token: 0x040013E6 RID: 5094
		public GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.State finish;
	}
}
