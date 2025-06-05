using System;
using KSerialization;

// Token: 0x0200107F RID: 4223
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterPurifier : StateMachineComponent<WaterPurifier.StatesInstance>
{
	// Token: 0x060055D3 RID: 21971 RVA: 0x0028DFD4 File Offset: 0x0028C1D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<WaterPurifier>(-2094018600, WaterPurifier.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x0028E028 File Offset: 0x0028C228
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

	// Token: 0x04003CBF RID: 15551
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003CC0 RID: 15552
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x04003CC1 RID: 15553
	private static readonly EventSystem.IntraObjectHandler<WaterPurifier> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<WaterPurifier>(delegate(WaterPurifier component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x02001080 RID: 4224
	public class StatesInstance : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.GameInstance
	{
		// Token: 0x060055D7 RID: 21975 RVA: 0x000DC762 File Offset: 0x000DA962
		public StatesInstance(WaterPurifier smi) : base(smi)
		{
		}
	}

	// Token: 0x02001081 RID: 4225
	public class States : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier>
	{
		// Token: 0x060055D8 RID: 21976 RVA: 0x0028E07C File Offset: 0x0028C27C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (WaterPurifier.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (WaterPurifier.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (WaterPurifier.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (WaterPurifier.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll()).Exit(delegate(WaterPurifier.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
		}

		// Token: 0x04003CC2 RID: 15554
		public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State off;

		// Token: 0x04003CC3 RID: 15555
		public WaterPurifier.States.OnStates on;

		// Token: 0x02001082 RID: 4226
		public class OnStates : GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State
		{
			// Token: 0x04003CC4 RID: 15556
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State waiting;

			// Token: 0x04003CC5 RID: 15557
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pre;

			// Token: 0x04003CC6 RID: 15558
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working;

			// Token: 0x04003CC7 RID: 15559
			public GameStateMachine<WaterPurifier.States, WaterPurifier.StatesInstance, WaterPurifier, object>.State working_pst;
		}
	}
}
