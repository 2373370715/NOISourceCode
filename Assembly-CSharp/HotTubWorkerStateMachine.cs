using System;
using UnityEngine;

// Token: 0x0200143C RID: 5180
public class HotTubWorkerStateMachine : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x06006A43 RID: 27203 RVA: 0x002EBB3C File Offset: 0x002E9D3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims("anim_interacts_hottub_kanim", 0f);
		this.pre_front.PlayAnim("working_pre_front").OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim("working_pre_back").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((HotTubWorkerStateMachine.StatesInstance smi) => HotTubWorkerStateMachine.workAnimLoopVariants[UnityEngine.Random.Range(0, HotTubWorkerStateMachine.workAnimLoopVariants.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.loop_reenter).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.loop_reenter.GoTo(this.loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim("working_pst_back").OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim("working_pst_front").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x0400509F RID: 20639
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x040050A0 RID: 20640
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x040050A1 RID: 20641
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x040050A2 RID: 20642
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop_reenter;

	// Token: 0x040050A3 RID: 20643
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x040050A4 RID: 20644
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x040050A5 RID: 20645
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x040050A6 RID: 20646
	public StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x040050A7 RID: 20647
	public static string[] workAnimLoopVariants = new string[]
	{
		"working_loop1",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x0200143D RID: 5181
	public class StatesInstance : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x06006A46 RID: 27206 RVA: 0x000EA213 File Offset: 0x000E8413
		public StatesInstance(WorkerBase master) : base(master)
		{
			base.sm.worker.Set(master, base.smi);
		}
	}
}
