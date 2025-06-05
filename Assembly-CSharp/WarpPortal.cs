using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001075 RID: 4213
public class WarpPortal : Workable
{
	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x0600558F RID: 21903 RVA: 0x000DC2DD File Offset: 0x000DA4DD
	public bool ReadyToWarp
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied.waiting);
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06005590 RID: 21904 RVA: 0x000DC2FF File Offset: 0x000DA4FF
	public bool IsWorking
	{
		get
		{
			return this.warpPortalSMI.IsInsideState(this.warpPortalSMI.sm.occupied);
		}
	}

	// Token: 0x06005591 RID: 21905 RVA: 0x000DC31C File Offset: 0x000DA51C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
	}

	// Token: 0x06005592 RID: 21906 RVA: 0x0028D5A4 File Offset: 0x0028B7A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpPortalSMI = new WarpPortal.WarpPortalSM.Instance(this);
		this.warpPortalSMI.sm.isCharged.Set(!this.IsConsumed, this.warpPortalSMI, false);
		this.warpPortalSMI.StartSM();
		this.selectEventHandle = Game.Instance.Subscribe(-1503271301, new Action<object>(this.OnObjectSelected));
	}

	// Token: 0x06005593 RID: 21907 RVA: 0x000DC33B File Offset: 0x000DA53B
	private void OnObjectSelected(object data)
	{
		if (data != null && (GameObject)data == base.gameObject && Components.LiveMinionIdentities.Count > 0)
		{
			this.Discover();
		}
	}

	// Token: 0x06005594 RID: 21908 RVA: 0x000DC366 File Offset: 0x000DA566
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(this.selectEventHandle);
		base.OnCleanUp();
	}

	// Token: 0x06005595 RID: 21909 RVA: 0x0028D618 File Offset: 0x0028B818
	private void Discover()
	{
		if (this.discovered)
		{
			return;
		}
		ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).SetDiscovered(true);
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.minions = new GameObject[]
		{
			Components.LiveMinionIdentities[0].gameObject
		};
		statesInstance.callback = delegate()
		{
			ManagementMenu.Instance.OpenClusterMap();
			ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(this.GetTargetWorldID()).GetMyWorldLocation(), 0.5f);
		};
		statesInstance.ShowEventPopup();
		this.discovered = true;
	}

	// Token: 0x06005596 RID: 21910 RVA: 0x000DC37E File Offset: 0x000DA57E
	public void StartWarpSequence()
	{
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.occupied.warping);
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x000DC3A0 File Offset: 0x000DA5A0
	public void CancelAssignment()
	{
		this.CancelChore();
		this.assignable.Unassign();
		this.warpPortalSMI.GoTo(this.warpPortalSMI.sm.idle);
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x0028D6A8 File Offset: 0x0028B8A8
	private int GetTargetWorldID()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
		foreach (WarpReceiver component in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				return component.GetMyWorldId();
			}
		}
		global::Debug.LogError("No receiver world found for warp portal sender");
		return -1;
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x0028D704 File Offset: 0x0028B904
	private void Warp()
	{
		if (base.worker == null || base.worker.HasTag(GameTags.Dying) || base.worker.HasTag(GameTags.Dead))
		{
			return;
		}
		WarpReceiver warpReceiver = null;
		foreach (WarpReceiver warpReceiver2 in UnityEngine.Object.FindObjectsOfType<WarpReceiver>())
		{
			if (warpReceiver2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpReceiver = warpReceiver2;
				break;
			}
		}
		if (warpReceiver == null)
		{
			SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
			warpReceiver = UnityEngine.Object.FindObjectOfType<WarpReceiver>();
		}
		if (warpReceiver != null)
		{
			this.delayWarpRoutine = base.StartCoroutine(this.DelayedWarp(warpReceiver));
		}
		else
		{
			global::Debug.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
		}
		if (SelectTool.Instance.selected == base.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x000DC3CE File Offset: 0x000DA5CE
	public IEnumerator DelayedWarp(WarpReceiver receiver)
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		int myWorldId = base.worker.GetMyWorldId();
		int myWorldId2 = receiver.GetMyWorldId();
		GameUtil.FocusCameraOnWorld(myWorldId2, Grid.CellToPos(Grid.PosToCell(receiver)), 10f, null, true);
		WorkerBase worker = base.worker;
		worker.StopWork();
		receiver.ReceiveWarpedDuplicant(worker);
		ClusterManager.Instance.MigrateMinion(worker.GetComponent<MinionIdentity>(), myWorldId2, myWorldId);
		this.delayWarpRoutine = null;
		yield break;
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x000DC3E4 File Offset: 0x000DA5E4
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x000DC3F8 File Offset: 0x000DA5F8
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x0028D7E0 File Offset: 0x0028B9E0
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Migrate, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_warp_portal_sender_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.workLayer = Grid.SceneLayer.Building;
		this.workAnims = new HashedString[]
		{
			"sending_pre",
			"sending_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"sending_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"idle_loop"
		};
		this.showProgressBar = false;
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x000DC409 File Offset: 0x000DA609
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
		if (this.delayWarpRoutine != null)
		{
			base.StopCoroutine(this.delayWarpRoutine);
			this.delayWarpRoutine = null;
		}
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x000DC446 File Offset: 0x000DA646
	private void CompleteChore()
	{
		this.IsConsumed = true;
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x000D0F1E File Offset: 0x000CF11E
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x04003C90 RID: 15504
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x04003C91 RID: 15505
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04003C92 RID: 15506
	private Chore chore;

	// Token: 0x04003C93 RID: 15507
	private WarpPortal.WarpPortalSM.Instance warpPortalSMI;

	// Token: 0x04003C94 RID: 15508
	private Notification notification;

	// Token: 0x04003C95 RID: 15509
	public const float RECHARGE_TIME = 3000f;

	// Token: 0x04003C96 RID: 15510
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04003C97 RID: 15511
	[Serialize]
	public float rechargeProgress;

	// Token: 0x04003C98 RID: 15512
	[Serialize]
	private bool discovered;

	// Token: 0x04003C99 RID: 15513
	private int selectEventHandle = -1;

	// Token: 0x04003C9A RID: 15514
	private Coroutine delayWarpRoutine;

	// Token: 0x04003C9B RID: 15515
	private static readonly HashedString[] printing_anim = new HashedString[]
	{
		"printing_pre",
		"printing_loop",
		"printing_pst"
	};

	// Token: 0x02001076 RID: 4214
	public class WarpPortalSM : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal>
	{
		// Token: 0x060055A5 RID: 21925 RVA: 0x0028D8C4 File Offset: 0x0028BAC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				if (smi.master.rechargeProgress != 0f)
				{
					smi.GoTo(this.recharging);
				}
			}).DefaultState(this.idle);
			this.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.IsConsumed = false;
				smi.sm.isCharged.Set(true, smi, false);
				smi.master.SetAssignable(true);
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((WarpPortal.WarpPortalSM.Instance smi) => smi.master, this.become_occupied).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.IsFalse);
			this.become_occupied.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(smi.master.worker, smi);
				smi.GoTo(this.occupied.get_on);
			});
			this.occupied.OnTargetLost(this.worker, this.idle).Target(this.worker).TagTransition(GameTags.Dying, this.idle, false).Target(this.masterTarget).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				this.worker.Set(null, smi);
			});
			this.occupied.get_on.PlayAnim("sending_pre").OnAnimQueueComplete(this.occupied.waiting);
			this.occupied.waiting.PlayAnim("sending_loop", KAnim.PlayMode.Loop).ToggleNotification((WarpPortal.WarpPortalSM.Instance smi) => smi.CreateDupeWaitingNotification()).Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).Exit(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			});
			this.occupied.warping.PlayAnim("sending_pst").OnAnimQueueComplete(this.do_warp);
			this.do_warp.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.Warp();
			}).GoTo(this.recharging);
			this.recharging.Enter(delegate(WarpPortal.WarpPortalSM.Instance smi)
			{
				smi.master.SetAssignable(false);
				smi.master.IsConsumed = true;
				this.isCharged.Set(false, smi, false);
			}).PlayAnim("recharge", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.WarpPortalCharging, (WarpPortal.WarpPortalSM.Instance smi) => smi.master).Update(delegate(WarpPortal.WarpPortalSM.Instance smi, float dt)
			{
				smi.master.rechargeProgress += dt;
				if (smi.master.rechargeProgress > 3000f)
				{
					this.isCharged.Set(true, smi, false);
					smi.master.rechargeProgress = 0f;
					smi.GoTo(this.idle);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x04003C9C RID: 15516
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State idle;

		// Token: 0x04003C9D RID: 15517
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State become_occupied;

		// Token: 0x04003C9E RID: 15518
		public WarpPortal.WarpPortalSM.OccupiedStates occupied;

		// Token: 0x04003C9F RID: 15519
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State do_warp;

		// Token: 0x04003CA0 RID: 15520
		public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State recharging;

		// Token: 0x04003CA1 RID: 15521
		public StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.BoolParameter isCharged;

		// Token: 0x04003CA2 RID: 15522
		private StateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.TargetParameter worker;

		// Token: 0x02001077 RID: 4215
		public class OccupiedStates : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State
		{
			// Token: 0x04003CA3 RID: 15523
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State get_on;

			// Token: 0x04003CA4 RID: 15524
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State waiting;

			// Token: 0x04003CA5 RID: 15525
			public GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.State warping;
		}

		// Token: 0x02001078 RID: 4216
		public new class Instance : GameStateMachine<WarpPortal.WarpPortalSM, WarpPortal.WarpPortalSM.Instance, WarpPortal, object>.GameInstance
		{
			// Token: 0x060055AD RID: 21933 RVA: 0x000DC57A File Offset: 0x000DA77A
			public Instance(WarpPortal master) : base(master)
			{
			}

			// Token: 0x060055AE RID: 21934 RVA: 0x0028DBC8 File Offset: 0x0028BDC8
			public Notification CreateDupeWaitingNotification()
			{
				if (base.master.worker != null)
				{
					return new Notification(MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.NAME.Replace("{dupe}", base.master.worker.name), NotificationType.Neutral, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.WARP_PORTAL_DUPE_READY.TOOLTIP.Replace("{dupe}", base.master.worker.name), null, false, 0f, null, null, base.master.transform, true, false, false);
				}
				return null;
			}
		}
	}
}
