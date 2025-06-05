using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020017B3 RID: 6067
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkDock")]
public class RemoteWorkerDock : KMonoBehaviour
{
	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x06007CBD RID: 31933 RVA: 0x000F6892 File Offset: 0x000F4A92
	// (set) Token: 0x06007CBE RID: 31934 RVA: 0x000F689A File Offset: 0x000F4A9A
	public RemoteWorkerSM RemoteWorker
	{
		get
		{
			return this.remoteWorker;
		}
		private set
		{
			this.remoteWorker = value;
			this.worker = ((value != null) ? new Ref<KSelectable>(value.GetComponent<KSelectable>()) : null);
		}
	}

	// Token: 0x06007CBF RID: 31935 RVA: 0x000F68C0 File Offset: 0x000F4AC0
	public WorkerBase GetActiveTerminalWorker()
	{
		if (this.terminal == null)
		{
			return null;
		}
		return this.terminal.worker;
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x06007CC0 RID: 31936 RVA: 0x000F68DD File Offset: 0x000F4ADD
	public bool IsOperational
	{
		get
		{
			return this.operational.IsOperational;
		}
	}

	// Token: 0x06007CC1 RID: 31937 RVA: 0x0032E5E8 File Offset: 0x0032C7E8
	private bool canWork(IRemoteDockWorkTarget provider)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(provider.Approachable.GetCell(), out num3, out num4);
		return num2 == num4 && Math.Abs(num - num3) <= 12;
	}

	// Token: 0x06007CC2 RID: 31938 RVA: 0x000F68EA File Offset: 0x000F4AEA
	private void considerProvider(IRemoteDockWorkTarget provider)
	{
		if (this.canWork(provider))
		{
			this.providers.Add(provider);
		}
	}

