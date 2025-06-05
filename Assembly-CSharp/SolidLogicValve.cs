using System;
using KSerialization;

// Token: 0x02000FD4 RID: 4052
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>
{
	// Token: 0x06005182 RID: 20866 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x000D992C File Offset: 0x000D7B2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x000D993F File Offset: 0x000D7B3F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04003963 RID: 14691
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003964 RID: 14692
	[MyCmpReq]
	private SolidConduitBridge bridge;

	// Token: 0x02000FD5 RID: 4053
	public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
	{
		// Token: 0x06005186 RID: 20870 RVA: 0x002802D0 File Offset: 0x0027E4D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidLogicValve.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidLogicValve.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
			this.on.idle.PlayAnim("on").Transition(this.on.working, (SolidLogicValve.StatesInstance smi) => smi.IsDispensing(), UpdateRate.SIM_200ms);
			this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).Transition(this.on.idle, (SolidLogicValve.StatesInstance smi) => !smi.IsDispensing(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04003965 RID: 14693
		public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;

		// Token: 0x04003966 RID: 14694
		public SolidLogicValve.States.ReadyStates on;

		// Token: 0x02000FD6 RID: 4054
		public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
		{
			// Token: 0x04003967 RID: 14695
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;

			// Token: 0x04003968 RID: 14696
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
		}
	}

	// Token: 0x02000FD8 RID: 4056
	public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
	{
		// Token: 0x06005191 RID: 20881 RVA: 0x000D997E File Offset: 0x000D7B7E
		public StatesInstance(SolidLogicValve master) : base(master)
		{
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x000D9987 File Offset: 0x000D7B87
		public bool IsDispensing()
		{
			return base.master.bridge.IsDispensing;
		}
	}
}
