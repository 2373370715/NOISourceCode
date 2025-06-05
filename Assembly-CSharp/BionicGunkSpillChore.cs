using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class BionicGunkSpillChore : Chore<BionicGunkSpillChore.StatesInstance>
{
	// Token: 0x06001DCA RID: 7626 RVA: 0x000B8236 File Offset: 0x000B6436
	public static bool HasSuit(BionicGunkSpillChore.StatesInstance smi)
	{
		return smi.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x001BC7F4 File Offset: 0x001BA9F4
	public static void ExpellGunkUpdate(BionicGunkSpillChore.StatesInstance smi, float dt)
	{
		float num = GunkMonitor.GUNK_CAPACITY * (dt / 10f);
		if (num >= smi.gunkMonitor.CurrentGunkMass)
		{
			smi.GoTo(smi.sm.pst);
			return;
		}
		smi.gunkMonitor.ExpellGunk(num, null);
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x001BC83C File Offset: 0x001BAA3C
	public BionicGunkSpillChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.ExpellGunk, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicGunkSpillChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x04001303 RID: 4867
	public const float EVENT_DURATION = 10f;

	// Token: 0x04001304 RID: 4868
	public const string PRE_ANIM_NAME = "oiloverload_pre";

	// Token: 0x04001305 RID: 4869
	public const string LOOP_ANIM_NAME = "oiloverload_loop";

	// Token: 0x04001306 RID: 4870
	public const string PST_ANIM_NAME = "overload_pst";

	// Token: 0x04001307 RID: 4871
	public const string SUIT_PRE_ANIM_NAME = "oiloverload_helmet_pre";

	// Token: 0x04001308 RID: 4872
	public const string SUIT_LOOP_ANIM_NAME = "oiloverload_helmet_loop";

	// Token: 0x04001309 RID: 4873
	public const string SUIT_PST_ANIM_NAME = "oiloverload_helmet_pst";

	// Token: 0x0200068D RID: 1677
	public class States : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore>
	{
		// Token: 0x06001DCD RID: 7629 RVA: 0x001BC884 File Offset: 0x001BAA84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			base.Target(this.worker);
			this.root.ToggleAnims("anim_bionic_oil_overload_kanim", 0f).ToggleEffect("ExpellingGunk").ToggleTag(GameTags.MakingMess).DoNotification((BionicGunkSpillChore.StatesInstance smi) => smi.stressfullyEmptyingGunk).Enter(delegate(BionicGunkSpillChore.StatesInstance smi)
			{
				if (Sim.IsRadiationEnabled() && smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			});
			this.enter.DefaultState(this.enter.noSuit);
			this.enter.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.enter.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.enter.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.enter.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.running);
			this.running.DefaultState(this.running.noSuit).Update(new Action<BionicGunkSpillChore.StatesInstance, float>(BionicGunkSpillChore.ExpellGunkUpdate), UpdateRate.SIM_200ms, false);
			this.running.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.running.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("oiloverload_loop", KAnim.PlayMode.Loop);
			this.running.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.running.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_loop", KAnim.PlayMode.Loop);
			this.pst.DefaultState(this.pst.noSuit);
			this.pst.noSuit.EventTransition(GameHashes.EquippedItemEquipper, this.pst.suit, new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit)).PlayAnim("overload_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.pst.suit.EventTransition(GameHashes.UnequippedItemEquipper, this.pst.noSuit, GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Not(new StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.Transition.ConditionCallback(BionicGunkSpillChore.HasSuit))).PlayAnim("oiloverload_helmet_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x0400130A RID: 4874
		public BionicGunkSpillChore.States.SuitAnimState enter;

		// Token: 0x0400130B RID: 4875
		public BionicGunkSpillChore.States.SuitAnimState running;

		// Token: 0x0400130C RID: 4876
		public BionicGunkSpillChore.States.SuitAnimState pst;

		// Token: 0x0400130D RID: 4877
		public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State complete;

		// Token: 0x0400130E RID: 4878
		public StateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.TargetParameter worker;

		// Token: 0x0200068E RID: 1678
		public class SuitAnimState : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State
		{
			// Token: 0x0400130F RID: 4879
			public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State noSuit;

			// Token: 0x04001310 RID: 4880
			public GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.State suit;
		}
	}

	// Token: 0x02000690 RID: 1680
	public class StatesInstance : GameStateMachine<BionicGunkSpillChore.States, BionicGunkSpillChore.StatesInstance, BionicGunkSpillChore, object>.GameInstance
	{
		// Token: 0x06001DD4 RID: 7636 RVA: 0x001BCB74 File Offset: 0x001BAD74
		public StatesInstance(BionicGunkSpillChore master, GameObject worker) : base(master)
		{
			this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
			base.sm.worker.Set(worker, base.smi, false);
		}

		// Token: 0x04001314 RID: 4884
		public Notification stressfullyEmptyingGunk = new Notification(DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.STRESSFULLYEMPTYINGOIL.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);

		// Token: 0x04001315 RID: 4885
		public GunkMonitor.Instance gunkMonitor;
	}
}
