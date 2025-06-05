using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D51 RID: 3409
[SerializationConfig(MemberSerialization.OptIn)]
public class Desalinator : StateMachineComponent<Desalinator.StatesInstance>
{
	// Token: 0x1700033F RID: 831
	// (get) Token: 0x0600421C RID: 16924 RVA: 0x000CF338 File Offset: 0x000CD538
	// (set) Token: 0x0600421D RID: 16925 RVA: 0x000CF340 File Offset: 0x000CD540
	public float SaltStorageLeft
	{
		get
		{
			return this._storageLeft;
		}
		set
		{
			this._storageLeft = value;
			base.smi.sm.saltStorageLeft.Set(value, base.smi, false);
		}
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x0024E2C0 File Offset: 0x0024C4C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<Desalinator>(-2094018600, Desalinator.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x0024E314 File Offset: 0x0024C514
	private void OnConduitConnectionChanged(object data)
	{
		bool pause = (bool)data;
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(pause, "pipe connected");
			}
		}
	}

	// Token: 0x06004220 RID: 16928 RVA: 0x0024E368 File Offset: 0x0024C568
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.HasSalt || base.smi.emptyChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_desalinator_needs_emptying", UI.USERMENUACTIONS.EMPTYDESALINATOR.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyEmpty);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0024E3FC File Offset: 0x0024C5FC
	private bool CheckCanConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].CanConvertAtAll())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x0024E444 File Offset: 0x0024C644
	private bool CheckEnoughMassToConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].HasEnoughMassToStartConverting(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002D92 RID: 11666
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x04002D93 RID: 11667
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002D94 RID: 11668
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x04002D95 RID: 11669
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002D96 RID: 11670
	[Serialize]
	public float maxSalt = 1000f;

	// Token: 0x04002D97 RID: 11671
	[Serialize]
	private float _storageLeft = 1000f;

	// Token: 0x04002D98 RID: 11672
	private ElementConverter[] converters;

	// Token: 0x04002D99 RID: 11673
	private static readonly EventSystem.IntraObjectHandler<Desalinator> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<Desalinator>(delegate(Desalinator component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x02000D52 RID: 3410
	public class StatesInstance : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.GameInstance
	{
		// Token: 0x06004226 RID: 16934 RVA: 0x000CF3BE File Offset: 0x000CD5BE
		public StatesInstance(Desalinator smi) : base(smi)
		{
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06004227 RID: 16935 RVA: 0x000CF3C7 File Offset: 0x000CD5C7
		public bool HasSalt
		{
			get
			{
				return base.master.storage.Has(ElementLoader.FindElementByHash(SimHashes.Salt).tag);
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000CF3E8 File Offset: 0x000CD5E8
		public bool IsFull()
		{
			return base.master.SaltStorageLeft <= 0f;
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x000CF3FF File Offset: 0x000CD5FF
		public bool IsSaltRemoved()
		{
			return !this.HasSalt;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x0024E48C File Offset: 0x0024C68C
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			DesalinatorWorkableEmpty component = base.master.GetComponent<DesalinatorWorkableEmpty>();
			this.emptyChore = new WorkChore<DesalinatorWorkableEmpty>(Db.Get().ChoreTypes.EmptyDesalinator, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.smi.master.remoteChore.SetChore(this.emptyChore);
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x000CF40A File Offset: 0x000CD60A
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
				base.smi.master.remoteChore.SetChore(null);
			}
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x0024E510 File Offset: 0x0024C710
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			ListPool<GameObject, Desalinator>.PooledList pooledList = ListPool<GameObject, Desalinator>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x0024E59C File Offset: 0x0024C79C
		public void UpdateStorageLeft()
		{
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			base.master.SaltStorageLeft = base.master.maxSalt - base.master.storage.GetMassAvailable(tag);
		}

		// Token: 0x04002D9A RID: 11674
		public Chore emptyChore;
	}

	// Token: 0x02000D53 RID: 3411
	public class States : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator>
	{
		// Token: 0x0600422E RID: 16942 RVA: 0x0024E5DC File Offset: 0x0024C7DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (Desalinator.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (Desalinator.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (Desalinator.StatesInstance smi) => smi.master.CheckEnoughMassToConvert());
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (Desalinator.StatesInstance smi) => !smi.master.CheckCanConvert()).ParamTransition<float>(this.saltStorageLeft, this.full, (Desalinator.StatesInstance smi, float p) => smi.IsFull()).EventHandler(GameHashes.OnStorageChange, delegate(Desalinator.StatesInstance smi)
			{
				smi.UpdateStorageLeft();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
			this.earlyEmpty.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForEmpty);
			this.earlyWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.full.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForEmpty);
			this.fullWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.DesalinatorNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.empty.PlayAnim("off").Enter("ResetStorage", delegate(Desalinator.StatesInstance smi)
			{
				smi.master.SaltStorageLeft = smi.master.maxSalt;
			}).GoTo(this.on.waiting);
		}

		// Token: 0x04002D9B RID: 11675
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State off;

		// Token: 0x04002D9C RID: 11676
		public Desalinator.States.OnStates on;

		// Token: 0x04002D9D RID: 11677
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State full;

		// Token: 0x04002D9E RID: 11678
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State fullWaitingForEmpty;

		// Token: 0x04002D9F RID: 11679
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyEmpty;

		// Token: 0x04002DA0 RID: 11680
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyWaitingForEmpty;

		// Token: 0x04002DA1 RID: 11681
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State empty;

		// Token: 0x04002DA2 RID: 11682
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"working_pst",
			"off"
		};

		// Token: 0x04002DA3 RID: 11683
		public StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter saltStorageLeft = new StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter(0f);

		// Token: 0x02000D54 RID: 3412
		public class OnStates : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State
		{
			// Token: 0x04002DA4 RID: 11684
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State waiting;

			// Token: 0x04002DA5 RID: 11685
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pre;

			// Token: 0x04002DA6 RID: 11686
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working;

			// Token: 0x04002DA7 RID: 11687
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pst;
		}
	}
}
