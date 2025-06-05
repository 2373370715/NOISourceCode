using System;

// Token: 0x02000A67 RID: 2663
public class Dreamer : GameStateMachine<Dreamer, Dreamer.Instance>
{
	// Token: 0x0600304B RID: 12363 RVA: 0x00208F74 File Offset: 0x00207174
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notDreaming;
		this.notDreaming.OnSignal(this.startDreaming, this.dreaming, (Dreamer.Instance smi) => smi.currentDream != null);
		this.dreaming.Enter(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(Dreamer.PrepareDream)).OnSignal(this.stopDreaming, this.notDreaming).Update(new Action<Dreamer.Instance, float>(this.UpdateDream), UpdateRate.SIM_EVERY_TICK, false).Exit(new StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State.Callback(this.RemoveDream));
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000C3D46 File Offset: 0x000C1F46
	private void RemoveDream(Dreamer.Instance smi)
	{
		smi.SetDream(null);
		NameDisplayScreen.Instance.StopDreaming(smi.gameObject);
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x000C3D5F File Offset: 0x000C1F5F
	private void UpdateDream(Dreamer.Instance smi, float dt)
	{
		NameDisplayScreen.Instance.DreamTick(smi.gameObject, dt);
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x000C3D72 File Offset: 0x000C1F72
	private static void PrepareDream(Dreamer.Instance smi)
	{
		NameDisplayScreen.Instance.SetDream(smi.gameObject, smi.currentDream);
	}

	// Token: 0x0400212A RID: 8490
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal stopDreaming;

	// Token: 0x0400212B RID: 8491
	public StateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.Signal startDreaming;

	// Token: 0x0400212C RID: 8492
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State notDreaming;

	// Token: 0x0400212D RID: 8493
	public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State dreaming;

	// Token: 0x02000A68 RID: 2664
	public class DreamingState : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400212E RID: 8494
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State hidden;

		// Token: 0x0400212F RID: 8495
		public GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.State visible;
	}

	// Token: 0x02000A69 RID: 2665
	public new class Instance : GameStateMachine<Dreamer, Dreamer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003051 RID: 12369 RVA: 0x000C3D9A File Offset: 0x000C1F9A
		public Instance(IStateMachineTarget master) : base(master)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000C3DB5 File Offset: 0x000C1FB5
		public void SetDream(Dream dream)
		{
			this.currentDream = dream;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000C3DBE File Offset: 0x000C1FBE
		public void StartDreaming()
		{
			base.sm.startDreaming.Trigger(base.smi);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000C3DD6 File Offset: 0x000C1FD6
		public void StopDreaming()
		{
			this.SetDream(null);
			base.sm.stopDreaming.Trigger(base.smi);
		}

		// Token: 0x04002130 RID: 8496
		public Dream currentDream;
	}
}
