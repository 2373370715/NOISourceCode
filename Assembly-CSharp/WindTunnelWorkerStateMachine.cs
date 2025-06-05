using System;
using UnityEngine;

// Token: 0x02001A80 RID: 6784
public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase>
{
	// Token: 0x06008D7B RID: 36219 RVA: 0x00376538 File Offset: 0x00374738
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.OverrideAnim);
		this.pre_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreFrontAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreBackAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.LoopAnim, KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (WindTunnelWorkerStateMachine.StatesInstance smi) => smi.GetComponent<WorkerBase>().GetState() == WorkerBase.State.PendingCompletion);
		this.pst_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstBackAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstFrontAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x04006AC0 RID: 27328
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_front;

	// Token: 0x04006AC1 RID: 27329
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pre_back;

	// Token: 0x04006AC2 RID: 27330
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State loop;

	// Token: 0x04006AC3 RID: 27331
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_back;

	// Token: 0x04006AC4 RID: 27332
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State pst_front;

	// Token: 0x04006AC5 RID: 27333
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.State complete;

	// Token: 0x04006AC6 RID: 27334
	public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.TargetParameter worker;

	// Token: 0x02001A81 RID: 6785
	public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, WorkerBase, object>.GameInstance
	{
		// Token: 0x06008D7D RID: 36221 RVA: 0x00100F07 File Offset: 0x000FF107
		public StatesInstance(WorkerBase master, VerticalWindTunnelWorkable workable) : base(master)
		{
			this.workable = workable;
			base.sm.worker.Set(master, base.smi);
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06008D7E RID: 36222 RVA: 0x00100F2E File Offset: 0x000FF12E
		public HashedString OverrideAnim
		{
			get
			{
				return this.workable.overrideAnim;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06008D7F RID: 36223 RVA: 0x00100F3B File Offset: 0x000FF13B
		public string PreFrontAnim
		{
			get
			{
				return this.workable.preAnims[0];
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06008D80 RID: 36224 RVA: 0x00100F4A File Offset: 0x000FF14A
		public string PreBackAnim
		{
			get
			{
				return this.workable.preAnims[1];
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06008D81 RID: 36225 RVA: 0x00100F59 File Offset: 0x000FF159
		public string LoopAnim
		{
			get
			{
				return this.workable.loopAnim;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06008D82 RID: 36226 RVA: 0x00100F66 File Offset: 0x000FF166
		public string PstBackAnim
		{
			get
			{
				return this.workable.pstAnims[0];
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06008D83 RID: 36227 RVA: 0x00100F75 File Offset: 0x000FF175
		public string PstFrontAnim
		{
			get
			{
				return this.workable.pstAnims[1];
			}
		}

		// Token: 0x04006AC7 RID: 27335
		private VerticalWindTunnelWorkable workable;
	}
}
