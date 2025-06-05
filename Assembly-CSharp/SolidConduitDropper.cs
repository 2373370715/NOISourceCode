using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000FC6 RID: 4038
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDropper : StateMachineComponent<SolidConduitDropper.SMInstance>
{
	// Token: 0x06005157 RID: 20823 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005158 RID: 20824 RVA: 0x000D9754 File Offset: 0x000D7954
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x000D9767 File Offset: 0x000D7967
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x0027FEE8 File Offset: 0x0027E0E8
	private void Update()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
		base.smi.sm.isclosed.Set(!this.operational.IsOperational, base.smi, false);
		this.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x0400393E RID: 14654
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400393F RID: 14655
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04003940 RID: 14656
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x02000FC7 RID: 4039
	public class SMInstance : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.GameInstance
	{
		// Token: 0x0600515C RID: 20828 RVA: 0x000D9777 File Offset: 0x000D7977
		public SMInstance(SolidConduitDropper master) : base(master)
		{
		}
	}

	// Token: 0x02000FC8 RID: 4040
	public class States : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper>
	{
		// Token: 0x0600515D RID: 20829 RVA: 0x0027FF60 File Offset: 0x0027E160
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("Update", delegate(SolidConduitDropper.SMInstance smi, float dt)
			{
				smi.master.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.closed, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.idle, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
		}

		// Token: 0x04003941 RID: 14657
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter consuming;

		// Token: 0x04003942 RID: 14658
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter isclosed;

		// Token: 0x04003943 RID: 14659
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State idle;

		// Token: 0x04003944 RID: 14660
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State working;

		// Token: 0x04003945 RID: 14661
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State post;

		// Token: 0x04003946 RID: 14662
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State closed;
	}
}
