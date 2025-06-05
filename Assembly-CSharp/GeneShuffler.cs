using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000DC9 RID: 3529
[AddComponentMenu("KMonoBehaviour/Workable/GeneShuffler")]
public class GeneShuffler : Workable
{
	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060044BC RID: 17596 RVA: 0x000D0D6C File Offset: 0x000CEF6C
	public bool WorkComplete
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working.complete);
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060044BD RID: 17597 RVA: 0x000D0D8E File Offset: 0x000CEF8E
	public bool IsWorking
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working);
		}
	}

	// Token: 0x060044BE RID: 17598 RVA: 0x000D0DAB File Offset: 0x000CEFAB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x060044BF RID: 17599 RVA: 0x002573E8 File Offset: 0x002555E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showProgressBar = false;
		this.geneShufflerSMI = new GeneShuffler.GeneShufflerSM.Instance(this);
		this.RefreshRechargeChore();
		this.RefreshConsumedState();
		base.Subscribe<GeneShuffler>(-1697596308, GeneShuffler.OnStorageChangeDelegate);
		this.geneShufflerSMI.StartSM();
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x000D0DD1 File Offset: 0x000CEFD1
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x000D0DE2 File Offset: 0x000CEFE2
	private void Recharge()
	{
		this.SetConsumed(false);
		this.RequestRecharge(false);
		this.RefreshRechargeChore();
		this.RefreshSideScreen();
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x000D0DFE File Offset: 0x000CEFFE
	private void SetConsumed(bool consumed)
	{
		this.IsConsumed = consumed;
		this.RefreshConsumedState();
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x000D0E0D File Offset: 0x000CF00D
	private void RefreshConsumedState()
	{
		this.geneShufflerSMI.sm.isCharged.Set(!this.IsConsumed, this.geneShufflerSMI, false);
	}

	// Token: 0x060044C4 RID: 17604 RVA: 0x00257438 File Offset: 0x00255638
	private void OnStorageChange(object data)
	{
		if (this.storage_recursion_guard)
		{
			return;
		}
		this.storage_recursion_guard = true;
		if (this.IsConsumed)
		{
			for (int i = this.storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.storage.items[i];
				if (!(gameObject == null) && gameObject.IsPrefabID(GeneShuffler.RechargeTag))
				{
					this.storage.ConsumeIgnoringDisease(gameObject);
					this.Recharge();
					break;
				}
			}
		}
		this.storage_recursion_guard = false;
	}

	// Token: 0x060044C5 RID: 17605 RVA: 0x002574C0 File Offset: 0x002556C0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.notification = new Notification(MISC.NOTIFICATIONS.GENESHUFFLER.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.GENESHUFFLER.TOOLTIP + notificationList.ReduceMessages(false), null, false, 0f, null, null, null, true, false, false);
		this.notifier.Add(this.notification, "");
		this.DeSelectBuilding();
	}

	// Token: 0x060044C6 RID: 17606 RVA: 0x000D0E35 File Offset: 0x000CF035
	private void DeSelectBuilding()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x060044C7 RID: 17607 RVA: 0x000D0E50 File Offset: 0x000CF050
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060044C8 RID: 17608 RVA: 0x000D0E5A File Offset: 0x000CF05A
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("aborted");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060044C9 RID: 17609 RVA: 0x000D0E8C File Offset: 0x000CF08C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("stopped");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060044CA RID: 17610 RVA: 0x00257534 File Offset: 0x00255734
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		CameraController.Instance.CameraGoTo(base.transform.GetPosition(), 1f, false);
		this.ApplyRandomTrait(worker);
		this.assignable.Unassign();
		this.DeSelectBuilding();
		this.notifier.Remove(this.notification);
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x0025758C File Offset: 0x0025578C
	private void ApplyRandomTrait(WorkerBase worker)
	{
		Traits component = worker.GetComponent<Traits>();
		List<string> list = new List<string>();
		foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
		{
			if (!component.HasTrait(traitVal.id))
			{
				list.Add(traitVal.id);
			}
		}
		if (list.Count > 0)
		{
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			Trait trait = Db.Get().traits.TryGet(id);
			worker.GetComponent<Traits>().Add(trait);
			InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			string text = string.Format(UI.GENESHUFFLERMESSAGE.BODY_SUCCESS, worker.GetProperName(), trait.GetName(), trait.GetTooltip());
			infoDialogScreen.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text).AddDefaultOK(false);
			this.SetConsumed(true);
			return;
		}
		InfoDialogScreen infoDialogScreen2 = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		string text2 = string.Format(UI.GENESHUFFLERMESSAGE.BODY_FAILURE, worker.GetProperName());
		infoDialogScreen2.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text2).AddDefaultOK(false);
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x0025771C File Offset: 0x0025591C
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		base.GetComponent<Workable>().SetWorkTime(float.PositiveInfinity);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.GeneShuffle, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_neuralvacillator_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x000D0EBE File Offset: 0x000CF0BE
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x000D0EE0 File Offset: 0x000CF0E0
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x000D0EF4 File Offset: 0x000CF0F4
	public void RequestRecharge(bool request)
	{
		this.RechargeRequested = request;
		this.RefreshRechargeChore();
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x000D0F03 File Offset: 0x000CF103
	private void RefreshRechargeChore()
	{
		this.delivery.Pause(!this.RechargeRequested, "No recharge requested");
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x000D0F1E File Offset: 0x000CF11E
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x000D0F3D File Offset: 0x000CF13D
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x04002FB4 RID: 12212
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04002FB5 RID: 12213
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04002FB6 RID: 12214
	[MyCmpReq]
	public ManualDeliveryKG delivery;

	// Token: 0x04002FB7 RID: 12215
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04002FB8 RID: 12216
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04002FB9 RID: 12217
	[Serialize]
	public bool RechargeRequested;

	// Token: 0x04002FBA RID: 12218
	private Chore chore;

	// Token: 0x04002FBB RID: 12219
	private GeneShuffler.GeneShufflerSM.Instance geneShufflerSMI;

	// Token: 0x04002FBC RID: 12220
	private Notification notification;

	// Token: 0x04002FBD RID: 12221
	private static Tag RechargeTag = new Tag("GeneShufflerRecharge");

	// Token: 0x04002FBE RID: 12222
	private static readonly EventSystem.IntraObjectHandler<GeneShuffler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<GeneShuffler>(delegate(GeneShuffler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002FBF RID: 12223
	private bool storage_recursion_guard;

	// Token: 0x02000DCA RID: 3530
	public class GeneShufflerSM : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler>
	{
		// Token: 0x060044D6 RID: 17622 RVA: 0x0025778C File Offset: 0x0025598C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("on").Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(true);
			}).Exit(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pre).ParamTransition<bool>(this.isCharged, this.consumed, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsFalse);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.working.complete);
			this.working.complete.ToggleStatusItem(Db.Get().BuildingStatusItems.GeneShuffleCompleted, null).Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).WorkableStopTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pst);
			this.working.pst.OnAnimQueueComplete(this.consumed);
			this.consumed.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsTrue);
			this.recharging.PlayAnim("recharging", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04002FC0 RID: 12224
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State idle;

		// Token: 0x04002FC1 RID: 12225
		public GeneShuffler.GeneShufflerSM.WorkingStates working;

		// Token: 0x04002FC2 RID: 12226
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State consumed;

		// Token: 0x04002FC3 RID: 12227
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State recharging;

		// Token: 0x04002FC4 RID: 12228
		public StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.BoolParameter isCharged;

		// Token: 0x02000DCB RID: 3531
		public class WorkingStates : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State
		{
			// Token: 0x04002FC5 RID: 12229
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pre;

			// Token: 0x04002FC6 RID: 12230
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State loop;

			// Token: 0x04002FC7 RID: 12231
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State complete;

			// Token: 0x04002FC8 RID: 12232
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pst;
		}

		// Token: 0x02000DCC RID: 3532
		public new class Instance : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.GameInstance
		{
			// Token: 0x060044D9 RID: 17625 RVA: 0x000D0F94 File Offset: 0x000CF194
			public Instance(GeneShuffler master) : base(master)
			{
			}
		}
	}
}
