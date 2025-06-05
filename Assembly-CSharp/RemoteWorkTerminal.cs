using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001795 RID: 6037
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkTerminal")]
public class RemoteWorkTerminal : Workable
{
	// Token: 0x06007C2F RID: 31791 RVA: 0x0032D53C File Offset: 0x0032B73C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_terminal_kanim")
		};
		this.InitializeWorkingInteracts();
		this.synchronizeAnims = true;
		this.showProgressBar = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.surpressWorkerForceSync = true;
		this.kbac.onAnimComplete += this.PlayNextWorkingAnim;
	}

	// Token: 0x06007C30 RID: 31792 RVA: 0x0032D5A8 File Offset: 0x0032B7A8
	private void InitializeWorkingInteracts()
	{
		if (RemoteWorkTerminal.NUM_WORKING_INTERACTS != -1)
		{
			return;
		}
		KAnimFileData data = this.overrideAnims[0].GetData();
		RemoteWorkTerminal.NUM_WORKING_INTERACTS = 0;
		for (;;)
		{
			string anim_name = string.Format("working_loop_{0}", RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1);
			if (data.GetAnim(anim_name) == null)
			{
				break;
			}
			RemoteWorkTerminal.NUM_WORKING_INTERACTS++;
		}
	}

	// Token: 0x06007C31 RID: 31793 RVA: 0x0032D600 File Offset: 0x0032B800
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkAnims;
		}
		return RemoteWorkTerminal.normalWorkAnims;
	}

	// Token: 0x06007C32 RID: 31794 RVA: 0x0032D640 File Offset: 0x0032B840
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkPstAnim;
		}
		return RemoteWorkTerminal.normalWorkPstAnim;
	}

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x06007C33 RID: 31795 RVA: 0x000F617A File Offset: 0x000F437A
	// (set) Token: 0x06007C34 RID: 31796 RVA: 0x000F618D File Offset: 0x000F438D
	public RemoteWorkerDock CurrentDock
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (((@ref != null) ? @ref.Get() : null) != null)
			{
				this.dock.Get().StopWorking(this);
			}
			this.dock = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06007C35 RID: 31797 RVA: 0x000F61C6 File Offset: 0x000F43C6
	// (set) Token: 0x06007C36 RID: 31798 RVA: 0x000F61D8 File Offset: 0x000F43D8
	public RemoteWorkerDock FutureDock
	{
		get
		{
			return this.future_dock ?? this.CurrentDock;
		}
		set
		{
			this.CurrentDock = value;
		}
	}

	// Token: 0x06007C37 RID: 31799 RVA: 0x000F61E1 File Offset: 0x000F43E1
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.kbac.Queue(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StartWorking(this);
	}

	// Token: 0x06007C38 RID: 31800 RVA: 0x000F6218 File Offset: 0x000F4418
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StopWorking(this);
	}

	// Token: 0x06007C39 RID: 31801 RVA: 0x000F6232 File Offset: 0x000F4432
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.CurrentDock == null || this.CurrentDock.OnRemoteWorkTick(dt);
	}

	// Token: 0x06007C3A RID: 31802 RVA: 0x000F6250 File Offset: 0x000F4450
	private HashedString GetWorkingLoop()
	{
		return string.Format("working_loop_{0}", UnityEngine.Random.Range(1, RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1));
	}

	// Token: 0x06007C3B RID: 31803 RVA: 0x000F6273 File Offset: 0x000F4473
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (base.worker.GetState() == WorkerBase.State.Working)
		{
			this.kbac.Play(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x04005D96 RID: 23958
	[Serialize]
	private Ref<RemoteWorkerDock> dock;

	// Token: 0x04005D97 RID: 23959
	private static int NUM_WORKING_INTERACTS = -1;

	// Token: 0x04005D98 RID: 23960
	[MyCmpReq]
	private KBatchedAnimController kbac;

	// Token: 0x04005D99 RID: 23961
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04005D9A RID: 23962
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre"
	};

	// Token: 0x04005D9B RID: 23963
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04005D9C RID: 23964
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"working_hat_pst"
	};

	// Token: 0x04005D9D RID: 23965
	public RemoteWorkerDock future_dock;
}