	// Token: 0x06007CC3 RID: 31939 RVA: 0x000F6901 File Offset: 0x000F4B01
	private void forgetProvider(IRemoteDockWorkTarget provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06007CC4 RID: 31940 RVA: 0x0032E630 File Offset: 0x0032C830
	private static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 3; i++)
		{
			text += "011223345789"[UnityEngine.Random.Range(0, "011223345789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x06007CC5 RID: 31941 RVA: 0x0032E688 File Offset: 0x0032C888
	protected override void OnSpawn()
	{
		base.OnSpawn();
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME)
		{
			component.SetName(RemoteWorkerDock.GenerateName());
		}
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Components.RemoteWorkerDocks.Add(this.GetMyWorldId(), this);
		this.add_provider_binding = new Action<IRemoteDockWorkTarget>(this.considerProvider);
		this.remove_provider_binding = new Action<IRemoteDockWorkTarget>(this.forgetProvider);
		Components.RemoteDockWorkTargets.Register(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		Ref<KSelectable> @ref = this.worker;
		RemoteWorkerSM remoteWorkerSM;
		if (@ref == null)
		{
			remoteWorkerSM = null;
		}
		else
		{
			KSelectable kselectable = @ref.Get();
			remoteWorkerSM = ((kselectable != null) ? kselectable.GetComponent<RemoteWorkerSM>() : null);
		}
		this.remoteWorker = remoteWorkerSM;
		if (this.remoteWorker == null)
		{
			this.RequestNewWorker(null);
			return;
		}
		this.remoteWorkerDestroyedEventId = this.remoteWorker.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
	}

	// Token: 0x06007CC6 RID: 31942 RVA: 0x0032E79C File Offset: 0x0032C99C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteWorkerDocks.Remove(this.GetMyWorldId(), this);
		Components.RemoteDockWorkTargets.Unregister(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		if (this.remoteWorker != null)
		{
			this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
		}
		if (this.newRemoteWorkerHandle.IsValid)
		{
			this.newRemoteWorkerHandle.ClearScheduler();
		}
	}

	// Token: 0x06007CC7 RID: 31943 RVA: 0x0032E814 File Offset: 0x0032CA14
	public void CollectChores(ChoreConsumerState duplicant_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		if (this.remoteWorker == null)
		{
			return;
		}
		ChoreConsumerState consumerState = this.remoteWorker.ConsumerState;
		consumerState.resume = duplicant_state.resume;
		foreach (IRemoteDockWorkTarget remoteDockWorkTarget in this.providers)
		{
			Chore remoteDockChore = remoteDockWorkTarget.RemoteDockChore;
			if (remoteDockChore != null)
			{
				remoteDockChore.CollectChores(consumerState, succeeded_contexts, incomplete_contexts, failed_contexts, false);
			}
		}
	}

	// Token: 0x06007CC8 RID: 31944 RVA: 0x000F6910 File Offset: 0x000F4B10
	public bool AvailableForWorkBy(RemoteWorkTerminal terminal)
	{
		return this.terminal == null || this.terminal == terminal;
	}

	// Token: 0x06007CC9 RID: 31945 RVA: 0x000F692E File Offset: 0x000F4B2E
	public bool HasWorker()
	{
		return this.remoteWorker != null;
	}

	// Token: 0x06007CCA RID: 31946 RVA: 0x0032E89C File Offset: 0x0032CA9C
	public void SetNextChore(RemoteWorkTerminal terminal, Chore.Precondition.Context chore_context)
	{
		global::Debug.Assert(this.worker != null);
		global::Debug.Assert(this.terminal == null || this.terminal == terminal);
		this.terminal = terminal;
		if (this.remoteWorker != null)
		{
			this.remoteWorker.SetNextChore(chore_context);
		}
	}

	// Token: 0x06007CCB RID: 31947 RVA: 0x0032E8FC File Offset: 0x0032CAFC
	public bool StartWorking(RemoteWorkTerminal terminal)
	{
		if (this.terminal == null)
		{
			this.terminal = terminal;
		}
		if (this.terminal == terminal && this.remoteWorker != null)
		{
			this.remoteWorker.ActivelyControlled = true;
			return true;
		}
		return false;
	}

	// Token: 0x06007CCC RID: 31948 RVA: 0x000F693C File Offset: 0x000F4B3C
	public void StopWorking(RemoteWorkTerminal terminal)
	{
		if (terminal == this.terminal)
		{
			this.terminal = null;
			if (this.remoteWorker != null)
			{
				this.remoteWorker.ActivelyControlled = false;
			}
		}
	}

	// Token: 0x06007CCD RID: 31949 RVA: 0x000F696D File Offset: 0x000F4B6D
	public bool OnRemoteWorkTick(float dt)
	{
		return this.remoteWorker == null || (!this.remoteWorker.ActivelyWorking && !this.remoteWorker.HasChoreQueued());
	}

	// Token: 0x06007CCE RID: 31950 RVA: 0x000F699C File Offset: 0x000F4B9C
	private void OnStorageChanged(object _)
	{
		if (this.remoteWorker == null || this.worker.Get() == null)
		{
			this.RequestNewWorker(null);
		}
	}

	// Token: 0x06007CCF RID: 31951 RVA: 0x0032E94C File Offset: 0x0032CB4C
	private void RequestNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		Tag build_MATERIAL_TAG = RemoteWorkerConfig.BUILD_MATERIAL_TAG;
		if (this.storage.FindFirstWithMass(build_MATERIAL_TAG, 200f) == null)
		{
			if (!this.activeFetch)
			{
				this.activeFetch = true;
				FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
				fetchList.Add(build_MATERIAL_TAG, null, 200f, Operational.State.None);
				fetchList.Submit(delegate
				{
					this.activeFetch = false;
					this.RequestNewWorker(null);
				}, true);
				return;
			}
		}
		else
		{
			this.MakeNewWorker(null);
		}
	}

	// Token: 0x06007CD0 RID: 31952 RVA: 0x0032E9D8 File Offset: 0x0032CBD8
	private void MakeNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		if (this.storage.GetAmountAvailable(RemoteWorkerConfig.BUILD_MATERIAL_TAG) < 200f)
		{
			return;
		}
		PrimaryElement elem = this.storage.FindFirstWithMass(RemoteWorkerConfig.BUILD_MATERIAL_TAG, 200f);
		if (elem == null)
		{
			return;
		}
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.storage.ConsumeAndGetDisease(elem.ElementID.CreateTag(), 200f, out num, out disease, out temperature);
		this.status_item_handle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RemoteWorkDockMakingWorker, null);
		this.newRemoteWorkerHandle = GameScheduler.Instance.Schedule("MakeRemoteWorker", 2f, delegate(object _)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(RemoteWorkerConfig.ID), this.transform.position, Grid.SceneLayer.Creatures, null, 0);
			if (this.remoteWorkerDestroyedEventId != -1 && this.remoteWorker != null)
			{
				this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
			}
			this.RemoteWorker = gameObject.GetComponent<RemoteWorkerSM>();
			this.remoteWorker.HomeDepot = this;
			this.remoteWorker.playNewWorker = true;
			gameObject.SetActive(true);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = elem.ElementID;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from construction material");
			}
			this.remoteWorkerDestroyedEventId = gameObject.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
			this.newRemoteWorkerHandle.ClearScheduler();
			this.GetComponent<KSelectable>().RemoveStatusItem(this.status_item_handle, false);
		}, null, null);
	}

	// Token: 0x04005DF6 RID: 24054
	[Serialize]
	protected Ref<KSelectable> worker;

	// Token: 0x04005DF7 RID: 24055
	protected RemoteWorkerSM remoteWorker;

	// Token: 0x04005DF8 RID: 24056
	private int remoteWorkerDestroyedEventId = -1;

	// Token: 0x04005DF9 RID: 24057
	protected RemoteWorkTerminal terminal;

	// Token: 0x04005DFA RID: 24058
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005DFB RID: 24059
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005DFC RID: 24060
	[MyCmpAdd]
	private UserNameable nameable;

	// Token: 0x04005DFD RID: 24061
	[MyCmpAdd]
	private RemoteWorkerDock.NewWorker new_worker_;

	// Token: 0x04005DFE RID: 24062
	[MyCmpAdd]
	private RemoteWorkerDock.EnterableDock enter_;

	// Token: 0x04005DFF RID: 24063
	[MyCmpAdd]
	private RemoteWorkerDock.ExitableDock exit_;

	// Token: 0x04005E00 RID: 24064
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerRecharger recharger_;

	// Token: 0x04005E01 RID: 24065
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerGunkRemover gunk_remover_;

	// Token: 0x04005E02 RID: 24066
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerOilRefiller oil_refiller_;

	// Token: 0x04005E03 RID: 24067
	private Guid status_item_handle;

	// Token: 0x04005E04 RID: 24068
	private SchedulerHandle newRemoteWorkerHandle;

	// Token: 0x04005E05 RID: 24069
	private List<IRemoteDockWorkTarget> providers = new List<IRemoteDockWorkTarget>();

	// Token: 0x04005E06 RID: 24070
	private Action<IRemoteDockWorkTarget> add_provider_binding;

	// Token: 0x04005E07 RID: 24071
	private Action<IRemoteDockWorkTarget> remove_provider_binding;

	// Token: 0x04005E08 RID: 24072
	private bool activeFetch;

	// Token: 0x020017B4 RID: 6068
	public class NewWorker : Workable
	{
		// Token: 0x06007CD3 RID: 31955 RVA: 0x0032EABC File Offset: 0x0032CCBC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.NewWorker.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("new_worker");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x06007CD4 RID: 31956 RVA: 0x000AF929 File Offset: 0x000ADB29
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x000F69F0 File Offset: 0x000F4BF0
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			worker.GetComponent<RemoteWorkerSM>().Docked = true;
		}

		// Token: 0x04005E09 RID: 24073
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"new_worker"
		};
	}

	// Token: 0x020017B5 RID: 6069
	public class EnterableDock : Workable
	{
		// Token: 0x06007CD8 RID: 31960 RVA: 0x0032EB40 File Offset: 0x0032CD40
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
			this.workAnims = RemoteWorkerDock.EnterableDock.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("enter_dock");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x000F6A23 File Offset: 0x000F4C23
		protected override void OnCompleteWork(WorkerBase worker)
		{
			worker.GetComponent<RemoteWorkerSM>().Docked = true;
			base.OnCompleteWork(worker);
		}

		// Token: 0x04005E0A RID: 24074
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"enter_dock"
		};
	}

	// Token: 0x020017B6 RID: 6070
	public class ExitableDock : Workable
	{
		// Token: 0x06007CDC RID: 31964 RVA: 0x0032EBD8 File Offset: 0x0032CDD8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.ExitableDock.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("exit_dock");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x000F6A56 File Offset: 0x000F4C56
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			worker.GetComponent<RemoteWorkerSM>().Docked = false;
		}

		// Token: 0x04005E0B RID: 24075
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"exit_dock"
		};
	}

	// Token: 0x020017B7 RID: 6071
	public class WorkerRecharger : Workable
	{
		// Token: 0x06007CE0 RID: 31968 RVA: 0x0032EC5C File Offset: 0x0032CE5C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.WorkerRecharger.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerRecharging;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x0032ECC8 File Offset: 0x0032CEC8
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
			this.progress = ((component != null) ? component.ChargeRatio : 0f);
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x0032ED24 File Offset: 0x0032CF24
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
			if (component != null)
			{
				this.progress = component.ChargeRatio;
				return component.ApplyDeltaEnergy(7.5f * dt) == 0f;
			}
			return true;
		}

		// Token: 0x04005E0C RID: 24076
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"recharge_pre",
			"recharge_loop"
		};

		// Token: 0x04005E0D RID: 24077
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"recharge_pst"
		};

		// Token: 0x04005E0E RID: 24078
		private float progress;
	}

	// Token: 0x020017B8 RID: 6072
	public class WorkerGunkRemover : Workable
	{
		// Token: 0x06007CE6 RID: 31974 RVA: 0x0032EDC4 File Offset: 0x0032CFC4
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
			};
			this.workAnims = RemoteWorkerDock.WorkerGunkRemover.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerDraining;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x0032EE4C File Offset: 0x0032D04C
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				this.progress = 1f - component.GetMassAvailable(SimHashes.LiquidGunk) / 20.000002f;
			}
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x0032EEB4 File Offset: 0x0032D0B4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(SimHashes.LiquidGunk);
				float num = Math.Min(massAvailable, 3.3333337f * dt);
				this.progress = 1f - massAvailable / 20.000002f;
				if (num > 0f)
				{
					component.TransferMass(this.storage, SimHashes.LiquidGunk.CreateTag(), num, false, false, true);
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005E0F RID: 24079
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"drain_gunk_pre",
			"drain_gunk_loop"
		};

		// Token: 0x04005E10 RID: 24080
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"drain_gunk_pst"
		};

		// Token: 0x04005E11 RID: 24081
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005E12 RID: 24082
		private float progress;
	}

	// Token: 0x020017B9 RID: 6073
	public class WorkerOilRefiller : Workable
	{
		// Token: 0x06007CEC RID: 31980 RVA: 0x0032EF88 File Offset: 0x0032D188
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
			};
			this.workAnims = RemoteWorkerDock.WorkerOilRefiller.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerOiling;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x0032F010 File Offset: 0x0032D210
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
				this.progress = massAvailable / 20.000002f;
			}
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x0032F074 File Offset: 0x0032D274
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
				float num = Math.Min(20.000002f - massAvailable, 2.5000002f * dt);
				this.progress = massAvailable / 20.000002f;
				if (num > 0f)
				{
					this.storage.TransferMass(component, GameTags.LubricatingOil, num, false, false, true);
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005E13 RID: 24083
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"oil_pre",
			"oil_loop"
		};

		// Token: 0x04005E14 RID: 24084
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"oil_pst"
		};

		// Token: 0x04005E15 RID: 24085
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005E16 RID: 24086
		private float progress;
	}
}
