using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
	// Token: 0x060020F9 RID: 8441 RVA: 0x001CA888 File Offset: 0x001C8A88
	public StressEmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		this.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000BA0A7 File Offset: 0x000B82A7
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x001CA914 File Offset: 0x001C8B14
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "StressEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "StressEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04001609 RID: 5641
	private Func<StatusItem> getStatusItem;

	// Token: 0x02000756 RID: 1878
	public class StatesInstance : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
	{
		// Token: 0x060020FC RID: 8444 RVA: 0x000BA0C3 File Offset: 0x000B82C3
		public StatesInstance(StressEmoteChore master, GameObject emoter, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x0400160A RID: 5642
		public HashedString[] emoteAnims;

		// Token: 0x0400160B RID: 5643
		public HashedString emoteKAnim;

		// Token: 0x0400160C RID: 5644
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02000757 RID: 1879
	public class States : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
	{
		// Token: 0x060020FD RID: 8445 RVA: 0x001CA98C File Offset: 0x001C8B8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((StressEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((StressEmoteChore.StatesInstance smi) => smi.emoteAnims, (StressEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null);
		}

		// Token: 0x0400160D RID: 5645
		public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;
	}
}
